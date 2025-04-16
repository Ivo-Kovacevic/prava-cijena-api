using System.Text;
using System.Text.Json;
using PravaCijena.Api.Config;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Services.Gemini.GeminiRequest;

namespace PravaCijena.Api.Services.Gemini;

public class GeminiService : ApiConfig, IGeminiService
{
    private readonly HttpClient _httpClient;

    public GeminiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> CompareProductNamesAsync(string existingProduct, string newProduct)
    {
        var requestBody = new GeminiRequest.GeminiRequest()
        {
            Contents =
            [
                new Content
                {
                    Parts =
                    [
                        new Part
                        {
                            Text = $@"You are an AI specializing in grocery product name matching.
                            Input: {{
                                existingProduct: {existingProduct},
                                newProduct: {newProduct}
                            }}

                            Task: Determine if the two names refer to the exact same product item, considering brand, variant, and *exact* size/quantity. Ignore minor differences in capitalization, punctuation, word order, and common abbreviations (g/G, l/L).
                            Output: Respond *only* with a JSON object:
                            {{
                              ""productName"": ""[First input existing product name]"",
                              ""sameProduct"": [true if they are the same item, false otherwise]
                            }}"
                        }
                    ]
                }
            ],
            GenerationConfig = new GenerationConfig
            {
                ResponseMimeType = "application/json"
            }
        };

        var url =
            $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={GeminiApiKey}";
        var json = JsonSerializer.Serialize(requestBody);
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        return responseBody;
    }
}

