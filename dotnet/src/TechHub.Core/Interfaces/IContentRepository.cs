using TechHub.Core.Models;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Repository for accessing content items from markdown files
/// </summary>
public interface IContentRepository
{
    /// <summary>
    /// Get all content items across all collections
    /// </summary>
    Task<IReadOnlyList<ContentItem>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get content items filtered by collection
    /// </summary>
    Task<IReadOnlyList<ContentItem>> GetByCollectionAsync(
        string collection,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get content items filtered by category
    /// </summary>
    Task<IReadOnlyList<ContentItem>> GetByCategoryAsync(
        string category,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a single content item by ID within a collection
    /// </summary>
    Task<ContentItem?> GetByIdAsync(
        string collection,
        string id,
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
