namespace PravaCijena.Api.Models;

public class AutomationResult
{
    public int CreatedProducts { get; set; } = 0;
    public int UpdatedProducts { get; set; } = 0;
    public int CreatedProductStores { get; set; } = 0;
    public int UpdatedProductStores { get; set; } = 0;
    public int CreatedPrices { get; set; } = 0;
    public int UpdatedPrices { get; set; } = 0;
}