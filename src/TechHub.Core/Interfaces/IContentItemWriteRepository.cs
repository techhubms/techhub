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
    /// Upserts a processed content item into <c>content_items</c>, rebuilds its entries in
    /// <c>content_tags_expanded</c> and replaces its timeline events (<see cref="ProcessedContentItem.TimelineEvents"/>,
    /// extracted from the source), all in one transaction: when any step fails the item is not stored at all,
    /// so processing it again can still succeed.
    /// </summary>
    Task UpsertProcessedItemAsync(ProcessedContentItem item, CancellationToken ct = default);

    /// <summary>
    /// Updates only the <c>ai_metadata</c> column for an existing content item,
    /// identified by its primary key (<paramref name="collectionName"/>, <paramref name="slug"/>).
    /// Used by the roundup backfill to avoid duplicate-key conflicts on <c>external_url</c>.
    /// </summary>
    Task UpdateAiMetadataAsync(string collectionName, string slug, string aiMetadataJson, CancellationToken ct = default);

    /// <summary>
    /// Updates the <c>ai_metadata</c> column and replaces the timeline events of the item in one transaction,
    /// so a failing event write leaves the previous metadata in place and the backfill can retry.
    /// Only applies when the article is still at <paramref name="expectedRevision"/> (<see cref="RoundupArticle.Revision"/>),
    /// checked under the article's write lock. Returns <c>false</c> and changes nothing when the article was edited
    /// after it was read, so metadata and events classified from outdated text are never stored.
    /// </summary>
    Task<bool> UpdateAiMetadataIfUnchangedAsync(string collectionName, string slug, string aiMetadataJson, TimelineEventReplacement timeline, long expectedRevision, CancellationToken ct = default);
}
