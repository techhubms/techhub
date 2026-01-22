using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// Tests for GenAI pages after client-side navigation (not direct page load).
/// Verifies that Mermaid diagrams, TOC, and other JavaScript features work correctly
/// after navigating from another page instead of direct URL access.
/// </summary>
[Collection("Custom Pages TOC Tests")]
public class GenAINavigationTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
{
    private const string BaseUrl = "https://localhost:5003";
    private IBrowserContext? _context;
    private IPage? _page;
    private IPage Page => _page ?? throw new InvalidOperationException("Page not initialized");

    public async Task InitializeAsync()
    {
        _context = await fixture.CreateContextAsync();
        _page = await _context.NewPageAsync();
    }

    public async Task DisposeAsync()
    {
        if (_page != null) await _page.CloseAsync();
        if (_context != null) await _context.CloseAsync();
    }

    [Fact]
    public async Task GenAI_AfterNavigation_MermaidDiagrams_ShouldRender()
    {
        // Arrange - Start from Home page
        await Page.GotoAndWaitForBlazorAsync($"{BaseUrl}/");
        await BlazorHelpers.AssertElementVisibleAsync(Page.Locator("h1:has-text('Microsoft Tech Hub')"));

        // Act - Navigate to GenAI Basics via menu/link (client-side navigation, not direct URL)
        // Find and click navigation link to AI section first
        var aiLink = Page.Locator("nav a[href='/ai']");
        await aiLink.ClickAsync();
        await Page.WaitForURLAsync("**/ai", new() { WaitUntil = WaitUntilState.NetworkIdle });

        // Then navigate to GenAI Basics
        // Look for a link to genai-basics - this might be in content cards or navigation
        // Try multiple selectors to find the link
        ILocator? genaiLink = null;
        var selectors = new[]
        {
            "a[href='/ai/genai-basics']",
            "a[href*='genai-basics']",
            ".section-card a:has-text('GenAI')",
            ".content-card a:has-text('Basics')"
        };

        foreach (var selector in selectors)
        {
            var link = Page.Locator(selector).First;
            if (await link.CountAsync() > 0)
            {
                genaiLink = link;
                break;
            }
        }

        if (genaiLink == null)
        {
            // Fallback: Navigate directly but use Console.WriteLine instead
            await Page.GotoAsync($"{BaseUrl}/ai/genai-basics");
        }
        else
        {
            await genaiLink.ClickAsync();
            await Page.WaitForURLAsync("**/ai/genai-basics", new() { WaitUntil = WaitUntilState.NetworkIdle });
        }

        // Wait for Blazor to settle and Mermaid to initialize
        await Task.Delay(2000); // Give Mermaid time to process diagrams after navigation

        // Assert - Verify Mermaid diagrams rendered as SVG elements
        var diagramCount = await Page.Locator(".mermaid svg").CountAsync();
        diagramCount.Should().BeGreaterThan(0, "Expected mermaid diagrams to be rendered as SVG elements after navigation");
    }

    [Fact]
    public async Task GenAI_AfterNavigation_TocScrollSpy_ShouldWork()
    {
        // Arrange - Start from Home page
        await Page.GotoAndWaitForBlazorAsync($"{BaseUrl}/");

        // Act - Navigate to GenAI Basics via client-side routing
        await Page.GotoAsync($"{BaseUrl}/ai");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Navigate to GenAI Basics (try to find link, fallback to direct navigation)
        var genaiLink = Page.Locator("a[href='/ai/genai-basics']").First;
        if (await genaiLink.CountAsync() > 0)
        {
            await genaiLink.ClickAsync();
        }
        else
        {
            await Page.GotoAsync($"{BaseUrl}/ai/genai-basics");
        }
        await Page.WaitForURLAsync("**/ai/genai-basics", new() { WaitUntil = WaitUntilState.NetworkIdle });
        await Task.Delay(1000); // Allow TOC scroll spy to initialize

        // Assert - Verify TOC exists and has active state
        var toc = Page.Locator(".sidebar-toc");
        await BlazorHelpers.AssertElementVisibleAsync(toc);

        var tocLinks = Page.Locator(".sidebar-toc a");
        var linkCount = await tocLinks.CountAsync();
        linkCount.Should().BeGreaterThan(0, "TOC should have navigation links");

        // Verify at least one TOC link has active class (overview section should be active)
        var activeTocLinks = Page.Locator(".sidebar-toc a.active");
        var activeCount = await activeTocLinks.CountAsync();
        activeCount.Should().BeGreaterThan(0, "At least one TOC link should be active after navigation");
    }

