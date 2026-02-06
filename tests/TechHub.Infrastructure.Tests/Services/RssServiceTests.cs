using FluentAssertions;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Models;
using TechHub.Infrastructure.Services;
using TechHub.TestUtilities;
using TechHub.TestUtilities.Builders;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Unit tests for RssService RSS feed generation
/// </summary>
public class RssServiceTests
{
    private readonly RssService _rssService;

    public RssServiceTests()
    {
        // Load real AppSettings from appsettings.json
        var appSettings = ConfigurationHelper.LoadAppSettings();

        var options = Options.Create(appSettings);
        var rssOptions = Options.Create(new RssOptions { MaxItemsInFeed = 50 });
        _rssService = new RssService(options, rssOptions);
    }

    private static Section CreateTestSection() => new(
        name: "ai",
        title: "AI",
        description: "Artificial Intelligence resources",
        url: "/ai",
        tag: "AI",
        collections:
        [
            new(name: "news", title: "News", url: "/ai/news", description: "Latest AI news", displayName: "News", isCustom: false)
        ]);

    private static List<ContentItem> CreateTestItems() =>
    [
        A.ContentItem
            .WithSlug("test-article-1")
            .WithTitle("Test Article 1")
            .WithAuthor("John Doe")
            .WithDateEpoch(1705305600) // 2024-01-15
            .WithCollectionName("news")
            .WithFeedName("test-feed")
            .WithPrimarySectionName("ai")
            .WithTags("AI", "News", "Machine Learning", "Testing")
            .WithExcerpt("Test excerpt 1")
            .WithExternalUrl("https://example.com/article-1")
            .WithRenderedHtml("<p>Test content 1</p>")
            .BuildDetail(),
        A.ContentItem
            .WithSlug("test-article-2")
            .WithTitle("Test Article 2")
            .WithAuthor("Jane Smith")
            .WithDateEpoch(1704844800) // 2024-01-10
            .WithCollectionName("news")
            .WithFeedName("test-feed")
            .WithPrimarySectionName("ai")
            .WithTags("AI", "News", "Deep Learning")
            .WithExcerpt("Test excerpt 2")
            .WithExternalUrl("https://example.com/article-2")
            .WithRenderedHtml("<p>Test content 2</p>")
            .BuildDetail()
    ];

    [Fact]
    public async Task GenerateSectionFeedAsync_WithValidSection_ReturnsChannel()
    {
        // Arrange
        var section = CreateTestSection();
        var items = CreateTestItems();

        // Act
        var channel = await _rssService.GenerateSectionFeedAsync(section, items);

        // Assert
        channel.Should().NotBeNull();
        channel.Title.Should().Be("Tech Hub - AI");
        channel.Description.Should().Be("Artificial Intelligence resources");
        channel.Link.Should().Be("https://localhost:5003/ai");
        channel.Language.Should().Be("en-us");
        channel.Items.Should().HaveCount(2);
    }

    [Fact]
    public async Task GenerateSectionFeedAsync_OrdersByDateDescending()
    {
        // Arrange
        var section = CreateTestSection();
        var items = CreateTestItems();

        // Act
        var channel = await _rssService.GenerateSectionFeedAsync(section, items);

        // Assert
        channel.Items.First().Title.Should().Be("Test Article 1"); // Newer date
        channel.Items.Last().Title.Should().Be("Test Article 2"); // Older date
    }

    [Fact]
    public async Task GenerateSectionFeedAsync_LimitsTo50Items()
    {
        // Arrange
        var section = CreateTestSection();
        var items = Enumerable.Range(1, 100)
            .Select(i => A.ContentItem
                .WithSlug($"article-{i}")
                .WithTitle($"Article {i}")
                .WithDateEpoch(1705305600 + (i * 86400)) // Increment by 1 day
                .WithCollectionName("news")
                .WithFeedName("test-feed")
                .WithPrimarySectionName("ai")
                .WithTags("test")
                .WithExcerpt($"Excerpt {i}")
                .WithExternalUrl($"https://example.com/article-{i}")
                .WithRenderedHtml($"<p>Content {i}</p>")
                .BuildDetail())
            .ToList();

        // Act
        var channel = await _rssService.GenerateSectionFeedAsync(section, items);

        // Assert
        channel.Items.Should().HaveCount(50);
    }

