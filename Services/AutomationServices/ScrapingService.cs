using System.Net;
using HtmlAgilityPack;
using PravaCijena.Api.Dto.Category;
using PravaCijena.Api.Dto.Store;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Services.AutomationServices;

public class ScrapingService : IScrapingService
{
    private readonly HttpClient _httpClient;
    private readonly IProductRepository _productRepository;
    private readonly IStoreRepository _storeRepository;

    public ScrapingService(
        HttpClient httpClient,
        IStoreRepository storeRepository,
        IProductRepository productRepository
    )
    {
        _httpClient = httpClient;
        _storeRepository = storeRepository;
        _productRepository = productRepository;
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
            if (store.Slug != "konzum")
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

                        var productNodes = htmlDocument.DocumentNode.SelectNodes(store.ProductListXPath);
                        if (productNodes.Count == 0 || urlInfo.EquivalentCategoryId == null)
                        {
                            hasMoreProducts = false;
                            continue;
                        }

                        await ProductNodesToProducts(productNodes, urlInfo.EquivalentCategoryId.Value);


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

    private async Task ProductNodesToProducts(
        HtmlNodeCollection productNodes,
        Guid equivalentCategoryId
    )
    {
        foreach (var productNode in productNodes)
        {
            ProductPreview? productPreview;
            var productName = WebUtility.HtmlDecode(
                productNode.SelectNodes(".//a[@class='link-to-product']")[1].InnerText.Trim()
            );
            var imageUrl = productNode.SelectSingleNode(".//img").GetAttributeValue("src", "");

            await _productRepository.CreateAsync(new Product
            {
                Name = productName,
                Barcode = "",
                ImageUrl = imageUrl,
                CategoryId = equivalentCategoryId
            });
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