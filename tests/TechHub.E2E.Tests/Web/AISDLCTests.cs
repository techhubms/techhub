using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for AI-Powered SDLC custom page.
/// Reference implementation for custom pages with table of contents.
/// </summary>
[Collection("Custom Pages TOC Tests")]
public class AISDLCTests : IAsyncLifetime
{
    private readonly PlaywrightCollectionFixture _fixture;

    public AISDLCTests(PlaywrightCollectionFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        _fixture = fixture;
    }

    private const string PageUrl = "/ai/sdlc";
    private IBrowserContext? _context;
    private IPage? _page;
    private IPage Page => _page ?? throw new InvalidOperationException("Page not initialized");

    public async Task InitializeAsync()
    {
        _context = await _fixture.CreateContextAsync();
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
    public async Task AISDLC_ShouldLoad_Successfully()
    {
        // Act
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Check page title attribute contains expected text
        await Assertions.Expect(Page).ToHaveTitleAsync(new System.Text.RegularExpressions.Regex("AI SDLC"));
    }

    [Fact]
    public async Task AISDLC_ShouldRender_WithToc_ForPhaseSections()
    {
        // Arrange - Navigate and wait for page to fully render
        await Page.GotoRelativeAsync(PageUrl);

        // Wait for the TOC to render (it requires page data to load first)
        var toc = Page.Locator(".sidebar-toc");
        await Assertions.Expect(toc).ToBeVisibleAsync(new() { Timeout = 10000 });

        // Assert - Sidebar TOC should exist for this page with headings
        var tocExists = await toc.CountAsync();
        tocExists.Should().BeGreaterThan(0, "Expected TOC for page with SDLC phase headings");

        // Content should render - use main element specifically to avoid strict mode violation
        var mainContent = Page.Locator("main");
        await Assertions.Expect(mainContent).ToBeVisibleAsync(new() { Timeout = 5000 });
    }

    [Fact]
    public async Task AISDLC_ShouldDisplay_SDLCPhases()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Should have SDLC phases section
        var phasesSection = Page.Locator(".sdlc-phases");
        await phasesSection.AssertElementVisibleAsync();

        // Should have multiple phase cards
        var phaseCards = Page.Locator(".sdlc-phase-card");
        var phaseCount = await phaseCards.CountAsync();
        phaseCount.Should().BeGreaterThanOrEqualTo(5, "Expected at least 5 SDLC phases (Ideation, Planning, Design, Implementation, Testing, Deployment, Maintenance)");
    }

    [Fact]
    public async Task AISDLC_PhaseCards_ShouldHave_ColoredBorders()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Phase cards should have phase-specific CSS classes for colored left borders
        var ideationCard = Page.Locator(".sdlc-phase-ideation").First;
        await ideationCard.AssertElementVisibleAsync();

        // Verify that the CSS class is applied (border colors are defined in CSS)
        var className = await ideationCard.GetAttributeAsync("class");
        className.Should().Contain("sdlc-phase-", "Phase cards should have phase-specific CSS classes for styling");
        className.Should().Contain("sdlc-phase-ideation", "First phase should be ideation");
    }

    [Fact]
    public async Task AISDLC_PhaseCards_ShouldBe_Collapsible()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Get first phase header and content
        var phaseHeader = Page.Locator(".sdlc-phase-header").First;
        var phaseContent = Page.Locator(".sdlc-phase-content").First;

        // Assert - Content should be hidden initially (no expanded class)
        var contentClasses = await phaseContent.GetAttributeAsync("class");
        contentClasses.Should().NotContain("expanded", "Phase content should be collapsed by default");

        // Act - Click to expand
        await phaseHeader.ClickBlazorElementAsync(waitForUrlChange: false);

        // Assert - Content should get the 'expanded' class after click
        await Assertions.Expect(phaseContent).ToHaveClassAsync(
            new System.Text.RegularExpressions.Regex("expanded"),
            new() { Timeout = 3000 });
    }

    [Fact]
    public async Task AISDLC_PhaseCards_ShouldDisplay_IconsAndNames()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Each phase should have an icon and name
        var firstPhaseHeader = Page.Locator(".sdlc-phase-header").First;
        await firstPhaseHeader.AssertElementVisibleAsync();

        // Should have phase icon (emoji)
        var phaseIcon = firstPhaseHeader.Locator(".sdlc-phase-icon");
        await phaseIcon.AssertElementVisibleAsync();
        var iconText = await phaseIcon.TextContentAsync();
        iconText.Should().NotBeNullOrWhiteSpace("Phase should have an icon (emoji)");

        // Should have phase name heading
        await Assertions.Expect(firstPhaseHeader.Locator("h3")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task AISDLC_PhaseContent_ShouldDisplay_AIEnhancements()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Expand first phase
        var firstPhaseHeader = Page.Locator(".sdlc-phase-header").First;
        var firstPhaseContent = Page.Locator(".sdlc-phase-content").First;
        await firstPhaseHeader.ClickBlazorElementAsync(waitForUrlChange: false);
        await Assertions.Expect(firstPhaseContent).ToHaveClassAsync(
            new System.Text.RegularExpressions.Regex("expanded"),
            new() { Timeout = 3000 });

        // Assert - Expanded phase should show AI enhancements
        var aiSection = firstPhaseContent.Locator(".sdlc-phase-ai");
        await aiSection.AssertElementVisibleAsync();

        // Should have AI enhancement heading
        var aiHeading = aiSection.Locator("h4");
        await aiHeading.AssertElementVisibleAsync();
        var headingText = await aiHeading.TextContentAsync();
        headingText.Should().Contain("AI", "Should have AI enhancements section heading");
    }

    [Fact]
    public async Task AISDLC_Intro_ShouldDisplay()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Should have intro section
        var intro = Page.Locator(".custom-page-intro");
        await intro.AssertElementVisibleAsync();

        // Should have descriptive text about SDLC
        var introText = await intro.TextContentAsync();
        introText.Should().Contain("Software Development", "Intro should describe SDLC");
    }

    [Fact]
    public async Task AISDLC_ShouldHave_NoConsoleErrors()
    {
        // Arrange
        var consoleErrors = new List<string>();
        Page.Console += (_, msg) =>
        {
            if (msg.Type == "error")
            {
                consoleErrors.Add(msg.Text);
            }
        };

        // Act
        await Page.GotoRelativeAsync(PageUrl);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert - Should have no console errors (filter WebSocket connection errors from Blazor)
        var significantErrors = consoleErrors
            .Where(e => !e.Contains("WebSocket"))
            .Where(e => !e.Contains("ERR_CONNECTION_REFUSED"))
            .ToList();

        significantErrors.Should().BeEmpty("Page should load without JavaScript errors");
    }
}
