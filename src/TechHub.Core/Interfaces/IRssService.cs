using TechHub.Core.DTOs;
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
    Task<RssChannelDto> GenerateSectionFeedAsync(
        Section section,
        IReadOnlyList<ContentItem> items,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generate RSS feed for a specific collection
    /// </summary>
    Task<RssChannelDto> GenerateCollectionFeedAsync(
        string collection,
        IReadOnlyList<ContentItem> items,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Convert RSS channel DTO to XML string
    /// </summary>
    string SerializeToXml(RssChannelDto channel);
}
