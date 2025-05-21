using System.ComponentModel.DataAnnotations;

namespace PravaCijena.Api.Dto.Product;

public class CreateProductRequestDto
{
    [Required]
    [MinLength(1, ErrorMessage = "Name cannot be empty")]
    [MaxLength(255, ErrorMessage = "Name cannot be longer than 255 characters")]
    public required string Name { get; set; }

    [Required] public required string Barcode { get; set; }

    [Required]
    [Url(ErrorMessage = "Image URL must be in URL format")]
    public required string ImageUrl { get; set; }
}