using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PravaCijena.Api.Dto.User;
using PravaCijena.Api.Interfaces;

namespace PravaCijena.Api.Controllers;

[ApiController]
[Route("/api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IHostEnvironment _env;

    public UserController(IUserService userService, IHostEnvironment env)
    {
        _userService = userService;
        _env = env;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var user = await _userService.Register(registerDto);
        if (user == null)
        {
            return BadRequest();
        }

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var user = await _userService.Login(loginDto);
        if (user == null)
        {
            return Unauthorized();
        }

        return Ok(user);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var username = User.FindFirstValue(ClaimTypes.GivenName);

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username))
        {
            return Unauthorized();
        }

        return Ok(new UserInfoDto { Email = email, Username = username });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = _env.IsProduction(), // Inject IWebHostEnvironment
            SameSite = SameSiteMode.None,
            Path = "/"
        };

        if (_env.IsProduction())
        {
            cookieOptions.Domain = ".pravacijena.eu";
        }

        Response.Cookies.Delete("jwtToken", cookieOptions);

        return Ok(new { message = "Logged out successfully" });
    }
}