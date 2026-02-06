namespace TechHub.Core.Models;

/// <summary>
/// Content item model - used for list views where full content is not needed.
/// For detail views with full content and rendered HTML, use ContentItemDetail.
/// </summary>
public record ContentItem
{
    public string Slug { get; }
    public string Title { get; }
    public string Author { get; }
    public long DateEpoch { get; }
    public string CollectionName { get; }
    public string? SubcollectionName { get; }
    public string FeedName { get; }

    /// <summary>
    /// Primary section name - stored in frontmatter and database.
    /// Computed from section_names using priority order during content sync.
    /// Priority: github-copilot > ai > ml > dotnet > azure > devops > security.
    /// </summary>
    public string PrimarySectionName { get; }

    public IReadOnlyList<string> Tags { get; private set; } = [];

    /// <summary>
    /// List of sections this content belongs to (parsed from database boolean columns).
    /// </summary>
    public IReadOnlyList<string> Sections { get; private set; } = [];

    public string Excerpt { get; }
    public string ExternalUrl { get; }

    /// <summary>
    /// Set the tags for this content item.
    /// </summary>
    public void SetTags(IReadOnlyList<string> tags)
    {
        ArgumentNullException.ThrowIfNull(tags);

        if (tags.Count == 0)
        {
            throw new ArgumentException("At least one tag must be provided", nameof(tags));
        }

        Tags = tags;
    }

    /// <summary>
    /// GitHub Copilot subscription plans this feature is available in (e.g., "Free", "Pro", "Business", "Pro+", "Enterprise")
    /// Used for filtering features by plan tier on the Features page
    /// </summary>
    public IReadOnlyList<string> Plans { get; init; }

    /// <summary>
    /// Indicates whether this feature is available in GitHub Enterprise Server (GHES)
    /// Used for filtering features with GHES support
    /// </summary>
    public bool GhesSupport { get; }

    /// <summary>
    /// Indicates whether this content is a draft (not yet released)
    /// Draft content shows as "Coming Soon" on the Features page
    /// </summary>
    public bool Draft { get; }

