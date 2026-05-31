namespace TechHub.Core.Configuration;

public sealed class NewsletterOptions
{
    public const string SectionName = "Newsletter";

    /// <summary>
    /// Whether scheduled newsletter sending is enabled. Defaults to false.
    /// Set to <c>true</c> only in production via <c>appsettings.Production.json</c>.
    /// </summary>
    public bool ScheduledSendEnabled { get; init; }
    public string Endpoint { get; init; } = string.Empty;
    public string SenderAddress { get; init; } = string.Empty;
    public string WebsiteBaseUrl { get; init; } = string.Empty;
    public int CheckIntervalMinutes { get; init; } = 30;
    public string UnsubscribeSecret { get; init; } = string.Empty;
    public string AdminReportEmailAddress { get; init; } = string.Empty;
    public int DailyDigestHourLocal { get; init; } = 9;
    public string DailyDigestTimeZoneId { get; init; } = "Europe/Brussels";
    public int SendDelayMs { get; init; } = 200;
}
