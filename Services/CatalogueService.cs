using System.Text;
using System.Text.Json;

namespace api.Services;

public class CatalogueService
{
    private readonly string _geminiApiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
    private readonly HttpClient _httpClient;

    public CatalogueService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> ExtractDataFromPdf(string pdfFilePath, string prompt)
    {
        if (!File.Exists(pdfFilePath))
        {
            return "Error: File not found.";
        }

        // Read the PDF file as bytes
        var pdfData = await File.ReadAllBytesAsync(pdfFilePath);

        // Convert to Base64
        var base64Pdf = Convert.ToBase64String(pdfData);

        // Prepare the request body
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
                        new { text = prompt }
                    }
                }
            }
        };

        var requestJson = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

        // Send request to Google Gemini API
        var responseMessage = await _httpClient.PostAsync(
            $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_geminiApiKey}",
            content
        );

        if (!responseMessage.IsSuccessStatusCode)
        {
            return $"Error: {responseMessage.StatusCode} - {await responseMessage.Content.ReadAsStringAsync()}";
        }

        return await responseMessage.Content.ReadAsStringAsync();
    }
}