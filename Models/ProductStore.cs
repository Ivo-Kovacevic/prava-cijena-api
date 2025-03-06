using System.ComponentModel.DataAnnotations;

namespace api.Models;

public class ProductStore
{
    public Guid Id { get; set; }
    public required Guid ProductId { get; set; }
    public required Guid StoreId { get; set; }
    public Product Product { get; set; }
    public Store Store { get; set; }
    public required string ProductUrl { get; set; }
    public required decimal LatestPrice { get; set; }
    public ICollection<Price> Prices = [];
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}