using PravaCijena.Api.Models;

namespace PravaCijena.Api.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}