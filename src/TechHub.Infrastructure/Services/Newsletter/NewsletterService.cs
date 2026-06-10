using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;
using TechHub.Core.Models.Admin;
using TechHub.Infrastructure.Services.RoundupGeneration;

namespace TechHub.Infrastructure.Services.Newsletter;

[SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "Newsletter content is locale-agnostic plain text/HTML.")]
[SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance", Justification = "Interface types keep method contracts simpler.")]
public sealed class NewsletterService : INewsletterService
{
    private readonly IDbConnection _connection;
    private readonly INewsletterSubscriberRepository _subscriberRepository;
    private readonly IContentRepository _contentRepository;
    private readonly NewsletterOptions _options;
    private readonly AppSettings _appSettings;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<NewsletterService> _logger;
    private readonly NewsletterTemplateProvider _templates;

    public NewsletterService(
        IDbConnection connection,
        INewsletterSubscriberRepository subscriberRepository,
        IContentRepository contentRepository,
        IOptions<NewsletterOptions> options,
        IOptions<AppSettings> appSettings,
        IEmailSender emailSender,
        ILogger<NewsletterService> logger,
        NewsletterTemplateProvider templates)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(subscriberRepository);
        ArgumentNullException.ThrowIfNull(contentRepository);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(appSettings);
        ArgumentNullException.ThrowIfNull(emailSender);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(templates);

        _connection = connection;
        _subscriberRepository = subscriberRepository;
        _contentRepository = contentRepository;
        _options = options.Value;
        _appSettings = appSettings.Value;
        _emailSender = emailSender;
        _logger = logger;
        _templates = templates;
    }

    public async Task<bool> SendCombinedWeeklyAsync(IReadOnlyList<string> roundupSlugs, string? sendTargetKey = null, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(roundupSlugs);
        if (roundupSlugs.Count == 0)
        {
            return false;
        }

        // Load roundup content for each slug
        var roundups = new List<RoundupRow>(roundupSlugs.Count);
        foreach (var slug in roundupSlugs)
        {
            var roundup = await GetRoundupBySlugAsync(slug, ct);
            if (roundup is not null)
            {
                roundups.Add(roundup);
            }
        }

        if (roundups.Count == 0)
        {
            return false;
        }

        var targetKey = string.IsNullOrWhiteSpace(sendTargetKey)
            ? $"batch:{string.Join(",", roundups.Select(r => r.Slug).OrderBy(s => s, StringComparer.OrdinalIgnoreCase))}"
            : sendTargetKey.Trim();

        if (await _subscriberRepository.HasBeenSentAsync("weekly-roundup", targetKey, ct))
        {
            return false;
        }

        if (!IsUnsubscribeSecretConfigured("combined-weekly"))
        {
            await _subscriberRepository.LogSendAsync("weekly-roundup", targetKey, 0, 0, "failed", "Unsubscribe secret is not configured", ct);
            return false;
        }

        // Collect all weekly subscribers across the relevant sections, deduplicated by email
        var allSubscribers = new Dictionary<string, NewsletterSubscriber>(StringComparer.OrdinalIgnoreCase);
        foreach (var roundup in roundups)
        {
            var sectionSubscribers = await _subscriberRepository.GetActiveSubscribersAsync(roundup.SectionName, weekly: true, ct);
            foreach (var sub in sectionSubscribers)
            {
                allSubscribers.TryAdd(sub.Email, sub);
            }
        }

        var roundupsBySection = roundups.ToDictionary(r => r.SectionName, StringComparer.OrdinalIgnoreCase);

        var attempted = 0;
        var successful = 0;

        foreach (var subscriber in allSubscribers.Values)
        {
            // Find the roundups this subscriber cares about (intersection of their sections and new roundups)
            var relevantRoundups = subscriber.WeeklySections
                .Where(s => roundupsBySection.ContainsKey(s))
                .Select(s => roundupsBySection[s])
                .ToList();
            relevantRoundups = OrderRoundupsByWebsiteOrder(relevantRoundups);

            if (relevantRoundups.Count == 0)
            {
                continue;
            }

            attempted++;

            try
            {
                var unsubscribeUrl = BuildUnsubscribeUrl(subscriber.Email);
                var manageUrl = BuildManageUrl(subscriber.Email, _options.UnsubscribeSecret);
                var subject = relevantRoundups.Count == 1
                    ? relevantRoundups[0].Title
                    : $"TechHub Weekly Digest — {string.Join(", ", relevantRoundups.Select(r => GetSectionTitle(r.SectionName)))}";
                var html = RenderCombinedWeeklyHtml(relevantRoundups, unsubscribeUrl, manageUrl);
                var text = BuildCombinedWeeklyPlainText(relevantRoundups, unsubscribeUrl, manageUrl);

                var emailSent = await SendEmailAsync(subscriber.Email, subject, html, text, ct);
                if (emailSent)
                {
                    successful++;
                }

                await _subscriberRepository.RecordSubscriberSendAsync(subscriber.Email, isWeekly: true, succeeded: emailSent, ct);
            }
#pragma warning disable CA1031 // Best-effort: continue with other subscribers if one fails
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogWarning(ex, "Failed sending weekly newsletter to subscriber — skipping");
                await _subscriberRepository.RecordSubscriberSendAsync(subscriber.Email, isWeekly: true, succeeded: false, CancellationToken.None);
            }
#pragma warning restore CA1031
        }

        var sendStatus = attempted == 0 || successful == attempted ? "sent" : successful > 0 ? "partial" : "failed";
        var sendError = attempted == 0 || successful == attempted
            ? null
            : $"Delivered to {successful} of {attempted} subscribers";
        await _subscriberRepository.LogSendAsync("weekly-roundup", targetKey, successful, attempted - successful, sendStatus, sendError, ct);
        return attempted == 0 || successful > 0;
    }

    public async Task<bool> SendTestWeeklyAsync(IReadOnlyList<string> sections, string recipientEmail, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(sections);
        ArgumentNullException.ThrowIfNull(recipientEmail);

        if (sections.Count == 0)
        {
            return false;
        }

        if (!IsUnsubscribeSecretConfigured("test-weekly"))
        {
            return false;
        }

        const string Sql = """
            SELECT slug
            FROM content_items
            WHERE collection_name = 'roundups'
              AND primary_section_name = @Section
            ORDER BY date_epoch DESC
            LIMIT 1
            """;

        // Load the latest roundup for each requested section
        var roundups = new List<RoundupRow>(sections.Count);
        foreach (var section in sections)
        {
            var slug = await _connection.ExecuteScalarAsync<string?>(new CommandDefinition(Sql, new { Section = section }, cancellationToken: ct));
            if (slug is null)
            {
                _logger.LogWarning("Newsletter test weekly: no roundup found for section {Section}", section);
                continue;
            }

            var roundup = await GetRoundupBySlugAsync(slug, ct);
            if (roundup is not null)
            {
                roundups.Add(roundup);
            }
        }

        if (roundups.Count == 0)
        {
            return false;
        }

        roundups = OrderRoundupsByWebsiteOrder(roundups);

        var logKey = $"test-weekly@{DateTime.UtcNow:yyyyMMddHHmmss}";
        var unsubscribeUrl = BuildUnsubscribeUrl(recipientEmail);
        var manageUrl = BuildManageUrl(recipientEmail, _options.UnsubscribeSecret);
        var subject = roundups.Count == 1
            ? $"[Test] {roundups[0].Title}"
            : $"[Test] TechHub Weekly Digest — {string.Join(", ", roundups.Select(r => GetSectionTitle(r.SectionName)))}";
        var html = RenderCombinedWeeklyHtml(roundups, unsubscribeUrl, manageUrl);
        var text = BuildCombinedWeeklyPlainText(roundups, unsubscribeUrl, manageUrl);
        var sent = await SendEmailAsync(recipientEmail, subject, html, text, ct);
        await _subscriberRepository.LogSendAsync(
            "test-send",
            logKey,
            sent ? 1 : 0,
            sent ? 0 : 1,
            sent ? "sent" : "failed",
            sent ? null : "Delivery failed",
            ct);
        return sent;
    }

    public async Task<bool> SendTestDailyEmailAsync(IReadOnlyList<string> sections, string recipientEmail, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(sections);
        ArgumentNullException.ThrowIfNull(recipientEmail);

        if (sections.Count == 0)
        {
            return false;
        }

        if (!IsUnsubscribeSecretConfigured("test-daily"))
        {
            return false;
        }

        TimeZoneInfo testTz;
        try
        {
            testTz = string.IsNullOrWhiteSpace(_options.DailyDigestTimeZoneId)
                ? TimeZoneInfo.Utc
                : TimeZoneInfo.FindSystemTimeZoneById(_options.DailyDigestTimeZoneId);
        }
        catch (Exception ex) when (ex is TimeZoneNotFoundException or InvalidTimeZoneException)
        {
            _logger.LogWarning(ex, "Invalid DailyDigestTimeZoneId '{TimeZoneId}'. Falling back to UTC.", _options.DailyDigestTimeZoneId);
            testTz = TimeZoneInfo.Utc;
        }

        var localNow = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, testTz);
        var yesterday = DateOnly.FromDateTime(localNow.DateTime.Date.AddDays(-1));
        var (start, end) = GetDayUtcWindow(yesterday);
        var sectionNames = OrderSectionsByWebsiteOrder(sections);
        var itemsBySection = await GetDailyItemsBySectionAsync(sectionNames, start, end, ct);
        var html = BuildDailyOverviewHtml(yesterday, sectionNames, itemsBySection, recipientEmail);
        var text = BuildDailyOverviewText(yesterday, sectionNames, itemsBySection, recipientEmail);
        var logKey = $"daily-{yesterday:yyyyMMdd}@{DateTime.UtcNow:yyyyMMddHHmmss}";
        var sent = await SendEmailAsync(recipientEmail, $"[Test] TechHub Daily Overview — {yesterday:yyyy-MM-dd}", html, text, ct);
        await _subscriberRepository.LogSendAsync(
            "test-daily",
            logKey,
            sent ? 1 : 0,
            sent ? 0 : 1,
            sent ? "sent" : "failed",
            sent ? null : "Delivery failed",
            ct);
        return sent;
    }

    public async Task<bool> SendManageLinkEmailAsync(string email, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(email);

        var secret = _options.UnsubscribeSecret;
        if (string.IsNullOrWhiteSpace(secret))
        {
            _logger.LogError("Newsletter unsubscribe secret is not configured; skipping manage link email");
            return false;
        }

        var subscriber = await _subscriberRepository.GetSubscriberByEmailAsync(email, ct);
        if (subscriber is null)
        {
            // Don't reveal whether an email is subscribed — silently succeed
            return true;
        }

        var manageUrl = BuildManageUrl(email, secret);
        var html = BuildAccountActionHtml(
                title: "Manage your TechHub newsletter subscription",
                message: "Click the button below to manage your subscription preferences.",
                actionLabel: "Manage subscription",
                actionUrl: manageUrl);
        var text = $"""
            Manage your TechHub newsletter subscription

            Click the link below to manage your newsletter preferences.

            {manageUrl}
            """;

        return await SendEmailAsync(email, "Manage your TechHub newsletter subscription", html, text, ct);
    }

    public async Task<NewsletterSubscriber?> GetSubscriberPreferencesAsync(string email, string token, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(token);

        var secret = _options.UnsubscribeSecret;
        if (string.IsNullOrWhiteSpace(secret) || !IsValidUnsubscribeToken(email, token, secret))
        {
            return null;
        }

        return await _subscriberRepository.GetSubscriberByEmailAsync(email, ct);
    }

    public async Task<bool> UpdateSubscriberPreferencesAsync(
        string email,
        string token,
        string? displayName,
        IReadOnlyList<string> weeklySections,
        IReadOnlyList<string> dailySections,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(token);
        ArgumentNullException.ThrowIfNull(weeklySections);
        ArgumentNullException.ThrowIfNull(dailySections);

        var secret = _options.UnsubscribeSecret;
        if (string.IsNullOrWhiteSpace(secret) || !IsValidUnsubscribeToken(email, token, secret))
        {
            return false;
        }

        return await _subscriberRepository.UpdateSubscriberByEmailAsync(email, displayName, weeklySections, dailySections, ct);
    }

    public async Task<bool> SendDailyOverviewAsync(DateOnly day, CancellationToken ct = default)
    {
        var targetKey = day.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        const string SendKind = "daily-overview";

        if (await _subscriberRepository.HasBeenSentAsync(SendKind, targetKey, ct))
        {
            return false;
        }

        if (!IsUnsubscribeSecretConfigured("daily-overview"))
        {
            return false;
        }

        var sections = await _contentRepository.GetAllSectionsAsync(ct);
        var sectionNames = sections
            .Where(s => !string.Equals(s.Name, "all", StringComparison.OrdinalIgnoreCase))
            .Select(s => s.Name)
            .ToList();

        var (start, end) = GetDayUtcWindow(day);
        var itemsBySection = await GetDailyItemsBySectionAsync(sectionNames, start, end, ct);

        var dailySubscribers = new List<NewsletterSubscriber>();
        var page = 1;
        const int BatchSize = 500;
        while (true)
        {
            var batch = await _subscriberRepository.GetSubscribersAsync(page: page, pageSize: BatchSize, ct: ct);
            dailySubscribers.AddRange(batch.Where(s => s.IsConfirmed && s.DailySections.Count > 0));
            if (batch.Count < BatchSize)
            {
                break;
            }

            page++;
        }

        if (dailySubscribers.Count == 0)
        {
            await _subscriberRepository.LogSendAsync(SendKind, targetKey, 0, 0, "sent", null, ct);
            return true;
        }

        var sent = 0;
        var eligible = 0;
        foreach (var subscriber in dailySubscribers)
        {
            var selectedSections = subscriber.DailySections
                .Where(section => itemsBySection.ContainsKey(section))
                .ToList();

            if (selectedSections.Count == 0)
            {
                continue; // No news for any of this subscriber's sections today — skip email
            }

            eligible++;
            selectedSections = OrderSectionsByWebsiteOrder(selectedSections);

            var html = BuildDailyOverviewHtml(day, selectedSections, itemsBySection, subscriber.Email);
            var text = BuildDailyOverviewText(day, selectedSections, itemsBySection, subscriber.Email);
            var emailSent = await SendEmailAsync(subscriber.Email, $"TechHub Daily Overview — {targetKey}", html, text, ct);
            if (emailSent)
            {
                sent++;
            }

            await _subscriberRepository.RecordSubscriberSendAsync(subscriber.Email, isWeekly: false, succeeded: emailSent, ct);
        }

        var sendStatus = eligible == 0 || sent == eligible ? "sent" : sent > 0 ? "partial" : "failed";
        var sendError = eligible == 0 || sent == eligible
            ? null
            : sent > 0
                ? $"Delivered to {sent} of {eligible} eligible subscribers"
                : "Delivery failed for all eligible subscribers";
        await _subscriberRepository.LogSendAsync(SendKind, targetKey, sent, eligible - sent, sendStatus, sendError, ct);
        return sent > 0;
    }

    public async Task<bool> SendAdminStatusReportAsync(DateOnly day, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(_options.AdminReportEmailAddress))
        {
            return false;
        }

        var targetKey = day.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        const string SendKind = "admin-status";
        if (await _subscriberRepository.HasBeenSentAsync(SendKind, targetKey, ct))
        {
            return false;
        }

        var stats = await _subscriberRepository.GetDailyReportStatsAsync(ct);

        var hasFailures = stats.FailedProcessedUrlsLast24Hours > 0
            || stats.FailedJobsLast24Hours > 0
            || stats.FailedNewsletterSendsLast24Hours > 0;

        if (!hasFailures)
        {
            return false;
        }

        var html = BuildAdminStatusHtml(day, stats);
        var text = BuildAdminStatusText(day, stats);

        var sent = await SendEmailAsync(_options.AdminReportEmailAddress, $"TechHub Daily Status Report — {targetKey}", html, text, ct);
        await _subscriberRepository.LogSendAsync(SendKind, targetKey, sent ? 1 : 0, sent ? 0 : 1, sent ? "sent" : "failed", sent ? null : "Unable to send admin status report", ct);
        return sent;
    }

    public async Task<bool> SendConfirmationEmailAsync(string email, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(email);

        if (string.IsNullOrWhiteSpace(_options.UnsubscribeSecret))
        {
            _logger.LogError("Newsletter unsubscribe secret is not configured; skipping confirmation email");
            return false;
        }

        var confirmUrl = BuildConfirmUrl(email);
        var html = BuildAccountActionHtml(
                title: "Confirm your TechHub newsletter subscription",
                message: "Click the button below to confirm your subscription. If you did not sign up, you can safely ignore this email.",
                actionLabel: "Confirm subscription",
                actionUrl: confirmUrl);
        var text = $"""
            Confirm your TechHub newsletter subscription

            Click the link below to confirm your subscription.
            If you did not sign up, you can safely ignore this email.

            {confirmUrl}
            """;

        return await SendEmailAsync(email, "Confirm your TechHub newsletter subscription", html, text, ct);
    }

    public async Task<ConfirmSubscriptionResult> ConfirmSubscriberAsync(string email, string token, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(token);

        var secret = _options.UnsubscribeSecret;
        if (string.IsNullOrWhiteSpace(secret))
        {
            return ConfirmSubscriptionResult.InvalidToken;
        }

        if (!IsValidConfirmToken(email, token, secret))
        {
            return ConfirmSubscriptionResult.InvalidToken;
        }

        return await _subscriberRepository.ConfirmSubscriberAsync(email, ct);
    }

    public static string BuildUnsubscribeToken(string email, string secret)
    {
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(secret);
        if (string.IsNullOrWhiteSpace(secret))
        {
            throw new ArgumentException("Newsletter unsubscribe secret is required.", nameof(secret));
        }

        var normalizedEmail = email.Trim().ToLowerInvariant();
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(normalizedEmail));
        return WebEncoders.Base64UrlEncode(hash);
    }

    public static string BuildConfirmToken(string email, string secret)
    {
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(secret);
        if (string.IsNullOrWhiteSpace(secret))
        {
            throw new ArgumentException("Newsletter secret is required.", nameof(secret));
        }

        var normalizedEmail = email.Trim().ToLowerInvariant();
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes("confirm:" + normalizedEmail));
        return WebEncoders.Base64UrlEncode(hash);
    }

    public static bool IsValidConfirmToken(string email, string token, string secret)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return false;
        }

        try
        {
            var expectedToken = BuildConfirmToken(email, secret);
            var expectedBytes = WebEncoders.Base64UrlDecode(expectedToken);
            var tokenBytes = WebEncoders.Base64UrlDecode(token);
            if (expectedBytes.Length != tokenBytes.Length)
            {
                return false;
            }

            return CryptographicOperations.FixedTimeEquals(expectedBytes, tokenBytes);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public static bool IsValidUnsubscribeToken(string email, string token, string secret)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return false;
        }

        try
        {
            var expectedToken = BuildUnsubscribeToken(email, secret);
            var expectedBytes = WebEncoders.Base64UrlDecode(expectedToken);
            var tokenBytes = WebEncoders.Base64UrlDecode(token);
            if (expectedBytes.Length != tokenBytes.Length)
            {
                return false;
            }

            return CryptographicOperations.FixedTimeEquals(expectedBytes, tokenBytes);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    private Task<bool> SendEmailAsync(string recipientEmail, string subject, string html, string plainText, CancellationToken ct) =>
        _emailSender.SendAsync(recipientEmail, subject, html, plainText, ct);

    private async Task<RoundupRow?> GetRoundupBySlugAsync(string roundupSlug, CancellationToken ct)
    {
        const string Sql = """
            SELECT
                slug AS Slug,
                title AS Title,
                excerpt AS Introduction,
                content AS Content,
                primary_section_name AS SectionName
            FROM content_items
            WHERE collection_name = 'roundups'
              AND slug = @Slug
            LIMIT 1
            """;

        return await _connection.QuerySingleOrDefaultAsync<RoundupRow>(new CommandDefinition(
            Sql,
            new { Slug = roundupSlug.Trim() },
            cancellationToken: ct));
    }

    private async Task<IReadOnlyDictionary<string, IReadOnlyList<DailyItemRow>>> GetDailyItemsBySectionAsync(
        IReadOnlyList<string> sectionNames,
        DateTime startUtc,
        DateTime endUtc,
        CancellationToken ct)
    {
        var result = sectionNames.ToDictionary(
            sectionName => sectionName,
            _ => (IReadOnlyList<DailyItemRow>)[],
            StringComparer.OrdinalIgnoreCase);

        if (sectionNames.Count == 0)
        {
            return result;
        }

        const string Sql = """
            WITH ranked AS (
                SELECT
                    slug AS Slug,
                    title AS Title,
                    collection_name AS CollectionName,
                    primary_section_name AS SectionName,
                    ROW_NUMBER() OVER (
                        PARTITION BY primary_section_name
                        ORDER BY created_at DESC
                    ) AS RowNumber
                FROM content_items
                WHERE primary_section_name = ANY(@SectionNames)
                  AND created_at >= @StartUtc
                  AND created_at < @EndUtc
                  AND collection_name <> 'roundups'
            )
            SELECT
                Slug,
                Title,
                CollectionName,
                SectionName
            FROM ranked
            WHERE RowNumber <= @PerSectionLimit
            """;

        var rows = await _connection.QueryAsync<DailyItemBySectionRow>(new CommandDefinition(
            Sql,
            new
            {
                SectionNames = sectionNames,
                StartUtc = startUtc,
                EndUtc = endUtc,
                PerSectionLimit = 100
            },
            cancellationToken: ct));

        foreach (var group in rows.GroupBy(row => row.SectionName, StringComparer.OrdinalIgnoreCase))
        {
            result[group.Key] = group
                .Select(row => new DailyItemRow
                {
                    Slug = row.Slug,
                    Title = row.Title,
                    CollectionName = row.CollectionName
                })
                .OrderBy(row => row.Title, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        return result;
    }

    private IEnumerable<object> GetRoundupLinks(RoundupRow roundup)
    {
        var baseRoundupUrl = BuildAbsoluteUrl($"/{roundup.SectionName}/roundups/{roundup.Slug}");
        return roundup.Content
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Where(line => line.StartsWith("## ", StringComparison.Ordinal))
            .Select(line => line[3..].Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Select(header => (object)new
            {
                href = $"{baseRoundupUrl}#{RoundupContentBuilder.BuildAnchor(header)}",
                text = header
            });
    }

    private string RenderCombinedWeeklyHtml(IReadOnlyList<RoundupRow> roundups, string unsubscribeUrl, string manageUrl)
    {
        var footer = _templates.SubscriberFooter(new { manageUrl, unsubscribeUrl });
        var content = _templates.WeeklyContent(new
        {
            roundups = roundups.Select(r => new
            {
                sectionTitle = GetSectionTitle(r.SectionName),
                sectionTag = GetSectionTag(r.SectionName),
                title = StripRoundupPrefix(r.Title),
                introduction = r.Introduction,
                roundupUrl = BuildAbsoluteUrl($"/{r.SectionName}/roundups/{r.Slug}"),
                links = GetRoundupLinks(r)
            }),
            footer
        });
        return _templates.Shell(new
        {
            title = "TechHub Weekly Digest",
            cardClass = "th-card--weekly",
            maxWidth = "900px",
            content
        });
    }

    private string BuildCombinedWeeklyPlainText(IReadOnlyList<RoundupRow> roundups, string unsubscribeUrl, string manageUrl)
    {
        var sb = new StringBuilder();
        sb.AppendLine("TechHub Weekly Digest");
        sb.AppendLine(new string('=', 40));
        sb.AppendLine();

        foreach (var roundup in roundups)
        {
            var sectionTitle = GetSectionTitle(roundup.SectionName);
            var fullRoundupUrl = BuildAbsoluteUrl($"/{roundup.SectionName}/roundups/{roundup.Slug}");
            sb.AppendLine(sectionTitle);
            sb.AppendLine(new string('-', sectionTitle.Length));
            sb.AppendLine(StripRoundupPrefix(roundup.Title));
            sb.AppendLine();
            sb.AppendLine(roundup.Introduction);
            sb.AppendLine();
            sb.Append("Read the full roundup: ");
            sb.AppendLine(fullRoundupUrl);
            sb.AppendLine();
        }

        sb.AppendLine($"Manage subscription: {manageUrl}");
        sb.AppendLine($"Unsubscribe: {unsubscribeUrl}");
        return sb.ToString();
    }

    private string GetSectionTitle(string slug) =>
        _appSettings.Content.Sections.TryGetValue(slug, out var config) ? config.Title : slug;

    private string GetSectionTag(string slug) =>
        _appSettings.Content.Sections.TryGetValue(slug, out var config) ? config.Tag : slug;

    private static string StripRoundupPrefix(string title)
    {
        var colonIndex = title.IndexOf(": ", StringComparison.Ordinal);
        return colonIndex >= 0 ? title[(colonIndex + 2)..] : title;
    }

    private List<string> OrderSectionsByWebsiteOrder(IEnumerable<string> sectionNames)
    {
        var sectionOrder = _appSettings.Content.Sections
            .Where(kvp => !string.Equals(kvp.Key, "all", StringComparison.OrdinalIgnoreCase))
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Order,
                StringComparer.OrdinalIgnoreCase);

        return sectionNames
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Trim().ToLowerInvariant())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(s => sectionOrder.TryGetValue(s, out var order) ? order : int.MaxValue)
            .ThenBy(s => GetSectionTitle(s), StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private List<RoundupRow> OrderRoundupsByWebsiteOrder(IEnumerable<RoundupRow> roundups)
    {
        var orderedSections = OrderSectionsByWebsiteOrder(roundups.Select(r => r.SectionName));
        var sectionIndex = orderedSections
            .Select((section, index) => new { section, index })
            .ToDictionary(x => x.section, x => x.index, StringComparer.OrdinalIgnoreCase);

        return roundups
            .OrderBy(r => sectionIndex.TryGetValue(r.SectionName, out var index) ? index : int.MaxValue)
            .ThenBy(r => GetSectionTitle(r.SectionName), StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private string BuildDailyOverviewHtml(
        DateOnly day,
        IReadOnlyList<string> sections,
        IReadOnlyDictionary<string, IReadOnlyList<DailyItemRow>> itemsBySection,
        string email)
    {
        var unsubscribeUrl = BuildUnsubscribeUrl(email);
        var manageUrl = BuildManageUrl(email, _options.UnsubscribeSecret);
        var footer = _templates.SubscriberFooter(new { manageUrl, unsubscribeUrl });
        var content = _templates.DailyContent(new
        {
            date = day.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            sections = sections
                .Where(s => itemsBySection.TryGetValue(s, out var rows) && rows.Count > 0)
                .Select(s => new
                {
                    title = GetSectionTitle(s),
                    items = itemsBySection[s].Select(item => new
                    {
                        url = BuildAbsoluteUrl($"/{s}/all?search={Uri.EscapeDataString(item.Title)}&exact=true"),
                        title = item.Title,
                        collectionName = item.CollectionName
                    })
                }),
            footer
        });
        return _templates.Shell(new
        {
            title = "TechHub Daily Overview",
            cardClass = "th-card--daily",
            maxWidth = "1100px",
            content
        });
    }

    private string BuildDailyOverviewText(
        DateOnly day,
        IReadOnlyList<string> sections,
        IReadOnlyDictionary<string, IReadOnlyList<DailyItemRow>> itemsBySection,
        string email)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"TechHub Daily Overview — {day:yyyy-MM-dd}");
        sb.AppendLine();

        foreach (var section in sections)
        {
            var items = itemsBySection.TryGetValue(section, out var rows) ? rows : [];
            if (items.Count == 0)
            {
                continue; // Section has no news today — skip it
            }

            sb.AppendLine(GetSectionTitle(section));
            foreach (var item in items)
            {
                var filteredUrl = BuildAbsoluteUrl($"/{section}/all?search={Uri.EscapeDataString(item.Title)}&exact=true");
                sb.AppendLine($"- {item.Title} ({item.CollectionName})");
                sb.AppendLine($"  {filteredUrl}");
            }

            sb.AppendLine();
        }

        sb.AppendLine($"Manage subscription: {BuildManageUrl(email, _options.UnsubscribeSecret)}");
        sb.AppendLine($"Unsubscribe: {BuildUnsubscribeUrl(email)}");
        return sb.ToString();
    }

    private string BuildAdminStatusHtml(DateOnly day, NewsletterDailyReportStats stats)
    {
        var content = _templates.AdminContent(new
        {
            newContentItems = stats.NewContentItemsLast24Hours.ToString(CultureInfo.InvariantCulture),
            failedProcessedUrls = stats.FailedProcessedUrlsLast24Hours.ToString(CultureInfo.InvariantCulture),
            failedJobs = stats.FailedJobsLast24Hours.ToString(CultureInfo.InvariantCulture),
            failedNewsletterSends = stats.FailedNewsletterSendsLast24Hours.ToString(CultureInfo.InvariantCulture),
            newSubscribers = stats.NewSubscribersLast24Hours.ToString(CultureInfo.InvariantCulture),
            activeSubscribers = stats.ActiveSubscribers.ToString(CultureInfo.InvariantCulture),
            unconfirmedSubscribers = stats.UnconfirmedSubscribers.ToString(CultureInfo.InvariantCulture)
        });
        return _templates.Shell(new
        {
            title = $"TechHub Daily Status Report \u2014 {day.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}",
            cardClass = "th-card--admin",
            maxWidth = "600px",
            content
        });
    }

    private string BuildAccountActionHtml(string title, string message, string actionLabel, string actionUrl)
    {
        var content = _templates.AccountAction(new { message, actionLabel, actionUrl });
        return _templates.Shell(new
        {
            title,
            cardClass = "th-card--account",
            maxWidth = "640px",
            content
        });
    }

    private static string BuildAdminStatusText(DateOnly day, NewsletterDailyReportStats stats) =>
        string.Create(CultureInfo.InvariantCulture, $"""
            TechHub Daily Status Report — {day:yyyy-MM-dd}

            New content items (24h): {stats.NewContentItemsLast24Hours}
            Failed processed URLs (24h): {stats.FailedProcessedUrlsLast24Hours}
            Failed background jobs (24h): {stats.FailedJobsLast24Hours}
            Failed newsletter sends (24h): {stats.FailedNewsletterSendsLast24Hours}
            New newsletter subscribers (24h): {stats.NewSubscribersLast24Hours}
            Active subscribers: {stats.ActiveSubscribers}
            Unconfirmed subscribers: {stats.UnconfirmedSubscribers}
            """);

    private string BuildAbsoluteUrl(string pathOrAbsolute)
    {
        if (Uri.TryCreate(pathOrAbsolute, UriKind.Absolute, out var absolute) &&
            (absolute.Scheme == Uri.UriSchemeHttp || absolute.Scheme == Uri.UriSchemeHttps))
        {
            return absolute.ToString();
        }

        var baseUrl = _options.WebsiteBaseUrl.TrimEnd('/');
        return string.IsNullOrWhiteSpace(baseUrl) ? pathOrAbsolute : $"{baseUrl}{pathOrAbsolute}";
    }

    private string BuildUnsubscribeUrl(string email)
    {
        var token = BuildUnsubscribeToken(email, _options.UnsubscribeSecret);
        return BuildAbsoluteUrl($"/newsletter/unsubscribe?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}");
    }

    private string BuildManageUrl(string email, string secret)
    {
        var token = BuildUnsubscribeToken(email, secret);
        return BuildAbsoluteUrl($"/newsletter/manage?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}");
    }

    private string BuildConfirmUrl(string email)
    {
        var token = BuildConfirmToken(email, _options.UnsubscribeSecret);
        return BuildAbsoluteUrl($"/newsletter/confirm?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}");
    }

    private bool IsUnsubscribeSecretConfigured(string operationName)
    {
        if (!string.IsNullOrWhiteSpace(_options.UnsubscribeSecret))
        {
            return true;
        }

        _logger.LogError("Newsletter unsubscribe secret is not configured; skipping {OperationName}", operationName);
        return false;
    }

    private sealed class RoundupRow
    {
        public string Slug { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string Introduction { get; init; } = string.Empty;
        public string Content { get; init; } = string.Empty;
        public string SectionName { get; init; } = string.Empty;
    }

    private sealed class DailyItemRow
    {
        public string Slug { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string CollectionName { get; init; } = string.Empty;
    }

    private sealed class DailyItemBySectionRow
    {
        public string Slug { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string CollectionName { get; init; } = string.Empty;
        public string SectionName { get; init; } = string.Empty;
    }

    private (DateTime startUtc, DateTime endUtc) GetDayUtcWindow(DateOnly day)
    {
        TimeZoneInfo tz;
        try
        {
            tz = string.IsNullOrWhiteSpace(_options.DailyDigestTimeZoneId)
                ? TimeZoneInfo.Utc
                : TimeZoneInfo.FindSystemTimeZoneById(_options.DailyDigestTimeZoneId);
        }
        catch (Exception ex) when (ex is TimeZoneNotFoundException or InvalidTimeZoneException)
        {
            _logger.LogWarning(
                ex,
                "Invalid DailyDigestTimeZoneId '{TimeZoneId}'. Falling back to UTC.",
                _options.DailyDigestTimeZoneId);
            tz = TimeZoneInfo.Utc;
        }

        var localStart = day.ToDateTime(TimeOnly.MinValue);
        var localEnd = day.AddDays(1).ToDateTime(TimeOnly.MinValue);
        return (TimeZoneInfo.ConvertTimeToUtc(localStart, tz), TimeZoneInfo.ConvertTimeToUtc(localEnd, tz));
    }
}

internal static class WebEncoders
{
    public static string Base64UrlEncode(byte[] data)
    {
        var base64 = Convert.ToBase64String(data);
        return base64.TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }

    public static byte[] Base64UrlDecode(string value)
    {
        var s = value.Replace('-', '+').Replace('_', '/');
        s = (s.Length % 4) switch
        {
            2 => s + "==",
            3 => s + "=",
            _ => s
        };
        return Convert.FromBase64String(s);
    }
}
