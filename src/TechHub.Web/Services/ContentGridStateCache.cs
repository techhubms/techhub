using TechHub.Core.Models;

namespace TechHub.Web.Services;

/// <summary>
/// Snapshot of grid state: loaded items, batch position, and total count.
/// </summary>
public sealed class CachedGridState
{
    public required List<ContentItem> Items { get; init; }
    public required int CurrentBatch { get; init; }
    public required bool HasMoreContent { get; init; }
    public required long TotalCount { get; init; }
}

/// <summary>
/// Circuit-scoped cache that preserves ContentItemsGrid state across enhanced navigations.
/// When users scroll through infinite-scroll content, navigate to a detail page, and press
/// back, the grid can restore all previously loaded items from this cache instead of starting
/// with a single batch. This prevents the browser's scroll restoration from triggering a
/// cascade of batch loads that scrolls to "End of content".
/// </summary>
public sealed class ContentGridStateCache
{
    private readonly Dictionary<string, CachedGridState> _cache = new(StringComparer.Ordinal);

    /// <summary>
    /// Gets the cached grid state for the given key, or null if not cached.
    /// Returns a copy of the items list so the caller can mutate it safely.
    /// </summary>
    public CachedGridState? Get(string key)
    {
        if (!_cache.TryGetValue(key, out var state))
        {
            return null;
        }

        return new CachedGridState
        {
            Items = new List<ContentItem>(state.Items),
            CurrentBatch = state.CurrentBatch,
            HasMoreContent = state.HasMoreContent,
            TotalCount = state.TotalCount
        };
    }

    /// <summary>
    /// Stores or updates grid state for the given key.
    /// Takes a snapshot of the items list to avoid shared mutable state.
    /// </summary>
    public void Set(string key, List<ContentItem> items, int currentBatch, bool hasMoreContent, long totalCount)
    {
        _cache[key] = new CachedGridState
        {
            Items = new List<ContentItem>(items),
            CurrentBatch = currentBatch,
            HasMoreContent = hasMoreContent,
            TotalCount = totalCount
        };
    }
}
