using PravaCijena.Api.Dto.Store;

namespace PravaCijena.Api.Dto.ProductStore;

public class PageProductStoreDto : ProductStoreDto
{
    public StoreDto Store { get; set; }
}