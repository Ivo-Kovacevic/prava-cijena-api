using System.ComponentModel.DataAnnotations.Schema;

namespace PravaCijena.Api.Models;

public class Product : BaseNamedEntity
{
    [NotMapped] public ICollection<ProductStore> ProductStores { get; set; } = [];

    [NotMapped] public ICollection<ProductValue> ProductValues { get; set; } = [];

    public string? Brand { get; set; }
    public string? ImageUrl { get; set; }
    public required string Barcode { get; set; }
    public required Guid CategoryId { get; set; }
    public decimal LowestPrice { get; set; }

    [NotMapped] public Category Category { get; set; }
    [NotMapped] public List<Cart> CartItems { get; set; } = [];
    [NotMapped] public List<SavedProduct> SavedProducts { get; set; } = [];
}