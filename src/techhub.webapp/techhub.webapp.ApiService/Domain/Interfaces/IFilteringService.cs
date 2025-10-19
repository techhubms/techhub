using techhub.webapp.ApiService.Domain.Models;

namespace techhub.webapp.ApiService.Domain.Interfaces;

/// <summary>
/// Filtering service interface implementing Tech Hub filtering logic
/// </summary>
public interface IFilteringService
{
    /// <summary>
    /// Applies date filter to content items
    /// </summary>
    /// <param name="items">Content items to filter</param>
    /// <param name="days">Number of days to include (0 = today only, 7 = last 7 days, etc.)</param>
    IEnumerable<ContentItem> ApplyDateFilter(IEnumerable<ContentItem> items, int days);

    /// <summary>
    /// Applies tag filter with subset matching (AND logic for multiple tags)
    /// </summary>
    /// <param name="items">Content items to filter</param>
    /// <param name="tags">Normalized tags to filter by (e.g., "ai", "github copilot")</param>
    IEnumerable<ContentItem> ApplyTagFilter(IEnumerable<ContentItem> items, List<string> tags);

    /// <summary>
    /// Applies section filter
    /// </summary>
    /// <param name="items">Content items to filter</param>
    /// <param name="sectionKey">Section key (ai, github-copilot, ml, azure, coding, devops, security)</param>
    IEnumerable<ContentItem> ApplySectionFilter(IEnumerable<ContentItem> items, string sectionKey);

    /// <summary>
    /// Applies collection filter
    /// </summary>
    /// <param name="items">Content items to filter</param>
    /// <param name="collectionType">Collection type (news, posts, videos, community, events, roundups)</param>
    IEnumerable<ContentItem> ApplyCollectionFilter(IEnumerable<ContentItem> items, string collectionType);

    /// <summary>
    /// Applies text search across titles, descriptions, authors, and tags
    /// </summary>
    /// <param name="items">Content items to filter</param>
    /// <param name="searchQuery">Search query string (case-insensitive, partial matching)</param>
    IEnumerable<ContentItem> ApplyTextSearch(IEnumerable<ContentItem> items, string searchQuery);

    /// <summary>
    /// Applies "N + Same-Day" content limiting rule per collection
    /// </summary>
    /// <param name="items">Content items to limit</param>
    /// <param name="limit">Number of items to include per collection (default 20)</param>
    IEnumerable<ContentItem> ApplyContentLimiting(IEnumerable<ContentItem> items, int limit = 20);

    /// <summary>
    /// Applies N-day recency filter (default 7 days)
    /// </summary>
    /// <param name="items">Content items to filter</param>
    /// <param name="days">Number of days to include (default 7)</param>
    IEnumerable<ContentItem> ApplyRecencyFilter(IEnumerable<ContentItem> items, int days = 7);

    /// <summary>
    /// Determines which date filters to show based on content availability
    /// </summary>
    /// <param name="items">Content items to analyze</param>
    /// <returns>List of eligible date filter options (in days: 0, 2, 3, 7, 30, 90, etc.)</returns>
    List<int> GetEligibleDateFilters(IEnumerable<ContentItem> items);

    /// <summary>
    /// Counts content items matching specific tags
    /// </summary>
    /// <param name="items">Content items to count</param>
    /// <param name="tags">Tags to match (with subset matching)</param>
    int CountByTags(IEnumerable<ContentItem> items, List<string> tags);

    /// <summary>
    /// Counts content items matching specific date filter
    /// </summary>
    /// <param name="items">Content items to count</param>
    /// <param name="days">Date filter option in days</param>
    int CountByDateFilter(IEnumerable<ContentItem> items, int days);

    /// <summary>
    /// Builds tag relationships for pre-calculated subset matching
    /// </summary>
    /// <param name="items">Content items to analyze</param>
    /// <returns>Dictionary mapping normalized tags to post indices</returns>
    Dictionary<string, List<int>> BuildTagRelationships(IEnumerable<ContentItem> items);
}
