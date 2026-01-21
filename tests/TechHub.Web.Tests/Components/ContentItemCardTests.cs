using Bunit;
using FluentAssertions;
using TechHub.Core.DTOs;
using TechHub.Web.Components;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for ContentItemCard.razor component
/// </summary>
public class ContentItemCardTests : TestContext
{
    [Fact]
    public void ContentItemCard_RendersTitle_Correctly()
    {
        // Arrange
        var item = CreateTestContentItem("Example Post", "2024-01-15");

        // Act
        var cut = RenderComponent<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item));

        // Assert
        var title = cut.Find(".content-title");
        title.TextContent.Should().Be("Example Post");
    }

    [Fact]
    public void ContentItemCard_RendersExcerpt_WhenProvided()
    {
        // Arrange
        var item = new ContentItemDto
        {
            Slug = "example-post",
            Title = "Example Post",
            Author = null,
            DateEpoch = DateTimeOffset.Parse("2024-01-15").ToUnixTimeSeconds(),
            DateIso = "2024-01-15",
            CollectionName = "blogs",
            SectionNames = ["ai"],
            PrimarySectionName = "ai",
            Tags = [],
            Excerpt = "<p>This is the excerpt of the post.</p>",
            ExternalUrl = "https://example.com/post",
            Url = "/ai/blogs/example-post"
        };

        // Act
        var cut = RenderComponent<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item));

        // Assert
        var excerpt = cut.Find(".content-excerpt");
        excerpt.InnerHtml.Should().Contain("This is the excerpt of the post.");
    }

    [Fact]
    public void ContentItemCard_ShowsAuthor_WhenProvided()
    {
        // Arrange
        var item = new ContentItemDto
        {
            Slug = "example-post",
            Title = "Example Post",

            Author = "John Doe",
            DateEpoch = DateTimeOffset.Parse("2024-01-15").ToUnixTimeSeconds(),
            DateIso = "2024-01-15",
            CollectionName = "blogs",
            SectionNames = ["ai"],
            PrimarySectionName = "ai",
            Tags = [],
            Excerpt = "",
            ExternalUrl = "https://example.com/post",
            Url = "/ai/blogs/example-post"
        };

        // Act
        var cut = RenderComponent<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item));

        // Assert
        var author = cut.Find(".content-author");
        author.TextContent.Should().Be("by John Doe");
    }

    [Fact]
    public void ContentItemCard_DoesNotShowAuthor_WhenNotProvided()
    {
        // Arrange
        var item = CreateTestContentItem("Example Post", "2024-01-15");

        // Act
        var cut = RenderComponent<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item));

        // Assert
        var authors = cut.FindAll(".content-author");
        authors.Should().BeEmpty();
    }

    [Fact]
    public void ContentItemCard_DisplaysTags_UpToMaximum5()
    {
        // Arrange
        var item = new ContentItemDto
        {
            Slug = "example-post",
            Title = "Example Post",

            Author = null,
            DateEpoch = DateTimeOffset.Parse("2024-01-15").ToUnixTimeSeconds(),
            DateIso = "2024-01-15",
            CollectionName = "blogs",
            SectionNames = ["ai"],
            PrimarySectionName = "ai",
            Tags = ["ai", "copilot", "productivity", "dotnet"],
            Excerpt = "",
            ExternalUrl = "https://example.com/post",
            Url = "/ai/blogs/example-post"
        };

        // Act
        var cut = RenderComponent<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item));

        // Assert
        var tags = cut.FindAll(".content-item-card-tag");
        tags.Should().HaveCount(4);
        tags[0].TextContent.Should().Be("ai");
        tags[1].TextContent.Should().Be("copilot");
    }

    [Fact]
    public void ContentItemCard_ShowsMoreIndicator_WhenMoreThan5Tags()
    {
        // Arrange
        var item = new ContentItemDto
        {
            Slug = "example-post",
            Title = "Example Post",

            Author = null,
            DateEpoch = DateTimeOffset.Parse("2024-01-15").ToUnixTimeSeconds(),
            DateIso = "2024-01-15",
            CollectionName = "blogs",
            SectionNames = ["ai"],
            PrimarySectionName = "ai",
            Tags = ["ai", "copilot", "productivity", "dotnet", "azure", "github", "vscode"],
            Excerpt = "",
            ExternalUrl = "https://example.com/post",
            Url = "/ai/blogs/example-post"
        };

        // Act
        var cut = RenderComponent<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item));

        // Assert
        var tags = cut.FindAll(".content-item-card-tag");
        tags.Should().HaveCount(5, "should only show first 5 tags");

        var moreIndicator = cut.Find(".content-item-card-tag-more");
        moreIndicator.TextContent.Should().Be("+2 more");
    }

    [Fact]
    public void ContentItemCard_ShowsCollectionBadge_WhenShowCollectionBadgeIsTrue()
    {
        // Arrange
        var item = new ContentItemDto
        {
            Slug = "example-post",
            Title = "Example Post",

            Author = null,
            DateEpoch = DateTimeOffset.Parse("2024-01-15").ToUnixTimeSeconds(),
            DateIso = "2024-01-15",
            CollectionName = "news",
            SectionNames = ["ai"],
            PrimarySectionName = "ai",
            Tags = [],
            Excerpt = "",
            ExternalUrl = "https://example.com/post",
            Url = "/ai/blogs/example-post"
        };

        // Act
        var cut = RenderComponent<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item)
            .Add(p => p.ShowCollectionBadge, true));

        // Assert
        var badge = cut.Find(".collection-badge-white");
        badge.TextContent.Should().Be("News");
    }

    [Fact]
    public void ContentItemCard_HidesCollectionBadge_WhenShowCollectionBadgeIsFalse()
    {
        // Arrange
        var item = new ContentItemDto
        {
            Slug = "example-post",
            Title = "Example Post",

            Author = null,
            DateEpoch = DateTimeOffset.Parse("2024-01-15").ToUnixTimeSeconds(),
            DateIso = "2024-01-15",
            CollectionName = "news",
            SectionNames = ["ai"],
            PrimarySectionName = "ai",
            Tags = [],
            Excerpt = "",
            ExternalUrl = "https://example.com/post",
            Url = "/ai/blogs/example-post"
        };

        // Act
        var cut = RenderComponent<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item)
            .Add(p => p.ShowCollectionBadge, false));

        // Assert
        var badges = cut.FindAll(".collection-badge-white");
        badges.Should().BeEmpty();
    }

    [Fact]
    public void ContentItemCard_ExternalLink_OpensInNewTab()
    {
        // Arrange
        var item = new ContentItemDto
        {
            Slug = "example-post",
            Title = "Example Post",

            Author = null,
            DateEpoch = DateTimeOffset.Parse("2024-01-15").ToUnixTimeSeconds(),
            DateIso = "2024-01-15",
            CollectionName = "blogs",
            SectionNames = ["ai"],
            PrimarySectionName = "ai",
            Tags = [],
            Excerpt = "",
            ExternalUrl = "https://techcommunity.microsoft.com/example",
            Url = "/ai/blogs/example-post"
        };

        // Act
        var cut = RenderComponent<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item));

        // Assert
        var link = cut.Find(".content-item-card");
        link.GetAttribute("href").Should().Be("https://techcommunity.microsoft.com/example");
        link.GetAttribute("target").Should().Be("_blank");
        link.GetAttribute("rel").Should().Be("noopener noreferrer");
    }

    [Fact]
    public void ContentItemCard_InternalLink_UsesInternalUrl()
    {
        // Arrange
        var item = new ContentItemDto
        {
            Slug = "example-post",
            Title = "Example Post",

            Author = null,
            DateEpoch = DateTimeOffset.Parse("2024-01-15").ToUnixTimeSeconds(),
            DateIso = "2024-01-15",
            CollectionName = "blogs",
            SectionNames = ["ai"],
            PrimarySectionName = "ai",
            Tags = [],
            Excerpt = "",
            ExternalUrl = "https://example.com/post",
            Url = "/ai/videos/example-post"
        };

        // Act
        var cut = RenderComponent<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item));

        // Assert
        var link = cut.Find(".content-item-card");
        link.GetAttribute("href").Should().Be("/ai/videos/example-post");
        link.HasAttribute("target").Should().BeFalse("internal links should not have target attribute");
    }

    [Fact]
    public void ContentItemCard_FormatsDate_AsRelativeTime()
    {
        // Arrange
        var twoDaysAgo = DateTimeOffset.UtcNow.AddDays(-2).ToUnixTimeSeconds();
        var item = new ContentItemDto
        {
            Slug = "example-post",
            Title = "Example Post",

            Author = null,
            DateEpoch = twoDaysAgo,
            DateIso = "2024-01-15",
            CollectionName = "blogs",
            SectionNames = ["ai"],
            PrimarySectionName = "ai",
            Tags = [],
            Excerpt = "",
            ExternalUrl = "https://example.com/post",
            Url = "/ai/blogs/example-post"
        };

        // Act
        var cut = RenderComponent<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item));

        // Assert
        var time = cut.Find("time");
        time.TextContent.Should().Be("2 days ago");
    }

    [Fact]
    public void ContentItemCard_FormatsToday_AsToday()
    {
        // Arrange
        var today = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var item = new ContentItemDto
        {
            Slug = "example-post",
            Title = "Example Post",

            Author = null,
            DateEpoch = today,
            DateIso = "2024-01-15",
            CollectionName = "blogs",
            SectionNames = ["ai"],
            PrimarySectionName = "ai",
            Tags = [],
            Excerpt = "",
            ExternalUrl = "https://example.com/post",
            Url = "/ai/blogs/example-post"
        };

        // Act
        var cut = RenderComponent<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item));

        // Assert
        var time = cut.Find("time");
        time.TextContent.Should().Be("Today");
    }

    [Fact]
    public void ContentItemCard_CapitalizesCollectionName_Correctly()
    {
        // Arrange
        var item = new ContentItemDto
        {
            Slug = "example-post",
            Title = "Example Post",

            Author = null,
            DateEpoch = DateTimeOffset.Parse("2024-01-15").ToUnixTimeSeconds(),
            DateIso = "2024-01-15",
            CollectionName = "community",
            SectionNames = ["ai"],
            PrimarySectionName = "ai",
            Tags = [],
            Excerpt = "",
            ExternalUrl = "https://example.com/post",
            Url = "/ai/blogs/example-post"
        };

        // Act
        var cut = RenderComponent<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item)
            .Add(p => p.ShowCollectionBadge, true));

        // Assert
        var badge = cut.Find(".collection-badge-white");
        badge.TextContent.Should().Be("Community");
    }

    [Fact]
    public void ContentItemCard_HasProperDateTimeAttribute()
    {
        // Arrange
        var item = CreateTestContentItem("Example Post", "2024-01-15");

        // Act
        var cut = RenderComponent<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item));

        // Assert
        var time = cut.Find("time");
        time.GetAttribute("datetime").Should().Be("2024-01-15");
    }

    [Fact]
    public void ContentItemCard_HasAccessibleAriaLabel_ForExternalLink()
    {
        // Arrange
        var item = new ContentItemDto
        {
            Slug = "example-post",
            Title = "Example Post",

            Author = null,
            DateEpoch = DateTimeOffset.Parse("2024-01-15").ToUnixTimeSeconds(),
            DateIso = "2024-01-15",
            CollectionName = "blogs",
            SectionNames = ["ai"],
            PrimarySectionName = "ai",
            Tags = [],
            Excerpt = "",
            ExternalUrl = "https://example.com",
            Url = "/ai/blogs/example-post"
        };

        // Act
        var cut = RenderComponent<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item));

        // Assert
        var link = cut.Find(".content-item-card");
        link.GetAttribute("aria-label").Should().Be("Example Post - opens in new tab");
    }

    [Fact]
    public void ContentItemCard_HasAccessibleAriaLabel_ForInternalLink()
    {
        // Arrange
        var item = new ContentItemDto
        {
            Slug = "example-post",
            Title = "Example Post",

            Author = null,
            DateEpoch = DateTimeOffset.Parse("2024-01-15").ToUnixTimeSeconds(),
            DateIso = "2024-01-15",
            CollectionName = "blogs",
            SectionNames = ["ai"],
            PrimarySectionName = "ai",
            Tags = [],
            Excerpt = "",
            ExternalUrl = "https://example.com/post",
            Url = "/ai/blogs/example-post"
        };

        // Act
        var cut = RenderComponent<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item));

        // Assert
        var link = cut.Find(".content-item-card");
        link.GetAttribute("aria-label").Should().Be("Example Post");
    }

    private static ContentItemDto CreateTestContentItem(string title, string dateIso)
    {
        return new ContentItemDto
        {
            Slug = title.ToLowerInvariant().Replace(" ", "-"),
            Title = title,

            Author = null,
            DateEpoch = DateTimeOffset.Parse(dateIso).ToUnixTimeSeconds(),
            DateIso = dateIso,
            CollectionName = "blogs",
            SectionNames = ["ai"],
            PrimarySectionName = "ai",
            Tags = [],
            Excerpt = "",
            ExternalUrl = "https://example.com/post",
            Url = "/ai/blogs/example-post"
        };
    }
}
