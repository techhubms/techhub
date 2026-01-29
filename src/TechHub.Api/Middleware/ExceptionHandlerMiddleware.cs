using System.Net;
using System.Text.Json;

namespace TechHub.Api.Middleware;

/// <summary>
/// Global exception handler middleware that catches all unhandled exceptions
/// and returns consistent error responses
/// </summary>
#pragma warning disable CA1812 // Internal class is never instantiated - instantiated by ASP.NET Core middleware pipeline
internal sealed class ExceptionHandlerMiddleware
#pragma warning restore CA1812
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlerMiddleware> logger,
        IHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(next);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(environment);

        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            await _next(context);
        }
#pragma warning disable CA1031 // Do not catch general exception types - intentional in exception handler middleware
        catch (Exception ex)
#pragma warning restore CA1031
        {
            _logger.LogError(ex, "Unhandled exception occurred. Path: {Path}", context.Request.Path);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception switch
        {
            ArgumentException => (int)HttpStatusCode.BadRequest,
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var response = new ErrorResponse
        {
            Status = context.Response.StatusCode,
            Message = exception.Message,
            Detail = _environment.IsDevelopment() ? exception.StackTrace : null,
            Path = context.Request.Path,
            Timestamp = DateTimeOffset.UtcNow
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, _jsonOptions));
    }

    private sealed record ErrorResponse
    {
        public required int Status { get; init; }
        public required string Message { get; init; }
        public string? Detail { get; init; }
        public required string Path { get; init; }
        public required DateTimeOffset Timestamp { get; init; }
    }
}
