namespace api.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required string ImageUrl { get; set; }
    public required int CategoryId { get; set; }
}