using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using PravaCijena.Api.Dto.Category;
using PravaCijena.Api.Dto.Store;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Services.AutomationServices;

public class ScrapingService : IScrapingService
{
    private readonly IAutomationService _automationService;
    private readonly HttpClient _httpClient;
    private readonly IStoreRepository _storeRepository;

    public ScrapingService(
        HttpClient httpClient,
        IAutomationService automationService,
        IStoreRepository storeRepository
    )
    {
        _httpClient = httpClient;
        _automationService = automationService;
        _storeRepository = storeRepository;
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
        var storesWithCategories = await _storeRepository.GetAllWithCategories();
        var random = new Random();
        foreach (var store in storesWithCategories)
        {
            var urlsInfo = GetCategoryUrls(store.BaseUrl, store.Categories);

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
                        if (productNodes.Count == 0)
                        {
                            hasMoreProducts = false;
                            continue;
                        }

                        var productPreviews = ProductNodesToProductPreviewList(productNodes, store);

                        await _automationService.HandleFoundProducts(
                            productPreviews,
                            store,
                            urlInfo.EquivalentCategoryId,
                            results
                        );
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

    private static List<ProductPreview> ProductNodesToProductPreviewList(
        HtmlNodeCollection productNodes,
        StoreWithCategoriesDto store
    )
    {
        var productPreviews = new List<ProductPreview>();

        foreach (var productNode in productNodes)
        {
            ProductPreview? productPreview;
            switch (store.Slug)
            {
                case "konzum":
                    productPreview = GetKonzumNameAndPrice(productNode, store.StoreUrl);
                    break;
                case "tommy":
                    productPreview = GetTommyNameAndPrice(productNode, store.StoreUrl);
                    break;
                default:
                    continue;
            }

            if (productPreview != null)
            {
                productPreviews.Add(productPreview);
            }
        }

        return productPreviews;
    }

    private static ProductPreview? GetKonzumNameAndPrice(HtmlNode productNode, string storeUrl)
    {
        // Product name
        var productName = WebUtility.HtmlDecode(
            productNode.SelectNodes(".//a[@class='link-to-product']")[1].InnerText.Trim()
        );

        // Get product price and in two vars
        // Konzum keeps price in euros and cents separated so that's why price needs to be retrieved like this
        var primaryPriceNode = productNode.SelectSingleNode(
            ".//div[@class='price--primary']//span[@class='price--kn']"
        ).InnerText.Trim();
        var secondaryPriceNode = productNode.SelectSingleNode(
            ".//div[@class='price--primary']//div[@class='price__ul']//span[@class='price--li']"
        ).InnerText.Trim();

        // Combine primary and secondary prices
        var formattedPrice = $"{primaryPriceNode}.{secondaryPriceNode}";

        // Product and image url
        var productUrl = productNode.SelectNodes(".//a")[0].GetAttributeValue("href", "");
        var imageUrl = productNode.SelectSingleNode(".//img").GetAttributeValue("src", "");

        if (decimal.TryParse(formattedPrice, out var productPrice))
        {
            return new ProductPreview
            {
                Name = productName,
                Price = productPrice,
                ProductUrl = storeUrl + productUrl,
                ImageUrl = imageUrl
            };
        }

        return null;
    }

    private static ProductPreview? GetTommyNameAndPrice(HtmlNode productNode, string storeUrl)
    {
        // Product name
        var productName = WebUtility.HtmlDecode(
            productNode.SelectSingleNode(".//h3/a").InnerText.Trim()
        );

        // Product price
        var priceText = productNode.SelectSingleNode(
            ".//span[@class='mt-auto inline-block-block text-sm font-bold text-gray-900']"
        ).InnerText.Trim().Replace(',', '.');

        var matchedPrice = Regex.Match(priceText, @"\d+(\.\d+)?");

        // Product and image url
        var productUrl = productNode.SelectNodes(".//a")[0].GetAttributeValue("href", "");

        // Can't get image url this way, it is in format like this: data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7
        // var imageUrl = productNode.SelectSingleNode(".//img").GetAttributeValue("src", "");

        if (matchedPrice.Success && decimal.TryParse(matchedPrice.Value, out var productPrice))
        {
            return new ProductPreview
            {
                Name = productName,
                Price = productPrice,
                ProductUrl = storeUrl + productUrl,
                ImageUrl = null
            };
        }

        return null;
    }

    private static IEnumerable<UrlInfoDto> GetCategoryUrls(
        string baseUrl,
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