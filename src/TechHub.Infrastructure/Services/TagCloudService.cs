using Microsoft.Extensions.Options;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// Service for generating tag clouds with quantile-based sizing and scoping logic
/// </summary>
public class TagCloudService : ITagCloudService
{
    private readonly IContentRepository _contentRepository;
    private readonly FilteringOptions _filteringOptions;

    public TagCloudService(
        IContentRepository contentRepository,
        IOptions<FilteringOptions> filteringOptions)
    {
        ArgumentNullException.ThrowIfNull(contentRepository);
        ArgumentNullException.ThrowIfNull(filteringOptions);

        _contentRepository = contentRepository;
        _filteringOptions = filteringOptions.Value;
    }

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
                request.CollectionName,
                request.Slug,
                cancellationToken);
        }

        // Calculate date filter if LastDays is specified
        DateTimeOffset? dateFrom = request.LastDays.HasValue
            ? DateTimeOffset.UtcNow.AddDays(-request.LastDays.Value)
            : null;

        // Determine section/collection filters based on scope
        // "all" is a virtual section/collection - means no filter
        string? sectionFilter = request.Scope == TagCloudScope.Section &&
                                !string.IsNullOrWhiteSpace(request.SectionName) &&
                                !request.SectionName.Equals("all", StringComparison.OrdinalIgnoreCase)
            ? request.SectionName
            : null;

        string? collectionFilter = request.Scope == TagCloudScope.Collection &&
                                   !string.IsNullOrWhiteSpace(request.CollectionName) &&
                                   !request.CollectionName.Equals("all", StringComparison.OrdinalIgnoreCase)
            ? request.CollectionName
            : null;

        // Get top N tag counts (repository filters section/collection tags internally)
        var tagCounts = await _contentRepository.GetTagCountsAsync(
            dateFrom: dateFrom,
            dateTo: null,
            sectionName: sectionFilter,
            collectionName: collectionFilter,
            maxTags: request.MaxTags,
            minUses: request.MinUses,
            ct: cancellationToken);

        if (tagCounts.Count == 0)
        {
            return [];
        }

        // Apply quantile-based sizing to top N tags
        var tagCloudItems = ApplyQuantileSizing([.. tagCounts]);

        return tagCloudItems;
    }

    private async Task<IReadOnlyList<TagCloudItem>> GetContentItemTagsAsync(
        string? collectionName,
        string? slug,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(collectionName) || string.IsNullOrWhiteSpace(slug))
        {
            return [];
        }

        var item = await _contentRepository.GetBySlugAsync(collectionName, slug, includeDraft: false, cancellationToken);

        if (item == null)
        {
            return [];
        }

        // Return all tags for the content item with Medium size (no quantile sizing for single item)
        return [.. item.Tags
            .Select(tag => new TagCloudItem
            {
                Tag = tag,
                Count = 1,
                Size = TagSize.Medium
            })];
    }

    private List<TagCloudItem> ApplyQuantileSizing(List<TagWithCount> sortedTags)
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
            var tag = sortedTags[i];
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
                Tag = tag.Tag,
                Count = tag.Count,
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
