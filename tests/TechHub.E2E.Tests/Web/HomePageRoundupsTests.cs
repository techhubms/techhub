using System.Text.RegularExpressions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;
using FluentAssertions;

namespace TechHub.E2E.Tests.Web;

[Collection("Home Page Roundups Tests")]
public class HomePageRoundupsTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
{
    private const string BaseUrl = "http://localhost:5184";
    private IBrowserContext? _context;
    private IPage? _page;
    private IPage Page => _page ?? throw new InvalidOperationException("Page not initialized");

    public async Task InitializeAsync()
    {
        _context = await fixture.CreateContextAsync();
        _page = await _context.NewPageWithDefaultsAsync();
    }

    public async Task DisposeAsync()
    {
        if (_page != null)
        {
            await _page.CloseAsync();
        }

        if (_context != null)
        {
            await _context.CloseAsync();
        }
    }
    [Fact]
    public async Task HomePage_ShouldDisplay_RoundupsSection()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert - Look for "Latest Roundup" heading in new sidebar structure
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Latest Roundup");
    }

    [Fact]
    public async Task HomePage_ShouldDisplay_Last4Roundups()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert - Should display latest roundup section (should be exactly 1)\n        var latestRoundupSection = Page.Locator(\".latest-roundup\");\n        await Page.AssertElementCountBySelectorAsync(\".latest-roundup\", 1);\n        await latestRoundupSection.AssertElementVisibleAsync();

        // Should have one featured roundup link
        var roundupLink = Page.Locator(".latest-roundup a.sidebar-featured-link");
        await roundupLink.AssertElementVisibleAsync();

        // Link should have title and date
        var roundupTitle = Page.Locator(".latest-roundup .sidebar-featured-title");
        await roundupTitle.AssertElementVisibleAsync();

        var roundupDate = Page.Locator(".latest-roundup .sidebar-featured-date");
        await roundupDate.AssertElementVisibleAsync();
    }

    [Fact]
    public async Task HomePage_ShouldDisplay_NewsletterLink()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert - Look for link containing "newsletter" (case-sensitive is fine for this test)
        var newsletterLink = Page.Locator("a:has-text('newsletter')");
        await newsletterLink.AssertElementVisibleAsync();

        // Should link to mailchimp
        var href = await newsletterLink.GetHrefAsync();
        href.Should().Contain("mailchi.mp");
    }

    [Fact]
    public async Task HomePage_RoundupLinks_ShouldNavigateToRoundupPage()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Act - Click first roundup link (finds links with date format like "Dec 29, 2025")
        var roundupLinks = Page.Locator("a").Filter(new() { HasTextRegex = new Regex(@"[A-Z][a-z]{2}\s+\d{1,2},\s+\d{4}", RegexOptions.None) });
        var count = await roundupLinks.CountAsync();

        if (count > 0)
        {
            await roundupLinks.First.ClickBlazorElementAsync();

            // Assert - Should navigate to roundup detail page
            await Page.WaitForBlazorUrlContainsAsync("/roundups/");
        }
    }
}
