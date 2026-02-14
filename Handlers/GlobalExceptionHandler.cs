using System.Net;
using bugdgetwarsapi.Expections;
using Microsoft.AspNetCore.Diagnostics;

namespace bugdgetwarsapi.Handlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, message) = GetExceptionDetails(exception);
        _logger.LogError(exception, message);
        httpContext.Response.StatusCode = (int)statusCode;
        await httpContext.Response.WriteAsJsonAsync(message, cancellationToken);
        return true;
    }

    private (HttpStatusCode statusCode, string messsage) GetExceptionDetails(Exception exception)
    {
        {
            return exception switch
            {
                UserAlreadyExistsException => (HttpStatusCode.Conflict, exception.Message),
                RefreshTokenExpection => (HttpStatusCode.Unauthorized, exception.Message),
                LoginFailedException => (HttpStatusCode.Unauthorized, exception.Message),
                RegistrationFailedExpection => (HttpStatusCode.BadRequest, exception.Message),
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
            };
        }
    }
}