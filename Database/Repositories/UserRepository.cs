using bugdgetwarsapi.Authencation.Abstracts;
using bugdgetwarsapi.Models;
using Microsoft.EntityFrameworkCore;

namespace bugdgetwarsapi.Database.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApplicationUser?> GetUserByRefreshToken(string refreshToken)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
    }
}