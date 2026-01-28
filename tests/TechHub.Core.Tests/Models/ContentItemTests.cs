using FluentAssertions;
using TechHub.Core.Models;
using TechHub.TestUtilities.Builders;

namespace TechHub.Core.Tests.Models;

/// <summary>
/// Unit tests for ContentItem domain model
/// </summary>
public class ContentItemTests
{
    private static ContentItem CreateValidContentItem()
    {
        return A.ContentItem
            .WithSlug("test-article")
            .WithTitle("Test Article")
            .WithAuthor("Test Author")
            .WithDateEpoch(1705305600)
            .WithCollectionName("news")
            .WithFeedName("test-feed")
            .WithTags("AI", "News", "Machine Learning")
            .WithExternalUrl("https://example.com")
            .WithRenderedHtml("<p>Test content</p>")
            .Build();
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
        // Arrange & Act
        var act = () => A.ContentItem
            .WithDateEpoch(1704844800)
            .WithCollectionName("news")
            .WithExternalUrl("https://example.com")
            .Build();

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
        var item = A.ContentItem
            .WithSlug("test-slug")
            .WithTitle("Test Title")
            .WithAuthor("Test Author")
            .WithDateEpoch(1705305600)
            .WithCollectionName("news")
            .WithExternalUrl("https://example.com")
            .Build();

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
        var contentItem = CreateContentItemWithCollection("videos");

        // Act
        var result = contentItem.GetHref();

        // Assert
        result.Should().Be("/ai/videos/test-slug");
    }

    [Fact]
    public void GetHref_IgnoresExternalUrl_ForInternalCollections()
    {
        // Arrange - internal collection ("videos") ignores externalUrl parameter
        var contentItem = CreateContentItemWithCollection("videos", externalUrl: "https://ignored.com");

        // Act
        var result = contentItem.GetHref();

        // Assert - should build URL from section/collection/slug, not use externalUrl
        result.Should().Be("/ai/videos/test-slug");
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
        string externalUrl = "https://example.com",
        string title = "Test Title")
    {
        return A.ContentItem
            .WithTitle(title)
            .WithDateEpoch(1704067200)
            .WithCollectionName(collectionName)
            .WithExternalUrl(externalUrl)
            .Build();
    }
}
