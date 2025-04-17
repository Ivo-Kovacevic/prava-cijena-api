using System.Text.Json.Serialization;

namespace PravaCijena.Api.Services.Gemini.GeminiResponse;

public class GeminiResponseModel
{
    [JsonPropertyName("candidates")] public List<Candidate> Candidates { get; set; }
    [JsonPropertyName("usageMetaData")] public UsageMetadata UsageMetaData { get; set; }
    [JsonPropertyName("modelVersion")] public string ModelVersion { get; set; }
}