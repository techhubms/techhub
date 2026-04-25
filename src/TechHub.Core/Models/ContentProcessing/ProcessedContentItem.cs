namespace TechHub.Core.Models.ContentProcessing;

/// <summary>
/// A content item that has been categorized by AI and is ready to be written to the database.
/// </summary>
public sealed class ProcessedContentItem
{
    /// <summary>URL-friendly slug derived from the title and date.</summary>
    public required string Slug { get; init; }

    /// <summary>Cleaned article title (may differ from raw feed title).</summary>
    public required string Title { get; init; }

    /// <summary>Full markdown content of the article.</summary>
    public string Content { get; init; } = string.Empty;

    /// <summary>Short excerpt (≤300 chars) for use in cards and SEO.</summary>
    public required string Excerpt { get; init; }

    /// <summary>Unix timestamp of the publication date (seconds since epoch).</summary>
    public required long DateEpoch { get; init; }

    /// <summary>Target collection name (e.g. "blogs", "news", "videos", "community").</summary>
    public required string CollectionName { get; init; }

    /// <summary>Optional subcollection name for further categorization.</summary>
    public string? SubcollectionName { get; init; }

    /// <summary>Canonical external URL of the original article. Used to detect duplicates.</summary>
    public required string ExternalUrl { get; init; }

    /// <summary>Author name, if available.</summary>
    public string? Author { get; init; }

    /// <summary>Name of the RSS feed this item originated from.</summary>
    public required string FeedName { get; init; }

    /// <summary>Tags assigned by AI categorization (original casing).</summary>
    public IReadOnlyList<string> Tags { get; init; } = [];

    /// <summary>
    /// Sections this item belongs to (e.g. "ai", "azure", "dotnet", "devops",
    /// "github-copilot", "ml", "security").
    /// </summary>
    public IReadOnlyList<string> Sections { get; init; } = [];

    /// <summary>Primary section name — determined by AI as the most relevant section.</summary>
    public required string PrimarySectionName { get; init; }

    /// <summary>SHA-256 hash of the content for change detection on re-processing.</summary>
    public required string ContentHash { get; init; }

    /// <summary>
    /// AI-extracted metadata for roundup generation.
    /// Includes roundup summary, key topics, and relevance level.
    /// </summary>
    public RoundupMetadata? RoundupMetadata { get; init; }

    /// <summary>
    /// Creates a copy of this item with the specified subcollection name.
    /// </summary>
    public ProcessedContentItem WithSubcollectionName(string subcollectionName)
    {
        return new ProcessedContentItem
        {
            Slug = Slug,
            Title = Title,
            Content = Content,
            Excerpt = Excerpt,
            DateEpoch = DateEpoch,
            CollectionName = CollectionName,
            SubcollectionName = subcollectionName,
            ExternalUrl = ExternalUrl,
            Author = Author,
            FeedName = FeedName,
            Tags = Tags,
            Sections = Sections,
            PrimarySectionName = PrimarySectionName,
            ContentHash = ContentHash,
            RoundupMetadata = RoundupMetadata
        };
    }

    /// <summary>
    /// Creates a copy of this item with the specified content.
    /// </summary>
    public ProcessedContentItem WithContent(string content)
    {
        return new ProcessedContentItem
        {
            Slug = Slug,
            Title = Title,
            Content = content,
            Excerpt = Excerpt,
            DateEpoch = DateEpoch,
            CollectionName = CollectionName,
            SubcollectionName = SubcollectionName,
            ExternalUrl = ExternalUrl,
            Author = Author,
            FeedName = FeedName,
            Tags = Tags,
            Sections = Sections,
            PrimarySectionName = PrimarySectionName,
            ContentHash = ContentHash,
            RoundupMetadata = RoundupMetadata
        };
    }

    /// <summary>
    /// Creates a copy of this item with the specified date epoch.
    /// Used to cap future-dated items to the processing date.
    /// </summary>
    public ProcessedContentItem WithDateEpoch(long dateEpoch)
    {
        return new ProcessedContentItem
        {
            Slug = Slug,
            Title = Title,
            Content = Content,
            Excerpt = Excerpt,
            DateEpoch = dateEpoch,
            CollectionName = CollectionName,
            SubcollectionName = SubcollectionName,
            ExternalUrl = ExternalUrl,
            Author = Author,
            FeedName = FeedName,
            Tags = Tags,
            Sections = Sections,
            PrimarySectionName = PrimarySectionName,
            ContentHash = ContentHash,
            RoundupMetadata = RoundupMetadata
        };
    }

    /// <summary>
    /// Creates a copy of this item with the specified tags.
    /// </summary>
    public ProcessedContentItem WithTags(IReadOnlyList<string> tags)
    {
        return new ProcessedContentItem
        {
            Slug = Slug,
            Title = Title,
            Content = Content,
            Excerpt = Excerpt,
            DateEpoch = DateEpoch,
            CollectionName = CollectionName,
            SubcollectionName = SubcollectionName,
            ExternalUrl = ExternalUrl,
            Author = Author,
            FeedName = FeedName,
            Tags = tags,
            Sections = Sections,
            PrimarySectionName = PrimarySectionName,
            ContentHash = ContentHash,
            RoundupMetadata = RoundupMetadata
        };
    }
}
