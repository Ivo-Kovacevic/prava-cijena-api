using System.ComponentModel.DataAnnotations;

namespace api.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string ImageUrl { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}