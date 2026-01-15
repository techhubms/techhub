using Bunit;
using FluentAssertions;
using TechHub.Core.DTOs;
using TechHub.Web.Components;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for SectionCard.razor component
/// </summary>
public class SectionCardTests : TestContext
{
    [Fact]
    public void SectionCard_RendersTitle_Correctly()
    {
        // Arrange
        var section = CreateTestSection("AI", "Artificial Intelligence", "AI description");

        // Act
        var cut = RenderComponent<SectionCard>(parameters => parameters
            .Add(p => p.Section, section));

        // Assert
        var h2 = cut.Find("h2");
        h2.TextContent.Should().Be("Artificial Intelligence");
    }

    [Fact]
    public void SectionCard_RendersDescription_Correctly()
    {
        // Arrange
        var section = CreateTestSection("ai", "AI", "Your gateway to the AI revolution");

        // Act
        var cut = RenderComponent<SectionCard>(parameters => parameters
            .Add(p => p.Section, section));

        // Assert
        var description = cut.Find(".section-description");
        description.TextContent.Should().Be("Your gateway to the AI revolution");
    }

    [Fact]
    public void SectionCard_LinksToSectionUrl_Correctly()
    {
        // Arrange
        var section = CreateTestSection("github-copilot", "GitHub Copilot", "Master GitHub Copilot");

        // Act
        var cut = RenderComponent<SectionCard>(parameters => parameters
            .Add(p => p.Section, section));

        // Assert
        var link = cut.Find(".section-card");
        link.GetAttribute("href").Should().Be("/github-copilot");
    }

    [Fact]
    public void SectionCard_HasBackgroundImage_Correctly()
    {
        // Arrange
        var section = CreateTestSection("ai", "AI", "AI description");

        // Act
        var cut = RenderComponent<SectionCard>(parameters => parameters
            .Add(p => p.Section, section));

        // Assert
        var header = cut.Find(".section-card-header");
        var style = header.GetAttribute("style");
        style.Should().Contain("background-image: url('/images/section-backgrounds/ai.jpg')");
    }

    [Fact]
    public void SectionCard_DisplaysCollectionBadges_WhenCollectionsExist()
    {
        // Arrange
        var collections = new List<CollectionReferenceDto>
        {
            new CollectionReferenceDto
            {
                Name = "news",
                Title = "News",
                Url = "/ai/news",
                Description = "News articles",
                IsCustom = false,
                DisplayName = "News"
            },
            new CollectionReferenceDto
            {
                Name = "blogs",
                Title = "Blogs",
                Url = "/ai/blogs",
                Description = "Blog posts",
                IsCustom = false,
                DisplayName = "Blogs"
            }
        };
        var section = new SectionDto
        {
            Name = "ai",
            Title = "AI",
            Description = "AI description",
            Url = "/ai",
            BackgroundImage = "/images/section-backgrounds/ai.jpg",
            Collections = collections
        };

        // Act
        var cut = RenderComponent<SectionCard>(parameters => parameters
            .Add(p => p.Section, section));

        // Assert
        var badges = cut.FindAll(".collection-badge");
        badges.Should().HaveCount(2);
        badges[0].TextContent.Should().Be("News");
        badges[1].TextContent.Should().Be("Blogs");
    }

    [Fact]
    public void SectionCard_DisplaysFirst4Collections_WhenMoreThan4Exist()
    {
        // Arrange
        var collections = new List<CollectionReferenceDto>
        {
            new CollectionReferenceDto { Name = "news", Title = "News", Url = "/github-copilot/news", Description = "News", IsCustom = false, DisplayName = "News" },
            new CollectionReferenceDto { Name = "blogs", Title = "Blogs", Url = "/github-copilot/blogs", Description = "Blogs", IsCustom = false, DisplayName = "Blogs" },
            new CollectionReferenceDto { Name = "videos", Title = "Videos", Url = "/github-copilot/videos", Description = "Videos", IsCustom = false, DisplayName = "Videos" },
            new CollectionReferenceDto { Name = "community", Title = "Community", Url = "/github-copilot/community", Description = "Community", IsCustom = false, DisplayName = "Community Posts" },
            new CollectionReferenceDto { Name = "features", Title = "Features", Url = "/github-copilot/features", Description = "Features", IsCustom = true, DisplayName = "Features" },
            new CollectionReferenceDto { Name = "handbook", Title = "Handbook", Url = "/github-copilot/handbook", Description = "Handbook", IsCustom = true, DisplayName = "Handbook" }
        };
        var section = new SectionDto
        {
            Name = "github-copilot",
            Title = "GitHub Copilot",
            Description = "Description",
            Url = "/github-copilot",
            BackgroundImage = "/images/section-backgrounds/github-copilot.jpg",
            Collections = collections
        };

        // Act
        var cut = RenderComponent<SectionCard>(parameters => parameters
            .Add(p => p.Section, section));

        // Assert
        var badges = cut.FindAll(".collection-badge, .custom-page-badge");
        badges.Should().HaveCount(4, "should only show first 4 collections");

        var moreIndicator = cut.Find(".collection-badge-more");
        moreIndicator.TextContent.Should().Be("+2 more");
    }

