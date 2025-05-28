namespace PravaCijena.Api.Dto.Product;

public class ProductComparison
{
    public required string ExistingProduct { get; set; }
    public required string NewProduct { get; set; }
    public bool? IsSameProduct { get; set; }
}