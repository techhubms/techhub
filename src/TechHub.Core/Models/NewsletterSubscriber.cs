namespace TechHub.Core.Models;

public sealed class NewsletterSubscriber
{
    public long Id { get; init; }
    public required string Email { get; init; }
    public string? DisplayName { get; init; }
    public bool IsConfirmed { get; init; }
    public DateTimeOffset SubscribedAt { get; init; }
    public DateTimeOffset? ConfirmedAt { get; init; }
    public IReadOnlyList<string> WeeklySections { get; init; } = [];
    public IReadOnlyList<string> DailySections { get; init; } = [];
    public DateTimeOffset? LastDailySentAt { get; init; }
    public bool? LastDailySucceeded { get; init; }
    public DateTimeOffset? LastWeeklySentAt { get; init; }
    public bool? LastWeeklySucceeded { get; init; }
}
