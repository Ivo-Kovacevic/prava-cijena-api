using System.Text.Json.Serialization;

namespace PravaCijena.Api.Services.Gemini.GeminiResponse;

public class Content
{
    [JsonPropertyName("parts")] public List<Part> Parts { get; set; }
    [JsonPropertyName("role")] public string Role { get; set; }
}