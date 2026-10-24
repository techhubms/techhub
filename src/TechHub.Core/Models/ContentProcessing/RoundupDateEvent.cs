namespace TechHub.Core.Models.ContentProcessing;

/// <summary>
/// A dated lifecycle event after the roundup week, with the article that states it.
/// Feeds the "Dates to know" block of a weekly roundup.
/// </summary>
public sealed record RoundupDateEvent
{
    /// <summary>Event kind, see <see cref="TimelineEvent.ValidKinds"/>.</summary>
    public required string Kind { get; init; }

    /// <summary>Official product name.</summary>
    public required string Product { get; init; }

    /// <summary>Official feature name, or <c>null</c> for the whole product.</summary>
    public string? Feature { get; init; }

    /// <summary>One line describing what happens.</summary>
    public required string Description { get; init; }

    /// <summary>Date of the event (first day of the month or quarter for coarse precision).</summary>
    public required DateOnly Date { get; init; }

    /// <summary>Date precision: day, month or quarter.</summary>
    public required string DatePrecision { get; init; }

    /// <summary>Title of the article that states the event.</summary>
    public required string SourceTitle { get; init; }

    /// <summary>Link to the article that states the event.</summary>
    public required string SourceUrl { get; init; }
}
