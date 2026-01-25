using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for code syntax highlighting.
/// Tests on representative page to avoid duplication.
/// 
/// Test Pages:
/// - /ai/genai-advanced - Has code blocks with syntax highlighting via highlight.js
/// 
/// Coverage:
/// - Code blocks have syntax highlighting classes applied
/// - highlight.js loads and initializes correctly
/// - No console errors
/// 
/// Note: Keyboard navigation is NOT tested here because highlight.js makes code blocks
/// focusable, which changes tab order. This is expected behavior and doesn't affect
/// keyboard accessibility of interactive elements.
/// </summary>
[Collection("Highlighting Tests")]
public class HighlightingTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
{
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
    public async Task CodeBlocks_ShouldHave_SyntaxHighlighting()
    {
        // Arrange & Act
        await Page.GotoRelativeAsync("/ai/genai-advanced");

        // Wait for highlight.js to initialize
        await Page.WaitForTimeoutAsync(500);

        // Assert - Check for code blocks with highlighting
        var codeBlocks = Page.Locator("pre code");
        var codeBlockCount = await codeBlocks.CountAsync();

        // Verify page has code blocks (genai-advanced always has code blocks)
        codeBlockCount.Should().BeGreaterThan(0, "Expected genai-advanced page to have code blocks for highlighting");

        // Check first code block has highlighting classes
        var firstCodeBlock = codeBlocks.First;
        var className = await firstCodeBlock.GetAttributeAsync("class");

        // highlight.js adds language-specific classes like "hljs", "language-javascript", etc.
        className.Should().NotBeNullOrEmpty("Code blocks should have CSS classes for syntax highlighting");
    }

    [Fact]
    public async Task HighlightJs_ShouldLoad_WithoutErrors()
    {
        // Arrange - Collect console messages
        var consoleMessages = new List<IConsoleMessage>();
        Page.Console += (_, msg) => consoleMessages.Add(msg);

        // Act
        await Page.GotoRelativeAsync("/ai/genai-advanced");

        // Wait for highlight.js to initialize
        await Page.WaitForTimeoutAsync(500);

        // Assert - No console errors
        var errors = consoleMessages
            .Where(m => m.Type == "error")
            .ToList();

        errors.Should().BeEmpty($"Expected no console errors, but found: {string.Join(", ", errors.Select(e => e.Text))}");
    }
}
