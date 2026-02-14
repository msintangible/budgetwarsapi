using bugdgetwarsapi.Authencation.Abstracts;
using bugdgetwarsapi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace bugdgetwarsapi.Controllers;

[ApiController]
[Route("api/[controller]")] // This makes the route: api/auth
public class AuthController : ControllerBase
{
    private readonly IAccountService _accountService;

    // We inject the service you already created instead of raw Managers
    public AuthController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
    {
        await _accountService.RegisterAsync(dto);
        return Ok(new { message = "User registered successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
    {
        await _accountService.LoginAsync(dto);
        return Ok(new { message = "Login successful" });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        // Extract the refresh token from the cookie manually
        var refreshToken = Request.Cookies["REFRESH_Token"];

        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized("Refresh token missing");

        await _accountService.RefreshTokenAsync(refreshToken);
        return Ok(new { message = "Token refreshed" });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        // To log out a cookie-based JWT system, we simply tell the browser to delete them
        Response.Cookies.Delete("ACCESS_Token");
        Response.Cookies.Delete("REFRESH_Token");
        return Ok(new { message = "Logged out successfully" });
    }
}