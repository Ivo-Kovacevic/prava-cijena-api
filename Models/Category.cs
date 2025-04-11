namespace PravaCijena.Api.Models;

public class Category : BaseNamedEntity
{
    public string? ImageUrl { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public string HexColor { get; set; }
    public Category? ParentCategory { get; set; }
    public ICollection<Category> Subcategories { get; set; } = [];
    public ICollection<Product> Products { get; set; } = [];
    public ICollection<Label> Labels { get; set; } = [];
    public ICollection<StoreCategory> StoreCategories { get; set; } = [];
}