using PravaCijena.Api.Models;

namespace PravaCijena.Api.Dto.Product;

public class ProductDto : BaseEntity
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required string ImageUrl { get; set; }
    public required Guid CategoryId { get; set; }
    public decimal LowestPrice { get; set; }

}