using PravaCijena.Api.Dto.ProductStore;
using PravaCijena.Api.Dto.Store;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Dto.StoreLocation;

public class StoreLocationDto : BaseEntity
{
    public required string City { get; set; }
    public required string Address { get; set; }
    public required Guid StoreId { get; set; }
    public StoreDto Store { get; set; }
    public ICollection<ProductStoreDto> LocationProducts { get; set; } = [];
}