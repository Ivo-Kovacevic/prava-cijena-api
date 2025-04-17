using System.Text.Json.Serialization;

namespace PravaCijena.Api.Services.Gemini.GeminiResponse;

public class Part
{
    [JsonPropertyName("text")] public string Text { get; set; }
}