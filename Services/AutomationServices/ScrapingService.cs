using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using PravaCijena.Api.Dto.Category;
using PravaCijena.Api.Dto.Store;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;
using PravaCijena.Api.Services.Gemini.GeminiResponse;

namespace PravaCijena.Api.Services.AutomationServices;

public class ScrapingService : IScrapingService
{
    private readonly IGeminiService _geminiService;
    private readonly HttpClient _httpClient;
    private readonly IPriceRepository _priceRepository;
    private readonly IProductRepository _productRepository;
    private readonly IProductStoreRepository _productStoreRepository;
    private readonly IStoreRepository _storeRepository;

    public ScrapingService(
        HttpClient httpClient,
        IGeminiService geminiService,
        IProductRepository productRepository,
        IStoreRepository storeRepository,
        IProductStoreRepository productStoreRepository,
        IPriceRepository priceRepository
    )
    {
        _httpClient = httpClient;
        _geminiService = geminiService;
        _productRepository = productRepository;
        _storeRepository = storeRepository;
        _productStoreRepository = productStoreRepository;
        _priceRepository = priceRepository;
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
        var semaphore = new SemaphoreSlim(100);
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
                        await Task.Delay(random.Next(2, 5) * 1000);

                        var pageUrl = $"{urlInfo.Url}?{store.PageQuery}={page}&{store.LimitQuery}={perPage}";
                        Console.WriteLine(pageUrl);

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
                        var mappedProducts = await MapScrapedToExistingProducts(productPreviews);

                        var tasks = mappedProducts.Select(async mappedProduct =>
                        {
                            await semaphore.WaitAsync();
                            try
                            {
                                return await _geminiService.CompareProductsAsync(mappedProduct);
                            }
                            finally
                            {
                                semaphore.Release();
                            }
                        }).ToList();

                        var comparedProducts = (await Task.WhenAll(tasks)).ToList();
                        await HandleComparedProducts(comparedProducts, store, urlInfo.EquivalentCategoryId, results);
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

    private async Task<List<MappedProduct>> MapScrapedToExistingProducts(List<ProductPreview> productPreviews)
    {
        var mappedProducts = new List<MappedProduct>();
        foreach (var productPreview in productPreviews)
        {
            var existingProduct = (await _productRepository.Search(productPreview.Name)).FirstOrDefault();
            if (existingProduct != null)
            {
                mappedProducts.Add(
                    new MappedProduct
                    {
                        ExistingProduct = existingProduct,
                        ProductPreview = productPreview
                    }
                );
            }
        }

        return mappedProducts;
    }

    private async Task HandleComparedProducts(
        List<ComparedResult> comparedResults,
        StoreWithCategoriesDto store,
        Guid? equivalentCategoryId,
        AutomationResult results
    )
    {
        foreach (var comparedResult in comparedResults)
            try
            {
                if (comparedResult.ExistingProduct.Name == "Zott Belriso Mliječni desert s rižom razni okusi 200 g" ||
                    comparedResult.ProductPreview.Name == "Zott Belriso Mliječni desert s rižom razni okusi 200 g"
                   )
                {
                    Console.WriteLine("FFS");
                }

                /*
                 * Update product if it's similar enough
                 */
                if (comparedResult.ExistingProduct.Similarity >= 0.95 || comparedResult.IsSameProduct)
                {
                    /*
                     * Update the lowest price if current price is from yesterday or if it's lower than current price
                     */
                    if (comparedResult.ExistingProduct.UpdatedAt.Date < DateTime.UtcNow.Date ||
                        comparedResult.ProductPreview.Price < comparedResult.ExistingProduct.LowestPrice
                       )
                    {
                        await _productRepository.UpdateLowestPriceAsync(
                            comparedResult.ExistingProduct.Id,
                            comparedResult.ProductPreview.Price
                        );
                        results.UpdatedProducts++;
                    }

                    var productStore = await _productStoreRepository.GetProductStoreByIdsAsync(
                        comparedResult.ExistingProduct.Id,
                        store.Id
                    );

                    if (productStore == null)
                    {
                        productStore = await _productStoreRepository.CreateAsync(new ProductStore
                        {
                            ProductId = comparedResult.ExistingProduct.Id,
                            StoreId = store.Id,
                            ProductUrl = comparedResult.ProductPreview.ProductUrl,
                            LatestPrice = comparedResult.ProductPreview.Price
                        });
                        results.CreatedProductStores++;

                        await _priceRepository.CreateAsync(new Price
                        {
                            Amount = comparedResult.ProductPreview.Price,
                            ProductStoreId = productStore.Id
                        });
                        results.CreatedPrices++;

                        continue;
                    }

                    await _productStoreRepository.UpdatePriceAsync(
                        productStore.Id,
                        comparedResult.ProductPreview.Price
                    );
                    results.UpdatedProductStores++;

                    var latestPrice = (await _priceRepository.GetPricesByProductStoreIdAsync(productStore.Id))
                        .FirstOrDefault();
                    if (latestPrice == null || latestPrice.CreatedAt.Date != DateTime.UtcNow.Date)
                    {
                        await _priceRepository.CreateAsync(new Price
                        {
                            Amount = comparedResult.ProductPreview.Price,
                            ProductStoreId = productStore.Id
                        });
                        results.CreatedPrices++;
                    }

                    continue;
                }

                /*
                 * Create new entries for each product that doesn't exist
                 */
                if (comparedResult.ExistingProduct.Similarity < 0.95 && !comparedResult.IsSameProduct &&
                    equivalentCategoryId.HasValue)
                {
                    var newProduct = await _productRepository.CreateAsync(new Product
                    {
                        Name = comparedResult.ProductPreview.Name,
                        ImageUrl = comparedResult.ProductPreview.ImageUrl,
                        CategoryId = equivalentCategoryId.Value,
                        LowestPrice = comparedResult.ProductPreview.Price
                    });
                    results.CreatedProducts++;

                    var productStore = await _productStoreRepository.CreateAsync(new ProductStore
                    {
                        ProductId = newProduct.Id,
                        StoreId = store.Id,
                        ProductUrl = comparedResult.ProductPreview.ProductUrl,
                        LatestPrice = comparedResult.ProductPreview.Price
                    });
                    results.CreatedProductStores++;

                    await _priceRepository.CreateAsync(new Price
                    {
                        Amount = comparedResult.ProductPreview.Price,
                        ProductStoreId = productStore.Id
                    });
                    results.CreatedPrices++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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