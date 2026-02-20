using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Core.Models;
using TechHub.TestUtilities.Builders;
using TechHub.Web.Components;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for NavHeader.razor component - hamburger menu and mobile navigation.
/// Tests verify the hierarchical mobile menu with expandable sections.
/// </summary>
public class NavHeaderTests : BunitContext
{
    private readonly SectionCache _sectionCache;

    public NavHeaderTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;

        _sectionCache = new SectionCache();
        _sectionCache.Initialize(
        [
            new Section(
                "github-copilot",
                "GitHub Copilot",
                "GitHub Copilot resources",
                "/github-copilot",
                "GitHub Copilot",
                [
                    new Collection("news", "News", "/github-copilot/news", "Latest news", "News"),
                    new Collection("blogs", "Blogs", "/github-copilot/blogs", "Blog posts", "Blogs"),
                    new Collection("features", "Features", "/github-copilot/features", "Features page", "Features", isCustom: true, order: 1),
                    new Collection("handbook", "Handbook", "/github-copilot/handbook", "Handbook page", "Handbook", isCustom: true, order: 2)
                ]
            ),
            new Section(
                "ai",
                "Artificial Intelligence",
                "AI resources",
                "/ai",
                "AI",
                [
                    new Collection("news", "News", "/ai/news", "Latest news", "News"),
                    new Collection("blogs", "Blogs", "/ai/blogs", "Blog posts", "Blogs")
                ]
            )
        ]);

