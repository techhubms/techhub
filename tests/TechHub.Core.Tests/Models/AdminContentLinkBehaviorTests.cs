using FluentAssertions;
using TechHub.Core.Models;
using TechHub.Core.Models.Admin;

namespace TechHub.Core.Tests.Models;

public class AdminContentLinkBehaviorTests
{
    [Fact]
    public void ContentItemListItem_GetHref_ForInternalCollection_UsesInternalPath()
    {
        // Arrange
        var item = CreateAdminContentItem(collectionName: "videos", externalUrl: "https://youtube.com/watch?v=demo");

        // Act
        var href = item.GetHref();

        // Assert
        href.Should().Be("/ai/videos/test-item");
    }

    [Fact]
    public void ContentItemListItem_GetHref_ForExternalCollection_UsesExternalUrl()
    {
        // Arrange
        var item = CreateAdminContentItem(collectionName: "blogs", externalUrl: "https://example.com/post");

        // Act
        var href = item.GetHref();

        // Assert
        href.Should().Be("https://example.com/post");
    }

    [Fact]
    public void VscodeUpdateListItem_GetHref_ForInternalCollection_UsesInternalPath()
    {
        // Arrange
        var item = new VscodeUpdateListItem
        {
            CollectionName = "videos",
            Slug = "vscode-update",
            Title = "VS Code Update",
            ExternalUrl = "https://youtube.com/watch?v=demo",
            PrimarySectionName = "github-copilot",
            DateEpoch = 0,
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        var href = item.GetHref();

        // Assert
        href.Should().Be("/github-copilot/videos/vscode-update");
    }

    [Fact]
    public void GhcFeatureContentLink_GetHref_ForExternalCollection_UsesExternalUrl()
    {
        // Arrange
        var link = new GhcFeatureContentLink
        {
            FeatureSlug = "feature",
            CollectionName = "news",
            ItemSlug = "news-item",
            ItemExternalUrl = "https://example.com/news-item"
        };

        // Act
        var href = link.GetHref();

        // Assert
        href.Should().Be("https://example.com/news-item");
    }

    [Fact]
    public void GhcFeatureContentLink_GetHref_ForInternalCollection_UsesInternalPath()
    {
        // Arrange
        var link = new GhcFeatureContentLink
        {
            FeatureSlug = "feature",
            CollectionName = "videos",
            ItemSlug = "video-item",
            ItemPrimarySectionName = "github-copilot"
        };

        // Act
        var href = link.GetHref();

        // Assert
        href.Should().Be("/github-copilot/videos/video-item");
    }

    private static ContentItemListItem CreateAdminContentItem(string collectionName, string externalUrl)
    {
        return new ContentItemListItem
        {
            Slug = "test-item",
            CollectionName = collectionName,
            Title = "Test Item",
            Author = "Author",
            FeedName = "Feed",
            ExternalUrl = externalUrl,
            PrimarySectionName = "ai",
            AllSections = "ai",
            DateEpoch = 0,
            CreatedAt = DateTimeOffset.UtcNow,
            HasProcessedUrl = true
        };
    }
}
