using techhub.webapp.ApiService.Domain.Models;

namespace techhub.webapp.ApiService.Domain.Interfaces;

/// <summary>
/// Repository interface for content item data access
/// </summary>
public interface IContentRepository
{
    /// <summary>
    /// Retrieves a content item by its unique identifier
    /// </summary>
    Task<ContentItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all content items
    /// </summary>
    Task<IEnumerable<ContentItem>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves content items by collection type
    /// </summary>
    /// <param name="collectionType">Collection type (news, posts, videos, community, events, roundups)</param>
    Task<IEnumerable<ContentItem>> GetByCollectionAsync(string collectionType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves content items by section key
    /// </summary>
    /// <param name="sectionKey">Section key (ai, github-copilot, ml, azure, coding, devops, security)</param>
    Task<IEnumerable<ContentItem>> GetBySectionAsync(string sectionKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves content items by categories
    /// </summary>
    /// <param name="categories">List of category names to filter by</param>
    Task<IEnumerable<ContentItem>> GetByCategoriesAsync(List<string> categories, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves content items by tags (supports subset matching)
    /// </summary>
    /// <param name="tags">List of normalized tags to filter by</param>
    Task<IEnumerable<ContentItem>> GetByTagsAsync(List<string> tags, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves content items published since a specific date
    /// </summary>
    Task<IEnumerable<ContentItem>> GetPublishedSinceAsync(DateTimeOffset since, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves content items published between two dates
    /// </summary>
    Task<IEnumerable<ContentItem>> GetPublishedBetweenAsync(DateTimeOffset start, DateTimeOffset end, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new content item
    /// </summary>
    Task AddAsync(ContentItem item, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing content item
    /// </summary>
    Task UpdateAsync(ContentItem item, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a content item by its unique identifier
    /// </summary>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if content with the given canonical URL already exists
    /// </summary>
    Task<bool> ExistsAsync(string canonicalUrl, CancellationToken cancellationToken = default);
}
