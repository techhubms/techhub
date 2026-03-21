using System.Xml;
using Microsoft.Extensions.Options;
using TechHub.ContentProcessor.Models;
using TechHub.ContentProcessor.Options;

namespace TechHub.ContentProcessor.Services;

/// <summary>
/// Downloads and parses RSS/Atom feeds into <see cref="RawFeedItem"/> instances
/// using standard <see cref="XmlDocument"/> parsing (no extra NuGet packages required).
/// </summary>
public sealed class RssFeedIngestionService
{
    private readonly HttpClient _httpClient;
    private readonly ContentProcessorOptions _options;
    private readonly ILogger<RssFeedIngestionService> _logger;

    public RssFeedIngestionService(
        HttpClient httpClient,
        IOptions<ContentProcessorOptions> options,
        ILogger<RssFeedIngestionService> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// Downloads the feed at <paramref name="feedConfig"/> and returns items published
    /// within the configured age limit, newest first.
    /// </summary>
    public async Task<IReadOnlyList<RawFeedItem>> IngestAsync(
        FeedConfig feedConfig,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(feedConfig);

        _logger.LogInformation("Downloading RSS feed: {FeedName} ({Url})", feedConfig.Name, feedConfig.Url);

        string xmlContent;
        try
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(TimeSpan.FromSeconds(_options.RequestTimeoutSeconds));

            xmlContent = await _httpClient.GetStringAsync(feedConfig.Url, cts.Token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to download feed {FeedName} from {Url}", feedConfig.Name, feedConfig.Url);
            return [];
        }

        var cutoff = DateTimeOffset.UtcNow.AddDays(-_options.ItemAgeLimitDays);
        var collectionName = NormalizeCollectionName(feedConfig.OutputDir);

        try
        {
            var items = ParseFeed(xmlContent, feedConfig, collectionName, cutoff);

            _logger.LogInformation(
                "Feed {FeedName}: {Count} items within age limit of {Days} days",
                feedConfig.Name, items.Count, _options.ItemAgeLimitDays);

            return items;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to parse feed {FeedName}", feedConfig.Name);
            return [];
        }
    }

    private static List<RawFeedItem> ParseFeed(
        string xmlContent,
        FeedConfig feedConfig,
        string collectionName,
        DateTimeOffset cutoff)
    {
        var doc = new XmlDocument();
        doc.LoadXml(xmlContent);

        var nsMgr = new XmlNamespaceManager(doc.NameTable);
        nsMgr.AddNamespace("atom", "http://www.w3.org/2005/Atom");
        nsMgr.AddNamespace("media", "http://search.yahoo.com/mrss/");
        nsMgr.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");

        // Determine feed format: RSS 2.0 or Atom
        var isAtom = doc.DocumentElement?.NamespaceURI == "http://www.w3.org/2005/Atom"
            || doc.DocumentElement?.Name == "feed";

        return isAtom
            ? ParseAtomFeed(doc, nsMgr, feedConfig, collectionName, cutoff)
            : ParseRssFeed(doc, nsMgr, feedConfig, collectionName, cutoff);
    }

    private static List<RawFeedItem> ParseRssFeed(
        XmlDocument doc,
        XmlNamespaceManager nsMgr,
        FeedConfig feedConfig,
        string collectionName,
        DateTimeOffset cutoff)
    {
        var items = new List<RawFeedItem>();
        var itemNodes = doc.SelectNodes("//channel/item");

        if (itemNodes == null)
        {
            return items;
        }

        foreach (XmlNode itemNode in itemNodes)
        {
            var title = GetNodeText(itemNode, "title") ?? string.Empty;
            var link = GetNodeText(itemNode, "link") ?? string.Empty;
            var description = StripHtml(GetNodeText(itemNode, "description") ?? string.Empty);
            var author = GetNodeText(itemNode, "author")
                ?? GetNodeText(itemNode, "dc:creator", nsMgr);
            var pubDateStr = GetNodeText(itemNode, "pubDate");

            if (!TryParseDate(pubDateStr, out var pubDate) || pubDate < cutoff)
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(link))
            {
                continue;
            }

            var tags = new List<string>();
            var categoryNodes = itemNode.SelectNodes("category");
            if (categoryNodes != null)
            {
                foreach (XmlNode cat in categoryNodes)
                {
                    if (!string.IsNullOrWhiteSpace(cat.InnerText))
                    {
                        tags.Add(cat.InnerText.Trim());
                    }
                }
            }

            items.Add(new RawFeedItem
            {
                Title = title,
                ExternalUrl = link.Trim(),
                PublishedAt = pubDate,
                Description = description,
                Author = author?.Trim(),
                FeedTags = tags,
                FeedName = feedConfig.Name,
                CollectionName = collectionName
            });
        }

        return [.. items.OrderByDescending(i => i.PublishedAt)];
    }

