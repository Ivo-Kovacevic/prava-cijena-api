using System.Text;
using System.Text.Json;
using PravaCijena.Api.Config;
using PravaCijena.Api.Interfaces;
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

    public async Task<string> SendRequestAsync(List<Part> parts, JsonElement? responseSchema)
    {
        var requestBody = new GeminiRequestModel
        {
            Contents =
            [
                new Content
                {
                    Parts = parts
                }
            ],
            GenerationConfig = new GenerationConfig
            {
                ResponseMimeType = "application/json",
                ResponseSchema = responseSchema
            }
        };

        var url =
            $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash-lite:generateContent?key={GeminiApiKey}";
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

        return geminiResponse.Candidates.First().Content.Parts.First().Text;
    }
}