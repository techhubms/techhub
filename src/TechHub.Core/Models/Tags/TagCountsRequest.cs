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
    public int? MaxTags { get; }
    public int MinUses { get; }
    
    /// <summary>
    /// Optional: Currently selected tags for dynamic count calculation.
    /// When provided, counts show items that match these tags AND each tag in the result.
    /// </summary>
    public IReadOnlyList<string>? Tags { get; }

    public TagCountsRequest(
        string sectionName,
        string collectionName,
        int minUses = 1,
        int? maxTags = null,
        DateTimeOffset? dateFrom = null,
        DateTimeOffset? dateTo = null,
        IReadOnlyList<string>? tags = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sectionName);
        ArgumentException.ThrowIfNullOrWhiteSpace(collectionName);

        if (minUses < 1)
        {
            throw new ArgumentException($"MinUses must be at least 1, got {minUses}", nameof(minUses));
        }

        if (maxTags.HasValue && maxTags.Value < 1)
        {
            throw new ArgumentException($"MaxTags must be at least 1, got {maxTags.Value}", nameof(maxTags));
        }

        if (maxTags.HasValue && maxTags.Value > 50)
        {
            throw new ArgumentException($"MaxTags cannot exceed 50, got {maxTags.Value}", nameof(maxTags));
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

        if (MaxTags.HasValue)
        {
            parts.Add($"max:{MaxTags.Value}");
        }

        parts.Add($"min:{MinUses}");
        
        if (Tags != null && Tags.Count > 0)
        {
            var sortedTags = Tags.OrderBy(t => t, StringComparer.OrdinalIgnoreCase);
            parts.Add($"tags:{string.Join(",", sortedTags)}");
        }

        return string.Join("|", parts);
    }
}
