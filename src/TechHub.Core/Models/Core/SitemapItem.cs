namespace TechHub.Core.Models;

/// <summary>
/// Lightweight model for sitemap generation.
/// Contains only the fields needed to build a sitemap URL entry.
/// </summary>
public record SitemapItem
{
    public required string Slug { get; init; }
    public required string PrimarySectionName { get; init; }
    public required string CollectionName { get; init; }

    /// <summary>
    /// Unix epoch seconds — used as the lastmod date in the sitemap.
    /// </summary>
    public required long DateEpoch { get; init; }
}
