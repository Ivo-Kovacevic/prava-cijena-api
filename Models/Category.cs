using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace api.Models;

public class Category
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required string ImageUrl { get; set; }
    public int? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    public ICollection<Category> Subcategories { get; } = new List<Category>();
    public ICollection<Product> Products { get; } = new List<Product>();
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
