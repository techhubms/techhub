using Bunit;
using FluentAssertions;
using TechHub.Core.Models;
using TechHub.TestUtilities.Builders;
using TechHub.Web.Components;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for ContentItemCard.razor component
/// </summary>
public class ContentItemCardTests : BunitContext
{
    [Fact]
    public void ContentItemCard_RendersTitle_Correctly()
    {
        // Arrange
        var item = CreateTestContentItem("Example Post", "2024-01-15");

        // Act
        var cut = Render<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item));

        // Assert
        var title = cut.Find(".card-title");
        title.TextContent.Should().Be("Example Post");
    }

    [Fact]
    public void ContentItemCard_RendersExcerpt_WhenProvided()
    {
        // Arrange
        var item = A.ContentItem
            .WithTitle("Example Post")
            .WithDate(DateTime.Parse("2024-01-15"))
            .WithCollectionName("blogs")
            .WithPrimarySectionName("ai")
            .WithExcerpt("<p>This is the excerpt of the post.</p>")
            .WithExternalUrl("https://example.com/post")
            .Build();

        // Act
        var cut = Render<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item));

        // Assert
        var excerpt = cut.Find(".card-description");
        excerpt.InnerHtml.Should().Contain("This is the excerpt of the post.");
    }

    [Fact]
    public void ContentItemCard_ShowsAuthor_WhenProvided()
    {
        // Arrange
        var item = A.ContentItem
            .WithTitle("Example Post")
            .WithAuthor("John Doe")
            .WithDate(DateTime.Parse("2024-01-15"))
            .WithCollectionName("blogs")
            .WithPrimarySectionName("ai")
            .WithExternalUrl("https://example.com/post")
            .Build();

        // Act
        var cut = Render<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item));

        // Assert
        var author = cut.Find(".content-author");
        author.TextContent.Should().Be(" by John Doe");
    }

    [Fact]
    public void ContentItemCard_DisplaysTags_UpToMaximum5()
    {
        // Arrange
        var item = A.ContentItem
            .WithTitle("Example Post")
            .WithDate(DateTime.Parse("2024-01-15"))
            .WithCollectionName("blogs")
            .WithPrimarySectionName("ai")
            .WithTags("ai", "copilot", "productivity", "dotnet")
            .WithExternalUrl("https://example.com/post")
            .Build();

        // Act
        var cut = Render<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item));

        // Assert
        var tags = cut.FindAll(".badge-tag");
        tags.Should().HaveCount(4);
        tags[0].TextContent.Should().Be("ai");
        tags[1].TextContent.Should().Be("copilot");
    }

    [Fact]
    public void ContentItemCard_ShowsMoreIndicator_WhenMoreThan5Tags()
    {
        // Arrange
        var item = A.ContentItem
            .WithTitle("Example Post")
            .WithDate(DateTime.Parse("2024-01-15"))
            .WithCollectionName("blogs")
            .WithPrimarySectionName("ai")
            .WithTags("ai", "copilot", "productivity", "dotnet", "azure", "github", "vscode")
            .WithExternalUrl("https://example.com/post")
            .Build();

        // Act
        var cut = Render<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item)
            .Add(p => p.ShowCollectionBadge, false));

        // Assert
        var tags = cut.FindAll(".badge-tag");
        tags.Should().HaveCount(5, "should only show first 5 tags");

        var moreIndicator = cut.Find(".badge-expandable");
        moreIndicator.TextContent.Trim().Should().Be("+2 more");
    }

    [Fact]
    public void ContentItemCard_ShowsCollectionBadge_WhenShowCollectionBadgeIsTrue()
    {
        // Arrange
        var item = A.ContentItem
            .WithCollectionName("news")
            .WithPrimarySectionName("ai")
            .WithExternalUrl("https://example.com/post")
            .Build();

        // Act
        var cut = Render<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item)
            .Add(p => p.ShowCollectionBadge, true));

        // Assert
        var badge = cut.Find(".badge-purple");
        badge.TextContent.Should().Be("News");
    }

    [Fact]
    public void ContentItemCard_HidesCollectionBadge_WhenShowCollectionBadgeIsFalse()
    {
        // Arrange
        var item = A.ContentItem
            .WithCollectionName("news")
            .WithPrimarySectionName("ai")
            .WithExternalUrl("https://example.com/post")
            .Build();

        // Act
        var cut = Render<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item)
            .Add(p => p.ShowCollectionBadge, false));

        // Assert
        var badges = cut.FindAll(".badge-purple");
        badges.Should().BeEmpty();
    }

    [Fact]
    public void ContentItemCard_ExternalLink_OpensInNewTab()
    {
        // Arrange
        var item = A.ContentItem
            .WithCollectionName("blogs")
            .WithPrimarySectionName("ai")
            .WithExternalUrl("https://techcommunity.microsoft.com/example")
            .Build();

        // Act
        var cut = Render<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item));

        // Assert
        var link = cut.Find(".card-link");
        link.GetAttribute("href").Should().Be("https://techcommunity.microsoft.com/example");
        link.GetAttribute("target").Should().Be("_blank");
        link.GetAttribute("rel").Should().Be("noopener noreferrer");
    }

    [Fact]
    public void ContentItemCard_InternalLink_UsesInternalUrl()
    {
        // Arrange
        var item = A.ContentItem
            .WithSlug("example-post")
            .WithCollectionName("videos")
            .WithPrimarySectionName("ai")
            .WithExternalUrl("https://www.youtube.com/watch?v=example")
            .Build();

        // Act
        var cut = Render<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item));

        // Assert
        var link = cut.Find(".card-link");
        link.GetAttribute("href").Should().Be("/ai/videos/example-post");
        link.HasAttribute("target").Should().BeFalse("internal links should not have target attribute");
    }

    [Fact]
    public void ContentItemCard_FormatsDate_AsRelativeTime()
    {
        // Arrange
        var twoDaysAgo = DateTimeOffset.UtcNow.AddDays(-2).ToUnixTimeSeconds();
        var item = A.ContentItem
            .WithDateEpoch(twoDaysAgo)
            .WithCollectionName("blogs")
            .WithPrimarySectionName("ai")
            .WithExternalUrl("https://example.com/post")
            .Build();

        // Act
        var cut = Render<ContentItemCard>(parameters => parameters
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
        var item = A.ContentItem
            .WithDateEpoch(today)
            .WithCollectionName("blogs")
            .WithPrimarySectionName("ai")
            .WithExternalUrl("https://example.com/post")
            .Build();

        // Act
        var cut = Render<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item));

        // Assert
        var time = cut.Find("time");
        time.TextContent.Should().Be("Today");
    }

    [Fact]
    public void ContentItemCard_CapitalizesCollectionName_Correctly()
    {
        // Arrange
        var item = A.ContentItem
            .WithCollectionName("community")
            .WithPrimarySectionName("ai")
            .WithExternalUrl("https://example.com/post")
            .Build();

        // Act
        var cut = Render<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item)
            .Add(p => p.ShowCollectionBadge, true));

        // Assert
        var badge = cut.Find(".badge-purple");
        badge.TextContent.Should().Be("Community");
    }

    [Fact]
    public void ContentItemCard_HasProperDateTimeAttribute()
    {
        // Arrange
        var item = CreateTestContentItem("Example Post", "2024-01-15");

        // Act
        var cut = Render<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item));

        // Assert
        var time = cut.Find("time");
        time.GetAttribute("datetime").Should().Be("2024-01-15");
    }

    [Fact]
    public void ContentItemCard_HasAccessibleAriaLabel_ForExternalLink()
    {
        // Arrange
        var item = A.ContentItem
            .WithTitle("Example Post")
            .WithCollectionName("blogs")
            .WithPrimarySectionName("ai")
            .WithExternalUrl("https://example.com")
            .Build();

        // Act
        var cut = Render<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item));

        // Assert
        var link = cut.Find(".card-link");
        link.GetAttribute("aria-label").Should().Be("Example Post - opens in new tab");
    }

    [Fact]
    public void ContentItemCard_CollectionBadge_AppearsBeforeTags()
    {
        // Arrange
        var item = A.ContentItem
            .WithTitle("Example Post")
            .WithCollectionName("news")
            .WithPrimarySectionName("ai")
            .WithTags("ai", "copilot")
            .WithExternalUrl("https://example.com/post")
            .Build();

        // Act
        var cut = Render<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item)
            .Add(p => p.ShowCollectionBadge, true));

        // Assert - Collection badge should be the first child in card-tags
        var cardTags = cut.Find(".card-tags");
        var allBadges = cardTags.Children;
        allBadges.Should().HaveCountGreaterThanOrEqualTo(3, "should have collection badge + 2 tag badges");
        allBadges[0].ClassList.Should().Contain("badge-purple", "collection badge should be first");
        allBadges[0].TextContent.Should().Be("News");
        allBadges[1].ClassList.Should().Contain("badge-tag", "tag badges should follow collection badge");
    }

    [Fact]
    public void ContentItemCard_TagBadges_UseBadgeTagClass()
    {
        // Arrange
        var item = A.ContentItem
            .WithTitle("Example Post")
            .WithDate(DateTime.Parse("2024-01-15"))
            .WithCollectionName("blogs")
            .WithPrimarySectionName("ai")
            .WithTags("ai", "copilot")
            .WithExternalUrl("https://example.com/post")
            .Build();

        // Act
        var cut = Render<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item)
            .Add(p => p.ShowCollectionBadge, false));

        // Assert - Tag badges should use the badge-tag class
        var tagBadges = cut.FindAll(".badge-tag");
        tagBadges.Should().HaveCount(2);
        tagBadges[0].TextContent.Should().Be("ai");
        tagBadges[1].TextContent.Should().Be("copilot");

        // Should NOT have any collection badge (ShowCollectionBadge is false)
        var collectionBadges = cut.FindAll(".badge-purple");
        collectionBadges.Should().BeEmpty("collection badge should be hidden when ShowCollectionBadge is false");
    }

    [Fact]
    public void ContentItemCard_HasAccessibleAriaLabel_ForInternalLink()
    {
        // Arrange
        var item = A.ContentItem
            .WithTitle("Example Post")
            .WithCollectionName("videos")
            .WithPrimarySectionName("ai")
            .WithExternalUrl("https://www.youtube.com/watch?v=example")
            .Build();

        // Act
        var cut = Render<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item));

        // Assert
        var link = cut.Find(".card-link");
        link.GetAttribute("aria-label").Should().Be("Example Post");
    }

    private static ContentItem CreateTestContentItem(string title, string date)
    {
        return A.ContentItem
            .WithTitle(title)
            .WithDate(DateTime.Parse(date))
            .WithCollectionName("blogs")
            .WithPrimarySectionName("ai")
            .WithExternalUrl("https://example.com/post")
            .Build();
    }
}

