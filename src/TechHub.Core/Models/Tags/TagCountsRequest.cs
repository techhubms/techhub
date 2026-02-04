namespace TechHub.Core.Models;

/// <summary>
/// Request for tag counts with optional filtering.
/// Returns top N tags (sorted by count descending) above minUses threshold.
/// </summary>
public class TagCountsRequest
{
    public DateTimeOffset? DateFrom { get; }
    public DateTimeOffset? DateTo { get; }
    public string SectionName { get; }
    public string CollectionName { get; }
    public int? MaxTags { get; }
    public int MinUses { get; }

    public TagCountsRequest(
        string sectionName,
        string collectionName,
        int minUses = 1,
        int? maxTags = null,
        DateTimeOffset? dateFrom = null,
        DateTimeOffset? dateTo = null)
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
    }

    /// <summary>
    /// Generate cache key for this tag counts request.
    /// Includes all filter parameters.
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

        return string.Join("|", parts);
    }
}
