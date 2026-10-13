using System.ComponentModel.DataAnnotations;

namespace TechHub.Web.MCP;

/// <summary>
/// Settings for the MCP server, bound from the "Mcp" configuration section.
/// </summary>
internal sealed class McpOptions : IValidatableObject
{
    public const string ConfigurationKey = "Mcp";

    /// <summary>
    /// Canonical absolute base URL used for item links in tool results. Configured rather than taken
    /// from the request Host header, which any caller can forge.
    /// </summary>
    [Required]
    public string BaseUrl { get; init; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Origin-only: item links are built as BaseUrl + canonical href, and RoundupContentProvider.ParseUrl
        // requires exactly three path segments, so a base path would produce URLs the server rejects.
        if (!Uri.TryCreate(BaseUrl, UriKind.Absolute, out var uri)
            || (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp)
            || string.IsNullOrEmpty(uri.Host)
            || !string.IsNullOrEmpty(uri.UserInfo)
            || uri.AbsolutePath != "/"
            || BaseUrl.Contains('?', StringComparison.Ordinal)
            || BaseUrl.Contains('#', StringComparison.Ordinal))
        {
            yield return new ValidationResult(
                "BaseUrl must be an origin-only absolute http(s) URL (scheme, host, optional port) with no credentials, path, query or fragment.",
                [nameof(BaseUrl)]);
        }
    }
}
