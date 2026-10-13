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
        // [Url] only checks the prefix, so values like "https://" or "ftp://host" would pass.
        if (!Uri.TryCreate(BaseUrl, UriKind.Absolute, out var uri)
            || (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp)
            || string.IsNullOrEmpty(uri.Host)
            || BaseUrl.Contains('?', StringComparison.Ordinal)
            || BaseUrl.Contains('#', StringComparison.Ordinal))
        {
            yield return new ValidationResult(
                "BaseUrl must be an absolute http(s) URL with a host and no query or fragment.",
                [nameof(BaseUrl)]);
        }
    }
}
