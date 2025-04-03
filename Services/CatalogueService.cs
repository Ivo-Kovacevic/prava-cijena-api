using System.Text;
using System.Text.Json;
using PravaCijena.Api.Models;
using PravaCijena.Api.Config;
using PravaCijena.Api.Interfaces;

namespace PravaCijena.Api.Services;

public class CatalogueService : ApiConfig, ICatalogueService
{
    private readonly HttpClient _httpClient;

    public CatalogueService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> ExtractDataFromPdf(IFormFile pdfFile)
    {
        using var memoryStream = new MemoryStream();
        await pdfFile.CopyToAsync(memoryStream);
        var base64Pdf = Convert.ToBase64String(memoryStream.ToArray());
        
        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new object[]
                    {
                        new
                        {
                            inline_data = new
                            {
                                mime_type = "application/pdf",
                                data = base64Pdf
                            }
                        },
                        new
                        {
                            text =
                                "Extract product names and prices from this document.\nReturn the results as a JSON object with the following format:\n[\n  {\n    \"product\": \"Product Name\",\n    \"price\": \"Price\"\n  },\n  ...\n]\n\nProduct name needs to be in the following format: \"brand-name product-name other-product-info product-weight-or-volume\"\nExample: \"Dukat svježe mlijeko 3,2 % m.m. 1L\""
                        }
                    }
                }
            }
        };

        var requestJson = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

        // Send request to Google Gemini API
        var responseMessage = await _httpClient.PostAsync(
            $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={GeminiApiKey}",
            content
        );

        if (!responseMessage.IsSuccessStatusCode)
        {
            return $"Error: {responseMessage.StatusCode} - {await responseMessage.Content.ReadAsStringAsync()}";
        }

        return await responseMessage.Content.ReadAsStringAsync();
    }
}