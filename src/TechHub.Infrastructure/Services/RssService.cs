using System.Text;
using System.Xml;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// Service for generating RSS 2.0 feeds from content items
/// </summary>
public class RssService : IRssService
{
    private const string SiteTitle = "Tech Hub";
    private const string SiteUrl = "https://tech.hub.ms";
    private const string Language = "en-us";
    private const int MaxItemsInFeed = 50;

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
            .Take(MaxItemsInFeed)
            .ToList();

        var rssItems = sortedItems.Select(CreateRssItem).ToList();

        var channel = new RssChannel
        {
            Title = $"{SiteTitle} - {section.Title}",
            Description = section.Description,
            Link = $"{SiteUrl}{section.Url}",
            Language = Language,
            LastBuildDate = sortedItems.FirstOrDefault() != null
                ? DateTimeOffset.FromUnixTimeSeconds(sortedItems.First().DateEpoch)
                : DateTimeOffset.UtcNow,
            Items = rssItems
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
            .Take(MaxItemsInFeed)
            .ToList();

        var rssItems = sortedItems.Select(CreateRssItem).ToList();

        var channel = new RssChannel
        {
            Title = $"{SiteTitle} - {FormatCollectionTitle(collectionName)}",
            Description = $"Latest {collectionName} from {SiteTitle}",
            Link = $"{SiteUrl}/all/{collectionName}",
            Language = Language,
            LastBuildDate = sortedItems.FirstOrDefault() != null
                ? DateTimeOffset.FromUnixTimeSeconds(sortedItems.First().DateEpoch)
                : DateTimeOffset.UtcNow,
            Items = rssItems
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

        using var stringWriter = new StringWriter();
        using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
        {
            xmlWriter.WriteStartDocument();

            // <rss version="2.0">
            xmlWriter.WriteStartElement("rss");
            xmlWriter.WriteAttributeString("version", "2.0");

            // <channel>
            xmlWriter.WriteStartElement("channel");

            // Channel metadata
            xmlWriter.WriteElementString("title", channel.Title);
            xmlWriter.WriteElementString("description", channel.Description);
            xmlWriter.WriteElementString("link", channel.Link);
            xmlWriter.WriteElementString("language", channel.Language);
            xmlWriter.WriteElementString("lastBuildDate", channel.LastBuildDate.ToString("R")); // RFC1123

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

                if (!string.IsNullOrWhiteSpace(item.Author))
                {
                    xmlWriter.WriteElementString("author", item.Author);
                }

                foreach (var sectionName in item.SectionNames)
                {
                    xmlWriter.WriteElementString("category", sectionName);
                }

                xmlWriter.WriteEndElement(); // </item>
            }

            xmlWriter.WriteEndElement(); // </channel>
            xmlWriter.WriteEndElement(); // </rss>

            xmlWriter.WriteEndDocument();
        }

        return stringWriter.ToString();
    }

    private static RssItem CreateRssItem(ContentItem item)
    {
        // External collections (news, blogs, community) link to original source
        // Internal collections (videos, roundups, custom) link to our site
        var isExternal = item.CollectionName is "news" or "blogs" or "community";
        var link = isExternal && !string.IsNullOrWhiteSpace(item.ExternalUrl)
            ? item.ExternalUrl
            : $"{SiteUrl}/{item.CollectionName}/{item.Slug}";

        return new RssItem
        {
            Title = item.Title,
            Description = item.Excerpt,
            Link = link,
            Guid = link,
            PubDate = DateTimeOffset.FromUnixTimeSeconds(item.DateEpoch),
            Author = item.Author,
            SectionNames = item.SectionNames
        };
    }

    private static string FormatCollectionTitle(string collectionName)
    {
        return collectionName switch
        {
            "news" => "News",
            "videos" => "Videos",
            "community" => "Community",
            "blogs" => "Blogs",
            "roundups" => "Roundups",
            _ => collectionName.ToUpperInvariant()
        };
    }
}
