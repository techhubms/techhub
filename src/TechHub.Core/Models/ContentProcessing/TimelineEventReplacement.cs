namespace TechHub.Core.Models.ContentProcessing;

/// <summary>
/// The timeline events that replace those of an article, written in the same transaction as the article change itself.
/// </summary>
/// <param name="Events">All events of the article. An empty list removes the previous ones.</param>
/// <param name="ExtractedFrom">Where the events were extracted from (source or stored content).</param>
/// <param name="PromptVersion">Version of the extraction prompt that produced the events.</param>
public sealed record TimelineEventReplacement(IReadOnlyList<TimelineEvent> Events, string ExtractedFrom, int PromptVersion);
