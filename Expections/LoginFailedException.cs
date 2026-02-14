namespace bugdgetwarsapi.Expections;

public class LoginFailedException(string email) : Exception($"Login failed for user with email {email}.")
{
}