namespace techhub.webapp.ApiService.Domain.Models;

/// <summary>
/// Represents events, conferences, and meetups.
/// </summary>
public class Event : ContentItem
{
    public Event()
    {
        CollectionType = "events";
    }

    /// <summary>
    /// Event start date/time (required).
    /// </summary>
    public required DateTimeOffset StartDate { get; init; }

    /// <summary>
    /// Event end date/time (optional).
    /// </summary>
    public DateTimeOffset? EndDate { get; init; }

    /// <summary>
    /// Event location or "Online" (required).
    /// </summary>
    public required string Location { get; init; }

    /// <summary>
    /// Event registration link (optional).
    /// </summary>
    public string? RegistrationUrl { get; set; }

    /// <summary>
    /// Validates the event including event-specific rules.
    /// </summary>
    public override void Validate()
    {
        base.Validate();

        if (string.IsNullOrWhiteSpace(Location))
            throw new ArgumentException("Location cannot be empty", nameof(Location));

        if (EndDate.HasValue && EndDate.Value < StartDate)
            throw new ArgumentException("EndDate cannot be before StartDate", nameof(EndDate));
    }
}
