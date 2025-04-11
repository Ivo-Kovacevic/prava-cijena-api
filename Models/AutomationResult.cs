namespace PravaCijena.Api.Models;

public class AutomationResult
{
    public required int CreatedProducts { get; set; }
    public required int UpdatedProducts { get; set; }
    public required int CreatedProductStores { get; set; }
    public required int UpdatedProductStores { get; set; }
    public required int CreatedPrices { get; set; }
    public required int UpdatedPrices { get; set; }
}