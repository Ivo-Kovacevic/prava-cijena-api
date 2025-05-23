using PravaCijena.Api.Models;

namespace PravaCijena.Api.Dto.Store;

public class StoreDto : BaseNamedEntity
{
    public string? BaseUrl { get; set; }
    public string? ProductListXPath { get; set; }
    public required string StoreUrl { get; set; }
    public required string ImageUrl { get; set; }
    public List<StoreCategoryDto> Categories { get; set; } = [];
}