using System.Net;
using HtmlAgilityPack;
using PravaCijena.Api.Dto.Category;
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
            var urlsInfo = GetCategoryUrls(store.BaseUrl, store.Categories);

            foreach (var urlInfo in urlsInfo)
                try
                {
                    // ------------ Add random delay between scraping websites ------------
                    await Task.Delay(random.Next(2, 5) * 1000);

                    var html = await _httpClient.GetStringAsync(urlInfo.Url);
                    Console.WriteLine(urlInfo.Url);

                    var htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(html);

                    var productNodes = htmlDocument.DocumentNode.SelectNodes(store.ProductListXPath);
                    if (productNodes.Count == 0)
                    {
                        continue;
                    }

                    var updated = await UpdateProductPrices(productNodes, store, urlInfo.EquivalentCategoryId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to scrape URL: {urlInfo.Url}, Error: {ex.Message}");
                }
        }

        return 1;
    }

    private async Task<(int, int)> UpdateProductPrices(
        HtmlNodeCollection productNodes,
        StoreWithCategoriesDto store,
        Guid? equivalentCategoryId
    )
    {
        var productsUpdated = 0;
        var productsCreated = 0;
        foreach (var productNode in productNodes)
        {
            ProductPreviewDto? productPreviewDto;
            switch (store.Slug)
            {
                case "konzum":
                    productPreviewDto = GetKonzumNameAndPrice(productNode, store.StoreUrl);
                    break;
                // case "tommy":
                //     productPreviewDto = GetTommyNameAndPrice(productNode);
                //     break;
                default:
                    return (0, 0);
            }

            if (productPreviewDto == null)
            {
                continue;
            }

            var existingProduct = (await _productRepository.Search(productPreviewDto.Name)).FirstOrDefault();
            if (existingProduct == null || existingProduct.Similarity is > 0.5 and < 0.95)
            {
                continue;
            }

            /*
             * Create new entries for each product that doesn't exist
             */
            if (existingProduct.Similarity <= 0.5 && equivalentCategoryId != null)
            {
                var newProduct = await _productRepository.CreateAsync(new Product
                {
                    Name = productPreviewDto.Name,
                    ImageUrl = productPreviewDto.ImageUrl,
                    CategoryId = equivalentCategoryId.Value,
                    LowestPrice = productPreviewDto.Price
                });
                productsCreated++;

                var productStore = await _productStoreRepository.CreateAsync(new ProductStore
                {
                    ProductId = newProduct.Id,
                    StoreId = store.Id,
                    ProductUrl = productPreviewDto.ProductUrl,
                    LatestPrice = productPreviewDto.Price
                });

                await _priceRepository.CreateAsync(new Price
                {
                    Amount = productPreviewDto.Price,
                    ProductStoreId = productStore.Id
                });

                continue;
            }

            /*
             * Update product if it's similar enough
             */
            if (existingProduct.Similarity >= 0.95)
            {
                /*
                 * Update the lowest price if current price is from yesterday or if it's lower than current price
                 */
                if (existingProduct.UpdatedAt.Date < DateTime.UtcNow.Date ||
                    productPreviewDto.Price < existingProduct.LowestPrice
                   )
                {
                    await _productRepository.UpdateLowestPriceAsync(existingProduct.Id, productPreviewDto.Price);
                }

                productsUpdated++;

                var productStore = await _productStoreRepository.GetProductStoreByIdsAsync(
                    existingProduct.Id,
                    store.Id
                );

                if (productStore == null)
                {
                    productStore = await _productStoreRepository.CreateAsync(new ProductStore
                    {
                        ProductId = existingProduct.Id,
                        StoreId = store.Id,
                        ProductUrl = productPreviewDto.ProductUrl,
                        LatestPrice = productPreviewDto.Price
                    });

                    await _priceRepository.CreateAsync(new Price
                    {
                        Amount = productPreviewDto.Price,
                        ProductStoreId = productStore.Id
                    });

                    continue;
                }

                await _productStoreRepository.UpdatePriceAsync(productStore.Id, productPreviewDto.Price);

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

        return (productsUpdated, productsCreated);
    }

    private static ProductPreviewDto? GetKonzumNameAndPrice(HtmlNode productNode, string storeUrl)
    {
        // Product name
        var productName = WebUtility.HtmlDecode(
            productNode.SelectNodes(".//a[@class='link-to-product']")[1].InnerText.Trim()
        );

        // Get product price and in two vars
        // Konzum keeps price in euros and cents separated so that's why price needs to be retrieved like this
        var primaryPriceNode = productNode.SelectSingleNode(
            ".//div[@class='price--primary']//span[@class='price--kn']"
        );
        var secondaryPriceNode = productNode.SelectSingleNode(
            ".//div[@class='price--primary']//div[@class='price__ul']//span[@class='price--li']"
        );

        // Combine primary and secondary prices
        var formattedPrice = $"{primaryPriceNode.InnerText.Trim()}.{secondaryPriceNode.InnerText.Trim()}";

        // Image url
        var imageUrl = productNode.SelectSingleNode(".//img").GetAttributeValue("src", "");
        var productUrl = productNode.SelectNodes(".//a")[0].GetAttributeValue("href", "");

        if (decimal.TryParse(formattedPrice, out var productPrice))
        {
            return new ProductPreviewDto
            {
                Name = productName,
                Price = productPrice,
                ProductUrl = storeUrl + productUrl,
                ImageUrl = imageUrl
            };
        }

        return null;
    }

    // private static ProductPreviewDto GetTommyNameAndPrice(HtmlNode productNode)
    // {
    //     var productName = productNode.SelectNodes(".//a[@class='link-to-product']")[1].InnerText.Trim();
    //
    //     var primaryPriceNode = productNode.SelectSingleNode(
    //         ".//div[@class='price--primary']//span[@class='price--kn']"
    //     );
    //     var secondaryPriceNode = productNode.SelectSingleNode(
    //         ".//div[@class='price--primary']//div[@class='price__ul']//span[@class='price--li']"
    //     );
    //
    //     // Combine primary and secondary prices
    //     var primaryPrice = primaryPriceNode.InnerText.Trim();
    //     var secondaryPrice = secondaryPriceNode.InnerText.Trim();
    //
    //     var formattedPrice = $"{primaryPrice}.{secondaryPrice}";
    //
    //     if (decimal.TryParse(formattedPrice, out var productPrice))
    //     {
    //         return new ProductPreviewDto
    //         {
    //             Name = productName,
    //             Price = productPrice
    //         };
    //     }
    //
    //     return null;
    // }

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