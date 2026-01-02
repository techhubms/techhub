namespace TechHub.Core.Models;

/// <summary>
/// Reference to a content collection within a section
/// </summary>
public class CollectionReference
{
    /// <summary>
    /// Display title for the collection (e.g., "Latest News", "Videos")
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Collection identifier matching directory name (e.g., "news", "blogs", "videos")
    /// </summary>
    public required string Collection { get; init; }

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
    private static readonly HashSet<string> ValidCollections = new()
    {
        "news", "videos", "community", "blogs", "roundups"
    };

    /// <summary>
    /// Validates that the collection reference is properly formatted
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Collection))
            throw new ArgumentException("Collection cannot be empty", nameof(Collection));

        if (!ValidCollections.Contains(Collection))
            throw new ArgumentException(
                $"Collection must be one of: {string.Join(", ", ValidCollections)}",
                nameof(Collection));

        if (string.IsNullOrWhiteSpace(Title))
            throw new ArgumentException("Collection title cannot be empty", nameof(Title));

        if (string.IsNullOrWhiteSpace(Url))
            throw new ArgumentException("Collection URL cannot be empty", nameof(Url));

        if (!Url.StartsWith('/'))
            throw new ArgumentException("Collection URL must start with '/'", nameof(Url));
    }
}
