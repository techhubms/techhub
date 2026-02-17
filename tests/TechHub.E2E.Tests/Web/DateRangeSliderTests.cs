using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for the date range slider component.
/// Tests that the slider renders, defaults to correct state,
/// presets work, and URL state is maintained.
/// Based on spec: /workspaces/techhub/specs/001b-date-range-slider/spec.md
/// </summary>
public class DateRangeSliderTests : PlaywrightTestBase
{
    public DateRangeSliderTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task DateRangeSlider_RendersOnSectionPage()
    {
        // Arrange & Act - Navigate to a section page
        await Page.GotoRelativeAsync("/github-copilot");
        await WaitForSliderReadyAsync();

        // Assert - Date range slider should be visible
        var slider = Page.Locator(".date-range-slider");
        await Assertions.Expect(slider).ToBeVisibleAsync();

        // Should have the "Date Range" heading
        var heading = Page.Locator("nav.sidebar-section h2:has-text('Date Range')");
        await Assertions.Expect(heading).ToBeVisibleAsync();
    }

    [Fact]
    public async Task DateRangeSlider_HasPresetButtons()
    {
        // Arrange & Act
        await Page.GotoRelativeAsync("/github-copilot");
        await WaitForSliderReadyAsync();

        // Assert - Five preset buttons
        var presets = Page.Locator(".date-preset-button");
        await Assertions.Expect(presets).ToHaveCountAsync(5);

        // Verify preset labels
        await Assertions.Expect(presets.Nth(0)).ToHaveTextAsync("7d");
        await Assertions.Expect(presets.Nth(1)).ToHaveTextAsync("30d");
        await Assertions.Expect(presets.Nth(2)).ToHaveTextAsync("90d");
        await Assertions.Expect(presets.Nth(3)).ToHaveTextAsync("1y");
        await Assertions.Expect(presets.Nth(4)).ToHaveTextAsync("All");
    }

    [Fact]
    public async Task DateRangeSlider_DefaultsTo90DaysActive()
    {
        // Arrange & Act
        await Page.GotoRelativeAsync("/github-copilot");
        await WaitForSliderReadyAsync();

        // Assert - 90d preset should have active class by default
        var ninetyDayButton = Page.Locator(".date-preset-button:has-text('90d')");
        await Assertions.Expect(ninetyDayButton).ToHaveClassAsync(new System.Text.RegularExpressions.Regex("active"));
    }

    [Fact]
    public async Task DateRangeSlider_HasTwoSliderInputs()
    {
        // Arrange & Act
        await Page.GotoRelativeAsync("/github-copilot");
        await WaitForSliderReadyAsync();

        // Assert - Two range inputs for from/to
        var fromSlider = Page.Locator(".slider-from");
        var toSlider = Page.Locator(".slider-to");
        await Assertions.Expect(fromSlider).ToBeVisibleAsync();
        await Assertions.Expect(toSlider).ToBeVisibleAsync();

        // Verify ARIA labels for accessibility
        await Assertions.Expect(fromSlider).ToHaveAttributeAsync("aria-label", "Start date");
        await Assertions.Expect(toSlider).ToHaveAttributeAsync("aria-label", "End date");
    }

    [Fact]
    public async Task DateRangeSlider_PresetClick_ChangesActiveState()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot");
        await WaitForSliderReadyAsync();

        // Act - Click the "30d" preset (don't wait for URL change, wait for class change instead)
        var thirtyDayButton = Page.Locator(".date-preset-button:has-text('30d')");
        await thirtyDayButton.ClickBlazorElementAsync(waitForUrlChange: false);

        // Assert - 30d should become active, 90d should not be active
        await Assertions.Expect(thirtyDayButton).ToHaveClassAsync(
            new System.Text.RegularExpressions.Regex("active"));

