namespace PravaCijena.Api.Dto.Product;

public class ProductPreview
{
    public required string Name { get; set; }
    public required string Brand { get; set; }
    public required decimal Price { get; set; }
    public required string Barcode { get; set; }
    public Guid? CategoryId { get; set; }
    public string? ProductUrl { get; set; }
    public string? ImageUrl { get; set; }
}