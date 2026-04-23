namespace TechHub.Core.Models.Admin;

/// <summary>
/// A content item entry for the admin listing page.
/// Lightweight projection from <c>content_items</c> with key fields for browsing and management.
/// </summary>
public sealed class ContentItemListItem
{
    /// <summary>URL-friendly slug (part of the primary key).</summary>
    public required string Slug { get; init; }

    /// <summary>Collection name (part of the primary key).</summary>
    public required string CollectionName { get; init; }

    /// <summary>Article title.</summary>
    public required string Title { get; init; }

    /// <summary>Content author.</summary>
    public required string Author { get; init; }

    /// <summary>Feed that produced this item (e.g. "Azure Blog", "TechHub").</summary>
    public required string FeedName { get; init; }

    /// <summary>External URL or internal path.</summary>
    public required string ExternalUrl { get; init; }

    /// <summary>Primary section name (e.g. "ai", "azure").</summary>
    public required string PrimarySectionName { get; init; }

    /// <summary>Comma-separated list of all section names (e.g. "ai, azure, dotnet").</summary>
    public string? AllSections { get; init; }

    /// <summary>Unix epoch timestamp of the publish date.</summary>
    public long DateEpoch { get; init; }

    /// <summary>When this item was created in the database.</summary>
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>Whether a matching processed_urls record exists.</summary>
    public bool HasProcessedUrl { get; init; }
}
