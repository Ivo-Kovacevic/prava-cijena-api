namespace api.Models;

public class Price
{
    public int Id { get; set; }
    public required decimal Amount { get; set; }
    public required int ProductStoreId { get; set; }
    public ProductStore ProductStore { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}