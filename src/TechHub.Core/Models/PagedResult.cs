namespace TechHub.Core.Models;

/// <summary>
/// A page of results with total count for server-side pagination.
/// </summary>
/// <typeparam name="T">The type of items in the page.</typeparam>
public sealed class PagedResult<T>
{
    /// <summary>The items on this page.</summary>
    public required IReadOnlyList<T> Items { get; init; }

    /// <summary>Total number of items matching the filter (across all pages).</summary>
    public required int TotalCount { get; init; }
}
