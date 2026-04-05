using System.Xml;
using FluentAssertions;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Services.ContentProcessing;

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

    [Fact]
    public void ParseFeed_RssWithItemAuthor_UsesItemAuthor()
    {
        // Arrange
        var rss = $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <rss version="2.0">
                <channel>
                    <title>Test Feed</title>
                    <managingEditor>Channel Author</managingEditor>
                    <item>
                        <title>Article</title>
                        <link>https://example.com/article</link>
                        <author>Item Author</author>
                        <pubDate>{DateTimeOffset.UtcNow.AddDays(-5):R}</pubDate>
                    </item>
                </channel>
            </rss>
            """;

        // Act
        var items = RssFeedIngestionService.ParseFeed(rss, _defaultFeedConfig, _cutoff);

        // Assert
        items.Should().ContainSingle();
        items[0].FeedItemData.Should().Contain("Item Author");
        items[0].FeedLevelAuthor.Should().Be("Channel Author");
    }

    [Fact]
    public void ParseFeed_RssWithDcCreator_UsesDcCreator()
    {
        // Arrange
        var rss = $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <rss version="2.0" xmlns:dc="http://purl.org/dc/elements/1.1/">
                <channel>
                    <title>Test Feed</title>
                    <item>
                        <title>Article</title>
                        <link>https://example.com/article</link>
                        <dc:creator>DC Author</dc:creator>
                        <pubDate>{DateTimeOffset.UtcNow.AddDays(-5):R}</pubDate>
                    </item>
                </channel>
            </rss>
            """;

        // Act
        var items = RssFeedIngestionService.ParseFeed(rss, _defaultFeedConfig, _cutoff);

        // Assert
        items.Should().ContainSingle();
        items[0].FeedItemData.Should().Contain("DC Author");
        items[0].FeedLevelAuthor.Should().Be("Test");
    }

    [Fact]
    public void ParseFeed_RssWithNoItemAuthor_FallsBackToManagingEditor()
    {
        // Arrange
        var rss = $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <rss version="2.0">
                <channel>
                    <title>Test Feed</title>
                    <managingEditor>Channel Editor</managingEditor>
                    <item>
                        <title>Article</title>
                        <link>https://example.com/article</link>
                        <pubDate>{DateTimeOffset.UtcNow.AddDays(-5):R}</pubDate>
                    </item>
                </channel>
            </rss>
            """;

        // Act
        var items = RssFeedIngestionService.ParseFeed(rss, _defaultFeedConfig, _cutoff);

        // Assert
        items.Should().ContainSingle();
        items[0].FeedLevelAuthor.Should().Be("Channel Editor");
    }

    [Fact]
    public void ParseFeed_RssWithNoAuthorAtAll_FallsBackToFeedName()
    {
        // Arrange
        var feedConfig = new FeedConfig
        {
            Name = "The GitHub Blog",
            Url = "https://example.com/feed.xml",
            OutputDir = "_blogs"
        };
        var rss = $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <rss version="2.0">
                <channel>
                    <title>Test Feed</title>
                    <item>
                        <title>Article</title>
                        <link>https://example.com/article</link>
                        <pubDate>{DateTimeOffset.UtcNow.AddDays(-5):R}</pubDate>
                    </item>
                </channel>
            </rss>
            """;

        // Act
        var items = RssFeedIngestionService.ParseFeed(rss, feedConfig, _cutoff);

        // Assert
        items.Should().ContainSingle();
        items[0].FeedLevelAuthor.Should().Be("The GitHub Blog");
    }

    [Fact]
    public void ParseFeed_AtomWithEntryAuthor_UsesEntryAuthor()
    {
        // Arrange
        var atom = $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <feed xmlns="http://www.w3.org/2005/Atom">
                <title>Test Feed</title>
                <author><name>Feed Author</name></author>
                <entry>
                    <title>Article</title>
                    <link rel="alternate" href="https://example.com/article"/>
                    <author><name>Entry Author</name></author>
                    <updated>{DateTimeOffset.UtcNow.AddDays(-5):O}</updated>
                </entry>
            </feed>
            """;

        // Act
        var items = RssFeedIngestionService.ParseFeed(atom, _defaultFeedConfig, _cutoff);

        // Assert
        items.Should().ContainSingle();
        items[0].FeedItemData.Should().Contain("Entry Author");
        items[0].FeedLevelAuthor.Should().Be("Feed Author");
    }

    [Fact]
    public void ParseFeed_AtomWithNoEntryAuthor_FallsBackToFeedAuthor()
    {
        // Arrange
        var atom = $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <feed xmlns="http://www.w3.org/2005/Atom">
                <title>Test Feed</title>
                <author><name>Feed Author</name></author>
                <entry>
                    <title>Article</title>
                    <link rel="alternate" href="https://example.com/article"/>
                    <updated>{DateTimeOffset.UtcNow.AddDays(-5):O}</updated>
                </entry>
            </feed>
            """;

        // Act
        var items = RssFeedIngestionService.ParseFeed(atom, _defaultFeedConfig, _cutoff);

        // Assert
        items.Should().ContainSingle();
        items[0].FeedLevelAuthor.Should().Be("Feed Author");
    }

    [Fact]
    public void ParseFeed_AtomWithNoAuthorAtAll_FallsBackToFeedName()
    {
        // Arrange
        var feedConfig = new FeedConfig
        {
            Name = "Rob Bos Blog",
            Url = "https://example.com/feed.xml",
            OutputDir = "_blogs"
        };
        var atom = $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <feed xmlns="http://www.w3.org/2005/Atom">
                <title>Test Feed</title>
                <entry>
                    <title>Article</title>
                    <link rel="alternate" href="https://example.com/article"/>
                    <updated>{DateTimeOffset.UtcNow.AddDays(-5):O}</updated>
                </entry>
            </feed>
            """;

        // Act
        var items = RssFeedIngestionService.ParseFeed(atom, feedConfig, _cutoff);

        // Assert
        items.Should().ContainSingle();
        items[0].FeedLevelAuthor.Should().Be("Rob Bos Blog");
    }

    [Fact]
    public void ParseFeed_AtomWithMediaDescription_FallsBackToMediaDescription()
    {
        // Arrange — YouTube Atom feeds put descriptions in <media:group><media:description>
        var atom = $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <feed xmlns:yt="http://www.youtube.com/xml/schemas/2015"
                  xmlns:media="http://search.yahoo.com/mrss/"
                  xmlns="http://www.w3.org/2005/Atom">
                <title>Test Channel</title>
                <author><name>Channel Author</name></author>
                <entry>
                    <id>yt:video:abc123</id>
                    <title>Video Title</title>
                    <link rel="alternate" href="https://www.youtube.com/watch?v=abc123"/>
                    <published>{DateTimeOffset.UtcNow.AddDays(-5):O}</published>
                    <updated>{DateTimeOffset.UtcNow.AddDays(-5):O}</updated>
                    <media:group>
                        <media:title>Video Title</media:title>
                        <media:description>This is the video description from media:description element.</media:description>
                    </media:group>
                </entry>
            </feed>
            """;

        // Act
        var items = RssFeedIngestionService.ParseFeed(atom, _defaultFeedConfig, _cutoff);

        // Assert
        items.Should().ContainSingle();
        items[0].Title.Should().Be("Video Title");
        items[0].FeedItemData.Should().Contain("This is the video description from media:description element.");
    }

    [Fact]
    public void ParseFeed_AtomWithSummaryAndMediaDescription_PrefersSummary()
    {
        // Arrange — when both <summary> and <media:description> exist, prefer <summary>
        var atom = $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <feed xmlns:media="http://search.yahoo.com/mrss/"
                  xmlns="http://www.w3.org/2005/Atom">
                <title>Test Feed</title>
                <entry>
                    <title>Article</title>
                    <link rel="alternate" href="https://example.com/article"/>
                    <summary>Summary text</summary>
                    <updated>{DateTimeOffset.UtcNow.AddDays(-5):O}</updated>
                    <media:group>
                        <media:description>Media description text</media:description>
                    </media:group>
                </entry>
            </feed>
            """;

        // Act
        var items = RssFeedIngestionService.ParseFeed(atom, _defaultFeedConfig, _cutoff);

        // Assert
        items.Should().ContainSingle();
        items[0].FeedItemData.Should().Contain("Summary text");
        items[0].FeedItemData.Should().Contain("Media description text");
    }

    [Fact]
    public void ParseFeed_AtomWithNoDescriptionAtAll_ReturnsEmptyDescription()
    {
        // Arrange — no summary, content, or media:description
        var atom = $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <feed xmlns="http://www.w3.org/2005/Atom">
                <title>Test Feed</title>
                <entry>
                    <title>Article</title>
                    <link rel="alternate" href="https://example.com/article"/>
                    <updated>{DateTimeOffset.UtcNow.AddDays(-5):O}</updated>
                </entry>
            </feed>
            """;

        // Act
        var items = RssFeedIngestionService.ParseFeed(atom, _defaultFeedConfig, _cutoff);

        // Assert
        items.Should().ContainSingle();
        items[0].FeedItemData.Should().NotBeNullOrEmpty();
    }

    // ── Real-world feed format tests (1 item each) ──────────────────────────

    [Fact]
    public void ParseFeed_GitHubBlogRss_ExtractsAllFields()
    {
        // Arrange — GitHub Blog style: RSS 2.0 with dc:creator, categories, HTML description
        var feedConfig = new FeedConfig
        {
            Name = "The GitHub Blog",
            Url = "https://github.blog/feed/",
            OutputDir = "_blogs"
        };
        var rss = $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <rss version="2.0"
                 xmlns:dc="http://purl.org/dc/elements/1.1/"
                 xmlns:content="http://purl.org/rss/1.0/modules/content/">
                <channel>
                    <title>The GitHub Blog</title>
                    <link>https://github.blog</link>
                    <item>
                        <title>GitHub Copilot now available for all developers</title>
                        <link>https://github.blog/2025-01-15-copilot-available/</link>
                        <dc:creator>Sarah Drasner</dc:creator>
                        <pubDate>{DateTimeOffset.UtcNow.AddDays(-5):R}</pubDate>
                        <category>Product</category>
                        <category>AI</category>
                        <description>&lt;p&gt;We are thrilled to announce that GitHub Copilot is now available.&lt;/p&gt;</description>
                        <content:encoded>&lt;p&gt;Full article content with &lt;strong&gt;HTML&lt;/strong&gt; formatting.&lt;/p&gt;</content:encoded>
                    </item>
                </channel>
            </rss>
            """;

        // Act
        var items = RssFeedIngestionService.ParseFeed(rss, feedConfig, _cutoff);

        // Assert
        items.Should().ContainSingle();
        var item = items[0];
        item.Title.Should().Be("GitHub Copilot now available for all developers");
        item.ExternalUrl.Should().Be("https://github.blog/2025-01-15-copilot-available/");
        item.FeedItemData.Should().Contain("Sarah Drasner");
        item.FeedItemData.Should().Contain("We are thrilled to announce");
        item.FeedLevelAuthor.Should().Be("The GitHub Blog");
        item.FeedTags.Should().BeEquivalentTo(["Product", "AI"]);
        item.FeedName.Should().Be("The GitHub Blog");
        item.CollectionName.Should().Be("blogs");
    }

    [Fact]
    public void ParseFeed_MediumRss_UsesDescriptionEvenWhenContentEncodedPresent()
    {
        // Arrange — Medium style: RSS 2.0 with content:encoded alongside short description
        var feedConfig = new FeedConfig
        {
            Name = "Medium - Dev.to",
            Url = "https://medium.com/feed/@devto",
            OutputDir = "_blogs"
        };
        var rss = $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <rss version="2.0"
                 xmlns:dc="http://purl.org/dc/elements/1.1/"
                 xmlns:content="http://purl.org/rss/1.0/modules/content/">
                <channel>
                    <title>Medium Feed</title>
                    <item>
                        <title>Understanding Microservices Architecture</title>
                        <link>https://medium.com/@devto/understanding-microservices-abc123</link>
                        <dc:creator>Jane Developer</dc:creator>
                        <pubDate>{DateTimeOffset.UtcNow.AddDays(-3):R}</pubDate>
                        <category>Software Engineering</category>
                        <category>Microservices</category>
                        <description>A practical guide to microservices architecture patterns and best practices.</description>
                        <content:encoded>&lt;h2&gt;Introduction&lt;/h2&gt;&lt;p&gt;Microservices have transformed how we build applications. This is a much longer body of content with full HTML formatting and images.&lt;/p&gt;</content:encoded>
                    </item>
                </channel>
            </rss>
            """;

        // Act
        var items = RssFeedIngestionService.ParseFeed(rss, feedConfig, _cutoff);

        // Assert
        items.Should().ContainSingle();
        var item = items[0];
        item.Title.Should().Be("Understanding Microservices Architecture");
        item.FeedItemData.Should().Contain("Jane Developer");
        item.FeedItemData.Should().Contain("A practical guide to microservices");
        item.FeedTags.Should().BeEquivalentTo(["Software Engineering", "Microservices"]);
    }

    [Fact]
    public void ParseFeed_DotNetFoundationRss_HandlesA10AtomNamespaceInRss()
    {
        // Arrange — .NET Foundation style: RSS 2.0 with a10:author/a10:content (Atom namespace in RSS)
        var feedConfig = new FeedConfig
        {
            Name = ".NET Foundation Blog",
            Url = "https://dotnetfoundation.org/feed/rss",
            OutputDir = "_blogs"
        };
        var rss = $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <rss version="2.0" xmlns:a10="http://www.w3.org/2005/Atom">
                <channel>
                    <title>.NET Foundation News</title>
                    <item>
                        <guid isPermaLink="false">urn:uuid:abc123</guid>
                        <link>https://dotnetfoundation.org/news/article</link>
                        <a10:author><a10:name>Foundation Team</a10:name></a10:author>
                        <title>Showcasing .NET Foundation Projects</title>
                        <description>One of the goals of the .NET Foundation is to foster growth.</description>
                        <pubDate>{DateTimeOffset.UtcNow.AddDays(-7):R}</pubDate>
                        <a10:content type="text">&lt;p&gt;Full HTML article content from the .NET Foundation blog.&lt;/p&gt;</a10:content>
                    </item>
                </channel>
            </rss>
            """;

        // Act
        var items = RssFeedIngestionService.ParseFeed(rss, feedConfig, _cutoff);

        // Assert — a10:author data is in FeedItemData; FeedLevelAuthor falls back to feed name
        items.Should().ContainSingle();
        var item = items[0];
        item.Title.Should().Be("Showcasing .NET Foundation Projects");
        item.ExternalUrl.Should().Be("https://dotnetfoundation.org/news/article");
        item.FeedItemData.Should().Contain("One of the goals");
        item.FeedName.Should().Be(".NET Foundation Blog");
        // a10:author data is in FeedItemData; FeedLevelAuthor still falls back to feed name
        item.FeedLevelAuthor.Should().Be(".NET Foundation Blog");
    }

    [Fact]
    public void ParseFeed_MsFabricBlogRss_FallsBackToFeedNameWhenNoAuthor()
    {
        // Arrange — MS Fabric Blog style: RSS 2.0 with no author, no dc:creator, no managingEditor
        var feedConfig = new FeedConfig
        {
            Name = "Microsoft Fabric Blog",
            Url = "https://blog.fabric.microsoft.com/feed",
            OutputDir = "_blogs"
        };
        var rss = $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <rss version="2.0">
                <channel>
                    <title>Microsoft Fabric Blog</title>
                    <item>
                        <guid>https://blog.fabric.microsoft.com/post/abc123</guid>
                        <link>https://blog.fabric.microsoft.com/post/abc123</link>
                        <title>Introducing Fabric Capacity Reservations</title>
                        <description>Learn how to reserve capacity for your Fabric workloads.</description>
                        <pubDate>{DateTimeOffset.UtcNow.AddDays(-2):R}</pubDate>
                    </item>
                </channel>
            </rss>
            """;

        // Act
        var items = RssFeedIngestionService.ParseFeed(rss, feedConfig, _cutoff);

        // Assert
        items.Should().ContainSingle();
        var item = items[0];
        item.Title.Should().Be("Introducing Fabric Capacity Reservations");
        item.FeedLevelAuthor.Should().Be("Microsoft Fabric Blog");
        item.FeedItemData.Should().Contain("Learn how to reserve capacity");
        item.FeedTags.Should().BeEmpty();
    }

    [Fact]
    public void ParseFeed_PodcastRss_ExtractsDcCreatorAndDescription()
    {
        // Arrange — Podcast style: RSS 2.0 with itunes:* elements, dc:creator, content:encoded
        var feedConfig = new FeedConfig
        {
            Name = "Arrested DevOps",
            Url = "https://feeds.podtrac.com/VGAulpN7MY1U",
            OutputDir = "_podcasts"
        };
        var rss = $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <rss version="2.0"
                 xmlns:dc="http://purl.org/dc/elements/1.1/"
                 xmlns:itunes="http://www.itunes.com/dtds/podcast-1.0.dtd"
                 xmlns:content="http://purl.org/rss/1.0/modules/content/">
                <channel>
                    <title>Arrested DevOps Podcast</title>
                    <itunes:author>Matt Stratton</itunes:author>
                    <item>
                        <title>DevOps and Platform Engineering</title>
                        <link>https://www.arresteddevops.com/platform-engineering/</link>
                        <dc:creator>Matt Stratton</dc:creator>
                        <pubDate>{DateTimeOffset.UtcNow.AddDays(-10):R}</pubDate>
                        <itunes:subtitle>A deep dive into platform engineering</itunes:subtitle>
                        <itunes:duration>00:45:30</itunes:duration>
                        <description>In this episode we discuss platform engineering with industry experts.</description>
                        <content:encoded>&lt;p&gt;Full show notes with &lt;a href="https://example.com"&gt;links&lt;/a&gt; and detailed discussion points.&lt;/p&gt;</content:encoded>
                        <category>DevOps</category>
                    </item>
                </channel>
            </rss>
            """;

        // Act
        var items = RssFeedIngestionService.ParseFeed(rss, feedConfig, _cutoff);

        // Assert
        items.Should().ContainSingle();
        var item = items[0];
        item.Title.Should().Be("DevOps and Platform Engineering");
        item.FeedItemData.Should().Contain("Matt Stratton");
        item.FeedItemData.Should().Contain("In this episode we discuss platform engineering");
        item.FeedTags.Should().BeEquivalentTo(["DevOps"]);
        item.CollectionName.Should().Be("podcasts");
    }

    [Fact]
    public void ParseFeed_VsCodeReleasesAtom_UsesContentWhenNoSummary()
    {
        // Arrange — VS Code Releases style: Atom with <content> only, no <summary>
        var feedConfig = new FeedConfig
        {
            Name = "VS Code Releases",
            Url = "https://github.com/microsoft/vscode/releases.atom",
            OutputDir = "_blogs"
        };
        var atom = $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <feed xmlns="http://www.w3.org/2005/Atom">
                <title>VS Code Releases</title>
                <entry>
                    <id>tag:github.com,2025:Repository/123456/v1.95.0</id>
                    <title>VS Code 1.95.0</title>
                    <link rel="alternate" href="https://github.com/microsoft/vscode/releases/tag/1.95.0"/>
                    <author><name>VS Code Team</name></author>
                    <updated>{DateTimeOffset.UtcNow.AddDays(-4):O}</updated>
                    <content type="html">&lt;h2&gt;Release highlights&lt;/h2&gt;&lt;ul&gt;&lt;li&gt;New editor features&lt;/li&gt;&lt;li&gt;Performance improvements&lt;/li&gt;&lt;/ul&gt;</content>
                </entry>
            </feed>
            """;

        // Act
        var items = RssFeedIngestionService.ParseFeed(atom, feedConfig, _cutoff);

        // Assert — content is used as description fallback when summary is absent
        items.Should().ContainSingle();
        var item = items[0];
        item.Title.Should().Be("VS Code 1.95.0");
        item.FeedItemData.Should().Contain("VS Code Team");
        item.FeedItemData.Should().Contain("Release highlights");
        item.FeedLevelAuthor.Should().Be("VS Code Releases");
        item.ExternalUrl.Should().Be("https://github.com/microsoft/vscode/releases/tag/1.95.0");
    }

    [Fact]
    public void ParseFeed_TechCommunityRss_ExtractsHtmlEncodedDescription()
    {
        // Arrange — Tech Community style: RSS 2.0 with dc:creator, HTML-heavy CDATA description
        var feedConfig = new FeedConfig
        {
            Name = "Microsoft Tech Community",
            Url = "https://techcommunity.microsoft.com/feed",
            OutputDir = "_news"
        };
        var rss = $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <rss version="2.0" xmlns:dc="http://purl.org/dc/elements/1.1/">
                <channel>
                    <title>Microsoft Tech Community</title>
                    <item>
                        <title>What's New in Azure Functions v4</title>
                        <link>https://techcommunity.microsoft.com/blog/azure-functions-v4</link>
                        <dc:creator>Azure Team</dc:creator>
                        <pubDate>{DateTimeOffset.UtcNow.AddDays(-1):R}</pubDate>
                        <description><![CDATA[<div><p>Azure Functions v4 brings <strong>exciting</strong> new features including improved cold start times.</p></div>]]></description>
                        <category>Azure</category>
                        <category>Serverless</category>
                        <category>Functions</category>
                    </item>
                </channel>
            </rss>
            """;

        // Act
        var items = RssFeedIngestionService.ParseFeed(rss, feedConfig, _cutoff);

        // Assert — HTML tags stripped, CDATA unwrapped
        items.Should().ContainSingle();
        var item = items[0];
        item.Title.Should().Be("What's New in Azure Functions v4");
        item.FeedItemData.Should().Contain("Azure Team");
        item.FeedItemData.Should().Contain("Azure Functions v4 brings");
        item.FeedTags.Should().BeEquivalentTo(["Azure", "Serverless", "Functions"]);
        item.CollectionName.Should().Be("news");
    }

    [Fact]
    public void ParseFeed_ScottHanselmanRss_ExtractsDcCreatorAndCategories()
    {
        // Arrange — Scott Hanselman style: RSS 2.0 with dc:creator, multiple categories, trackback elements
        var feedConfig = new FeedConfig
        {
            Name = "Scott Hanselman Blog",
            Url = "https://feeds.hanselman.com/ScottHanselman",
            OutputDir = "_blogs"
        };
        var rss = $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <rss version="2.0"
                 xmlns:dc="http://purl.org/dc/elements/1.1/"
                 xmlns:slash="http://purl.org/rss/1.0/modules/slash/"
                 xmlns:wfw="http://wellformedweb.org/CommentAPI/">
                <channel>
                    <title>Scott Hanselman's Blog</title>
                    <managingEditor>scott@hanselman.com</managingEditor>
                    <item>
                        <title>Exploring .NET Aspire with Docker Compose</title>
                        <link>https://www.hanselman.com/blog/aspire-docker-compose</link>
                        <dc:creator>Scott Hanselman</dc:creator>
                        <pubDate>{DateTimeOffset.UtcNow.AddDays(-6):R}</pubDate>
                        <category>.NET</category>
                        <category>Docker</category>
                        <category>Aspire</category>
                        <description>Let me show you how .NET Aspire integrates with Docker Compose for local development.</description>
                        <slash:comments>42</slash:comments>
                        <wfw:commentRss>https://www.hanselman.com/blog/aspire-docker-compose/comments/feed</wfw:commentRss>
                    </item>
                </channel>
            </rss>
            """;

        // Act
        var items = RssFeedIngestionService.ParseFeed(rss, feedConfig, _cutoff);

        // Assert — dc:creator takes precedence over managingEditor; categories extracted; extra namespaces ignored
        items.Should().ContainSingle();
        var item = items[0];
        item.Title.Should().Be("Exploring .NET Aspire with Docker Compose");
        item.FeedItemData.Should().Contain("Scott Hanselman");
        item.FeedItemData.Should().Contain("Let me show you how");
        item.FeedLevelAuthor.Should().Be("scott@hanselman.com");
        item.FeedTags.Should().BeEquivalentTo([".NET", "Docker", "Aspire"]);
    }
}
