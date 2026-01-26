using TechHub.Core.DTOs;
using TechHub.Core.Models;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Repository for accessing content items from markdown files.
/// All methods return content sorted by date (DateEpoch) in descending order (newest first).
/// </summary>
public interface IContentRepository
{
    /// <summary>
    /// Initialize the repository by loading all data from disk.
    /// Should be called once at application startup.
    /// Returns the loaded collection for logging purposes.
    /// </summary>
    Task<IReadOnlyList<ContentItem>> InitializeAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all content items across all collections
    /// </summary>
    Task<IReadOnlyList<ContentItem>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get content items filtered by collection name.
    /// Special case: collectionName="all" returns all content across all collections.
    /// </summary>
    /// <param name="collectionName">Collection name to filter by</param>
    /// <param name="includeDraft">If true, include draft items in results. Default is false.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IReadOnlyList<ContentItem>> GetByCollectionAsync(
        string collectionName,
        bool includeDraft = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get content items filtered by section name.
    /// Matches against the Sections property which contains section names like "AI", "GitHub Copilot".
    /// </summary>
    /// <param name="sectionName">Section name to filter by</param>
    /// <param name="includeDraft">If true, include draft items in results. Default is false.</param>
    /// <param name="cancellationToken">Cancellationtoken</param>
    Task<IReadOnlyList<ContentItem>> GetBySectionAsync(
        string sectionName,
        bool includeDraft = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a single content item by slug within a collection
    /// </summary>
    Task<ContentItem?> GetBySlugAsync(
        string collectionName,
        string slug,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Search content items by text query (title, excerpt, tags)
    /// </summary>
    Task<IReadOnlyList<ContentItem>> SearchAsync(
        string query,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all unique tags across all content
    /// </summary>
    Task<IReadOnlyList<string>> GetAllTagsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Filter content items by tags and/or date range with optional section/collection scoping
    /// </summary>
    /// <param name="request">Filter request with tags, date range, and scope</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Filtered content items</returns>
    Task<IReadOnlyList<ContentItem>> FilterAsync(
        FilterRequest request,
        CancellationToken cancellationToken = default);
}
