using PravaCijena.Api.Dto.Store;

namespace PravaCijena.Api.Dto.Product;

public class PageProductDto : ProductDto
{
    public List<StoreWithPageInfoDto> Stores { get; set; }
}