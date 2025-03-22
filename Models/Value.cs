namespace api.Models;

public class Value : BaseEntity
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required Guid AttributeId { get; set; }
    public Label Label { get; set; }
    public ICollection<ProductOption> OptionProducts { get; set; } = [];
}