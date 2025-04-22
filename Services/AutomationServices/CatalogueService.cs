using System.Text.Json;
using HtmlAgilityPack;
using PravaCijena.Api.Config;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;
using PravaCijena.Api.Services.Gemini.GeminiRequest;

namespace PravaCijena.Api.Services.AutomationServices;

public class CatalogueService : ApiConfig, ICatalogueService
{
    private readonly IAutomationService _automationService;
    private readonly IGeminiService _geminiService;
    private readonly HttpClient _httpClient;

    public CatalogueService(HttpClient httpClient, IAutomationService automationService, IGeminiService geminiService)
    {
        _httpClient = httpClient;
        _geminiService = geminiService;
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

        var store = new
        {
            Name = "Plodine",
            CatalogueListUrl = "https://www.plodine.hr/aktualni-katalozi",
            CatalogueListXPath = "//div[@class='catalog__wrap']//a[@class='btn btn--iconR']"
        };

        var html = await _httpClient.GetStringAsync(store.CatalogueListUrl);
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        var pdfUrls = htmlDocument.DocumentNode.SelectNodes(store.CatalogueListXPath)
            .Select(p => p.GetAttributeValue("href", ""));


        foreach (var pdfUrl in pdfUrls)
        {
            var pdfBytes = await _httpClient.GetByteArrayAsync(pdfUrl);
            var base64Pdf = Convert.ToBase64String(pdfBytes);
            var response = await _geminiService.SendRequestAsync([
                new Part
                {
                    Text = @"You are an AI specializing in extracting product names and prices from catalogues.

                             ### TASK
                             Extract product names and prices from this document. Return the results as a JSON object with the following format:
                             Only extract food and drinks, not other non edible products. Keep only the first letter capital. 
                             Product name needs to be in the following format: ""brand-name product-name other-product-info product-weight-or-volume""
                             Product name and weight/volume MUST BE INCLUDED because otherwise it is impossible to recognize which product it is. Include brand name and other info where applicable.

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

        return results;
    }
}