namespace PravaCijena.Api.Dto.Product;

public class ExistingProduct
{
    public required Guid ExistingProductId { get; set; }
    public required string ExistingProductName { get; set; }
    public bool IsSameProduct { get; set; }
}