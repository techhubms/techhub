using Microsoft.Extensions.Options;
using TechHub.ContentProcessor.Models;
using TechHub.ContentProcessor.Options;

namespace TechHub.ContentProcessor.Services;

/// <summary>
/// Fetches the full HTML content for a given article URL and extracts the main body text
/// as Markdown using <see cref="Markdig"/>.
/// </summary>
public sealed class ArticleContentService
{
    private readonly HttpClient _httpClient;
    private readonly ContentProcessorOptions _options;
    private readonly ILogger<ArticleContentService> _logger;

    public ArticleContentService(
        HttpClient httpClient,
        IOptions<ContentProcessorOptions> options,
        ILogger<ArticleContentService> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// Fetches the full HTML content for <paramref name="item"/> and returns a new instance
    /// with <see cref="RawFeedItem.FullContent"/> populated.
    /// YouTube items are returned as-is (no content to fetch).
    /// </summary>
    public async Task<RawFeedItem> EnrichWithContentAsync(RawFeedItem item, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(item);

        // YouTube items don't have scrapable article content
        if (item.IsYouTube)
        {
            return item;
        }

        if (string.IsNullOrWhiteSpace(item.ExternalUrl))
        {
            return item;
        }

        try
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(TimeSpan.FromSeconds(_options.RequestTimeoutSeconds));

            _logger.LogDebug("Fetching content for: {Url}", item.ExternalUrl);

            using var response = await _httpClient.GetAsync(item.ExternalUrl, cts.Token);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "Failed to fetch content for {Url}: HTTP {Status}",
                    item.ExternalUrl, (int)response.StatusCode);
                return item;
            }

            var html = await response.Content.ReadAsStringAsync(cts.Token);
            var mainContent = ExtractMainContent(html, item.ExternalUrl);

            if (string.IsNullOrWhiteSpace(mainContent))
            {
                _logger.LogDebug("No extractable content found for {Url}", item.ExternalUrl);
                return item;
            }

            return new RawFeedItem
            {
                Title = item.Title,
                ExternalUrl = item.ExternalUrl,
                PublishedAt = item.PublishedAt,
                Description = item.Description,
                Author = item.Author,
                FeedTags = item.FeedTags,
                FeedName = item.FeedName,
                CollectionName = item.CollectionName,
                FullContent = mainContent
            };
        }
        catch (OperationCanceledException) when (!ct.IsCancellationRequested)
        {
            _logger.LogWarning("Timeout fetching content for {Url}", item.ExternalUrl);
            return item;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error fetching content for {Url}", item.ExternalUrl);
            return item;
        }
    }

    /// <summary>
    /// Extracts the main article body from raw HTML.
    /// Uses a heuristic priority: article > main > body.
    /// Returns plain text with some Markdown formatting preserved.
    /// </summary>
    private static string ExtractMainContent(string html, string sourceUrl)
    {
        if (string.IsNullOrWhiteSpace(html))
        {
            return string.Empty;
        }

        // Look for <article>, <main>, or the largest <div> with class "content"
        // using lightweight regex heuristics (avoids heavy HTML parser dependency)
        var candidates = new[]
        {
            ExtractTagContent(html, "article"),
            ExtractTagContent(html, "main"),
        };

        var best = candidates
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .OrderByDescending(c => c!.Length)
            .FirstOrDefault();

        if (string.IsNullOrWhiteSpace(best))
        {
            // Fall back to stripping all HTML
            best = StripHtmlTags(html);
        }
        else
        {
            best = StripHtmlTags(best);
        }

        // Truncate to avoid sending massive payloads to the AI
        const int MaxLength = 50_000;
        if (best.Length > MaxLength)
        {
            best = best[..MaxLength];
            _ = sourceUrl; // used for future logging
        }

        return best.Trim();
    }

    private static string? ExtractTagContent(string html, string tagName)
    {
        var pattern = $@"<{tagName}[^>]*>([\s\S]*?)</{tagName}>";
        var match = System.Text.RegularExpressions.Regex.Match(
            html, pattern,
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        return match.Success ? match.Groups[1].Value : null;
    }

    private static string StripHtmlTags(string html)
    {
        // Remove script and style blocks first
        var cleaned = System.Text.RegularExpressions.Regex.Replace(
            html,
            @"<(script|style)[^>]*>[\s\S]*?</(script|style)>",
            string.Empty,
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // Strip remaining HTML tags
        cleaned = System.Text.RegularExpressions.Regex.Replace(cleaned, @"<[^>]+>", " ");

        // Collapse whitespace
        cleaned = System.Text.RegularExpressions.Regex.Replace(cleaned, @"\s{2,}", " ");

        return cleaned.Trim();
    }
}
