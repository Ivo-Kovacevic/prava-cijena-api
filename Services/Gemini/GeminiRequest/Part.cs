using System.Text.Json.Serialization;

namespace PravaCijena.Api.Services.Gemini.GeminiRequest;

public class Part
{
    [JsonPropertyName("text")] public string? Text { get; set; }
    [JsonPropertyName("inline_data")] public InlineData? InlineData { get; set; }
}