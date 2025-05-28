using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;
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
    private readonly IGeminiService _geminiService;
    private readonly HttpClient _httpClient;
    private readonly IProductRepository _productRepository;
    private readonly IStoreRepository _storeRepository;

    public ScrapingService(
        HttpClient httpClient,
        IStoreRepository storeRepository,
        IProductRepository productRepository,
        IGeminiService geminiService
    )
    {
        _httpClient = httpClient;
        _storeRepository = storeRepository;
        _productRepository = productRepository;
        _geminiService = geminiService;
    }

    public async Task RunScraper()
    {
        DotEnv.Load(new DotEnvOptions(probeForEnv: true));
        var cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
        cloudinary.Api.Secure = true;

        var storesWithCategories = await _storeRepository.GetAllWithMetadata();
        var random = new Random();
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
                        if (productNodes.Count == 0)
                        {
                            hasMoreProducts = false;
                            continue;
                        }

                        foreach (var productNode in productNodes)
                        {
                            var productName = WebUtility.HtmlDecode(
                                productNode.SelectNodes(".//a[@class='link-to-product']")[1].InnerText.Trim()
                            );
                            var imageUrl = productNode.SelectSingleNode(".//img").GetAttributeValue("src", "");

                            var similarProducts = await _productRepository.Search(productName, 1, 10);
                            var comparedProducts = await CompareProducts(
                                productName,
                                similarProducts.Select(p => new ExistingProduct
                                {
                                    ExistingProductId = p.Id,
                                    ExistingProductName = p.Name
                                }).ToList()
                            );

                            if (comparedProducts.ExistingProducts.Any(p => p.IsSameProduct))
                            {
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
                                
                                var updatedUrl = Regex.Replace(uploadResult.SecureUrl.ToString(), @"/v\d+/", "/e_background_removal/");
                                updatedUrl = Regex.Replace(updatedUrl, @"\.\w+$", ".webp");
                                var productsToUpdate = new List<Product>();

                                foreach (var existingProduct in comparedProducts.ExistingProducts)
                                    if (existingProduct.IsSameProduct)
                                    {
                                        var product = similarProducts.FirstOrDefault(p =>
                                            p.Id == existingProduct.ExistingProductId);
                                        if (product != null)
                                        {
                                            product.ImageUrl = updatedUrl;
                                            productsToUpdate.Add(product);
                                        }
                                    }

                                await _productRepository.BulkUpdateAsync(productsToUpdate);
                            }
                        }

                        page++;

                        if (productNodes.Count < 100)
                        {
                            hasMoreProducts = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to scrape URL: {urlInfo.Url}, Error: {ex.Message}");
                }
        }
    }

    private async Task<ProductMatchingResponse?> CompareProducts(string productName, List<ExistingProduct> similarProducts)
    {
        var responseSchema = JsonDocument.Parse(@"{
                           ""type"": ""OBJECT"",
                           ""properties"": {
                             ""newProductWithImage"": { ""type"": ""STRING"" },
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
                           ""required"": [""newProductWithImage"", ""existingProducts""]
                       }").RootElement;

        try
        {
            var responseCategorizedProduct = await _geminiService.SendRequestAsync(
                [
                    new Part
                    {
                        Text =
                            $@"You will receive name of newly found product from Konzum web shop that I have an image for, and a list of products in my database that don't have images for.
                               Your task is to compare the names and decide if any of the existing products are the same or similar enough to the new one.
                               Put field 'isSameProduct' to equal true in cases when brand is same but differences are in weight, milk fat or similar.
                               Be reasonably flexible with your matching. For example: 'Nutella 750g' can match with 'Nutella 700g', 'Nutella 3KG', or even 'NAMAZ NUTELLA'.

                          INPUT: {JsonSerializer.Serialize(new { NewProductWithImage = productName, ExistingProducts = similarProducts }, new JsonSerializerOptions
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