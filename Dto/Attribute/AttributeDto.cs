using api.Models;

namespace api.Dto.Attribute;

public class AttributeDto : BaseEntity
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required Guid CategoryId { get; set; }
}