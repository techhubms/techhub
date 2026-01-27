using TechHub.Core.Models;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Service for generating tag clouds with quantile-based sizing and scoping logic
/// </summary>
public interface ITagCloudService
{
    /// <summary>
    /// Generate tag cloud for the specified scope
    /// </summary>
    /// <param name="request">Tag cloud request parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of tag cloud items with size categories</returns>
    Task<IReadOnlyList<TagCloudItem>> GetTagCloudAsync(
        TagCloudRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all tags with their usage counts for the specified scope
    /// </summary>
    /// <param name="sectionName">Optional section name for scoping</param>
    /// <param name="collectionName">Optional collection name for scoping</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>All tags with counts</returns>
    Task<AllTagsResponse> GetAllTagsAsync(
        string? sectionName = null,
        string? collectionName = null,
        CancellationToken cancellationToken = default);
}
