using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Core.Models;
using TechHub.TestUtilities.Builders;
using TechHub.Web.Components;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for ContentItemCard.razor component
/// </summary>
public class ContentItemCardTests : BunitContext
{
    public ContentItemCardTests()
    {
        var sectionCache = new SectionCache();
        sectionCache.Initialize(
        [
            new Section(
                "ai",
                "Artificial Intelligence",
                "AI content",
                "/ai",
                "AI",
                [
                    new Collection("news", "News", "/ai/news", "News", "News", false),
                    new Collection("blogs", "Blogs", "/ai/blogs", "Blogs", "Blog Posts", false),
                    new Collection("videos", "Videos", "/ai/videos", "Videos", "Videos", false),
                    new Collection("community", "Community", "/ai/community", "Community", "Community Posts", false)
                ]
            )
        ]);
        Services.AddSingleton(sectionCache);
    }

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
        // Arrange - use Brussels time to ensure "2 days ago" is correct in Brussels timezone
        var brusselsZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Brussels");
        var nowBrussels = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, brusselsZone);
        var twoDaysAgoBrussels = new DateTimeOffset(nowBrussels.AddDays(-2), brusselsZone.GetUtcOffset(nowBrussels.AddDays(-2)));
        var item = A.ContentItem
            .WithDateEpoch(twoDaysAgoBrussels.ToUnixTimeSeconds())
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
        // Arrange - use Brussels time to ensure "Today" is correct in Brussels timezone
        var brusselsZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Brussels");
        var nowBrussels = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, brusselsZone);
        var todayBrussels = new DateTimeOffset(nowBrussels.Date.AddHours(12), brusselsZone.GetUtcOffset(nowBrussels.Date.AddHours(12)));
        var item = A.ContentItem
            .WithDateEpoch(todayBrussels.ToUnixTimeSeconds())
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
    public void ContentItemCard_TimeElement_HasTitleWithFullDate()
    {
        // Arrange
        var item = CreateTestContentItem("Example Post", "2024-06-15");

        // Act
        var cut = Render<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item));

        // Assert - time element should have a title attribute for hover tooltip
        var time = cut.Find("time");
        time.HasAttribute("title").Should().BeTrue("time element should have a title for hover tooltip");
        time.GetAttribute("title").Should().Contain("June 15, 2024");
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

    [Fact]
    public void ContentItemCard_ActiveFilterTag_ShowsActiveClass()
    {
        // Arrange
        var item = A.ContentItem
            .WithTitle("Example Post")
            .WithDate(DateTime.Parse("2024-01-15"))
            .WithCollectionName("blogs")
            .WithPrimarySectionName("ai")
            .WithTags("ai", "copilot", "dotnet")
            .WithExternalUrl("https://example.com/post")
            .Build();

        // Act - Pass "ai" as an active filter tag
        var cut = Render<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item)
            .Add(p => p.ShowCollectionBadge, false)
            .Add(p => p.ActiveFilterTags, new[] { "ai" }));

        // Assert - The "ai" badge should have the active class
        var activeBadges = cut.FindAll(".badge-tag-active");
        activeBadges.Should().HaveCount(1, "only the 'ai' tag matches the active filter");
        activeBadges[0].TextContent.Should().Be("ai");

        // Other tags should NOT have the active class
        var inactiveBadges = cut.FindAll(".badge-tag:not(.badge-tag-active)");
        inactiveBadges.Should().HaveCount(2, "'copilot' and 'dotnet' are not active filters");
    }

    [Fact]
    public void ContentItemCard_NoActiveFilterTags_NoActiveClass()
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

        // Act - No active filter tags
        var cut = Render<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item)
            .Add(p => p.ShowCollectionBadge, false));

        // Assert - No badges should have the active class
        var activeBadges = cut.FindAll(".badge-tag-active");
        activeBadges.Should().BeEmpty("no active filter tags are set");
    }

    [Fact]
    public void ContentItemCard_ActiveFilterTag_IsCaseInsensitive()
    {
        // Arrange
        var item = A.ContentItem
            .WithTitle("Example Post")
            .WithDate(DateTime.Parse("2024-01-15"))
            .WithCollectionName("blogs")
            .WithPrimarySectionName("ai")
            .WithTags("AI", "GitHub Copilot")
            .WithExternalUrl("https://example.com/post")
            .Build();

        // Act - Active filters use lowercase (as stored in URL)
        var cut = Render<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item)
            .Add(p => p.ShowCollectionBadge, false)
            .Add(p => p.ActiveFilterTags, new[] { "ai", "github copilot" }));

        // Assert - Both should be active despite different casing
        var activeBadges = cut.FindAll(".badge-tag-active");
        activeBadges.Should().HaveCount(2, "both tags match active filters (case-insensitive)");
    }

    [Fact]
    public void ContentItemCard_ActiveTag_HasDeselectAriaLabel()
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
            .Add(p => p.ShowCollectionBadge, false)
            .Add(p => p.ActiveFilterTags, new[] { "ai" }));

        // Assert - Active tag should have "Remove filter" aria-label
        var activeTag = cut.Find(".badge-tag-active");
        activeTag.GetAttribute("aria-label").Should().Be("Remove filter: ai");

        // Inactive tag should keep "Filter by" aria-label
        var inactiveTag = cut.FindAll(".badge-tag:not(.badge-tag-active)").First();
        inactiveTag.GetAttribute("aria-label").Should().Be("Filter by copilot");
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

    [Fact]
    public void ContentItemCard_CollectionBadge_FallsBackToAll_WhenCollectionNotInSection()
    {
        // Arrange - Item has "roundups" collection which doesn't exist in the "ai" section
        var item = A.ContentItem
            .WithTitle("Weekly Roundup")
            .WithCollectionName("roundups")
            .WithPrimarySectionName("ai")
            .WithExternalUrl("https://example.com/roundup")
            .Build();

        // Act
        var cut = Render<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item)
            .Add(p => p.SectionName, "ai")
            .Add(p => p.ShowCollectionBadge, true));

        // Assert - Badge link should fall back to /all/roundups since "roundups" doesn't exist in "ai"
        var badge = cut.Find(".badge-purple");
        badge.GetAttribute("href").Should().Be("/all/roundups");
    }

    [Fact]
    public void ContentItemCard_CollectionBadge_UsesCurrentSection_WhenCollectionExists()
    {
        // Arrange - Item has "news" collection which exists in the "ai" section
        var item = A.ContentItem
            .WithTitle("AI News")
            .WithCollectionName("news")
            .WithPrimarySectionName("ai")
            .WithExternalUrl("https://example.com/news")
            .Build();

        // Act
        var cut = Render<ContentItemCard>(parameters => parameters
            .Add(p => p.Item, item)
            .Add(p => p.SectionName, "ai")
            .Add(p => p.ShowCollectionBadge, true));

        // Assert - Badge link should use the current section since "news" exists in "ai"
        var badge = cut.Find(".badge-purple");
        badge.GetAttribute("href").Should().Be("/ai/news");
    }
}

