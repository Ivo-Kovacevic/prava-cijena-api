using System.Text.Json;
using System.Text.Json.Serialization;

namespace PravaCijena.Api.Services.Gemini.GeminiRequest;

public class GenerationConfig
{
    [JsonPropertyName("response_mime_type")]
    public string ResponseMimeType { get; set; }

    [JsonPropertyName("responseSchema")] public JsonElement? ResponseSchema { get; set; }
}