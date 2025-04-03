using PravaCijena.Api.Models;

namespace PravaCijena.Api.Dto.Label;

public class LabelDto : BaseEntity
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required Guid CategoryId { get; set; }
}