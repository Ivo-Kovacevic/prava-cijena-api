namespace PravaCijena.Api.Models.AutomationModels;

public class Category
{
    public Category(string name, List<Category> subcategories = null)
    {
        Name = name;
        Subcategories = subcategories ?? [];
    }

    public string Name { get; set; }
    public List<Category> Subcategories { get; set; }
}