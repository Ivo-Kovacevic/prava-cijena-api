using PravaCijena.Api.Dto.ProductStore;

namespace PravaCijena.Api.Dto.Product;

public class PageProductDto : ProductDto
{
    public List<PageProductStoreDto> ProductStores { get; set; }
}