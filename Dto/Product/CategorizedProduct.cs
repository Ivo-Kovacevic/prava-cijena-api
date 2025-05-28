using PravaCijena.Api.Dto.Category;

namespace PravaCijena.Api.Dto.Product;

public class CategorizedProduct
{
    public required string ProductName { get; set; }
    public required BaseCategory Category { get; set; }
}