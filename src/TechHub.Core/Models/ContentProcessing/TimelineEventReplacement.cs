namespace TechHub.Core.Models.ContentProcessing;

/// <summary>
/// The timeline events that replace those of an article, written in the same transaction as the article change itself.
/// </summary>
/// <param name="Events">All events of the article. An empty list removes the previous ones.</param>
/// <param name="ExtractedFrom">Where the events were extracted from (source or stored content).</param>
/// <param name="PromptVersion">Version of the extraction prompt that produced the events.</param>
/// <param name="Products">
/// When set, replaces the products in the <c>ai_metadata</c> of the article and relinks them to the catalog. An article without
/// metadata gets an object with only the products (when there are any), which the roundup metadata backfill still treats as unclassified.
/// <c>null</c> leaves the stored products alone.
/// </param>
public sealed record TimelineEventReplacement(
    IReadOnlyList<TimelineEvent> Events,
    string ExtractedFrom,
    int PromptVersion,
    IReadOnlyList<string>? Products = null);