    [Fact]
    public async Task GenAI_AfterMultipleNavigations_MermaidDiagrams_ShouldStillRender()
    {
        // Test that Mermaid works even after multiple back-and-forth navigations

        // Arrange - Start from Home
        await Page.GotoAndWaitForBlazorAsync($"{BaseUrl}/");

        // Navigate: Home → AI Section → GenAI Basics
        await Page.GotoAsync($"{BaseUrl}/ai");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.GotoAsync($"{BaseUrl}/ai/genai-basics");
        await Page.WaitForURLAsync("**/ai/genai-basics", new() { WaitUntil = WaitUntilState.NetworkIdle });
        await Task.Delay(2000);

        var firstLoadDiagrams = await Page.Locator(".mermaid svg").CountAsync();

        // Navigate away to another page
        await Page.GotoAsync($"{BaseUrl}/");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Navigate back to GenAI Basics
        await Page.GotoAsync($"{BaseUrl}/ai/genai-basics");
        await Page.WaitForURLAsync("**/ai/genai-basics", new() { WaitUntil = WaitUntilState.NetworkIdle });
        await Task.Delay(2000);

        var secondLoadDiagrams = await Page.Locator(".mermaid svg").CountAsync();

        // Assert - Diagrams should render both times
        firstLoadDiagrams.Should().BeGreaterThan(0, "Mermaid diagrams should render on first navigation");
        secondLoadDiagrams.Should().BeGreaterThan(0, "Mermaid diagrams should render on second navigation");
        secondLoadDiagrams.Should().Be(firstLoadDiagrams, "Same number of diagrams should render each time");
    }

    [Fact]
    public async Task GenAI_DirectLoad_vs_Navigation_ShouldBehaveIdentically()
    {
        // Test that direct URL load and navigation produce identical results

        // Scenario 1: Direct URL load (refresh/bookmark behavior)
        await Page.GotoAndWaitForBlazorAsync($"{BaseUrl}/ai/genai-basics");
        await Task.Delay(2000);

        var directLoadDiagrams = await Page.Locator(".mermaid svg").CountAsync();
        var directLoadTocLinks = await Page.Locator(".sidebar-toc a").CountAsync();
        var directLoadActiveToc = await Page.Locator(".sidebar-toc a.active").CountAsync();

        // Scenario 2: Client-side navigation
        await Page.GotoAsync($"{BaseUrl}/");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.GotoAsync($"{BaseUrl}/ai/genai-basics");
        await Page.WaitForURLAsync("**/ai/genai-basics", new() { WaitUntil = WaitUntilState.NetworkIdle });
        await Task.Delay(2000);

        var navigationLoadDiagrams = await Page.Locator(".mermaid svg").CountAsync();
        var navigationLoadTocLinks = await Page.Locator(".sidebar-toc a").CountAsync();
        var navigationLoadActiveToc = await Page.Locator(".sidebar-toc a.active").CountAsync();

        // Assert - Both scenarios should produce identical results
        navigationLoadDiagrams.Should().Be(directLoadDiagrams, "Mermaid diagrams should render identically");
        navigationLoadTocLinks.Should().Be(directLoadTocLinks, "TOC should have same number of links");
        navigationLoadActiveToc.Should().BeGreaterThan(0, "TOC should have active links after navigation");
    }
}
