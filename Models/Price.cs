namespace api.Models;

public class Price : BaseEntity
{
    public required decimal Amount { get; set; }
    public required Guid ProductStoreId { get; set; }
    public ProductStore ProductStore { get; set; }
}