    [Fact]
    public void SectionCard_DistinguishesCustomPages_WithSpecialBadge()
    {
        // Arrange
        var collections = new List<CollectionReferenceDto>
        {
            new CollectionReferenceDto
            {
                Name = "news",
                Title = "News",
                Url = "/github-copilot/news",
                Description = "News",
                IsCustom = false,
                DisplayName = "News"
            },
            new CollectionReferenceDto
            {
                Name = "features",
                Title = "Features",
                Url = "/github-copilot/features",
                Description = "Features",
                IsCustom = true,
                DisplayName = "Features"
            }
        };
        var section = new SectionDto
        {
            Name = "github-copilot",
            Title = "GitHub Copilot",
            Description = "Description",
            Url = "/github-copilot",
            BackgroundImage = "/images/section-backgrounds/github-copilot.jpg",
            Collections = collections
        };

        // Act
        var cut = RenderComponent<SectionCard>(parameters => parameters
            .Add(p => p.Section, section));

        // Assert
        var allBadges = cut.FindAll(".collection-badge");
        var customBadges = cut.FindAll(".custom-page-badge");

        allBadges.Should().HaveCount(2, "there are 2 total collections (1 regular + 1 custom)");
        customBadges.Should().HaveCount(1, "there is 1 custom page");
        customBadges[0].TextContent.Should().Be("Features");
    }

    [Fact]
    public void SectionCard_HasProperAriaLabels_ForAccessibility()
    {
        // Arrange
        var section = CreateTestSection("ai", "AI", "AI description");

        // Act
        var cut = RenderComponent<SectionCard>(parameters => parameters
            .Add(p => p.Section, section));

        // Assert
        var link = cut.Find(".section-card");
        link.GetAttribute("aria-label").Should().Be("View AI section");

        var header = cut.Find(".section-card-header");
        header.GetAttribute("aria-label").Should().Be("AI section background");
    }

    [Fact]
    public void SectionCard_CollectionBadges_HaveAccessibleLabels()
    {
        // Arrange
        var collections = new List<CollectionReferenceDto>
        {
            new CollectionReferenceDto
            {
                Name = "news",
                Title = "News",
                Url = "/ai/news",
                Description = "News articles",
                IsCustom = false,
                DisplayName = "News"
            }
        };
        var section = new SectionDto
        {
            Name = "ai",
            Title = "AI",
            Description = "AI description",
            Url = "/ai",
            BackgroundImage = "/images/section-backgrounds/ai.jpg",
            Collections = collections
        };

        // Act
        var cut = RenderComponent<SectionCard>(parameters => parameters
            .Add(p => p.Section, section));

        // Assert
        var collectionNav = cut.Find(".section-collections");
        collectionNav.GetAttribute("aria-label").Should().Be("AI collections");

        var badge = cut.Find(".collection-badge");
        badge.GetAttribute("aria-label").Should().Be("View News collection");
    }

    [Fact]
    public void SectionCard_DoesNotRenderCollections_WhenNoneExist()
    {
        // Arrange
        var section = CreateTestSection("security", "Security", "Security content");

        // Act
        var cut = RenderComponent<SectionCard>(parameters => parameters
            .Add(p => p.Section, section));

        // Assert
        var collectionNav = cut.FindAll(".section-collections");
        collectionNav.Should().BeEmpty();
    }

    private static SectionDto CreateTestSection(string name, string title, string description)
    {
        return new SectionDto
        {
            Name = name,
            Title = title,
            Description = description,
            Url = $"/{name}",
            BackgroundImage = $"/images/section-backgrounds/{name}.jpg",
            Collections = []
        };
    }
}
