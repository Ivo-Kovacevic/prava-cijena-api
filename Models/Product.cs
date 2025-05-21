namespace PravaCijena.Api.Models;

public class Product : BaseNamedEntity
{
    public ICollection<ProductStore> ProductStores { get; set; } = [];
    public ICollection<ProductValue> ProductValues { get; set; } = [];
    public string? Brand { get; set; }
    public string? ImageUrl { get; set; }
    public required string Barcode { get; set; }
    public required Guid CategoryId { get; set; }
    public decimal LowestPrice { get; set; }
    public Category Category { get; set; }
}