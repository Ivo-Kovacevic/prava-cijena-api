using System.IO.Compression;
using System.Text;
using PravaCijena.Api.Dto.Store;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Services.AutomationServices;

public class StructuredDataService : IStructuredDataService
{
    private readonly IAutomationService _automationService;
    private readonly HttpClient _httpClient;
    private readonly IStoreRepository _storeRepository;


    public StructuredDataService(
        IAutomationService automationService,
        HttpClient httpClient,
        IStoreRepository storeRepository
    )
    {
        _automationService = automationService;
        _httpClient = httpClient;
        _storeRepository = storeRepository;
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
        var storesWithMetadata = await _storeRepository.GetAllWithCategories();

        foreach (var store in storesWithMetadata)
            switch (store.Slug)
            {
                case "kaufland":
                    await HandleKaufland(store);
                    break;
                default:
                    continue;
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

            foreach (var preview in previews)
            {
            }
        }
    }
}