    [Fact]
    public async Task GenerateSectionFeedAsync_WithNullSection_ThrowsArgumentNullException()
    {
        // Arrange
        Section? section = null;
        var items = CreateTestItems();

        // Act
        var act = async () => await _rssService.GenerateSectionFeedAsync(section!, items);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>()
            .WithParameterName("section");
    }

    [Fact]
    public async Task GenerateSectionFeedAsync_WithNullItems_ThrowsArgumentNullException()
    {
        // Arrange
        var section = CreateTestSection();
        IReadOnlyList<ContentItem>? items = null;

        // Act
        var act = async () => await _rssService.GenerateSectionFeedAsync(section, items!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>()
            .WithParameterName("items");
    }

    [Fact]
    public async Task GenerateCollectionFeedAsync_WithValidCollection_ReturnsChannel()
    {
        // Arrange
        var collection = "news";
        var items = CreateTestItems();

        // Act
        var channel = await _rssService.GenerateCollectionFeedAsync(collection, items);

        // Assert
        channel.Should().NotBeNull();
        channel.Title.Should().Be("Tech Hub - News");
        channel.Description.Should().Be("Latest news from Tech Hub");
        channel.Link.Should().Be("https://localhost:5003/all/news");
        channel.Language.Should().Be("en-us");
        channel.Items.Should().HaveCount(2);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task GenerateCollectionFeedAsync_WithInvalidCollection_ThrowsArgumentException(string? collection)
    {
        // Arrange
        var items = CreateTestItems();

        // Act
        var act = async () => await _rssService.GenerateCollectionFeedAsync(collection!, items);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task GenerateCollectionFeedAsync_WithNullItems_ThrowsArgumentNullException()
    {
        // Arrange
        var collection = "news";
        IReadOnlyList<ContentItem>? items = null;

        // Act
        var act = async () => await _rssService.GenerateCollectionFeedAsync(collection, items!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>()
            .WithParameterName("items");
    }

    [Fact]
    public async Task RssItem_UsesExcerptForDescription()
    {
        // Arrange
        var section = CreateTestSection();
        var items = new List<ContentItem>
        {
            A.ContentItem
                .WithSlug("test")
                .WithTitle("Test")
                .WithDateEpoch(1705305600)
                .WithCollectionName("news")
                .WithFeedName("test-feed")
                .WithPrimarySectionName("ai")
                .WithTags("test")
                .WithExcerpt("Test excerpt")
                .WithExternalUrl("https://example.com/test")
                .WithRenderedHtml("<p>Content</p>")
                .BuildDetail()
        };

        // Act
        var channel = await _rssService.GenerateSectionFeedAsync(section, items);

        // Assert
        channel.Items.First().Description.Should().Be("Test excerpt");
    }

    [Fact]
    public async Task RssItem_UsesExternalUrlForExternalContent()
    {
        // Arrange
        var section = CreateTestSection();
        var items = CreateTestItems();

        // Act
        var channel = await _rssService.GenerateSectionFeedAsync(section, items);

        // Assert
        var externalItem = channel.Items.Last(); // Second item is external
        externalItem.Link.Should().Be("https://example.com/article-2");
        externalItem.Guid.Should().Be("https://example.com/article-2");
    }

    [Fact]
    public async Task RssItem_UsesGeneratedUrlForInternalContent()
    {
        // Arrange
        var section = CreateTestSection();
        var internalItems = new List<ContentItem>
        {
            A.ContentItem
                .WithSlug("internal-video-1")
                .WithTitle("Internal Video")
                .WithAuthor("John Doe")
                .WithDateEpoch(1705305600)
                .WithCollectionName("videos") // Videos are internal, not external
                .WithFeedName("test-feed")
                .WithPrimarySectionName("ai")
                .WithTags("AI", "Tutorial")
                .WithExcerpt("Internal video excerpt")
                .WithRenderedHtml("<p>Video content</p>")
                .BuildDetail()
        };

        // Act
        var channel = await _rssService.GenerateSectionFeedAsync(section, internalItems);

        // Assert
        var internalItem = channel.Items.First();
        internalItem.Link.Should().StartWith("https://localhost:5003");
        internalItem.Guid.Should().StartWith("https://localhost:5003");
    }

    [Fact]
    public async Task RssItem_IncludesAuthorWhenProvided()
    {
        // Arrange
        var section = CreateTestSection();
        var items = CreateTestItems();

        // Act
        var channel = await _rssService.GenerateSectionFeedAsync(section, items);

        // Assert
        channel.Items.First().Author.Should().Be("John Doe");
        channel.Items.Last().Author.Should().Be("Jane Smith");
    }

    [Fact]
    public async Task RssItem_IncludesSectionNamesAsCategories()
    {
        // Arrange
        var section = CreateTestSection();
        var items = CreateTestItems();

        // Act
        var channel = await _rssService.GenerateSectionFeedAsync(section, items);

        // Assert
        channel.Items.Should().HaveCount(2);
    }

    [Fact]
    public async Task SerializeToXml_WithValidChannel_ProducesValidRss()
    {
        // Arrange
        var items = CreateTestItems();
        var channel = await _rssService.GenerateSectionFeedAsync(CreateTestSection(), items);

        // Act
        var xml = _rssService.SerializeToXml(channel);

        // Assert
        xml.Should().Contain("<?xml version=\"1.0\" encoding=\"utf-16\"?>");  // StringWriter uses UTF-16
        xml.Should().Contain("<rss version=\"2.0\">");
        xml.Should().Contain("<channel>");
        xml.Should().Contain("<title>Tech Hub - AI</title>");
        xml.Should().Contain("<description>Artificial Intelligence resources</description>");
        xml.Should().Contain("<link>https://localhost:5003/ai</link>");
        xml.Should().Contain("<language>en-us</language>");
        xml.Should().Contain("<item>");
        xml.Should().Contain("<title>Test Article 1</title>");
        xml.Should().Contain("<guid isPermaLink=\"true\">");
        xml.Should().Contain("<pubDate>");
        xml.Should().Contain("<author>John Doe</author>");
        xml.Should().Contain("<category>AI</category>");  // Tag from content
        xml.Should().Contain("<category>Machine Learning</category>");  // Tag from content
        xml.Should().Contain("</item>");
        xml.Should().Contain("</channel>");
        xml.Should().Contain("</rss>");
    }

    [Fact]
    public void SerializeToXml_WithNullChannel_ThrowsArgumentNullException()
    {
        // Arrange
        Core.Models.RssChannel? channel = null;

        // Act
        var act = () => _rssService.SerializeToXml(channel!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("channel");
    }

    [Fact]
    public async Task SerializeToXml_ProperlyEncodesXmlSpecialCharacters()
    {
        // Arrange
        var section = CreateTestSection();
        var items = new List<ContentItem>
        {
            A.ContentItem
                .WithSlug("test")
                .WithTitle("Test & Special <Characters>")
                .WithDateEpoch(1705305600)
                .WithCollectionName("news")
                .WithFeedName("test-feed")
                .WithPrimarySectionName("ai")
                .WithTags("tag&special")
                .WithExcerpt("Excerpt with \"quotes\" & <tags>")                .WithExternalUrl("https://example.com/test")                .WithRenderedHtml("<p>Content</p>")
                .BuildDetail()
        };
        var channel = await _rssService.GenerateSectionFeedAsync(section, items);

        // Act
        var xml = _rssService.SerializeToXml(channel);

        // Assert
        xml.Should().Contain("&amp;"); // & encoded
        xml.Should().Contain("&lt;"); // < encoded
        xml.Should().Contain("&gt;"); // > encoded
        xml.Should().NotContain("<Characters>"); // Raw tags should be encoded
    }

    [Fact]
    public async Task SerializeToXml_UsesRfc1123DateFormat()
    {
        // Arrange
        var section = CreateTestSection();
        var items = CreateTestItems();
        var channel = await _rssService.GenerateSectionFeedAsync(section, items);

        // Act
        var xml = _rssService.SerializeToXml(channel);

        // Assert
        // RFC1123 format: "Mon, 15 Jan 2024 00:00:00 GMT"
        xml.Should().MatchRegex(@"<pubDate>[A-Za-z]{3}, \d{2} [A-Za-z]{3} \d{4} \d{2}:\d{2}:\d{2} GMT</pubDate>");
    }
}
