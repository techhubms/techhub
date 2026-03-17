namespace TechHub.Web.Middleware;

/// <summary>
/// Middleware that redirects requests from subdomain shortcuts (e.g., ghc.xebia.ms)
/// to the configured section path (e.g., /github-copilot).
/// Unknown subdomains are redirected to the primary host for that domain.
/// Shortcuts are configured in appsettings.json under "SubdomainShortcuts".
/// Primary hosts are configured under "PrimaryHosts" (e.g., ["tech.xebia.ms", "tech.hub.ms"]).
/// Passthrough subdomains (configured under "PassthroughSubdomains") are never redirected.
/// </summary>
public class SubdomainRedirectMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Dictionary<string, string> _shortcuts;
    private readonly HashSet<string> _primaryHosts;
    private readonly HashSet<string> _passthroughSubdomains;
    private readonly Dictionary<string, string> _domainToPrimaryHost;
    private readonly ILogger<SubdomainRedirectMiddleware> _logger;

    public SubdomainRedirectMiddleware(
        RequestDelegate next,
        IConfiguration configuration,
        ILogger<SubdomainRedirectMiddleware> logger)
    {
        ArgumentNullException.ThrowIfNull(next);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(logger);

        _next = next;
        _logger = logger;

        // Load shortcuts from configuration at startup (case-insensitive lookup)
        _shortcuts = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var section = configuration.GetSection("SubdomainShortcuts");
        foreach (var child in section.GetChildren())
        {
            if (!string.IsNullOrEmpty(child.Value))
            {
                _shortcuts[child.Key] = child.Value;
            }
        }

        // Load passthrough subdomains (e.g., "staging-tech" — never redirected)
        _passthroughSubdomains = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var passthroughSubdomains = configuration.GetSection("PassthroughSubdomains").Get<string[]>() ?? [];
        foreach (var sub in passthroughSubdomains)
        {
            _passthroughSubdomains.Add(sub);
        }

        // Load primary hosts and build domain-to-primary-host mapping
        // e.g., "tech.xebia.ms" -> domain "xebia.ms" maps to primary "tech.xebia.ms"
        _primaryHosts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        _domainToPrimaryHost = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var primaryHosts = configuration.GetSection("PrimaryHosts").Get<string[]>() ?? [];
        foreach (var primaryHost in primaryHosts)
        {
            _primaryHosts.Add(primaryHost);
            var dotIdx = primaryHost.IndexOf('.', StringComparison.Ordinal);
            if (dotIdx > 0)
            {
                var baseDomain = primaryHost[(dotIdx + 1)..];
                _domainToPrimaryHost[baseDomain] = primaryHost;
            }
        }
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var host = context.Request.Host.Host;

        // Skip processing for primary hosts (tech.xebia.ms, tech.hub.ms, localhost, etc.)
        if (_primaryHosts.Contains(host) || !host.Contains('.', StringComparison.Ordinal))
        {
            await _next(context);
            return;
        }

        // Extract the subdomain and base domain
        var dotIndex = host.IndexOf('.', StringComparison.Ordinal);
        if (dotIndex > 0)
        {
            var subdomain = host[..dotIndex];
            var baseDomain = host[(dotIndex + 1)..];

            // Passthrough subdomains are never redirected (e.g., staging-tech)
            if (_passthroughSubdomains.Contains(subdomain))
            {
                await _next(context);
                return;
            }

            if (_shortcuts.TryGetValue(subdomain, out var sectionPath))
            {
                // Known shortcut: redirect to the section path on the primary host.
                // Must use an absolute URL to change the hostname, otherwise the browser
                // stays on the subdomain and the middleware loops (e.g., ai.hub.ms -> /ai -> /ai/ai -> ...).
                if (_domainToPrimaryHost.TryGetValue(baseDomain, out var targetHost))
                {
                    var originalPath = context.Request.Path.Value == "/" ? "" : context.Request.Path.Value;
                    var redirectUrl = $"https://{targetHost}/{sectionPath}{originalPath}{context.Request.QueryString}";

                    _logger.LogInformation(
                        "Subdomain shortcut redirect: {Host} -> {RedirectUrl}",
                        host,
                        redirectUrl);

                    context.Response.StatusCode = StatusCodes.Status301MovedPermanently;
                    context.Response.Headers.Location = redirectUrl;
                    return;
                }

                // Shortcut matched but base domain has no primary host — pass through
                await _next(context);
                return;
            }

            // Unknown subdomain on a known domain: redirect to primary host
            if (_domainToPrimaryHost.TryGetValue(baseDomain, out var primaryHost))
            {
                var originalPath = context.Request.Path.Value == "/" ? "" : context.Request.Path.Value;
                var redirectUrl = $"https://{primaryHost}{originalPath}{context.Request.QueryString}";

                _logger.LogInformation(
                    "Unknown subdomain redirect: {Host} -> {RedirectUrl}",
                    host,
                    redirectUrl);

                context.Response.StatusCode = StatusCodes.Status301MovedPermanently;
                context.Response.Headers.Location = redirectUrl;
                return;
            }
        }

        await _next(context);
    }
}

/// <summary>
/// Extension methods for registering the SubdomainRedirectMiddleware.
/// </summary>
public static class SubdomainRedirectMiddlewareExtensions
{
    public static IApplicationBuilder UseSubdomainRedirects(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SubdomainRedirectMiddleware>();
    }
}
