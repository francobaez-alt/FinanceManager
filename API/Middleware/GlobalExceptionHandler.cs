using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IWebHostEnvironment _environment;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger,
        IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        LogException(httpContext, exception);
        var problemDetails = CreateProblemDetails(
            httpContext,
            exception);

        httpContext.Response.StatusCode =
            problemDetails.Status!.Value;

        httpContext.Response.ContentType =
            "application/problem+json";

        await httpContext.Response
            .WriteAsJsonAsync(
                problemDetails,
                cancellationToken);

        return true;
    }

    private void LogException(
        HttpContext context,
        Exception exception)
    {
        var userId =
            context.User
            .FindFirst(ClaimTypes.NameIdentifier)
            ?.Value;

        var requestData = new
        {
            TraceId = context.TraceIdentifier,

            UserId = userId,

            Method = context.Request.Method,

            Path = context.Request.Path,

            Query = context.Request.QueryString.Value,

            Endpoint = context
                .GetEndpoint()
                ?.DisplayName,

            Ip = context.Connection
                .RemoteIpAddress
                ?.ToString()
        };

        _logger.LogError(
            exception,
            """
            Unhandled exception occurred.

            Request:
            {@RequestData}
            """,
            requestData);
    }

    private ProblemDetails CreateProblemDetails(
        HttpContext context,
        Exception exception)
    {
        var detail =
            _environment.IsDevelopment()
                ? exception.Message
                : "An unexpected error occurred.";

        var problem = new ProblemDetails
        {
            Status =
            StatusCodes.Status500InternalServerError,

            Title =
            "Internal Server Error",

            Detail = detail,

            Instance =
            context.Request.Path
        };

        problem.Extensions["traceId"] =
            context.TraceIdentifier;

        problem.Extensions["timestamp"] =
            DateTime.UtcNow;

        return problem;
    }
}