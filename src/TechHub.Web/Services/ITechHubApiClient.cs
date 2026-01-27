using TechHub.Core.Models;

namespace TechHub.Web.Services;

/// <summary>
/// Interface for Tech Hub API client
/// Enables mocking in unit tests
/// </summary>
internal interface ITechHubApiClient
{
    /// <summary>
    /// Get tag cloud for specified scope
    /// </summary>
    Task<IReadOnlyList<TagCloudItem>?> GetTagCloudAsync(
        TagCloudScope scope,
        string? sectionName = null,
        string? collectionName = null,
        string? slug = null,
        int? maxTags = null,
        int? minUses = null,
        int? lastDays = null,
        CancellationToken cancellationToken = default);
}
