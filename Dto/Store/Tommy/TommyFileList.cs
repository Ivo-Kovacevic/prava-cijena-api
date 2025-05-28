using System.Text.Json.Serialization;

namespace PravaCijena.Api.Dto.Store.Tommy;

public class TommyFileList
{
    [JsonPropertyName("hydra:member")] public List<TommyFile> Items { get; set; }
}