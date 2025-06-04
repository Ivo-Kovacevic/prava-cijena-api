namespace PravaCijena.Api.Dto.Store;

public class StoreWithPageInfoDto : StoreDto
{
    public required decimal Price { get; set; }
    public string? ProductUrl { get; set; }
}