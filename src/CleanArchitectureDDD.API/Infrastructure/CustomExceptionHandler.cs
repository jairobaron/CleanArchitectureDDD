using CleanArchitectureDDD.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureDDD.API.Infrastructure;
/// <summary>
/// Class Filter Api Exception
/// </summary>
public class CustomExceptionHandler : IExceptionHandler
{
    private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers;
    /// <summary>
    /// Class constructor
    /// </summary> 
    public CustomExceptionHandler()
    {
        //_exceptionHandlers = [typeof(NotFoundException), HandleNotFoundException];
        // Register known exception types and handlers.
        _exceptionHandlers = new()
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(Application.Common.Exceptions.NotFoundException), HandleNotFoundException },
                { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
                { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
            };
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var exceptionType = exception.GetType();

        if (_exceptionHandlers.ContainsKey(exceptionType))
        {
            await _exceptionHandlers[exceptionType].Invoke(httpContext, exception);
            return true;
        }

        return false;
    }
    /// <summary>
    /// Handle Validation Exception
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="ex"></param>
    private async Task HandleValidationException(HttpContext httpContext, Exception ex)
    {
        var exception = (ValidationException)ex;

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsJsonAsync(new ValidationProblemDetails(exception.Errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        });
    }

    /// <summary>
    /// Handle Not Found Exception
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="ex"></param>
    private async Task HandleNotFoundException(HttpContext httpContext, Exception ex)
    {
        var exception = (Application.Common.Exceptions.NotFoundException)ex;

        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails()
        {
            Status = StatusCodes.Status404NotFound,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "The specified resource was not found.",
            Detail = exception.Message
        });
    }
    /// <summary>
    /// Handle Unauthorized Access Exception
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="ex"></param>
    private async Task HandleUnauthorizedAccessException(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Unauthorized",
            Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
        });
    }
    /// <summary>
    /// Handle Forbidden Access Exception
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="ex"></param>
    private async Task HandleForbiddenAccessException(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status403Forbidden,
            Title = "Forbidden",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
        });
    }
    /// <summary>
    /// Handle Unknown Exception
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="ex"></param>
    private async Task HandleUnknownException(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occurred while processing your request. " + ex.Message,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        });
    }
}
