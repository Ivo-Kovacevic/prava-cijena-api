using PravaCijena.Api.Helpers;

namespace PravaCijena.Api.Models;

public class Label : BaseNamedEntity
{
    public required Guid CategoryId { get; set; }
    public Category Category { get; set; }
    public ICollection<Value> Values { get; set; } = [];
}