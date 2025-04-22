using System.Text.Json.Serialization;

namespace PravaCijena.Api.Services.Gemini.GeminiRequest;

public class InlineData
{
    [JsonPropertyName("mime_type")] public required string MimeType { get; set; }
    [JsonPropertyName("data")] public required string Data { get; set; }
}