namespace bugdgetwarsapi.Expections;

public class RegistrationFailedExpection(IEnumerable<string> errors)
    : Exception("Registration failed: " + string.Join(", ", errors))
{
}