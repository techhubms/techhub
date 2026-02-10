using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for Developer Experience Space custom page.
/// Verifies page-specific content and structured framework sections.
/// 
/// Common component tests are in separate test files:
/// - SidebarTocTests.cs: Table of contents behavior
/// </summary>
[Collection("Custom Pages Tests")]
public class DXSpaceTests : IAsyncLifetime
{
    private readonly PlaywrightCollectionFixture _fixture;

    public DXSpaceTests(PlaywrightCollectionFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        _fixture = fixture;
    }

    private const string PageUrl = "/devops/dx-space";
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
        // The page uses expandable sections with titles like "DORA Metrics", "SPACE Framework", etc.
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Developer Experience Space", level: 1);

        // Check for expandable section cards by looking for text content in the page
        var pageContent = await Page.ContentAsync();
        pageContent.Should().Contain("DORA Metrics");
        pageContent.Should().Contain("SPACE Framework");
        pageContent.Should().Contain("Developer Experience");  // Part of the DevEx/DX section title

        // Page should have main content heading (excluding banner heading)
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

        // Should have blockquote with Nicole Forsgren quote
        var quote = intro.Locator(".dx-quote");
        await quote.AssertElementVisibleAsync();
        var quoteText = await quote.TextContentAsync();
        quoteText.Should().Contain("Nicole Forsgren", "Expected quote attribution to Nicole Forsgren");
    }

    [Fact]
    public async Task DXSpace_SectionCards_ShouldBe_Collapsible()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Get first section card header and content
        var sectionHeader = Page.Locator(".dx-card-header").First;
        var sectionContent = Page.Locator(".dx-card-content").First;

        // Assert - Content should be collapsed initially (check for 'expanded' class)
        var contentClasses = await sectionContent.GetAttributeAsync("class");
        var isExpanded = contentClasses?.Contains("expanded") ?? false;

        // If already expanded (first section might be open by default), verify it's visible
        if (isExpanded)
        {
            await Assertions.Expect(sectionContent).ToBeVisibleAsync();
        }
        else
        {
            // Act - Click to expand
            await sectionHeader.ClickBlazorElementAsync(waitForUrlChange: false);

            // Assert - Content should get the 'expanded' class after click
            await Assertions.Expect(sectionContent).ToHaveClassAsync(
                new System.Text.RegularExpressions.Regex("expanded"),
                new() { Timeout = 3000 });
        }
    }

    [Fact]
    public async Task DXSpace_DORA_ShouldDisplay_FourKeyMetrics()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // DORA section is expanded by default, so no need to click
        // Assert - Should have metrics grid
        var metricsGrid = Page.Locator(".dx-metrics-grid");
        await metricsGrid.AssertElementVisibleAsync();

        // Should have 4 metric cards (Deployment Frequency, Lead Time, MTTR, Change Failure Rate)
        var metricCards = metricsGrid.Locator(".dx-metric-card");
        var metricCount = await metricCards.CountAsync();
        metricCount.Should().Be(4, "DORA should have exactly 4 key metrics");

        // Each metric should have an icon
        var firstMetric = metricCards.First;
        var metricIcon = firstMetric.Locator(".dx-metric-icon");
        await metricIcon.AssertElementVisibleAsync();
    }

    [Fact]
    public async Task DXSpace_SPACE_ShouldDisplay_FiveDimensions()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Expand SPACE section
        var spaceHeader = Page.Locator(".dx-card-header").Filter(new() { HasText = "SPACE" }).First;
        var spaceContent = Page.Locator(".dx-card-content").Filter(new() { Has = Page.Locator(".dx-space-grid") });
        await spaceHeader.ClickBlazorElementAsync(waitForUrlChange: false);
        await Assertions.Expect(spaceContent).ToHaveClassAsync(
            new System.Text.RegularExpressions.Regex("expanded"),
            new() { Timeout = 3000 });

        // Assert - Should have SPACE grid
        var spaceGrid = Page.Locator(".dx-space-grid");
        await spaceGrid.AssertElementVisibleAsync();

        // Should have 5 dimension cards (Satisfaction, Performance, Activity, Communication, Efficiency)
        var spaceCards = spaceGrid.Locator(".dx-space-card");
        var spaceCount = await spaceCards.CountAsync();
        spaceCount.Should().Be(5, "SPACE should have exactly 5 dimensions");

        // Each dimension should have a letter badge
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

        // Expand DevEx section
        var devExHeader = Page.Locator(".dx-card-header").Filter(new() { HasText = "Developer Experience" }).First;
        var devExContent = devExHeader.Locator("xpath=following-sibling::div[contains(@class, 'dx-card-content')]").First;
        await devExHeader.ClickBlazorElementAsync(waitForUrlChange: false);
        await Assertions.Expect(devExContent).ToHaveClassAsync(
            new System.Text.RegularExpressions.Regex("expanded"),
            new() { Timeout = 3000 });

        // Assert - Should have DevEx pillars grid
        var pillarsGrid = Page.Locator(".dx-devex-grid");
        var gridExists = await pillarsGrid.CountAsync();
        if (gridExists > 0)
        {
            // Should have Flow State pillars
            var pillarCards = pillarsGrid.Locator(".dx-devex-card");
            var pillarCount = await pillarCards.CountAsync();
            pillarCount.Should().BeGreaterThan(0, "DevEx should have Flow State pillars");
        }
    }

    [Fact]
    public async Task DXSpace_ShouldDisplay_AllThreeSections()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Wait for content to be loaded and rendered
        var sectionCards = Page.Locator(".dx-section-card");
        await Assertions.Expect(sectionCards.First).ToBeVisibleAsync();

        // Assert - Should have all three framework sections
        var sectionCount = await sectionCards.CountAsync();
        sectionCount.Should().BeGreaterThanOrEqualTo(3, "Expected at least 3 framework sections (DORA, SPACE, DevEx)");
    }

    [Fact]
    public async Task DXSpace_SectionCards_ShouldHave_BordersAndRoundedCorners()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Section cards should have proper styling
        var sectionCard = Page.Locator(".dx-section-card").First;
        await sectionCard.AssertElementVisibleAsync();

        // Verify border and border-radius are set
        var borderStyle = await sectionCard.EvaluateAsync<string>("el => window.getComputedStyle(el).border");
        borderStyle.Should().NotBeNullOrWhiteSpace("Section cards should have borders");

        var borderRadius = await sectionCard.EvaluateAsync<string>("el => window.getComputedStyle(el).borderRadius");
        borderRadius.Should().NotBe("0px", "Section cards should have rounded corners");
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
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert - Should have no console errors (filter WebSocket connection errors from Blazor)
        var significantErrors = consoleErrors
            .Where(e => !e.Contains("WebSocket"))
            .Where(e => !e.Contains("ERR_CONNECTION_REFUSED"))
            .ToList();

        significantErrors.Should().BeEmpty("Page should load without JavaScript errors");
    }
}
