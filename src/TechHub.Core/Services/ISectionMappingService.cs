namespace TechHub.Core.Services;

/// <summary>
/// Service for mapping between section names, URLs, and display titles.
/// Provides centralized section metadata access.
/// </summary>
public interface ISectionMappingService
{
    /// <summary>
    /// Gets the display title for a section URL.
    /// </summary>
    /// <param name="sectionUrl">Section URL (e.g., "github-copilot", "ai")</param>
    /// <returns>Display title (e.g., "GitHub Copilot", "AI") or the URL if not found</returns>
    string GetSectionTitle(string sectionUrl);

    /// <summary>
    /// Gets the section URL from a section title.
    /// </summary>
    /// <param name="sectionName">Section title (e.g., "GitHub Copilot", "AI")</param>
    /// <returns>Section URL (e.g., "github-copilot", "ai") or normalized input if not found</returns>
    string GetSectionUrl(string sectionName);

    /// <summary>
    /// Gets the display name for a collection.
    /// </summary>
    /// <param name="collectionName">Collection name (e.g., "news", "videos")</param>
    /// <returns>Display name (e.g., "News", "Videos")</returns>
    string GetCollectionDisplayName(string collectionName);
}
