using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using HtmlAgilityPack;
using PravaCijena.Api.Dto.Store;
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
                    case "lidl":
                        await HandleLidl(store, urlNodes);
                        break;
                    case "studenac":
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
            var xdoc = XDocument.Load(entryStream);

            await HandleXml(xdoc, store, storeLocation);
        }
    }

    private async Task HandleCsv(string[] lines, StoreWithMetadataDto store, StoreLocation storeLocation)
    {
        if (store.CsvNameColumn == null || store.CsvPriceColumn == null || store.CsvBarcodeColumn == null)
        {
            return;
        }

        var semaphore = new SemaphoreSlim(20);
        var tasks = new List<Task>();

        var numGeminiCalls = 0;

        foreach (var line in lines.Skip(1))
        {
            await semaphore.WaitAsync();

            var task = Task.Run(async () =>
            {
                using var scope = _serviceProvider.CreateScope(); // this is the magic
                var productRepo = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                var productStoreRepo = scope.ServiceProvider.GetRequiredService<IProductStoreRepository>();
                var categoryRepo = scope.ServiceProvider.GetRequiredService<ICategoryRepository>();
                var geminiService = scope.ServiceProvider.GetRequiredService<IGeminiService>();

                try
                {
                    var cols = line.Split(',');
                    var productName = cols[store.CsvNameColumn.Value].Trim();
                    var priceStr = cols[store.CsvPriceColumn.Value].Trim().Replace(',', '.');
                    var barcode = cols[store.CsvBarcodeColumn.Value].Trim();

                    if (!decimal.TryParse(priceStr, out var price) || barcode.Length < 1)
                    {
                        return;
                    }

                    var product = await productRepo.GetProductByBarcodeAsync(barcode);

                    if (product == null)
                    {
                        product = await CategorizeProduct(productName, price, barcode, categoryRepo,
                            productRepo, geminiService);
                        if (product == null)
                        {
                            return;
                        }

                        numGeminiCalls++;
                    }

                    if (price < product.LowestPrice)
                    {
                        await productRepo.UpdateLowestPriceAsync(product.Id, price);
                    }

                    var productStore =
                        await productStoreRepo.GetProductStoreByIdsAsync(product.Id, storeLocation.Id);
                    if (productStore == null)
                    {
                        await productStoreRepo.CreateAsync(new ProductStore
                        {
                            ProductId = product.Id,
                            StoreLocationId = storeLocation.Id,
                            LatestPrice = price
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    semaphore.Release();
                }
            });
            tasks.Add(task);
        }

        await Task.WhenAll(tasks);
        
        Console.WriteLine($"Number of Gemini API calls: {numGeminiCalls}");
    }

    private async Task HandleXml(XDocument xdoc, StoreWithMetadataDto store, StoreLocation storeLocation)
    {
        if (store.XmlNameElement == null || store.XmlPriceElement == null || store.XmlBarcodeElement == null)
        {
            return;
        }

        foreach (var p in xdoc.Descendants("Proizvod"))
        {
            var productName = p.Element(store.XmlNameElement)?.Value;
            var priceStr = p.Element(store.XmlPriceElement)?.Value;
            var barcode = p.Element(store.XmlBarcodeElement)?.Value;

            if (productName == null || !decimal.TryParse(priceStr, out var price) || barcode.Length < 5)
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
                              Capitalize all letters.
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

    private async Task<Product?> CategorizeProduct(
        string productName, decimal price,
        string barcode,
        ICategoryRepository categoryRepo,
        IProductRepository productRepo,
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
                          If no appropriate subcategory is found, categorize it in top category ""Ostale namirnice"".
                          DO NOT MODIFY PRODUCT NAME.

                          ### OUTPUT RULES:
                          - Output MUST be a valid JSON object, not array, just single object.
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

        return await productRepo.CreateAsync(new Product
        {
            Name = productName,
            CategoryId = categorizedProduct.Category.Id,
            LowestPrice = price,
            Barcode = barcode
        });
    }
}