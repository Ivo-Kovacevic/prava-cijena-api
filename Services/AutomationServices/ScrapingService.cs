using HtmlAgilityPack;
using PravaCijena.Api.Dto.Product;
using PravaCijena.Api.Dto.Store;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;

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
        var storesWithCategories = await _storeRepository.GetAllWithCategories();
        var random = new Random();
        foreach (var store in storesWithCategories)
        {
            var urls = GetCategoryUrls(store.BaseUrl, store.Categories);

            foreach (var url in urls)
                try
                {
                    Console.WriteLine(url);
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
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to scrape URL: {url}, Error: {ex.Message}");
                }
        }

        return 1;
    }

    private async Task<int> UpdateProductPrices(HtmlNodeCollection productNodes, StoreDto store)
    {
        var productsUpdated = 0;
        var productsCreated = 0;
        foreach (var productNode in productNodes)
        {
            ProductPreviewDto productPreviewDto;
            switch (store.Slug)
            {
                case "konzum":
                    productPreviewDto = GetKonzumNameAndPrice(productNode);
                    break;
                case "tommy":
                    productPreviewDto = GetTommyNameAndPrice(productNode);
                    break;
                default:
                    return 0;
            }

            var existingProduct = (await _productRepository.Search(productPreviewDto.Name)).FirstOrDefault();
            if (existingProduct == null)
            {
                continue;
            }

            // Create new data for each product that doesn't exist
            // if (existingProduct.Similarity < 0.9)
            // {
            //     _productRepository.CreateAsync();
            //     _productStoreRepository.CreateAsync();
            //     _priceRepository.CreateAsync();
            //     productsCreated++;
            // }

            // If existing product name is similar enough with found product name
            if (existingProduct.Similarity >= 0.9)
            {
                // Update the lowest product price if current price is from yesterday or if it's lower than current price
                if (existingProduct.UpdatedAt.Date < DateTime.UtcNow.Date
                    || productPreviewDto.Price < existingProduct.LowestPrice
                   )
                {
                    await _productRepository.UpdatePriceAsync(existingProduct.Id, productPreviewDto.Price);
                    productsUpdated++;
                }

                var productStore = await _productStoreRepository.GetProductStoreByIdsAsync(
                    existingProduct.Id,
                    store.Id
                );

                // Create new ProductStore if it doesn't exist or just update price if it does
                if (productStore == null)
                {
                    productStore = await _productStoreRepository.CreateAsync(new ProductStore
                    {
                        ProductId = existingProduct.Id,
                        StoreId = store.Id,
                        LatestPrice = productPreviewDto.Price
                    });
                }
                else
                {
                    await _productStoreRepository.UpdatePriceAsync(productStore.Id, productPreviewDto.Price);
                }

                // Create today's price if it doesn't exist
                var latestPrice = (await _priceRepository.GetPricesByProductStoreIdAsync(productStore.Id))
                    .FirstOrDefault();
                if (latestPrice == null || latestPrice.CreatedAt.Date != DateTime.UtcNow.Date)
                {
                    await _priceRepository.CreateAsync(new Price
                    {
                        Amount = productPreviewDto.Price,
                        ProductStoreId = productStore.Id
                    });
                }
            }
        }

        return productsUpdated;
    }

    private static ProductPreviewDto GetKonzumNameAndPrice(HtmlNode productNode)
    {
        var productName = productNode.SelectNodes(".//a[@class='link-to-product']")[1].InnerText.Trim();

        var primaryPriceNode = productNode.SelectSingleNode(
            ".//div[@class='price--primary']//span[@class='price--kn']"
        );
        var secondaryPriceNode = productNode.SelectSingleNode(
            ".//div[@class='price--primary']//div[@class='price__ul']//span[@class='price--li']"
        );

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

        var primaryPriceNode = productNode.SelectSingleNode(
            ".//div[@class='price--primary']//span[@class='price--kn']"
        );
        var secondaryPriceNode = productNode.SelectSingleNode(
            ".//div[@class='price--primary']//div[@class='price__ul']//span[@class='price--li']"
        );

        // Combine primary and secondary prices
        var primaryPrice = primaryPriceNode.InnerText.Trim();
        var secondaryPrice = secondaryPriceNode.InnerText.Trim();

        var formattedPrice = $"{primaryPrice}.{secondaryPrice}";

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

    private static IEnumerable<string> GetCategoryUrls(
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
                yield return $"{baseUrl}/{newPath}";
            }
            else
            {
                foreach (var url in GetCategoryUrls(baseUrl, category.Subcategories, newPath)) yield return url;
            }
        }
    }
}