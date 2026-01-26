using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for GitHub Copilot Handbook custom page.
/// Verifies book information display and page-specific features.
/// 
/// Common component tests (TOC, keyboard nav) are in:
/// - SidebarTocTests.cs: Table of contents behavior
/// </summary>
[Collection("Custom Pages TOC Tests")]
public class HandbookTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
{
    private const string PageUrl = "/github-copilot/handbook";
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
    public async Task Handbook_ShouldLoad_Successfully()
    {
        // Act
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Check page title attribute contains expected text
        await Assertions.Expect(Page).ToHaveTitleAsync(new System.Text.RegularExpressions.Regex("GitHub Copilot Handbook"));
    }

    [Fact]
    public async Task Handbook_HeroSection_ShouldDisplay()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Check hero section exists
        var hero = Page.Locator(".handbook-hero");
        await hero.AssertElementVisibleAsync();

        // Check book cover image
        var bookCover = hero.Locator("img");
        await bookCover.AssertElementVisibleAsync();

        // Check title
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "The GitHub Copilot Handbook", level: 1);

        // Check CTA button
        var ctaButton = hero.Locator("a.handbook-cta");
        await ctaButton.AssertElementVisibleAsync();
    }

    [Fact]
    public async Task Handbook_ShouldDisplay_BookInformation()
    {
        // Act
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Check for book title heading
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "The GitHub Copilot Handbook", level: 1);

        // Check for author names in text (they appear in bold within paragraphs, not as headings)
        var pageContent = await Page.ContentAsync();
        pageContent.Should().Contain("Rob Bos");
        pageContent.Should().Contain("Randy Pagels");

        // Check for Amazon link (may be in a sentence, use text contains)
        var amazonLinkExists = await Page.GetByRole(AriaRole.Link).Filter(new() { HasText = "Amazon" }).CountAsync() > 0;
        amazonLinkExists.Should().BeTrue("Expected to find a link containing 'Amazon' text");
    }

    [Fact]
    public async Task Handbook_ShouldNot_HaveConsoleErrors()
    {
        // Arrange - Collect console messages
        var consoleMessages = new List<IConsoleMessage>();
        Page.Console += (_, msg) => consoleMessages.Add(msg);

        // Act
        await Page.GotoRelativeAsync(PageUrl);

        // Wait briefly for any console errors to be logged
        await Page.WaitForTimeoutAsync(500);

        // Assert - No console errors (filter WebSocket connection errors from Blazor)
        var errors = consoleMessages
            .Where(m => m.Type == "error")
            .Where(m => !m.Text.Contains("WebSocket"))
            .Where(m => !m.Text.Contains("ERR_CONNECTION_REFUSED"))
            .ToList();

        errors.Should().BeEmpty($"Expected no console errors on {PageUrl}, but found: {string.Join(", ", errors.Select(e => e.Text))}");
    }

}
