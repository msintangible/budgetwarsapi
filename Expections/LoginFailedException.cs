namespace bugdgetwarsapi.Expections;

public class LoginFailedException(String email) : Exception($"Login failed for user with email {email}.")
{
    
}