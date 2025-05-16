using System.IO.Compression;
using System.Text;
using System.Text.Json;
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
    private readonly IGeminiService _geminiService;
    private readonly HttpClient _httpClient;
    private readonly IProductRepository _productRepository;
    private readonly IProductStoreRepository _productStoreRepository;
    private readonly IServiceProvider _serviceProvider;
    private readonly IStoreLocationRepository _storeLocationRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly ICategoryRepository _categoryRepository;

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
            switch (store.DataLocation)
            {
                case "on-site":
                    await HandleUrlsOnSite(store);
                    break;
                case "in-zip":
                    break;
                default:
                    continue;
            }
    }

    private async Task HandleUrlsOnSite(StoreWithMetadataDto store)
    {
        if (store.PriceUrlListXPath == null || store.PriceUrlXPath == null || store.PriceUrlType == null ||
            store.BarcodeColumn == null || store.PriceColumn == null)
        {
            return;
        }

        var html = await _httpClient.GetStringAsync(store.PriceListUrl);
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);
        var mainNode = htmlDocument.DocumentNode.SelectSingleNode(store.PriceUrlListXPath);
        var urlNodes = mainNode.SelectNodes(store.PriceUrlXPath);

        var semaphore = new SemaphoreSlim(100);
        var tasks = new List<Task>();

        foreach (var urlNode in urlNodes)
        {
            var href = urlNode.GetAttributeValue("href", "");
            string fullUrl;
            switch (store.PriceUrlType)
            {
                case "absolute":
                    fullUrl = href;
                    break;
                case "relative":
                    fullUrl = new Uri(new Uri(store.StoreUrl), href).ToString();
                    break;
                default:
                    continue;
            }

            var csvData = await _httpClient.GetStringAsync(fullUrl);
            var lines = csvData.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            var responseStoreLocation = await _geminiService.SendRequestAsync([
                new Part
                {
                    Text = $@"You are an AI specializing in extracting city names and addresses.
                              Input: {JsonSerializer.Serialize(fullUrl)}

                              ### TASK:
                              Extract city name and address. Do no include any other info such as zipcode, store type...

                              ### OUTPUT RULES:
                              - Output MUST be a valid JSON object, not array, just single object.
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
                continue;
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
                    StoreId = store.Id
                });
            }

            foreach (var line in lines.Skip(1))
            {
                // using var scope = _serviceProvider.CreateScope();
                // var productRepo = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                // var categoryRepo = scope.ServiceProvider.GetRequiredService<ICategoryRepository>();
                // var productStoreRepo = scope.ServiceProvider.GetRequiredService<IProductStoreRepository>();
                try
                {
                    var cols = line.Split(',');
                    var productName = cols[0].Trim();
                    var barcodeStr = cols[store.BarcodeColumn.Value].Trim();
                    var priceStr = cols[store.PriceColumn.Value].Trim().Replace(',', '.');

                    if (!long.TryParse(barcodeStr, out var barcode)
                        || !decimal.TryParse(priceStr, out var price)
                       )
                    {
                        continue;
                    }

                    var product = await _productRepository.GetProductByBarcodeAsync(barcode);

                    if (product == null)
                    {
                        var allCategories =
                            (await _categoryRepository.GetAllCategoriesWithSubcategoriesAsync())
                            .Select(c => c.ToBaseCategory());

                        var responseCategorizedProduct = await _geminiService.SendRequestAsync([
                            new Part
                            {
                                Text = $@"You are an AI specializing in categorizing products.
                                              Input: {JsonSerializer.Serialize(new { ProductName = productName, Categories = allCategories })}

                                              Each object has:
                                              - productName: Uncategorized product.
                                              - categories: All available categories with their subcategories.

                                              ### TASK:
                                              Categorize product in the most appropriate subcategory.
                                              Categorize product in subcategory and not top category.
                                              You can categorize product in top category if no subcategories are appropriate and top category has no subcategories.
                                              If no appropriate subcategory is found, categorize it in top category ""Ostalo"".

                                              ### OUTPUT RULES:
                                              - Output MUST be a valid JSON object, not array, just single object.
                                              - Object must have:
                                                - productName (unchanged from input)
                                                - category (containing category name and GUID)
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
                            return;
                        }

                        product = await _productRepository.CreateAsync(new Product
                        {
                            Name = productName,
                            CategoryId = categorizedProduct.Category.Id,
                            LowestPrice = price,
                            Barcode = barcode
                        });
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
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                // finally
                // {
                //     semaphore.Release();
                // }
            }

            // await Task.WhenAll(tasks);
            // tasks.Clear();
        }
    }

    private async Task HandleKaufland(StoreWithMetadataDto store)
    {
        var url =
            "https://www.kaufland.hr/content/dam/kaufland/website/consumer/hr_HR/download/document/2025/mpc/Popis_maloprodajnih_cijena_15_5_2025.zip";
        Console.WriteLine("Downloading zip...");

        await using var zipStream = await _httpClient.GetStreamAsync(url);
        using var memoryStream = new MemoryStream();
        await zipStream.CopyToAsync(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);

        using var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read);

        foreach (var entry in archive.Entries)
        {
            // iterate over store.StoreLocations and connect address with file name
            if (!entry.FullName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var nameWithoutExtension = Path.GetFileNameWithoutExtension(entry.FullName);
            var parts = nameWithoutExtension.Split(',');
            var addressFromFile = parts[0].Replace("Supermarket_", "").Replace("Hipermarket_", "").Replace('_', ' ')
                .Trim();

            var matchedLocation = store.StoreLocations.FirstOrDefault(s => s.Address == addressFromFile);

            if (matchedLocation == null)
            {
                Console.WriteLine($"No match found for address: {addressFromFile}");
                continue;
            }

            await using var entryStream = entry.Open();
            using var reader = new StreamReader(entryStream, Encoding.GetEncoding("windows-1250"));

            var content = await reader.ReadToEndAsync();

            var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var previews = new List<ProductPreview>();

            foreach (var line in lines.Skip(1))
            {
                var cols = line.Split('\t');
                if (cols.Length < 6)
                {
                    continue;
                }

                var name = cols[0].Trim();
                var priceStr = cols[5].Trim().Replace(',', '.');
                var barcodeStr = cols[13].Trim();

                if (decimal.TryParse(priceStr, out var price)
                    && long.TryParse(barcodeStr, out var barcode)
                   )
                {
                    previews.Add(new ProductPreview
                    {
                        Name = name,
                        Price = price,
                        Barcode = barcode
                    });
                }
            }
        }
    }
}