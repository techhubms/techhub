using TechHub.Core.Validation;

namespace TechHub.Web.Middleware;

/// <summary>
/// Middleware that handles HEAD requests without engaging the Blazor SSR pipeline.
///
/// <para>
/// Two categories of HEAD request are handled differently:
/// </para>
/// <list type="bullet">
///   <item><b>Extension-less Blazor page routes</b> — Returned immediately with
///         <c>200 OK</c> and <c>Content-Type: text/html</c>, bypassing Blazor SSR entirely.
///         The SSR pipeline makes an API call for every page render; when the API is slow,
///         bots and crawlers (Slack link previews, SEO scanners) disconnect after their
///         15–25 s timeout and record 499 (client closed request) in telemetry. Since HEAD
///         only requires response headers — not a body — returning 200 immediately avoids
///         these 499s; note that for unknown routes the response may not mirror a GET (which
///         would 404 after SSR slug validation), so this is a deliberate performance
///         trade-off rather than a strict semantic guarantee.
///         Known minimal API endpoints (<c>/version</c>, <c>/health</c>, <c>/alive</c>)
///         are excluded from this path and fall through to the rewrite path below.</item>
///   <item><b>File-extension paths and minimal API endpoints</b> — Rewrites the method to
///         GET, sets <c>Response.Body = Stream.Null</c> to suppress body output, and calls
///         the next middleware. <c>MapStaticAssets</c> serves the correct headers (ETag,
///         Content-Length, Cache-Control) without sending bytes; <c>MapGet</c> endpoints
///         (RSS feeds, /version, /health, /alive, etc.) also work correctly this way.</item>
/// </list>
///
/// <para>
/// This middleware must be placed immediately before <c>UseRouting()</c> so that the method
/// rewrite (HEAD → GET) is visible to endpoint selection. <c>UseInvalidRouteSegmentFilter</c>
/// must run before this middleware so that invalid paths are already rejected with 404.
/// </para>
/// </summary>
public class HeadRequestMiddleware
{
    // Exact extension-less endpoints that must NOT be short-circuited.
    // They are not Blazor page routes so they must go through the HEAD→GET rewrite
    // path so their handlers can run and return correct headers/status codes.
    private static readonly HashSet<string> _excludedExactPaths = new(StringComparer.OrdinalIgnoreCase)
    {
        "/version",
        "/health",
        "/alive",
        "/signin-oidc",
        "/admin/logout",
    };

    // Prefix-based endpoints that must NOT be short-circuited.
    private static readonly string[] _excludedPrefixes =
    [
        "/_blazor",
        "/MicrosoftIdentity",
    ];

    private readonly RequestDelegate _next;

    public HeadRequestMiddleware(RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(next);
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (!HttpMethods.IsHead(context.Request.Method))
        {
            await _next(context);
            return;
        }

        // Determine whether this is an extension-less path (Blazor page route) or a
        // file-extension path (static asset, RSS feed, etc.) by inspecting the last segment.
        var path = context.Request.Path.Value ?? "/";
        var lastSlash = path.LastIndexOf('/');
        var lastSegment = path.AsSpan()[(lastSlash + 1)..];
        var isExtensionless = !lastSegment.Contains('.');

        if (isExtensionless && !IsShortCircuitExcluded(context.Request.Path))
        {
            // Validate all path segments structurally before short-circuiting.
            // InvalidRouteSegmentMiddleware validates only the first segment; segments
            // beyond that (collection, slug, date-component, etc.) are checked here so
            // that clearly invalid paths return 404 rather than a misleading 200.
            // IsValidSlug covers all structurally valid URL segments (letters, digits,
            // hyphens) and rejects dots, spaces, special characters, and other patterns
            // that can never match a Blazor route.
            var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (segments.Any(segment => !RouteParameterValidator.IsValidSlug(segment)))
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            // Extension-less path → Blazor page route. Short-circuit with 200 immediately.
            // Calling next would trigger Blazor SSR → API call → potential slow response →
            // bot/crawler timeout → 499. HEAD only needs headers, not a body.
            context.Response.StatusCode = StatusCodes.Status200OK;
            context.Response.ContentType = "text/html; charset=utf-8";
            return;
        }

        // File-extension path: rewrite HEAD → GET so endpoint selection (which registers
        // only GET handlers) can match. Stream.Null suppresses body output as HEAD requires.
        context.Request.Method = HttpMethods.Get;
        var originalBody = context.Response.Body;
        context.Response.Body = Stream.Null;
        try
        {
            await _next(context);
        }
        finally
        {
            context.Response.Body = originalBody;
            context.Request.Method = HttpMethods.Head;
        }
    }

    private static bool IsShortCircuitExcluded(PathString path)
    {
        if (_excludedExactPaths.Contains(path.Value ?? string.Empty))
        {
            return true;
        }

        return _excludedPrefixes.Any(prefix => path.StartsWithSegments(prefix, StringComparison.OrdinalIgnoreCase));
    }
}

/// <summary>
/// Extension method for registering <see cref="HeadRequestMiddleware"/>.
/// </summary>
public static class HeadRequestMiddlewareExtensions
{
    public static IApplicationBuilder UseHeadRequestHandling(this IApplicationBuilder builder)
        => builder.UseMiddleware<HeadRequestMiddleware>();
}
