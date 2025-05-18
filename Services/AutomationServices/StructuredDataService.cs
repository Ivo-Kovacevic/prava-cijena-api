using System.Collections.Concurrent;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using HtmlAgilityPack;
using PravaCijena.Api.Dto.Store;
using PravaCijena.Api.Helpers;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Mappers;
using PravaCijena.Api.Models;
using PravaCijena.Api.Services.Gemini.GeminiRequest;

namespace PravaCijena.Api.Services.AutomationServices;

public class StructuredDataService : IStructuredDataService
{
    private readonly IAutomationService _automationService;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IGeminiService _geminiService;
    private readonly HttpClient _httpClient;
    private readonly IProductRepository _productRepository;
    private readonly IProductStoreRepository _productStoreRepository;
    private readonly IServiceProvider _serviceProvider;
    private readonly IStoreLocationRepository _storeLocationRepository;
    private readonly IStoreRepository _storeRepository;

    public StructuredDataService(
        IAutomationService automationService,
        HttpClient httpClient,
        IStoreRepository storeRepository,
        IProductRepository productRepository,
        IProductStoreRepository productStoreRepository,
        IGeminiService geminiService,
        IServiceProvider serviceProvider,
        IStoreLocationRepository storeLocationRepository,
        ICategoryRepository categoryRepository
    )
    {
        _automationService = automationService;
        _httpClient = httpClient;
        _storeRepository = storeRepository;
        _productRepository = productRepository;
        _productStoreRepository = productStoreRepository;
        _geminiService = geminiService;
        _serviceProvider = serviceProvider;
        _storeLocationRepository = storeLocationRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task SyncStoreFiles()
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
        var storesWithMetadata = await _storeRepository.GetAllWithMetadata();

        foreach (var store in storesWithMetadata)
            try
            {
                if (store.PriceUrlListXPath == null || store.PriceUrlXPath == null)
                {
                    continue;
                }

                var html = await _httpClient.GetStringAsync(store.PriceListUrl);
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);
                var mainNode = htmlDocument.DocumentNode.SelectSingleNode(store.PriceUrlListXPath);
                var urlNodes = mainNode.SelectNodes(store.PriceUrlXPath);

                switch (store.Slug)
                {
                    case "konzum":
                        await HandleKonzum(store, urlNodes);
                        break;
                    case "lidl--":
                        await HandleLidl(store, urlNodes);
                        break;
                    case "studenac--":
                        await HandleStudenac(store, urlNodes);
                        break;
                    default:
                        continue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
    }

    private async Task HandleKonzum(StoreWithMetadataDto store, HtmlNodeCollection urlNodes)
    {
        foreach (var urlNode in urlNodes)
        {
            var href = urlNode.GetAttributeValue("href", "");
            var fullUrl = new Uri(new Uri(store.StoreUrl), href).ToString();

            var storeLocation = await HandleStoreLocation(fullUrl, store.Id);
            if (storeLocation == null)
            {
                continue;
            }

            var csvData = await _httpClient.GetStringAsync(fullUrl);
            var lines = csvData.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            await HandleCsv(lines, store, storeLocation);
        }
    }

    private async Task HandleLidl(StoreWithMetadataDto store, HtmlNodeCollection urlNodes)
    {
        var zipUrl = urlNodes.Last().GetAttributeValue("href", "");

        var zipBytes = await _httpClient.GetByteArrayAsync(zipUrl);
        using var zipStream = new MemoryStream(zipBytes);
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);

        foreach (var entry in archive.Entries)
        {
            var storeLocation = await HandleStoreLocation(entry.FullName, store.Id);
            if (storeLocation == null)
            {
                continue;
            }

            await using var entryStream = entry.Open();
            using var reader = new StreamReader(entryStream, Encoding.GetEncoding("windows-1250"));

            var content = await reader.ReadToEndAsync();

            var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            await HandleCsv(lines, store, storeLocation);
        }
    }

    private async Task HandleStudenac(StoreWithMetadataDto store, HtmlNodeCollection urlNodes)
    {
        var zipUrl = urlNodes.First().GetAttributeValue("href", "");

        var zipBytes = await _httpClient.GetByteArrayAsync(zipUrl);
        using var zipStream = new MemoryStream(zipBytes);
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);

