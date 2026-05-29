namespace TechHub.Core.Models.Admin;

public sealed class NewsletterDailyReportStats
{
    public int NewContentItemsLast24Hours { get; init; }
    public int FailedProcessedUrlsLast24Hours { get; init; }
    public int FailedJobsLast24Hours { get; init; }
    public int NewSubscribersLast24Hours { get; init; }
    public int ActiveSubscribers { get; init; }
    public int UnconfirmedSubscribers { get; init; }
}
