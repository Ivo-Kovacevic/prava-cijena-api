using System.Collections.Concurrent;
using System.IO.Compression;
using System.Text;
using System.Text.Encodings.Web;
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
        var results = new AutomationResult();
        var storesWithMetadata = await _storeRepository.GetAllWithMetadata();

        var existingSlugs = new ConcurrentDictionary<string, byte>(
            (await _productRepository.GetAllSlugsAsync())
            .Select(slug => new KeyValuePair<string, byte>(slug, 0)),
            StringComparer.OrdinalIgnoreCase
        );

        var existingBarcodes = new ConcurrentDictionary<string, byte>(
            (await _productRepository.GetAllBarcodesAsync())
            .Select(barcode => new KeyValuePair<string, byte>(barcode, 0)),
            StringComparer.OrdinalIgnoreCase
        );

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
                        await HandleKonzum(store, urlNodes, existingSlugs, existingBarcodes);
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

    private async Task HandleKonzum(StoreWithMetadataDto store, HtmlNodeCollection urlNodes,
        ConcurrentDictionary<string, byte> existingSlugs, ConcurrentDictionary<string, byte> existingBarcodes
    )
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

            await HandleCsv(lines, store, storeLocation, existingSlugs, existingBarcodes);
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

            // await HandleCsv(lines, store, storeLocation);
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

    private async Task HandleCsv(string[] lines, StoreWithMetadataDto store, StoreLocation storeLocation,
        ConcurrentDictionary<string, byte> existingSlugs, ConcurrentDictionary<string, byte> existingBarcodes)
    {
        if (store.CsvNameColumn == null || store.CsvBrandColumn == null || store.CsvPriceColumn == null ||
            store.CsvBarcodeColumn == null)
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

        var allBarcodesInFile = productPreviews.Select(x => x.Barcode).Distinct().ToList();

        var productsByBarcodes = await _productRepository.GetProductsByBarcodesBatchAsync(allBarcodesInFile);
        var dbProductsByBarcode = productsByBarcodes.ToDictionary(p => p.Barcode, StringComparer.OrdinalIgnoreCase);

        var productIds = dbProductsByBarcode.Values.Select(p => p.Id).ToList();

        var existingProductStores =
            await _productStoreRepository.GetProductStoresByIdsBatchAsync(productIds, storeLocation.Id);
        var productStoreLookup =
            existingProductStores.ToDictionary(ps => ps.Product.Barcode, StringComparer.OrdinalIgnoreCase);


        var productPreviewsToUpdate = new List<ProductPreview>();
        var productStoresToUpdate = new List<ProductStore>();
        var productPreviewsToCreate = new List<ProductPreview>();
        var productStoresToCreate = new List<ProductStore>();

        foreach (var productPreview in productPreviews)
            if (dbProductsByBarcode.TryGetValue(productPreview.Barcode, out var dbProduct))
            {
                if (productPreview.Price < dbProduct.LowestPrice || dbProduct.UpdatedAt.Date < DateTime.UtcNow.Date)
                {
                    productPreviewsToUpdate.Add(productPreview);
                }

                if (productStoreLookup.TryGetValue(productPreview.Barcode, out var productStore))
                {
                    if (productStore.LatestPrice != productPreview.Price)
                    {
                        productStore.LatestPrice = productPreview.Price;
                        productStoresToUpdate.Add(productStore);
                    }
                }
                else
                {
                    // Product exists, but store mapping doesn't
                    productStoresToCreate.Add(new ProductStore
                    {
                        ProductId = dbProduct.Id,
                        StoreLocationId = storeLocation.Id,
                        LatestPrice = productPreview.Price
                    });
                }

                existingBarcodes.TryAdd(productPreview.Barcode, 0);
            }
            else
            {
                var baseName = productPreview.Name;
                var slug = SlugHelper.GenerateSlug(baseName);
                var i = 0;

                while (!existingSlugs.TryAdd(slug, 0))
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

                productPreviewsToCreate.Add(productPreview);

                if (productStoreLookup.TryGetValue(productPreview.Barcode, out var productStore))
                {
                    productStore.LatestPrice = productPreview.Price;
                    productStoresToCreate.Add(productStore);
                }
            }

        if (productPreviewsToUpdate.Count > 0)
        {
            await _productRepository.UpdateLowestPricesBatchAsync(productPreviewsToUpdate);
        }

        if (productStoresToUpdate.Count > 0)
        {
            await _productStoreRepository.UpdateProductStoresBatchAsync(productStoresToUpdate);
        }

        if (productPreviewsToCreate.Count > 0)
        {
            var semaphore = new SemaphoreSlim(50);
            var tasks = new List<Task>();

            var batches = productPreviewsToCreate
                .Select((product, index) => new { product, index })
                .GroupBy(x => x.index / 5)
                .Select(g => g.Select(x => x.product).ToList());

            foreach (var batch in batches)
            {
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        await semaphore.WaitAsync();
                        using var scope = _serviceProvider.CreateScope();
                        var categoryRepo = scope.ServiceProvider.GetRequiredService<ICategoryRepository>();
                        var geminiService = scope.ServiceProvider.GetRequiredService<IGeminiService>();
                        var productRepo = scope.ServiceProvider.GetRequiredService<IProductRepository>();

                        var productNames = batch.Select(p => p.Name).ToList();
                        var results = await CategorizeProducts(productNames, categoryRepo, geminiService);

                        if (results == null)
                        {
                            return;
                        }

                        var categorizedProducts = new List<Product>();
                        for (var i = 0; i < results.Count; i++)
                            categorizedProducts.Add(new Product
                            {
                                Name = batch[i].Name,
                                Brand = batch[i].Brand,
                                CategoryId = results[i].Category.Id,
                                LowestPrice = batch[i].Price,
                                Barcode = batch[i].Barcode
                            });

                        await productRepo.CreateProductsBatchAsync(categorizedProducts);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            await Task.WhenAll(tasks);
        }


        var newlyInsertedBarcodes = productPreviewsToCreate.Select(p => p.Barcode).ToList();
        var newDbProducts = await _productRepository.GetProductsByBarcodesBatchAsync(newlyInsertedBarcodes);
        var newDbProductsByBarcode = newDbProducts.ToDictionary(p => p.Barcode, StringComparer.OrdinalIgnoreCase);

        foreach (var preview in productPreviewsToCreate)
            if (newDbProductsByBarcode.TryGetValue(preview.Barcode, out var newProduct))
            {
                productStoresToCreate.Add(new ProductStore
                {
                    ProductId = newProduct.Id,
                    StoreLocationId = storeLocation.Id,
                    LatestPrice = preview.Price
                });
            }

        const int maxRowsPerBatch = 5000;
        for (int i = 0; i < productStoresToCreate.Count; i += maxRowsPerBatch)
        {
            var chunk = productStoresToCreate.Skip(i).Take(maxRowsPerBatch).ToList();
            await _productStoreRepository.CreateProductStoresBatchAsync(chunk);
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
        var responseSchema = JsonDocument.Parse(@"{
                           ""type"": ""OBJECT"",
                           ""properties"": {
                             ""city"": { ""type"": ""STRING"" },
                             ""address"": { ""type"": ""STRING"" }
                           },
                           ""required"": [""city"", ""address""]
                       }").RootElement;

        var responseStoreLocation = await _geminiService.SendRequestAsync([
                new Part
                {
                    Text = $@"Extract city name and address.
                          Capitalize all letters in both city and address.
                          Remove characters such as underscores or plus signs.
                          Do no include any other info such as zipcode, store type...

                          Input: {JsonSerializer.Serialize(filename, new JsonSerializerOptions
                          {
                              WriteIndented = false,
                              Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                          })}
                         "
                }
            ],
            responseSchema);

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

    private async Task<List<CategorizedProduct>?> CategorizeProducts(
        List<string> productNames,
        ICategoryRepository categoryRepo,
        IGeminiService geminiService
    )
    {
        var allCategories =
            (await categoryRepo.GetAllCategoriesWithSubcategoriesAsync())
            .Select(c => c.ToBaseCategory());

        var responseSchema = JsonDocument.Parse(@"{
                         ""type"": ""ARRAY"",
                         ""items"": {
                           ""type"": ""OBJECT"",
                           ""properties"": {
                             ""productName"": { ""type"": ""STRING"" },
                             ""category"": {
                                ""type"": ""OBJECT"",
                                ""properties"": {
                                   ""name"": { ""type"": ""STRING"" },
                                   ""id"": { ""type"": ""STRING"" }
                                },
                                ""required"": [""name"", ""id""]
                             }
                           },
                           ""required"": [""productName"", ""category""]
                         }
                       }").RootElement;

        try
        {
            var responseCategorizedProduct = await geminiService.SendRequestAsync(
                [
                    new Part
                    {
                        Text = $@"Categorize products in the most appropriate category.
                          If no appropriate category is found, categorize product it in category ""Ostale namirnice i proizvodi"".
                          Expand abbreviations when you're confident (e.g. DALM → DALMATINSKA, DETERDŽ → DETERDŽENT, POLUTV → POLUTVRDI, EX DJEV → EKSTRA DJEVIČANSKO, and other similar).
                          Keep the product name close to the original — do not translate, do not rewrite whole phrases, and do not invent new words.

                          INPUT: {JsonSerializer.Serialize(new { ProductNames = productNames, Categories = allCategories }, new JsonSerializerOptions
                          {
                              WriteIndented = false,
                              Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                          })}
                         "
                    }
                ],
                responseSchema
            );

            var categorizedProducts = JsonSerializer.Deserialize<List<CategorizedProduct>>
            (
                responseCategorizedProduct, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if (categorizedProducts == null)
            {
                return null;
            }

            return categorizedProducts;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}