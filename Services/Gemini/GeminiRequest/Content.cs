using System.Text.Json.Serialization;

namespace PravaCijena.Api.Services.Gemini.GeminiRequest;

public class Content
{
    [JsonPropertyName("parts")] public required List<Part> Parts { get; set; }
}