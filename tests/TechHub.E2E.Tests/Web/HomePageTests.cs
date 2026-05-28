using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

public class HomePageTests : PlaywrightTestBase
{
    public HomePageTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    // Roundup Sidebar Section Tests
    [Fact]
    public async Task HomePage_ShouldDisplay_LatestRoundupSection()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert - Look for "Latest Roundups" heading in sidebar
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Latest Roundups");
    }

    [Fact]
    public async Task HomePage_LatestRoundupSection_ShouldDisplay_RoundupLinksPerSection()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert - Should display latest roundups section
        var latestRoundupsSection = Page.Locator(".latest-roundups");
        await latestRoundupsSection.AssertElementVisibleAsync();

        // Should have at least one roundup list item
        var roundupLinks = Page.Locator(".latest-roundups a.sidebar-content-button");
        var count = await roundupLinks.CountAsync();
        count.Should().BeGreaterThan(0, "homepage should display at least one per-section roundup link");

        // Each link should have a title and section label
        var firstLink = roundupLinks.First;
        await firstLink.Locator(".sidebar-content-button-title").AssertElementVisibleAsync();
        await firstLink.Locator(".sidebar-item-section").AssertElementVisibleAsync();
    }

    [Fact]
    public async Task HomePage_ShouldDisplay_NewsletterLink()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert - Look for link containing "newsletter"
        var newsletterLink = Page.Locator("a:has-text('newsletter')");
        await newsletterLink.AssertElementVisibleAsync();

        // Should link to internal subscribe page (href check)
        var href = await newsletterLink.GetHrefAsync();
        href.Should().Contain("/newsletter/subscribe",
            "newsletter link should point to the internal subscribe page, not an external URL");
    }

    // General Sidebar Tests
    [Fact]
    public async Task HomePage_ShouldDisplay_Sidebar()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert
        await Page.AssertElementVisibleBySelectorAsync(".sidebar");
    }

    [Fact]
    public async Task HomePage_Sidebar_ShouldDisplay_LatestItemsSection()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert - Use specific heading "Latest Content" to avoid ambiguity with "Latest Roundup"
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Latest Content");
    }

    [Fact]
    public async Task HomePage_Sidebar_ShouldDisplay_UpTo10LatestItems()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert - Find latest items section
        var latestSection = Page.Locator(".latest-items, .sidebar-latest");

        var count = await latestSection.GetElementCountBySelectorAsync("a");
        count.Should().BeInRange(1, 10, $"Expected 1-10 latest item links, but found {count}");
    }

    [Fact]
    public async Task HomePage_Sidebar_ShouldDisplay_PopularTagsSection()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Popular Tags");
    }

    [Fact]
    public async Task HomePage_Sidebar_ShouldDisplay_TagLinks()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert - Wait for tag cloud to render, then count tag buttons
        var tagButtons = Page.Locator(".tag-cloud-item");
        await Assertions.Expect(tagButtons.First).ToBeVisibleAsync();
        var count = await tagButtons.CountAsync();
        count.Should().BeGreaterThan(0, $"Expected at least one tag button, but found {count}");
    }

    [Fact]
    public async Task HomePage_Sidebar_LatestItemLinks_ShouldNavigateToContent()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Act - Click first latest item link (if any)
        var latestSection = Page.Locator(".latest-items, .sidebar-latest");
        var itemLinks = latestSection.Locator("a").Filter(new() { HasNotText = "Latest" });
        var count = await latestSection.GetElementCountBySelectorAsync("a");

        if (count > 0)
        {
            var firstLink = itemLinks.First;
            var target = await firstLink.GetAttributeAsync("target");
            var isExternalLink = target == "_blank";

            if (isExternalLink)
            {
                // External links open in new tab - verify the link has correct attributes
                // and a valid href (we don't click because it would open a new tab to external site)
                var href = await firstLink.GetAttributeAsync("href");
                href.Should().NotBeNullOrWhiteSpace("External link should have a valid href");
                href.Should().StartWith("http", "External link should have an absolute URL");
            }
            else
            {
                // Internal links should navigate within the current page
                await firstLink.ClickAndExpectAsync(async () =>
                    await Assertions.Expect(Page).Not.ToHaveURLAsync(
                        new Regex($"^{Regex.Escape(BlazorHelpers.BaseUrl)}/?$")));
            }
        }
    }

    [Fact]
    public async Task HomePage_Sidebar_TagLinks_ShouldFilterByTag()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Act - Click first tag link (if any)
        var tagsSection = Page.Locator(".popular-tags, .sidebar-tags");
        var tagLinks = tagsSection.Locator("a");
        var count = await tagsSection.GetElementCountBySelectorAsync("a");

        if (count > 0)
        {
            await tagLinks.First.ClickAndExpectAsync(async () =>
                await Assertions.Expect(Page).Not.ToHaveURLAsync(
                    new Regex($"^{Regex.Escape(BlazorHelpers.BaseUrl)}/?$")));

            // Assert - Should navigate to filtered view or section with tag parameter
            var currentUrl = Page.Url;
            (currentUrl.Contains("tag=", StringComparison.OrdinalIgnoreCase) ||
                currentUrl.Contains("tags=", StringComparison.OrdinalIgnoreCase) ||
                currentUrl != $"{BlazorHelpers.BaseUrl}/")
                .Should().BeTrue($"Expected URL to change or contain tag parameter, but got: {currentUrl}");
        }
    }
}
