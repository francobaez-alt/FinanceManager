using Application.Exceptions;
using FluentValidation;
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
        _logger.LogError(exception, "An unhandled exception occurred.");

        var problemDetails = exception switch
        {
            ValidationException ex => CreateValidationProblemDetails(ex, httpContext),

            UserAlreadyExistsException => CreateProblemDetails(
                StatusCodes.Status409Conflict,
                "User already exists",
                exception.Message,
                httpContext),

            NotFoundException => CreateProblemDetails(
                StatusCodes.Status404NotFound,
                "Resource not found",
                exception.Message,
                httpContext),

            BusinessException => CreateProblemDetails(
                StatusCodes.Status400BadRequest,
                "Business rule violation",
                exception.Message,
                httpContext),

            UnauthorizedException => CreateProblemDetails(
                StatusCodes.Status401Unauthorized,
                "Unauthorized access",
                exception.Message,
                httpContext),

            ArgumentException => CreateProblemDetails(
                StatusCodes.Status400BadRequest,
                "Invalid argument",
                exception.Message,
                httpContext),

            UnauthorizedAccessException => CreateProblemDetails(
                StatusCodes.Status401Unauthorized,
                "Unauthorized access",
                exception.Message,
                httpContext),

            _ => CreateProblemDetails(
                StatusCodes.Status500InternalServerError,
                "An unexpected error occurred",
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

    private static ProblemDetails CreateValidationProblemDetails(
    ValidationException exception,
    HttpContext context)
    {
        var errors = exception.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage));

        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation failed",
            Detail = "One or more validation errors occurred.",
            Instance = context.Request.Path
        };

        problem.Extensions["traceId"] = context.TraceIdentifier;
        problem.Extensions["errors"] = errors;

        return problem;
    }
}