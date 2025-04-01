using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

public class Product : BaseNamedEntity
{
    public ICollection<ProductValue> ProductOptions = [];
    public ICollection<ProductStore> ProductStores = [];
    public required string ImageUrl { get; set; }
    public required Guid CategoryId { get; set; }
    public Category Category { get; set; }

    [NotMapped] public double Similarity { get; set; }
}