using SmartReader;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Services.ContentProcessing;

/// <summary>
/// Fetches the full HTML content for an article URL and extracts the main body text.
/// YouTube items are enriched with transcript text from closed captions.
/// </summary>
public sealed partial class ArticleContentService : IArticleContentService
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

            // Fall back to content:encoded / Atom <content> embedded in the feed.
            // Handles bot-blocking (e.g. Medium behind Cloudflare returns 403 but embeds the full article in RSS).
            html ??= item.EmbeddedHtml;

            if (html is null)
            {
                return item;
            }

            var mainContent = ExtractMainContent(html, item.ExternalUrl);

            if (string.IsNullOrWhiteSpace(mainContent))
            {
                return item;
            }

            return new RawFeedItem
            {
                Title = item.Title,
                ExternalUrl = item.ExternalUrl,
                PublishedAt = item.PublishedAt,
                FeedItemData = item.FeedItemData,
                FeedLevelAuthor = item.FeedLevelAuthor,
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

    private static string ExtractMainContent(string html, string? url = null)
    {
        if (string.IsNullOrWhiteSpace(html))
        {
            return string.Empty;
        }

        // 1. Try Mozilla Readability (SmartReader) — handles non-semantic sites, link-dense pages,
        //    sidebar detection, and content scoring just like Firefox Reader View does.
        var smartReaderResult = TrySmartReader(html, url);
        if (!string.IsNullOrWhiteSpace(smartReaderResult))
        {
            var markdown = HtmlToMarkdownConverter.Convert(smartReaderResult);
            if (!string.IsNullOrWhiteSpace(markdown))
            {
                return markdown.Trim();
            }
        }

        // 2. Fallback: manual semantic extraction (article > main > full page).
        //    Used when SmartReader yields no content (e.g. content:encoded-wrapped minimal HTML).
        return ExtractMainContentFallback(html);
    }

    /// <summary>
    /// Attempts article extraction using Mozilla Readability (SmartReader).
    /// Returns the inner HTML of the extracted article, or <see langword="null"/> if the page
    /// does not have enough content for Readability to work (e.g. very short embedded HTML).
    /// </summary>
    private static string? TrySmartReader(string html, string? url)
    {
        try
        {
            var uri = Uri.TryCreate(url, UriKind.Absolute, out var parsed)
                ? parsed
                : new Uri("https://article.local/");

            using var reader = new Reader(uri.AbsoluteUri, html)
            {
                // Preserve all CSS classes so that code-block language classes
                // (e.g. class="language-csharp") survive into HtmlToMarkdownConverter.
                KeepClasses = true,
            };
            var article = reader.GetArticle();

            return article.IsReadable ? article.Content : null;
        }
        catch (Exception ex) when (ex is not OutOfMemoryException and not StackOverflowException)
        {
            return null;
        }
    }

    /// <summary>
    /// Fallback extraction used when SmartReader cannot identify article content
    /// (e.g. minimal RSS-embedded HTML that doesn't score high enough for Readability).
    ///
    /// Priority order:
    ///   1. Semantic HTML elements: &lt;article&gt;, &lt;main&gt;
    ///   2. Well-known content class patterns on any element:
    ///      prefixed  — body-*, content-*
    ///      suffixed  — *-body, *-content, *-post, *-entry, *-article, *-text
    ///      exact     — post, entry, story, blog-post
    ///   3. &lt;body&gt; element
    ///   4. Full HTML (last resort)
    ///
    /// Page-level chrome (header/nav/footer) is stripped before selection so the
    /// candidates don't include site navigation and page headers.
    /// </summary>
    private static string ExtractMainContentFallback(string html)
    {
        // Pre-pass: remove page-level noise.
        // Not delegated to HtmlToMarkdownConverter because the converter may also receive
        // article-level HTML where <header> legitimately contains the article title.
        // <head> is removed first so <title> text doesn't leak into the markdown output.
        var cleanedHtml = PageHeadBlock().Replace(html, string.Empty);
        cleanedHtml = PageHeaderBlock().Replace(cleanedHtml, string.Empty);
        cleanedHtml = PageNavBlock().Replace(cleanedHtml, string.Empty);
        cleanedHtml = PageFooterBlock().Replace(cleanedHtml, string.Empty);

        // 1. Semantic HTML containers.
        var semantic = new[]
        {
            ExtractTagContent(cleanedHtml, "article"),
            ExtractTagContent(cleanedHtml, "main"),
        };
        var best = PickLongest(semantic);

        // 2. Well-known class/id attribute patterns on any element,
        //    tried in specificity order (more specific patterns first).
        if (best is null)
        {
            // Each pattern captures the inner HTML of the first matching element.
            // Patterns are tried in order; we stop at the first non-empty match.
            string?[] patternCandidates =
            [
                // Exact class/id values commonly used for the main article body.
                ExtractByClassOrId(cleanedHtml, @"\b(?:post|entry|story|blog-post)\b"),

                // Suffix patterns: *-body, *-content, *-post, *-entry, *-article, *-text
                ExtractByClassOrId(cleanedHtml, @"\b\w+-(?:body|content|post|entry|article|text)\b"),

                // Prefix patterns: body-*, content-*
                ExtractByClassOrId(cleanedHtml, @"\b(?:body|content)-\w+"),
            ];
            best = PickLongest(patternCandidates);
        }

        // 3. <body> element — strips the outer page furniture but keeps everything inside.
        best ??= ExtractTagContent(cleanedHtml, "body");

        // 4. Full HTML — absolute last resort.
        best ??= cleanedHtml;

        // Convert HTML to structured markdown (noise removed, headings/lists/code/links preserved).
        var markdown = HtmlToMarkdownConverter.Convert(best);

        return markdown.Trim();
    }

    /// <summary>
    /// Returns the longest non-whitespace candidate, or <see langword="null"/> if all are empty.
    /// </summary>
    private static string? PickLongest(string?[] candidates) =>
        candidates
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .OrderByDescending(c => c!.Length)
            .FirstOrDefault();

    /// <summary>
    /// Extracts the inner HTML of the first element whose <c>class</c> or <c>id</c> attribute
    /// contains a value matching <paramref name="classPattern"/>.
    /// Returns <see langword="null"/> when no match is found.
    /// </summary>
    private static string? ExtractByClassOrId(string html, string classPattern)
    {
        // Match any opening tag that has a class="…" or id="…" attribute containing the pattern.
        // Then capture everything up to the matching closing tag (greedy, so handles nesting).
        var attrPattern =
            $@"<(\w+)[^>]*(?:class|id)=""[^""]*{classPattern}[^""]*""[^>]*>([\s\S]*?)</\1>";
        var match = System.Text.RegularExpressions.Regex.Match(
            html, attrPattern,
            System.Text.RegularExpressions.RegexOptions.IgnoreCase,
            TimeSpan.FromSeconds(2));
        return match.Success ? match.Groups[2].Value : null;
    }

    [System.Text.RegularExpressions.GeneratedRegex(@"<head[^>]*>[\s\S]*?</head>",
        System.Text.RegularExpressions.RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 5000)]
    private static partial System.Text.RegularExpressions.Regex PageHeadBlock();

    [System.Text.RegularExpressions.GeneratedRegex(@"<header[^>]*>[\s\S]*?</header>",
        System.Text.RegularExpressions.RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 5000)]
    private static partial System.Text.RegularExpressions.Regex PageHeaderBlock();

    [System.Text.RegularExpressions.GeneratedRegex(@"<nav[^>]*>[\s\S]*?</nav>",
        System.Text.RegularExpressions.RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 5000)]
    private static partial System.Text.RegularExpressions.Regex PageNavBlock();

    [System.Text.RegularExpressions.GeneratedRegex(@"<footer[^>]*>[\s\S]*?</footer>",
        System.Text.RegularExpressions.RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 5000)]
    private static partial System.Text.RegularExpressions.Regex PageFooterBlock();

    private static string? ExtractTagContent(string html, string tagName)
    {
        // Use greedy match to capture content up to the LAST closing tag,
        // correctly handling nested elements of the same type.
        var pattern = $@"<{tagName}[^>]*>([\s\S]*)</{tagName}>";
        var match = System.Text.RegularExpressions.Regex.Match(
            html, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));
        return match.Success ? match.Groups[1].Value : null;
    }

    private async Task<RawFeedItem> EnrichYouTubeWithTranscriptAsync(RawFeedItem item, CancellationToken ct)
    {
        var result = await _transcriptService.GetTranscriptAsync(item.ExternalUrl, ct);
        if (!result.IsSuccess)
        {
            return new RawFeedItem
            {
                Title = item.Title,
                ExternalUrl = item.ExternalUrl,
                PublishedAt = item.PublishedAt,
                FeedItemData = item.FeedItemData,
                FeedLevelAuthor = item.FeedLevelAuthor,
                FeedTags = item.FeedTags,
                FeedName = item.FeedName,
                CollectionName = item.CollectionName,
                FullContent = item.FullContent,
                TranscriptFailureReason = result.FailureReason
            };
        }

        return new RawFeedItem
        {
            Title = item.Title,
            ExternalUrl = item.ExternalUrl,
            PublishedAt = item.PublishedAt,
            FeedItemData = item.FeedItemData,
            FeedLevelAuthor = item.FeedLevelAuthor,
            FeedTags = item.FeedTags,
            FeedName = item.FeedName,
            CollectionName = item.CollectionName,
            FullContent = result.Text
        };
    }
}
