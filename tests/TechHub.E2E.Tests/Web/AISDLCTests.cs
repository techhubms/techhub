using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for AI-Powered SDLC custom page.
/// All content is rendered from API data (no collapsible sections).
/// Common component tests (TOC, scroll spy) are in SidebarTocTests.cs.
/// </summary>
public class AISDLCTests : PlaywrightTestBase
{
    public AISDLCTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    private const string PageUrl = "/ai/sdlc";

    [Fact]
    public async Task AISDLC_ShouldLoad_Successfully()
    {
        // Act
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Check page title attribute contains expected text
        await Assertions.Expect(Page).ToHaveTitleAsync(new System.Text.RegularExpressions.Regex("AI SDLC"));
    }

    [Fact]
    public async Task AISDLC_ShouldRender_WithToc()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Sidebar TOC should exist with headings
        var toc = Page.Locator(".sidebar-toc");
        await Assertions.Expect(toc).ToBeVisibleAsync();

        // TOC should have links for major sections
        var tocLinks = toc.Locator("a");
        var linkCount = await tocLinks.CountAsync();
        linkCount.Should().BeGreaterThanOrEqualTo(5, "TOC should have links for phases, methodologies, benefits, challenges, preconditions, metrics");
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
        phaseCount.Should().BeGreaterThanOrEqualTo(5, "Expected at least 5 SDLC phases");
    }

    [Fact]
    public async Task AISDLC_PhaseCards_ShouldHave_ColoredBorders()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Phase cards should have phase-specific CSS classes
        var ideationCard = Page.Locator(".sdlc-phase-ideation").First;
        await ideationCard.AssertElementVisibleAsync();

        var className = await ideationCard.GetAttributeAsync("class");
        className.Should().Contain("sdlc-phase-ideation", "First phase should be ideation");
    }

    [Fact]
    public async Task AISDLC_PhaseCards_ShouldDisplay_ContentDirectly()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Phase body content should be visible without clicking (no collapsible)
        var phaseBody = Page.Locator(".sdlc-phase-body").First;
        await phaseBody.AssertElementVisibleAsync();

        // Should have AI enhancements section visible
        var aiSection = phaseBody.Locator(".sdlc-phase-ai");
        await aiSection.AssertElementVisibleAsync();
    }

    [Fact]
    public async Task AISDLC_PhaseCards_ShouldDisplay_IconsAndNames()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - First phase should have icon and name
        var firstPhaseHeader = Page.Locator(".sdlc-phase-header").First;
        await firstPhaseHeader.AssertElementVisibleAsync();

        var phaseIcon = firstPhaseHeader.Locator(".sdlc-phase-icon");
        await phaseIcon.AssertElementVisibleAsync();
        var iconText = await phaseIcon.TextContentAsync();
        iconText.Should().NotBeNullOrWhiteSpace("Phase should have an icon (emoji)");

        await Assertions.Expect(firstPhaseHeader.Locator("h3")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task AISDLC_ShouldDisplay_BenefitsSection()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Benefits section with items
        var benefitItems = Page.Locator(".sdlc-benefit-item");
        var count = await benefitItems.CountAsync();
        count.Should().BeGreaterThan(0, "Expected benefit items");
    }

    [Fact]
    public async Task AISDLC_ShouldDisplay_ChallengesSection()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Challenges section with items
        var challengeItems = Page.Locator(".sdlc-challenge-item");
        var count = await challengeItems.CountAsync();
        count.Should().BeGreaterThan(0, "Expected challenge items");
    }

    [Fact]
    public async Task AISDLC_ShouldDisplay_PreconditionsSection()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Preconditions rendered as flat items (no card styling)
        var preconditionItems = Page.Locator(".sdlc-precondition-item");
        var count = await preconditionItems.CountAsync();
        count.Should().BeGreaterThan(0, "Expected precondition items");

        // Each item should have icon next to title
        var firstHeader = preconditionItems.First.Locator(".sdlc-precondition-header");
        await firstHeader.AssertElementVisibleAsync();
    }

    [Fact]
    public async Task AISDLC_ShouldDisplay_MethodologiesSection()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Should have methodology diagrams (mermaid)
        var mermaidDiagrams = Page.Locator(".sdlc-methodology-diagram");
        var count = await mermaidDiagrams.CountAsync();
        count.Should().BeGreaterThan(0, "Expected methodology diagrams");
    }

    [Fact]
    public async Task AISDLC_Intro_ShouldDisplay()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Should have intro section with text from API
        var intro = Page.Locator(".custom-page-intro");
        await intro.AssertElementVisibleAsync();

        var introText = await intro.TextContentAsync();
        introText.Should().Contain("Software Development", "Intro should describe SDLC");
    }

    [Fact]
    public async Task AISDLC_AllHeadings_ShouldComeFromAPI()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Key section headings should be visible (driven from API data)
        var pageContent = await Page.ContentAsync();
        pageContent.Should().Contain("SDLC Phases");
        pageContent.Should().Contain("Benefits of a Structured SDLC");
        pageContent.Should().Contain("Common Challenges");
        pageContent.Should().Contain("Preconditions for AI-Augmented Development");
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

        // Assert
        var significantErrors = consoleErrors
            .Where(e => !e.Contains("WebSocket"))
            .Where(e => !e.Contains("ERR_CONNECTION_REFUSED"))
            .ToList();

        significantErrors.Should().BeEmpty("Page should load without JavaScript errors");
    }
}