        foreach (var entry in archive.Entries)
        {
            var storeLocation = await HandleStoreLocation(entry.FullName, store.Id);
            if (storeLocation == null)
            {
                continue;
            }

            await using var entryStream = entry.Open();
            var xDoc = XDocument.Load(entryStream);

            await HandleXml(xDoc, store, storeLocation);
        }
    }

    private async Task HandleCsv(string[] lines, StoreWithMetadataDto store, StoreLocation storeLocation)
    {
        if (store.CsvNameColumn == null || store.CsvBrandColumn == null || store.CsvPriceColumn == null || store.CsvBarcodeColumn == null)
        {
            return;
        }

        var productPreviews = new List<ProductPreview>();

        foreach (var line in lines.Skip(1))
        {
            var cols = line.Split(',');
            var productName = cols[store.CsvNameColumn.Value].Trim();
            var productBrand = cols[store.CsvBrandColumn.Value].Trim();
            var priceStr = cols[store.CsvPriceColumn.Value].Trim().Replace(',', '.');
            var barcode = cols[store.CsvBarcodeColumn.Value].Trim();

            if (!decimal.TryParse(priceStr, out var price) || barcode.Length < 8)
            {
                continue;
            }

            productPreviews.Add(new ProductPreview
            {
                Name = productName,
                Brand = productBrand,
                Price = price,
                Barcode = barcode
            });
        }

        var allBarcodes = productPreviews.Select(x => x.Barcode).Distinct().ToList();
        var allSlugs = productPreviews.Select(x => SlugHelper.GenerateSlug(x.Name)).Distinct().ToList();
        var productsByBarcodes = await _productRepository.GetProductsByBarcodesBatchAsync(allBarcodes);
        var productsBySlugs = await _productRepository.GetProductsBySlugsBatchAsync(allSlugs);

        var dbProductsByBarcode = productsByBarcodes.ToDictionary(p => p.Barcode, StringComparer.OrdinalIgnoreCase);
        var existingSlugs = new HashSet<string>(productsBySlugs.Select(p => p.Slug), StringComparer.OrdinalIgnoreCase);

        var productPreviewsToUpdate = new List<ProductPreview>();
        var productPreviewsToCategorize = new List<ProductPreview>();

        foreach (var productPreview in productPreviews)
            if (dbProductsByBarcode.TryGetValue(productPreview.Barcode, out var dbProduct))
            {
                var needsUpdate = productPreview.Price < dbProduct.LowestPrice ||
                                  dbProduct.UpdatedAt.Date < DateTime.UtcNow.Date;

                if (needsUpdate)
                {
                    productPreviewsToUpdate.Add(productPreview);
                }
            }
            else
            {
                var baseName = productPreview.Name;
                var slug = SlugHelper.GenerateSlug(baseName);
                var i = 0;

                while (existingSlugs.Contains(slug))
                {
                    if (i == 0)
                    {
                        productPreview.Name = $"{productPreview.Brand} {baseName}";
                    }
                    else
                    {
                        productPreview.Name = $"{productPreview.Brand} {baseName} {i}";
                    }
                    
                    slug = SlugHelper.GenerateSlug(productPreview.Name);
                    i++;
                }

                existingSlugs.Add(slug);

                productPreviewsToCategorize.Add(productPreview);
            }

        if (productPreviewsToUpdate.Count > 0)
        {
            await _productRepository.UpdateLowestPricesBatchAsync(productPreviewsToUpdate);
        }

        if (productPreviewsToCategorize.Count > 0)
        {
            var categorizedProducts = new ConcurrentBag<Product>();

            await Parallel.ForEachAsync(productPreviewsToCategorize, new ParallelOptions
                {
                    MaxDegreeOfParallelism = 100
                },
                async (productPreview, ct) =>
                {
                    using var scope = _serviceProvider.CreateScope();
                    var categoryRepo = scope.ServiceProvider.GetRequiredService<ICategoryRepository>();
                    var geminiService = scope.ServiceProvider.GetRequiredService<IGeminiService>();

                    CategorizedProduct? categorizedProduct = null;
                    try
                    {
                        categorizedProduct = await CategorizeProduct(productPreview.Name, categoryRepo, geminiService);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    if (categorizedProduct != null)
                    {
                        categorizedProducts.Add(new Product
                        {
                            Name = productPreview.Name,
                            Brand = productPreview.Brand,
                            CategoryId = categorizedProduct.Category.Id,
                            LowestPrice = productPreview.Price,
                            Barcode = productPreview.Barcode
                        });
                    }
                });

            await _productRepository.CreateProductsBatchAsync(categorizedProducts.ToList());
        }
    }

    private async Task HandleXml(XDocument xDoc, StoreWithMetadataDto store, StoreLocation storeLocation)
    {
        if (store.XmlNameElement == null || store.XmlPriceElement == null || store.XmlBarcodeElement == null)
        {
            return;
        }

        foreach (var p in xDoc.Descendants("Proizvod"))
        {
            var productName = p.Element(store.XmlNameElement)?.Value;
            var priceStr = p.Element(store.XmlPriceElement)?.Value;
            var barcode = p.Element(store.XmlBarcodeElement)?.Value;

            if (productName == null || !decimal.TryParse(priceStr, out var price) || barcode.Length < 8)
            {
                continue;
            }

            var product = await _productRepository.GetProductByBarcodeAsync(barcode);

            if (product == null)
            {
                // product = await CategorizeProduct(productName, price, barcode);
                if (product == null)
                {
                    continue;
                }
            }

            if (price < product.LowestPrice)
            {
                await _productRepository.UpdateLowestPriceAsync(product.Id, price);
            }

            var productStore =
                await _productStoreRepository.GetProductStoreByIdsAsync(product.Id, storeLocation.Id);
            if (productStore == null)
            {
                await _productStoreRepository.CreateAsync(new ProductStore
                {
                    ProductId = product.Id,
                    StoreLocationId = storeLocation.Id,
                    LatestPrice = price
                });
            }
        }
    }

    private async Task<StoreLocation?> HandleStoreLocation(string filename, Guid storeId)
    {
        var responseStoreLocation = await _geminiService.SendRequestAsync([
            new Part
            {
                Text = $@"You are an AI specializing in extracting city names and addresses.
                              Input: {JsonSerializer.Serialize(filename)}

                              ### TASK:
                              Extract city name and address.
                              Capitalize all letters and remove other non-letter characters such as underscore, plus sign, dots or similar.
                              Do no include any other info such as zipcode, store type...

                              ### OUTPUT RULES:
                              - Output MUST be a valid JSON object, not array, just single object.
                              - Capitalize all letters in both city and address.
                              - Do NOT change or truncate any fields.
                              - Do NOT include comments or explanations.
                              - Do NOT output anything except the JSON.

                              ### EXAMPLE OUTPUT:
                              {{
                                 ""city"": ""cityName"",
                                 ""address"": ""address""
                              }}
                           "
            }
        ]);

        var foundStoreLocation = JsonSerializer.Deserialize<BaseStoreLocation>
        (
            responseStoreLocation, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        );

        if (foundStoreLocation == null)
        {
            return null;
        }

        var storeLocation = await _storeLocationRepository.GetByCityAndAddressAsync(
            foundStoreLocation.City, foundStoreLocation.Address
        );

        if (storeLocation == null)
        {
            storeLocation = await _storeLocationRepository.Create(new StoreLocation
            {
                City = foundStoreLocation.City,
                Address = foundStoreLocation.Address,
                StoreId = storeId
            });
        }

        return storeLocation;
    }

    private async Task<CategorizedProduct?> CategorizeProduct(
        string productName,
        ICategoryRepository categoryRepo,
        IGeminiService geminiService
    )
    {
        var allCategories =
            (await categoryRepo.GetAllCategoriesWithSubcategoriesAsync())
            .Select(c => c.ToBaseCategory());

        var responseCategorizedProduct = await geminiService.SendRequestAsync([
            new Part
            {
                Text = $@"You are an AI specializing in categorizing products.
                          Input: {JsonSerializer.Serialize(new { ProductName = productName, Categories = allCategories })}

                          ### TASK:
                          Categorize product in the most appropriate category.
                          If no appropriate category is found, categorize it in category ""Ostale namirnice i proizvodi"".
                          DO NOT MODIFY PRODUCT NAME.

                          ### OUTPUT RULES:
                          - Output MUST be a valid JSON object, not array, just single object.
                          - Output must be in the following format:
                            {{
                                ""productName"": ""productName"",
                                ""category"":
                                {{
                                    ""name"": ""categoryName"",
                                    ""id"": categoryId
                                }}
                            }}
                          - Do NOT change or truncate any fields.
                          - Do NOT include comments or explanations.
                          - Do NOT output anything except the JSON.

                          ### EXAMPLE OUTPUT:
                          {{
                             ""productName"": ""productName"",
                             ""category"": {{
                                 ""name"": ""categoryName"",
                                 ""id"": categoryId
                             }}
                          }}
                       "
            }
        ]);
        Console.WriteLine("START");
        Console.WriteLine(responseCategorizedProduct);
        Console.WriteLine("END");
        var categorizedProduct = JsonSerializer.Deserialize<CategorizedProduct>
        (
            responseCategorizedProduct, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        );

        if (categorizedProduct == null)
        {
            return null;
        }

        return categorizedProduct;
    }
}