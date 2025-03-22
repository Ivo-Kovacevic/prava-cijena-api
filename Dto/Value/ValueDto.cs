using api.Models;

namespace api.Dto.Value;

public class ValueDto : BaseEntity
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required Guid AttributeId { get; set; }
}