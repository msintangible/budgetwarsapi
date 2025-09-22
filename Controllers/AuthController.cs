using bugdgetwarsapi.DTOs;
using bugdgetwarsapi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace bugdgetwarsapi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager; // handles  crud operations for  user
    private readonly SignInManager<ApplicationUser> _signInManager; // focus on handling the authecation process  logging in and logging out  2fa and cookes

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
    
    {
        
        // Normalize email and username
        var normalizedEmail = _userManager.NormalizeEmail(dto.Email);
        var normalizedUserName = _userManager.NormalizeName(dto.Username);

        // Check if email already exists
        var existingEmail = await _userManager.FindByEmailAsync(normalizedEmail);
        if (existingEmail != null)
            return BadRequest("Email is already in use");

        // Check if username already exists
        var existingUserName = await _userManager.FindByNameAsync(normalizedUserName);
        if (existingUserName != null)
            return BadRequest("Username is already taken");

        var user = new ApplicationUser { UserName = dto.Username, Email = dto.Email };
        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok("User registered successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
    {
        var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, isPersistent: false, lockoutOnFailure: false);
        if (!result.Succeeded) return Unauthorized("Invalid credentials");

        return Ok("User logged in successfully");
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok("User logged out");
    }
}
