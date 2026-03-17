using System.Text.RegularExpressions;

namespace TechHub.Web.Middleware;

/// <summary>
/// Middleware that 301-redirects URLs containing date-prefixed slugs (YYYY-MM-DD-slug format)
/// to the canonical URL with the date prefix stripped.
/// E.g. /ai/videos/2026-01-12-my-article -> /ai/videos/my-article
/// If the canonical URL doesn't exist, the normal routing pipeline returns a 404.
/// </summary>
public partial class RedirectDatePrefixedSlugsMiddleware
{
    private readonly RequestDelegate _next;

    public RedirectDatePrefixedSlugsMiddleware(RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(next);

        _next = next;
    }

    // Matches the date prefix segment: /YYYY-MM-DD- anywhere in the path.
    // Capture group 1 is everything before the date, group 2 is the remainder after the date.
    [GeneratedRegex(@"^(.*)/\d{4}-\d{2}-\d{2}-(.+)$", RegexOptions.Compiled)]
    private static partial Regex DatePrefixPattern();

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var path = context.Request.Path.Value;

        if (!string.IsNullOrEmpty(path))
        {
            var match = DatePrefixPattern().Match(path);
            if (match.Success)
            {
                // Rebuild path without the date prefix, preserving any query string
                var cleanPath = match.Groups[1].Value + "/" + match.Groups[2].Value;
                var redirectUrl = cleanPath + (context.Request.QueryString.HasValue ? context.Request.QueryString.Value : string.Empty);

                context.Response.StatusCode = StatusCodes.Status301MovedPermanently;
                context.Response.Headers.Location = redirectUrl;
                return;
            }
        }

        await _next(context);
    }
}

/// <summary>
/// Extension methods for registering the RedirectDatePrefixedSlugsMiddleware
/// </summary>
public static class RedirectDatePrefixedSlugsMiddlewareExtensions
{
    public static IApplicationBuilder UseRedirectDatePrefixedSlugs(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RedirectDatePrefixedSlugsMiddleware>();
    }
}
