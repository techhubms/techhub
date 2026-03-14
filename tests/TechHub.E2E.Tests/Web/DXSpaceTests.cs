using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for Developer Experience Space custom page.
/// All content is rendered from API data (no collapsible sections).
/// Common component tests (TOC, scroll spy) are in SidebarTocTests.cs.
/// </summary>
public class DXSpaceTests : PlaywrightTestBase
{
    public DXSpaceTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    private const string PageUrl = "/devops/dx-space";

    [Fact]
    public async Task DXSpace_ShouldLoad_Successfully()
    {
        // Act
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Check page title attribute contains expected text
        await Assertions.Expect(Page).ToHaveTitleAsync(new Regex("Developer Experience Space"));
    }

    [Fact]
    public async Task DXSpace_ShouldDisplay_FrameworkSections()
    {
        // Act
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Check for section titles from the structured JSON data
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Developer Experience Space", level: 1);

        // All sections should be visible (no longer collapsed)
        var pageContent = await Page.ContentAsync();
        pageContent.Should().Contain("DORA Metrics");
        pageContent.Should().Contain("SPACE Framework");
        pageContent.Should().Contain("Developer Experience");

        var mainHeading = Page.Locator(".page-h1");
        await mainHeading.AssertElementVisibleAsync();
    }

    [Fact]
    public async Task DXSpace_Intro_ShouldDisplay_QuoteAndContent()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Should have intro section with quote
        var intro = Page.Locator(".dx-intro");
        await intro.AssertElementVisibleAsync();

        var quote = intro.Locator(".dx-quote");
        await quote.AssertElementVisibleAsync();
        var quoteText = await quote.TextContentAsync();
        quoteText.Should().Contain("Nicole Forsgren", "Expected quote attribution to Nicole Forsgren");
    }

    [Fact]
    public async Task DXSpace_DORA_ShouldDisplay_FourKeyMetrics()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Metrics grid should be visible (all content is flat, no expand needed)
        var metricsGrid = Page.Locator(".dx-metrics-grid");
        await metricsGrid.AssertElementVisibleAsync();

        // Should have 4 metric cards
        var metricCards = metricsGrid.Locator(".dx-metric-card");
        var metricCount = await metricCards.CountAsync();
        metricCount.Should().Be(4, "DORA should have exactly 4 key metrics");

        var firstMetric = metricCards.First;
        var metricIcon = firstMetric.Locator(".dx-metric-icon");
        await metricIcon.AssertElementVisibleAsync();
    }

    [Fact]
    public async Task DXSpace_SPACE_ShouldDisplay_FiveDimensions()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - SPACE grid should be visible (no expand needed)
        var spaceGrid = Page.Locator(".dx-space-grid");
        await spaceGrid.AssertElementVisibleAsync();

        var spaceCards = spaceGrid.Locator(".dx-space-card");
        var spaceCount = await spaceCards.CountAsync();
        spaceCount.Should().Be(5, "SPACE should have exactly 5 dimensions");

        var firstDimension = spaceCards.First;
        var letter = firstDimension.Locator(".dx-space-letter");
        await letter.AssertElementVisibleAsync();
        var letterText = await letter.TextContentAsync();
        letterText.Should().HaveLength(1, "SPACE dimension should have a single letter badge");
    }

    [Fact]
    public async Task DXSpace_DevEx_ShouldDisplay_FlowStatePillars()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - DevEx grid should be visible (no expand needed)
        var pillarsGrid = Page.Locator(".dx-devex-grid");
        await pillarsGrid.AssertElementVisibleAsync();

        var pillarCards = pillarsGrid.Locator(".dx-devex-card");
        var pillarCount = await pillarCards.CountAsync();
        pillarCount.Should().BeGreaterThan(0, "DevEx should have Flow State pillars");
    }

    [Fact]
    public async Task DXSpace_ShouldDisplay_AllSections()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - All h2 sections should be visible on the page
        var h2Headings = Page.Locator("article h2");
        var headingCount = await h2Headings.CountAsync();
        headingCount.Should().BeGreaterThanOrEqualTo(7, "Expected at least 7 sections (DORA, SPACE, DevEx, Relationships, Getting Started, Tools, Best Practices)");
    }

    [Fact]
    public async Task DXSpace_ShouldRender_WithToc()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Sidebar TOC should exist with headings
        var toc = Page.Locator(".sidebar-toc");
        await Assertions.Expect(toc).ToBeVisibleAsync();

        var tocLinks = toc.Locator("a");
        var linkCount = await tocLinks.CountAsync();
        linkCount.Should().BeGreaterThanOrEqualTo(7, "TOC should have links for all framework sections");
    }

    [Fact]
    public async Task DXSpace_ShouldHave_NoConsoleErrors()
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
