using System.Diagnostics;

namespace TechHub.Api.Middleware;

/// <summary>
/// Middleware that logs incoming HTTP requests with method, path, status code, and duration.
/// </summary>
public sealed class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<RequestLoggingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var stopwatch = Stopwatch.StartNew();
        var method = context.Request.Method;
        var path = context.Request.Path + context.Request.QueryString;

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            var statusCode = context.Response.StatusCode;
            var duration = stopwatch.ElapsedMilliseconds;

            // Use appropriate log level based on status code
            if (statusCode >= 500)
            {
                _logger.LogError("{Method} {Path} responded {StatusCode} in {Duration}ms", method, path, statusCode, duration);
            }
            else if (statusCode >= 400)
            {
                _logger.LogWarning("{Method} {Path} responded {StatusCode} in {Duration}ms", method, path, statusCode, duration);
            }
            else
            {
                _logger.LogInformation("{Method} {Path} responded {StatusCode} in {Duration}ms", method, path, statusCode, duration);
            }
        }
    }
}
