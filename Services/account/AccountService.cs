using bugdgetwarsapi.Authencation.Abstracts;
using bugdgetwarsapi.DTOs;
using bugdgetwarsapi.Expections;
using bugdgetwarsapi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;

namespace bugdgetwarsapi.Services.account;

public class AccountService : IAccountService
{
    private readonly IAuthTokenProcessor _authTokenProcessor;//dependency injection
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserRepository _userRepository;
    
    
    public AccountService(IAuthTokenProcessor authTokenProcessor, UserManager<ApplicationUser> userManager, IUserRepository userRepository)
    {
        _authTokenProcessor = authTokenProcessor;
        _userManager = userManager;
        _userRepository = userRepository;
    }

    public async Task RegisterAsync(UserRegisterDto registerRequest)
    {
        var userExists = await _userManager.FindByEmailAsync(registerRequest.Email) != null;
        if (userExists)
        {
            throw new UserAlreadyExistsException(email: registerRequest.Email);
        }
        
        var user = ApplicationUser.Create(
            email: registerRequest.Email,
            firstName: registerRequest.FirstName,
            lastName: registerRequest.LastName
        );
        user.PasswordHash= _userManager.PasswordHasher.HashPassword(user, registerRequest.Password);
        var result = await _userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            throw new  RegistrationFailedExpection(result.Errors.Select(e=>e.Description));
                
            
        }
    }
    public async Task  LoginAsync(UserLoginDto loginRequest)
    {
        var user = await _userManager.FindByEmailAsync(loginRequest.Email);
        if (user == null ||!await _userManager.CheckPasswordAsync(user, loginRequest.Password))
        {
            throw new LoginFailedException(email: loginRequest.Email);
        }
        
        // Map ApplicationUser to UserDto expected by the token generator
        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email!,
            UserName = user.UserName!
        };

        var (jwtToken, _) = _authTokenProcessor.GenerateJwtToken(userDto);
        var refreshToken = _authTokenProcessor.GenerateRefreshToken();
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = refreshTokenExpiry;
        
        
        await _userManager.UpdateAsync(user);
        _authTokenProcessor.writeAuthTokenAsHttpOnlyCookie("ACCeSS_TOKEN", jwtToken, DateTime.UtcNow.AddMinutes(15));
        _authTokenProcessor.writeAuthTokenAsHttpOnlyCookie("REFRESH_TOKEN", refreshToken, refreshTokenExpiry);
    }
    
    public async Task RefreshTokenAsync(string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new RefreshTokenExpection("Refresh token is missing.");
        }
        var user = await _userRepository.GetUserByRefreshToken(refreshToken);
        if (user == null)
        {
            throw new RefreshTokenExpection("unable to retrieve user  refresh token.");
        }

        if (user.RefreshTokenExpiry < DateTime.UtcNow)
        {
            throw new RefreshTokenExpection("Refresh token has expired.");
        }

    }

}