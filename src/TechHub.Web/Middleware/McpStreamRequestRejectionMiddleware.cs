namespace TechHub.Web.Middleware;

/// <summary>
/// Answers <c>GET /mcp</c> requests that ask for an event stream (Accept: <c>text/event-stream</c>, without
/// <c>text/html</c>) with 405. The stateless MCP server has no server-to-client stream, and without this
/// guard those MCP client requests would fall through to the browser landing page served at the same address.
/// </summary>
internal sealed class McpStreamRequestRejectionMiddleware
{
    private static readonly PathString _mcpPath = new("/mcp");

    private readonly RequestDelegate _next;

    public McpStreamRequestRejectionMiddleware(RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(next);
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (HttpMethods.IsGet(context.Request.Method)
            && context.Request.Path.Equals(_mcpPath, StringComparison.OrdinalIgnoreCase)
            && AcceptsEventStreamOnly(context.Request))
        {
            context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
            context.Response.Headers.Allow = HttpMethods.Post;
            context.Response.ContentType = "text/plain; charset=utf-8";

            // A non-empty body keeps UseStatusCodePagesWithReExecute from replacing it with an HTML page.
            await context.Response.WriteAsync("This MCP server is stateless and has no event stream. Send JSON-RPC requests with POST.", context.RequestAborted);
            return;
        }

        await _next(context);
    }

    private static bool AcceptsEventStreamOnly(HttpRequest request)
    {
        var eventStream = false;
        foreach (var mediaType in request.GetTypedHeaders().Accept)
        {
            if ((mediaType.Quality ?? 1) <= 0)
            {
                continue;
            }

            if (mediaType.MediaType.Equals("text/html", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            eventStream |= mediaType.MediaType.Equals("text/event-stream", StringComparison.OrdinalIgnoreCase);
        }

        return eventStream;
    }
}

internal static class McpStreamRequestRejectionMiddlewareExtensions
{
    public static IApplicationBuilder UseMcpStreamRequestRejection(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<McpStreamRequestRejectionMiddleware>();
    }
}
