using bugdgetwarsapi.Options;
using Microsoft.Extensions.Options;

namespace bugdgetwarsapi.Authencation.Processors;

public class AuthTokenProcessor
{
    private readonly JwtOptions _jwtOptions;
    
    public AuthTokenProcessor(IOptions<JwtOptions> options)
    {
       _jwtOptions = options.Value;  //acess jwtoptions values    
    }
    
    
}