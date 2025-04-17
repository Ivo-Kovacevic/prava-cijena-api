using System.Text.Json.Serialization;

namespace PravaCijena.Api.Services.Gemini.GeminiResponse;

public class Candidate
{
    [JsonPropertyName("content")] public Content Content { get; set; }
    [JsonPropertyName("finishReason")] public string FinishReason { get; set; }
    [JsonPropertyName("avgLogprobs")] public double AvgLogprobs { get; set; }
}