        Services.AddSingleton(_sectionCache);
    }

    [Fact]
    public void NavHeader_RendersHamburgerButton()
    {
        // Act
        var cut = Render<NavHeader>();

        // Assert - Hamburger button should exist in markup (hidden on desktop via CSS)
        var hamburgerBtn = cut.Find(".hamburger-btn");
        hamburgerBtn.Should().NotBeNull();
        hamburgerBtn.GetAttribute("aria-label").Should().Be("Open navigation menu");
        hamburgerBtn.GetAttribute("aria-expanded").Should().Be("false");
    }

    [Fact]
    public void NavHeader_HamburgerClick_OpensMenu()
    {
        // Arrange
        var cut = Render<NavHeader>();

        // Act
        cut.Find(".hamburger-btn").Click();

        // Assert
        var mobileMenu = cut.Find(".mobile-menu");
        mobileMenu.ClassList.Should().Contain("open");

        var hamburgerBtn = cut.Find(".hamburger-btn");
        hamburgerBtn.GetAttribute("aria-expanded").Should().Be("true");
        hamburgerBtn.GetAttribute("aria-label").Should().Be("Close navigation menu");
    }

    [Fact]
    public void NavHeader_MobileMenu_ContainsAllSections()
    {
        // Arrange
        var cut = Render<NavHeader>();

        // Act
        cut.Find(".hamburger-btn").Click();

        // Assert - Should have section headers for each section (+ About link)
        var sectionHeaders = cut.FindAll(".mobile-menu-section-header");
        sectionHeaders.Count.Should().Be(3);
        sectionHeaders[0].TextContent.Should().Contain("GitHub Copilot");
        sectionHeaders[1].TextContent.Should().Contain("Artificial Intelligence");
        sectionHeaders[2].TextContent.Should().Contain("About");
    }

    [Fact]
    public void NavHeader_MobileMenu_ContainsAboutLink()
    {
        // Arrange
        var cut = Render<NavHeader>();

        // Act
        cut.Find(".hamburger-btn").Click();

        // Assert - About is the last section header
        var sectionHeaders = cut.FindAll(".mobile-menu-section-header");
        var aboutLink = sectionHeaders.Last();
        aboutLink.TextContent.Should().Contain("About");
        aboutLink.GetAttribute("href").Should().Be("/about");
    }

    [Fact]
    public void NavHeader_SectionClick_ExpandsSubItems()
    {
        // Arrange
        var cut = Render<NavHeader>();
        cut.Find(".hamburger-btn").Click();

        // Act - Click on GitHub Copilot section header to expand
        var sectionHeader = cut.FindAll(".mobile-menu-section-header")[0];
        sectionHeader.Click();

        // Assert - Should show sub-items
        var subItems = cut.FindAll(".mobile-menu-sub-items a");
        subItems.Count.Should().BeGreaterThan(0);

        // Should contain "All" + regular collections + custom pages
        var subItemTexts = subItems.Select(s => s.TextContent.Trim()).ToList();
        subItemTexts.Should().Contain("All");
        subItemTexts.Should().Contain("News");
        subItemTexts.Should().Contain("Blogs");
        subItemTexts.Should().Contain("Features");
        subItemTexts.Should().Contain("Handbook");
    }

    [Fact]
    public void NavHeader_SectionClick_TogglesExpansion()
    {
        // Arrange
        var cut = Render<NavHeader>();
        cut.Find(".hamburger-btn").Click();

        // Act - Click to expand
        cut.FindAll(".mobile-menu-section-header")[0].Click();
        cut.FindAll(".mobile-menu-sub-items").Count.Should().Be(1);

        // Re-query after re-render to get fresh element references (bUnit requirement)
        cut.FindAll(".mobile-menu-section-header")[0].Click();

        // Assert - Sub-items should no longer be visible
        var subItemsCollapsed = cut.FindAll(".mobile-menu-sub-items");
        subItemsCollapsed.Count.Should().Be(0);
    }

    [Fact]
    public void NavHeader_SectionExpanded_AriaAttributeUpdates()
    {
        // Arrange
        var cut = Render<NavHeader>();
        cut.Find(".hamburger-btn").Click();

        cut.FindAll(".mobile-menu-section-header")[0].GetAttribute("aria-expanded").Should().Be("false");

        // Act
        cut.FindAll(".mobile-menu-section-header")[0].Click();

        // Assert - Re-query to get fresh element reference after re-render
        cut.FindAll(".mobile-menu-section-header")[0].GetAttribute("aria-expanded").Should().Be("true");
    }

    [Fact]
    public void NavHeader_MenuOpen_ShowsOverlay()
    {
        // Arrange
        var cut = Render<NavHeader>();

        // Act
        cut.Find(".hamburger-btn").Click();

        // Assert
        var overlay = cut.Find(".mobile-menu-overlay");
        overlay.Should().NotBeNull();
    }

    [Fact]
    public void NavHeader_OverlayClick_ClosesMenu()
    {
        // Arrange
        var cut = Render<NavHeader>();
        cut.Find(".hamburger-btn").Click();

        // Act
        cut.Find(".mobile-menu-overlay").Click();

        // Assert
        var mobileMenu = cut.Find(".mobile-menu");
        mobileMenu.ClassList.Should().NotContain("open");
        cut.FindAll(".mobile-menu-overlay").Count.Should().Be(0);
    }

    [Fact]
    public void NavHeader_HamburgerClickAgain_ClosesMenu()
    {
        // Arrange
        var cut = Render<NavHeader>();
        cut.Find(".hamburger-btn").Click();
        cut.Find(".mobile-menu").ClassList.Should().Contain("open");

        // Act
        cut.Find(".hamburger-btn").Click();

        // Assert
        var mobileMenu = cut.Find(".mobile-menu");
        mobileMenu.ClassList.Should().NotContain("open");
    }

    [Fact]
    public void NavHeader_StillRendersDesktopNavLinks()
    {
        // Act
        var cut = Render<NavHeader>();

        // Assert - Desktop nav links should still be in the markup (shown via CSS on desktop)
        var navLinks = cut.FindAll(".nav-links .nav-link");
        navLinks.Count.Should().BeGreaterThan(0);

        var linkTexts = navLinks.Select(l => l.TextContent.Trim()).ToList();
        linkTexts.Should().Contain("GitHub Copilot");
        linkTexts.Should().Contain("Artificial Intelligence");
        linkTexts.Should().Contain("About");
    }

    [Fact]
    public void NavHeader_HamburgerButton_HasThreeLines()
    {
        // Act
        var cut = Render<NavHeader>();

        // Assert - Hamburger icon should have 3 span elements for the lines
        var lines = cut.FindAll(".hamburger-btn .hamburger-line");
        lines.Count.Should().Be(3);
    }

    [Fact]
    public void NavHeader_MenuOpen_HamburgerHasOpenClass()
    {
        // Arrange
        var cut = Render<NavHeader>();

        // Act
        cut.Find(".hamburger-btn").Click();

        // Assert
        cut.Find(".hamburger-btn").ClassList.Should().Contain("open");
    }
}
