using bugdgetwarsapi.Models;

namespace bugdgetwarsapi.Authencation.Abstracts;

public interface IUserRepository
{
    Task<ApplicationUser?> GetUserByRefreshToken(string refreshToken);
}