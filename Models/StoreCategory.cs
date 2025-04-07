namespace PravaCijena.Api.Models;

public class StoreCategory : BaseEntity
{
    public string Name { get; set; }
    public Guid? StoreId { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public Guid? EquivalentCategoryId { get; set; }
    public Store? Store { get; set; }
    public List<StoreCategory> Subcategories { get; set; }
    public Category? EquivalentCategory { get; set; }
    public StoreCategory? ParentStoreCategory { get; set; }
}