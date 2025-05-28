using System.Text.Json.Serialization;

namespace PravaCijena.Api.Dto.Store.Tommy;

public class TommyFile
{
    [JsonPropertyName("@id")] public string Id { get; set; }

    [JsonPropertyName("fileName")] public string FileName { get; set; }
}