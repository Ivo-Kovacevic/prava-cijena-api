using System.Xml;
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
        // var storesWithMetadata = await _storeRepository.GetAllWithCategories();
        var random = new Random();
        var dummyUrls = new List<string>
        {
            "https://res.cloudinary.com/dqbe0apqn/raw/upload/v1747162753/products.csv",
            "https://res.cloudinary.com/dqbe0apqn/raw/upload/v1747162753/products.xml"
        };

        foreach (var url in dummyUrls)
            try
            {
                // ------------ Add random delay between getting files ------------
                Console.WriteLine($"Delaying request to \"{url}\" by few seconds...");
                await Task.Delay(random.Next(2, 5) * 1000);

                await using var stream = await _httpClient.GetStreamAsync(url);
                using var reader = new StreamReader(stream);
                var fileContent = await reader.ReadToEndAsync();

                var extension = Path.GetExtension(url).ToLower();
                var productPreviews = FileDataToProductPreviewList(fileContent, extension);

                // await _automationService.HandleFoundProducts(
                //     productPreviews,
                //     store,
                //     null,
                //     results
                // );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to scrape URL: {url}, Error: {ex.Message}");
            }
    }

    private List<ProductPreview>? FileDataToProductPreviewList(string fileContent, string extension)
    {
        switch (extension)
        {
            case ".csv":
                return ParseCsv(fileContent);
            case ".xml":
                return ParseXml(fileContent);
            default:
                return null;
        }
    }

    private List<ProductPreview> ParseCsv(string content)
    {
        var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var previews = new List<ProductPreview>();

        foreach (var line in lines.Skip(1))
        {
            var cols = line.Split(',');
            if (cols.Length < 4)
            {
                continue;
            }

            previews.Add(new ProductPreview
            {
                Name = cols[0],
                Price = decimal.Parse(cols[1])
            });
        }

        return previews;
    }

    private List<ProductPreview> ParseXml(string xmlData)
    {
        var doc = new XmlDocument();
        doc.LoadXml(xmlData);

        var previews = new List<ProductPreview>();
        var nodes = doc.SelectNodes("//Product");

        foreach (XmlNode node in nodes)
            previews.Add(new ProductPreview
            {
                Name = node["Name"]?.InnerText,
                Price = decimal.Parse(node["Price"]?.InnerText ?? "0")
            });

        return previews;
    }
}