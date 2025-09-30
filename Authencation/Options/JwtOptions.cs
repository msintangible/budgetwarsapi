namespace bugdgetwarsapi.Options;

public class JwtOptions
{ 
    public const string jwtOptionsKey = "JwtOptions";
    
    public string secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpirationTimeMinutes { get; set; }
}