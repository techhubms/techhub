using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Core.Models;
using TechHub.Web.Components;

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

    [Fact]
    public void SubNav_OnDetailPage_CollectionLinkDoesNotPreventDefault()
    {
        // Arrange - Navigate to a content detail page under the roundups collection
        var navMan = Services.GetRequiredService<NavigationManager>();
        navMan.NavigateTo("/all/roundups/weekly-ai-roundup-2026-03-09");

        var section = new Section(
            "all",
            "All Content",
            "All content",
            "/all",
            "All",
            [
                new Collection("roundups", "Roundups", "/all/roundups", "Weekly roundups", "Roundups"),
                new Collection("news", "News", "/all/news", "Latest news", "News")
            ]
        );

        // Act
        var cut = Render<SubNav>(parameters => parameters
            .Add(p => p.Section, section));

        // Assert - The Roundups link should have "active" class (visual highlight)
        var roundupsLink = cut.Find("a.btn-subnav[href='/all/roundups']");
        roundupsLink.ClassList.Should().Contain("active",
            "the parent collection link should be visually highlighted on a detail page");

        // Assert - Clicking Roundups should NOT call scrollTo (should navigate instead)
        // When preventDefault is true, OnSubNavClick calls ScrollToTopAndClearHash which invokes window.scrollTo
        roundupsLink.Click();
        JSInterop.Invocations.Should().NotContain(i => i.Identifier == "window.scrollTo",
            "clicking a collection link from a detail page should navigate, not scroll to top");
    }

    [Fact]
    public void SubNav_OnExactCollectionPage_CollectionLinkScrollsToTop()
    {
        // Arrange - Navigate to the exact collection page
        var navMan = Services.GetRequiredService<NavigationManager>();
        navMan.NavigateTo("/all/roundups");

        var section = new Section(
            "all",
            "All Content",
            "All content",
            "/all",
            "All",
            [
                new Collection("roundups", "Roundups", "/all/roundups", "Weekly roundups", "Roundups"),
                new Collection("news", "News", "/all/news", "Latest news", "News")
            ]
        );

        // Act
        var cut = Render<SubNav>(parameters => parameters
            .Add(p => p.Section, section));

        // Assert - Clicking Roundups on the exact same page should scroll to top
        var roundupsLink = cut.Find("a.btn-subnav[href='/all/roundups']");
        roundupsLink.Click();
        JSInterop.Invocations.Should().Contain(i => i.Identifier == "window.scrollTo",
            "clicking a collection link when already on that page should scroll to top");
    }

    [Fact]
    public void SubNav_OnExactPageWithHash_ClickClearsHashAndScrollsToTop()
    {
        // Arrange - Navigate to a page with a hash fragment
        var navMan = Services.GetRequiredService<NavigationManager>();
        navMan.NavigateTo("/all/roundups#some-section");

        var section = new Section(
            "all",
            "All Content",
            "All content",
            "/all",
            "All",
            [
                new Collection("roundups", "Roundups", "/all/roundups", "Weekly roundups", "Roundups"),
                new Collection("news", "News", "/all/news", "Latest news", "News")
            ]
        );

        // Act
        var cut = Render<SubNav>(parameters => parameters
            .Add(p => p.Section, section));

        var roundupsLink = cut.Find("a.btn-subnav[href='/all/roundups']");
        roundupsLink.Click();

        // Assert - Should scroll to top
        JSInterop.Invocations.Should().Contain(i => i.Identifier == "window.scrollTo",
            "clicking the active link should scroll to top");

        // Assert - Hash should be removed (NavigateTo called with path without hash)
        var lastNavUri = navMan.Uri;
        lastNavUri.Should().NotContain("#",
            "the hash should be cleared from the URL after clicking the active subnav link");
    }
}
