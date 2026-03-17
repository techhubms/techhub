namespace TechHub.Web.Middleware;

/// <summary>
/// Middleware that strips .html extensions from URL paths and issues a 301 permanent redirect
/// to the canonical URL. This handles legacy URLs (e.g., /github-copilot/features.html)
/// that would otherwise match the generic /{sectionName}/{collectionName} route and
/// cause tag cloud API calls with invalid collection names.
/// </summary>
public class StripHtmlExtensionMiddleware
{
    private readonly RequestDelegate _next;

    public StripHtmlExtensionMiddleware(RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(next);

        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var path = context.Request.Path.Value;

        if (!string.IsNullOrEmpty(path) && path.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
        {
            var cleanPath = path[..^5]; // strip ".html"
            var query = context.Request.QueryString;

            // Redirect to the canonical URL without .html
            var redirectUrl = string.IsNullOrEmpty(cleanPath) ? "/" : cleanPath;
            redirectUrl += query.HasValue ? query.Value : string.Empty;

            context.Response.StatusCode = StatusCodes.Status301MovedPermanently;
            context.Response.Headers.Location = redirectUrl;
            return;
        }

        await _next(context);
    }
}

/// <summary>
/// Extension methods for registering StripHtmlExtensionMiddleware.
/// </summary>
public static class StripHtmlExtensionMiddlewareExtensions
{
    public static IApplicationBuilder UseStripHtmlExtension(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<StripHtmlExtensionMiddleware>();
    }
}
