namespace PravaCijena.Api.Dto.Product;

public class ProductPreviewDto
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string? ProductUrl { get; set; }
    public string? ImageUrl { get; set; }
}