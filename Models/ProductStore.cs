using System.ComponentModel.DataAnnotations;

namespace api.Models;

public class ProductStore
{
    public int Id { get; set; }
    public required int ProductId { get; set; }
    public required int StoreId { get; set; }
    public Product Product { get; set; }
    public Store Store { get; set; }
    public required decimal LatestPrice { get; set; }
    public required string ProductUrl { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}