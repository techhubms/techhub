using TechHub.E2E.Tests.Helpers;
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Xunit;

namespace TechHub.E2E.Tests.Web;

[Collection("Home Page Roundups Tests")]
public class HomePageRoundupsTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
{
    private const string BaseUrl = "http://localhost:5184";
    private IBrowserContext? _context;
    private IPage Page => _page ?? throw new InvalidOperationException("Page not initialized");
    private IPage? _page;

    public async Task InitializeAsync()
    {
        _context = await fixture.CreateContextAsync();
        _page = await _context.NewPageAsync();
    }

    public async Task DisposeAsync()
    {
        if (_page != null)
            await _page.CloseAsync();

        if (_context != null)
            await _context.CloseAsync();
    }
    [Fact]
    public async Task HomePage_ShouldDisplay_RoundupsSection()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert - Look for "Latest Roundup" heading in new sidebar structure
        await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Latest Roundup" })).ToBeVisibleAsync();
    }

    [Fact]
    public async Task HomePage_ShouldDisplay_Last4Roundups()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert - Should display latest roundup section (use .First to avoid strict mode violation)
        var latestRoundupSection = Page.Locator(".latest-roundup").First;
        await Assertions.Expect(latestRoundupSection).ToBeVisibleAsync();

        // Should have one featured roundup link
        var roundupLink = Page.Locator(".latest-roundup a.sidebar-featured-link");
        await Assertions.Expect(roundupLink).ToBeVisibleAsync();

        // Link should have title and date
        var roundupTitle = Page.Locator(".latest-roundup .sidebar-featured-title");
        await Assertions.Expect(roundupTitle).ToBeVisibleAsync();

        var roundupDate = Page.Locator(".latest-roundup .sidebar-featured-date");
        await Assertions.Expect(roundupDate).ToBeVisibleAsync();
    }

    [Fact]
    public async Task HomePage_ShouldDisplay_NewsletterLink()
    {
        // Act
        await Page.GotoRelativeAsync("/");

        // Assert
        var newsletterLink = Page.GetByRole(AriaRole.Link, new() { NameRegex = new Regex("newsletter", RegexOptions.IgnoreCase) });
        await Assertions.Expect(newsletterLink).ToBeVisibleAsync();

        // Should link to mailchimp
        var href = await newsletterLink.GetHrefAsync();
        Assert.Contains("mailchi.mp", href);
    }

    [Fact]
    public async Task HomePage_RoundupLinks_ShouldNavigateToRoundupPage()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Act - Click first roundup link (if any)
        var roundupLinks = Page.Locator("a").Filter(new() { HasTextRegex = new Regex(@"\d{4}-\d{2}-\d{2}", RegexOptions.IgnoreCase) });
        var count = await roundupLinks.CountAsync();

        if (count > 0)
        {
            await roundupLinks.First.ClickAsync();

            // Assert - Should navigate to roundup detail page
            await Page.WaitForURLAsync(new Regex("/roundups/", RegexOptions.IgnoreCase));
            await Assertions.Expect(Page).ToHaveURLAsync(new Regex("/roundups/", RegexOptions.IgnoreCase));
        }
    }
}
