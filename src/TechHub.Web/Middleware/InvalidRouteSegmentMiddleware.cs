using System.Text.RegularExpressions;

namespace TechHub.Web.Middleware;

/// <summary>
/// Middleware that short-circuits requests whose first path segment is structurally
/// invalid (e.g. contains dots, percent-encoding, or characters that can never match
/// a section, collection, or known page route). Such requests get a 404 immediately,
/// before Blazor routing runs and before any child components start rendering.
///
/// This prevents junk URLs like /github-copilot/features.html (after .html stripping
/// falls through) or scanner probe paths like /wp-admin from reaching the Blazor
/// pipeline and triggering spurious API calls (e.g. tag cloud requests with garbage
/// collection names).
///
/// Valid first segments: lowercase letters and hyphens only, must start with a letter.
/// This matches every real section name, plus "all", "not-found", "about", "error", etc.
/// </summary>
public partial class InvalidRouteSegmentMiddleware
{
    private readonly RequestDelegate _next;

    public InvalidRouteSegmentMiddleware(RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(next);

        _next = next;
    }

    // A valid segment is lowercase letters + hyphens, starting with a letter.
    // Matches the same character set as RouteParameterValidator.IsValidNameSegment.
    [GeneratedRegex(@"^[a-z][a-z-]*$", RegexOptions.Compiled)]
    private static partial Regex ValidSegmentPattern();

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var path = context.Request.Path.Value;

        if (!string.IsNullOrEmpty(path) && path != "/")
        {
            // Static file requests (final segment contains a dot) are passed through
            // unconditionally. Blazor routes never have file extensions; static assets
            // always do (served by MapStaticAssets / UseStaticFiles downstream).
            if (PathHasFileExtension(path))
            {
                await _next(context);
                return;
            }

            // Extract the first path segment (everything between the first and second slash)
            var span = path.AsSpan().TrimStart('/');
            var slashIdx = span.IndexOf('/');
            var firstSegment = slashIdx >= 0 ? span[..slashIdx].ToString() : span.ToString();

            // Allow Blazor/framework internal paths (/_blazor SignalR hub, /_framework JS, /_content)
            if (firstSegment.StartsWith('_'))
            {
                await _next(context);
                return;
            }

            // Allow Microsoft Identity platform paths (/MicrosoftIdentity/Account/SignIn, /SignOut)
            if (string.Equals(firstSegment, "MicrosoftIdentity", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            // If the segment contains anything other than lowercase letters and hyphens
            // it can never match a Blazor route — return 404.
            if (!string.IsNullOrEmpty(firstSegment) && !ValidSegmentPattern().IsMatch(firstSegment))
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }
        }

        await _next(context);
    }

    private static bool PathHasFileExtension(string path)
    {
        var lastSlash = path.LastIndexOf('/');
        return path.AsSpan()[(lastSlash + 1)..].Contains('.');
    }
}

/// <summary>
/// Extension methods for registering InvalidRouteSegmentMiddleware.
/// </summary>
public static class InvalidRouteSegmentMiddlewareExtensions
{
    public static IApplicationBuilder UseInvalidRouteSegmentFilter(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<InvalidRouteSegmentMiddleware>();
    }
}
