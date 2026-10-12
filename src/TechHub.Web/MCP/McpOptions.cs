using System.ComponentModel.DataAnnotations;

namespace TechHub.Web.MCP;

/// <summary>
/// Settings for the MCP server, bound from the "Mcp" configuration section.
/// </summary>
internal sealed class McpOptions
{
    public const string ConfigurationKey = "Mcp";

    /// <summary>
    /// Canonical absolute base URL used for item links in tool results. Configured rather than taken
    /// from the request Host header, which any caller can forge.
    /// </summary>
    [Required]
    [Url]
    public string BaseUrl { get; init; } = string.Empty;
}
