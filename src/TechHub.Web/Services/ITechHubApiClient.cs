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
    Task<IReadOnlyList<TagCloudItem>?> GetTagCloudAsync(
        string sectionName,
        string collectionName,
        int? maxTags = null,
        int? minUses = null,
        int? lastDays = null,
        List<string>? selectedTags = null,
        string? fromDate = null,
        string? toDate = null,
        CancellationToken cancellationToken = default);
}
