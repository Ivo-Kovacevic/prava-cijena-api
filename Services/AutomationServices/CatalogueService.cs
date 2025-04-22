using System.Text.Json;
using PravaCijena.Api.Config;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;
using PravaCijena.Api.Services.Gemini.GeminiRequest;
using Part = PravaCijena.Api.Services.Gemini.GeminiRequest.Part;

namespace PravaCijena.Api.Services.AutomationServices;

public class CatalogueService : ApiConfig, ICatalogueService
{
    private readonly IGeminiService _geminiService;
    private readonly HttpClient _httpClient;

    public CatalogueService(HttpClient httpClient, IGeminiService geminiService)
    {
        _httpClient = httpClient;
        _geminiService = geminiService;
    }

    public async Task<List<ProductPreview>> AnalyzePdf(IFormFile pdfFile)
    {
        using var memoryStream = new MemoryStream();
        await pdfFile.CopyToAsync(memoryStream);
        var base64Pdf = Convert.ToBase64String(memoryStream.ToArray());

        // Send request to Google Gemini API
        var response = await _geminiService.SendRequestAsync([
            new Part
            {
                Text =
                    "Extract product names and prices from this document.\nReturn the results as a JSON object with the following format:\n[\n  {\n    \"name\": \"Product Name\",\n    \"price\": decimal  },\n  ...\n]\n\nKeep only the first letter capital. Product name needs to be in the following format: \"brand-name product-name other-product-info product-weight-or-volume\"\nExample: \"Dukat svježe mlijeko 3,2 % m.m. 1L\"."
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

        return result;
    }
}