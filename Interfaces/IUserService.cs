using PravaCijena.Api.Dto.User;

namespace PravaCijena.Api.Interfaces;

public interface IUserService
{
    public Task<UserInfoDto?> Register(RegisterDto registerDto);
    public Task<UserInfoDto?> Login(LoginDto loginDto);
}