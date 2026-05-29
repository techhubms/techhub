using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Reflection;
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
    private readonly string _htmlTemplate;

    public NewsletterService(
        IDbConnection connection,
        INewsletterSubscriberRepository subscriberRepository,
        IContentRepository contentRepository,
        IOptions<NewsletterOptions> options,
        IOptions<AppSettings> appSettings,
        IEmailSender emailSender,
        ILogger<NewsletterService> logger)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(subscriberRepository);
        ArgumentNullException.ThrowIfNull(contentRepository);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(appSettings);
        ArgumentNullException.ThrowIfNull(emailSender);
        ArgumentNullException.ThrowIfNull(logger);

        _connection = connection;
        _subscriberRepository = subscriberRepository;
        _contentRepository = contentRepository;
        _options = options.Value;
        _appSettings = appSettings.Value;
        _emailSender = emailSender;
        _logger = logger;
        _htmlTemplate = LoadTemplate();
    }

    public async Task<bool> SendRoundupNewsletterAsync(string roundupSlug, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(roundupSlug);

        var roundup = await GetRoundupBySlugAsync(roundupSlug, ct);
        if (roundup is null)
        {
            return false;
        }

        const string SendKind = "weekly-roundup";
        if (await _subscriberRepository.HasBeenSentAsync(SendKind, roundupSlug, ct))
        {
            return false;
        }

        var subscribers = await _subscriberRepository.GetActiveSubscribersAsync(roundup.SectionName, weekly: true, ct);
        if (subscribers.Count == 0)
        {
            await _subscriberRepository.LogSendAsync(SendKind, roundupSlug, 0, "sent", null, ct);
            return true;
        }

        try
        {
            var sectionLinksHtml = BuildSectionLinksHtml(roundup);
            var fullRoundupUrl = BuildAbsoluteUrl($"/{roundup.SectionName}/roundups/{roundup.Slug}");
            var successful = 0;

            foreach (var subscriber in subscribers)
            {
                var unsubscribeUrl = BuildUnsubscribeUrl(subscriber.Email);
                var manageUrl = BuildManageUrl(subscriber.Email, _options.UnsubscribeSecret);
                var html = RenderRoundupTemplate(roundup.Title, roundup.Introduction, sectionLinksHtml, fullRoundupUrl, unsubscribeUrl, manageUrl);
                var text = BuildRoundupPlainText(roundup, fullRoundupUrl, unsubscribeUrl, manageUrl);
                if (await SendEmailAsync(subscriber.Email, roundup.Title, html, text, ct))
                {
                    successful++;
                }
            }

            var status = successful == subscribers.Count ? "sent" : successful > 0 ? "partial" : "failed";
            var error = successful == subscribers.Count
                ? null
                : successful > 0
                    ? $"Delivered to {successful} of {subscribers.Count} subscribers"
                    : "Delivery failed for all subscribers";
            await _subscriberRepository.LogSendAsync(SendKind, roundupSlug, successful, status, error, ct);
            return successful > 0;
        }
        catch (Exception ex) when (ex is not OutOfMemoryException and not StackOverflowException)
        {
            await _subscriberRepository.LogSendAsync(SendKind, roundupSlug, 0, "failed", ex.Message, ct);
            _logger.LogError(ex, "Failed sending roundup newsletter for {RoundupSlug}", roundupSlug);
            return false;
        }
    }

    public async Task<bool> SendTestEmailAsync(string roundupSlug, string recipientEmail, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(roundupSlug);
        ArgumentNullException.ThrowIfNull(recipientEmail);

        // Use a timestamp-unique key so each test send always creates a new log row
        // (production sends use the slug alone — tests use slug@timestamp)
        var logKey = $"{roundupSlug}@{DateTime.UtcNow:yyyyMMddHHmmss}";

        var roundup = await GetRoundupBySlugAsync(roundupSlug, ct);
        if (roundup is null)
        {
            await _subscriberRepository.LogSendAsync("test-send", logKey, 0, "failed", $"Roundup '{roundupSlug}' not found", ct);
            return false;
        }

        if (!IsUnsubscribeSecretConfigured("test-email"))
        {
            await _subscriberRepository.LogSendAsync("test-send", logKey, 0, "failed", "Unsubscribe secret is not configured", ct);
            return false;
        }

        var sectionLinksHtml = BuildSectionLinksHtml(roundup);
        var fullRoundupUrl = BuildAbsoluteUrl($"/{roundup.SectionName}/roundups/{roundup.Slug}");
        var unsubscribeUrl = BuildUnsubscribeUrl(recipientEmail);
        var manageUrl = BuildManageUrl(recipientEmail, _options.UnsubscribeSecret);
        var html = RenderRoundupTemplate(roundup.Title, roundup.Introduction, sectionLinksHtml, fullRoundupUrl, unsubscribeUrl, manageUrl);
        var text = BuildRoundupPlainText(roundup, fullRoundupUrl, unsubscribeUrl, manageUrl);
        var sent = await SendEmailAsync(recipientEmail, $"[Test] {roundup.Title}", html, text, ct);
        await _subscriberRepository.LogSendAsync(
            "test-send",
            logKey,
            sent ? 1 : 0,
            sent ? "sent" : "failed",
            sent ? null : $"Delivery failed to {recipientEmail}",
            ct);
        return sent;
    }

    public async Task<bool> SendCombinedWeeklyAsync(IReadOnlyList<string> roundupSlugs, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(roundupSlugs);
        if (roundupSlugs.Count == 0)
        {
            return false;
        }

        if (!IsUnsubscribeSecretConfigured("combined-weekly"))
        {
            foreach (var slug in roundupSlugs)
            {
                await _subscriberRepository.LogSendAsync("weekly-roundup", slug, 0, "failed", "Unsubscribe secret is not configured", ct);
            }

            return false;
        }

        // Skip slugs that have already been sent
        var newSlugs = new List<string>(roundupSlugs.Count);
        foreach (var slug in roundupSlugs)
        {
            if (!await _subscriberRepository.HasBeenSentAsync("weekly-roundup", slug, ct))
            {
                newSlugs.Add(slug);
            }
        }

        if (newSlugs.Count == 0)
        {
            return false;
        }

        // Load roundup content for each new slug
        var roundups = new List<RoundupRow>(newSlugs.Count);
        foreach (var slug in newSlugs)
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

        // Track per-slug delivery counts: (expected recipients, successful sends)
        var slugStats = newSlugs.ToDictionary(s => s, _ => (Expected: 0, Actual: 0), StringComparer.OrdinalIgnoreCase);

        try
        {
            foreach (var subscriber in allSubscribers.Values)
            {
                // Find the roundups this subscriber cares about (intersection of their sections and new roundups)
                var relevantRoundups = subscriber.WeeklySections
                    .Where(s => roundupsBySection.ContainsKey(s))
                    .Select(s => roundupsBySection[s])
                    .ToList();

                if (relevantRoundups.Count == 0)
                {
                    continue;
                }

                foreach (var r in relevantRoundups)
                {
                    slugStats[r.Slug] = (slugStats[r.Slug].Expected + 1, slugStats[r.Slug].Actual);
                }

                var unsubscribeUrl = BuildUnsubscribeUrl(subscriber.Email);
                var manageUrl = BuildManageUrl(subscriber.Email, _options.UnsubscribeSecret);
                var subject = relevantRoundups.Count == 1
                    ? relevantRoundups[0].Title
                    : $"TechHub Weekly Digest — {string.Join(", ", relevantRoundups.Select(r => GetSectionTitle(r.SectionName)))}";
                var html = RenderCombinedWeeklyHtml(relevantRoundups, unsubscribeUrl, manageUrl);
                var text = BuildCombinedWeeklyPlainText(relevantRoundups, unsubscribeUrl, manageUrl);

                if (await SendEmailAsync(subscriber.Email, subject, html, text, ct))
                {
                    foreach (var r in relevantRoundups)
                    {
                        slugStats[r.Slug] = (slugStats[r.Slug].Expected, slugStats[r.Slug].Actual + 1);
                    }
                }
            }
        }
        catch (Exception ex) when (ex is not OutOfMemoryException and not StackOverflowException)
        {
            foreach (var slug in newSlugs)
            {
                var actual = slugStats[slug].Actual;
                var status = actual > 0 ? "partial" : "failed";
                await _subscriberRepository.LogSendAsync("weekly-roundup", slug, actual, status, ex.Message, ct);
            }

            _logger.LogError(ex, "Failed sending combined weekly newsletter");
            return false;
        }

        // Log each slug with its individual delivery outcome
        var anySent = false;
        foreach (var slug in newSlugs)
        {
            var (expected, actual) = slugStats[slug];
            if (expected == 0)
            {
                await _subscriberRepository.LogSendAsync("weekly-roundup", slug, 0, "sent", null, ct);
                anySent = true;
            }
            else
            {
                var status = actual == expected ? "sent" : actual > 0 ? "partial" : "failed";
                var error = actual < expected ? $"Delivered to {actual} of {expected} subscribers" : null;
                await _subscriberRepository.LogSendAsync("weekly-roundup", slug, actual, status, error, ct);
                if (actual > 0)
                {
                    anySent = true;
                }
            }
        }

        return anySent;
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
            sent ? "sent" : "failed",
            sent ? null : $"Delivery failed to {recipientEmail}",
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

        var yesterday = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
        var (start, end) = GetDayUtcWindow(yesterday);
        var sectionNames = sections.Select(s => s.Trim().ToLowerInvariant()).ToList();
        var itemsBySection = await GetDailyItemsBySectionAsync(sectionNames, start, end, ct);
        var html = BuildDailyOverviewHtml(yesterday, sectionNames, itemsBySection, recipientEmail);
        var text = BuildDailyOverviewText(yesterday, sectionNames, itemsBySection, recipientEmail);
        var logKey = $"daily-{yesterday:yyyyMMdd}@{DateTime.UtcNow:yyyyMMddHHmmss}";
        var sent = await SendEmailAsync(recipientEmail, $"[Test] TechHub Daily Overview — {yesterday:yyyy-MM-dd}", html, text, ct);
        await _subscriberRepository.LogSendAsync(
            "test-daily",
            logKey,
            sent ? 1 : 0,
            sent ? "sent" : "failed",
            sent ? null : $"Delivery failed to {recipientEmail}",
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
        var html = $"""
            <html><body style="font-family:Segoe UI,Arial,sans-serif;color:#1f2937;">
              <h2>Manage your TechHub newsletter subscription</h2>
              <p>Click the button below to manage your subscription preferences.</p>
              <p><a href="{WebUtility.HtmlEncode(manageUrl)}" style="display:inline-block;padding:10px 20px;background:#111827;color:#fff;text-decoration:none;border-radius:6px;">Manage subscription</a></p>
              <p style="color:#6b7280;font-size:0.875rem;">Or copy this link into your browser:<br>{WebUtility.HtmlEncode(manageUrl)}</p>
            </body></html>
            """;
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
            await _subscriberRepository.LogSendAsync(SendKind, targetKey, 0, "sent", null, ct);
            return true;
        }

        var sent = 0;
        foreach (var subscriber in dailySubscribers)
        {
            var selectedSections = subscriber.DailySections
                .Where(section => itemsBySection.ContainsKey(section))
                .ToList();

            var html = BuildDailyOverviewHtml(day, selectedSections, itemsBySection, subscriber.Email);
            var text = BuildDailyOverviewText(day, selectedSections, itemsBySection, subscriber.Email);
            if (await SendEmailAsync(subscriber.Email, $"TechHub Daily Overview — {targetKey}", html, text, ct))
            {
                sent++;
            }
        }

        var sendStatus = sent == dailySubscribers.Count ? "sent" : sent > 0 ? "partial" : "failed";
        var sendError = sent == dailySubscribers.Count
            ? null
            : sent > 0
                ? $"Delivered to {sent} of {dailySubscribers.Count} subscribers"
                : "Delivery failed for all subscribers";
        await _subscriberRepository.LogSendAsync(SendKind, targetKey, sent, sendStatus, sendError, ct);
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
        var html = BuildAdminStatusHtml(day, stats);
        var text = BuildAdminStatusText(day, stats);

        var sent = await SendEmailAsync(_options.AdminReportEmailAddress, $"TechHub Daily Status Report — {targetKey}", html, text, ct);
        await _subscriberRepository.LogSendAsync(SendKind, targetKey, sent ? 1 : 0, sent ? "sent" : "failed", sent ? null : "Unable to send admin status report", ct);
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
        var html = $"""
            <!DOCTYPE html>
            <html lang="en">
            <head>
              <meta charset="utf-8" />
              <meta name="viewport" content="width=device-width, initial-scale=1.0" />
            </head>
            <body style="font-family:Segoe UI,Arial,sans-serif;color:#1f2937;">
              <h2>Confirm your TechHub newsletter subscription</h2>
              <p>Click the button below to confirm your subscription. If you did not sign up, you can safely ignore this email.</p>
              <p><a href="{WebUtility.HtmlEncode(confirmUrl)}" style="display:inline-block;padding:10px 20px;background:#7c3aed;color:#fff;text-decoration:none;border-radius:6px;">Confirm subscription</a></p>
              <p style="color:#6b7280;font-size:0.875rem;">Or copy this link into your browser:<br>{WebUtility.HtmlEncode(confirmUrl)}</p>
            </body></html>
            """;
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
                .ToList();
        }

        return result;
    }

    private string BuildSectionLinksHtml(RoundupRow roundup)
    {
        var headers = roundup.Content
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Where(line => line.StartsWith("## ", StringComparison.Ordinal))
            .Select(line => line[3..].Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var baseRoundupUrl = BuildAbsoluteUrl($"/{roundup.SectionName}/roundups/{roundup.Slug}");
        var sb = new StringBuilder();
        sb.Append("<ul style=\"padding-left:18px;margin:0;\">");
        foreach (var header in headers)
        {
            var href = $"{baseRoundupUrl}#{RoundupContentBuilder.BuildAnchor(header)}";
            sb.Append("<li style=\"margin:0 0 8px 0;\">");
            sb.Append($"<a href=\"{WebUtility.HtmlEncode(href)}\" style=\"color:#2563eb;text-decoration:none;\">");
            sb.Append(WebUtility.HtmlEncode(header));
            sb.Append("</a></li>");
        }

        sb.Append("</ul>");
        return sb.ToString();
    }

    private string RenderRoundupTemplate(string title, string introduction, string sectionLinks, string fullRoundupUrl, string unsubscribeUrl, string manageUrl) =>
        _htmlTemplate
            .Replace("{Title}", WebUtility.HtmlEncode(title), StringComparison.Ordinal)
            .Replace("{Introduction}", WebUtility.HtmlEncode(introduction), StringComparison.Ordinal)
            .Replace("{SectionLinks}", sectionLinks, StringComparison.Ordinal)
            .Replace("{FullRoundupUrl}", WebUtility.HtmlEncode(fullRoundupUrl), StringComparison.Ordinal)
            .Replace("{ManageUrl}", WebUtility.HtmlEncode(manageUrl), StringComparison.Ordinal)
            .Replace("{UnsubscribeUrl}", WebUtility.HtmlEncode(unsubscribeUrl), StringComparison.Ordinal);

    private static string BuildRoundupPlainText(RoundupRow roundup, string fullRoundupUrl, string unsubscribeUrl, string manageUrl) =>
        string.Create(CultureInfo.InvariantCulture, $"""
            {roundup.Title}

            {roundup.Introduction}

            Read the full roundup: {fullRoundupUrl}

            Manage subscription: {manageUrl}
            Unsubscribe: {unsubscribeUrl}
            """);

    private string RenderCombinedWeeklyHtml(IReadOnlyList<RoundupRow> roundups, string unsubscribeUrl, string manageUrl)
    {
        var sb = new StringBuilder();
        sb.Append("""
            <!DOCTYPE html>
            <html lang="en">
            <head>
              <meta charset="utf-8" />
              <meta name="viewport" content="width=device-width, initial-scale=1.0" />
            </head>
            <body style="margin:0;padding:0;background:#f5f7fb;font-family:Segoe UI,Arial,sans-serif;color:#1f2937;">
              <table role="presentation" width="100%" cellspacing="0" cellpadding="0" style="padding:24px 12px;">
                <tr>
                  <td align="center">
                    <table role="presentation" width="100%" cellspacing="0" cellpadding="0" style="max-width:900px;background:#ffffff;border-radius:12px;overflow:hidden;">
                      <tr>
                        <td style="background:#111827;color:#ffffff;padding:20px 28px;font-size:20px;font-weight:700;">TechHub Weekly Digest</td>
                      </tr>
                      <tr>
                        <td style="padding:28px;">
            """);

        for (var i = 0; i < roundups.Count; i++)
        {
            var roundup = roundups[i];
            var sectionTitle = GetSectionTitle(roundup.SectionName);
            var fullRoundupUrl = BuildAbsoluteUrl($"/{roundup.SectionName}/roundups/{roundup.Slug}");
            var sectionLinksHtml = BuildSectionLinksHtml(roundup);

            if (i > 0)
            {
                sb.Append("<hr style=\"border:none;border-top:1px solid #e5e7eb;margin:28px 0;\" />");
            }

            sb.Append(CultureInfo.InvariantCulture, $"""
                <div>
                  <p style="margin:0 0 4px 0;font-size:12px;font-weight:600;letter-spacing:0.05em;text-transform:uppercase;color:#6b7280;">{WebUtility.HtmlEncode(sectionTitle)}</p>
                  <h2 style="margin:0 0 12px 0;font-size:22px;color:#111827;">{WebUtility.HtmlEncode(roundup.Title)}</h2>
                  <p style="margin:0 0 16px 0;line-height:1.6;color:#374151;">{WebUtility.HtmlEncode(roundup.Introduction)}</p>
                  <h3 style="margin:0 0 8px 0;font-size:15px;color:#111827;">📑 In this roundup</h3>
                  <div style="margin:0 0 16px 0;">{sectionLinksHtml}</div>
                  <p style="margin:0;">
                    <a href="{WebUtility.HtmlEncode(fullRoundupUrl)}" style="display:inline-block;background:#111827;color:#ffffff;text-decoration:none;padding:10px 14px;border-radius:8px;font-size:14px;">Read the full {WebUtility.HtmlEncode(sectionTitle)} roundup →</a>
                  </p>
                </div>
                """);
        }

        sb.Append(CultureInfo.InvariantCulture, $"""
                          <p style="margin:28px 0 0 0;font-size:12px;color:#6b7280;border-top:1px solid #e5e7eb;padding-top:20px;">
                            You received this because you subscribed to TechHub newsletters.<br />
                            <a href="{WebUtility.HtmlEncode(manageUrl)}" style="color:#2563eb;">Manage subscription</a> &nbsp;·&nbsp;
                            <a href="{WebUtility.HtmlEncode(unsubscribeUrl)}" style="color:#2563eb;">Unsubscribe</a>
                          </p>
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
              </table>
            </body>
            </html>
            """);

        return sb.ToString();
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
            sb.AppendLine(roundup.Title);
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

    private string BuildDailyOverviewHtml(
        DateOnly day,
        IReadOnlyList<string> sections,
        IReadOnlyDictionary<string, IReadOnlyList<DailyItemRow>> itemsBySection,
        string email)
    {
        var unsubscribeUrl = BuildUnsubscribeUrl(email);
        var manageUrl = BuildManageUrl(email, _options.UnsubscribeSecret);

        var sb = new StringBuilder();
        sb.Append($"""
            <!DOCTYPE html>
            <html lang="en">
            <head>
              <meta charset="utf-8" />
              <meta name="viewport" content="width=device-width, initial-scale=1.0" />
            </head>
            <body style="margin:0;padding:0;background:#f5f7fb;font-family:Segoe UI,Arial,sans-serif;color:#1f2937;">
              <table role="presentation" width="100%" cellspacing="0" cellpadding="0" style="padding:24px 12px;">
                <tr>
                  <td align="center">
                    <table role="presentation" width="100%" cellspacing="0" cellpadding="0" style="max-width:1100px;background:#ffffff;border-radius:12px;overflow:hidden;">
                      <tr>
                        <td style="background:#111827;color:#ffffff;padding:20px 28px;font-size:20px;font-weight:700;">TechHub Daily Overview</td>
                      </tr>
                      <tr>
                        <td style="padding:28px;">
                          <p style="margin:0 0 20px 0;font-size:15px;color:#6b7280;">{day:yyyy-MM-dd}</p>
            """);

        if (sections.Count == 0)
        {
            sb.Append("<p style=\"color:#374151;\">No daily subscriptions are active.</p>");
        }
        else
        {
            foreach (var section in sections)
            {
                var items = itemsBySection.TryGetValue(section, out var rows) ? rows : [];
                sb.Append($"<h3 style=\"margin:20px 0 8px 0;font-size:18px;font-weight:700;color:#111827;\">{WebUtility.HtmlEncode(GetSectionTitle(section))}</h3>");
                if (items.Count == 0)
                {
                    sb.Append("<p style=\"margin:0 0 8px 0;color:#6b7280;\">No new content items in the last 24 hours.</p>");
                    continue;
                }

                sb.Append("<ul style=\"margin:0 0 8px 0;padding-left:20px;\">");
                foreach (var item in items)
                {
                    var filteredUrl = BuildAbsoluteUrl($"/{section}/all?types={Uri.EscapeDataString(item.CollectionName)}&search={Uri.EscapeDataString(item.Title)}&from={day:yyyy-MM-dd}&to={day:yyyy-MM-dd}");
                    sb.Append("<li style=\"margin-bottom:6px;font-size:16px;line-height:1.5;\">");
                    sb.Append($"<a href=\"{WebUtility.HtmlEncode(filteredUrl)}\" style=\"color:#2563eb;text-decoration:none;\">{WebUtility.HtmlEncode(item.Title)}</a>");
                    sb.Append($" <span style=\"color:#6b7280;font-size:14px;\">({WebUtility.HtmlEncode(item.CollectionName)})</span>");
                    sb.Append("</li>");
                }

                sb.Append("</ul>");
            }
        }

        sb.Append($"""
                          <p style="margin:28px 0 0 0;font-size:12px;color:#6b7280;border-top:1px solid #e5e7eb;padding-top:20px;">
                            You received this because you subscribed to TechHub newsletters.<br />
                            <a href="{WebUtility.HtmlEncode(manageUrl)}" style="color:#2563eb;">Manage subscription</a> &nbsp;·&nbsp;
                            <a href="{WebUtility.HtmlEncode(unsubscribeUrl)}" style="color:#2563eb;">Unsubscribe</a>
                          </p>
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
              </table>
            </body>
            </html>
            """);
        return sb.ToString();
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
            sb.AppendLine(GetSectionTitle(section));
            var items = itemsBySection.TryGetValue(section, out var rows) ? rows : [];
            if (items.Count == 0)
            {
                sb.AppendLine("- No new content items");
            }
            else
            {
                foreach (var item in items)
                {
                    var filteredUrl = BuildAbsoluteUrl($"/{section}/all?types={Uri.EscapeDataString(item.CollectionName)}&search={Uri.EscapeDataString(item.Title)}&from={day:yyyy-MM-dd}&to={day:yyyy-MM-dd}");
                    sb.AppendLine($"- {item.Title} ({item.CollectionName})");
                    sb.AppendLine($"  {filteredUrl}");
                }
            }

            sb.AppendLine();
        }

        sb.AppendLine($"Manage subscription: {BuildManageUrl(email, _options.UnsubscribeSecret)}");
        sb.AppendLine($"Unsubscribe: {BuildUnsubscribeUrl(email)}");
        return sb.ToString();
    }

    private static string BuildAdminStatusHtml(DateOnly day, NewsletterDailyReportStats stats) =>
        $"""
        <html><body style="font-family:Segoe UI,Arial,sans-serif;color:#1f2937;">
          <h2>TechHub Daily Status Report — {day:yyyy-MM-dd}</h2>
          <ul>
            <li>New content items (24h): {stats.NewContentItemsLast24Hours}</li>
            <li>Failed processed URLs (24h): {stats.FailedProcessedUrlsLast24Hours}</li>
            <li>Failed background jobs (24h): {stats.FailedJobsLast24Hours}</li>
            <li>New newsletter subscribers (24h): {stats.NewSubscribersLast24Hours}</li>
            <li>Active subscribers: {stats.ActiveSubscribers}</li>
            <li>Unconfirmed subscribers: {stats.UnconfirmedSubscribers}</li>
          </ul>
        </body></html>
        """;

    private static string BuildAdminStatusText(DateOnly day, NewsletterDailyReportStats stats) =>
        string.Create(CultureInfo.InvariantCulture, $"""
            TechHub Daily Status Report — {day:yyyy-MM-dd}

            New content items (24h): {stats.NewContentItemsLast24Hours}
            Failed processed URLs (24h): {stats.FailedProcessedUrlsLast24Hours}
            Failed background jobs (24h): {stats.FailedJobsLast24Hours}
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

    private static string LoadTemplate()
    {
        var assembly = Assembly.GetExecutingAssembly();
        const string ResourceName = "TechHub.Infrastructure.Data.Resources.newsletter-roundup-template.html";
        using var stream = assembly.GetManifestResourceStream(ResourceName);
        if (stream is null)
        {
            return "{Title}<br/>{Introduction}<br/>{SectionLinks}<br/><a href=\"{FullRoundupUrl}\">Read</a><br/><a href=\"{UnsubscribeUrl}\">Unsubscribe</a>";
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
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
