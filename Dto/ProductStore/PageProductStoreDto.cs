using PravaCijena.Api.Dto.StoreLocation;

namespace PravaCijena.Api.Dto.ProductStore;

public class PageProductStoreDto : ProductStoreDto
{
    public StoreLocationDto Store { get; set; }
}