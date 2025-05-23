using System.IO.Compression;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Web;
using System.Xml.Linq;
using HtmlAgilityPack;
using PravaCijena.Api.Context;
using PravaCijena.Api.Dto.Store;
using PravaCijena.Api.Dto.Store.Spar;
using PravaCijena.Api.Helpers;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Mappers;
using PravaCijena.Api.Models;
using PravaCijena.Api.Services.Gemini.GeminiRequest;
using static System.Text.RegularExpressions.Regex;

namespace PravaCijena.Api.Services.AutomationServices;

public class StructuredDataService : IStructuredDataService
{
    private readonly IAutomationService _automationService;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ProductProcessingContext _context;
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
        ICategoryRepository categoryRepository,
        ProductProcessingContext productProcessingContext
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
        _context = productProcessingContext;
    }

    public async Task SyncStoreFiles()
    {
        var results = new AutomationResult();
        var storesWithMetadata = await _storeRepository.GetAllWithMetadata();

        await _context.InitializeAsync(_productRepository);

        foreach (var store in storesWithMetadata)
            try
            {
                if (store.PriceUrlListXPath == null || store.PriceUrlXPath == null)
                {
                    continue;
                }

                switch (store.Slug)
                {
                    case "konzum-":
                        await HandleKonzum(store);
                        break;
                    case "lidl-":
                        await HandleLidl(store);
                        break;
                    case "studenac-":
                        await HandleStudenac(store);
                        break;
                    case "plodine-":
                        await HandlePlodine(store);
                        break;
                    case "spar":
                        await HandleSpar(store);
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

    private async Task HandleKonzum(StoreWithMetadataDto store)
    {
        var page = 1;
        var hasMoreProducts = true;

        while (hasMoreProducts)
        {
            var pageUrl = $"{store.PriceListUrl}?page={page}";

            var html = await _httpClient.GetStringAsync(pageUrl);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var urlNodes = htmlDocument.DocumentNode.SelectNodes("//a[@format='csv']")
                .DistinctBy(node => node.GetAttributeValue("href", string.Empty))
                .ToList();

            if (urlNodes.Count < 50)
            {
                hasMoreProducts = false;

                if (urlNodes.Count == 0)
                {
                    continue;
                }
            }

            foreach (var urlNode in urlNodes)
            {
                var href = urlNode.GetAttributeValue("href", "");
                var fullUrl = new Uri(new Uri(store.StoreUrl), href).ToString();
                var csvData = await _httpClient.GetStringAsync(fullUrl);

                var uri = new Uri(fullUrl);
                var queryParams = HttpUtility.ParseQueryString(uri.Query);
                var titleRaw = queryParams["title"];

                if (string.IsNullOrEmpty(titleRaw))
                {
                    continue;
                }

                var decodedTitle = Uri.UnescapeDataString(titleRaw);
                var filenameWithoutExtension = Path.GetFileNameWithoutExtension(decodedTitle);
                var parts = filenameWithoutExtension.Split(',');

                if (parts.Length < 2)
                {
                    continue;
                }

                var addressCity = parts[1];

                var match = Match(addressCity, @"(.+?)\s(\d{5})\s(.+)");
                if (!match.Success)
                {
                    continue;
                }

                var address = match.Groups[1].Value.Trim().ToUpper();
                var city = match.Groups[3].Value.Trim().ToUpper();

                var storeLocation = await _storeLocationRepository.GetByCityAndAddressAsync(city, address);
                if (storeLocation == null)
                {
                    storeLocation = await _storeLocationRepository.Create(new StoreLocation
                    {
                        City = city,
                        Address = address,
                        StoreId = store.Id
                    });
                }

                var lines = csvData.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                await HandleCsv(lines, store, storeLocation);
            }

            page++;
        }
    }

    private async Task HandleLidl(StoreWithMetadataDto store)
    {
        var html = await _httpClient.GetStringAsync(store.PriceListUrl);
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);
        var urlNodes = htmlDocument.DocumentNode.SelectNodes("//a[contains(@href, '.zip')]");

        var zipUrl = urlNodes.Last().GetAttributeValue("href", "");

        var zipBytes = await _httpClient.GetByteArrayAsync(zipUrl);
        using var zipStream = new MemoryStream(zipBytes);
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);

        foreach (var entry in archive.Entries)
        {
            var match = Match(entry.FullName, @"^[^\s]+ \d+_(.+?)_([\d\s_A-Za-z]+)_([0-9]{5})_(.+?)_");

            if (!match.Success)
            {
                continue;
            }

            var street = match.Groups[1].Value.Replace('_', ' ').Trim().ToUpper();
            var houseNumber = match.Groups[2].Value.Replace('_', ' ').Trim().ToUpper();
            var city = match.Groups[4].Value.Replace('_', ' ').Trim().ToUpper();

            var address = $"{street} {houseNumber}";

            await using var entryStream = entry.Open();
            using var reader = new StreamReader(entryStream, Encoding.GetEncoding("windows-1250"));

            var content = await reader.ReadToEndAsync();

            var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            var storeLocation = await _storeLocationRepository.GetByCityAndAddressAsync(city, address);
            if (storeLocation == null)
            {
                storeLocation = await _storeLocationRepository.Create(new StoreLocation
                {
                    City = city,
                    Address = address,
                    StoreId = store.Id
                });
            }

            await HandleCsv(lines, store, storeLocation);
        }
    }

    private async Task HandleStudenac(StoreWithMetadataDto store)
    {
        var date = DateTime.Today;
        var zipUrl = $"https://www.studenac.hr/cjenici/PROIZVODI-{date:yyyy-MM-dd}.zip";

        var zipBytes = await _httpClient.GetByteArrayAsync(zipUrl);
        using var zipStream = new MemoryStream(zipBytes);
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);

        foreach (var entry in archive.Entries)
        {
            await using var entryStream = entry.Open();
            var xDoc = XDocument.Load(entryStream);

            var fullAddress = xDoc.Descendants("Adresa").First().Value;
            var pattern = @"^(.*?)([A-ZČĆĐŠŽ][A-ZČĆĐŠŽ\s]+)$";
            var match = Match(fullAddress, pattern);

            if (!match.Success)
            {
                continue;
            }

            var address = match.Groups[1].Value.Trim().ToUpper();
            var city = match.Groups[2].Value.Trim().ToUpper();

            var storeLocation = await _storeLocationRepository.GetByCityAndAddressAsync(city, address);
            if (storeLocation == null)
            {
                storeLocation = await _storeLocationRepository.Create(new StoreLocation
                {
                    City = city,
                    Address = address,
                    StoreId = store.Id
                });
            }

            await HandleXml(xDoc, store, storeLocation);
        }
    }

    private async Task HandlePlodine(StoreWithMetadataDto store)
    {
        var html = await _httpClient.GetStringAsync(store.PriceListUrl);
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);
        var urlNodes = htmlDocument.DocumentNode.SelectNodes("//ul[@class='accordion']//a");

        var zipUrl = urlNodes.First().GetAttributeValue("href", "");

        var zipBytes = await _httpClient.GetByteArrayAsync(zipUrl);
        using var zipStream = new MemoryStream(zipBytes);
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);

        foreach (var entry in archive.Entries)
        {
            var parts = Path.GetFileNameWithoutExtension(entry.FullName).Split('_');

            if (parts.Length < 6)
            {
                continue;
            }

            // Skip the first part (HIPERMARKET / SUPERMARKET)
            var streetParts = new List<string>();
            var i = 1;

            // Collect street parts until we hit the house number (e.g., "14A", "2", "36B")
            while (i < parts.Length && !IsMatch(parts[i], @"^\d+[A-Z]*$"))
            {
                streetParts.Add(parts[i]);
                i++;
            }

            if (i >= parts.Length - 2)
            {
                continue;
            }

            var houseNumber = parts[i++];
            var zip = parts[i++];
            var cityParts = new List<string>();

            // Remaining parts (at least one expected) are city name (can be multi-word)
            while (i < parts.Length && !IsMatch(parts[i], @"^\d{3}$")) // until we hit store code like 064
            {
                cityParts.Add(parts[i]);
                i++;
            }

            var address = $"{string.Join(' ', streetParts)} {houseNumber}".ToUpper();
            var city = string.Join(' ', cityParts).ToUpper();

            await using var entryStream = entry.Open();
            using var reader = new StreamReader(entryStream, Encoding.GetEncoding("windows-1250"));

            var content = await reader.ReadToEndAsync();

            var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            var storeLocation = await _storeLocationRepository.GetByCityAndAddressAsync(city, address);
            if (storeLocation == null)
            {
                storeLocation = await _storeLocationRepository.Create(new StoreLocation
                {
                    City = city,
                    Address = address,
                    StoreId = store.Id
                });
            }

            await HandleCsv(lines, store, storeLocation);
        }
    }

    private async Task HandleSpar(StoreWithMetadataDto store)
    {
        var date = DateTime.UtcNow; // Or local time if needed
        var dateString = date.ToString("yyyyMMdd");
        var jsonUrl = $"https://www.spar.hr/datoteke_cjenici/Cjenik{dateString}.json";

        var response = await _httpClient.GetStringAsync(jsonUrl);

        var data = JsonSerializer.Deserialize<SparFileList>(response);

        if (data?.Files == null)
        {
            return;
        }

        foreach (var file in data.Files)
        {
            var fullUrl = file.Url;
            var csvData = await _httpClient.GetStringAsync(fullUrl);

            var fileName = Path.GetFileNameWithoutExtension(file.Name);
            var parts = fileName.Split('_');

            // Let's assume the city is the second word after 'hipermarket_'
            // e.g. hipermarket_zadar_bleiburskih... => city = zadar
            var city = parts.Length > 1 ? parts[1].ToUpper() : null;

            // Address = everything between city and the "_interspar" part
            var addressParts = parts.Skip(2)
                .TakeWhile(p => !p.StartsWith("interspar", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var address = string.Join(' ', addressParts).ToUpper();

            if (string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(address))
            {
                continue;
            }

            var storeLocation = await _storeLocationRepository.GetByCityAndAddressAsync(city, address);
            if (storeLocation == null)
            {
                storeLocation = await _storeLocationRepository.Create(new StoreLocation
                {
                    City = city,
                    Address = address,
                    StoreId = store.Id
                });
            }

            var lines = csvData.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            await HandleCsv(lines, store, storeLocation);
        }
    }

    private async Task HandleCsv(string[] lines, StoreWithMetadataDto store, StoreLocation storeLocation)
    {
        if (store.CsvNameColumn == null || store.CsvBrandColumn == null || store.CsvPriceColumn == null ||
            store.CsvBarcodeColumn == null)
        {
            return;
        }

        var productPreviews = new List<ProductPreview>();

        foreach (var line in lines.Skip(1))
        {
            var cols = line.Split(store.CsvDelimiter);
            var productName = cols[store.CsvNameColumn.Value].Trim();
            var productBrand = cols[store.CsvBrandColumn.Value].Trim();
            var barcode = cols[store.CsvBarcodeColumn.Value].Trim();
            var priceStr = cols[store.CsvPriceColumn.Value].Trim().Replace(',', '.');

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

        await ProcessProductPreviews(productPreviews, storeLocation);
    }

    private async Task ProcessProductPreviews(List<ProductPreview> productPreviews, StoreLocation storeLocation)
    {
        // var productsToCreate = new List<Product>();
        var productsToUpdate = new List<Product>();
        var productStoresToUpdate = new List<ProductStore>();
        var productPreviewsToCategorize = new List<ProductPreview>();
        var productStoresToCreate = new List<ProductStore>();

        var allBarcodesInFile = productPreviews.Select(x => x.Barcode).Distinct().ToList();

        var existingProducts = (await _productRepository.GetProductsByBarcodesBatchAsync(allBarcodesInFile))
            .ToDictionary(p => p.Barcode, StringComparer.OrdinalIgnoreCase);
        var existingProductStores =
            (await _productStoreRepository.GetProductStoresByProductBarcodesBatchAsync(allBarcodesInFile,
                storeLocation.Id)).ToDictionary(ps => ps.Product.Barcode, StringComparer.OrdinalIgnoreCase);

        foreach (var productPreview in productPreviews)
            // Schedule new product to create cause barcode doesn't exist
            if (_context.ExistingBarcodes.TryAdd(productPreview.Barcode, 0))
            {
                var baseName = productPreview.Name;
                var slug = SlugHelper.GenerateSlug(baseName);
                var i = 0;

                while (!_context.ExistingSlugs.TryAdd(slug, 0))
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

                productPreviewsToCategorize.Add(productPreview);
            }

            // Barcode already exist so check if price needs to be updated
            else
            {
                if (existingProducts.TryGetValue(productPreview.Barcode, out var existingProduct))
                {
                    if (productPreview.Price < existingProduct.LowestPrice)
                    {
                        existingProduct.LowestPrice = productPreview.Price;
                        productsToUpdate.Add(existingProduct);
                    }
                }

                if (existingProductStores.TryGetValue(productPreview.Barcode, out var existingProductStore))
                {
                    existingProductStore.LatestPrice = productPreview.Price;
                    productStoresToUpdate.Add(existingProductStore);
                }
                else if (existingProduct != null &&
                         !productStoresToCreate.Any(ps =>
                             ps.ProductId == existingProduct.Id && ps.StoreLocationId == storeLocation.Id))
                {
                    productStoresToCreate.Add(new ProductStore
                    {
                        Id = Guid.NewGuid(),
                        ProductId = existingProduct.Id,
                        StoreLocationId = storeLocation.Id,
                        LatestPrice = productPreview.Price
                    });
                }
            }

        try
        {
            await _productRepository.BulkUpdateAsync(productsToUpdate);
            await _productStoreRepository.BulkCreateAsync(productStoresToCreate);
            await _productStoreRepository.BulkUpdateAsync(productStoresToUpdate);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        if (productPreviewsToCategorize.Count > 0)
        {
            var semaphore = new SemaphoreSlim(20);
            var tasks = new List<Task>();

            var batches = productPreviewsToCategorize
                .Select((product, index) => new { product, index })
                .GroupBy(x => x.index / 5)
                .Select(g => g.Select(x => x.product).ToList());

            foreach (var batch in batches)
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        await semaphore.WaitAsync();
                        using var scope = _serviceProvider.CreateScope();
                        var productStoreRepo = scope.ServiceProvider.GetRequiredService<IProductStoreRepository>();
                        var categoryRepo = scope.ServiceProvider.GetRequiredService<ICategoryRepository>();
                        var geminiService = scope.ServiceProvider.GetRequiredService<IGeminiService>();
                        var productRepo = scope.ServiceProvider.GetRequiredService<IProductRepository>();

                        var productNames = batch.Select(p => p.Name).ToList();
                        var results = await CategorizeProducts(productNames, categoryRepo, geminiService);

                        if (results == null)
                        {
                            return;
                        }

                        var productsToCreate = new List<Product>();
                        for (var i = 0; i < results.Count; i++)
                            productsToCreate.Add(new Product
                            {
                                Name = batch[i].Name,
                                Brand = batch[i].Brand,
                                CategoryId = results[i].Category.Id,
                                LowestPrice = batch[i].Price,
                                Barcode = batch[i].Barcode
                            });

                        await productRepo.BulkCreateAsync(productsToCreate);
                        var newProducts = await productRepo
                            .GetProductsByBarcodesBatchAsync(productsToCreate.Select(p => p.Barcode).ToList());

                        var productStores = new List<ProductStore>();
                        foreach (var newProduct in newProducts)
                            productStores.Add(new ProductStore
                            {
                                ProductId = newProduct.Id,
                                StoreLocationId = storeLocation.Id
                            });

                        await productStoreRepo.BulkCreateAsync(productStores);
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

            await Task.WhenAll(tasks);
        }
    }

    private async Task HandleXml(XDocument xDoc, StoreWithMetadataDto store, StoreLocation storeLocation)
    {
        if (store.XmlNameElement == null || store.XmlBrandElement == null || store.XmlPriceElement == null ||
            store.XmlBarcodeElement == null)
        {
            return;
        }

        var productPreviews = new List<ProductPreview>();

        foreach (var p in xDoc.Descendants("Proizvod"))
        {
            var productName = p.Element(store.XmlNameElement)?.Value;
            var productBrand = p.Element(store.XmlBrandElement)?.Value;
            var priceStr = p.Element(store.XmlPriceElement)?.Value;
            var barcode = p.Element(store.XmlBarcodeElement)?.Value;

            if (productName == null || productBrand == null || !decimal.TryParse(priceStr, out var price) ||
                barcode == null || barcode.Length < 8)
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

        await ProcessProductPreviews(productPreviews, storeLocation);
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

    private async Task<List<CategorizedProduct>?> CategorizeProducts(List<string> productNames,
        ICategoryRepository categoryRepo, IGeminiService geminiService)
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