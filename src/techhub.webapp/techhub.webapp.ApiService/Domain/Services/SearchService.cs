namespace techhub.webapp.ApiService.Domain.Services;

using techhub.webapp.ApiService.Domain.Interfaces;
using techhub.webapp.ApiService.Domain.Models;

/// <summary>
/// Implementation of text search functionality.
/// Provides in-memory search with content indexing.
/// </summary>
public class SearchService : ISearchService
{
    private readonly IContentRepository _repository;
    private readonly Dictionary<Guid, string> _searchIndex = new();
    private readonly object _lock = new();

    public SearchService(IContentRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IEnumerable<ContentItem>> SearchAsync(string query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Enumerable.Empty<ContentItem>();

        var allItems = await _repository.GetAllAsync(cancellationToken);
        var normalizedQuery = query.ToLowerInvariant();

        return allItems.Where(item =>
        {
            lock (_lock)
            {
                if (_searchIndex.TryGetValue(item.Id, out var indexedContent))
                {
                    return indexedContent.Contains(normalizedQuery);
                }
                
                // If not indexed, search directly
                var content = BuildSearchContent(item);
                return content.Contains(normalizedQuery);
            }
        });
    }

    public async Task<IEnumerable<ContentItem>> SearchByTitleAsync(string query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Enumerable.Empty<ContentItem>();

        var allItems = await _repository.GetAllAsync(cancellationToken);
        var normalizedQuery = query.ToLowerInvariant();

        return allItems.Where(item => item.Title.ToLowerInvariant().Contains(normalizedQuery));
    }

    public async Task<IEnumerable<ContentItem>> SearchByDescriptionAsync(string query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Enumerable.Empty<ContentItem>();

        var allItems = await _repository.GetAllAsync(cancellationToken);
        var normalizedQuery = query.ToLowerInvariant();

        return allItems.Where(item => item.Description.ToLowerInvariant().Contains(normalizedQuery));
    }

    public async Task<IEnumerable<ContentItem>> SearchByAuthorAsync(string query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Enumerable.Empty<ContentItem>();

        var allItems = await _repository.GetAllAsync(cancellationToken);
        var normalizedQuery = query.ToLowerInvariant();

        return allItems.Where(item => item.Author.ToLowerInvariant().Contains(normalizedQuery));
    }

    public async Task<IEnumerable<ContentItem>> SearchByTagsAsync(string query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Enumerable.Empty<ContentItem>();

        var allItems = await _repository.GetAllAsync(cancellationToken);
        var normalizedQuery = query.ToLowerInvariant();

        return allItems.Where(item =>
            item.Tags.Any(tag => tag.ToLowerInvariant().Contains(normalizedQuery)) ||
            item.TagsNormalized.Any(tag => tag.ToLowerInvariant().Contains(normalizedQuery)));
    }

    public Task IndexContentAsync(ContentItem item, CancellationToken cancellationToken = default)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        var content = BuildSearchContent(item);
        
        lock (_lock)
        {
            _searchIndex[item.Id] = content;
        }

        return Task.CompletedTask;
    }

    public Task RemoveFromIndexAsync(Guid itemId, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            _searchIndex.Remove(itemId);
        }

        return Task.CompletedTask;
    }

    public async Task RebuildIndexAsync(CancellationToken cancellationToken = default)
    {
        var allItems = await _repository.GetAllAsync(cancellationToken);

        lock (_lock)
        {
            _searchIndex.Clear();
            
            foreach (var item in allItems)
            {
                var content = BuildSearchContent(item);
                _searchIndex[item.Id] = content;
            }
        }
    }

    private static string BuildSearchContent(ContentItem item)
    {
        var parts = new[]
        {
            item.Title,
            item.Description,
            item.Author,
            string.Join(" ", item.Tags),
            string.Join(" ", item.TagsNormalized),
            string.Join(" ", item.Categories)
        };

        return string.Join(" ", parts).ToLowerInvariant();
    }
}
