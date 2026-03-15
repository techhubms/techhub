namespace TechHub.Core.Models;

/// <summary>
/// Request for facet counts (used separately from search for dynamic filtering).
/// </summary>
public class FacetRequest
{
    public IReadOnlyList<string> Tags { get; }
    public IReadOnlyList<string> Sections { get; }
    public IReadOnlyList<string> Collections { get; }
    public DateTimeOffset? DateFrom { get; }
    public DateTimeOffset? DateTo { get; }
    public IReadOnlyList<string> FacetFields { get; }
    public int MaxFacetValues { get; }

    public FacetRequest(
        IReadOnlyList<string> facetFields,
        IReadOnlyList<string> tags,
        IReadOnlyList<string> sections,
        IReadOnlyList<string> collections,
        int maxFacetValues = 50,
        DateTimeOffset? dateFrom = null,
        DateTimeOffset? dateTo = null)
    {
        ArgumentNullException.ThrowIfNull(facetFields);
        ArgumentNullException.ThrowIfNull(tags);
        ArgumentNullException.ThrowIfNull(sections);
        ArgumentNullException.ThrowIfNull(collections);

        if (facetFields.Count == 0)
        {
            throw new ArgumentException("FacetFields cannot be empty", nameof(facetFields));
        }

        if (maxFacetValues < 1)
        {
            throw new ArgumentException($"MaxFacetValues must be at least 1, got {maxFacetValues}", nameof(maxFacetValues));
        }

        if (maxFacetValues > 50)
        {
            throw new ArgumentException($"MaxFacetValues cannot exceed 50, got {maxFacetValues}", nameof(maxFacetValues));
        }

        if (dateFrom.HasValue && dateTo.HasValue && dateFrom > dateTo)
        {
            throw new ArgumentException("DateFrom cannot be after DateTo", nameof(dateFrom));
        }

        FacetFields = facetFields;
        MaxFacetValues = maxFacetValues;
        Tags = tags;
        Sections = sections;
        Collections = collections;
        DateFrom = dateFrom;
        DateTo = dateTo;
    }

    /// <summary>
    /// Generate cache key for this facet request.
    /// Includes all filter parameters and facet configuration.
    /// </summary>
    public string GetCacheKey()
    {
        var parts = new List<string> { "facets" };

        if (Tags.Count > 0)
        {
            parts.Add($"t:{string.Join(",", Tags.OrderBy(x => x))}");
        }

        if (Sections.Count > 0)
        {
            parts.Add($"s:{string.Join(",", Sections.OrderBy(x => x))}");
        }

        if (Collections.Count > 0)
        {
            parts.Add($"c:{string.Join(",", Collections.OrderBy(x => x))}");
        }

        if (DateFrom.HasValue)
        {
            parts.Add($"df:{DateFrom.Value.ToUnixTimeSeconds()}");
        }

        if (DateTo.HasValue)
        {
            parts.Add($"dt:{DateTo.Value.ToUnixTimeSeconds()}");
        }

        parts.Add($"fields:{string.Join(",", FacetFields.OrderBy(x => x))}");
        parts.Add($"max:{MaxFacetValues}");

        return string.Join("|", parts);
    }
}
