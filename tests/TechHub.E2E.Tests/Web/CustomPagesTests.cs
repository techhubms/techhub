using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

[Collection("Custom Pages Tests")]
public class CustomPagesTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
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
    [Theory]
    [InlineData("/ai/genai-applied", "GenAI Applied")]
    [InlineData("/ai/sdlc", "AI in the SDLC")]
    [InlineData("/github-copilot/features", "GitHub Copilot Features")]
    [InlineData("/github-copilot/handbook", "GitHub Copilot Handbook")]
    [InlineData("/github-copilot/levels-of-enlightenment", "Levels of Enlightenment")]
    [InlineData("/github-copilot/vscode-updates", "Visual Studio Code and GitHub Copilot - What's new in")] // Dynamic page shows latest video
    [InlineData("/devops/dx-space", "Developer Experience Space")]
    public async Task CustomPage_ShouldLoad_Successfully(string url, string expectedTitlePart)
    {
        // Act
        await Page.GotoRelativeAsync(url);

        // Assert - Check page title attribute contains expected text
        await Assertions.Expect(Page).ToHaveTitleAsync(new Regex(expectedTitlePart));
    }

    [Theory]
    [InlineData("/ai/genai-applied")]
    [InlineData("/ai/sdlc")]
    [InlineData("/github-copilot/features")]
    [InlineData("/github-copilot/handbook")]
    [InlineData("/github-copilot/levels-of-enlightenment")]
    [InlineData("/github-copilot/vscode-updates")]
    [InlineData("/devops/dx-space")]
    public async Task CustomPage_ShouldDisplay_Content(string url)
    {
        // Act
        await Page.GotoRelativeAsync(url);

        // Assert - Page should have main content heading (excluding banner heading)
        var mainHeading = Page.Locator(".page-h1");
        await mainHeading.AssertElementVisibleAsync();

        // Should have some content (paragraphs, lists, etc.)
        _ = Page.Locator("p");
        var count = await Page.GetElementCountBySelectorAsync("p");
        count.Should().BeGreaterThan(0, $"Expected at least one paragraph on {url}, but found {count}");
    }

    [Fact]
    public async Task GitHubCopilotHandbook_ShouldDisplay_BookInformation()
    {
        // Act
        await Page.GotoRelativeAsync("/github-copilot/handbook");

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
    public async Task GenAIApplied_ShouldDisplay_KeySections()
    {
        // Act
        await Page.GotoRelativeAsync("/ai/genai-applied");

        // Assert - Should have major section headings
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "GenAI Applied", level: 1);

        // Page should have some content paragraphs
        var paragraphCount = await Page.GetElementCountBySelectorAsync("p");
        paragraphCount.Should().BeGreaterThan(0, $"Expected at least one paragraph, but found {paragraphCount}");
    }

    [Fact]
    public async Task DXSpace_ShouldDisplay_FrameworkSections()
    {
        // Act
        await Page.GotoRelativeAsync("/devops/dx-space");

        // Assert - Check for section titles from the structured JSON data
        // The page uses expandable sections with titles like "DORA Metrics", "SPACE Framework", etc.
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Developer Experience Space", level: 1);

        // Check for expandable section cards by looking for text content in the page
        var pageContent = await Page.ContentAsync();
        pageContent.Should().Contain("DORA Metrics");
        pageContent.Should().Contain("SPACE Framework");
        pageContent.Should().Contain("Developer Experience");  // Part of the DevEx/DX section title
    }
}
