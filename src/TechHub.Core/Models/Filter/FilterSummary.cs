namespace TechHub.Core.Models;

/// <summary>
/// Summary of filters applied to the current result set
/// </summary>
public record FilterSummary
{
    /// <summary>
    /// Tags used in filtering
    /// </summary>
    public required IReadOnlyList<string> Tags { get; init; }

    /// <summary>
    /// Date range start (if applied)
    /// </summary>
    public DateTimeOffset? DateFrom { get; init; }

    /// <summary>
    /// Date range end (if applied)
    /// </summary>
    public DateTimeOffset? DateTo { get; init; }

    /// <summary>
    /// Section scope (if applied)
    /// </summary>
    public string? SectionName { get; init; }

    /// <summary>
    /// Collection scope (if applied)
    /// </summary>
    public string? CollectionName { get; init; }

    /// <summary>
    /// Whether any filters are active
    /// </summary>
    public bool HasActiveFilters => Tags.Count > 0 || DateFrom.HasValue || DateTo.HasValue;
}
