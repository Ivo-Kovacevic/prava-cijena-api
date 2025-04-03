using System.ComponentModel.DataAnnotations;

namespace PravaCijena.Api.Dto.Category;

public class UpdateCategoryRequestDto
{
    [MinLength(1, ErrorMessage = "Name cannot be empty")]
    [MaxLength(255, ErrorMessage = "Name cannot be longer than 255 characters")]
    public string? Name { get; set; }

    [Url(ErrorMessage = "Image URL must be in URL format")]
    public string? ImageUrl { get; set; }

    public Guid? ParentCategoryId { get; set; }
}