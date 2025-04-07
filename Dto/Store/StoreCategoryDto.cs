namespace PravaCijena.Api.Dto.Store;

public class StoreCategoryDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public Guid? StoreId { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public Guid? EquivalentCategoryId { get; set; }
    public List<StoreCategoryDto> Subcategories { get; set; }
}