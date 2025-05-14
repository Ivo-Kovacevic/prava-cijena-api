using System.Text.Json;
using HtmlAgilityPack;
using PravaCijena.Api.Config;
using PravaCijena.Api.Dto.Store;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;
using PravaCijena.Api.Services.Gemini.GeminiRequest;

namespace PravaCijena.Api.Services.AutomationServices;

public class CatalogueService : ApiConfig, ICatalogueService
{
    private readonly IAutomationService _automationService;
    private readonly IGeminiService _geminiService;
    private readonly HttpClient _httpClient;
    private readonly IStoreRepository _storeRepository;

    public CatalogueService(
        HttpClient httpClient,
        IAutomationService automationService,
        IGeminiService geminiService,
        IStoreRepository storeRepository
    )
    {
        _httpClient = httpClient;
        _geminiService = geminiService;
        _automationService = automationService;
        _storeRepository = storeRepository;
    }

    public async Task<AutomationResult> AnalyzePdfs()
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
        var stores = await _storeRepository.GetAllWithCategories();

        foreach (var store in stores)
        {
            if (store.CatalogueListUrl == null || store.CatalogueListXPath == null)
            {
                continue;
            }

            var html = await _httpClient.GetStringAsync(store.CatalogueListUrl);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var catalogueNodes = htmlDocument.DocumentNode.SelectNodes(store.CatalogueListXPath);

            var catalogueUrls = CatalogueNodesToCatalogueUrls(catalogueNodes, store);

            foreach (var catalogueUrl in catalogueUrls)
            {
                var pdfBytes = await _httpClient.GetByteArrayAsync(catalogueUrl);
                var base64Pdf = Convert.ToBase64String(pdfBytes);
                var response = await _geminiService.SendRequestAsync([
                    new Part
                    {
                        Text = @"You are an AI specializing in extracting product names and prices from catalogues.

                             ### TASK
                             Extract only specific food and drink products with clear names and prices from this document.

                             1. Ignore and do not include vague or generic product groups like:
                                - ""više vrsta"", ""razne vrste"", ""odabrane vrste"", etc.
                                - Products without listed flavor/type/variant names
                                - Products missing weight or volume
                             
                             2. If multiple variants are explicitly listed (e.g. ""lemon grass, green vital or vanilla""), create a separate product for each one:
                                - Example: ""Afrodita tekući sapun lemon grass, green vital ili vanilla 1 l"" →  
                                  - ""Afrodita tekući sapun lemon grass 1 l""  
                                  - ""Afrodita tekući sapun green vital 1 l""  
                                  - ""Afrodita tekući sapun vanilla 1 l""

                             3. Do not guess or hallucinate variant names. If they aren't explicitly mentioned, skip the product.

                             4. Product names must include brand, product type, variant/flavor (if any), and weight/volume.

                             ### OUTPUT FORMAT
                             Output must be in the following format:
                             [
                               {
                                   ""name"": ""Product Name"",
                                   ""price"": decimal  
                               },
                               ...
                             ]

                             ### EXAMPLE OUTPUT 
                             [
                               {
                                   ""name"": ""Dukat svježe mlijeko 3,2 % m.m. 1L"",
                                   ""price"": 1.49  
                               },
                               ...
                             ]
                    "
                    },
                    new Part
                    {
                        InlineData = new InlineData
                        {
                            MimeType = "application/pdf",
                            Data = base64Pdf
                        }
                    }
                ]);

                var result = JsonSerializer.Deserialize<List<ProductPreview>>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                Console.WriteLine(result);
            }
        }

        return results;
    }

    private static List<string> CatalogueNodesToCatalogueUrls(
        HtmlNodeCollection catalogueNodes,
        StoreWithMetadataDto store
    )
    {
        var catalogueUrls = new List<string>();

        foreach (var catalogueNode in catalogueNodes)
        {
            string catalogueUrl;
            switch (store.Slug)
            {
                case "plodine":
                    catalogueUrl = GetPlodineCatalogueUrl(catalogueNode);
                    break;
                case "kaufland":
                    catalogueUrl = GetSparCatalogueUrl(catalogueNode);
                    break;
                case "studenac":
                    catalogueUrl = GetStudenacCatalogueUrl(catalogueNode);
                    break;
                default:
                    continue;
            }

            if (catalogueUrl.Length > 0)
            {
                catalogueUrls.Add(catalogueUrl);
            }
        }

        return catalogueUrls;
    }

    private static string GetPlodineCatalogueUrl(HtmlNode catalogueNode)
    {
        return catalogueNode.GetAttributeValue("href", "");
    }

    private static string GetSparCatalogueUrl(HtmlNode catalogueNode)
    {
        return catalogueNode.GetAttributeValue("data-download-url", "");
    }

    private static string GetStudenacCatalogueUrl(HtmlNode catalogueNode)
    {
        return catalogueNode.SelectNodes(".//a")[1].GetAttributeValue("href", "");
    }
}