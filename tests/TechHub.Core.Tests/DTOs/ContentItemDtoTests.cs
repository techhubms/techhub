
using TechHub.Core.DTOs;

namespace TechHub.Core.Tests.DTOs;

public class ContentItemDtoTests
{
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
        var dto = CreateTestDto(collectionName);

        // Act
        var result = dto.LinksExternally();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetHref_ReturnsExternalUrl_ForExternalCollections()
    {
        // Arrange
        var dto = CreateTestDto("news", externalUrl: "https://example.com/article");

        // Act
        var result = dto.GetHref();

        // Assert
        Assert.Equal("https://example.com/article", result);
    }

    [Fact]
    public void GetHref_ReturnsUrl_ForInternalCollections()
    {
        // Arrange
        var dto = CreateTestDto("videos", url: "/ai/videos/test-video");

        // Act
        var result = dto.GetHref();

        // Assert
        Assert.Equal("/ai/videos/test-video", result);
    }

    [Fact]
    public void GetHref_ReturnsEmptyString_WhenExternalUrlIsNull()
    {
        // Arrange
        var dto = CreateTestDto("news", externalUrl: null);

        // Act
        var result = dto.GetHref();

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void GetTarget_ReturnsBlank_ForExternalCollections()
    {
        // Arrange
        var dto = CreateTestDto("blogs");

        // Act
        var result = dto.GetTarget();

        // Assert
        Assert.Equal("_blank", result);
    }

    [Fact]
    public void GetTarget_ReturnsNull_ForInternalCollections()
    {
        // Arrange
        var dto = CreateTestDto("videos");

        // Act
        var result = dto.GetTarget();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetRel_ReturnsNoopenerNoreferrer_ForExternalCollections()
    {
        // Arrange
        var dto = CreateTestDto("community");

        // Act
        var result = dto.GetRel();

        // Assert
        Assert.Equal("noopener noreferrer", result);
    }

    [Fact]
    public void GetRel_ReturnsNull_ForInternalCollections()
    {
        // Arrange
        var dto = CreateTestDto("roundups");

        // Act
        var result = dto.GetRel();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetAriaLabel_IncludesOpensInNewTab_ForExternalCollections()
    {
        // Arrange
        var dto = CreateTestDto("news", title: "Breaking News");

        // Act
        var result = dto.GetAriaLabel();

        // Assert
        Assert.Equal("Breaking News - opens in new tab", result);
    }

    [Fact]
    public void GetAriaLabel_ReturnsTitle_ForInternalCollections()
    {
        // Arrange
        var dto = CreateTestDto("videos", title: "Tutorial Video");

        // Act
        var result = dto.GetAriaLabel();

        // Assert
        Assert.Equal("Tutorial Video", result);
    }

    [Fact]
    public void GetDataEnhanceNav_ReturnsTrue_ForInternalCollections()
    {
        // Arrange
        var dto = CreateTestDto("videos");

        // Act
        var result = dto.GetDataEnhanceNav();

        // Assert
        Assert.Equal("true", result);
    }

    [Fact]
    public void GetDataEnhanceNav_ReturnsNull_ForExternalCollections()
    {
        // Arrange
        var dto = CreateTestDto("news");

        // Act
        var result = dto.GetDataEnhanceNav();

        // Assert
        Assert.Null(result);
    }

    private static ContentItemDto CreateTestDto(
        string collectionName,
        string? externalUrl = "https://example.com",
        string? url = "/test/url",
        string? title = "Test Title")
    {
        return new ContentItemDto
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
