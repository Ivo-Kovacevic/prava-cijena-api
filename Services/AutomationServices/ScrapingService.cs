using System.Collections.Concurrent;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using HtmlAgilityPack;
using PravaCijena.Api.Dto.Category;
using PravaCijena.Api.Dto.Product;
using PravaCijena.Api.Dto.Store;
using PravaCijena.Api.Helpers;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;
using PravaCijena.Api.Services.Gemini.GeminiRequest;

namespace PravaCijena.Api.Services.AutomationServices;

public class ScrapingService : IScrapingService
{
    private static readonly ConcurrentDictionary<Guid, byte> _inProgressProductLinks = new();
    private readonly IConfiguration _config;
    private readonly IGeminiService _geminiService;
    private readonly HttpClient _httpClient;
    private readonly IProductRepository _productRepository;
    private readonly IProductStoreLinkRepository _productStoreLinkRepository;
    private readonly IServiceProvider _serviceProvider;
    private readonly IStoreRepository _storeRepository;

    public ScrapingService(
        HttpClient httpClient,
        IStoreRepository storeRepository,
        IProductRepository productRepository,
        IProductStoreLinkRepository productStoreLinkRepository,
        IGeminiService geminiService,
        IConfiguration config,
        IServiceProvider serviceProvider
    )
    {
        _httpClient = httpClient;
        _storeRepository = storeRepository;
        _productRepository = productRepository;
        _productStoreLinkRepository = productStoreLinkRepository;
        _geminiService = geminiService;
        _config = config;
        _serviceProvider = serviceProvider;
    }


