using System.Text.Json.Serialization;

namespace PravaCijena.Api.Services.Gemini.GeminiResponse;

public class TokenDetail
{
    [JsonPropertyName("modality")] public string Modality { get; set; }
    [JsonPropertyName("tokenCount")] public int TokenCount { get; set; }
}