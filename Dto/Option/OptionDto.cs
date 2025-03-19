using api.Models;

namespace api.Dto.Option;

public class OptionDto : BaseEntity
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required Guid AttributeId { get; set; }
}