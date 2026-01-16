namespace TechHub.Core.Interfaces;

/// <summary>
/// Service for intelligent tag matching with subset logic and word boundaries
/// </summary>
public interface ITagMatchingService
{
    /// <summary>
    /// Check if a search tag matches an item tag using subset matching with word boundaries
    /// </summary>
    /// <param name="searchTag">Tag being searched for (e.g., "AI")</param>
    /// <param name="itemTag">Tag on content item (e.g., "Generative AI")</param>
    /// <returns>True if search tag matches item tag</returns>
    /// <remarks>
    /// Examples:
    /// - "AI" matches "AI", "Generative AI", "Azure AI" (word boundary)
    /// - "AI" does NOT match "AIR" (not a word boundary)
    /// - "Visual Studio" matches "Visual Studio", "Visual Studio Code"
    /// </remarks>
    bool Matches(string searchTag, string itemTag);

    /// <summary>
    /// Check if any selected tags match any of the item's tags
    /// </summary>
    /// <param name="selectedTags">Tags selected for filtering</param>
    /// <param name="itemTags">Tags on content item</param>
    /// <returns>True if any selected tag matches any item tag (OR logic)</returns>
    bool MatchesAny(IEnumerable<string> selectedTags, IEnumerable<string> itemTags);

    /// <summary>
    /// Normalize tag for matching (lowercase, trim whitespace)
    /// </summary>
    /// <param name="tag">Tag to normalize</param>
    /// <returns>Normalized tag</returns>
    string Normalize(string tag);
}
