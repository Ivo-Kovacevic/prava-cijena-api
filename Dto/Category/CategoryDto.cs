using api.Models;

namespace api.Dto.Category;

public class CategoryDto : BaseEntity
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public string? ImageUrl { get; set; }
    public Guid? ParentCategoryId { get; set; }
}