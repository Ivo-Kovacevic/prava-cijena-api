using System.ComponentModel.DataAnnotations;

namespace PravaCijena.Api.Dto.Price;

public class CreatePriceRequestDto
{
    [Required] public required decimal Amount { get; set; }
}