using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for the Discord sidebar section on the GitHub Copilot Community page.
/// </summary>
public class CommunityDiscordSidebarTests : PlaywrightTestBase
{
    public CommunityDiscordSidebarTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task CommunityPage_Sidebar_ShouldDisplay_DiscordSection()
    {
        // Act
        await Page.GotoRelativeAsync("/github-copilot/community");

        // Assert - Discord section should appear in the sidebar
        var sidebar = Page.Locator("aside.sidebar");
        await Assertions.Expect(sidebar.GetByRole(AriaRole.Heading, new() { Name = "Discord" })).ToBeVisibleAsync();
    }

    [Fact]
    public async Task CommunityPage_Sidebar_DiscordSection_ShouldHave_InviteLink()
    {
        // Act
        await Page.GotoRelativeAsync("/github-copilot/community");

        // Assert - Discord invite link should be present in sidebar
        var sidebar = Page.Locator("aside.sidebar");
        var discordLink = sidebar.GetByRole(AriaRole.Link).Filter(new() { HasText = "Join our Discord server" });
        await Assertions.Expect(discordLink.First).ToBeVisibleAsync();

        var href = await discordLink.First.GetAttributeAsync("href");
        href.Should().Be("https://discord.gg/cURHV9TvFS");
    }

    [Fact]
    public async Task OtherSectionPage_Sidebar_ShouldNot_Display_DiscordSection()
    {
        // Act - navigate to a non-github-copilot section
        await Page.GotoRelativeAsync("/ai/community");

        // Assert - Discord section should NOT appear in the sidebar
        var sidebar = Page.Locator("aside.sidebar");
        await Assertions.Expect(sidebar.GetByRole(AriaRole.Heading, new() { Name = "Discord" })).Not.ToBeVisibleAsync();
    }
}
