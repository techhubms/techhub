namespace TechHub.Core.Models.ContentProcessing;

/// <summary>
/// A dated lifecycle event of a product or feature (preview, GA, deprecation, ...), as stated by an article.
/// </summary>
public sealed record TimelineEvent
{
    /// <summary>Event kinds the AI may report.</summary>
    public static readonly IReadOnlySet<string> ValidKinds = new HashSet<string>(StringComparer.Ordinal)
    {
        "announced", "preview", "ga", "launch", "release", "deprecation", "retirement",
        "end-of-support", "breaking-change", "pricing-change", "event"
    };

    /// <summary>Date precisions the AI may report.</summary>
    public static readonly IReadOnlySet<string> ValidPrecisions = new HashSet<string>(StringComparer.Ordinal)
    {
        "day", "month", "quarter"
    };

    /// <summary>The article states the date.</summary>
    public const string DateSourceExplicit = "explicit";

    /// <summary>The article says the event happens now; the date is the article's publication date, set by code.</summary>
    public const string DateSourcePublication = "publication";

    /// <summary>Event kind, one of <see cref="ValidKinds"/>.</summary>
    public required string Kind { get; init; }

    /// <summary>Official product name, e.g. "GitHub Copilot".</summary>
    public required string Product { get; init; }

    /// <summary>Official feature name within the product, or <c>null</c> when the event is about the whole product.</summary>
    public string? Feature { get; init; }

    /// <summary>One line describing what happens.</summary>
    public required string Description { get; init; }

    /// <summary>
    /// A short headline that says what the event is (for example "Java 8, 11 and 17 support ends"), so a card on a timeline is
    /// understood at a glance. <c>null</c> for events stored before titles existed: they are shown as product and feature.
    /// </summary>
    public string? Title { get; init; }

    /// <summary>The longest title that is accepted; a longer one is not a headline and is dropped.</summary>
    public const int MaxTitleLength = 120;

    /// <summary>The date of the event (first day of the month or quarter for coarse precision).</summary>
    public required DateOnly Date { get; init; }

    /// <summary>One of <see cref="ValidPrecisions"/>.</summary>
    public required string DatePrecision { get; init; }

    /// <summary><see cref="DateSourceExplicit"/> or <see cref="DateSourcePublication"/>.</summary>
    public required string DateSource { get; init; }

    /// <summary>The sentence from the article that states the event. Kept for spot checks.</summary>
    public string? Quote { get; init; }
}
