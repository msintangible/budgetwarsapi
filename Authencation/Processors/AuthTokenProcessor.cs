using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using bugdgetwarsapi.Authencation.Abstracts;
using bugdgetwarsapi.DTOs;
using bugdgetwarsapi.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace bugdgetwarsapi.Authencation.Processors;

public class AuthTokenProcessor : IAuthTokenProcessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtOptions _jwtOptions;


    public AuthTokenProcessor(IOptions<JwtOptions> options, IHttpContextAccessor httpContextAccessor)
    {
        _jwtOptions = options.Value; //acess jwtoptions values    
        _httpContextAccessor = httpContextAccessor;
    }

    public (string jwtToken, DateTime expiresAtUtc) GenerateJwtToken(UserDto user)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.secret));

        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var claim = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.ToString())
        };
        var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationTimeMinutes);
        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claim,
            expires: expires,
            signingCredentials: credentials
        );
        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        return (jwtToken, expires);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public void writeAuthTokenAsHttpOnlyCookie(string cookieName, string token, DateTime expiration)
    {
        _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, token, new CookieOptions
        {
            HttpOnly = true,
            Expires = expiration,
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });
    }
}