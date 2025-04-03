using System.ComponentModel.DataAnnotations;

namespace PravaCijena.Api.Dto.Label;

public class CreateLabelRequestDto
{
    [Required]
    [MinLength(1, ErrorMessage = "Name cannot be empty")]
    [MaxLength(255, ErrorMessage = "Name cannot be longer than 255 characters")]
    public required string Name { get; set; }
}