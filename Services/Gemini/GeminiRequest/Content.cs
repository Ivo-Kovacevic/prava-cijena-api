using System.Text.Json.Serialization;

namespace PravaCijena.Api.Services.Gemini.GeminiRequest;

public class Content
{
    [JsonPropertyName("parts")] public List<Part> Parts { get; set; }
}