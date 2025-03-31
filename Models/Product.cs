using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

public class Product : BaseEntity
{
    public ICollection<ProductOption> ProductOptions = [];
    public ICollection<ProductStore> ProductStores = [];
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required string ImageUrl { get; set; }
    public required Guid CategoryId { get; set; }
    public Category Category { get; set; }

    [NotMapped] public double Similarity { get; set; }
}