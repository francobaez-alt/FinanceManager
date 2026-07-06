using Microsoft.AspNetCore.Diagnostics;

namespace API.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unhandled exception occurred.");

            var (statusCode, title) = exception switch
            {
                Application.Exceptions.UserAlreadyExistsException => (StatusCodes.Status409Conflict, "User already exists"),
                Application.Exceptions.NotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
                Application.Exceptions.BusinessException => (StatusCodes.Status400BadRequest, "Business rule violation"),
                ArgumentException => (StatusCodes.Status400BadRequest, "Invalid argument"),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized access"),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };

            var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/problem+json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}
