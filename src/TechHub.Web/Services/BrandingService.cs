namespace TechHub.Web.Services;

/// <summary>
/// Provides domain-based branding configuration.
/// When accessed via tech.xebia.ms, shows Xebia branding.
/// When accessed via tech.hub.ms or any other domain, shows Tech Hub branding.
/// Override with appsetting "Branding:Force" set to "xebia" or "techhub" for local testing.
/// </summary>
public sealed class BrandingService
{
    private const string XebiaDomain = "tech.xebia.ms";

    public BrandingService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        ArgumentNullException.ThrowIfNull(configuration);

        var forceBranding = configuration["Branding:Force"];
        if (!string.IsNullOrEmpty(forceBranding))
        {
            IsXebiaBranding = string.Equals(forceBranding, "xebia", StringComparison.OrdinalIgnoreCase);
        }
        else
        {
            var host = httpContextAccessor.HttpContext?.Request.Host.Host;
            IsXebiaBranding = string.Equals(host, XebiaDomain, StringComparison.OrdinalIgnoreCase);
        }
    }

    /// <summary>
    /// Whether the current request is served under the Xebia domain.
    /// </summary>
    public bool IsXebiaBranding { get; }

    /// <summary>
    /// The site name to display in the top-left logo area.
    /// </summary>
    public string SiteName => IsXebiaBranding ? "Xebia" : "Tech Hub";

    /// <summary>
    /// Whether to show the GitHub Copilot logo next to the site name.
    /// </summary>
    public bool ShowCopilotLogo => IsXebiaBranding;
}
