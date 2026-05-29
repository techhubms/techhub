namespace TechHub.Core.Models.Admin;

public sealed class NewsletterSendLogEntry
{
    public long Id { get; init; }
    public string SendKind { get; init; } = string.Empty;
    public string TargetKey { get; init; } = string.Empty;
    public DateTimeOffset SentAt { get; init; }
    public int RecipientCount { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? ErrorMessage { get; init; }
}
