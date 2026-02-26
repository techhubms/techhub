using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Core.Models;
using TechHub.Web.Components;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for SubNav.razor component - horizontal navigation below page header.
/// </summary>
public class SubNavTests : BunitContext
{
    public SubNavTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void SubNav_WithNoParameters_RendersNothing()
    {
        // Act - SubNav with no Section and no Sections renders the empty state
        var cut = Render<SubNav>();

        // Assert - Should not render any markup (collapsed)
        cut.Markup.Trim().Should().BeEmpty(
            "empty SubNav should collapse and render no HTML when there is no content");
    }

    [Fact]
    public void SubNav_WithSection_RendersNavElement()
    {
        // Arrange
        var section = new Section(
            "ai",
            "Artificial Intelligence",
            "AI resources",
            "/ai",
            "AI",
            [
                new Collection("news", "News", "/ai/news", "Latest news", "News"),
                new Collection("blogs", "Blogs", "/ai/blogs", "Blog posts", "Blogs")
            ]
        );

        // Act
        var cut = Render<SubNav>(parameters => parameters
            .Add(p => p.Section, section));

        // Assert - Should render a nav element with sub-nav class
        var nav = cut.Find("nav.sub-nav");
        nav.Should().NotBeNull();
        nav.GetAttribute("aria-label").Should().Be("Artificial Intelligence navigation");
    }

    [Fact]
    public void SubNav_WithSections_RendersHomepageNav()
    {
        // Arrange
        var sections = new List<Section>
        {
            new("ai", "Artificial Intelligence", "AI", "/ai", "AI",
                [new Collection("news", "News", "/ai/news", "Latest news", "News")]),
            new("github-copilot", "GitHub Copilot", "Copilot", "/github-copilot", "GitHub Copilot",
                [new Collection("news", "News", "/github-copilot/news", "Latest news", "News")])
        };

        // Act
        var cut = Render<SubNav>(parameters => parameters
            .Add(p => p.Sections, sections));

        // Assert - Should render homepage subnav
        var nav = cut.Find("nav.sub-nav.sub-nav-homepage");
        nav.Should().NotBeNull();
        nav.GetAttribute("aria-label").Should().Be("Site navigation");
    }

    [Fact]
    public void SubNav_WithSections_DropdownToggle_ShowsHomeAsActiveLabel()
    {
        // Arrange
        var sections = new List<Section>
        {
            new("ai", "Artificial Intelligence", "AI", "/ai", "AI",
                [new Collection("news", "News", "/ai/news", "Latest news", "News")])
        };

        // Act
        var cut = Render<SubNav>(parameters => parameters
            .Add(p => p.Sections, sections));

        // Assert - Default active label should be "Home" (since bUnit's default URL is "/")
        var toggleButton = cut.Find(".sub-nav-dropdown-toggle");
        toggleButton.TextContent.Should().Contain("Home");
    }

    [Fact]
    public void SubNav_WithSections_DropdownMenu_ContainsHomeAndAboutUs()
    {
        // Arrange
        var sections = new List<Section>
        {
            new("ai", "Artificial Intelligence", "AI", "/ai", "AI",
                [new Collection("news", "News", "/ai/news", "Latest news", "News")]),
            new("github-copilot", "GitHub Copilot", "Copilot", "/github-copilot", "GitHub Copilot",
                [new Collection("news", "News", "/github-copilot/news", "Latest news", "News")])
        };

        // Act - Render and open the dropdown
        var cut = Render<SubNav>(parameters => parameters
            .Add(p => p.Sections, sections));
        cut.Find(".sub-nav-dropdown-toggle").Click();

        // Assert - Should contain Home, sections, and About Us
        var items = cut.FindAll(".sub-nav-dropdown-item");
        items.Should().HaveCount(4, "Home + 2 sections + About Us");
        items[0].TextContent.Trim().Should().Be("Home");
        items[1].TextContent.Trim().Should().Be("Artificial Intelligence");
        items[2].TextContent.Trim().Should().Be("GitHub Copilot");
        items[3].TextContent.Trim().Should().Be("About Us");
    }

    [Fact]
    public void SubNav_WithSections_OnHomepage_HomeIsActive()
    {
        // Arrange - bUnit default URL is "http://localhost/" which maps to Home
        var sections = new List<Section>
        {
            new("ai", "Artificial Intelligence", "AI", "/ai", "AI",
                [new Collection("news", "News", "/ai/news", "Latest news", "News")])
        };

        // Act - Render and open dropdown
        var cut = Render<SubNav>(parameters => parameters
            .Add(p => p.Sections, sections));
        cut.Find(".sub-nav-dropdown-toggle").Click();

        // Assert - Home link should have active class
        var homeLink = cut.Find(".sub-nav-dropdown-item[href='/']");
        homeLink.ClassList.Should().Contain("active");

        // About Us should NOT be active
        var aboutLink = cut.Find(".sub-nav-dropdown-item[href='/about']");
        aboutLink.ClassList.Should().NotContain("active");
    }

    [Fact]
    public void SubNav_WithEmptySections_RendersNothing()
    {
        // Arrange - empty sections list
        var sections = new List<Section>();

        // Act
        var cut = Render<SubNav>(parameters => parameters
            .Add(p => p.Sections, sections));

        // Assert - Should not render any markup when sections list is empty
        cut.Markup.Trim().Should().BeEmpty(
            "SubNav with empty Sections list should collapse and render no HTML");
    }
}
