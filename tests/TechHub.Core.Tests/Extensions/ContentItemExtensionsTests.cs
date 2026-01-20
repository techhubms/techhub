using FluentAssertions;
using TechHub.Core.Extensions;
using TechHub.Core.Models;

namespace TechHub.Core.Tests.Extensions;

/// <summary>
/// Unit tests for ContentItemExtensions mapping methods
/// </summary>
public class ContentItemExtensionsTests
{
    private static ContentItem CreateTestContentItem()
    {
        return new ContentItem
        {
            Slug = "2024-01-15-test-article",
            Title = "Test Article",
            Description = "Test description",
            Author = "Test Author",
            DateEpoch = 1705305600, // 2024-01-15 00:00:00 UTC
            CollectionName = "news",
            SectionNames = ["ai", "azure"],
            Tags = ["AI", "Azure", "News", "Machine Learning", "Cloud"],
            RenderedHtml = "<p>Test content</p>",
            Excerpt = "Test excerpt",
            ExternalUrl = "https://example.com",
            ViewingMode = null
        };
    }

    [Fact]
    public void ToDto_ConvertsAllProperties()
    {
        // Arrange
        var item = CreateTestContentItem();
        var sectionUrl = "/ai";

        // Act
        var dto = item.ToDto(sectionUrl);

        // Assert
        dto.Slug.Should().Be("2024-01-15-test-article");
        dto.Title.Should().Be("Test Article");
        dto.Description.Should().Be("Test description");
        dto.Author.Should().Be("Test Author");
        dto.DateEpoch.Should().Be(1705305600);
        dto.DateIso.Should().Be("2024-01-15");
        dto.CollectionName.Should().Be("news");
        dto.SectionNames.Should().BeEquivalentTo(["ai", "azure"]);
        dto.PrimarySection.Should().Be("ai"); // First section in priority order
        dto.Tags.Should().BeEquivalentTo(["AI", "Azure", "News", "Machine Learning", "Cloud"]);
        dto.Excerpt.Should().Be("Test excerpt");
        dto.ExternalUrl.Should().Be("https://example.com");
        // dto.Url.Should().Be("/ai/news/2024-01-15-test-article");
    }

    [Fact]
    public void ToDto_ThrowsArgumentNullException_WhenItemIsNull()
    {
        // Arrange
        ContentItem? item = null;
        var sectionUrl = "/ai";

        // Act
        var act = () => item!.ToDto(sectionUrl);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToDetailDto_ConvertsAllProperties()
    {
        // Arrange
        var item = CreateTestContentItem();
        var sectionUrl = "/ai";

        // Act
        var dto = item.ToDetailDto(sectionUrl);

        // Assert
        dto.Slug.Should().Be("2024-01-15-test-article");
        dto.Title.Should().Be("Test Article");
        dto.Description.Should().Be("Test description");
        dto.Author.Should().Be("Test Author");
        dto.DateEpoch.Should().Be(1705305600);
        dto.DateIso.Should().Be("2024-01-15");
        dto.CollectionName.Should().Be("news");
        dto.SectionNames.Should().BeEquivalentTo(["ai", "azure"]);
        dto.PrimarySection.Should().Be("ai");
        dto.Tags.Should().BeEquivalentTo(["AI", "Azure", "News", "Machine Learning", "Cloud"]);
        dto.RenderedHtml.Should().Be("<p>Test content</p>");
        dto.Excerpt.Should().Be("Test excerpt");
        dto.ExternalUrl.Should().Be("https://example.com");
        dto.Url.Should().Be("/ai/news/2024-01-15-test-article");
    }

    [Fact]
    public void ToDetailDto_ThrowsArgumentNullException_WhenItemIsNull()
    {
        // Arrange
        ContentItem? item = null;
        var sectionUrl = "/ai";

        // Act
        var act = () => item!.ToDetailDto(sectionUrl);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToDtos_ConvertsMultipleItems()
    {
        // Arrange
        var items = new List<ContentItem>
        {
            CreateTestContentItem(),
            new ContentItem
            {
                Slug = "2024-01-16-test-article-2",
                Title = "Test Article 2",
                Description = "Test description",
                Author = "Test Author",
                DateEpoch = 1705305600,
                CollectionName = "news",
                SectionNames = ["ai"],
                Tags = ["AI", "News", "test"],
                RenderedHtml = "<p>Test</p>",
                Excerpt = "Test excerpt"
            }
        };
        var sectionUrl = "/ai";

        // Act
        var dtos = items.ToDtos(sectionUrl);

        // Assert
        dtos.Should().HaveCount(2);
        dtos[0].Slug.Should().Be("2024-01-15-test-article");
        dtos[0].Title.Should().Be("Test Article");
        dtos[0].PrimarySection.Should().Be("ai");
        dtos[1].Slug.Should().Be("2024-01-16-test-article-2");
        dtos[1].Title.Should().Be("Test Article 2");
        dtos[1].PrimarySection.Should().Be("ai");
    }

    [Fact]
    public void ToDtos_ReturnsEmptyList_WhenNoItems()
    {
        // Arrange
        var items = Enumerable.Empty<ContentItem>();
        var sectionUrl = "/ai";

        // Act
        var dtos = items.ToDtos(sectionUrl);

        // Assert
        dtos.Should().BeEmpty();
    }

    [Theory]
    [InlineData("/ai", "/ai/news/2024-01-15-test-article")]
    [InlineData("/github-copilot", "/github-copilot/news/2024-01-15-test-article")]
    [InlineData("/azure", "/azure/news/2024-01-15-test-article")]
    public void ToDto_GeneratesCorrectUrl_ForDifferentSections(string sectionUrl, string expectedUrl)
    {
        // Arrange
        var item = CreateTestContentItem();

        // Act
        var dto = item.ToDto(sectionUrl);

        // Assert
        dto.Url.Should().Be(expectedUrl);
    }
}