    /// <summary>
    /// JSON deserialization constructor - used when deserializing from API responses.
    /// Parameters must match the JSON property names exactly (case-insensitive).
    /// </summary>
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
        string externalUrl,
        bool draft,
        string? subcollectionName,
        IReadOnlyList<string> plans,
        bool ghesSupport)
        : this(slug, title, author, dateEpoch, collectionName, feedName, primarySectionName, excerpt, externalUrl, draft, subcollectionName, plans?.Count > 0 ? string.Join(",", plans) : null, ghesSupport)
    {
        SetTags(tags);
        Sections = sections ?? [];
    }

    /// <summary>
    /// Full constructor - used when creating ContentItem from files or database.
    /// Boolean section parameters are converted to Sections list.
    /// </summary>
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
        bool draft,
        string? subcollectionName,
        string? plans,
        bool ghesSupport,
        string? tagsCsv = null,
        bool isAi = false,
        bool isAzure = false,
        bool isDotNet = false,
        bool isDevOps = false,
        bool isGitHubCopilot = false,
        bool isMl = false,
        bool isSecurity = false)
    {
        // Validate all required properties
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

        // Truncate long excerpts instead of throwing - database may have legacy data
        if (excerpt.Length > 1000)
        {
            excerpt = excerpt[..1000];
        }

        // ExternalUrl is required for all collections except roundups
        if (collectionName != "roundups" && string.IsNullOrWhiteSpace(externalUrl))
        {
            throw new ArgumentException(
                $"ExternalUrl is required for collection '{collectionName}'. Slug: {slug}, Title: {title}",
                nameof(externalUrl));
        }

        // Plans and GhesSupport are ONLY for ghc-features subcollection
        var plansList = string.IsNullOrWhiteSpace(plans)
            ? []
            : plans.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (subcollectionName == "ghc-features")
        {
            if (plansList.Length == 0)
            {
                throw new ArgumentException(
                    $"Plans are required for ghc-features subcollection. Slug: {slug}, Title: {title}",
                    nameof(plans));
            }
        }
        else
        {
            // For non-ghc-features, Plans/GhesSupport should NOT be set
            if (plansList.Length > 0)
            {
                throw new ArgumentException(
                    $"Plans should only be set for ghc-features subcollection. Slug: {slug}, Subcollection: {subcollectionName}",
                    nameof(plans));
            }

            if (ghesSupport)
            {
                throw new ArgumentException(
                    $"GhesSupport should only be set for ghc-features subcollection. Slug: {slug}, Subcollection: {subcollectionName}",
                    nameof(ghesSupport));
            }
        }

        Slug = slug;
        Title = title;
        Author = author;
        DateEpoch = dateEpoch;
        CollectionName = collectionName;
        SubcollectionName = subcollectionName;
        FeedName = feedName;
        PrimarySectionName = primarySectionName;

        // Parse tags from comma-separated string
        Tags = string.IsNullOrWhiteSpace(tagsCsv)
            ? []
            : tagsCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        // Parse sections from boolean columns
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
        ExternalUrl = externalUrl;
        Plans = plansList;
        GhesSupport = ghesSupport;
        Draft = draft;
    }

    /// <summary>
    /// Determines if this item links to an external source (vs linking internally to our site).
    /// News, blogs, and community items redirect to the original source.
    /// Videos and roundups (and custom pages) link internally since we can present them on our site.
    /// </summary>
    public bool LinksExternally() =>
        CollectionName is "news" or "blogs" or "community";

    /// <summary>
    /// Gets the contextual href for this content item.
    /// For items that link externally (news, blogs, community), returns the external URL.
    /// For internal items, when sectionOverride is provided (e.g., "ai"), generates URL like /ai/collection/slug.
    /// Otherwise uses PrimarySectionName: /primary-section/collection/slug.
    /// This allows URLs to be contextual to the current section being browsed.
    /// </summary>
    /// <param name="sectionOverride">Optional section name to use instead of PrimarySectionName</param>
    /// <returns>External URL for external links, or URL path like /section/collection/slug for internal links</returns>
    public string GetHref(string? sectionOverride = null)
    {
        // For items that link externally, always return the external URL
        if (LinksExternally())
        {
            return ExternalUrl;
        }

        // Roundups are only accessible via /all/roundups/ (they don't exist in individual section collections)
        if (CollectionName == "roundups")
        {
            return $"/all/roundups/{Slug}".ToLowerInvariant();
        }

        // For internal links, build contextual URL
        var section = sectionOverride ?? PrimarySectionName;

        // Always use collection name in URL, never subcollection
        // Subcollections are for filtering only (e.g., ghc-features, vscode-updates)
        // Example: ghc-features item has collection="videos", so URL is /section/videos/slug
        return $"/{section}/{CollectionName}/{Slug}".ToLowerInvariant();
    }

    /// <summary>
    /// Gets the link target attribute (opens in new tab for items that link externally)
    /// </summary>
    public string? GetTarget() =>
        LinksExternally() ? "_blank" : null;

    /// <summary>
    /// Gets the link rel attribute (security attributes for items that link externally)
    /// </summary>
    public string? GetRel() =>
        LinksExternally() ? "noopener noreferrer" : null;

    /// <summary>
    /// Gets the aria-label for accessibility
    /// </summary>
    public string GetAriaLabel() =>
        LinksExternally() ? $"{Title} - opens in new tab" : Title;

    /// <summary>
    /// Computed property: Date as DateTime (UTC)
    /// </summary>
    public DateTime DateUtc => DateTimeOffset.FromUnixTimeSeconds(DateEpoch).UtcDateTime;

    /// <summary>
    /// Section priority order (matches the menubar order).
    /// Used to determine which section is "primary" when an item belongs to multiple sections.
    /// </summary>
    private static readonly string[] _sectionPriorityOrder =
    [
        "github-copilot",
        "ai",
        "ml",
        "dotnet",
        "azure",
        "devops",
        "security"
    ];

    /// <summary>
    /// Computes the primary section name from a list of section names using priority rules.
    /// Priority order: github-copilot > ai > ml > dotnet > azure > devops > security.
    /// </summary>
    /// <param name="sectionNames">List of section names (e.g., ["ai", "github-copilot"])</param>
    /// <returns>The primary section name, or "all" if no priority section found</returns>
    public static string ComputePrimarySectionName(IReadOnlyList<string> sectionNames)
    {
        ArgumentNullException.ThrowIfNull(sectionNames);

        if (sectionNames.Count == 0)
        {
            return "all";
        }

        // Find the first section that matches in priority order
        foreach (var prioritySection in _sectionPriorityOrder)
        {
            if (sectionNames.Contains(prioritySection, StringComparer.OrdinalIgnoreCase))
            {
                return prioritySection;
            }
        }

        // No priority match found, return first section (we already checked Count > 0 above)
        return sectionNames[0].ToLowerInvariant();
    }
}
