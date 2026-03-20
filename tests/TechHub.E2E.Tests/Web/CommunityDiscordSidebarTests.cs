using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for the Discord custom page at /github-copilot/discord.
/// Discord was moved from the Community sidebar to its own dedicated page.
/// </summary>
public class CommunityDiscordSidebarTests : PlaywrightTestBase
{
    public CommunityDiscordSidebarTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task DiscordPage_ShouldDisplay_DiscordContent()
    {
        // Act
        await Page.GotoRelativeAsync("/github-copilot/discord");

        // Assert - Discord page should show heading and invite link
        await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "GitHub Copilot Community Discord" })).ToBeVisibleAsync();
    }

    [Fact]
    public async Task DiscordPage_ShouldHave_InviteLink()
    {
        // Act
        await Page.GotoRelativeAsync("/github-copilot/discord");

        // Assert - Discord invite link should be present
        var discordLink = Page.GetByRole(AriaRole.Link).Filter(new() { HasText = "Join our Discord server" });
        await Assertions.Expect(discordLink.First).ToBeVisibleAsync();

        var href = await discordLink.First.GetAttributeAsync("href");
        href.Should().Be("https://discord.gg/cURHV9TvFS");
    }

    [Fact]
    public async Task CommunityPage_Sidebar_ShouldNot_Display_DiscordSection()
    {
        // Act - navigate to the community page (Discord was removed from sidebar)
        await Page.GotoRelativeAsync("/github-copilot/community");

        // Assert - Discord section should NOT appear in the sidebar
        var sidebar = Page.Locator("aside.sidebar");
        await Assertions.Expect(sidebar.GetByRole(AriaRole.Heading, new() { Name = "Discord" })).Not.ToBeVisibleAsync();
    }
}
