using Microsoft.AspNetCore.Http.Features;

namespace TechHub.Web.Middleware;

/// <summary>
/// Middleware that rejects HTTP methods that this application never needs to serve,
/// short-circuiting the pipeline before any URL normalization or routing runs.
///
/// <list type="bullet">
///   <item>GET and HEAD are allowed for all paths.</item>
///   <item>WebSocket upgrade requests are always allowed, regardless of method:
///         HTTP/1.1 WebSocket uses GET (already covered above);
///         HTTP/2 WebSocket (RFC 8441) uses CONNECT with <c>:protocol=websocket</c>.
///         Detected via <see cref="IHttpExtendedConnectFeature"/> (set by Kestrel).
///         Blocking these breaks the Blazor SignalR circuit.</item>
///   <item>POST is allowed only for the Blazor SignalR hub (<c>/_blazor/*</c>),
///         Microsoft Identity OIDC flows (<c>/MicrosoftIdentity/*</c>),
///         the OIDC redirect callback (<c>/signin-oidc</c>),
///         and the admin logout endpoint (<c>/admin/logout</c>).</item>
///   <item>All other methods (OPTIONS, PUT, DELETE, PATCH, CONNECT without WebSocket
///         upgrade, TRACE) are rejected with 405 Method Not Allowed.</item>
/// </list>
///
/// Why block OPTIONS: this site has no CORS policy and no cross-origin JavaScript consumers.
/// OPTIONS is exclusively a browser CORS preflight mechanism and is never sent by a real
/// visitor. Any OPTIONS request is a scanner or security probe.
/// </summary>
public class HttpMethodFilterMiddleware
{
    private readonly RequestDelegate _next;

    // Prefixes that accept POST — checked with StartsWithSegments for exact boundary matching.
    private static readonly string[] _postAllowedPrefixes =
    [
        "/_blazor",
        "/MicrosoftIdentity",
    ];

    // Exact paths that accept POST — checked case-insensitively.
    private static readonly string[] _postAllowedExact =
    [
        "/signin-oidc",
        "/admin/logout",
    ];

    public HttpMethodFilterMiddleware(RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(next);
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var method = context.Request.Method;

        if (HttpMethods.IsGet(method) || HttpMethods.IsHead(method))
        {
            await _next(context);
            return;
        }

        // Allow HTTP/2 extended CONNECT for WebSocket upgrade (RFC 8441).
        // HTTP/1.1 WebSocket uses GET + Upgrade header (already handled above).
        // HTTP/2 WebSocket arrives as CONNECT + :protocol=websocket, signalled by Kestrel
        // via IHttpExtendedConnectFeature. Blocking it breaks the Blazor SignalR circuit.
        var extendedConnect = context.Features.Get<IHttpExtendedConnectFeature>();
        if (extendedConnect?.IsExtendedConnect == true &&
            string.Equals(extendedConnect.Protocol, "websocket", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        if (HttpMethods.IsPost(method) && IsPostAllowed(context.Request.Path))
        {
            await _next(context);
            return;
        }

        context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
    }

    internal static bool IsPostAllowed(PathString path)
    {
        if (_postAllowedPrefixes.Any(prefix => path.StartsWithSegments(prefix, StringComparison.OrdinalIgnoreCase)))
        {
            return true;
        }

        if (_postAllowedExact.Any(exact => path.Equals(exact, StringComparison.OrdinalIgnoreCase)))
        {
            return true;
        }

        return false;
    }
}

/// <summary>
/// Extension methods for registering HttpMethodFilterMiddleware.
/// </summary>
public static class HttpMethodFilterMiddlewareExtensions
{
    public static IApplicationBuilder UseHttpMethodFilter(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<HttpMethodFilterMiddleware>();
    }
}
