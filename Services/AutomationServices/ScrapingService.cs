using HtmlAgilityPack;
using PravaCijena.Api.Config;
using PravaCijena.Api.Dto.Product;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;
using PravaCijena.Api.Models.AutomationModels;
using Category = PravaCijena.Api.Models.AutomationModels.Category;

namespace PravaCijena.Api.Services.AutomationServices;

public class ScrapingService : IScrapingService
{
    private readonly HttpClient _httpClient;
    private readonly IPriceRepository _priceRepository;
    private readonly IProductRepository _productRepository;
    private readonly IProductStoreRepository _productStoreRepository;
    private readonly IStoreRepository _storeRepository;

    public ScrapingService(
        HttpClient httpClient,
        IProductRepository productRepository,
        IStoreRepository storeRepository,
        IProductStoreRepository productStoreRepository,
        IPriceRepository priceRepository
    )
    {
        _httpClient = httpClient;
        _productRepository = productRepository;
        _storeRepository = storeRepository;
        _productStoreRepository = productStoreRepository;
        _priceRepository = priceRepository;
    }

    public async Task<int> RunScraper()
    {
        // var random = new Random();
        // foreach (var storeConfig in ScrapingConfiguration.StoresList)
        // {
        //     var urls = GetCategoryUrls(storeConfig.Url, storeConfig.Categories);
        //
        //     foreach (var url in urls)
        //         try
        //         {
        //             // ------------ Add random delay between scraping websites ------------
        //             await Task.Delay(random.Next(2, 5) * 1000);
        //
        //             var html = await _httpClient.GetStringAsync(url);
        //
        //             var htmlDocument = new HtmlDocument();
        //             htmlDocument.LoadHtml(html);
        //
        //             var productNodes = htmlDocument.DocumentNode.SelectNodes(storeConfig.ProductListXPath);
        //             if (productNodes == null)
        //             {
        //                 continue;
        //             }
        //
        //             var updated = await UpdateProductPrices(productNodes, storeConfig);
        //         }
        //         catch (Exception ex)
        //         {
        //             Console.WriteLine($"Failed to scrape URL: {url}, Error: {ex.Message}");
        //         }
        // }

        return 1;
    }

    private async Task<int> UpdateProductPrices(HtmlNodeCollection productNodes, StoreScrapingConfig storeSonfig)
    {
        var productsUpdated = 0;
        foreach (var productNode in productNodes)
        {
            ProductPreviewDto productPreview;
            switch (storeSonfig.StoreSlug)
            {
                case "konzum":
                    productPreview = GetKonzumNameAndPrice(productNode);
                    break;
                case "tommy":
                    productPreview = GetTommyNameAndPrice(productNode);
                    break;
                default:
                    return 0;
            }

            var products = await _productRepository.Search(productPreview.Name);
            if (!products.Any())
            {
                continue;
            }
            
            var similarExistingProduct = products.First();

            // Create new data for each product that doesn't exist
            // if (similarExistingProduct.Similarity < 0.9)
            // {
            //     _productRepository.CreateAsync();
            //     _productStoreRepository.CreateAsync();
            //     _priceRepository.CreateAsync();
            // }

            // If similar product already exists
            if (similarExistingProduct.Similarity >= 0.9)
            {
                if (productPreview.Price < similarExistingProduct.LowestPrice)
                {
                    await _productRepository.UpdatePriceAsync(similarExistingProduct.Id, productPreview.Price);
                }

                var store = await _storeRepository.GetBySlugAsync(storeSonfig.StoreSlug);
                var productStore = await _productStoreRepository.GetProductStoreByIdsAsync(
                    similarExistingProduct.Id,
                    store.Id
                );

                if (productStore == null)
                {
                    // Create new product store if it doesn't exist
                    productStore = await _productStoreRepository.CreateAsync(new ProductStore
                    {
                        ProductId = similarExistingProduct.Id,
                        StoreId = store.Id,
                        LatestPrice = productPreview.Price
                    });
                }
                else
                {
                    // Update price
                    await _productStoreRepository.UpdatePriceAsync(productStore.Id, productPreview.Price);
                }

                await _priceRepository.CreateAsync(new Price
                {
                    Amount = productPreview.Price,
                    ProductStoreId = productStore.Id
                });
            }
        }

        return productsUpdated;
    }

    private static ProductPreviewDto GetKonzumNameAndPrice(HtmlNode productNode)
    {
        var productName = productNode.SelectNodes(".//a[@class='link-to-product']")[1].InnerText.Trim();

        var primaryPriceNode =
            productNode.SelectSingleNode(".//div[@class='price--primary']//span[@class='price--kn']");
        var secondaryPriceNode =
            productNode.SelectSingleNode(
                ".//div[@class='price--primary']//div[@class='price__ul']//span[@class='price--li']");

        // Combine primary and secondary prices
        var primaryPrice = primaryPriceNode.InnerText.Trim();
        var secondaryPrice = secondaryPriceNode.InnerText.Trim();

        // Format the price (replace comma with dot and combine)
        var formattedPrice = $"{primaryPrice},{secondaryPrice}".Replace(',', '.');

        Console.WriteLine(productName);
        if (decimal.TryParse(formattedPrice, out var productPrice))
        {
            return new ProductPreviewDto
            {
                Name = productName,
                Price = productPrice
            };
        }

        return null;
    }

    private static ProductPreviewDto GetTommyNameAndPrice(HtmlNode productNode)
    {
        var productName = productNode.SelectNodes(".//a[@class='link-to-product']")[1].InnerText.Trim();

        var primaryPriceNode =
            productNode.SelectSingleNode(".//div[@class='price--primary']//span[@class='price--kn']");
        var secondaryPriceNode =
            productNode.SelectSingleNode(
                ".//div[@class='price--primary']//div[@class='price__ul']//span[@class='price--li']");

        // Combine primary and secondary prices
        var primaryPrice = primaryPriceNode.InnerText.Trim();
        var secondaryPrice = secondaryPriceNode.InnerText.Trim();

        // Format the price (replace comma with dot and combine)
        var formattedPrice = $"{primaryPrice},{secondaryPrice}".Replace(',', '.');

        if (decimal.TryParse(formattedPrice, out var productPrice))
        {
            return new ProductPreviewDto
            {
                Name = productName,
                Price = productPrice
            };
        }

        return null;
    }

    private static IEnumerable<string> GetCategoryUrls(string baseUrl, List<Category> categories, string path = "")
    {
        foreach (var category in categories)
        {
            var newPath = string.IsNullOrEmpty(path) ? category.Name : $"{path}/{category.Name}";

            if (category.Subcategories.Count == 0)
            {
                yield return $"{baseUrl}/{newPath}";
            }
            else
            {
                foreach (var url in GetCategoryUrls(baseUrl, category.Subcategories, newPath)) yield return url;
            }
        }
    }
}