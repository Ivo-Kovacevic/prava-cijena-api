using System.ComponentModel.DataAnnotations;

namespace PravaCijena.Api.Dto.User;

public class LoginDto
{
    [Required] public required string Email { get; set; }

    [Required] public required string Password { get; set; }
}