namespace api.Models;

public class Product
{
    public ICollection<ProductStore> ProductStores = [];
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required string ImageUrl { get; set; }
    public required Guid CategoryId { get; set; }
    public Category Category { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}