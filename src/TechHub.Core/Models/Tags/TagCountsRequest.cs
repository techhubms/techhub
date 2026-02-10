namespace TechHub.Core.Models;

/// <summary>
/// Request for tag counts with optional filtering.
/// Returns top N tags (sorted by count descending) above minUses threshold.
/// Supports dynamic count calculation when Tags filter is provided.
/// </summary>
public class TagCountsRequest
{
    public DateTimeOffset? DateFrom { get; }
    public DateTimeOffset? DateTo { get; }
    public string SectionName { get; }
    public string CollectionName { get; }
    public int MaxTags { get; }
    public int MinUses { get; }

    /// <summary>
    /// Optional: Currently selected tags for dynamic count calculation.
    /// When provided, counts show items that match these tags AND each tag in the result.
    /// </summary>
    public IReadOnlyList<string>? Tags { get; }

    /// <summary>
    /// Optional: Specific tags to get counts for.
    /// When provided, only returns counts for these exact tags (filtered by Tags intersection).
    /// Used to get updated counts for a baseline set of tags when filters change.
    /// </summary>
    public IReadOnlyList<string>? TagsToCount { get; }

    public TagCountsRequest(
        string sectionName,
        string collectionName,
        int maxTags,
        int minUses = 1,
        DateTimeOffset? dateFrom = null,
        DateTimeOffset? dateTo = null,
        IReadOnlyList<string>? tags = null,
        IReadOnlyList<string>? tagsToCount = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sectionName);
        ArgumentException.ThrowIfNullOrWhiteSpace(collectionName);

        if (minUses < 1)
        {
            throw new ArgumentException($"MinUses must be at least 1, got {minUses}", nameof(minUses));
        }

        if (maxTags < 1)
        {
            throw new ArgumentException($"MaxTags must be at least 1, got {maxTags}", nameof(maxTags));
        }

        if (maxTags > 50)
        {
            throw new ArgumentException($"MaxTags cannot exceed 50, got {maxTags}", nameof(maxTags));
        }

        if (dateFrom.HasValue && dateTo.HasValue && dateFrom > dateTo)
        {
            throw new ArgumentException("DateFrom cannot be after DateTo", nameof(dateFrom));
        }

        SectionName = sectionName;
        CollectionName = collectionName;
        MinUses = minUses;
        MaxTags = maxTags;
        DateFrom = dateFrom;
        DateTo = dateTo;
        Tags = tags;
        TagsToCount = tagsToCount;
    }

    /// <summary>
    /// Generate cache key for this tag counts request.
    /// Includes all filter parameters including selected tags.
    /// </summary>
    public string GetCacheKey()
    {
        var parts = new List<string> { "tagcounts" };

        if (DateFrom.HasValue)
        {
            parts.Add($"df:{DateFrom.Value.ToUnixTimeSeconds()}");
        }

        if (DateTo.HasValue)
        {
            parts.Add($"dt:{DateTo.Value.ToUnixTimeSeconds()}");
        }

        parts.Add($"s:{SectionName}");
        parts.Add($"c:{CollectionName}");

        parts.Add($"max:{MaxTags}");

        parts.Add($"min:{MinUses}");

        if (Tags != null && Tags.Count > 0)
        {
            var sortedTags = Tags.OrderBy(t => t, StringComparer.OrdinalIgnoreCase);
            parts.Add($"tags:{string.Join(",", sortedTags)}");
        }

        if (TagsToCount != null && TagsToCount.Count > 0)
        {
            var sortedTagsToCount = TagsToCount.OrderBy(t => t, StringComparer.OrdinalIgnoreCase);
            parts.Add($"ttc:{string.Join(",", sortedTagsToCount)}");
        }

        return string.Join("|", parts);
    }
}
