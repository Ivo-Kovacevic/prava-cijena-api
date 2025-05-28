namespace PravaCijena.Api.Dto.Product;

public class ProductMatchingResponse
{
    public required string NewProductWithImage { get; set; }
    public required List<ExistingProduct> ExistingProducts { get; set; }
}