using techhub.webapp.ApiService.Domain.Models;

namespace techhub.webapp.ApiService.Domain.Interfaces;

/// <summary>
/// Search service interface for text search functionality
/// </summary>
public interface ISearchService
{
    /// <summary>
    /// Performs full-text search across all content fields
    /// </summary>
    /// <param name="query">Search query string</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IEnumerable<ContentItem>> SearchAsync(string query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches content by title only
    /// </summary>
    Task<IEnumerable<ContentItem>> SearchByTitleAsync(string query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches content by description only
    /// </summary>
    Task<IEnumerable<ContentItem>> SearchByDescriptionAsync(string query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches content by author name
    /// </summary>
    Task<IEnumerable<ContentItem>> SearchByAuthorAsync(string query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches content by tags
    /// </summary>
    Task<IEnumerable<ContentItem>> SearchByTagsAsync(string query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds content to search index
    /// </summary>
    Task IndexContentAsync(ContentItem item, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes content from search index
    /// </summary>
    Task RemoveFromIndexAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Rebuilds entire search index
    /// </summary>
    Task RebuildIndexAsync(CancellationToken cancellationToken = default);
}
