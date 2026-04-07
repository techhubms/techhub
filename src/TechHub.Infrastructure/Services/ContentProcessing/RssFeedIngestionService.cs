using System.Globalization;
using System.Xml;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Services.ContentProcessing;

/// <summary>
/// Downloads and parses RSS/Atom feeds into <see cref="RawFeedItem"/> instances
/// using standard <see cref="XmlDocument"/> parsing (no extra NuGet packages required).
/// </summary>
public sealed class RssFeedIngestionService : IRssFeedIngestionService
{
    private readonly IRssFeedClient _feedClient;
    private readonly ContentProcessorOptions _options;
    private readonly ILogger<RssFeedIngestionService> _logger;

    public RssFeedIngestionService(
        IRssFeedClient feedClient,
        IOptions<ContentProcessorOptions> options,
        ILogger<RssFeedIngestionService> logger)
    {
        ArgumentNullException.ThrowIfNull(feedClient);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _feedClient = feedClient;
        _options = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// Downloads the feed at <paramref name="feedConfig"/> and returns a result containing items published
    /// within the configured age limit (newest first), or a failure if the feed could not be fetched or parsed.
    /// </summary>
    public async Task<FeedIngestionResult> IngestAsync(
        FeedConfig feedConfig,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(feedConfig);

        _logger.LogInformation("Downloading RSS feed: {FeedName} ({Url})", feedConfig.Name, feedConfig.Url);

        var xmlContent = await _feedClient.FetchFeedXmlAsync(feedConfig.Url, ct);
        if (xmlContent is null)
        {
            _logger.LogError("Failed to download feed {FeedName} from {Url}", feedConfig.Name, feedConfig.Url);
            return FeedIngestionResult.Failure($"Failed to download feed from {feedConfig.Url}");
        }

        var cutoff = DateTimeOffset.UtcNow.AddDays(-_options.ItemAgeLimitDays);

        try
        {
            var items = ParseFeed(xmlContent, feedConfig, cutoff);

            _logger.LogInformation(
                "Feed {FeedName}: {Count} items within age limit of {Days} days",
                feedConfig.Name, items.Count, _options.ItemAgeLimitDays);

            return FeedIngestionResult.Success(items);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex) when (ex is XmlException or FormatException or ArgumentException)
        {
            _logger.LogError(ex, "Failed to parse feed {FeedName}", feedConfig.Name);
            return FeedIngestionResult.Failure($"Failed to parse feed: {ex.Message}");
        }
    }

    internal static List<RawFeedItem> ParseFeed(string xmlContent, FeedConfig feedConfig, DateTimeOffset cutoff)
    {
        // Use a safe XmlReader to prohibit DTD processing and external entities (XXE prevention)
        var settings = new XmlReaderSettings
        {
            DtdProcessing = DtdProcessing.Prohibit,
            XmlResolver = null
        };

        var doc = new XmlDocument { XmlResolver = null };
        using (var reader = XmlReader.Create(new System.IO.StringReader(xmlContent), settings))
        {
            doc.Load(reader);
        }

        var nsMgr = new XmlNamespaceManager(doc.NameTable);
        nsMgr.AddNamespace("atom", "http://www.w3.org/2005/Atom");
        nsMgr.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
        nsMgr.AddNamespace("media", "http://search.yahoo.com/mrss/");

        var isAtom = doc.DocumentElement?.NamespaceURI == "http://www.w3.org/2005/Atom"
            || string.Equals(doc.DocumentElement?.Name, "feed", StringComparison.OrdinalIgnoreCase);

        return isAtom
            ? ParseAtomFeed(doc, nsMgr, feedConfig, cutoff)
            : ParseRssFeed(doc, feedConfig, cutoff);
    }

    private static List<RawFeedItem> ParseRssFeed(
        XmlDocument doc, FeedConfig feedConfig, DateTimeOffset cutoff)
    {
        var items = new List<RawFeedItem>();
        var itemNodes = doc.SelectNodes("//channel/item");
        if (itemNodes == null)
        {
            return items;
        }

        // Feed-level author fallback: managingEditor → webMaster → feed name
        var channelNode = doc.SelectSingleNode("//channel");
        var feedLevelAuthor = GetNodeText(channelNode!, "managingEditor")
            ?? GetNodeText(channelNode!, "webMaster")
            ?? feedConfig.Name;

        foreach (XmlNode itemNode in itemNodes)
        {
            var title = GetNodeText(itemNode, "title") ?? string.Empty;
            var link = GetNodeText(itemNode, "link") ?? string.Empty;
            var pubDateStr = GetNodeText(itemNode, "pubDate");

            if (!TryParseDate(pubDateStr, out var pubDate) || pubDate < cutoff || string.IsNullOrWhiteSpace(link))
            {
                continue;
            }

            // Extract tags — simple enough to keep deterministic
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

            // Convert the full XML node to compact text for AI processing
            var feedItemData = FeedItemXmlConverter.ToCompactText(itemNode);

            // Extract content:encoded (Medium, Ghost, WordPress) for fallback when HTTP fetch is blocked.
            // Use local-name() to match regardless of XML namespace prefix.
            var encodedNode = itemNode.SelectSingleNode("*[local-name()='encoded']");
            var embeddedHtml = !string.IsNullOrWhiteSpace(encodedNode?.InnerText)
                ? $"<html><body><article>{encodedNode!.InnerText}</article></body></html>"
                : null;

            items.Add(new RawFeedItem
            {
                Title = title,
                ExternalUrl = link.Trim(),
                PublishedAt = pubDate,
                FeedItemData = feedItemData,
                FeedLevelAuthor = feedLevelAuthor,
                FeedTags = tags,
                FeedName = feedConfig.Name,
                CollectionName = feedConfig.CollectionName,
                EmbeddedHtml = embeddedHtml
            });
        }

        return [.. items.OrderByDescending(i => i.PublishedAt)];
    }

    private static List<RawFeedItem> ParseAtomFeed(
        XmlDocument doc, XmlNamespaceManager nsMgr, FeedConfig feedConfig, DateTimeOffset cutoff)
    {
        var items = new List<RawFeedItem>();
        // SelectNodes returns an empty list (not null) when no nodes match,
        // so we must check Count before falling through to the prefixed query.
        var entryNodes = doc.SelectNodes("//entry", nsMgr);
        if (entryNodes is null or { Count: 0 })
        {
            entryNodes = doc.SelectNodes("//atom:entry", nsMgr);
        }

        if (entryNodes == null)
        {
            return items;
        }

        // Feed-level author fallback: <feed><author><name> → feed name
        var feedAuthorNode = doc.DocumentElement?.SelectSingleNode("author/name")
            ?? doc.DocumentElement?.SelectSingleNode("atom:author/atom:name", nsMgr);
        var feedLevelAuthor = feedAuthorNode?.InnerText?.Trim() ?? feedConfig.Name;

        foreach (XmlNode entry in entryNodes)
        {
            var title = GetAtomText(entry, "title", nsMgr) ?? string.Empty;
            var link = GetAtomLink(entry, nsMgr);
            var updatedStr = GetAtomText(entry, "updated", nsMgr) ?? GetAtomText(entry, "published", nsMgr);

            if (!TryParseDate(updatedStr, out var pubDate) || pubDate < cutoff || string.IsNullOrWhiteSpace(link))
            {
                continue;
            }

            // Extract tags — simple enough to keep deterministic
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

            // Convert the full XML node to compact text for AI processing
            var feedItemData = FeedItemXmlConverter.ToCompactText(entry);

            // Extract Atom <content> element for fallback when HTTP fetch is blocked
            var contentNode = entry.SelectSingleNode("content") ?? entry.SelectSingleNode("atom:content", nsMgr);
            var embeddedHtml = !string.IsNullOrWhiteSpace(contentNode?.InnerText)
                ? $"<html><body><article>{contentNode!.InnerText}</article></body></html>"
                : null;

            items.Add(new RawFeedItem
            {
                Title = title,
                ExternalUrl = link.Trim(),
                PublishedAt = pubDate,
                FeedItemData = feedItemData,
                FeedLevelAuthor = feedLevelAuthor,
                FeedTags = tags,
                FeedName = feedConfig.Name,
                CollectionName = feedConfig.CollectionName,
                EmbeddedHtml = embeddedHtml
            });
        }

        return [.. items.OrderByDescending(i => i.PublishedAt)];
    }

    private static string? GetNodeText(XmlNode node, string xpath, XmlNamespaceManager? nsMgr = null)
    {
        var selected = nsMgr != null ? node.SelectSingleNode(xpath, nsMgr) : node.SelectSingleNode(xpath);
        return selected?.InnerText?.Trim();
    }

    private static string? GetAtomText(XmlNode node, string elementName, XmlNamespaceManager nsMgr)
        => GetNodeText(node, elementName) ?? GetNodeText(node, $"atom:{elementName}", nsMgr);

    private static string? GetAtomLink(XmlNode entry, XmlNamespaceManager nsMgr)
    {
        var linkNode = entry.SelectSingleNode("link[@rel='alternate']")
            ?? entry.SelectSingleNode("atom:link[@rel='alternate']", nsMgr)
            ?? entry.SelectSingleNode("link")
            ?? entry.SelectSingleNode("atom:link", nsMgr);
        return linkNode?.Attributes?["href"]?.Value ?? linkNode?.InnerText?.Trim();
    }

    private static bool TryParseDate(string? dateStr, out DateTimeOffset result)
    {
        if (string.IsNullOrWhiteSpace(dateStr))
        {
            result = default;
            return false;
        }

        return DateTimeOffset.TryParse(dateStr, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
    }
}
