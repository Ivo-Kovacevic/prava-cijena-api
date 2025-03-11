namespace api.Models;

public class Option : BaseEntity
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required Guid AttributeId { get; set; }
    public Attribute Attribute { get; set; }
    public ICollection<ProductOption> OptionProducts { get; set; } = [];
}