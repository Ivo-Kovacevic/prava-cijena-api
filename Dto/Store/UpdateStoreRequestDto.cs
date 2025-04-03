using System.ComponentModel.DataAnnotations;

namespace PravaCijena.Api.Dto.Store;

public class UpdateStoreRequestDto
{
    [Required]
    [MinLength(1, ErrorMessage = "Name cannot be empty")]
    [MaxLength(255, ErrorMessage = "Name cannot be longer than 255 characters")]
    public string? Name { get; set; }

    [Required]
    [Url(ErrorMessage = "Store URL must be in URL format")]
    public string? StoreUrl { get; set; }

    [Required]
    [Url(ErrorMessage = "Image URL must be in URL format")]
    public string? ImageUrl { get; set; }
}