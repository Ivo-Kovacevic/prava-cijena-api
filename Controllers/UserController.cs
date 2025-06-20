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
    private readonly IHostEnvironment _env;
    private readonly IUserService _userService;

    public UserController(IUserService userService, IHostEnvironment env)
    {
        _userService = userService;
        _env = env;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var result = await _userService.Register(registerDto);

        if (!result.IsSuccess)
        {
            if (result.Error == "Email already exists")
            {
                return Conflict(new { error = result.Error });
            }

            return BadRequest(new { error = result.Error });
        }

        return Ok(result.Data);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var result = await _userService.Login(loginDto);

        if (!result.IsSuccess)
        {
            return Unauthorized(new { error = result.Error });
        }

        return Ok(result.Data);
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

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwtToken");

        return Ok(new { message = "Logged out successfully" });
    }
}