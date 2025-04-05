using HtmlAgilityPack;
using PravaCijena.Api.Dto.Product;
using PravaCijena.Api.Interfaces;

namespace PravaCijena.Api.Services.AutomationServices;

public class ScrapingService : IScrapingService
{
    private readonly HttpClient _httpClient;
    private readonly IPriceService _priceService;
    private readonly IProductRepository _productRepository;
    private readonly IProductStoreService _productStoreService;

    public ScrapingService(
        HttpClient httpClient,
        IProductRepository productRepository,
        IProductStoreService productStoreService,
        IPriceService priceService)
    {
        _httpClient = httpClient;
        _productRepository = productRepository;
        _productStoreService = productStoreService;
        _priceService = priceService;
    }

    public async Task<int> RunScraper()
    {
        // var url = "https://www.konzum.hr/web/t/kategorije/mlijecni-proizvodi-i-jaja/mlijeko";
        //
        // var html = await _httpClient.GetStringAsync(url);
        //
        // var htmlDocument = new HtmlDocument();
        // htmlDocument.LoadHtml(html);
        //
        // var xpath =
        //     "//div[@class='product-list product-list--md-5 js-product-layout-container product-list--grid']//div[@class='product-wrapper']";
        //
        // var productNodes = htmlDocument.DocumentNode.SelectNodes(xpath);
        // if (productNodes == null)
        // {
        //     return 0;
        // }
        //
        // var productsUpdated = await UpdateProductPrices(productNodes);
        //
        return 1;
    }

    private async Task<int> UpdateProductPrices(HtmlNodeCollection productNodes)
    {
        var productsUpdated = 0;
        foreach (var productNode in productNodes)
        {
            var productPreview = GetProductNameAndPrice(productNode);
            var products = await _productRepository.Search(productPreview.Name);
            var similarExistingProduct = products.First();

            // Update product if it is similar and if it wasn't updated today
            if (similarExistingProduct.Similarity >= 0.9 &&
                similarExistingProduct.UpdatedAt.Date != DateTime.UtcNow.Date
               )
            {
                similarExistingProduct.LowestPrice = productPreview.Price;

                productsUpdated++;
            }
        }

        return productsUpdated;
    }

    private ProductPreviewDto GetProductNameAndPrice(HtmlNode productNode)
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
}