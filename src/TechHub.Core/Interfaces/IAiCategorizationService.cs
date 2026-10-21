using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Calls Azure OpenAI to categorize a raw feed item and produce a processed content item.
/// </summary>
public interface IAiCategorizationService
{
    /// <summary>
    /// Step 1 only: classifies <paramref name="item"/> (include or exclude, sections, tags, excerpt, roundup metadata,
    /// timeline events) without writing the article body. The returned item, when included, has an empty
    /// <see cref="ProcessedContentItem.Content"/>.
    /// </summary>
    Task<CategorizationResult> ClassifyAsync(RawFeedItem item, CancellationToken ct = default);

    /// <summary>
    /// Classifies <paramref name="item"/> (step 1) and, only when it is included, rewrites it into the article
    /// markdown (step 2). Returns a <see cref="CategorizationResult"/> containing the processed item (or <c>null</c>
    /// if excluded) and the AI's explanation. A failing rewrite fails the whole item.
    /// </summary>
    Task<CategorizationResult> CategorizeAsync(RawFeedItem item, CancellationToken ct = default);
}
