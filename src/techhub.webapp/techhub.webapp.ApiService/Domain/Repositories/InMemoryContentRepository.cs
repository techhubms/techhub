namespace techhub.webapp.ApiService.Domain.Repositories;

using techhub.webapp.ApiService.Domain.Interfaces;
using techhub.webapp.ApiService.Domain.Models;

/// <summary>
/// In-memory implementation of IContentRepository for development and testing.
/// Thread-safe implementation using locks for concurrent access.
/// </summary>
public class InMemoryContentRepository : IContentRepository
{
    private readonly Dictionary<Guid, ContentItem> _items = new();
    private readonly object _lock = new();

    public Task<ContentItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            _items.TryGetValue(id, out var item);
            return Task.FromResult(item);
        }
    }

    public Task<IEnumerable<ContentItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            return Task.FromResult(_items.Values.AsEnumerable());
        }
    }

    public Task<IEnumerable<ContentItem>> GetByCollectionAsync(string collectionType, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var items = _items.Values
                .Where(x => x.CollectionType.Equals(collectionType, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(items);
        }
    }

    public Task<IEnumerable<ContentItem>> GetBySectionAsync(string sectionKey, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var items = _items.Values
                .Where(x => x.Categories.Any(c => c.Equals(sectionKey, StringComparison.OrdinalIgnoreCase)));
            return Task.FromResult(items);
        }
    }

    public Task<IEnumerable<ContentItem>> GetByCategoriesAsync(List<string> categories, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var items = _items.Values
                .Where(x => x.Categories.Any(c => categories.Contains(c, StringComparer.OrdinalIgnoreCase)));
            return Task.FromResult(items);
        }
    }

    public Task<IEnumerable<ContentItem>> GetByTagsAsync(List<string> tags, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var tagList = tags.Select(t => t.ToLowerInvariant()).ToList();
            var items = _items.Values
                .Where(x => x.TagsNormalized.Any(t => tagList.Contains(t.ToLowerInvariant())));
            return Task.FromResult(items);
        }
    }

    public Task<IEnumerable<ContentItem>> GetPublishedSinceAsync(DateTimeOffset since, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var items = _items.Values
                .Where(x => x.PublishedDate >= since)
                .OrderByDescending(x => x.PublishedDate)
                .AsEnumerable();
            return Task.FromResult(items);
        }
    }

    public Task<IEnumerable<ContentItem>> GetPublishedBetweenAsync(DateTimeOffset start, DateTimeOffset end, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var items = _items.Values
                .Where(x => x.PublishedDate >= start && x.PublishedDate <= end)
                .OrderByDescending(x => x.PublishedDate)
                .AsEnumerable();
            return Task.FromResult(items);
        }
    }

    public Task AddAsync(ContentItem item, CancellationToken cancellationToken = default)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        item.Validate();

        lock (_lock)
        {
            if (item.Id == Guid.Empty)
                item.Id = Guid.NewGuid();

            if (_items.ContainsKey(item.Id))
                throw new InvalidOperationException($"Item with ID {item.Id} already exists");

            _items[item.Id] = item;
        }

        return Task.CompletedTask;
    }

    public Task UpdateAsync(ContentItem item, CancellationToken cancellationToken = default)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        item.Validate();

        lock (_lock)
        {
            if (!_items.ContainsKey(item.Id))
                throw new InvalidOperationException($"Item with ID {item.Id} does not exist");

            _items[item.Id] = item;
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            _items.Remove(id);
        }

        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string canonicalUrl, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(canonicalUrl))
            return Task.FromResult(false);

        lock (_lock)
        {
            return Task.FromResult(_items.Values.Any(x => x.CanonicalUrl?.Equals(canonicalUrl, StringComparison.OrdinalIgnoreCase) == true));
        }
    }
}
