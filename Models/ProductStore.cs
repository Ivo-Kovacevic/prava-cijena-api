namespace PravaCijena.Api.Models;

public class ProductStore : BaseEntity
{
    public ICollection<Price> Prices = [];
    public required Guid ProductId { get; set; }
    public required Guid StoreLocationId { get; set; }
    public Product Product { get; set; }
    public StoreLocation StoreLocation { get; set; }
    public string? ProductUrl { get; set; }
    public decimal LatestPrice { get; set; }
}