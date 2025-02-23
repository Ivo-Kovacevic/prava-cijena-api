using Microsoft.EntityFrameworkCore;

namespace api.Models;

[Index(nameof(Slug), IsUnique = true)]
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string ImageUrl { get; set; }
    public ICollection<Product> Products { get; } = new List<Product>();
}
