using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Application.Exceptions;

namespace MovieBooking.Api.ExceptionHandlers;

public sealed class GlobalExceptionHandler : IExceptionHandler
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
        (int status, string title, string errorCode) = exception switch
        {
            ValidationException ex => (StatusCodes.Status400BadRequest, "Validation error", ex.ErrorCode),
            NotFoundException ex => (StatusCodes.Status404NotFound, "Not found", ex.ErrorCode),
            ConflictException ex => (StatusCodes.Status409Conflict, "Conflict", ex.ErrorCode),
            BusinessException ex => (StatusCodes.Status400BadRequest, "Business error", ex.ErrorCode),
            _ => (StatusCodes.Status500InternalServerError, "Server error", "SERVER_ERROR")
        };

        if (status >= 500)
            _logger.LogError(exception, "Unhandled exception");
        else
            _logger.LogWarning(exception, "Handled business exception");

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = exception.Message,
            Instance = httpContext.Request.Path
        };

        problem.Extensions["errorCode"] = errorCode;
        problem.Extensions["traceId"] = httpContext.TraceIdentifier;

        httpContext.Response.StatusCode = status;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}
