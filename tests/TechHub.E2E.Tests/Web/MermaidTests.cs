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
///
/// The Mermaid loader (see wwwroot/js/page-scripts.js) retries each CDN with
/// exponential backoff and falls back to a secondary CDN, so these tests can
/// assert unconditionally on diagram rendering.
/// </summary>
public class MermaidTests : PlaywrightTestBase
{
    private const string MermaidPageUrl = "/ai/genai-basics";

    public MermaidTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task MermaidDiagrams_ShouldRender_AsSvgElements()
    {
        await Page.GotoRelativeAsync(MermaidPageUrl);

        var mermaidDiagrams = Page.Locator("svg[id^='mermaid-']");
        await Assertions.Expect(mermaidDiagrams.First).ToBeVisibleAsync();

        var diagramCount = await mermaidDiagrams.CountAsync();

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

        await Page.GotoRelativeAsync(MermaidPageUrl);

        // Wait until diagrams render so any errors triggered during rendering surface first.
        await Assertions.Expect(Page.Locator(".mermaid svg").First).ToBeVisibleAsync();

        // Assert - No console errors (filter out ad-blocker / network errors outside our control)
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
        // Arrange - Start from Home page, then navigate via the AI section
        await Page.GotoRelativeAsync("/");
        await Page.GotoRelativeAsync("/ai");
        await Page.GotoRelativeAsync(MermaidPageUrl);

        var mermaidDiagrams = Page.Locator(".mermaid svg");
        await Assertions.Expect(mermaidDiagrams.First).ToBeVisibleAsync();

        var diagramCount = await mermaidDiagrams.CountAsync();
        diagramCount.Should().BeGreaterThan(0, "Expected mermaid diagrams to be rendered as SVG elements after navigation");
    }

    [Fact]
    public async Task MermaidDiagrams_AfterMultipleNavigations_ShouldStillRender()
    {
        // Test that Mermaid works even after multiple back-and-forth navigations
        var mermaidDiagrams = Page.Locator(".mermaid svg");

        // Navigate: Home → AI Section → GenAI Basics
        await Page.GotoRelativeAsync("/");
        await Page.GotoRelativeAsync("/ai");
        await Page.GotoRelativeAsync(MermaidPageUrl);

        await Assertions.Expect(mermaidDiagrams.First).ToBeVisibleAsync();
        var firstLoadDiagrams = await mermaidDiagrams.CountAsync();

        // Navigate away and back
        await Page.GotoRelativeAsync("/");
        await Page.GotoRelativeAsync(MermaidPageUrl);

        await Assertions.Expect(mermaidDiagrams.First).ToBeVisibleAsync();
        var secondLoadDiagrams = await mermaidDiagrams.CountAsync();

        firstLoadDiagrams.Should().BeGreaterThan(0, "Mermaid diagrams should render on first navigation");
        secondLoadDiagrams.Should().Be(firstLoadDiagrams, "Same number of diagrams should render each time");
    }

    [Fact]
    public async Task MermaidDiagrams_DirectLoad_vs_Navigation_ShouldBehaveIdentically()
    {
        var mermaidDiagrams = Page.Locator(".mermaid svg");

        // Scenario 1: Direct URL load (refresh/bookmark behavior)
        await Page.GotoRelativeAsync(MermaidPageUrl);
        await Assertions.Expect(mermaidDiagrams.First).ToBeVisibleAsync();
        var directLoadDiagrams = await mermaidDiagrams.CountAsync();

        // Scenario 2: Navigation from another page
        await Page.GotoRelativeAsync("/");
        await Page.GotoRelativeAsync(MermaidPageUrl);
        await Assertions.Expect(mermaidDiagrams.First).ToBeVisibleAsync();
        var navigationLoadDiagrams = await mermaidDiagrams.CountAsync();

        navigationLoadDiagrams.Should().Be(directLoadDiagrams, "Mermaid diagrams should render identically whether loaded directly or via navigation");
    }
}
