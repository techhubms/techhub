using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Calls Azure OpenAI to categorize a raw feed item and produce a processed content item.
/// </summary>
public interface IAiCategorizationService
{
    /// <summary>
    /// Calls Azure OpenAI to categorize <paramref name="item"/>.
    /// Returns a <see cref="CategorizationResult"/> containing the processed item (or <c>null</c> if excluded)
    /// and the AI's explanation for why the content was included or excluded.
    /// </summary>
    Task<CategorizationResult> CategorizeAsync(RawFeedItem item, CancellationToken ct = default);
}
