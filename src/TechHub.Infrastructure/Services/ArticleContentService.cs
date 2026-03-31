using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// Fetches the full HTML content for an article URL and extracts the main body text.
/// YouTube items are enriched with transcript text from closed captions.
/// </summary>
public sealed class ArticleContentService : IArticleContentService
{
    private readonly IArticleFetchClient _fetchClient;
    private readonly IYouTubeTranscriptService _transcriptService;

    public ArticleContentService(
        IArticleFetchClient fetchClient,
        IYouTubeTranscriptService transcriptService)
    {
        ArgumentNullException.ThrowIfNull(fetchClient);
        ArgumentNullException.ThrowIfNull(transcriptService);

        _fetchClient = fetchClient;
        _transcriptService = transcriptService;
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
            var html = await _fetchClient.FetchHtmlAsync(item.ExternalUrl, ct);
            if (html is null)
            {
                return item;
            }

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
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            throw;
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
        // Use greedy match to capture content up to the LAST closing tag,
        // correctly handling nested elements of the same type.
        var pattern = $@"<{tagName}[^>]*>([\s\S]*)</{tagName}>";
        var match = System.Text.RegularExpressions.Regex.Match(
            html, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));
        return match.Success ? match.Groups[1].Value : null;
    }

    private static string StripHtmlTags(string html)
    {
        var cleaned = System.Text.RegularExpressions.Regex.Replace(
            html, @"<(script|style)[^>]*>[\s\S]*?</(script|style)>",
            string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));
        cleaned = System.Text.RegularExpressions.Regex.Replace(cleaned, @"<[^>]+>", " ", System.Text.RegularExpressions.RegexOptions.None, TimeSpan.FromSeconds(2));
        return System.Text.RegularExpressions.Regex.Replace(cleaned, @"\s{2,}", " ", System.Text.RegularExpressions.RegexOptions.None, TimeSpan.FromSeconds(2)).Trim();
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
