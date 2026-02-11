using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for GitHub Copilot Levels of Enlightenment custom page.
/// Verifies level progression display and page-specific features.
/// 
/// Common component tests (TOC, keyboard nav) are in:
/// - SidebarTocTests.cs: Table of contents behavior
/// </summary>
public class LevelsOfEnlightenmentTests : PlaywrightTestBase
{
    public LevelsOfEnlightenmentTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    private const string PageUrl = "/github-copilot/levels-of-enlightenment";

    [Fact]
    public async Task LevelsOfEnlightenment_ShouldLoad_Successfully()
    {
        // Act
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Check page title attribute contains expected text
        await Assertions.Expect(Page).ToHaveTitleAsync(new Regex("Levels of Enlightenment"));
    }

    [Fact]
    public async Task LevelsOfEnlightenment_AllNineLevels_ShouldDisplay()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Act - Find all level sections
        var levelSections = Page.Locator(".levels-level");
        var levelCount = await levelSections.CountAsync();

        // Assert - Should have exactly 9 levels
        levelCount.Should().Be(9, "Expected exactly 9 levels of enlightenment");

        // Verify each level has a heading with title
        for (int i = 1; i <= 9; i++)
        {
            var levelHeading = Page.Locator($"h2[id^='level-{i}']");
            await levelHeading.AssertElementVisibleAsync();
        }
    }

    [Fact]
    public async Task LevelsOfEnlightenment_VideoEmbeds_ShouldRender()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Act - Find all video iframes
        var videoIframes = Page.Locator(".levels-video iframe");
        var videoCount = await videoIframes.CountAsync();

        // Assert - Should have 9 video embeds (one per level)
        videoCount.Should().Be(9, "Expected 9 video embeds (one per level)");

        // Verify first video iframe has YouTube source
        var firstVideo = videoIframes.First;
        var src = await firstVideo.GetAttributeAsync("src");
        src.Should().Match(s => s.Contains("youtube.com/embed") || s.Contains("youtube-nocookie.com/embed"), "Expected video iframe to have YouTube embed URL (standard or privacy-enhanced)");
    }

    [Fact]
    public async Task LevelsOfEnlightenment_OverviewSection_ShouldDisplayWithImage()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Overview heading exists
        var overviewHeading = Page.Locator("h2#overview");
        await overviewHeading.AssertElementVisibleAsync();

        // Assert - Overview image exists
        var overviewImage = Page.Locator(".levels-overview-image");
        await overviewImage.AssertElementVisibleAsync();

        // Verify image has alt text
        var altText = await overviewImage.GetAttributeAsync("alt");
        altText.Should().NotBeNullOrEmpty("Expected overview image to have alt text for accessibility");
    }

    [Fact]
    public async Task LevelsOfEnlightenment_PlaylistLink_ShouldBePresent()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Act - Find playlist link
        var playlistLink = Page.Locator(".levels-playlist-link");

        // Assert - Link exists and is visible
        await playlistLink.AssertElementVisibleAsync();

        // Verify link has correct URL
        var href = await playlistLink.GetAttributeAsync("href");
        href.Should().Contain("youtube.com/playlist", "Expected playlist link to point to YouTube playlist");

        // Verify link opens in new tab
        var target = await playlistLink.GetAttributeAsync("target");
        target.Should().Be("_blank", "Expected playlist link to open in new tab");
    }
}
