using TechHub.Core.Models;

namespace TechHub.Web.Services;

/// <summary>
/// Interface for Tech Hub API client
/// Enables mocking in unit tests
/// </summary>
internal interface ITechHubApiClient
{
    /// <summary>
    /// Get tag cloud for specified scope.
    /// Uses /api/sections/{sectionName}/collections/{collectionName}/tags endpoint.
    /// Pass "all" as collectionName for section-level tag cloud.
    /// Supports dynamic counts via selectedTags and date range parameters.
    /// </summary>
    /// <param name="sectionName">Section name</param>
    /// <param name="collectionName">Collection name (or "all")</param>
    /// <param name="maxTags">Maximum tags to return</param>
    /// <param name="minUses">Minimum usage count</param>
    /// <param name="lastDays">Only include tags from content in last N days</param>
    /// <param name="selectedTags">Currently selected tags for intersection counting</param>
    /// <param name="tagsToCount">Specific tags to get counts for (baseline tags)</param>
    /// <param name="fromDate">Start date filter</param>
    /// <param name="toDate">End date filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IReadOnlyList<TagCloudItem>?> GetTagCloudAsync(
        string sectionName,
        string collectionName,
        int? maxTags = null,
        int? minUses = null,
        int? lastDays = null,
        List<string>? selectedTags = null,
        List<string>? tagsToCount = null,
        string? fromDate = null,
        string? toDate = null,
        CancellationToken cancellationToken = default);
}
