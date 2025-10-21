namespace techhub.webapp.ApiService.Domain.Services;

using techhub.webapp.ApiService.Domain.Interfaces;
using techhub.webapp.ApiService.Domain.Models;

/// <summary>
/// Implementation of content filtering logic.
/// Implements "20 + Same-Day" rule, 7-day recency filter, and tag-based filtering.
/// </summary>
public class FilteringService : IFilteringService
{
    public IEnumerable<ContentItem> ApplyDateFilter(IEnumerable<ContentItem> items, int days)
    {
        if (days <= 0)
            throw new ArgumentException("Days must be positive", nameof(days));

        var cutoffDate = DateTimeOffset.UtcNow.AddDays(-days);
        return items.Where(x => x.PublishedDate >= cutoffDate);
    }

    public IEnumerable<ContentItem> ApplyTagFilter(IEnumerable<ContentItem> items, List<string> tags)
    {
        var normalizedTags = tags.Select(t => Tag.NormalizeTag(t)).ToList();
        
        return items.Where(item =>
        {
            // All selected tags must match (AND logic with subset matching)
            return normalizedTags.All(selectedTag =>
                item.TagsNormalized.Any(itemTag =>
                {
                    var normalizedItemTag = Tag.NormalizeTag(itemTag);
                    // Subset matching: check if selected tag is contained as complete words
                    return Tag.IsSubsetOf(selectedTag, normalizedItemTag);
                }));
        });
    }

    public IEnumerable<ContentItem> ApplySectionFilter(IEnumerable<ContentItem> items, string sectionKey)
    {
        return items.Where(x => x.Categories.Any(c => c.Equals(sectionKey, StringComparison.OrdinalIgnoreCase)));
    }

    public IEnumerable<ContentItem> ApplyCollectionFilter(IEnumerable<ContentItem> items, string collectionType)
    {
        return items.Where(x => x.CollectionType.Equals(collectionType, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<ContentItem> ApplyTextSearch(IEnumerable<ContentItem> items, string searchQuery)
    {
        if (string.IsNullOrWhiteSpace(searchQuery))
            return items;

        var query = searchQuery.ToLowerInvariant();
        
        return items.Where(item =>
        {
            // Search in title, description, author, and tags
            var content = $"{item.Title} {item.Description} {item.Author} {string.Join(" ", item.Tags)}".ToLowerInvariant();
            return content.Contains(query);
        });
    }

    public IEnumerable<ContentItem> ApplyContentLimiting(IEnumerable<ContentItem> items, int limit = 20)
    {
        if (limit <= 0)
            throw new ArgumentException("Limit must be positive", nameof(limit));

        var sorted = items.OrderByDescending(x => x.PublishedDate).ToList();
        
        if (sorted.Count <= limit)
            return sorted;

        // Get the date of the Nth item
        var limitItem = sorted[limit - 1];
        var limitDate = limitItem.PublishedDate.Date;

        // Include all items from the same day as the limit item
        return sorted.Take(limit)
            .Concat(sorted.Skip(limit).TakeWhile(x => x.PublishedDate.Date == limitDate));
    }

    public IEnumerable<ContentItem> ApplyRecencyFilter(IEnumerable<ContentItem> items, int days = 7)
    {
        var cutoffDate = DateTimeOffset.UtcNow.AddDays(-days);
        return items.Where(x => x.PublishedDate >= cutoffDate);
    }

    public List<int> GetEligibleDateFilters(IEnumerable<ContentItem> items)
    {
        // Standard date filter options from config
        var availableFilters = new[] { 0, 2, 3, 4, 5, 6, 7, 14, 30, 60, 90, 180, 365 };
        
        var sortedItems = items.OrderByDescending(x => x.PublishedDate).ToList();
        var eligible = new List<int>();
        var previousCount = 0;

        foreach (var days in availableFilters.OrderBy(x => x))
        {
            var cutoffDate = DateTimeOffset.UtcNow.AddDays(-days);
            var count = sortedItems.Count(x => x.PublishedDate >= cutoffDate);

            if (count == 0)
                continue;

            // Apply 50% growth rule
            if (previousCount == 0 || count >= Math.Max(previousCount * 1.5, previousCount + 1))
            {
                eligible.Add(days);
                previousCount = count;
            }
        }

        // Remove last filter if it includes ≥85% of all posts
        if (eligible.Any())
        {
            var lastFilter = eligible.Last();
            var lastCutoff = DateTimeOffset.UtcNow.AddDays(-lastFilter);
            var lastCount = sortedItems.Count(x => x.PublishedDate >= lastCutoff);
            var totalCount = sortedItems.Count;

            if (totalCount > 0 && (double)lastCount / totalCount >= 0.85)
                eligible.RemoveAt(eligible.Count - 1);
        }

        return eligible;
    }

    public int CountByTags(IEnumerable<ContentItem> items, List<string> tags)
    {
        return ApplyTagFilter(items, tags).Count();
    }

    public int CountByDateFilter(IEnumerable<ContentItem> items, int days)
    {
        var cutoffDate = DateTimeOffset.UtcNow.AddDays(-days);
        return items.Count(x => x.PublishedDate >= cutoffDate);
    }

    public Dictionary<string, List<int>> BuildTagRelationships(IEnumerable<ContentItem> items)
    {
        var relationships = new Dictionary<string, List<int>>(StringComparer.OrdinalIgnoreCase);
        var itemsList = items.ToList();

        for (int i = 0; i < itemsList.Count; i++)
        {
            var item = itemsList[i];
            foreach (var tag in item.TagsNormalized)
            {
                var normalizedTag = Tag.NormalizeTag(tag);
                
                if (!relationships.ContainsKey(normalizedTag))
                    relationships[normalizedTag] = new List<int>();

                relationships[normalizedTag].Add(i);
            }
        }

        return relationships;
    }
}
