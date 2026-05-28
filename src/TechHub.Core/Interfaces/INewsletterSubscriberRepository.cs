using TechHub.Core.Models;
using TechHub.Core.Models.Admin;

namespace TechHub.Core.Interfaces;

public interface INewsletterSubscriberRepository
{
    Task<(long Id, bool NeedsConfirmation)> UpsertSubscriberAsync(
        string email,
        string? displayName,
        IReadOnlyList<string> weeklySections,
        IReadOnlyList<string> dailySections,
        CancellationToken ct = default);

    Task<bool> UnsubscribeAsync(string email, CancellationToken ct = default);

    Task<bool> ConfirmSubscriberAsync(string email, CancellationToken ct = default);

    Task<IReadOnlyList<NewsletterSubscriber>> GetActiveSubscribersAsync(
        string sectionName,
        bool weekly,
        CancellationToken ct = default);

    Task<IReadOnlyList<NewsletterSubscriber>> GetSubscribersAsync(
        int page = 1,
        int pageSize = 200,
        string? search = null,
        bool? isConfirmed = null,
        CancellationToken ct = default);

    Task<bool> UpdateSubscriberAsync(
        long id,
        string? displayName,
        IReadOnlyList<string> weeklySections,
        IReadOnlyList<string> dailySections,
        CancellationToken ct = default);

    Task<bool> DeleteSubscriberAsync(long id, CancellationToken ct = default);

    Task<bool> HasBeenSentAsync(string sendKind, string targetKey, CancellationToken ct = default);

    Task LogSendAsync(
        string sendKind,
        string targetKey,
        int recipientCount,
        string status,
        string? errorMessage,
        CancellationToken ct = default);

    Task<IReadOnlyList<NewsletterSendLogEntry>> GetSendLogAsync(int count = 100, CancellationToken ct = default);

    Task<NewsletterDailyReportStats> GetDailyReportStatsAsync(CancellationToken ct = default);
}
