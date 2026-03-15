using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for Google Analytics integration.
/// GA4 is only active in non-Development environments when a measurement ID is configured.
/// In E2E tests (Development mode), GA scripts should NOT be present.
/// </summary>
public class GoogleAnalyticsTests : PlaywrightTestBase
{
    public GoogleAnalyticsTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task GoogleAnalytics_InDevelopment_ShouldNotLoad_GAScript()
    {
        // Arrange - Navigate to homepage
        await Page.GotoRelativeAsync("/");

        // Act - Check page source for GA script
        var pageContent = await Page.ContentAsync();

        // Assert - GA script should NOT be present in Development mode
        pageContent.Should().NotContain("googletagmanager.com/gtag/js",
            "Google Analytics script should not load in Development environment");
        pageContent.Should().NotContain("window.dataLayer",
            "Google Analytics dataLayer should not be initialized in Development environment");
    }

    [Fact]
    public async Task GoogleAnalytics_InDevelopment_ShouldNotDefine_GtagFunction()
    {
        // Arrange - Navigate to any page
        await Page.GotoRelativeAsync("/");

        // Act - Check if gtag function exists in the browser
        var gtagExists = await Page.EvaluateAsync<bool>("() => typeof gtag === 'function'");

        // Assert - gtag should not be defined in Development mode
        gtagExists.Should().BeFalse("gtag function should not exist in Development environment");
    }

    [Fact]
    public async Task GoogleAnalytics_ShouldNotCause_ConsoleErrors()
    {
        // Arrange - Collect console messages
        var consoleMessages = new List<IConsoleMessage>();
        Page.Console += (_, msg) => consoleMessages.Add(msg);

        // Act - Navigate to the homepage
        await Page.GotoRelativeAsync("/");

        // Assert - No GA-related console errors
        var gaErrors = consoleMessages
            .Where(m => m.Type == "error")
            .Where(m => m.Text.Contains("gtag", StringComparison.OrdinalIgnoreCase)
                     || m.Text.Contains("google", StringComparison.OrdinalIgnoreCase)
                     || m.Text.Contains("analytics", StringComparison.OrdinalIgnoreCase))
            .ToList();

        gaErrors.Should().BeEmpty("No Google Analytics related console errors should occur");
    }
}
