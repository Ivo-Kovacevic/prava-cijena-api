using System.ComponentModel.DataAnnotations.Schema;
using PravaCijena.Api.Dto.ProductStore;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Dto.Product;

public class ProductDto : BaseNamedEntity
{
    [NotMapped] public List<ProductStoreDto>? ProductStores { get; set; } = [];

    public string? ImageUrl { get; set; }
    public required Guid CategoryId { get; set; }
    public decimal LowestPrice { get; set; }
}