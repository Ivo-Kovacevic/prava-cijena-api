using System.Text.Json.Serialization;

namespace PravaCijena.Api.Dto.Store.Spar;

public class SparFileList
{
    [JsonPropertyName("files")]
    public List<SparFile> Files { get; set; }
}