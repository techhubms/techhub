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
}
