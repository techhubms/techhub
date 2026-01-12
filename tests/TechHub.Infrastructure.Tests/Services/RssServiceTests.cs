using FluentAssertions;
using TechHub.Core.Models;
using TechHub.Infrastructure.Services;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Unit tests for RssService RSS feed generation
/// </summary>
public class RssServiceTests
{
    private readonly RssService _rssService = new();

    private static Section CreateTestSection() => new()
    {
        Name = "ai",
        Title = "AI",
        Description = "Artificial Intelligence resources",
        Url = "/ai",
        BackgroundImage = "/images/ai.jpg",
        Collections =
        [
            new() { Name = "news", Title = "News", Url = "/ai/news", Description = "Latest AI news", IsCustom = false }
        ]
    };

    private static List<ContentItem> CreateTestItems() =>
    [
        new ContentItem
        {
            Slug = "2024-01-15-test-article-1",
            Title = "Test Article 1",
            Description = "Test description 1",
            Author = "John Doe",
            DateEpoch = 1705305600, // 2024-01-15
            CollectionName = "news",
            SectionNames = ["ai"],
            Tags = ["AI", "News", "Machine Learning", "Testing"],
            RenderedHtml = "<p>Test content 1</p>",
            Excerpt = "Test excerpt 1",
            ViewingMode = null,
            ExternalUrl = null,
            AltCollection = null,
            VideoId = null
        },
        new ContentItem
        {
            Slug = "2024-01-10-test-article-2",
            Title = "Test Article 2",
            Description = "Test description 2",
            Author = "Jane Smith",
            DateEpoch = 1704844800, // 2024-01-10
            CollectionName = "news",
            SectionNames = ["ai"],
            Tags = ["AI", "News", "Deep Learning"],
            RenderedHtml = "<p>Test content 2</p>",
            Excerpt = "Test excerpt 2",
            ViewingMode = "external",
            ExternalUrl = "https://example.com/article-2",
            AltCollection = null,
            VideoId = null
        }
    ];

    [Fact]
    public async Task GenerateSectionFeedAsync_WithValidSection_ReturnsChannelDto()
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
        channel.Link.Should().Be("https://tech.hub.ms/ai");
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
            .Select(i => new ContentItem
            {
                Slug = $"2024-01-{i:D2}-article-{i}",
                Title = $"Article {i}",
                Description = $"Description {i}",
                Author = "Test Author",
                DateEpoch = 1705305600 + (i * 86400), // Increment by 1 day
                CollectionName = "news",
                SectionNames = ["AI"],
                Tags = ["test"],
                RenderedHtml = $"<p>Content {i}</p>",
                Excerpt = $"Excerpt {i}",
                AltCollection = null,
                VideoId = null,
                ViewingMode = null,
                ExternalUrl = null
            })
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
    public async Task GenerateCollectionFeedAsync_WithValidCollection_ReturnsChannelDto()
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
        channel.Link.Should().Be("https://tech.hub.ms/all/news");
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
    public async Task RssItem_UsesExcerptWhenAvailable()
    {
        // Arrange
        var section = CreateTestSection();
        var items = CreateTestItems();

        // Act
        var channel = await _rssService.GenerateSectionFeedAsync(section, items);

        // Assert
        channel.Items.First().Description.Should().Be("Test excerpt 1");
    }

    [Fact]
    public async Task RssItem_FallsBackToDescriptionWhenExcerptEmpty()
    {
        // Arrange
        var section = CreateTestSection();
        var items = new List<ContentItem>
        {
            new()
            {
                Slug = "test",
                Title = "Test",
                Description = "Description only",
                DateEpoch = 1705305600,
                CollectionName = "news",
                SectionNames = ["AI"],
                Tags = [],
                RenderedHtml = "<p>Content</p>",
                Excerpt = "", // Empty excerpt
                AltCollection = null,
                VideoId = null,
                ViewingMode = null,
                ExternalUrl = null,
                Author = null
            }
        };

        // Act
        var channel = await _rssService.GenerateSectionFeedAsync(section, items);

        // Assert
        channel.Items.First().Description.Should().Be("Description only");
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
        var items = CreateTestItems();

        // Act
        var channel = await _rssService.GenerateSectionFeedAsync(section, items);

        // Assert
        var internalItem = channel.Items.First(); // First item is internal
        internalItem.Link.Should().StartWith("https://tech.hub.ms");
        internalItem.Guid.Should().StartWith("https://tech.hub.ms");
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
        channel.Items.First().SectionNames.Should().Contain("ai");
        channel.Items.Last().SectionNames.Should().Contain("ai");
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
        xml.Should().Contain("<link>https://tech.hub.ms/ai</link>");
        xml.Should().Contain("<language>en-us</language>");
        xml.Should().Contain("<item>");
        xml.Should().Contain("<title>Test Article 1</title>");
        xml.Should().Contain("<guid isPermaLink=\"true\">");
        xml.Should().Contain("<pubDate>");
        xml.Should().Contain("<author>John Doe</author>");
        xml.Should().Contain("<category>ai</category>");  // Section name, not tag
        xml.Should().Contain("</item>");
        xml.Should().Contain("</channel>");
        xml.Should().Contain("</rss>");
    }

    [Fact]
    public void SerializeToXml_WithNullChannel_ThrowsArgumentNullException()
    {
        // Arrange
        Core.DTOs.RssChannelDto? channel = null;

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
            new()
            {
                Slug = "test",
                Title = "Test & Special <Characters>",
                Description = "Description with \"quotes\" & <tags>",
                DateEpoch = 1705305600,
                CollectionName = "news",
                SectionNames = ["AI"],
                Tags = ["tag&special"],
                RenderedHtml = "<p>Content</p>",
                Excerpt = "Excerpt with 'quotes'",
                AltCollection = null,
                VideoId = null,
                ViewingMode = null,
                ExternalUrl = null,
                Author = null
            }
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
