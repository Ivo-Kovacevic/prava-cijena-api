using api.Models;

namespace api.Dto.ProductStore;

public class ProductStoreDto : BaseEntity
{
    public required Guid ProductId { get; set; }
    public required Guid StoreId { get; set; }
    public string? ProductUrl { get; set; }
    public decimal? LatestPrice { get; set; }
}