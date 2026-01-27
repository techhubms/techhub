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
}
