using Bunit;
using FluentAssertions;
using TechHub.Core.DTOs;
using TechHub.Web.Components;

namespace TechHub.Web.Tests.Components;

public class SidebarCollectionNavTests : TestContext
{
    // No mocks needed - NavigationManager is provided by bUnit TestContext

    [Fact]
    public void Component_DisplaysCollections()
    {
        // Arrange - Component now receives section data directly, no API call
        var collections = new List<CollectionReferenceDto>
        {
            new CollectionReferenceDto { Title = "News", Name = "news", Url = "/ai/news", Description = "", IsCustom = false },
            new CollectionReferenceDto { Title = "Videos", Name = "videos", Url = "/ai/videos", Description = "", IsCustom = false },
            new CollectionReferenceDto { Title = "Blogs", Name = "blogs", Url = "/ai/blogs", Description = "", IsCustom = false }
        };

        var sectionDto = new SectionDto
        {
            Name = "ai",
            Title = "AI",
            Description = "AI Description",
            Url = "/ai",
            Category = "AI",
            BackgroundImage = "/images/ai-bg.jpg",
            Collections = collections
        };

        // Act - SSR navigation uses anchor tags instead of buttons and callbacks
        var cut = RenderComponent<SidebarCollectionNav>(parameters => parameters
            .Add(p => p.Section, sectionDto)
            .Add(p => p.SelectedCollection, "all"));

        // Assert - Component uses <a> tags for SSR navigation
        var links = cut.FindAll("a");
        Assert.Equal(4, links.Count); // "All" + 3 collections (RSS moved to SidebarRssLinks)

        Assert.Contains("All", links[0].TextContent);
        Assert.Contains("News", links[1].TextContent);
        Assert.Contains("Videos", links[2].TextContent);
        Assert.Contains("Blogs", links[3].TextContent);
    }

    [Fact]
    public void Component_HighlightsSelectedCollection()
    {
        // Arrange
        var collections = new List<CollectionReferenceDto>
        {
            new CollectionReferenceDto { Title = "News", Name = "news", Url = "/ai/news", Description = "", IsCustom = false }
        };

        var sectionDto = new SectionDto
        {
            Name = "ai",
            Title = "AI",
            Description = "AI Description",
            Url = "/ai",
            Category = "AI",
            BackgroundImage = "/images/ai-bg.jpg",
            Collections = collections
        };

        // Act - SSR uses anchor tags with active class for selected collection
        var cut = RenderComponent<SidebarCollectionNav>(parameters => parameters
            .Add(p => p.Section, sectionDto)
            .Add(p => p.SelectedCollection, "news"));

        // Assert - Check anchor tags for active class
        var links = cut.FindAll("a");
        var newsLink = links.First(a => a.TextContent.Contains("News"));
        Assert.Contains("active", newsLink.ClassName);
    }

    [Fact]
    public void Component_HasCorrectNavigationUrls()
    {
        // Arrange
        var collections = new List<CollectionReferenceDto>
        {
            new CollectionReferenceDto { Title = "News", Name = "news", Url = "/ai/news", Description = "", IsCustom = false }
        };

        var sectionDto = new SectionDto
        {
            Name = "ai",
            Title = "AI",
            Description = "AI Description",
            Url = "/ai",
            Category = "AI",
            BackgroundImage = "/images/ai-bg.jpg",
            Collections = collections
        };

        // Act - SSR navigation uses href attributes instead of callbacks
        var cut = RenderComponent<SidebarCollectionNav>(parameters => parameters
            .Add(p => p.Section, sectionDto)
            .Add(p => p.SelectedCollection, "all"));

        // Assert - Verify anchor tags have correct href attributes
        var links = cut.FindAll("a");
        var allLink = links.First(a => a.TextContent.Contains("All"));
        var newsLink = links.First(a => a.TextContent.Contains("News"));

        allLink.GetAttribute("href").Should().Be("/ai/all");
        newsLink.GetAttribute("href").Should().Be("/ai/news");
    }

    [Fact]
    public void Component_DisplaysCustomPages()
    {
        // Arrange - Test custom pages are displayed separately
        var collections = new List<CollectionReferenceDto>
        {
            new CollectionReferenceDto { Title = "News", Name = "news", Url = "/ai/news", Description = "", IsCustom = false },
            new CollectionReferenceDto { Title = "AI Handbook", Name = "handbook", Url = "/ai/handbook", Description = "", IsCustom = true }
        };

        var sectionDto = new SectionDto
        {
            Name = "ai",
            Title = "AI",
            Description = "AI Description",
            Url = "/ai",
            Category = "AI",
            BackgroundImage = "/images/ai-bg.jpg",
            Collections = collections
        };

        // Act - Custom pages appear in separate "Pages" section
        var cut = RenderComponent<SidebarCollectionNav>(parameters => parameters
            .Add(p => p.Section, sectionDto)
            .Add(p => p.SelectedCollection, "all"));

        // Assert - Should display both collections and custom pages sections
        var markup = cut.Markup;
        markup.Should().Contain("Collections");
        markup.Should().Contain("Pages");
        markup.Should().Contain("News");
        markup.Should().Contain("AI Handbook");
    }

    [Fact]
    public void Component_DoesNotDisplayRssFeed()
    {
        // Arrange - RSS feed moved to separate SidebarRssLinks component
        var collections = new List<CollectionReferenceDto>
        {
            new CollectionReferenceDto { Title = "News", Name = "news", Url = "/ai/news", Description = "", IsCustom = false }
        };

        var sectionDto = new SectionDto
        {
            Name = "ai",
            Title = "AI",
            Description = "AI Description",
            Url = "/ai",
            Category = "AI",
            BackgroundImage = "/images/ai-bg.jpg",
            Collections = collections
        };

        // Act - Component focuses on collection navigation only
        var cut = RenderComponent<SidebarCollectionNav>(parameters => parameters
            .Add(p => p.Section, sectionDto)
            .Add(p => p.SelectedCollection, "all"));

        // Assert - RSS feed is handled by SidebarRssLinks component
        var markup = cut.Markup;
        markup.Should().NotContain("Subscribe");
        markup.Should().NotContain("RSS Feed");
    }
}
