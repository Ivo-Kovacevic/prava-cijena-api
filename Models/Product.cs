namespace api.Models;

public class Product : BaseEntity
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required string ImageUrl { get; set; }
    public required Guid CategoryId { get; set; }
    public Category Category { get; set; }
    public ICollection<ProductStore> ProductStores = [];
    public ICollection<ProductOption> ProductOptions = [];
}