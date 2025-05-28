namespace PravaCijena.Api.Dto.Product;

public class MappedProduct
{
    public required Models.Product ExistingProduct { get; set; }
    public required ProductPreview ProductPreview { get; set; }
}