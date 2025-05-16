namespace PravaCijena.Api.Models;

public class CategorizedProduct
{
    public required string ProductName { get; set; }
    public required BaseCategory Category { get; set; }
}
