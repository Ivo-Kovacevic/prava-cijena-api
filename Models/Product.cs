using System.ComponentModel.DataAnnotations.Schema;

namespace PravaCijena.Api.Models;

public class Product : BaseNamedEntity
{
    public ICollection<ProductValue> ProductOptions = [];
    public ICollection<ProductStore> ProductStores = [];
    public required string ImageUrl { get; set; }
    public required Guid CategoryId { get; set; }
    public Category Category { get; set; }
}