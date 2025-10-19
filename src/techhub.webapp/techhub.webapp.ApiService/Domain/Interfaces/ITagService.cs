using techhub.webapp.ApiService.Domain.Models;

namespace techhub.webapp.ApiService.Domain.Interfaces;

/// <summary>
/// Tag service interface for tag processing and normalization
/// </summary>
public interface ITagService
{
    /// <summary>
    /// Normalizes tag from display format to normalized format
    /// </summary>
    /// <param name="display">Display format (e.g., "Visual Studio Code", "C#", "AI++")</param>
    /// <returns>Normalized format (e.g., "visual studio code", "csharp", "aiplusplus")</returns>
    string NormalizeTag(string display);

    /// <summary>
    /// Finds tags that subset-match the given tag
    /// </summary>
    /// <param name="tag">Normalized tag to match</param>
    /// <param name="allTags">All available tags</param>
    /// <returns>Tags that contain the given tag as complete words</returns>
    IEnumerable<Tag> GetRelatedTags(string tag, IEnumerable<Tag> allTags);

    /// <summary>
    /// Builds pre-calculated tag relationships for fast subset matching
    /// </summary>
    /// <param name="items">Content items to analyze</param>
    /// <returns>Dictionary mapping tags to content item indices</returns>
    Dictionary<string, List<int>> BuildTagRelationships(IEnumerable<ContentItem> items);

    /// <summary>
    /// Gets tag usage counts from content items
    /// </summary>
    /// <param name="items">Content items to analyze</param>
    /// <returns>Dictionary of normalized tag to count</returns>
    Dictionary<string, int> GetTagCounts(IEnumerable<ContentItem> items);

    /// <summary>
    /// Gets most frequently used tags
    /// </summary>
    /// <param name="items">Content items to analyze</param>
    /// <param name="count">Number of top tags to return</param>
    /// <returns>Most used tags with counts</returns>
    Task<IEnumerable<Tag>> GetMostUsedTagsAsync(IEnumerable<ContentItem> items, int count);

    /// <summary>
    /// Validates tag format and content
    /// </summary>
    /// <param name="tag">Tag to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    bool ValidateTag(string tag);

    /// <summary>
    /// Checks if one tag is a subset of another using word boundaries
    /// </summary>
    /// <param name="subset">The potential subset tag (e.g., "AI")</param>
    /// <param name="superset">The potential superset tag (e.g., "Azure AI")</param>
    /// <returns>True if subset is contained in superset as complete words</returns>
    bool IsSubsetOf(string subset, string superset);
}
