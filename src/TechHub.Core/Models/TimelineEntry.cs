namespace TechHub.Core.Models;

/// <summary>
/// A dated lifecycle event (preview, GA, deprecation, ...) of a section timeline. Articles that state the same event
/// (same date, kind, product and feature) are merged into one entry with all of them as <see cref="Sources"/>.
/// </summary>
public sealed record TimelineEntry
{
    /// <summary>The date of the event (first day of the month or quarter for coarse precision).</summary>
    public required DateOnly Date { get; init; }

    /// <summary><c>day</c>, <c>month</c> or <c>quarter</c>.</summary>
    public required string DatePrecision { get; init; }

    /// <summary>Event kind, see <c>TimelineEvent.ValidKinds</c>.</summary>
    public required string Kind { get; init; }

    /// <summary>The catalog name of the product, or the name as the AI wrote it when it is not in the catalog.</summary>
    public required string Product { get; init; }

    /// <summary>The catalog product, or <c>null</c> when the name is not in the catalog.</summary>
    public long? ProductId { get; init; }

    /// <summary>The catalog name of the feature, or the name as the AI wrote it. <c>null</c> when the event is about the whole product.</summary>
    public string? Feature { get; init; }

    /// <summary>One line describing what happens, from the oldest article that states the event.</summary>
    public required string Description { get; init; }

    /// <summary>The plans (Free, Pro, Business, ...) the event applies to. Empty when it applies to all or the plans are not in the catalog.</summary>
    public IReadOnlyList<string> Plans { get; init; } = [];

    /// <summary>The surfaces (Visual Studio Code, JetBrains IDEs, ...) the event applies to. Empty when it applies to all or the surfaces are not in the catalog.</summary>
    public IReadOnlyList<string> Surfaces { get; init; } = [];

    /// <summary>The articles that state the event, oldest first.</summary>
    public required IReadOnlyList<TimelineEntrySource> Sources { get; init; }
}

/// <summary>An article that states a <see cref="TimelineEntry"/>.</summary>
public sealed record TimelineEntrySource
{
    public required string CollectionName { get; init; }
    public required string Slug { get; init; }
    public required string Title { get; init; }

    /// <summary>The link of the original article, for the collections that link externally (news, blogs, community).</summary>
    public string? ExternalUrl { get; init; }

    /// <summary>The primary section of the article, used to build the Tech Hub address.</summary>
    public string? PrimarySectionName { get; init; }

    /// <summary>Where the article is read: its external address for external collections, else its Tech Hub page.</summary>
    public string GetHref() => ContentItem.BuildHref(CollectionName, Slug, ExternalUrl ?? string.Empty, PrimarySectionName);
}

/// <summary>What a section timeline contains: the events of the articles of a section, within an optional date range and kinds.</summary>
/// <param name="SectionName">The section whose articles are used. <c>all</c> covers every section.</param>
/// <param name="From">Only events on or after this date.</param>
/// <param name="To">Only events on or before this date.</param>
/// <param name="Kinds">Only these event kinds. Null or empty means every kind.</param>
public sealed record TimelineRequest(
    string SectionName,
    DateOnly? From = null,
    DateOnly? To = null,
    IReadOnlyList<string>? Kinds = null);
