namespace TechHub.Core.Models.ContentProcessing;

/// <summary>
/// A <see cref="TimelineEvent"/> as stored in the database, with the article it comes from.
/// </summary>
public sealed record StoredTimelineEvent
{
    /// <summary>Database id.</summary>
    public required long Id { get; init; }

    /// <summary>Collection of the source article.</summary>
    public required string CollectionName { get; init; }

    /// <summary>Slug of the source article.</summary>
    public required string ItemSlug { get; init; }

    /// <summary>Event kind, see <see cref="TimelineEvent.ValidKinds"/>.</summary>
    public required string Kind { get; init; }

    /// <summary>Official product name.</summary>
    public required string Product { get; init; }

    /// <summary>Official feature name, or <c>null</c> for the whole product.</summary>
    public string? Feature { get; init; }

    /// <summary>One line describing what happens.</summary>
    public required string Description { get; init; }

    /// <summary>Date of the event (first day of the month or quarter for coarse precision).</summary>
    public required DateOnly EventDate { get; init; }

    /// <summary>Date precision: day, month or quarter.</summary>
    public required string DatePrecision { get; init; }

    /// <summary>Whether the date was stated in the article or derived from its publication date.</summary>
    public required string DateSource { get; init; }

    /// <summary>The sentence from the article that states the event.</summary>
    public string? Quote { get; init; }

    /// <summary>Where the event was extracted from: source or stored-content.</summary>
    public required string ExtractedFrom { get; init; }
}
