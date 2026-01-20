using Microsoft.Extensions.Options;
using TechHub.Core.DTOs;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// Service for generating tag clouds with quantile-based sizing and scoping logic
/// </summary>
public class TagCloudService(
    IContentRepository contentRepository,
    IOptions<FilteringOptions> filteringOptions) : ITagCloudService
{
    private readonly IContentRepository _contentRepository = contentRepository ?? throw new ArgumentNullException(nameof(contentRepository));
    private readonly FilteringOptions _filteringOptions = filteringOptions?.Value ?? throw new ArgumentNullException(nameof(filteringOptions));

    /// <summary>
    /// Generate tag cloud for the specified scope
    /// </summary>
    public async Task<IReadOnlyList<TagCloudItem>> GetTagCloudAsync(
        TagCloudRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        // For content item scope, return only that item's tags
        if (request.Scope == TagCloudScope.Content)
        {
            return await GetContentItemTagsAsync(
                request.SectionName,
                request.CollectionName,
                request.ContentItemId,
                cancellationToken);
        }

        // Get content items based on scope
        var items = await GetItemsByScopeAsync(request, cancellationToken);

        // Filter by date range if specified
        if (request.LastDays.HasValue)
        {
            var cutoffEpoch = DateTimeOffset.UtcNow
                .AddDays(-request.LastDays.Value)
                .ToUnixTimeSeconds();

            items = items.Where(item => item.DateEpoch >= cutoffEpoch).ToList();
        }

        // Count tag occurrences
        var tagCounts = CountTags(items);

        // Filter by minimum uses and get top tags
        var filteredTags = tagCounts
            .Where(kvp => kvp.Value >= request.MinUses)
            .OrderByDescending(kvp => kvp.Value)
            .ToList();

        // Dynamic quantity: top MaxTags OR all tags with >= MinUses (whichever is fewer)
        var tagsToInclude = filteredTags.Take(request.MaxTags).ToList();

        if (tagsToInclude.Count == 0)
        {
            return [];
        }

        // Apply quantile-based sizing
        var tagCloudItems = ApplyQuantileSizing(tagsToInclude);

        return tagCloudItems;
    }

    /// <summary>
    /// Get all tags with their usage counts for the specified scope
    /// </summary>
    public async Task<AllTagsResponse> GetAllTagsAsync(
        string? sectionName = null,
        string? collectionName = null,
        CancellationToken cancellationToken = default)
    {
        // Get items based on scope
        IReadOnlyList<ContentItem> items;

        if (!string.IsNullOrWhiteSpace(collectionName) && !string.IsNullOrWhiteSpace(sectionName))
        {
            // Collection scope: filter by collection AND section
            var allCollectionItems = await _contentRepository.GetByCollectionAsync(collectionName, cancellationToken);
            items = allCollectionItems
                .Where(item => item.SectionNames.Contains(sectionName, StringComparer.OrdinalIgnoreCase))
                .ToList();
        }
        else if (!string.IsNullOrWhiteSpace(sectionName))
        {
            // Section scope
            items = await _contentRepository.GetBySectionAsync(sectionName, cancellationToken);
        }
        else
        {
            // Global scope
            items = await _contentRepository.GetAllAsync(cancellationToken);
        }

        // Count tags
        var tagCounts = CountTags(items);

        // Create response with tags sorted by count (descending), then alphabetically
        var tags = tagCounts
            .OrderByDescending(kvp => kvp.Value)
            .ThenBy(kvp => kvp.Key, StringComparer.OrdinalIgnoreCase)
            .Select(kvp => new TagWithCount
            {
                Tag = kvp.Key,
                Count = kvp.Value
            })
            .ToList();

        return new AllTagsResponse
        {
            Tags = tags,
            TotalCount = tags.Count
        };
    }

    private async Task<IReadOnlyList<TagCloudItem>> GetContentItemTagsAsync(
        string? sectionName,
        string? collectionName,
        string? contentItemId,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(collectionName) || string.IsNullOrWhiteSpace(contentItemId))
        {
            return [];
        }

        var item = await _contentRepository.GetBySlugAsync(collectionName, contentItemId, cancellationToken);

        if (item == null)
        {
            return [];
        }

        // Return all tags for the content item with Medium size (no quantile sizing for single item)
        return item.Tags
            .Select(tag => new TagCloudItem
            {
                Tag = tag,
                Count = 1,
                Size = TagSize.Medium
            })
            .ToList();
    }

    private async Task<IReadOnlyList<ContentItem>> GetItemsByScopeAsync(
        TagCloudRequest request,
        CancellationToken cancellationToken)
    {
        return request.Scope switch
        {
            TagCloudScope.Homepage => await _contentRepository.GetAllAsync(cancellationToken),

            TagCloudScope.Section when !string.IsNullOrWhiteSpace(request.SectionName) =>
                await _contentRepository.GetBySectionAsync(request.SectionName, cancellationToken),

            TagCloudScope.Collection when !string.IsNullOrWhiteSpace(request.CollectionName) &&
                                         !string.IsNullOrWhiteSpace(request.SectionName) =>
                await GetCollectionItemsAsync(request.SectionName, request.CollectionName, cancellationToken),

            _ => []
        };
    }

    private async Task<IReadOnlyList<ContentItem>> GetCollectionItemsAsync(
        string sectionName,
        string collectionName,
        CancellationToken cancellationToken)
    {
        // Handle "all" virtual collection - returns all content for the section across all collections
        if (collectionName.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            return await _contentRepository.GetBySectionAsync(sectionName, cancellationToken);
        }

        var allCollectionItems = await _contentRepository.GetByCollectionAsync(collectionName, cancellationToken);

        return allCollectionItems
            .Where(item => item.SectionNames.Contains(sectionName, StringComparer.OrdinalIgnoreCase))
            .ToList();
    }

    private Dictionary<string, int> CountTags(IEnumerable<ContentItem> items)
    {
        var tagCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (var item in items)
        {
            foreach (var tag in item.Tags)
            {
                if (tagCounts.ContainsKey(tag))
                {
                    tagCounts[tag]++;
                }
                else
                {
                    tagCounts[tag] = 1;
                }
            }
        }

        return tagCounts;
    }

    private IReadOnlyList<TagCloudItem> ApplyQuantileSizing(List<KeyValuePair<string, int>> sortedTags)
    {
        if (sortedTags.Count == 0)
        {
            return [];
        }

        // Get quantile percentiles from configuration
        var smallToMedium = _filteringOptions.TagCloud.QuantilePercentiles.SmallToMedium;
        var mediumToLarge = _filteringOptions.TagCloud.QuantilePercentiles.MediumToLarge;

        var totalTags = sortedTags.Count;
        var smallThreshold = (int)Math.Ceiling(totalTags * smallToMedium);
        var largeThreshold = (int)Math.Ceiling(totalTags * mediumToLarge);

        var result = new List<TagCloudItem>();

        for (int i = 0; i < sortedTags.Count; i++)
        {
            var kvp = sortedTags[i];
            TagSize size;

            // Top 25% (0-25% index) = Large
            // Middle 50% (25%-75% index) = Medium
            // Bottom 25% (75%-100% index) = Small
            if (i < smallThreshold)
            {
                size = TagSize.Large;
            }
            else if (i < largeThreshold)
            {
                size = TagSize.Medium;
            }
            else
            {
                size = TagSize.Small;
            }

            result.Add(new TagCloudItem
            {
                Tag = kvp.Key,
                Count = kvp.Value,
                Size = size
            });
        }

        return result;
    }
}

/// <summary>
/// Configuration options for filtering functionality
/// </summary>
public class FilteringOptions
{
    public TagCloudOptions TagCloud { get; set; } = new();
    public DateRangeOptions DateRange { get; set; } = new();
    public TagDropdownOptions TagDropdown { get; set; } = new();
}

public class TagCloudOptions
{
    public int DefaultMaxTags { get; set; } = 20;
    public int MinimumTagUses { get; set; } = 5;
    public int DefaultDateRangeDays { get; set; } = 90;
    public QuantilePercentilesOptions QuantilePercentiles { get; set; } = new();
}

public class QuantilePercentilesOptions
{
    public double SmallToMedium { get; set; } = 0.25;
    public double MediumToLarge { get; set; } = 0.75;
}

public class DateRangeOptions
{
    public int DefaultDays { get; set; } = 90;
    public PresetsOptions Presets { get; set; } = new();
}

public class PresetsOptions
{
    public int Last7Days { get; set; } = 7;
    public int Last30Days { get; set; } = 30;
    public int Last90Days { get; set; } = 90;
}

public class TagDropdownOptions
{
    public int VirtualScrollThreshold { get; set; } = 50;
    public bool EnableVirtualScroll { get; set; } = true;
}
