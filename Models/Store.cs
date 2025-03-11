namespace api.Models;

public class Store : BaseEntity
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required string StoreUrl { get; set; }
    public required string ImageUrl { get; set; }
    public ICollection<ProductStore> StoreProducts { get; set; } = [];
}