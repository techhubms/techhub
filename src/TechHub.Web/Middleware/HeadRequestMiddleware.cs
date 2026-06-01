namespace TechHub.Web.Middleware;

/// <summary>
/// Middleware that handles HEAD requests without engaging the Blazor SSR pipeline.
///
/// <para>
/// Two categories of HEAD request are handled differently:
/// </para>
/// <list type="bullet">
///   <item><b>Extension-less paths (Blazor page routes)</b> — Returned immediately with
///         <c>200 OK</c> and <c>Content-Type: text/html</c>, bypassing Blazor SSR entirely.
///         The SSR pipeline makes an API call for every page render; when the API is slow,
///         bots and crawlers (Slack link previews, SEO scanners) disconnect after their
///         15–25 s timeout and record 499 (client closed request) in telemetry. Since HEAD
///         only requires response headers — not a body — returning 200 immediately is
///         semantically correct and eliminates the 499s with ~23 s duration.</item>
///   <item><b>File-extension paths (static assets, RSS feeds)</b> — Rewrites the method to
///         GET, sets <c>Response.Body = Stream.Null</c> to suppress body output, and calls
///         the next middleware. <c>MapStaticAssets</c> serves the correct headers (ETag,
///         Content-Length, Cache-Control) without sending bytes; <c>MapGet</c> endpoints
///         (RSS feeds, /version, etc.) also work correctly this way.</item>
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

        if (isExtensionless)
        {
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
}

/// <summary>
/// Extension method for registering <see cref="HeadRequestMiddleware"/>.
/// </summary>
public static class HeadRequestMiddlewareExtensions
{
    public static IApplicationBuilder UseHeadRequestHandling(this IApplicationBuilder builder)
        => builder.UseMiddleware<HeadRequestMiddleware>();
}
