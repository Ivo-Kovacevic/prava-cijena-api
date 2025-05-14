using PravaCijena.Api.Models;

namespace PravaCijena.Api.Dto.ProductStore;

public class ProductStoreDto : BaseEntity
{
    public required Guid ProductId { get; set; }
    public required Guid StoreLocationId { get; set; }
    public string? ProductUrl { get; set; }
    public decimal? LatestPrice { get; set; }
}