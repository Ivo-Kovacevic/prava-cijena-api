namespace PravaCijena.Api.Dto.Store;

public class StoreWithPriceDto : StoreDto
{
    public required decimal Price { get; set; }
}