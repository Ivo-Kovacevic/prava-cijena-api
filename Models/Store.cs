namespace PravaCijena.Api.Models;

public class Store : BaseNamedEntity
{
    public required string StoreUrl { get; set; }
    public required string ImageUrl { get; set; }
    public ICollection<ProductStore> StoreProducts { get; set; } = [];
}