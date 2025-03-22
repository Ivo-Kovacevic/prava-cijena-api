using System.ComponentModel.DataAnnotations;

namespace api.Dto.Value;

public class UpdateValueRequestDto
{
    [MinLength(1, ErrorMessage = "Name cannot be empty")]
    [MaxLength(255, ErrorMessage = "Name cannot be longer than 255 characters")]
    public string? Name { get; set; }

    public Guid? AttributeId { get; set; }
}