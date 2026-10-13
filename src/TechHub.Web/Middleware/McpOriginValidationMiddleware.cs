using Microsoft.Extensions.Options;
using TechHub.Core.Logging;
using TechHub.Web.MCP;

namespace TechHub.Web.Middleware;

/// <summary>
/// Rejects MCP requests (<c>/mcp</c>) whose <c>Origin</c> header is present but not trusted with 403,
/// as required by the MCP Streamable HTTP transport to prevent DNS-rebinding attacks.
/// Requests without an <c>Origin</c> header (non-browser MCP clients) pass through.
/// Trusted origins: <c>https://</c> + each configured <c>PrimaryHosts</c> entry, the origin of
/// <c>Mcp:BaseUrl</c>, and (Development only) any loopback origin.
/// </summary>
internal sealed class McpOriginValidationMiddleware
{
    private static readonly PathString _mcpPath = new("/mcp");

    private readonly RequestDelegate _next;
    private readonly HashSet<string> _allowedOrigins;
    private readonly bool _allowLoopback;
    private readonly ILogger<McpOriginValidationMiddleware> _logger;

    public McpOriginValidationMiddleware(
        RequestDelegate next,
        IConfiguration configuration,
        IOptions<McpOptions> mcpOptions,
        IHostEnvironment environment,
        ILogger<McpOriginValidationMiddleware> logger)
    {
        ArgumentNullException.ThrowIfNull(next);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(mcpOptions);
        ArgumentNullException.ThrowIfNull(environment);
        ArgumentNullException.ThrowIfNull(logger);

        _next = next;
        _logger = logger;
        _allowLoopback = environment.IsDevelopment();

        _allowedOrigins = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            new Uri(mcpOptions.Value.BaseUrl).GetLeftPart(UriPartial.Authority),
        };

        foreach (var host in configuration.GetSection("PrimaryHosts").Get<string[]>() ?? [])
        {
            _allowedOrigins.Add($"https://{host}");
        }
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (!context.Request.Path.StartsWithSegments(_mcpPath, StringComparison.OrdinalIgnoreCase)
            || !context.Request.Headers.TryGetValue("Origin", out var originValues))
        {
            await _next(context);
            return;
        }

        var origin = originValues.ToString();
        if (IsAllowed(origin))
        {
            await _next(context);
            return;
        }

        _logger.LogInformation("Rejected MCP request with untrusted Origin {Origin}", origin.Sanitize());

        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        await context.Response.WriteAsync("Forbidden: untrusted Origin.", context.RequestAborted);
    }

    private bool IsAllowed(string origin)
    {
        if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri)
            || (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp))
        {
            return false;
        }

        return (_allowLoopback && uri.IsLoopback)
            || _allowedOrigins.Contains(uri.GetLeftPart(UriPartial.Authority));
    }
}

internal static class McpOriginValidationMiddlewareExtensions
{
    public static IApplicationBuilder UseMcpOriginValidation(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<McpOriginValidationMiddleware>();
    }
}
