using System.ComponentModel.DataAnnotations;

namespace api.Dto.Price;

public class CreatePriceRequestDto
{
    [Required] public required decimal Amount { get; set; }
}