namespace PravaCijena.Api.Models;

public class ProductComparison
{
    public required string ExistingProduct { get; set; }
    public required string NewProduct { get; set; }
    public bool? IsSameProduct { get; set; }
}