using System.Text.RegularExpressions;
using TechHub.Core.Security;

namespace TechHub.Web.Middleware;

/// <summary>
/// Middleware that short-circuits two categories of bad requests before Blazor routing runs:
///
/// 1. **Scanner/attacker probes** — paths that match well-known exploit probe patterns
///    (e.g. /wp-admin, /xmlrpc.php, /.env, /setup.php). These return a bare 404 immediately
///    without touching the Blazor pipeline, generating API calls, or recording telemetry.
///    The OpenTelemetry filter in ServiceDefaults uses the same <see cref="IsProbeRequest"/>
///    check so no Activity span is created for these requests at all.
///
/// 2. **Structurally invalid first segments** — segments containing dots, percent-encoding,
///    or characters that can never match a section, collection, or known page route. This
///    prevents junk URLs like /github-copilot/features.html (after .html stripping falls
///    through) from reaching Blazor and triggering spurious API calls.
///
/// Valid first segments: letters and hyphens only, starting with a letter. Matches every
/// real section name plus "all", "not-found", "about", "error", etc.
/// </summary>
public partial class InvalidRouteSegmentMiddleware
{
    private readonly RequestDelegate _next;

    public InvalidRouteSegmentMiddleware(RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(next);

        _next = next;
    }

    // A valid segment is letters + hyphens, starting with a letter (case-insensitive).
    // Matches the same character set as RouteParameterValidator.IsValidNameSegment.
    [GeneratedRegex(@"^[a-zA-Z][a-zA-Z-]*$", RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex ValidSegmentPattern();

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var path = context.Request.Path.Value;

        if (!string.IsNullOrEmpty(path) && path != "/")
        {
            // Probe check must run before the file-extension early-exit:
            // scanner requests like /.env and /setup.php have file extensions
            // but must be rejected before reaching any downstream middleware.
            if (IsProbeRequest(path))
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            // Static file requests (final segment contains a dot) are passed through
            // after the probe filter. Blazor routes never have file extensions; static
            // assets always do (served by MapStaticAssets / UseStaticFiles downstream).
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

            // If the segment contains anything other than letters and hyphens it can never match a Blazor route — return 404.
            if (!string.IsNullOrEmpty(firstSegment) && !ValidSegmentPattern().IsMatch(firstSegment))
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }
        }

        await _next(context);
    }

    /// <summary>
    /// Returns <c>true</c> if <paramref name="path"/> matches a known scanner or attacker
    /// probe pattern. Called both by <see cref="InvokeAsync"/> (to return 404) and by the
    /// OpenTelemetry request filter in ServiceDefaults (to suppress telemetry entirely).
    /// Probe definitions live in <see cref="ProbeDetector"/> (TechHub.Core).
    /// </summary>
    internal static bool IsProbeRequest(string path)
        => ProbeDetector.IsProbeRequest(path);

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
