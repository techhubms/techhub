namespace techhub.webapp.ApiService.Domain.Models;

/// <summary>
/// Represents weekly content summaries shown on the homepage.
/// </summary>
public class Roundup : ContentItem
{
    public Roundup()
    {
        CollectionType = "roundups";
    }

    /// <summary>
    /// Time period covered by this roundup (e.g., "Week of Jan 1-7, 2025").
    /// </summary>
    public required string RoundupPeriod { get; init; }

    /// <summary>
    /// Validates the roundup including roundup-specific rules.
    /// </summary>
    public override void Validate()
    {
        base.Validate();

        if (string.IsNullOrWhiteSpace(RoundupPeriod))
            throw new ArgumentException("RoundupPeriod cannot be empty", nameof(RoundupPeriod));
    }
}
