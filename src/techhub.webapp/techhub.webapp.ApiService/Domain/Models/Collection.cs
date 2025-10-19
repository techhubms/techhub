namespace techhub.webapp.ApiService.Domain.Models;

/// <summary>
/// Represents a content type grouping within a section.
/// Examples: news, posts, videos, community, events, roundups
/// </summary>
public class Collection
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Collection name (e.g., "news", "posts").
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Display title (e.g., "News").
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Collection description for meta tags and display.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// Collection URL (e.g., "/github-copilot/news.html").
    /// </summary>
    public required string Url { get; set; }

    /// <summary>
    /// Whether this is a manually created page (vs auto-generated).
    /// </summary>
    public bool IsCustom { get; set; }

    /// <summary>
    /// Parent section key.
    /// </summary>
    public required string SectionKey { get; set; }

    /// <summary>
    /// Display order within the section (lower numbers first).
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Validates the collection according to business rules.
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentException("Name cannot be empty", nameof(Name));

        var validNames = new[] { "news", "posts", "videos", "community", "events", "roundups" };
        if (!validNames.Contains(Name.ToLower()))
            throw new ArgumentException($"Name must be one of: {string.Join(", ", validNames)}", nameof(Name));

        if (string.IsNullOrWhiteSpace(Title))
            throw new ArgumentException("Title cannot be empty", nameof(Title));

        if (string.IsNullOrWhiteSpace(Url))
            throw new ArgumentException("Url cannot be empty", nameof(Url));

        if (string.IsNullOrWhiteSpace(SectionKey))
            throw new ArgumentException("SectionKey cannot be empty", nameof(SectionKey));
    }
}
