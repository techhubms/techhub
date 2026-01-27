namespace TechHub.Core.Models;

/// <summary>
/// A single facet value with its count.
/// </summary>
public record FacetValue
{
    /// <summary>
    /// Facet value (e.g., "Azure AI", "videos", "ai").
    /// </summary>
    public required string Value { get; init; }

    /// <summary>
    /// Number of items with this facet value in the current scope.
    /// </summary>
    public required long Count { get; init; }
}
