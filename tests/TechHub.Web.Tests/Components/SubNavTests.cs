using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
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
        // HomepageStatsState is injected by SubNav; provide an empty instance by default
        Services.AddSingleton(new HomepageStatsState());
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

        // Assert - Should render homepage subnav (always sub-nav-homepage-visible now)
        var nav = cut.Find("nav.sub-nav.sub-nav-homepage-visible");
        nav.Should().NotBeNull();
        nav.GetAttribute("aria-label").Should().Be("Site navigation");
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
        JSInterop.Invocations.Should().Contain(i => i.Identifier == "TechHub.scrollToTopAndClearHash",
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

        // Assert - Should scroll to top and clear hash via JS
        // Hash clearing is handled by the JS function TechHub.scrollToTopAndClearHash,
        // not by Blazor's NavigationManager, so we verify the JS call was made.
        JSInterop.Invocations.Should().Contain(i => i.Identifier == "TechHub.scrollToTopAndClearHash",
            "clicking the active link should scroll to top and clear the hash");
    }

    [Fact]
    public void SubNav_WithHomepageStats_ShowsStatChips()
    {
        // Arrange
        var statsState = Services.GetRequiredService<HomepageStatsState>();
        statsState.Update(recentCount: 12, totalCount: 1337, weekAgoDate: "2025-01-01", sectionsCount: 5);

        var sections = new List<Section>
        {
            new("ai", "Artificial Intelligence", "AI", "/ai", "AI",
                [new Collection("news", "News", "/ai/news", "Latest news", "News")])
        };

        // Act
        var cut = Render<SubNav>(parameters => parameters
            .Add(p => p.Sections, sections));

        // Assert - stat action buttons should appear in the subnav
        cut.Find(".btn-subnav-stat-new").Should().NotBeNull(
            "new-this-week stat button should be rendered when recentCount > 0");
        cut.Find(".btn-subnav-stat-total").Should().NotBeNull(
            "total count stat button should be rendered when stats are available");
        cut.Find(".btn-subnav-stat-total").TextContent.Should().Contain("1,337 items total",
            "total count should be formatted with thousands separator");
        cut.Markup.Should().NotContain("5 sections",
            "sections count is no longer shown in the subnav");
        cut.Find(".btn-subnav-stat-new").TextContent.Should().Contain("12 new",
            "recent count should be shown when > 0");
        cut.Find(".btn-subnav-stat-new").TextContent.Should().Contain("this week",
            "recent label should include 'this week' text");
    }

    [Fact]
    public void SubNav_WithHomepageStats_WhenNoRecentItems_HidesNewBadge()
    {
        // Arrange
        var statsState = Services.GetRequiredService<HomepageStatsState>();
        statsState.Update(recentCount: 0, totalCount: 500, weekAgoDate: "2025-01-01", sectionsCount: 3);

        var sections = new List<Section>
        {
            new("ai", "Artificial Intelligence", "AI", "/ai", "AI",
                [new Collection("news", "News", "/ai/news", "Latest news", "News")])
        };

        // Act
        var cut = Render<SubNav>(parameters => parameters
            .Add(p => p.Sections, sections));

        // Assert - "new this week" chip should NOT appear when recentCount is 0
        cut.Markup.Should().NotContain("new this week",
            "new-this-week chip should be hidden when recentCount is 0");
        cut.Markup.Should().Contain("500 items total",
            "total count should still appear");
    }

    [Fact]
    public void SubNav_WithHomepageStats_WhenNoDataYet_HidesStatChips()
    {
        // Arrange - HomepageStatsState with no data (TotalCount is null)
        // (Services already has an empty HomepageStatsState from constructor)

        var sections = new List<Section>
        {
            new("ai", "Artificial Intelligence", "AI", "/ai", "AI",
                [new Collection("news", "News", "/ai/news", "Latest news", "News")])
        };

        // Act
        var cut = Render<SubNav>(parameters => parameters
            .Add(p => p.Sections, sections));

        // Assert - stat chips should not appear when TotalCount is null
        cut.FindAll(".btn-subnav-stat-new").Should().BeEmpty(
            "new-this-week stat button should not render before stats are loaded");
        cut.FindAll(".btn-subnav-stat-total").Should().BeEmpty(
            "total count stat button should not render before stats are loaded");
    }

    [Fact]
    public void SubNav_HomepageWithNewsletter_ShowsNewsletterButton()
    {
        // Arrange
        var sections = new List<Section>
        {
            new("ai", "Artificial Intelligence", "AI", "/ai", "AI",
                [new Collection("news", "News", "/ai/news", "Latest news", "News")])
        };

        // Act — RSS is no longer shown in subnav (it moved to SectionBanner); only Newsletter
        var cut = Render<SubNav>(parameters => parameters
            .Add(p => p.Sections, sections)
            .Add(p => p.RssFeedUrl, "/rss/feed.xml")
            .Add(p => p.ShowNewsletter, true));

        // Assert - only Newsletter button is shown in the subnav; RSS is in the banner
        var newsletterLink = cut.Find("a.btn-subnav[href='/newsletter/subscribe']");
        newsletterLink.Should().NotBeNull("Newsletter button should be shown in the homepage subnav");
        newsletterLink.ClassList.Should().NotContain("btn-subnav-newsletter",
            "Newsletter button should not use the purple-accented btn-subnav-newsletter style");
        cut.Markup.Should().NotContain("/rss/feed.xml",
            "RSS link should not appear in the subnav (it moved to the section banner)");
    }

    [Fact]
    public void SubNav_WithHideStats_DoesNotShowStatsBox()
    {
        // Arrange - stats are loaded but HideStats=true
        var statsState = Services.GetRequiredService<HomepageStatsState>();
        statsState.Update(recentCount: 5, totalCount: 999, weekAgoDate: "2025-01-01", sectionsCount: 4);

        var sections = new List<Section>
        {
            new("ai", "Artificial Intelligence", "AI", "/ai", "AI",
                [new Collection("news", "News", "/ai/news", "Latest news", "News")])
        };

        // Act
        var cut = Render<SubNav>(parameters => parameters
            .Add(p => p.Sections, sections)
            .Add(p => p.HideStats, true));

        // Assert - stats box should not render when HideStats is true
        cut.FindAll(".btn-subnav-stat-new").Should().BeEmpty(
            "new-this-week stat button should be hidden when HideStats is true");
        cut.FindAll(".btn-subnav-stat-total").Should().BeEmpty(
            "total count stat button should not render when HideStats is true");
    }

    [Fact]
    public void SubNav_WithShowDiscord_ShowsDiscordButton()
    {
        // Arrange
        var sections = new List<Section>
        {
            new("ai", "Artificial Intelligence", "AI", "/ai", "AI",
                [new Collection("news", "News", "/ai/news", "Latest news", "News")])
        };

        // Act
        var cut = Render<SubNav>(parameters => parameters
            .Add(p => p.Sections, sections)
            .Add(p => p.ShowDiscord, true));

        // Assert - Discord button should be rendered in the subnav
        var discordLink = cut.Find("a.btn-subnav-discord");
        discordLink.Should().NotBeNull("Discord button should render when ShowDiscord is true");
        discordLink.GetAttribute("href").Should().Be("https://discord.gg/cURHV9TvFS",
            "Discord link should point to the correct server URL");
        discordLink.GetAttribute("target").Should().Be("_blank",
            "Discord link should open in a new tab");
    }

    [Fact]
    public void SubNav_WithShowHomeButton_ShowsHomeButton()
    {
        // Arrange
        var sections = new List<Section>
        {
            new("ai", "Artificial Intelligence", "AI", "/ai", "AI",
                [new Collection("news", "News", "/ai/news", "Latest news", "News")])
        };

        // Act
        var cut = Render<SubNav>(parameters => parameters
            .Add(p => p.Sections, sections)
            .Add(p => p.ShowHomeButton, true));

        // Assert - Home button should be rendered
        var homeLink = cut.Find("a.btn-subnav[href='/']");
        homeLink.Should().NotBeNull("Home button should be rendered when ShowHomeButton is true");
        homeLink.TextContent.Trim().Should().Be("Home", "Home button should have 'Home' label");
    }

    [Fact]
    public void SubNav_HomeButton_IsActiveOnHomepage()
    {
        // Arrange - navigate to homepage
        var navMan = Services.GetRequiredService<NavigationManager>();
        navMan.NavigateTo("/");

        var sections = new List<Section>
        {
            new("ai", "Artificial Intelligence", "AI", "/ai", "AI",
                [new Collection("news", "News", "/ai/news", "Latest news", "News")])
        };

        // Act
        var cut = Render<SubNav>(parameters => parameters
            .Add(p => p.Sections, sections)
            .Add(p => p.ShowHomeButton, true)
            .Add(p => p.ShowNewsletter, true));

        // Assert - Home button active, Newsletter button not active
        var homeLink = cut.Find("a.btn-subnav[href='/']");
        homeLink.ClassList.Should().Contain("active",
            "Home button should be highlighted when on the homepage");

        var newsletterLink = cut.Find("a.btn-subnav[href='/newsletter/subscribe']");
        newsletterLink.ClassList.Should().NotContain("active",
            "Newsletter button should not be highlighted on the homepage");
    }

    [Fact]
    public void SubNav_NewsletterButton_IsActiveOnNewsletterPage()
    {
        // Arrange - navigate to a newsletter page
        var navMan = Services.GetRequiredService<NavigationManager>();
        navMan.NavigateTo("/newsletter/subscribe");

        var sections = new List<Section>
        {
            new("ai", "Artificial Intelligence", "AI", "/ai", "AI",
                [new Collection("news", "News", "/ai/news", "Latest news", "News")])
        };

        // Act
        var cut = Render<SubNav>(parameters => parameters
            .Add(p => p.Sections, sections)
            .Add(p => p.ShowHomeButton, true)
            .Add(p => p.ShowNewsletter, true));

        // Assert - Newsletter button active, Home button not active
        var newsletterLink = cut.Find("a.btn-subnav[href='/newsletter/subscribe']");
        newsletterLink.ClassList.Should().Contain("active",
            "Newsletter button should be highlighted on any newsletter page");

        var homeLink = cut.Find("a.btn-subnav[href='/']");
        homeLink.ClassList.Should().NotContain("active",
            "Home button should not be highlighted on a newsletter page");
    }

    [Fact]
    public void SubNav_HomeButtonRenderedBeforeStats()
    {
        // Arrange - stats loaded so stat chips appear
        var statsState = Services.GetRequiredService<HomepageStatsState>();
        statsState.Update(recentCount: 5, totalCount: 100, weekAgoDate: "2025-01-01", sectionsCount: 2);

        var sections = new List<Section>
        {
            new("ai", "Artificial Intelligence", "AI", "/ai", "AI",
                [new Collection("news", "News", "/ai/news", "Latest news", "News")])
        };

        // Act
        var cut = Render<SubNav>(parameters => parameters
            .Add(p => p.Sections, sections)
            .Add(p => p.ShowHomeButton, true)
            .Add(p => p.ShowNewsletter, true));

        // Assert - Home appears before the stat chips in the DOM
        var wrapper = cut.Find(".sub-nav-wrapper");
        var children = wrapper.Children.ToList();
        var homeIndex = children.FindIndex(el => el.GetAttribute("href") == "/");
        var statIndex = children.FindIndex(el => el.ClassList.Contains("btn-subnav-stat-total"));
        var newsletterIndex = children.FindIndex(el => el.GetAttribute("href") == "/newsletter/subscribe");

        homeIndex.Should().BeGreaterThanOrEqualTo(0, "Home button should be present in the sub-nav-wrapper");
        statIndex.Should().BeGreaterThanOrEqualTo(0, "Total stat button should be present in the sub-nav-wrapper");
        newsletterIndex.Should().BeGreaterThanOrEqualTo(0, "Newsletter button should be present in the sub-nav-wrapper");

        homeIndex.Should().BeLessThan(statIndex,
            "Home button should appear to the left (before) the stat chips");
        statIndex.Should().BeLessThan(newsletterIndex,
            "Stat chips should appear between Home and Newsletter buttons");
    }
}
