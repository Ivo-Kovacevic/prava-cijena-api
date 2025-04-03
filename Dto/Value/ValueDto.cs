using PravaCijena.Api.Models;

namespace PravaCijena.Api.Dto.Value;

public class ValueDto : BaseEntity
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required Guid LabelId { get; set; }
}