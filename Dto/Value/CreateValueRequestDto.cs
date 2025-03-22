using System.ComponentModel.DataAnnotations;

namespace api.Dto.Value;

public class CreateValueRequestDto
{
    [Required]
    [MinLength(1, ErrorMessage = "Name cannot be empty")]
    [MaxLength(255, ErrorMessage = "Name cannot be longer than 255 characters")]
    public required string Name { get; set; }
}