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
    /// </summary>
    Task<IReadOnlyList<TagCloudItem>?> GetTagCloudAsync(
        string sectionName,
        string collectionName,
        int? maxTags = null,
        int? minUses = null,
        int? lastDays = null,
        CancellationToken cancellationToken = default);
}
