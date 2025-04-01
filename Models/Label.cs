using api.Helpers;

namespace api.Models;

public class Label : BaseNamedEntity
{
    public required Guid CategoryId { get; set; }
    public Category Category { get; set; }
    public ICollection<Value> Values { get; set; } = [];
}