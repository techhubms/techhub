namespace TechHub.Core.Models;

/// <summary>
/// Content item model — used for list views where full content is not needed.
/// For detail views with full content and rendered HTML, use ContentItemDetail.
/// </summary>
public record ContentItem
{
    public string Slug { get; }
    public string Title { get; }
    public string Author { get; }
    public long DateEpoch { get; }
    public string CollectionName { get; }
    public string FeedName { get; }

    /// <summary>
    /// Primary section name — stored in frontmatter and database.
    /// Priority: github-copilot > ai > ml > dotnet > azure > devops > security.
    /// </summary>
    public string PrimarySectionName { get; }

    public IReadOnlyList<string> Tags { get; private set; } = Array.Empty<string>();

    /// <summary>List of sections this content belongs to (parsed from database boolean columns).</summary>
    public IReadOnlyList<string> Sections { get; private set; } = Array.Empty<string>();

    public string Excerpt { get; }
    public string ExternalUrl { get; }

    public void SetTags(IReadOnlyList<string> tags)
    {
        ArgumentNullException.ThrowIfNull(tags);

        if (tags.Count == 0)
        {
            throw new ArgumentException("At least one tag must be provided", nameof(tags));
        }

        Tags = tags;
    }

    /// <summary>JSON deserialization constructor — parameters match JSON property names (case-insensitive).</summary>
    [System.Text.Json.Serialization.JsonConstructor]
    public ContentItem(
        string slug,
        string title,
        string author,
        long dateEpoch,
        string collectionName,
        string feedName,
        string primarySectionName,
        IReadOnlyList<string> tags,
        IReadOnlyList<string>? sections,
        string excerpt,
        string externalUrl)
        : this(slug, title, author, dateEpoch, collectionName, feedName, primarySectionName, excerpt, externalUrl)
    {
        SetTags(tags);
        Sections = sections ?? [];
    }

    /// <summary>Full constructor — used when mapping from database rows.</summary>
    public ContentItem(
        string slug,
        string title,
        string author,
        long dateEpoch,
        string collectionName,
        string feedName,
        string primarySectionName,
        string excerpt,
        string externalUrl,
        string? tagsCsv = null,
        bool isAi = false,
        bool isAzure = false,
        bool isDotNet = false,
        bool isDevOps = false,
        bool isGitHubCopilot = false,
        bool isMl = false,
        bool isSecurity = false)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            throw new ArgumentException("Content slug cannot be empty", nameof(slug));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Content title cannot be empty", nameof(title));
        }

        if (string.IsNullOrWhiteSpace(author))
        {
            throw new ArgumentException("Content author cannot be empty", nameof(author));
        }

        if (dateEpoch <= 0)
        {
            throw new ArgumentException("Date epoch must be a valid Unix timestamp", nameof(dateEpoch));
        }

        if (string.IsNullOrWhiteSpace(collectionName))
        {
            throw new ArgumentException("Collection name cannot be empty", nameof(collectionName));
        }

        if (string.IsNullOrWhiteSpace(feedName))
        {
            throw new ArgumentException("Feed name cannot be empty", nameof(feedName));
        }

        if (string.IsNullOrWhiteSpace(primarySectionName))
        {
            throw new ArgumentException(
                $"Primary section name is required. Slug: {slug}, Title: {title}, Collection: {collectionName}",
                nameof(primarySectionName));
        }

        ArgumentNullException.ThrowIfNull(excerpt);

        if (excerpt.Length > 1000)
        {
            excerpt = excerpt[..1000];
        }

        if (collectionName != "roundups" && string.IsNullOrWhiteSpace(externalUrl))
        {
            throw new ArgumentException(
                $"ExternalUrl is required for collection '{collectionName}'. Slug: {slug}, Title: {title}",
                nameof(externalUrl));
        }

        Slug = slug;
        Title = title;
        Author = author;
        DateEpoch = dateEpoch;
        CollectionName = collectionName;
        FeedName = feedName;
        PrimarySectionName = primarySectionName;

        Tags = string.IsNullOrWhiteSpace(tagsCsv)
            ? []
            : tagsCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var sections = new List<string>();

        if (isAi)
        {
            sections.Add("ai");
        }

        if (isAzure)
        {
            sections.Add("azure");
        }

        if (isDotNet)
        {
            sections.Add("dotnet");
        }

        if (isDevOps)
        {
            sections.Add("devops");
        }

        if (isGitHubCopilot)
        {
            sections.Add("github-copilot");
        }

        if (isMl)
        {
            sections.Add("ml");
        }

        if (isSecurity)
        {
            sections.Add("security");
        }

        Sections = sections;

        Excerpt = excerpt;
        ExternalUrl = externalUrl ?? string.Empty;
    }

    public bool LinksExternally() => CollectionLinksExternally(CollectionName);

    /// <summary>
    /// Gets the contextual href for this content item.
    /// External collections return ExternalUrl. Internal items return /{section}/{collection}/{slug}.
    /// </summary>
    public string GetHref(string? sectionOverride = null)
    {
        var section = sectionOverride ?? PrimarySectionName;
        return BuildHref(CollectionName, Slug, ExternalUrl, section);
    }

    public static bool CollectionLinksExternally(string collectionName)
    {
        ArgumentNullException.ThrowIfNull(collectionName);
        return collectionName.ToLowerInvariant() is "news" or "blogs" or "community";
    }

    public static string BuildHref(string collectionName, string slug, string externalUrl, string? primarySectionName = null)
    {
        ArgumentNullException.ThrowIfNull(collectionName);
        ArgumentNullException.ThrowIfNull(slug);
        ArgumentNullException.ThrowIfNull(externalUrl);

        var normalizedCollection = collectionName.ToLowerInvariant();
        var normalizedSlug = slug.ToLowerInvariant();

        if (CollectionLinksExternally(normalizedCollection))
        {
            return externalUrl;
        }

        if (!string.IsNullOrWhiteSpace(primarySectionName))
        {
            return $"/{primarySectionName.ToLowerInvariant()}/{normalizedCollection}/{normalizedSlug}";
        }

        return externalUrl;
    }

    public string? GetTarget() => LinksExternally() ? "_blank" : null;

    public string? GetRel() => LinksExternally() ? "noopener noreferrer" : null;

    public string GetAriaLabel() => LinksExternally() ? $"{Title} - opens in new tab" : Title;

    public DateTime DateUtc => DateTimeOffset.FromUnixTimeSeconds(DateEpoch).UtcDateTime;
}
