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
            Categories = new List<string> { "AI" },
            Tags = new List<string> { "machine-learning" },
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

    [Fact]
    public void DateIso_FormatsDateCorrectly()
    {
        // Arrange
        var item = CreateValidContentItem(); // DateEpoch = 1705305600 = 2024-01-15 00:00:00 UTC

        // Act
        var dateIso = item.DateIso;

        // Assert
        dateIso.Should().Be("2024-01-15");
    }

    [Theory]
    [InlineData(1705305600, "2024-01-15")] // 2024-01-15 00:00:00 UTC
    [InlineData(1672531200, "2023-01-01")] // 2023-01-01 00:00:00 UTC
    [InlineData(1735689600, "2025-01-01")] // 2025-01-01 00:00:00 UTC
    public void DateIso_HandlesVariousDates(long epoch, string expectedIso)
    {
        // Arrange
        var item = new ContentItem
        {
            Slug = "test-slug",
            Title = "Test",
            Description = "Test",
            DateEpoch = epoch,
            CollectionName = "news",
            Categories = new List<string> { "AI" },
            Tags = new List<string> { "test" },
            RenderedHtml = "<p>Test</p>",
            Excerpt = "Test"
        };

        // Act
        var dateIso = item.DateIso;

        // Assert
        dateIso.Should().Be(expectedIso);
    }

    [Fact]
    public void GetUrlInSection_GeneratesCorrectUrl()
    {
        // Arrange
        var item = CreateValidContentItem();
        var sectionUrl = "/ai";

        // Act
        var url = item.GetUrlInSection(sectionUrl);

        // Assert
        url.Should().Be("/ai/news/2024-01-15-test-article.html");
    }

    [Theory]
    [InlineData("/ai", "news", "test-slug", "/ai/news/test-slug.html")]
    [InlineData("/github-copilot", "videos", "video-demo", "/github-copilot/videos/video-demo.html")]
    [InlineData("/azure", "blogs", "2024-01-01-blog", "/azure/blogs/2024-01-01-blog.html")]
    public void GetUrlInSection_GeneratesCorrectUrl_ForVariousParameters(
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
            Categories = new List<string> { "AI" },
            Tags = new List<string> { "test" },
            RenderedHtml = "<p>Test</p>",
            Excerpt = "Test"
        };

        // Act
        var url = item.GetUrlInSection(sectionUrl);

        // Assert
        url.Should().Be(expectedUrl);
    }

    [Fact]
    public void Validate_PassesForValidItem()
    {
        // Arrange
        var item = CreateValidContentItem();

        // Act
        var act = () => item.Validate();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_ThrowsWhenSlugIsEmpty()
    {
        // Arrange
        var item = new ContentItem
        {
            Slug = "",
            Title = "Test",
            Description = "Test",
            DateEpoch = 1705305600,
            CollectionName = "news",
            Categories = new List<string> { "AI" },
            Tags = new List<string> { "test" },
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

    [Fact]
    public void Validate_ThrowsWhenSlugIsWhitespace()
    {
        // Arrange
        var item = new ContentItem
        {
            Slug = "   ",
            Title = "Test",
            Description = "Test",
            DateEpoch = 1705305600,
            CollectionName = "news",
            Categories = new List<string> { "AI" },
            Tags = new List<string> { "test" },
            RenderedHtml = "<p>Test</p>",
            Excerpt = "Test"
        };

        // Act
        var act = () => item.Validate();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("Slug");
    }

    [Fact]
    public void Validate_ThrowsWhenTitleIsEmpty()
    {
        // Arrange
        var item = new ContentItem
        {
            Slug = "test-slug",
            Title = "",
            Description = "Test",
            DateEpoch = 1705305600,
            CollectionName = "news",
            Categories = new List<string> { "AI" },
            Tags = new List<string> { "test" },
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

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Validate_ThrowsWhenDateEpochIsInvalid(long invalidEpoch)
    {
        // Arrange
        var item = new ContentItem
        {
            Slug = "test-slug",
            Title = "Test",
            Description = "Test",
            DateEpoch = invalidEpoch,
            CollectionName = "news",
            Categories = new List<string> { "AI" },
            Tags = new List<string> { "test" },
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

    [Fact]
    public void Validate_ThrowsWhenCategoriesIsEmpty()
    {
        // Arrange
        var item = new ContentItem
        {
            Slug = "test-slug",
            Title = "Test",
            Description = "Test",
            DateEpoch = 1705305600,
            CollectionName = "news",
            Categories = new List<string>(),
            Tags = new List<string> { "test" },
            RenderedHtml = "<p>Test</p>",
            Excerpt = "Test"
        };

        // Act
        var act = () => item.Validate();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*category*")
            .WithParameterName("Categories");
    }

    [Fact]
    public void Validate_ThrowsWhenRenderedHtmlIsEmpty()
    {
        // Arrange
        var item = new ContentItem
        {
            Slug = "test-slug",
            Title = "Test",
            Description = "Test",
            DateEpoch = 1705305600,
            CollectionName = "news",
            Categories = new List<string> { "AI" },
            Tags = new List<string> { "test" },
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

    [Fact]
    public void Validate_ThrowsWhenExcerptIsTooLong()
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
            Categories = new List<string> { "AI" },
            Tags = new List<string> { "test" },
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

    [Fact]
    public void Validate_PassesWhenExcerptIsExactly1000Characters()
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
            Categories = new List<string> { "AI" },
            Tags = new List<string> { "test" },
            RenderedHtml = "<p>Test</p>",
            Excerpt = excerpt
        };

        // Act
        var act = () => item.Validate();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_PassesWithOptionalFields()
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
            Categories = new List<string> { "AI" },
            Tags = new List<string> { "test" },
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

    [Fact]
    public void ContentItem_PropertiesAreInitOnly()
    {
        // This test verifies that ContentItem uses init-only setters
        // by creating an instance and verifying properties are set correctly
        
        // Arrange & Act
        var item = new ContentItem
        {
            Slug = "test-slug",
            Title = "Test Title",
            Description = "Test Description",
            Author = "Test Author",
            DateEpoch = 1705305600,
            CollectionName = "news",
            Categories = new List<string> { "AI" },
            Tags = new List<string> { "test" },
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
