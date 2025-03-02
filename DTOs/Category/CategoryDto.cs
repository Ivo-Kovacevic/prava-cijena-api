namespace api.DTOs.Category;

public class CategoryDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required string ImageUrl { get; set; }
    public int? ParentCategoryId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}