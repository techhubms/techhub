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
    private readonly IEmailSender _emailSender;
    private readonly ILogger<NewsletterService> _logger;
    private readonly string _htmlTemplate;

    public NewsletterService(
        IDbConnection connection,
        INewsletterSubscriberRepository subscriberRepository,
        IContentRepository contentRepository,
        IOptions<NewsletterOptions> options,
        IEmailSender emailSender,
        ILogger<NewsletterService> logger)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(subscriberRepository);
        ArgumentNullException.ThrowIfNull(contentRepository);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(emailSender);
        ArgumentNullException.ThrowIfNull(logger);

        _connection = connection;
        _subscriberRepository = subscriberRepository;
        _contentRepository = contentRepository;
        _options = options.Value;
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
                var html = RenderRoundupTemplate(roundup.Title, roundup.Introduction, sectionLinksHtml, fullRoundupUrl, unsubscribeUrl);
                var text = BuildRoundupPlainText(roundup, fullRoundupUrl, unsubscribeUrl);
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

        var roundup = await GetRoundupBySlugAsync(roundupSlug, ct);
        if (roundup is null)
        {
            return false;
        }

        if (!IsUnsubscribeSecretConfigured("test-email"))
        {
            return false;
        }

        var sectionLinksHtml = BuildSectionLinksHtml(roundup);
        var fullRoundupUrl = BuildAbsoluteUrl($"/{roundup.SectionName}/roundups/{roundup.Slug}");
        var unsubscribeUrl = BuildUnsubscribeUrl(recipientEmail);
        var html = RenderRoundupTemplate(roundup.Title, roundup.Introduction, sectionLinksHtml, fullRoundupUrl, unsubscribeUrl);
        var text = BuildRoundupPlainText(roundup, fullRoundupUrl, unsubscribeUrl);
        return await SendEmailAsync(recipientEmail, $"[Test] {roundup.Title}", html, text, ct);
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

    private string RenderRoundupTemplate(string title, string introduction, string sectionLinks, string fullRoundupUrl, string unsubscribeUrl) =>
        _htmlTemplate
            .Replace("{Title}", WebUtility.HtmlEncode(title), StringComparison.Ordinal)
            .Replace("{Introduction}", WebUtility.HtmlEncode(introduction), StringComparison.Ordinal)
            .Replace("{SectionLinks}", sectionLinks, StringComparison.Ordinal)
            .Replace("{FullRoundupUrl}", WebUtility.HtmlEncode(fullRoundupUrl), StringComparison.Ordinal)
            .Replace("{UnsubscribeUrl}", WebUtility.HtmlEncode(unsubscribeUrl), StringComparison.Ordinal);

    private static string BuildRoundupPlainText(RoundupRow roundup, string fullRoundupUrl, string unsubscribeUrl) =>
        string.Create(CultureInfo.InvariantCulture, $"""
            {roundup.Title}

            {roundup.Introduction}

            Read the full roundup: {fullRoundupUrl}

            Unsubscribe: {unsubscribeUrl}
            """);

    private string BuildDailyOverviewHtml(
        DateOnly day,
        IReadOnlyList<string> sections,
        IReadOnlyDictionary<string, IReadOnlyList<DailyItemRow>> itemsBySection,
        string email)
    {
        var sb = new StringBuilder();
        sb.Append("<html><body style=\"font-family:Segoe UI,Arial,sans-serif;color:#1f2937;\">");
        sb.Append($"<h2>TechHub Daily Overview — {day:yyyy-MM-dd}</h2>");

        if (sections.Count == 0)
        {
            sb.Append("<p>No daily subscriptions are active.</p>");
        }
        else
        {
            foreach (var section in sections)
            {
                var items = itemsBySection.TryGetValue(section, out var rows) ? rows : [];
                sb.Append($"<h3>{WebUtility.HtmlEncode(section)}</h3>");
                if (items.Count == 0)
                {
                    sb.Append("<p>No new content items in the last 24 hours.</p>");
                    continue;
                }

                sb.Append("<ul>");
                foreach (var item in items)
                {
                    var filteredUrl = BuildAbsoluteUrl($"/{section}/all?types={Uri.EscapeDataString(item.CollectionName)}&search={Uri.EscapeDataString(item.Title)}");
                    sb.Append("<li>");
                    sb.Append($"<a href=\"{WebUtility.HtmlEncode(filteredUrl)}\" style=\"color:#2563eb;text-decoration:none;\">{WebUtility.HtmlEncode(item.Title)}</a>");
                    sb.Append($" <span style=\"color:#6b7280;\">({WebUtility.HtmlEncode(item.CollectionName)})</span>");
                    sb.Append("</li>");
                }

                sb.Append("</ul>");
            }
        }

        sb.Append($"<p><a href=\"{WebUtility.HtmlEncode(BuildUnsubscribeUrl(email))}\">Unsubscribe</a></p>");
        sb.Append("</body></html>");
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
            sb.AppendLine(section);
            var items = itemsBySection.TryGetValue(section, out var rows) ? rows : [];
            if (items.Count == 0)
            {
                sb.AppendLine("- No new content items");
            }
            else
            {
                foreach (var item in items)
                {
                    var filteredUrl = BuildAbsoluteUrl($"/{section}/all?types={Uri.EscapeDataString(item.CollectionName)}&search={Uri.EscapeDataString(item.Title)}");
                    sb.AppendLine($"- {item.Title} ({item.CollectionName})");
                    sb.AppendLine($"  {filteredUrl}");
                }
            }

            sb.AppendLine();
        }

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
        if (Uri.TryCreate(pathOrAbsolute, UriKind.Absolute, out var absolute))
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
