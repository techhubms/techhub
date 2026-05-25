namespace TechHub.Core.Interfaces;

public interface INewsletterService
{
    Task<bool> SendRoundupNewsletterAsync(string roundupSlug, CancellationToken ct = default);
    Task<bool> SendTestEmailAsync(string roundupSlug, string recipientEmail, CancellationToken ct = default);
    Task<bool> SendDailyOverviewAsync(DateOnly day, CancellationToken ct = default);
    Task<bool> SendAdminStatusReportAsync(DateOnly day, CancellationToken ct = default);
}
