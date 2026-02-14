using Microsoft.AspNetCore.Identity;

namespace bugdgetwarsapi.Models;

public class ApplicationUser : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiry { get; set; }


    public static ApplicationUser Create(string email, string firstName, string lastName)
    {
        return new ApplicationUser
        {
            Email = email,
            UserName = email,
            FirstName = firstName,
            LastName = lastName
        };
    }

    public override string ToString()
    {
        return FirstName + " " + LastName;
    }
}