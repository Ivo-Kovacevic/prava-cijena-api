namespace PravaCijena.Api.Models;

public class StoreLocation : BaseEntity
{
    public required string City { get; set; }
    public required string Address { get; set; }
    public required Guid StoreId { get; set; }
    public Store Store { get; set; }
    public List<ProductStore> LocationProducts { get; set; } = [];
}