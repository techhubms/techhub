using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for Mermaid diagram rendering.
/// Tests on representative page to avoid duplication.
/// 
/// Test Pages:
/// - /ai/genai-basics - Has 11+ mermaid diagrams (flowcharts, sequence diagrams, etc.)
/// 
/// Coverage:
/// - Mermaid diagrams render as SVG elements on direct page load
/// - Mermaid diagrams render correctly after client-side navigation
/// - Mermaid diagrams persist across multiple navigations
/// - Multiple diagrams can coexist on same page
/// - Diagrams are visible to users
/// </summary>
[Collection("Mermaid Tests")]
public class MermaidTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
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
    public async Task MermaidDiagrams_ShouldRender_AsSvgElements()
    {
        // Arrange & Act
        await Page.GotoRelativeAsync("/ai/genai-basics");

        // Wait for mermaid diagrams to render
        await Page.WaitForMermaidDiagramsAsync();

        // Assert - Check for mermaid diagrams (rendered as SVG by mermaid.js)
        var mermaidDiagrams = Page.Locator("svg[id^='mermaid-']");
        var diagramCount = await mermaidDiagrams.CountAsync();

        diagramCount.Should().BeGreaterThan(0, "Expected mermaid diagrams to be rendered as SVG elements");

        // Verify at least some of the 11 expected diagrams are present
        // Note: Not all may be visible depending on viewport, so we check for at least 3
        diagramCount.Should().BeGreaterThanOrEqualTo(3, "Expected at least 3 mermaid diagrams to be visible");
    }

    [Fact]
    public async Task MermaidDiagrams_Page_ShouldNotHaveConsoleErrors()
    {
        // Arrange - Collect console messages
        var consoleMessages = new List<IConsoleMessage>();
        Page.Console += (_, msg) => consoleMessages.Add(msg);

        // Act
        await Page.GotoRelativeAsync("/ai/genai-basics");

        // Wait for page to fully load and mermaid to render
        await Page.WaitForMermaidDiagramsAsync();

        // Assert - No console errors (filter out ad-blocker errors)
        var errors = consoleMessages
            .Where(m => m.Type == "error")
            .Where(e => !e.Text.Contains("ERR_CONNECTION_REFUSED"))
            .Where(e => !e.Text.Contains("ERR_ADDRESS_INVALID"))
            .ToList();

        errors.Should().BeEmpty($"Expected no console errors, but found: {string.Join(", ", errors.Select(e => e.Text))}");
    }

    [Fact]
    public async Task MermaidDiagrams_AfterClientSideNavigation_ShouldRender()
    {
        // Arrange - Start from Home page
        await Page.GotoRelativeAsync("/");

        // Act - Navigate to GenAI Basics via client-side navigation
        await Page.GotoRelativeAsync("/ai");
        await Page.GotoRelativeAsync("/ai/genai-basics");

        // Wait for Mermaid diagrams to render
        var mermaidDiagrams = Page.Locator(".mermaid svg");
        await Assertions.Expect(mermaidDiagrams.First).ToBeVisibleAsync(new() { Timeout = 5000 });

        // Assert - Verify Mermaid diagrams rendered as SVG elements
        var diagramCount = await mermaidDiagrams.CountAsync();
        diagramCount.Should().BeGreaterThan(0, "Expected mermaid diagrams to be rendered as SVG elements after navigation");
    }

    [Fact]
    public async Task MermaidDiagrams_AfterMultipleNavigations_ShouldStillRender()
    {
        // Test that Mermaid works even after multiple back-and-forth navigations

        // Arrange - Start from Home
        await Page.GotoRelativeAsync("/");

        // Navigate: Home → AI Section → GenAI Basics
        await Page.GotoRelativeAsync("/ai");
        await Page.GotoRelativeAsync("/ai/genai-basics");

        // Wait for Mermaid diagrams to be visible
        var mermaidDiagrams = Page.Locator(".mermaid svg");
        await Assertions.Expect(mermaidDiagrams.First).ToBeVisibleAsync(new() { Timeout = 5000 });

        var firstLoadDiagrams = await mermaidDiagrams.CountAsync();

        // Navigate away to another page
        await Page.GotoRelativeAsync("/");

        // Navigate back to GenAI Basics
        await Page.GotoRelativeAsync("/ai/genai-basics");

        // Wait for Mermaid diagrams to be visible again
        await Assertions.Expect(mermaidDiagrams.First).ToBeVisibleAsync(new() { Timeout = 5000 });

        var secondLoadDiagrams = await mermaidDiagrams.CountAsync();

        // Assert - Diagrams should render both times
        firstLoadDiagrams.Should().BeGreaterThan(0, "Mermaid diagrams should render on first navigation");
        secondLoadDiagrams.Should().BeGreaterThan(0, "Mermaid diagrams should render on second navigation");
        secondLoadDiagrams.Should().Be(firstLoadDiagrams, "Same number of diagrams should render each time");
    }

    [Fact]
    public async Task MermaidDiagrams_DirectLoad_vs_Navigation_ShouldBehaveIdentically()
    {
        // Test that direct URL load and navigation produce identical results

        // Scenario 1: Direct URL load (refresh/bookmark behavior)
        await Page.GotoRelativeAsync("/ai/genai-basics");

        // Wait for Mermaid diagrams to be visible
        var mermaidDiagrams = Page.Locator(".mermaid svg");
        await Assertions.Expect(mermaidDiagrams.First).ToBeVisibleAsync(new() { Timeout = 5000 });

        var directLoadDiagrams = await mermaidDiagrams.CountAsync();

        // Scenario 2: Client-side navigation
        await Page.GotoRelativeAsync("/");
        await Page.GotoRelativeAsync("/ai/genai-basics");

        // Wait for Mermaid diagrams to be visible again
        await Assertions.Expect(mermaidDiagrams.First).ToBeVisibleAsync(new() { Timeout = 5000 });

        var navigationLoadDiagrams = await mermaidDiagrams.CountAsync();

        // Assert - Both scenarios should produce identical results
        navigationLoadDiagrams.Should().Be(directLoadDiagrams, "Mermaid diagrams should render identically whether loaded directly or via navigation");
    }
}
