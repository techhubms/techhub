using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Write operations for content items used by the content processing pipeline.
/// Separates write concerns from the read-focused <see cref="IContentRepository"/>.
/// </summary>
public interface IContentItemWriteRepository
{
    /// <summary>
    /// Checks whether a content item with the given external URL already exists.
    /// </summary>
    Task<bool> ExistsByExternalUrlAsync(string externalUrl, CancellationToken ct = default);

    /// <summary>
    /// Upserts a processed content item into <c>content_items</c> and rebuilds
    /// its entries in <c>content_tags_expanded</c>.
    /// </summary>
    Task UpsertProcessedItemAsync(ProcessedContentItem item, CancellationToken ct = default);

    /// <summary>
    /// Updates only the <c>ai_metadata</c> column for an existing content item,
    /// identified by its primary key (<paramref name="collectionName"/>, <paramref name="slug"/>).
    /// Used by the roundup backfill to avoid duplicate-key conflicts on <c>external_url</c>.
    /// </summary>
    Task UpdateAiMetadataAsync(string collectionName, string slug, string aiMetadataJson, CancellationToken ct = default);
}
