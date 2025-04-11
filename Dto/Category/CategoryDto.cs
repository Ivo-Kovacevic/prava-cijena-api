using PravaCijena.Api.Models;

namespace PravaCijena.Api.Dto.Category;

public class CategoryDto : BaseEntity
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public string HexColor { get; set; }
    public string? ImageUrl { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public ICollection<CategoryDto> Subcategories { get; set; } = [];
}