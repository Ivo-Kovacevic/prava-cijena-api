namespace api.Models;

public class Label : BaseEntity
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required Guid CategoryId { get; set; }
    public Category Category { get; set; }
    public ICollection<Value> Options { get; set; } = [];
}