    public async Task RunScraper()
    {
        var cloudinaryUrl = _config["ExternalServices:Cloudinary:CloudinaryUrl"];
        var cloudinary = new Cloudinary(cloudinaryUrl);
        cloudinary.Api.Secure = true;

        var storesWithCategories = await _storeRepository.GetAllWithMetadata();
        var random = new Random();
        var semaphore = new SemaphoreSlim(20);

        foreach (var store in storesWithCategories)
        {
            if (store.Slug != "konzum" || store.ProductListXPath == null)
            {
                continue;
            }

            var urlsInfo = GetCategoryUrls(store.BaseCategoryUrl, store.Categories);

            foreach (var urlInfo in urlsInfo)
                try
                {
                    var page = 1;
                    const int perPage = 100;
                    var hasMoreProducts = true;

                    while (hasMoreProducts)
                    {
                        // ------------ Add random delay between scraping websites ------------
                        var pageUrl = $"{urlInfo.Url}?{store.PageQuery}={page}&{store.LimitQuery}={perPage}";

                        Console.WriteLine($"Delaying request to \"{pageUrl}\" by few seconds...");
                        await Task.Delay(random.Next(2, 4) * 1000);

                        var html = await _httpClient.GetStringAsync(pageUrl);
                        var htmlDocument = new HtmlDocument();
                        htmlDocument.LoadHtml(html);

                        var productNodes =
                            htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'product-list')]/article");
                        if (productNodes == null || productNodes.Count == 0)
                        {
                            hasMoreProducts = false;
                            continue;
                        }

                        var productsToUpdate = new ConcurrentBag<Product>();
                        var productsStoreLinksToCreate = new ConcurrentBag<ProductStoreLink>();

                        var tasks = productNodes.Select(async productNode =>
                        {
                            try
                            {
                                await semaphore.WaitAsync();
                                using var scope = _serviceProvider.CreateScope();
                                var productStoreLinkRepo =
                                    scope.ServiceProvider.GetRequiredService<IProductStoreLinkRepository>();
                                var productRepo = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                                var productName = WebUtility.HtmlDecode(
                                    productNode.SelectNodes(".//a[@class='link-to-product']")[1].InnerText.Trim()
                                );
                                var productUrl = productNode.SelectSingleNode(".//a").GetAttributeValue("href", "");
                                var imageUrl = productNode.SelectSingleNode(".//img").GetAttributeValue("src", "");

                                var similarProducts = await productRepo.Search(productName, 1, 10);
                                var comparedProducts = await CompareProducts(
                                    productName,
                                    similarProducts.Select(p => new ExistingProduct
                                    {
                                        ExistingProductId = p.Id,
                                        ExistingProductName = p.Brand != null ? $"{p.Brand} {p.Name}" : p.Name
                                    }).ToList()
                                );

                                var sameProduct =
                                    comparedProducts.ExistingProducts.FirstOrDefault(p => p.IsSameProduct);

                                if (sameProduct == null ||
                                    !_inProgressProductLinks.TryAdd(sameProduct.ExistingProductId, 0))
                                {
                                    return;
                                }

                                var product =
                                    similarProducts.FirstOrDefault(p => p.Id == sameProduct.ExistingProductId);
                                if (product == null || product.ImageUrl != null)
                                {
                                    return;
                                }

                                // Upload image to cloudinary
                                var uploadParams = new ImageUploadParams
                                {
                                    File = new FileDescription(imageUrl),
                                    PublicId = SlugHelper.GenerateSlug(productName),
                                    UseFilename = true,
                                    UniqueFilename = true,
                                    Overwrite = false,
                                    Folder = "prava-cijena/proizvodi"
                                };
                                var uploadResult = await cloudinary.UploadAsync(uploadParams);

                                var updatedImageUrl = Regex.Replace(uploadResult.SecureUrl.ToString(), @"/v\d+/",
                                    "/e_background_removal/");
                                updatedImageUrl = Regex.Replace(updatedImageUrl, @"\.\w+$", ".webp");

                                product.ImageUrl = updatedImageUrl;
                                productsToUpdate.Add(product);

                                // Create product store link
                                var existingProductStoreLink =
                                    await productStoreLinkRepo.Get(store.Id, sameProduct.ExistingProductId);
                                if (existingProductStoreLink == null)
                                {
                                    productsStoreLinksToCreate.Add(new ProductStoreLink
                                    {
                                        StoreId = store.Id,
                                        ProductId = sameProduct.ExistingProductId,
                                        ProductLink = $"{store.StoreUrl}{productUrl}"
                                    });
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error while processing product node: {ex.Message}");
                            }
                            finally
                            {
                                semaphore.Release();
                            }
                        }).ToList();

                        await Task.WhenAll(tasks);

                        await _productRepository.BulkUpdateAsync(productsToUpdate.ToList());
                        await _productStoreLinkRepository.BulkCreateAsync(productsStoreLinksToCreate.ToList());

                        page++;

                        if (productNodes.Count < 100)
                        {
                            hasMoreProducts = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to scrape URL: {urlInfo}, Error: {ex.Message}");
                }
        }
    }

    private async Task<ProductMatchingResponse?> CompareProducts(string productName,
        List<ExistingProduct> similarProducts)
    {
        var responseSchema = JsonDocument.Parse(@"{
                           ""type"": ""OBJECT"",
                           ""properties"": {
                             ""newlyFoundProduct"": { ""type"": ""STRING"" },
                             ""existingProducts"": {
                               ""type"": ""ARRAY"",
                               ""items"": {
                                 ""type"": ""OBJECT"",
                                 ""properties"": {
                                   ""existingProductId"": { ""type"": ""STRING"" },
                                   ""existingProductName"": { ""type"": ""STRING"" },
                                   ""isSameProduct"": { ""type"": ""BOOLEAN""}
                                 },
                                 ""required"": [""existingProductId"", ""existingProductName"", ""isSameProduct""]
                               }
                             }
                           },
                           ""required"": [""newlyFoundProduct"", ""existingProducts""]
                       }").RootElement;

        try
        {
            var responseCategorizedProduct = await _geminiService.SendRequestAsync(
                [
                    new Part
                    {
                        Text =
                            $@"You will receive name of newly found product from Konzum web shop that I have an link and image for, and a list of products in my database that don't have links or images for.
                               Your task is to compare the names and decide if any of the existing products is the same product as the new one.
                               Put field 'isSameProduct' to equal true ONLY FOR ONE product for which you know is the same, that means that brand, type and other data must be same.
                               If no same products are found do not put true for any.

                          INPUT: {JsonSerializer.Serialize(new { NewlyFoundProduct = productName, ExistingProducts = similarProducts }, new JsonSerializerOptions
                          {
                              WriteIndented = false,
                              Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                          })}
                         "
                    }
                ],
                responseSchema
            );

            var categorizedProducts = JsonSerializer.Deserialize<ProductMatchingResponse>
            (
                responseCategorizedProduct, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            return categorizedProducts;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static IEnumerable<UrlInfoDto> GetCategoryUrls(
        string? baseUrl,
        List<StoreCategoryDto> categories,
        string path = ""
    )
    {
        foreach (var category in categories)
        {
            var newPath = string.IsNullOrEmpty(path) ? category.Name : $"{path}/{category.Name}";

            if (category.Subcategories.Count == 0)
            {
                yield return new UrlInfoDto
                {
                    Url = $"{baseUrl}/{newPath}",
                    EquivalentCategoryId = category.EquivalentCategoryId ?? null
                };
            }
            else
            {
                foreach (var url in GetCategoryUrls(baseUrl, category.Subcategories, newPath)) yield return url;
            }
        }
    }
}