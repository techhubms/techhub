using System.Text;
using System.Xml;
using TechHub.Core.DTOs;
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
    public Task<RssChannelDto> GenerateSectionFeedAsync(
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

        var channel = new RssChannelDto
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
    public Task<RssChannelDto> GenerateCollectionFeedAsync(
        string collection,
        IReadOnlyList<ContentItem> items,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(collection);
        ArgumentNullException.ThrowIfNull(items);

        var sortedItems = items
            .OrderByDescending(x => x.DateEpoch)
            .Take(MaxItemsInFeed)
            .ToList();

        var rssItems = sortedItems.Select(CreateRssItem).ToList();

        var channel = new RssChannelDto
        {
            Title = $"{SiteTitle} - {FormatCollectionTitle(collection)}",
            Description = $"Latest {collection} from {SiteTitle}",
            Link = $"{SiteUrl}/all/{collection}",
            Language = Language,
            LastBuildDate = sortedItems.FirstOrDefault() != null
                ? DateTimeOffset.FromUnixTimeSeconds(sortedItems.First().DateEpoch)
                : DateTimeOffset.UtcNow,
            Items = rssItems
        };

        return Task.FromResult(channel);
    }

    /// <inheritdoc/>
    public string SerializeToXml(RssChannelDto channel)
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

    private static RssItemDto CreateRssItem(ContentItem item)
    {
        var link = item.ViewingMode == "external" && !string.IsNullOrWhiteSpace(item.ExternalUrl)
            ? item.ExternalUrl
            : $"{SiteUrl}/{item.CollectionName}/{item.Slug}";

        var description = !string.IsNullOrWhiteSpace(item.Excerpt)
            ? item.Excerpt
            : item.Description;

        return new RssItemDto
        {
            Title = item.Title,
            Description = description,
            Link = link,
            Guid = link,
            PubDate = DateTimeOffset.FromUnixTimeSeconds(item.DateEpoch),
            Author = item.Author,
            SectionNames = item.SectionNames
        };
    }

    private static string FormatCollectionTitle(string collection)
    {
        return collection switch
        {
            "news" => "News",
            "videos" => "Videos",
            "community" => "Community",
            "blogs" => "Blogs",
            "roundups" => "Roundups",
            _ => collection.ToUpperInvariant()
        };
    }
}
