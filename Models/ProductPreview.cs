namespace PravaCijena.Api.Models;

public class ProductPreview
{
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public long? Barcode { get; set; }
    public string? ProductUrl { get; set; }
    public string? ImageUrl { get; set; }
}