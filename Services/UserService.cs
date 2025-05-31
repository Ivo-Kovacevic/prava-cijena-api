using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PravaCijena.Api.Dto.User;
using PravaCijena.Api.Interfaces;
using PravaCijena.Api.Models;

namespace PravaCijena.Api.Services;

public class UserService : IUserService
{
    private readonly IHostEnvironment _env;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly UserManager<User> _userManager;

    public UserService(
        UserManager<User> userManager,
        ITokenService tokenService,
        SignInManager<User> signInManager,
        IHttpContextAccessor httpContextAccessor,
        IHostEnvironment env
    )
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
        _httpContextAccessor = httpContextAccessor;
        _env = env;
    }

    public async Task<Result<UserInfoDto>> Register(RegisterDto registerDto)
    {
        var user = new User { UserName = registerDto.Email, Email = registerDto.Email };

        var createUser = await _userManager.CreateAsync(user, registerDto.Password);
        if (createUser.Succeeded == false)
        {
            var error = string.Join("\n", createUser.Errors.Select(e => e.Description));
            return Result<UserInfoDto>.Fail(error);
        }

        var roleResult = await _userManager.AddToRoleAsync(user, "User");
        if (roleResult.Succeeded == false)
        {
            return Result<UserInfoDto>.Fail("Failed to assign role.");
        }

        var token = _tokenService.CreateToken(user);
        SetJwtCookie(token);

        return Result<UserInfoDto>.Success(new UserInfoDto
        {
            Username = user.UserName,
            Email = user.Email
        });
    }

    public async Task<Result<UserInfoDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
        if (user == null)
        {
            return Result<UserInfoDto>.Fail("Email ili lozinka je netočna.");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (result.Succeeded == false)
        {
            return Result<UserInfoDto>.Fail("Email ili lozinka je netočna.");
        }

        var token = _tokenService.CreateToken(user);
        SetJwtCookie(token);

        return Result<UserInfoDto>.Success(new UserInfoDto
        {
            Username = user.UserName,
            Email = user.Email
        });
    }

    private void SetJwtCookie(string token)
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            throw new InvalidOperationException("HttpContext is not available to set cookie.");
        }

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7),
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/"
        };

        if (_env.IsProduction())
        {
            cookieOptions.Domain = ".pravacijena.eu";
        }

        _httpContextAccessor.HttpContext.Response.Cookies.Append("jwtToken", token, cookieOptions);
    }
}