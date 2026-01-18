namespace bugdgetwarsapi.Expections;

public class UserAlreadyExistsException (string email) : Exception($"User with email {email} already exists.")
{
}
