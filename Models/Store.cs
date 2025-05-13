namespace PravaCijena.Api.Models;

public class Store : BaseNamedEntity
{
    public required string StoreUrl { get; set; }
    public string? PriceListUrl { get; set; }
    public string? BaseCategoryUrl { get; set; }
    public string? CatalogueListUrl { get; set; }
    public string? ProductListXPath { get; set; }
    public string? CatalogueListXPath { get; set; }
    public required string ImageUrl { get; set; }
    public string? PageQuery { get; set; }
    public string? LimitQuery { get; set; }
    public ICollection<ProductStore> StoreProducts { get; set; } = [];
    public List<StoreCategory> Categories { get; set; }
}