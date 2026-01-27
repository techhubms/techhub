using FluentAssertions;
using TechHub.Core.Models;

namespace TechHub.Core.Tests.Models;

/// <summary>
/// Unit tests for ContentItem domain model
/// </summary>
public class ContentItemTests
{
    private static ContentItem CreateValidContentItem()
    {
        return new ContentItem
        {
            Slug = "test-article",
            Title = "Test Article",
            Author = "Test Author",
            DateEpoch = 1705305600, // 2024-01-15 00:00:00 UTC
            DateIso = "2024-01-15",
            CollectionName = "news",
            SectionNames = ["ai"],
            PrimarySectionName = "ai",
            Tags = ["AI", "News", "Machine Learning"],
            RenderedHtml = "<p>Test content</p>",
            Excerpt = "Test excerpt",
            ExternalUrl = null,
            Url = "/ai/news/test-article"
        };
    }

    [Fact]
    public void DateUtc_ConvertsEpochToDateTime()
    {
        // Arrange
        var item = CreateValidContentItem(); // DateEpoch = 1705305600 = 2024-01-15 08:00:00 UTC

        // Act
        var dateUtc = item.DateUtc;

        // Assert
        dateUtc.Year.Should().Be(2024);
        dateUtc.Month.Should().Be(1);
        dateUtc.Day.Should().Be(15);
        dateUtc.Hour.Should().Be(8);  // 1705305600 is 08:00:00 UTC, not 00:00:00
        dateUtc.Minute.Should().Be(0);
        dateUtc.Second.Should().Be(0);
        dateUtc.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Fact]
    public void Validate_WithValidData_DoesNotThrow()
    {
        // Arrange
        var item = new ContentItem
        {
            Slug = "test-slug",
            Title = "Test Title",
            DateEpoch = 1704844800,
            DateIso = "2024-01-10",
            CollectionName = "news",
            SectionNames = ["ai"],
            PrimarySectionName = "ai",
            Tags = ["test"],
            RenderedHtml = "<p>Test</p>",
            Excerpt = "Test excerpt",
            Url = "/ai/news/test-slug"
        };

        // Act
        var act = () => item.Validate();

        // Assert
        act.Should().NotThrow();
    }

    /// <summary>
    /// Test: ContentItem properties can be set during initialization
    /// Why: Verify record type with init-only properties works as expected
    /// </summary>
    [Fact]
    public void ContentItem_InitOnlyProperties_CanBeSetDuringInitialization()
    {
        // Arrange & Act
        var item = new ContentItem
        {
            Slug = "test-slug",
            Title = "Test Title",
            Author = "Test Author",
            DateEpoch = 1705305600,
            DateIso = "2024-01-15",
            CollectionName = "news",
            SectionNames = ["ai"],
            PrimarySectionName = "ai",
            Tags = ["test"],
            RenderedHtml = "<p>Test</p>",
            Excerpt = "Test excerpt",
            Url = "/ai/news/test-slug"
        };

        // Assert
        item.Title.Should().Be("Test Title");
        item.Author.Should().Be("Test Author");
        item.Slug.Should().Be("test-slug");
        item.DateEpoch.Should().Be(1705305600);
    }

    #region Link Behavior Tests

    [Theory]
    [InlineData("news", true)]
    [InlineData("blogs", true)]
    [InlineData("community", true)]
    [InlineData("videos", false)]
    [InlineData("roundups", false)]
    [InlineData("custom", false)]
    public void LinksExternally_ReturnsCorrectValue_BasedOnCollectionName(string collectionName, bool expected)
    {
        // Arrange
        var contentItem = CreateContentItemWithCollection(collectionName);

        // Act
        var result = contentItem.LinksExternally();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void GetHref_ReturnsExternalUrl_ForExternalCollections()
    {
        // Arrange
        var contentItem = CreateContentItemWithCollection("news", externalUrl: "https://example.com/article");

        // Act
        var result = contentItem.GetHref();

        // Assert
        result.Should().Be("https://example.com/article");
    }

    [Fact]
    public void GetHref_ReturnsUrl_ForInternalCollections()
    {
        // Arrange
        var contentItem = CreateContentItemWithCollection("videos", url: "/ai/videos/test-video");

        // Act
        var result = contentItem.GetHref();

        // Assert
        result.Should().Be("/ai/videos/test-video");
    }

    [Fact]
    public void GetHref_ReturnsEmptyString_WhenExternalUrlIsNull()
    {
        // Arrange
        var contentItem = CreateContentItemWithCollection("news", externalUrl: null);

        // Act
        var result = contentItem.GetHref();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetTarget_ReturnsBlank_ForExternalCollections()
    {
        // Arrange
        var contentItem = CreateContentItemWithCollection("blogs");

        // Act
        var result = contentItem.GetTarget();

        // Assert
        result.Should().Be("_blank");
    }

    [Fact]
    public void GetTarget_ReturnsNull_ForInternalCollections()
    {
        // Arrange
        var contentItem = CreateContentItemWithCollection("videos");

        // Act
        var result = contentItem.GetTarget();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetRel_ReturnsNoopenerNoreferrer_ForExternalCollections()
    {
        // Arrange
        var contentItem = CreateContentItemWithCollection("community");

        // Act
        var result = contentItem.GetRel();

        // Assert
        result.Should().Be("noopener noreferrer");
    }

    [Fact]
    public void GetRel_ReturnsNull_ForInternalCollections()
    {
        // Arrange
        var contentItem = CreateContentItemWithCollection("roundups");

        // Act
        var result = contentItem.GetRel();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetAriaLabel_IncludesOpensInNewTab_ForExternalCollections()
    {
        // Arrange
        var contentItem = CreateContentItemWithCollection("news", title: "Breaking News");

        // Act
        var result = contentItem.GetAriaLabel();

        // Assert
        result.Should().Be("Breaking News - opens in new tab");
    }

    [Fact]
    public void GetAriaLabel_ReturnsTitle_ForInternalCollections()
    {
        // Arrange
        var contentItem = CreateContentItemWithCollection("videos", title: "Tutorial Video");

        // Act
        var result = contentItem.GetAriaLabel();

        // Assert
        result.Should().Be("Tutorial Video");
    }

    [Fact]
    public void GetDataEnhanceNav_ReturnsTrue_ForInternalCollections()
    {
        // Arrange
        var contentItem = CreateContentItemWithCollection("videos");

        // Act
        var result = contentItem.GetDataEnhanceNav();

        // Assert
        result.Should().Be("true");
    }

    [Fact]
    public void GetDataEnhanceNav_ReturnsNull_ForExternalCollections()
    {
        // Arrange
        var contentItem = CreateContentItemWithCollection("news");

        // Act
        var result = contentItem.GetDataEnhanceNav();

        // Assert
        result.Should().BeNull();
    }

    #endregion

    private static ContentItem CreateContentItemWithCollection(
        string collectionName,
        string? externalUrl = "https://example.com",
        string? url = "/test/url",
        string? title = "Test Title")
    {
        return new ContentItem
        {
            Slug = "test-slug",
            Title = title ?? "Test Title",
            Author = "Test Author",
            DateEpoch = 1704067200,
            DateIso = "2024-01-01T00:00:00Z",
            CollectionName = collectionName,
            SectionNames = ["ai"],
            PrimarySectionName = "ai",
            Tags = [],
            Excerpt = "Test excerpt",
            ExternalUrl = externalUrl,
            Url = url ?? "/test/url"
        };
    }
}
