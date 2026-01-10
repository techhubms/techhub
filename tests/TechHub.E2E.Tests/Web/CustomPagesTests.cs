using TechHub.E2E.Tests.Helpers;
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Xunit;

namespace TechHub.E2E.Tests.Web;

[Collection("Custom Pages Tests")]
public class CustomPagesTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
{
    private const string BaseUrl = "http://localhost:5184";
    private IBrowserContext? _context;
    private IPage Page => _page ?? throw new InvalidOperationException("Page not initialized");
    private IPage? _page;

    public async Task InitializeAsync()
    {
        _context = await fixture.CreateContextAsync();
        _page = await _context.NewPageWithDefaultsAsync();
    }

    public async Task DisposeAsync()
    {
        if (_page != null)
            await _page.CloseAsync();

        if (_context != null)
            await _context.CloseAsync();
    }
    [Theory]
    [InlineData("/ai/genai-basics", "GenAI Basics")]
    [InlineData("/ai/genai-applied", "GenAI Applied")]
    [InlineData("/ai/genai-advanced", "GenAI Advanced")]
    [InlineData("/ai/sdlc", "SDLC")]
    [InlineData("/github-copilot/features", "Features")]
    [InlineData("/github-copilot/handbook", "GitHub Copilot Handbook")]
    [InlineData("/github-copilot/levels-of-enlightenment", "Levels of Enlightenment")]
    [InlineData("/github-copilot/vscode-updates", "VSCode Updates")]
    [InlineData("/devops/dx-space", "DX")]
    public async Task CustomPage_ShouldLoad_Successfully(string url, string expectedTitlePart)
    {
        // Act
        await Page.GotoRelativeAsync(url);

        // Assert - Check page title attribute contains expected text
        await Assertions.Expect(Page).ToHaveTitleAsync(new Regex(expectedTitlePart));
    }

    [Theory]
    [InlineData("/ai/genai-basics")]
    [InlineData("/ai/genai-applied")]
    [InlineData("/ai/genai-advanced")]
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

        // Assert - Page should have main heading
        var mainHeading = Page.GetByRole(AriaRole.Heading, new() { Level = 1 });
        await mainHeading.AssertElementVisibleAsync();

        // Should have some content (paragraphs, lists, etc.)
        var paragraphs = Page.Locator("p");
        var count = await Page.GetElementCountBySelectorAsync("p");
        Assert.True(count > 0, $"Expected at least one paragraph on {url}, but found {count}");
    }

    [Fact]
    public async Task GitHubCopilotHandbook_ShouldDisplay_BookInformation()
    {
        // Act
        await Page.GotoRelativeAsync("/github-copilot/handbook");

        // Assert - Check for author headings (more specific than just text)
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Rob Bos");
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Randy Pagels");

        // Check for Amazon link (may be in a sentence, use text contains)
        var amazonLinkExists = await Page.GetByRole(AriaRole.Link).Filter(new() { HasText = "Amazon" }).CountAsync() > 0;
        Assert.True(amazonLinkExists, "Expected to find a link containing 'Amazon' text");
    }

    [Fact]
    public async Task GenAIBasics_ShouldDisplay_TableOfContents()
    {
        // Act
        await Page.GotoRelativeAsync("/ai/genai-basics");

        // Assert - Should have major section headings (use exact names to avoid strict mode)
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "History");
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Models");
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Tokens & Tokenization");
    }

    [Fact]
    public async Task DXSpace_ShouldDisplay_FrameworkSections()
    {
        // Act
        await Page.GotoRelativeAsync("/devops/dx-space");

        // Assert - Check for placeholder content headings
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Overview");
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Key Topics");
    }
}
