using System.Text;
using System.Text.Json;
using PravaCijena.Api.Config;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;
using PravaCijena.Api.Services.Gemini.GeminiRequest;
using PravaCijena.Api.Services.Gemini.GeminiResponse;
using Content = PravaCijena.Api.Services.Gemini.GeminiRequest.Content;
using Part = PravaCijena.Api.Services.Gemini.GeminiRequest.Part;

namespace PravaCijena.Api.Services.Gemini;

public class GeminiService : ApiConfig, IGeminiService
{
    private readonly HttpClient _httpClient;

    public GeminiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ComparedResult> CompareProductsAsync(MappedProduct mappedProducts)
    {
        var requestBody = new GeminiRequestModel
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
                            Input: {JsonSerializer.Serialize(mappedProducts)}

                            Each object has:
                            - existingProduct: The product already in our database.
                            - productPreview: A newly scraped product to compare.

                            ### TASK:
                            Determine if the *existingProduct.name* and *productPreview.name* are same products, considering brand, variant, and *exact* size/quantity.
                            Ignore minor differences in capitalization, punctuation, word order, and common abbreviations (g/G, l/L).

                            ### OUTPUT RULES:
                            - Output MUST be a valid JSON object, not array, just single object.
                            - Each object must have:
                              - existingProduct (unchanged from input)
                              - productPreview (unchanged from input)
                              - isSameProduct (true/false)
                            - Do NOT change or truncate any fields.
                            - Do NOT include comments or explanations.
                            - Do NOT output anything except the JSON.

                            ### EXAMPLE OUTPUT:
                            {{
                               ""existingProduct"": {{ ... }},
                               ""productPreview"": {{ ... }},
                               ""isSameProduct"": true
                            }}
                            "
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
        var geminiResponse = JsonSerializer.Deserialize<GeminiResponseModel>(responseBody, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var text = geminiResponse.Candidates.First().Content.Parts.First().Text;
        var compareResult = JsonSerializer.Deserialize<ComparedResult>(text, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return compareResult;
    }
}