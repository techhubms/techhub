namespace TechHub.Core.Models;

/// <summary>
/// Represents the current state of applied filters for content discovery
/// Used for client-side filtering and URL state synchronization
/// </summary>
public class FilterState
{
    /// <summary>
    /// Selected date range preset (e.g., "last-30-days", "last-90-days", "all-time")
    /// </summary>
    public string? DateRange { get; init; }

    /// <summary>
    /// Selected tags for OR filtering
    /// </summary>
    public IReadOnlyList<string> SelectedTags { get; init; } = [];

    /// <summary>
    /// Text search query (searches title, excerpt, and tags)
    /// </summary>
    public string? SearchQuery { get; init; }

    /// <summary>
    /// Start date for custom date range (Unix timestamp)
    /// </summary>
    public long? StartDateEpoch { get; init; }

    /// <summary>
    /// End date for custom date range (Unix timestamp)
    /// </summary>
    public long? EndDateEpoch { get; init; }

    /// <summary>
    /// Whether any filters are currently active
    /// </summary>
    public bool HasActiveFilters =>
        !string.IsNullOrWhiteSpace(DateRange) ||
        SelectedTags.Count > 0 ||
        !string.IsNullOrWhiteSpace(SearchQuery);

    /// <summary>
    /// Create filter state from URL query parameters
    /// </summary>
    public static FilterState FromQueryString(Dictionary<string, string> queryParams)
    {
        return new FilterState
        {
            DateRange = queryParams.GetValueOrDefault("dateRange"),
            SelectedTags = queryParams.GetValueOrDefault("tags")?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? [],
            SearchQuery = queryParams.GetValueOrDefault("search"),
            StartDateEpoch = long.TryParse(queryParams.GetValueOrDefault("startDate"), out var start) ? start : null,
            EndDateEpoch = long.TryParse(queryParams.GetValueOrDefault("endDate"), out var end) ? end : null
        };
    }

    /// <summary>
    /// Convert filter state to URL query string
    /// </summary>
    public string ToQueryString()
    {
        var parts = new List<string>();

        if (!string.IsNullOrWhiteSpace(DateRange))
            parts.Add($"dateRange={Uri.EscapeDataString(DateRange)}");

        if (SelectedTags.Count > 0)
            parts.Add($"tags={Uri.EscapeDataString(string.Join(",", SelectedTags))}");

        if (!string.IsNullOrWhiteSpace(SearchQuery))
            parts.Add($"search={Uri.EscapeDataString(SearchQuery)}");

        if (StartDateEpoch.HasValue)
            parts.Add($"startDate={StartDateEpoch}");

        if (EndDateEpoch.HasValue)
            parts.Add($"endDate={EndDateEpoch}");

        return parts.Count > 0 ? "?" + string.Join("&", parts) : string.Empty;
    }
}
