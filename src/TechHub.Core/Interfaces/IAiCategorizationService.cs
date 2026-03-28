using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Calls Azure OpenAI to categorize a raw feed item and produce a processed content item.
/// </summary>
public interface IAiCategorizationService
{
    /// <summary>
    /// Calls Azure OpenAI to categorize <paramref name="item"/>.
    /// Returns <see langword="null"/> when the AI determines the content should be skipped.
    /// </summary>
    Task<ProcessedContentItem?> CategorizeAsync(RawFeedItem item, CancellationToken ct = default);
}
