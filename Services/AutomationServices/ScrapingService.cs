using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using HtmlAgilityPack;
using PravaCijena.Api.Dto.Category;
using PravaCijena.Api.Dto.Store;
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

    public async Task<AutomationResult> RunScraper()
    {
        var results = new AutomationResult
        {
            CreatedProducts = 0,
            UpdatedProducts = 0,
            CreatedProductStores = 0,
            UpdatedProductStores = 0,
            CreatedPrices = 0,
            UpdatedPrices = 0
        };
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
                        await Task.Delay(random.Next(2, 5) * 1000);

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

                        await ProductNodesToProducts(productNodes);

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

        return results;
    }

    private async Task ProductNodesToProducts(HtmlNodeCollection productNodes)
    {
        foreach (var productNode in productNodes)
        {
            ProductPreview? productPreview;
            var productName = WebUtility.HtmlDecode(
                productNode.SelectNodes(".//a[@class='link-to-product']")[1].InnerText.Trim()
            );
            var imageUrl = productNode.SelectSingleNode(".//img").GetAttributeValue("src", "");

            var existingProduct = await _productRepository.Search(productName, 1, 20);
        }
    }

    private async Task<List<CategorizedProduct>?> CompareProducts(string productName, List<string> existingProducts)
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
                                   ""isSimilarProduct"": { ""type"": ""BOOLEAN""}
                                 },
                                 ""required"": [""existingProductId"", ""existingProductName"", ""isSimilarProduct""]
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
                        Text = $@"You will receive name of newly found product from Konzum web shop that I have an image for, and a list of products in my database that don't have images for.
                                  Your task is to compare the names and decide if any of the existing products are the same or similar enough to the new one.
                                  Be reasonably flexible with your matching. For example: 'Nutella 750g' can match with 'Nutella 700g', 'Nutella 3KG', or even 'NAMAZ NUTELLA'

                          INPUT: {JsonSerializer.Serialize(new { NewProductWithImage = productName, ExistingProducts = existingProducts }, new JsonSerializerOptions
                          {
                              WriteIndented = false,
                              Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                          })}
                         "
                    }
                ],
                responseSchema
            );

            var categorizedProducts = JsonSerializer.Deserialize<List<CategorizedProduct>>
            (
                responseCategorizedProduct, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if (categorizedProducts == null)
            {
                return null;
            }

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