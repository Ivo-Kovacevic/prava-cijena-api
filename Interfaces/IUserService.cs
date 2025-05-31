using PravaCijena.Api.Dto.User;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Interfaces;

public interface IUserService
{
    public Task<Result<UserInfoDto>> Register(RegisterDto registerDto);
    public Task<Result<UserInfoDto>> Login(LoginDto loginDto);
}