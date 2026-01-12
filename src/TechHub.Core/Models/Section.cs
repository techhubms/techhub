namespace TechHub.Core.Models;

/// <summary>
/// Represents a top-level organizational unit grouping related content by topic
/// </summary>
public class Section
{
    /// <summary>
    /// URL-friendly name (lowercase with hyphens, e.g., "ai", "github-copilot").
    /// Used for: filtering content (matches ContentItem.SectionNames), URL paths, navigation, routing.
    /// NEVER use .ToLower() on Title for filtering - use Name directly.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Display title with proper casing (e.g., "AI", "GitHub Copilot").
    /// Used for: UI display ONLY.
    /// NEVER use this for filtering - use Name instead which matches ContentItem.SectionNames.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Brief description of the section
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// URL path (e.g., "/ai", "/github-copilot")
    /// </summary>
    public required string Url { get; init; }

    /// <summary>
    /// Path to section header background image
    /// </summary>
    public required string BackgroundImage { get; init; }

    /// <summary>
    /// Collections associated with this section
    /// </summary>
    public required IReadOnlyList<CollectionReference> Collections { get; init; }

    /// <summary>
    /// Validates that all required properties are correctly formatted
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentException("Section name cannot be empty", nameof(Name));

        if (!Name.All(c => char.IsLower(c) || c == '-'))
            throw new ArgumentException("Section name must be lowercase with hyphens only", nameof(Name));

        if (string.IsNullOrWhiteSpace(Title))
            throw new ArgumentException("Section title cannot be empty", nameof(Title));

        if (string.IsNullOrWhiteSpace(Url))
            throw new ArgumentException("Section URL cannot be empty", nameof(Url));

        if (!Url.StartsWith('/'))
            throw new ArgumentException("Section URL must start with '/'", nameof(Url));

        if (Collections.Count == 0)
            throw new ArgumentException("Section must have at least one collection", nameof(Collections));
    }
}
