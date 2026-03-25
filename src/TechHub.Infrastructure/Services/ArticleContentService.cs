using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// Fetches the full HTML content for an article URL and extracts the main body text.
/// YouTube items are enriched with transcript text from closed captions.
/// </summary>
public sealed class ArticleContentService
{
    private readonly HttpClient _httpClient;
    private readonly ContentProcessorOptions _options;
    private readonly YouTubeTranscriptService _transcriptService;
    private readonly ILogger<ArticleContentService> _logger;

    public ArticleContentService(
        HttpClient httpClient,
        IOptions<ContentProcessorOptions> options,
        YouTubeTranscriptService transcriptService,
        ILogger<ArticleContentService> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(transcriptService);
        ArgumentNullException.ThrowIfNull(logger);

        _httpClient = httpClient;
        _options = options.Value;
        _transcriptService = transcriptService;
        _logger = logger;
    }

    /// <summary>
    /// Fetches the full article content for <paramref name="item"/> and returns a new instance
    /// with <see cref="RawFeedItem.FullContent"/> populated.
    /// YouTube items are enriched with transcript text when available.
    /// Returns the original item unchanged on failure.
    /// </summary>
    public async Task<RawFeedItem> EnrichWithContentAsync(RawFeedItem item, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (item.IsYouTube)
        {
            return await EnrichYouTubeWithTranscriptAsync(item, ct);
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
                _logger.LogWarning("Failed to fetch content for {Url}: HTTP {Status}", item.ExternalUrl, (int)response.StatusCode);
                return item;
            }

            var html = await response.Content.ReadAsStringAsync(cts.Token);
            var mainContent = ExtractMainContent(html);

            if (string.IsNullOrWhiteSpace(mainContent))
            {
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
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "HTTP error fetching content for {Url}", item.ExternalUrl);
            return item;
        }
    }

    private static string ExtractMainContent(string html)
    {
        if (string.IsNullOrWhiteSpace(html))
        {
            return string.Empty;
        }

        var candidates = new[]
        {
            ExtractTagContent(html, "article"),
            ExtractTagContent(html, "main"),
        };

        var best = candidates
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .OrderByDescending(c => c!.Length)
            .FirstOrDefault()
            ?? html;

        best = StripHtmlTags(best);

        const int MaxLength = 50_000;
        return best.Length > MaxLength ? best[..MaxLength].Trim() : best.Trim();
    }

    private static string? ExtractTagContent(string html, string tagName)
    {
        var pattern = $@"<{tagName}[^>]*>([\s\S]*?)</{tagName}>";
        var match = System.Text.RegularExpressions.Regex.Match(
            html, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        return match.Success ? match.Groups[1].Value : null;
    }

    private static string StripHtmlTags(string html)
    {
        var cleaned = System.Text.RegularExpressions.Regex.Replace(
            html, @"<(script|style)[^>]*>[\s\S]*?</(script|style)>",
            string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        cleaned = System.Text.RegularExpressions.Regex.Replace(cleaned, @"<[^>]+>", " ");
        return System.Text.RegularExpressions.Regex.Replace(cleaned, @"\s{2,}", " ").Trim();
    }

    private async Task<RawFeedItem> EnrichYouTubeWithTranscriptAsync(RawFeedItem item, CancellationToken ct)
    {
        var transcript = await _transcriptService.GetTranscriptAsync(item.ExternalUrl, ct);
        if (string.IsNullOrWhiteSpace(transcript))
        {
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
            FullContent = transcript
        };
    }
}
