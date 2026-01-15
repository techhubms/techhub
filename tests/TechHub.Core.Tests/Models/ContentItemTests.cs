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
            Slug = "2024-01-15-test-article",
            Title = "Test Article",
            Description = "Test description",
            Author = "Test Author",
            DateEpoch = 1705305600, // 2024-01-15 00:00:00 UTC
            CollectionName = "news",
            AltCollection = null,
            SectionNames = ["ai"],
            Tags = ["AI", "News", "Machine Learning"],
            RenderedHtml = "<p>Test content</p>",
            Excerpt = "Test excerpt",
            ExternalUrl = null,
            VideoId = null,
            ViewingMode = null
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

    /// <summary>
    /// Test: DateIso property formats Unix epoch as ISO date string (YYYY-MM-DD)
    /// Why: Frontend needs consistent date format for display and filtering
    /// </summary>
    [Fact]
    public void DateIso_FormatsDateCorrectly()
    {
        // Arrange
        var item = CreateValidContentItem(); // DateEpoch = 1705305600 = 2024-01-15 08:00:00 UTC

        // Act
        var dateIso = item.DateIso;

        // Assert
        dateIso.Should().Be("2024-01-15");
    }

    /// <summary>
    /// Test: DateIso handles various epoch timestamps correctly
    /// Why: Verify date formatting works across different years
    /// </summary>
    [Theory]
    [InlineData(1705305600, "2024-01-15")] // 2024-01-15 08:00:00 UTC
    [InlineData(1672531200, "2023-01-01")] // 2023-01-01 00:00:00 UTC
    [InlineData(1735689600, "2025-01-01")] // 2025-01-01 00:00:00 UTC
    public void DateIso_VariousEpochValues_FormatsCorrectly(long epoch, string expectedIso)
    {
        // Arrange
        var item = new ContentItem
        {
            Slug = "test-slug",
            Title = "Test",
            Description = "Test",
            DateEpoch = epoch,
            CollectionName = "news",
            SectionNames = ["ai"],
            Tags = ["test"],
            RenderedHtml = "<p>Test</p>",
            Excerpt = "Test"
        };

        // Act
        var dateIso = item.DateIso;

        // Assert
        dateIso.Should().Be(expectedIso);
    }

    /// <summary>
    /// Test: GetUrlInSection constructs content URL with section prefix
    /// Why: Generate correct routing URLs for content items in section context
    /// </summary>
    [Fact]
    public void GetUrlInSection_ValidSection_GeneratesCorrectUrl()
    {
        // Arrange
        var item = CreateValidContentItem();
        var sectionUrl = "/ai";

        // Act
        var url = item.GetUrlInSection(sectionUrl);

        // Assert
        url.Should().Be("/ai/news/2024-01-15-test-article");
    }

    /// <summary>
    /// Test: GetUrlInSection handles various section/collection/slug combinations
    /// Why: URL generation must work for all content types
    /// </summary>
    [Theory]
    [InlineData("/ai", "news", "test-slug", "/ai/news/test-slug")]
    [InlineData("/github-copilot", "videos", "video-demo", "/github-copilot/videos/video-demo")]
    [InlineData("/azure", "blogs", "2024-01-01-blog", "/azure/blogs/2024-01-01-blog")]
    public void GetUrlInSection_VariousParameters_GeneratesCorrectUrls(
        string sectionUrl, string collectionName, string slug, string expectedUrl)
    {
        // Arrange
        var item = new ContentItem
        {
            Slug = slug,
            Title = "Test",
            Description = "Test",
            DateEpoch = 1705305600,
            CollectionName = collectionName,
            SectionNames = ["ai"],
            Tags = ["test"],
            RenderedHtml = "<p>Test</p>",
            Excerpt = "Test"
        };

        // Act
        var url = item.GetUrlInSection(sectionUrl);

        // Assert
        url.Should().Be(expectedUrl);
    }

    /// <summary>
    /// Test: Validate passes for fully populated valid content item
    /// Why: Ensure validation rules don't reject valid content
    /// </summary>
    [Fact]
    public void Validate_ValidItem_PassesWithoutException()
    {
        // Arrange
        var item = CreateValidContentItem();

        // Act
        var act = () => item.Validate();

        // Assert
        act.Should().NotThrow();
    }

    /// <summary>
    /// Test: Validate throws ArgumentException when Slug is empty string
    /// Why: Slug is required for URL generation
    /// </summary>
    [Fact]
    public void Validate_EmptySlug_ThrowsArgumentException()
    {
        // Arrange
        var item = new ContentItem
        {
            Slug = "",
            Title = "Test",
            Description = "Test",
            DateEpoch = 1705305600,
            CollectionName = "news",
            SectionNames = ["ai"],
            Tags = ["test"],
            RenderedHtml = "<p>Test</p>",
            Excerpt = "Test"
        };

        // Act
        var act = () => item.Validate();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*slug*")
            .WithParameterName("Slug");
    }

    /// <summary>
    /// Test: Validate throws ArgumentException when Slug is whitespace only
    /// Why: Whitespace-only slug is invalid for URLs
    /// </summary>
    [Fact]
    public void Validate_WhitespaceSlug_ThrowsArgumentException()
    {
        // Arrange
        var item = new ContentItem
        {
            Slug = "   ",
            Title = "Test",
            Description = "Test",
            DateEpoch = 1705305600,
            CollectionName = "news",
            SectionNames = ["ai"],
            Tags = ["test"],
            RenderedHtml = "<p>Test</p>",
            Excerpt = "Test"
        };

        // Act
        var act = () => item.Validate();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("Slug");
    }

    /// <summary>
    /// Test: Validate throws ArgumentException when Title is empty
    /// Why: Title is required for content display
    /// </summary>
    [Fact]
    public void Validate_EmptyTitle_ThrowsArgumentException()
    {
        // Arrange
        var item = new ContentItem
        {
            Slug = "test-slug",
            Title = "",
            Description = "Test",
            DateEpoch = 1705305600,
            CollectionName = "news",
            SectionNames = ["ai"],
            Tags = ["test"],
            RenderedHtml = "<p>Test</p>",
            Excerpt = "Test"
        };

        // Act
        var act = () => item.Validate();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*title*")
            .WithParameterName("Title");
    }

    /// <summary>
    /// Test: Validate throws ArgumentException when DateEpoch is zero or negative
    /// Why: DateEpoch must be positive Unix timestamp
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Validate_InvalidDateEpoch_ThrowsArgumentException(long invalidEpoch)
    {
        // Arrange
        var item = new ContentItem
        {
            Slug = "test-slug",
            Title = "Test",
            Description = "Test",
            DateEpoch = invalidEpoch,
            CollectionName = "news",
            SectionNames = ["ai"],
            Tags = ["test"],
            RenderedHtml = "<p>Test</p>",
            Excerpt = "Test"
        };

        // Act
        var act = () => item.Validate();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*epoch*")
            .WithParameterName("DateEpoch");
    }

    /// <summary>
    /// Test: Validate throws ArgumentException when SectionNames is empty array
    /// Why: Content must belong to at least one section
    /// </summary>
    [Fact]
    public void Validate_EmptySectionNames_ThrowsArgumentException()
    {
        // Arrange
        var item = new ContentItem
        {
            Slug = "test-slug",
            Title = "Test",
            Description = "Test",
            DateEpoch = 1705305600,
            CollectionName = "news",
            SectionNames = [],
            Tags = ["test"],
            RenderedHtml = "<p>Test</p>",
            Excerpt = "Test"
        };

        // Act
        var act = () => item.Validate();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*section*")
            .WithParameterName("SectionNames");
    }

    /// <summary>
    /// Test: Validate throws ArgumentException when RenderedHtml is empty
    /// Why: RenderedHtml is required for content display
    /// </summary>
    [Fact]
    public void Validate_EmptyRenderedHtml_ThrowsArgumentException()
    {
        // Arrange
        var item = new ContentItem
        {
            Slug = "test-slug",
            Title = "Test",
            Description = "Test",
            DateEpoch = 1705305600,
            CollectionName = "news",
            SectionNames = ["ai"],
            Tags = ["test"],
            RenderedHtml = "",
            Excerpt = "Test"
        };

        // Act
        var act = () => item.Validate();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*HTML*")
            .WithParameterName("RenderedHtml");
    }

    /// <summary>
    /// Test: Validate throws ArgumentException when Excerpt exceeds 1000 characters
    /// Why: Excerpt length limit prevents excessive UI text
    /// </summary>
    [Fact]
    public void Validate_ExcerptOver1000Characters_ThrowsArgumentException()
    {
        // Arrange
        var longExcerpt = new string('a', 1001); // 1001 characters
        var item = new ContentItem
        {
            Slug = "test-slug",
            Title = "Test",
            Description = "Test",
            DateEpoch = 1705305600,
            CollectionName = "news",
            SectionNames = ["ai"],
            Tags = ["test"],
            RenderedHtml = "<p>Test</p>",
            Excerpt = longExcerpt
        };

        // Act
        var act = () => item.Validate();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*1000*")
            .WithParameterName("Excerpt");
    }

    /// <summary>
    /// Test: Validate passes when Excerpt is exactly 1000 characters (boundary)
    /// Why: Verify boundary condition for excerpt length validation
    /// </summary>
    [Fact]
    public void Validate_ExcerptExactly1000Characters_Passes()
    {
        // Arrange
        var excerpt = new string('a', 1000);
        var item = new ContentItem
        {
            Slug = "test-slug",
            Title = "Test",
            Description = "Test",
            DateEpoch = 1705305600,
            CollectionName = "news",
            SectionNames = ["ai"],
            Tags = ["test"],
            RenderedHtml = "<p>Test</p>",
            Excerpt = excerpt
        };

        // Act
        var act = () => item.Validate();

        // Assert
        act.Should().NotThrow();
    }

    /// <summary>
    /// Test: Validate passes when optional fields are null
    /// Why: Ensure optional fields (Author, ExternalUrl, VideoId, ViewingMode) are truly optional
    /// </summary>
    [Fact]
    public void Validate_OptionalFieldsNull_Passes()
    {
        // Arrange
        var item = new ContentItem
        {
            Slug = "test-slug",
            Title = "Test",
            Description = "Test",
            Author = null,
            DateEpoch = 1705305600,
            CollectionName = "news",
            AltCollection = null,
            SectionNames = ["ai"],
            Tags = ["test"],
            RenderedHtml = "<p>Test</p>",
            Excerpt = "Test",
            ExternalUrl = null,
            VideoId = null,
            ViewingMode = null
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
            Description = "Test Description",
            Author = "Test Author",
            DateEpoch = 1705305600,
            CollectionName = "news",
            SectionNames = ["ai"],
            Tags = ["test"],
            RenderedHtml = "<p>Test</p>",
            Excerpt = "Test excerpt"
        };

        // Assert
        item.Title.Should().Be("Test Title");
        item.Author.Should().Be("Test Author");
        item.Slug.Should().Be("test-slug");
        item.DateEpoch.Should().Be(1705305600);
    }
}
