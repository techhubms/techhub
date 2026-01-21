namespace TechHub.Core.Models;

/// <summary>
/// Reference to a content collection within a section
/// </summary>
public class CollectionReference
{
    /// <summary>
    /// Display title for the collection with proper casing (e.g., "Latest News", "Videos").
    /// Used for: UI display ONLY.
    /// NEVER use .ToLower() on this for filtering - use Name instead.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// URL-friendly name matching directory name (lowercase, e.g., "news", "blogs", "videos").
    /// Used for: filtering content (matches ContentItem.CollectionName), URL paths, navigation, routing.
    /// NEVER use .ToLower() on Title for filtering - use Name directly.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// URL path for this collection within the section (e.g., "/ai/news")
    /// </summary>
    public required string Url { get; init; }

    /// <summary>
    /// Brief description of what this collection contains
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Whether this collection is manually created (true) or auto-generated (false)
    /// </summary>
    public bool IsCustom { get; init; }

    /// <summary>
    /// Valid collection types from _data/sections.json
    /// </summary>
    private static readonly HashSet<string> _validCollections =
    [
        "news", "videos", "community", "blogs", "roundups"
    ];

    /// <summary>
    /// Validates that the collection reference is properly formatted
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new ArgumentException("Collection name cannot be empty", nameof(Name));
        }

        // Only validate against known collections if this is not a custom collection
        if (!IsCustom && !_validCollections.Contains(Name))
        {
            throw new ArgumentException(
                $"Collection name must be one of: {string.Join(", ", _validCollections)} (or set IsCustom = true)",
                nameof(Name));
        }

        if (string.IsNullOrWhiteSpace(Title))
        {
            throw new ArgumentException("Collection title cannot be empty", nameof(Title));
        }

        if (string.IsNullOrWhiteSpace(Url))
        {
            throw new ArgumentException("Collection URL cannot be empty", nameof(Url));
        }

        if (!Url.StartsWith('/'))
        {
            throw new ArgumentException("Collection URL must start with '/'", nameof(Url));
        }
    }
}
