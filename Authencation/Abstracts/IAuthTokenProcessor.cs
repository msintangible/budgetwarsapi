using bugdgetwarsapi.DTOs;

namespace bugdgetwarsapi.Authencation.Abstracts;

public interface IAuthTokenProcessor
{
    (string jwtToken, DateTime expiresAtUtc) GenerateJwtToken(UserDto user);
    string GenerateRefreshToken();
    void writeAuthTokenAsHttpOnlyCookie(string cookieName, string token, DateTime expiration);
}