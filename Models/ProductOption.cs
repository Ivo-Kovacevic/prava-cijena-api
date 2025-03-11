namespace api.Models;

public class ProductOption : BaseEntity
{
    public required Guid ProductId { get; set; }
    public required Guid OptionId { get; set; }
    public Product Product { get; set; }
    public Option Option { get; set; }
}