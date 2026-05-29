using TechHub.Core.Models;

namespace TechHub.Core.Interfaces;

public interface INewsletterService
{
    Task<bool> SendRoundupNewsletterAsync(string roundupSlug, CancellationToken ct = default);
    Task<bool> SendCombinedWeeklyAsync(IReadOnlyList<string> roundupSlugs, CancellationToken ct = default);
    Task<bool> SendTestEmailAsync(string roundupSlug, string recipientEmail, CancellationToken ct = default);
    Task<bool> SendTestWeeklyAsync(IReadOnlyList<string> sections, string recipientEmail, CancellationToken ct = default);
    Task<bool> SendTestDailyEmailAsync(IReadOnlyList<string> sections, string recipientEmail, CancellationToken ct = default);
    Task<bool> SendDailyOverviewAsync(DateOnly day, CancellationToken ct = default);
    Task<bool> SendAdminStatusReportAsync(DateOnly day, CancellationToken ct = default);
    Task<bool> SendConfirmationEmailAsync(string email, CancellationToken ct = default);
    Task<bool> ConfirmSubscriberAsync(string email, string token, CancellationToken ct = default);
    Task<bool> SendManageLinkEmailAsync(string email, CancellationToken ct = default);
    Task<NewsletterSubscriber?> GetSubscriberPreferencesAsync(string email, string token, CancellationToken ct = default);
    Task<bool> UpdateSubscriberPreferencesAsync(string email, string token, string? displayName, IReadOnlyList<string> weeklySections, IReadOnlyList<string> dailySections, CancellationToken ct = default);
}
