using PravaCijena.Api.Models;

namespace PravaCijena.Api.Dto.Store;

public class StoreWithCategoriesDto : BaseEntity
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public string? BaseCategoryUrl { get; set; }
    public string? ProductListXPath { get; set; }
    public string? CatalogueListUrl { get; set; }
    public string? CatalogueListXPath { get; set; }
    public string? PageQuery { get; set; }
    public string? LimitQuery { get; set; }
    public required string StoreUrl { get; set; }
    public required string ImageUrl { get; set; }
    public List<StoreCategoryDto> Categories { get; set; } = [];
}