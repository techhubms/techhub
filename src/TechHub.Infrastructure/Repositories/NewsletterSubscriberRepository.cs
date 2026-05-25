using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Dapper;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;
using TechHub.Core.Models.Admin;

namespace TechHub.Infrastructure.Repositories;

[SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance", Justification = "Repository exposes interface-based contracts.")]
public sealed class NewsletterSubscriberRepository : INewsletterSubscriberRepository
{
    private readonly IDbConnection _connection;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public NewsletterSubscriberRepository(IDbConnection connection)
    {
        ArgumentNullException.ThrowIfNull(connection);
        _connection = connection;
    }

    public async Task<long> UpsertSubscriberAsync(
        string email,
        string? displayName,
        IReadOnlyList<string> weeklySections,
        IReadOnlyList<string> dailySections,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(email);

        var normalizedEmail = email.Trim().ToLowerInvariant();
        var preferences = SerializePreferences(weeklySections, dailySections);

        const string Sql = """
            INSERT INTO newsletter_subscribers
                (email, display_name, is_confirmed, confirmed_at, preferences, unsubscribed_at)
            VALUES
                (@Email, @DisplayName, TRUE, NOW(), CAST(@Preferences AS jsonb), NULL)
            ON CONFLICT (lower(email)) WHERE unsubscribed_at IS NULL
            DO UPDATE SET
                display_name = EXCLUDED.display_name,
                is_confirmed = TRUE,
                confirmed_at = COALESCE(newsletter_subscribers.confirmed_at, NOW()),
                preferences = EXCLUDED.preferences,
                unsubscribed_at = NULL
            RETURNING id
            """;

        return await _connection.ExecuteScalarAsync<long>(new CommandDefinition(
            Sql,
            new
            {
                Email = normalizedEmail,
                DisplayName = string.IsNullOrWhiteSpace(displayName) ? null : displayName.Trim(),
                Preferences = preferences
            },
            cancellationToken: ct));
    }

    public async Task<bool> UnsubscribeAsync(string email, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(email);

        const string Sql = """
            UPDATE newsletter_subscribers
            SET unsubscribed_at = NOW()
            WHERE lower(email) = lower(@Email)
              AND unsubscribed_at IS NULL
            """;

        var affected = await _connection.ExecuteAsync(new CommandDefinition(
            Sql,
            new { Email = email.Trim() },
            cancellationToken: ct));

        return affected > 0;
    }

    public async Task<IReadOnlyList<NewsletterSubscriber>> GetActiveSubscribersAsync(
        string sectionName,
        bool weekly,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(sectionName);

        const string Sql = """
            SELECT
                id AS Id,
                email AS Email,
                display_name AS DisplayName,
                is_confirmed AS IsConfirmed,
                subscribed_at AS SubscribedAt,
                confirmed_at AS ConfirmedAt,
                preferences::text AS Preferences
            FROM newsletter_subscribers
            WHERE is_confirmed = TRUE
              AND unsubscribed_at IS NULL
              AND COALESCE((preferences -> @PreferenceNode) ? @SectionName, FALSE)
            ORDER BY subscribed_at ASC
            """;

        var rows = await _connection.QueryAsync<SubscriberRow>(new CommandDefinition(
            Sql,
            new
            {
                PreferenceNode = weekly ? "weeklySections" : "dailySections",
                SectionName = sectionName.Trim().ToLowerInvariant()
            },
            cancellationToken: ct));

        return rows.Select(MapSubscriber).ToList();
    }

    public async Task<IReadOnlyList<NewsletterSubscriber>> GetSubscribersAsync(
        int page = 1,
        int pageSize = 200,
        string? search = null,
        bool? isConfirmed = null,
        CancellationToken ct = default)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 500);

        var whereParts = new List<string> { "unsubscribed_at IS NULL" };
        var parameters = new DynamicParameters();
        if (!string.IsNullOrWhiteSpace(search))
        {
            whereParts.Add("(email ILIKE @Search OR display_name ILIKE @Search)");
            parameters.Add("Search", $"%{search.Trim()}%");
        }

        if (isConfirmed.HasValue)
        {
            whereParts.Add("is_confirmed = @IsConfirmed");
            parameters.Add("IsConfirmed", isConfirmed.Value);
        }

        parameters.Add("Limit", pageSize);
        parameters.Add("Offset", (page - 1) * pageSize);
        var whereSql = string.Join(" AND ", whereParts);

        var sql = $"""
            SELECT
                id AS Id,
                email AS Email,
                display_name AS DisplayName,
                is_confirmed AS IsConfirmed,
                subscribed_at AS SubscribedAt,
                confirmed_at AS ConfirmedAt,
                preferences::text AS Preferences
            FROM newsletter_subscribers
            WHERE {whereSql}
            ORDER BY subscribed_at DESC, id DESC
            LIMIT @Limit OFFSET @Offset
            """;

        var rows = await _connection.QueryAsync<SubscriberRow>(new CommandDefinition(
            sql,
            parameters,
            cancellationToken: ct));

        return rows.Select(MapSubscriber).ToList();
    }

    public async Task<bool> UpdateSubscriberAsync(
        long id,
        string? displayName,
        IReadOnlyList<string> weeklySections,
        IReadOnlyList<string> dailySections,
        CancellationToken ct = default)
    {
        const string Sql = """
            UPDATE newsletter_subscribers
            SET display_name = @DisplayName,
                preferences = CAST(@Preferences AS jsonb)
            WHERE id = @Id
              AND unsubscribed_at IS NULL
            """;

        var affected = await _connection.ExecuteAsync(new CommandDefinition(
            Sql,
            new
            {
                Id = id,
                DisplayName = string.IsNullOrWhiteSpace(displayName) ? null : displayName.Trim(),
                Preferences = SerializePreferences(weeklySections, dailySections)
            },
            cancellationToken: ct));

        return affected > 0;
    }

    public async Task<bool> DeleteSubscriberAsync(long id, CancellationToken ct = default)
    {
        const string Sql = "DELETE FROM newsletter_subscribers WHERE id = @Id";
        var affected = await _connection.ExecuteAsync(new CommandDefinition(
            Sql,
            new { Id = id },
            cancellationToken: ct));
        return affected > 0;
    }

    public async Task<bool> HasBeenSentAsync(string sendKind, string targetKey, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(sendKind);
        ArgumentNullException.ThrowIfNull(targetKey);

        const string Sql = """
            SELECT EXISTS (
                SELECT 1
                FROM newsletter_send_log
                WHERE send_kind = @SendKind
                  AND target_key = @TargetKey
                  AND status = 'sent'
            )
            """;

        return await _connection.ExecuteScalarAsync<bool>(new CommandDefinition(
            Sql,
            new { SendKind = sendKind, TargetKey = targetKey },
            cancellationToken: ct));
    }

    public async Task LogSendAsync(
        string sendKind,
        string targetKey,
        int recipientCount,
        string status,
        string? errorMessage,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(sendKind);
        ArgumentNullException.ThrowIfNull(targetKey);
        ArgumentNullException.ThrowIfNull(status);

        const string Sql = """
            INSERT INTO newsletter_send_log
                (send_kind, target_key, recipient_count, status, error_message)
            VALUES
                (@SendKind, @TargetKey, @RecipientCount, @Status, @Error)
            ON CONFLICT (send_kind, target_key)
            DO UPDATE SET
                sent_at = NOW(),
                recipient_count = EXCLUDED.recipient_count,
                status = EXCLUDED.status,
                error_message = EXCLUDED.error_message
            """;

        await _connection.ExecuteAsync(new CommandDefinition(
            Sql,
            new
            {
                SendKind = sendKind,
                TargetKey = targetKey,
                RecipientCount = recipientCount,
                Status = status,
                Error = errorMessage
            },
            cancellationToken: ct));
    }

    public async Task<IReadOnlyList<NewsletterSendLogEntry>> GetSendLogAsync(int count = 100, CancellationToken ct = default)
    {
        const string Sql = """
            SELECT
                id AS Id,
                send_kind AS SendKind,
                target_key AS TargetKey,
                sent_at AS SentAt,
                recipient_count AS RecipientCount,
                status AS Status,
                error_message AS ErrorMessage
            FROM newsletter_send_log
            ORDER BY sent_at DESC, id DESC
            LIMIT @Count
            """;

        var rows = await _connection.QueryAsync<NewsletterSendLogEntry>(new CommandDefinition(
            Sql,
            new { Count = Math.Clamp(count, 1, 500) },
            cancellationToken: ct));
        return rows.ToList();
    }

    public async Task<NewsletterDailyReportStats> GetDailyReportStatsAsync(CancellationToken ct = default)
    {
        const string Sql = """
            SELECT
                (SELECT COUNT(*) FROM content_items WHERE created_at >= NOW() - INTERVAL '24 hours') AS NewContentItemsLast24Hours,
                (SELECT COUNT(*) FROM processed_urls WHERE status = 'failed' AND updated_at >= NOW() - INTERVAL '24 hours') AS FailedProcessedUrlsLast24Hours,
                (SELECT COUNT(*) FROM content_processing_jobs WHERE status = 'failed' AND started_at >= NOW() - INTERVAL '24 hours') AS FailedJobsLast24Hours,
                (SELECT COUNT(*) FROM newsletter_subscribers WHERE subscribed_at >= NOW() - INTERVAL '24 hours') AS NewSubscribersLast24Hours,
                (SELECT COUNT(*) FROM newsletter_subscribers WHERE is_confirmed = TRUE AND unsubscribed_at IS NULL) AS ActiveSubscribers,
                (SELECT COUNT(*) FROM newsletter_subscribers WHERE is_confirmed = FALSE AND unsubscribed_at IS NULL) AS UnconfirmedSubscribers
            """;

        return await _connection.QuerySingleAsync<NewsletterDailyReportStats>(new CommandDefinition(Sql, cancellationToken: ct));
    }

    private static NewsletterSubscriber MapSubscriber(SubscriberRow row)
    {
        var preferences = DeserializePreferences(row.Preferences);
        return new NewsletterSubscriber
        {
            Id = row.Id,
            Email = row.Email,
            DisplayName = row.DisplayName,
            IsConfirmed = row.IsConfirmed,
            SubscribedAt = row.SubscribedAt,
            ConfirmedAt = row.ConfirmedAt,
            WeeklySections = preferences.WeeklySections,
            DailySections = preferences.DailySections
        };
    }

    private static string SerializePreferences(IReadOnlyList<string> weeklySections, IReadOnlyList<string> dailySections)
    {
        var model = new SubscriberPreferences
        {
            WeeklySections = NormalizeSections(weeklySections),
            DailySections = NormalizeSections(dailySections)
        };

        return JsonSerializer.Serialize(model, _jsonOptions);
    }

    private static SubscriberPreferences DeserializePreferences(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return new SubscriberPreferences();
        }

        try
        {
            var parsed = JsonSerializer.Deserialize<SubscriberPreferences>(json, _jsonOptions);
            return parsed ?? new SubscriberPreferences();
        }
        catch (JsonException)
        {
            return new SubscriberPreferences();
        }
    }

    private static IReadOnlyList<string> NormalizeSections(IReadOnlyList<string> sections) =>
        sections
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Trim().ToLowerInvariant())
            .Where(IsValidSectionName)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(s => s, StringComparer.OrdinalIgnoreCase)
            .ToList();

    private static bool IsValidSectionName(string value) =>
        value.All(c => char.IsLower(c) || char.IsDigit(c) || c == '-');

    private sealed class SubscriberRow
    {
        public long Id { get; init; }
        public string Email { get; init; } = string.Empty;
        public string? DisplayName { get; init; }
        public bool IsConfirmed { get; init; }
        public DateTimeOffset SubscribedAt { get; init; }
        public DateTimeOffset? ConfirmedAt { get; init; }
        public string? Preferences { get; init; }
    }

    private sealed class SubscriberPreferences
    {
        public IReadOnlyList<string> WeeklySections { get; init; } = [];
        public IReadOnlyList<string> DailySections { get; init; } = [];
    }
}