        var ninetyDayButton = Page.Locator(".date-preset-button:has-text('90d')");
        await Assertions.Expect(ninetyDayButton).Not.ToHaveClassAsync(
            new System.Text.RegularExpressions.Regex("active"));
    }

    [Fact]
    public async Task DateRangeSlider_PresetClick_UpdatesUrl()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot");
        await WaitForSliderReadyAsync();

        // Act - Click the "7d" preset (don't wait for URL in ClickBlazorElementAsync,
        // use explicit WaitForURLAsync instead for more control)
        var sevenDayButton = Page.Locator(".date-preset-button:has-text('7d')");
        await sevenDayButton.ClickBlazorElementAsync(waitForUrlChange: false);

        // Assert - URL should contain from and to params
        // Use WaitForFunctionAsync instead of WaitForURLAsync because Blazor Server
        // updates URLs via history.replaceState (pushState), not HTTP navigation.
        await Page.WaitForConditionAsync(
            "() => window.location.href.includes('from=') && window.location.href.includes('to=')");

        var currentUrl = Page.Url;
        currentUrl.Should().Contain("from=");
        currentUrl.Should().Contain("to=");
    }

    [Fact]
    public async Task DateRangeSlider_HasDateRangeDisplay()
    {
        // Arrange & Act
        await Page.GotoRelativeAsync("/github-copilot");
        await WaitForSliderReadyAsync();

        // Assert - Date range display is present with aria-live
        var display = Page.Locator(".date-range-display");
        await Assertions.Expect(display).ToBeVisibleAsync();
        await Assertions.Expect(display).ToHaveAttributeAsync("aria-live", "polite");

        // Should contain date text (e.g., "Feb 10, 2026 – May 11, 2026")
        var label = Page.Locator(".date-range-label");
        await Assertions.Expect(label).ToContainTextAsync("–");
    }

    [Fact]
    public async Task DateRangeSlider_HasAccessibleNavLandmark()
    {
        // Arrange & Act
        await Page.GotoRelativeAsync("/github-copilot");
        await WaitForSliderReadyAsync();

        // Assert - nav element with aria-label
        var nav = Page.Locator("nav[aria-label='Filter by date range']");
        await Assertions.Expect(nav).ToBeVisibleAsync();
    }

    [Fact]
    public async Task DateRangeSlider_UrlFromTo_RestoresState()
    {
        // Arrange - Navigate with from/to in URL
        var toDate = DateOnly.FromDateTime(DateTime.Today);
        var fromDate = toDate.AddDays(-7);

        await Page.GotoRelativeAsync(
            $"/github-copilot?from={fromDate:yyyy-MM-dd}&to={toDate:yyyy-MM-dd}");
        await WaitForSliderReadyAsync();

        // Assert - 7d preset should be active (matching the URL dates)
        var sevenDayButton = Page.Locator(".date-preset-button:has-text('7d')");
        await Assertions.Expect(sevenDayButton).ToHaveClassAsync(
            new System.Text.RegularExpressions.Regex("active"));
    }

    [Fact]
    public async Task DateRangeSlider_RendersOnCollectionPage()
    {
        // Arrange & Act - Navigate to a collection page
        await Page.GotoRelativeAsync("/github-copilot/blogs");
        await WaitForSliderReadyAsync();

        // Assert - Date range slider should be visible on collection pages too
        var slider = Page.Locator(".date-range-slider");
        await Assertions.Expect(slider).ToBeVisibleAsync();
    }

    [Fact]
    public async Task DateRangeSlider_PreservesDateInTagUrl()
    {
        // Arrange - Navigate with date range
        var toDate = DateOnly.FromDateTime(DateTime.Today);
        var fromDate = toDate.AddDays(-30);

        await Page.GotoRelativeAsync(
            $"/github-copilot?from={fromDate:yyyy-MM-dd}&to={toDate:yyyy-MM-dd}");
        await WaitForSliderReadyAsync();

        // Wait for tag cloud to load
        await Assertions.Expect(Page.Locator(".tag-cloud-item").First).ToBeVisibleAsync();

        // Act - Click a tag
        var firstTag = Page.Locator(".tag-cloud-item:not(.disabled)").First;
        await firstTag.ClickBlazorElementAsync();

        // Assert - URL should still contain from/to along with tags
        await Page.WaitForURLAsync(url => url.Contains("tags="));
        var currentUrl = Page.Url;
        currentUrl.Should().Contain("from=", "date range should be preserved when selecting tags");
        currentUrl.Should().Contain("to=", "date range should be preserved when selecting tags");
        currentUrl.Should().Contain("tags=", "tag should be in URL");
    }

    // ============================================================================
    // HELPER METHODS
    // ============================================================================

    /// <summary>
    /// Waits for the date range slider to be ready (visible and interactive).
    /// </summary>
    private async Task WaitForSliderReadyAsync()
    {
        await Assertions.Expect(Page.Locator(".date-range-slider")).ToBeVisibleAsync();

        // Wait for preset buttons to be present (interactive mode ready)
        await Assertions.Expect(Page.Locator(".date-preset-button").First).ToBeVisibleAsync();
    }
}
