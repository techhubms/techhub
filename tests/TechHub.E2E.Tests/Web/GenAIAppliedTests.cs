using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for GenAI Applied custom page.
/// Verifies page-specific content and features.
/// 
/// Common component tests (TOC, mermaid) are in:
/// - SidebarTocTests.cs: Table of contents behavior
/// - MermaidTests.cs: Diagram rendering
/// </summary>
public class GenAIAppliedTests : PlaywrightTestBase
{
    public GenAIAppliedTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    private const string PageUrl = "/ai/genai-applied";

    [Fact]
    public async Task GenAIApplied_ShouldLoad_Successfully()
    {
        // Act
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Check page title attribute contains expected text
        await Assertions.Expect(Page).ToHaveTitleAsync(new Regex("GenAI Applied"));
    }

    [Fact]
    public async Task GenAIApplied_ShouldDisplay_KeySections()
    {
        // Act
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Should have major section headings
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "GenAI Applied", level: 1);

        // Page should have some content paragraphs
        var paragraphCount = await Page.GetElementCountBySelectorAsync("p");
        paragraphCount.Should().BeGreaterThan(0, $"Expected at least one paragraph, but found {paragraphCount}");

        // Page should have main content heading (excluding banner heading)
        var mainHeading = Page.Locator(".page-h1");
        await mainHeading.AssertElementVisibleAsync();
    }
}
