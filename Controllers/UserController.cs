using bugdgetwarsapi.DTOs;
using bugdgetwarsapi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bugdgetwarsapi.Controllers;
[ApiController]
[Route("api/user")]

public class UserController: Controller
{
    private readonly  UserManager<ApplicationUser> _userManager;

    public UserController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        var allUsers = await _userManager.Users.ToListAsync();
        var userDtos = allUsers.Select(user => new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName
        }).ToList();
        return Ok(userDtos);
    }
    
    [HttpGet("email/{email}")]
    public async Task<IActionResult>GetUserByEmail([FromRoute] string email)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Email == email);
        
        if (user == null)
            return NotFound("User not found");

        
        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName
        };
        
        return Ok(userDto);
    }
    
    [HttpGet("id/{id}")]
    public async Task<IActionResult>GetUserById([FromRoute] string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        
        if (user == null)
            return NotFound("User not found");

        
        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName
        };
        
        return Ok(userDto);
    }
    
    
}