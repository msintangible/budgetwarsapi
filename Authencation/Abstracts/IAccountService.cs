using bugdgetwarsapi.DTOs;

namespace bugdgetwarsapi.Authencation.Abstracts;

public interface IAccountService
{
    Task RegisterAsync(UserRegisterDto registerRequest);
    Task LoginAsync(UserLoginDto loginRequest);
    Task RefreshTokenAsync(string refreshToken);

}