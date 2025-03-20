using System.ComponentModel.DataAnnotations;

namespace api.Dto.Attribute;

public class UpdateAttributeRequestDto
{
    [MinLength(1, ErrorMessage = "Name cannot be empty")]
    [MaxLength(255, ErrorMessage = "Name cannot be longer than 255 characters")]
    public string? Name { get; set; }

    public Guid? CategoryId { get; set; }
}