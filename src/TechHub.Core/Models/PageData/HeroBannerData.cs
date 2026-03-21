namespace TechHub.Core.Models;

/// <summary>
/// Data structure for the hero banner shown above section content.
/// Cards are filtered client-side by their start/end dates.
/// </summary>
public record HeroBannerData
{
    public required IReadOnlyList<HeroBannerCard> Cards { get; init; }

    /// <summary>
    /// Optional label shown in the banner header (e.g. "Upcoming Events").
    /// When null or empty, no label text is displayed.
    /// </summary>
    public string? Label { get; init; }

    /// <summary>
    /// Optional URL for a secondary "find more" link button in the banner header.
    /// When null, no secondary link is rendered.
    /// </summary>
    public string? FindMoreUrl { get; init; }

    /// <summary>
    /// Optional display text for the secondary "find more" link button.
    /// Falls back to "Find more" when <see cref="FindMoreUrl"/> is set but this is null.
    /// </summary>
    public string? FindMoreText { get; init; }
}

/// <summary>
/// A single card displayed inside the hero banner.
/// </summary>
public record HeroBannerCard
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public string? LogoUrl { get; init; }

    /// <summary>Date from which this card becomes visible (inclusive), in yyyy-MM-dd format.</summary>
    public required string StartDate { get; init; }

    /// <summary>Date after which this card is no longer visible (inclusive), in yyyy-MM-dd format.</summary>
    public required string EndDate { get; init; }

    public string? LinkUrl { get; init; }
    public string? LinkText { get; init; }
}
