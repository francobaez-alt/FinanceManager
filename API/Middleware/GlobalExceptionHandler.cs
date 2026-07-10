using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API.Middleware;

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
        _logger.LogError(exception, exception.Message);

        var problemDetails = exception switch
        {
            ArgumentException => CreateProblemDetails(
                StatusCodes.Status400BadRequest,
                "Invalid argument",
                exception.Message,
                httpContext),

            UnauthorizedAccessException => CreateProblemDetails(
                StatusCodes.Status401Unauthorized,
                "Unauthorized",
                exception.Message,
                httpContext),

            _ => CreateProblemDetails(
                StatusCodes.Status500InternalServerError,
                "Internal Server Error",
                exception.Message,
                httpContext)
        };

        httpContext.Response.StatusCode = problemDetails.Status!.Value;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static ProblemDetails CreateProblemDetails(
        int statusCode,
        string title,
        string detail,
        HttpContext context)
    {
        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        problem.Extensions["traceId"] = context.TraceIdentifier;

        return problem;
    }
}