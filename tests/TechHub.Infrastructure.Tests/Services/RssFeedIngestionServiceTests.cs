using System.Xml;
using FluentAssertions;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Services;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Tests for RssFeedIngestionService XML parsing safety and feed parsing behavior.
/// Validates XXE protection and feed parsing via the internal ParseFeed method.
/// </summary>
public class RssFeedIngestionServiceTests
{
    private static readonly DateTimeOffset _cutoff = DateTimeOffset.UtcNow.AddDays(-365);

    private static readonly FeedConfig _defaultFeedConfig = new()
    {
        Name = "Test",
        Url = "https://example.com/feed.xml",
        OutputDir = "_blogs"
    };

    [Fact]
    public void ParseFeed_WithDtdInXml_ThrowsXmlException()
    {
        // Arrange - XML with DTD that should be rejected (XXE prevention)
        var xmlWithDtd = """
            <?xml version="1.0" encoding="UTF-8"?>
            <!DOCTYPE foo [<!ENTITY xxe SYSTEM "file:///etc/passwd">]>
            <rss version="2.0">
                <channel>
                    <title>Test Feed</title>
                    <item>
                        <title>&xxe;</title>
                        <link>https://example.com/article</link>
                        <pubDate>Mon, 24 Mar 2026 12:00:00 GMT</pubDate>
                    </item>
                </channel>
            </rss>
            """;

        // Act & Assert - DTD processing is prohibited, so parsing must throw
        var act = () => RssFeedIngestionService.ParseFeed(xmlWithDtd, _defaultFeedConfig, _cutoff);
        act.Should().Throw<XmlException>();
    }

    [Fact]
    public void ParseFeed_WithValidRssFeed_ParsesItems()
    {
        // Arrange
        var validRss = $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <rss version="2.0">
                <channel>
                    <title>Test Feed</title>
                    <item>
                        <title>Test Article</title>
                        <link>https://example.com/article</link>
                        <description>Article description</description>
                        <pubDate>{DateTimeOffset.UtcNow.AddDays(-5):R}</pubDate>
                    </item>
                </channel>
            </rss>
            """;

        // Act
        var items = RssFeedIngestionService.ParseFeed(validRss, _defaultFeedConfig, _cutoff);

        // Assert
        items.Should().ContainSingle();
        items[0].Title.Should().Be("Test Article");
        items[0].ExternalUrl.Should().Be("https://example.com/article");
    }

    [Fact]
    public void ParseFeed_WithValidAtomFeed_ParsesItems()
    {
        // Arrange
        var validAtom = $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <feed xmlns="http://www.w3.org/2005/Atom">
                <title>Test Feed</title>
                <entry>
                    <title>Atom Article</title>
                    <link rel="alternate" href="https://example.com/atom-article"/>
                    <summary>Atom description</summary>
                    <updated>{DateTimeOffset.UtcNow.AddDays(-5):O}</updated>
                </entry>
            </feed>
            """;

        // Act
        var items = RssFeedIngestionService.ParseFeed(validAtom, _defaultFeedConfig, _cutoff);

        // Assert
        items.Should().ContainSingle();
        items[0].Title.Should().Be("Atom Article");
    }
}
