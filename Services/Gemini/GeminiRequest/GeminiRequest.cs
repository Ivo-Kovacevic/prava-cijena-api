using System.Text.Json.Serialization;

namespace PravaCijena.Api.Services.Gemini.GeminiRequest;

public class GeminiRequest
{
    [JsonPropertyName("contents")] public List<Content> Contents { get; set; }
    [JsonPropertyName("generationConfig")] public GenerationConfig GenerationConfig { get; set; }
}
