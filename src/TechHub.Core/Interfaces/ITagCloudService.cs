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
}
