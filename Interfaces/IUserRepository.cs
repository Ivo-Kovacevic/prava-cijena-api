using PravaCijena.Api.Models;

namespace PravaCijena.Api.Interfaces;

public interface IUserRepository
{
    public Task Create(User user);
}