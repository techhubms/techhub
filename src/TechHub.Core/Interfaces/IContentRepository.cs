using TechHub.Core.Models;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Repository for accessing content items from markdown files.
/// All methods return content sorted by date (DateEpoch) in descending order (newest first).
/// </summary>
public interface IContentRepository
{
    /// <summary>
    /// Get all content items across all collections
    /// </summary>
    Task<IReadOnlyList<ContentItem>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get content items filtered by collection name
    /// </summary>
    Task<IReadOnlyList<ContentItem>> GetByCollectionAsync(
        string collectionName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get content items filtered by category
    /// </summary>
    Task<IReadOnlyList<ContentItem>> GetByCategoryAsync(
        string category,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a single content item by slug within a collection
    /// </summary>
    Task<ContentItem?> GetBySlugAsync(
        string collectionName,
        string slug,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Search content items by text query (title, description, tags)
    /// </summary>
    Task<IReadOnlyList<ContentItem>> SearchAsync(
        string query,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all unique tags across all content
    /// </summary>
    Task<IReadOnlyList<string>> GetAllTagsAsync(CancellationToken cancellationToken = default);
}
