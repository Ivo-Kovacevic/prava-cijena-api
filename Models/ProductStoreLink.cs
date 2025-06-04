namespace PravaCijena.Api.Models;

public class ProductStoreLink : BaseEntity
{
    public required Guid StoreId { get; set; }
    public required Guid ProductId { get; set; }
    public required string ProductLink { get; set; }

    public Store Store { get; set; }
    public Product Product { get; set; }
}