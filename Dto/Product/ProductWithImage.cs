namespace PravaCijena.Api.Dto.Product;

public class ProductWithImage
{
    public required string Name { get; set; }
    public required string ImageUrl { get; set; }
    public string? ProductUrl { get; set; }
}