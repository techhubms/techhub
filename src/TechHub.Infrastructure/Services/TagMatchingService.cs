using System.Text.RegularExpressions;
using TechHub.Core.Interfaces;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// Service for intelligent tag matching with subset logic and word boundaries
/// </summary>
public class TagMatchingService : ITagMatchingService
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
    public bool Matches(string searchTag, string itemTag)
    {
        if (string.IsNullOrWhiteSpace(searchTag) || string.IsNullOrWhiteSpace(itemTag))
        {
            return false;
        }

        var normalizedSearch = Normalize(searchTag);
        var normalizedItem = Normalize(itemTag);

        // Exact match
        if (normalizedSearch == normalizedItem)
        {
            return true;
        }

        // Word boundary match (search tag must be complete word in item tag)
        // \b ensures we match word boundaries
        var pattern = $@"\b{Regex.Escape(normalizedSearch)}\b";
        return Regex.IsMatch(normalizedItem, pattern, RegexOptions.IgnoreCase);
    }

    /// <summary>
    /// Check if any selected tags match any of the item's tags
    /// </summary>
    /// <param name="selectedTags">Tags selected for filtering</param>
    /// <param name="itemTags">Tags on content item</param>
    /// <returns>True if any selected tag matches any item tag (OR logic)</returns>
    public bool MatchesAny(IEnumerable<string> selectedTags, IEnumerable<string> itemTags)
    {
        var selectedTagsList = selectedTags?.ToList();
        var itemTagsList = itemTags?.ToList();

        // No filters means show all
        if (selectedTagsList == null || selectedTagsList.Count == 0)
        {
            return true;
        }

        // No tags on item means no match
        if (itemTagsList == null || itemTagsList.Count == 0)
        {
            return false;
        }

        // OR logic: any selected tag matches any item tag
        return selectedTagsList.Any(selectedTag =>
            itemTagsList.Any(itemTag => Matches(selectedTag, itemTag)));
    }

    /// <summary>
    /// Normalize tag for matching (lowercase, trim whitespace)
    /// </summary>
    /// <param name="tag">Tag to normalize</param>
    /// <returns>Normalized tag</returns>
    public string Normalize(string tag)
    {
        ArgumentNullException.ThrowIfNull(tag);

        return tag.Trim().ToLowerInvariant();
    }
}
