namespace PravaCijena.Api.Models;

public class Value : BaseNamedEntity
{
    public required Guid LabelId { get; set; }
    public Label Label { get; set; }
    public ICollection<ProductValue> ValueProducts { get; set; } = [];
}