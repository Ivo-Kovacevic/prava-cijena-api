using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Category;

public class CreateCategoryRequestDto
{
    [Required]
    [MinLength(1, ErrorMessage = "Name cannot be empty")]
    [MaxLength(255, ErrorMessage = "Name cannot be longer than 255 characters")]
    public required string Name { get; set; }
    
    [Url(ErrorMessage = "Image URL must be in URL format")]
    public string? ImageUrl { get; set; }
    
    public Guid? ParentCategoryId { get; set; }
}