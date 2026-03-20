using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

public class AboutPageTests : PlaywrightTestBase
{
    public AboutPageTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task AboutPage_ShouldLoad_Successfully()
    {
        // Act
        await Page.GotoRelativeAsync("/about");

        // Assert - Check page title attribute
        await Assertions.Expect(Page).ToHaveTitleAsync("About Us - Tech Hub");
    }

    [Fact]
    public async Task AboutPage_ShouldDisplay_TeamMembers()
    {
        // Act
        await Page.GotoRelativeAsync("/about");

        // Assert - Check for team member headings
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Reinier van Maanen");
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Rob Bos");
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Fokko Veegens");
    }

    [Fact]
    public async Task AboutPage_ShouldDisplay_TeamMemberPhotos()
    {
        // Act
        await Page.GotoRelativeAsync("/about");

        // Assert - Check for team member photos (exact alt text matching)
        await Page.AssertElementVisibleByAltTextAsync("Reinier van Maanen");
        await Page.AssertElementVisibleByAltTextAsync("Rob Bos");
        await Page.AssertElementVisibleByAltTextAsync("Fokko Veegens");
    }

    [Fact]
    public async Task AboutPage_ShouldDisplay_SocialLinks()
    {
        // Act
        await Page.GotoRelativeAsync("/about");

        // Assert - Verify each team member has correct social links

        // Reinier van Maanen - should have GitHub, LinkedIn, and Blog
        var reinierSection = Page.Locator("text=Reinier van Maanen").Locator("..");
        await reinierSection.AssertElementVisibleByRoleAsync(AriaRole.Link, "📘 GitHub");
        await reinierSection.AssertElementVisibleByRoleAsync(AriaRole.Link, "💼 LinkedIn");
        await reinierSection.AssertElementVisibleByRoleAsync(AriaRole.Link, "✍️ Blog");

        // Rob Bos - should have GitHub, LinkedIn, and Blog
        var robSection = Page.Locator("text=Rob Bos").Locator("..");
        await robSection.AssertElementVisibleByRoleAsync(AriaRole.Link, "📘 GitHub");
        await robSection.AssertElementVisibleByRoleAsync(AriaRole.Link, "💼 LinkedIn");
        await robSection.AssertElementVisibleByRoleAsync(AriaRole.Link, "✍️ Blog");

        // Fokko Veegens - should have GitHub and LinkedIn
        var fokkoSection = Page.Locator("text=Fokko Veegens").Locator("..");
        await fokkoSection.AssertElementVisibleByRoleAsync(AriaRole.Link, "📘 GitHub");
        await fokkoSection.AssertElementVisibleByRoleAsync(AriaRole.Link, "💼 LinkedIn");

        // Note: Fokko's blog link might not be visible or might be a placeholder
        // This test verifies the presence of GitHub and LinkedIn only
    }

    [Fact]
    public async Task AboutPage_Navigation_ShouldWorkFromHeader()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Act - Use ClickBlazorElementAsync for Blazor-enhanced navigation
        await Page.ClickElementByRoleAsync(AriaRole.Link, "About");

        // Assert
        await Page.WaitForBlazorUrlContainsAsync("/about");
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Reinier van Maanen");
    }

    [Fact]
    public async Task AboutPage_ShouldDisplay_DiscordSection()
    {
        // Act
        await Page.GotoRelativeAsync("/about");

        // Assert - Discord section should be visible with heading and invite link
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Discord", level: 2);

        var discordLink = Page.GetByRole(AriaRole.Link).Filter(new() { HasText = "Join our Discord server" });
        await Assertions.Expect(discordLink.First).ToBeVisibleAsync();

        var href = await discordLink.First.GetAttributeAsync("href");
        href.Should().Be("https://discord.gg/cURHV9TvFS");
    }

    [Fact]
    public async Task AboutPage_ShouldDisplay_VersionInfo()
    {
        // Act
        await Page.GotoRelativeAsync("/about");

        // Assert - Version info section should be visible at the bottom with .NET version
        var versionInfo = Page.Locator(".version-info");
        await Assertions.Expect(versionInfo).ToBeVisibleAsync();
        var text = await versionInfo.TextContentAsync();
        text.Should().Contain(".NET");
        text.Should().Contain("Tech Hub");
    }
}