    private static List<RawFeedItem> ParseAtomFeed(
        XmlDocument doc,
        XmlNamespaceManager nsMgr,
        FeedConfig feedConfig,
        string collectionName,
        DateTimeOffset cutoff)
    {
        var items = new List<RawFeedItem>();
        var nsPrefix = doc.DocumentElement?.NamespaceURI == "http://www.w3.org/2005/Atom" ? "atom" : "";
        var entryPath = nsPrefix.Length > 0 ? "atom:feed/atom:entry" : "//entry";

        // Try multiple XPath selectors
        var entryNodes = doc.SelectNodes(entryPath, nsMgr)
            ?? doc.SelectNodes("//entry", nsMgr);

        if (entryNodes == null)
        {
            return items;
        }

        foreach (XmlNode entry in entryNodes)
        {
            var title = GetAtomText(entry, "title", nsMgr) ?? string.Empty;
            var link = GetAtomLink(entry, nsMgr);
            var summary = StripHtml(GetAtomText(entry, "summary", nsMgr) ?? GetAtomText(entry, "content", nsMgr) ?? string.Empty);
            var author = GetAtomAuthorName(entry, nsMgr);
            var updatedStr = GetAtomText(entry, "updated", nsMgr) ?? GetAtomText(entry, "published", nsMgr);

            if (!TryParseDate(updatedStr, out var pubDate) || pubDate < cutoff)
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(link))
            {
                continue;
            }

            var tags = new List<string>();
            var categoryNodes = entry.SelectNodes("category") ?? entry.SelectNodes("atom:category", nsMgr);
            if (categoryNodes != null)
            {
                foreach (XmlNode cat in categoryNodes)
                {
                    var term = cat.Attributes?["term"]?.Value ?? cat.InnerText;
                    if (!string.IsNullOrWhiteSpace(term))
                    {
                        tags.Add(term.Trim());
                    }
                }
            }

            items.Add(new RawFeedItem
            {
                Title = title,
                ExternalUrl = link.Trim(),
                PublishedAt = pubDate,
                Description = summary,
                Author = author?.Trim(),
                FeedTags = tags,
                FeedName = feedConfig.Name,
                CollectionName = collectionName
            });
        }

        return [.. items.OrderByDescending(i => i.PublishedAt)];
    }

    private static string? GetNodeText(XmlNode node, string xpath, XmlNamespaceManager? nsMgr = null)
    {
        var selected = nsMgr != null
            ? node.SelectSingleNode(xpath, nsMgr)
            : node.SelectSingleNode(xpath);

        return selected?.InnerText?.Trim();
    }

    private static string? GetAtomText(XmlNode node, string elementName, XmlNamespaceManager nsMgr)
        => GetNodeText(node, elementName, null)
            ?? GetNodeText(node, $"atom:{elementName}", nsMgr);

    private static string? GetAtomLink(XmlNode entry, XmlNamespaceManager nsMgr)
    {
        // Try alternate link first
        var linkNode = entry.SelectSingleNode("link[@rel='alternate']")
            ?? entry.SelectSingleNode("atom:link[@rel='alternate']", nsMgr)
            ?? entry.SelectSingleNode("link")
            ?? entry.SelectSingleNode("atom:link", nsMgr);

        return linkNode?.Attributes?["href"]?.Value ?? linkNode?.InnerText?.Trim();
    }

    private static string? GetAtomAuthorName(XmlNode entry, XmlNamespaceManager nsMgr)
    {
        var authorNode = entry.SelectSingleNode("author/name")
            ?? entry.SelectSingleNode("atom:author/atom:name", nsMgr);

        return authorNode?.InnerText?.Trim();
    }

    private static bool TryParseDate(string? dateStr, out DateTimeOffset result)
    {
        if (string.IsNullOrWhiteSpace(dateStr))
        {
            result = default;
            return false;
        }

        // Try RFC 1123 (RSS)
        if (DateTimeOffset.TryParse(dateStr, System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out result))
        {
            return true;
        }

        result = default;
        return false;
    }

    /// <summary>Strips HTML tags from a string.</summary>
    private static string StripHtml(string html)
    {
        if (string.IsNullOrWhiteSpace(html))
        {
            return string.Empty;
        }

        return System.Text.RegularExpressions.Regex.Replace(html, "<[^>]*>", string.Empty).Trim();
    }

    /// <summary>
    /// Normalizes the outputDir value from the JSON config to a plain collection name.
    /// e.g. "_news" → "news", "blogs" → "blogs"
    /// </summary>
    private static string NormalizeCollectionName(string outputDir)
        => outputDir.TrimStart('_').ToLowerInvariant();
}
