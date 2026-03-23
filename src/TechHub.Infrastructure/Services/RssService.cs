using System.Text;
using System.Xml;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// Service for generating RSS 2.0 feeds from content items
/// </summary>
public class RssService : IRssService
{
    private readonly AppSettings _settings;
    private readonly RssOptions _rssOptions;
    private const string SiteTitle = "Tech Hub";
    private const string Language = "en-us";

    public RssService(IOptions<AppSettings> settings, IOptions<RssOptions> rssOptions)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(rssOptions);
        _settings = settings.Value;
        _rssOptions = rssOptions.Value;
    }

    /// <inheritdoc/>
    public Task<RssChannel> GenerateSectionFeedAsync(
        Section section,
        IReadOnlyList<ContentItem> items,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(section);
        ArgumentNullException.ThrowIfNull(items);

        var sortedItems = items
            .OrderByDescending(x => x.DateEpoch)
            .Take(_rssOptions.MaxItemsInFeed)
            .ToList();

        var rssItems = sortedItems.Select(CreateRssItem).ToList();

        var sectionUrlPath = section.Url == "/" ? "/all" : section.Url;
        var channel = new RssChannel
        {
            Title = $"{SiteTitle} - {section.Title}",
            Description = section.Description,
            Link = $"{_settings.BaseUrl}{section.Url}",
            Language = Language,
            LastBuildDate = sortedItems.FirstOrDefault() != null
                ? DateTimeOffset.FromUnixTimeSeconds(sortedItems.First().DateEpoch)
                : DateTimeOffset.UtcNow,
            Items = rssItems,
            FeedUrl = $"{_settings.BaseUrl}{sectionUrlPath}/feed.xml"
        };

        return Task.FromResult(channel);
    }

    /// <inheritdoc/>
    public Task<RssChannel> GenerateCollectionFeedAsync(
        string collectionName,
        IReadOnlyList<ContentItem> items,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(collectionName);
        ArgumentNullException.ThrowIfNull(items);

        var sortedItems = items
            .OrderByDescending(x => x.DateEpoch)
            .Take(_rssOptions.MaxItemsInFeed)
            .ToList();

        var rssItems = sortedItems.Select(CreateRssItem).ToList();

        // Generate collection title from collection name (e.g., "blogs" -> "Blogs")
        var collectionTitle = Collection.GetTagFromName(collectionName);

        var channel = new RssChannel
        {
            Title = $"{SiteTitle} - {collectionTitle}",
            Description = $"Latest {collectionName} from {SiteTitle}",
            Link = $"{_settings.BaseUrl}/all/{collectionName}",
            Language = Language,
            LastBuildDate = sortedItems.FirstOrDefault() != null
                ? DateTimeOffset.FromUnixTimeSeconds(sortedItems.First().DateEpoch)
                : DateTimeOffset.UtcNow,
            Items = rssItems,
            FeedUrl = $"{_settings.BaseUrl}/all/{collectionName}/feed.xml"
        };

        return Task.FromResult(channel);
    }

    /// <inheritdoc/>
    public string SerializeToXml(RssChannel channel)
    {
        ArgumentNullException.ThrowIfNull(channel);

        var settings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "  ",
            Encoding = Encoding.UTF8,
            OmitXmlDeclaration = false
        };

        using var memoryStream = new MemoryStream();
        using (var xmlWriter = XmlWriter.Create(memoryStream, settings))
        {
            xmlWriter.WriteStartDocument();

            // <rss version="2.0" xmlns:atom="..." xmlns:dc="...">
            xmlWriter.WriteStartElement("rss");
            xmlWriter.WriteAttributeString("version", "2.0");
            xmlWriter.WriteAttributeString("xmlns", "atom", "http://www.w3.org/2000/xmlns/", "http://www.w3.org/2005/Atom");
            xmlWriter.WriteAttributeString("xmlns", "dc", "http://www.w3.org/2000/xmlns/", "http://purl.org/dc/elements/1.1/");

            // <channel>
            xmlWriter.WriteStartElement("channel");

            // Channel metadata
            xmlWriter.WriteElementString("title", channel.Title);
            xmlWriter.WriteElementString("description", channel.Description);
            xmlWriter.WriteElementString("link", channel.Link);
            xmlWriter.WriteElementString("language", channel.Language);
            xmlWriter.WriteElementString("lastBuildDate", channel.LastBuildDate.ToString("R")); // RFC1123

            // atom:link rel="self" — identifies the canonical URL of this feed
            if (!string.IsNullOrWhiteSpace(channel.FeedUrl))
            {
                xmlWriter.WriteStartElement("atom", "link", "http://www.w3.org/2005/Atom");
                xmlWriter.WriteAttributeString("href", channel.FeedUrl);
                xmlWriter.WriteAttributeString("rel", "self");
                xmlWriter.WriteAttributeString("type", "application/rss+xml");
                xmlWriter.WriteEndElement();
            }

            // Items
            foreach (var item in channel.Items)
            {
                xmlWriter.WriteStartElement("item");

                xmlWriter.WriteElementString("title", item.Title);
                xmlWriter.WriteElementString("description", item.Description);
                xmlWriter.WriteElementString("link", item.Link);

                // <guid isPermaLink="true">...</guid>
                xmlWriter.WriteStartElement("guid");
                xmlWriter.WriteAttributeString("isPermaLink", "true");
                xmlWriter.WriteString(item.Guid);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteElementString("pubDate", item.PubDate.ToString("R")); // RFC1123

                // Use dc:creator instead of author — RSS 2.0 <author> requires an email address
                // per RFC 2822, but dc:creator accepts plain author names.
                if (!string.IsNullOrWhiteSpace(item.Author))
                {
                    xmlWriter.WriteElementString("dc", "creator", "http://purl.org/dc/elements/1.1/", item.Author);
                }

                // Write tags as categories
                foreach (var category in item.Categories)
                {
                    xmlWriter.WriteElementString("category", category);
                }

                xmlWriter.WriteEndElement(); // </item>
            }

            xmlWriter.WriteEndElement(); // </channel>
            xmlWriter.WriteEndElement(); // </rss>

            xmlWriter.WriteEndDocument();
        }

        return Encoding.UTF8.GetString(memoryStream.ToArray());
    }

    private RssItem CreateRssItem(ContentItem item)
    {
        var href = item.GetHref();

        // For internal items (relative URLs), prepend base URL
        if (!item.LinksExternally())
        {
            href = $"{_settings.BaseUrl}{href}";
        }

        return new RssItem
        {
            Title = item.Title,
            Description = item.Excerpt,
            Link = href,
            Guid = href,
            PubDate = DateTimeOffset.FromUnixTimeSeconds(item.DateEpoch),
            Author = item.Author,
            Categories = item.Tags
        };
    }
}
