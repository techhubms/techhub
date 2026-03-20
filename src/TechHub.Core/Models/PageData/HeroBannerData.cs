namespace TechHub.Core.Models;

/// <summary>
/// Data structure for the hero banner shown above section content.
/// Cards are filtered client-side by their start/end dates.
/// </summary>
public record HeroBannerData
{
    public required IReadOnlyList<HeroBannerCard> Cards { get; init; }
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
