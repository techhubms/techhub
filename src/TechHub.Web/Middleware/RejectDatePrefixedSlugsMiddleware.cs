using System.Text.RegularExpressions;

namespace TechHub.Web.Middleware;

/// <summary>
/// Middleware that rejects URLs containing date-prefixed slugs (YYYY-MM-DD-slug format).
/// This ensures old URL formats return 404 instead of being processed.
/// </summary>
public partial class RejectDatePrefixedSlugsMiddleware
{
    private readonly RequestDelegate _next;

    public RejectDatePrefixedSlugsMiddleware(RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(next);

        _next = next;
    }

    // Regex to match date prefix pattern: /YYYY-MM-DD-
    // Matches paths like: /ai/videos/2026-01-12-slug or /github-copilot/vscode-updates/2026-01-12-slug
    [GeneratedRegex(@"/\d{4}-\d{2}-\d{2}-", RegexOptions.Compiled)]
    private static partial Regex DatePrefixPattern();

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var path = context.Request.Path.Value;

        // Check if path contains date prefix pattern
        if (!string.IsNullOrEmpty(path) && DatePrefixPattern().IsMatch(path))
        {
            // Rewrite path to /not-found for processing
            context.Request.Path = "/not-found";
        }

        await _next(context);
    }
}

/// <summary>
/// Extension methods for registering the RejectDatePrefixedSlugsMiddleware
/// </summary>
public static class RejectDatePrefixedSlugsMiddlewareExtensions
{
    public static IApplicationBuilder UseRejectDatePrefixedSlugs(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RejectDatePrefixedSlugsMiddleware>();
    }
}
