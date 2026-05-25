namespace TechHub.Core.Configuration;

public sealed class NewsletterOptions
{
    public const string SectionName = "Newsletter";

    public bool ScheduledSendEnabled { get; init; }
    public string ConnectionString { get; init; } = string.Empty;
    public string SenderAddress { get; init; } = string.Empty;
    public string SenderDisplayName { get; init; } = "TechHub Weekly Roundup";
    public string WebsiteBaseUrl { get; init; } = string.Empty;
    public int CheckIntervalMinutes { get; init; } = 30;
    public string UnsubscribeSecret { get; init; } = string.Empty;
    public string AdminReportEmailAddress { get; init; } = string.Empty;
    public int DailyDigestHourLocal { get; init; } = 9;
    public string DailyDigestTimeZoneId { get; init; } = "Europe/Brussels";
    public int SendDelayMs { get; init; } = 200;
}
