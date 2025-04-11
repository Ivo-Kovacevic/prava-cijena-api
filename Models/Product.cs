namespace PravaCijena.Api.Models;

public class Product : BaseNamedEntity
{
    public ICollection<ProductStore> ProductStores = [];
    public ICollection<ProductValue> ProductValues = [];
    public string? ImageUrl { get; set; }
    public required Guid CategoryId { get; set; }
    public decimal LowestPrice { get; set; }
    public Category Category { get; set; }
}