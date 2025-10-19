namespace techhub.webapp.ApiService.Domain.Models;

/// <summary>
/// Base class for all content types in the Tech Hub.
/// Represents common properties shared by news articles, blog posts, videos, etc.
/// </summary>
public abstract class ContentItem
{
    /// <summary>
    /// Unique identifier for the content item.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Content title (required, max 200 characters).
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Brief summary or description (required, max 500 characters).
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// Content author name (required).
    /// </summary>
    public required string Author { get; set; }

    /// <summary>
    /// Publication date with timezone information.
    /// </summary>
    public required DateTimeOffset PublishedDate { get; set; }

    /// <summary>
    /// Original source URL for external content.
    /// </summary>
    public string? CanonicalUrl { get; set; }

    /// <summary>
    /// Site-relative URL following pattern: /YYYY-MM-DD-title-slug.html
    /// </summary>
    public required string Permalink { get; set; }

    /// <summary>
    /// Section mappings (e.g., ["AI", "GitHub Copilot"]).
    /// </summary>
    public required List<string> Categories { get; set; } = [];

    /// <summary>
    /// Technology keywords in display format (e.g., "Visual Studio Code").
    /// Minimum 3 tags required.
    /// </summary>
    public required List<string> Tags { get; set; } = [];

    /// <summary>
    /// Lowercase, standardized version of Tags for filtering.
    /// Must match Tags array length.
    /// </summary>
    public required List<string> TagsNormalized { get; set; } = [];

    /// <summary>
    /// Viewing mode: "internal" (self-contained) or "external" (links to source).
    /// </summary>
    public required string ViewingMode { get; set; }

    /// <summary>
    /// RSS feed source name (optional).
    /// </summary>
    public string? FeedName { get; set; }

    /// <summary>
    /// RSS feed URL (optional).
    /// </summary>
    public string? FeedUrl { get; set; }

    /// <summary>
    /// Markdown excerpt marker (typically "<!--excerpt_end-->").
    /// </summary>
    public string ExcerptSeparator { get; set; } = "<!--excerpt_end-->";

    /// <summary>
    /// Collection type (news, videos, posts, community, events, roundups).
    /// Set by derived class constructors.
    /// </summary>
    public string CollectionType { get; set; } = string.Empty;

    /// <summary>
    /// Full markdown content.
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// Validates the content item according to business rules.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when validation fails.</exception>
    public virtual void Validate()
    {
        if (string.IsNullOrWhiteSpace(Title))
            throw new ArgumentException("Title cannot be empty", nameof(Title));

        if (Title.Length > 200)
            throw new ArgumentException("Title cannot exceed 200 characters", nameof(Title));

        if (string.IsNullOrWhiteSpace(Description))
            throw new ArgumentException("Description cannot be empty", nameof(Description));

        if (Description.Length > 500)
            throw new ArgumentException("Description cannot exceed 500 characters", nameof(Description));

        if (string.IsNullOrWhiteSpace(Author))
            throw new ArgumentException("Author cannot be empty", nameof(Author));

        if (Categories.Count == 0)
            throw new ArgumentException("At least one category is required", nameof(Categories));

        if (Tags.Count < 3)
            throw new ArgumentException("At least 3 tags are required", nameof(Tags));

        if (Tags.Count != TagsNormalized.Count)
            throw new ArgumentException("Tags and TagsNormalized must have the same count", nameof(TagsNormalized));

        if (string.IsNullOrWhiteSpace(Permalink))
            throw new ArgumentException("Permalink cannot be empty", nameof(Permalink));

        if (!Permalink.StartsWith('/'))
            throw new ArgumentException("Permalink must start with '/'", nameof(Permalink));

        if (ViewingMode != "internal" && ViewingMode != "external")
            throw new ArgumentException("ViewingMode must be 'internal' or 'external'", nameof(ViewingMode));

        if (ViewingMode == "external" && string.IsNullOrWhiteSpace(CanonicalUrl))
            throw new ArgumentException("CanonicalUrl is required for external viewing mode", nameof(CanonicalUrl));

        if (string.IsNullOrWhiteSpace(CollectionType))
            throw new ArgumentException("CollectionType cannot be empty", nameof(CollectionType));
    }
}
