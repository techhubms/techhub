using TechHub.Core.Models;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Service for generating RSS feeds from content items
/// </summary>
public interface IRssService
{
    /// <summary>
    /// Generate RSS feed for a specific section
    /// </summary>
    Task<RssChannel> GenerateSectionFeedAsync(
        Section section,
        IReadOnlyList<ContentItem> items,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generate RSS feed for a specific collection
    /// </summary>
    Task<RssChannel> GenerateCollectionFeedAsync(
        string collectionName,
        IReadOnlyList<ContentItem> items,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Convert RSS channel to XML string
    /// </summary>
    string SerializeToXml(RssChannel channel);
}

