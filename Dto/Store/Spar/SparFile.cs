using System.Text.Json.Serialization;

namespace PravaCijena.Api.Dto.Store.Spar;


public class SparFile
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("URL")]
    public string Url { get; set; }

    [JsonPropertyName("SHA")]
    public string Sha { get; set; }
}