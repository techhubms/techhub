using TechHub.E2E.Tests.Helpers;
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Xunit;

namespace TechHub.E2E.Tests.Web;

[Collection("About Page Tests")]
public class AboutPageTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
{
    private const string BaseUrl = "http://localhost:5184";
    private IBrowserContext? _context;
    private IPage Page => _page ?? throw new InvalidOperationException("Page not initialized");
    private IPage? _page;

    public async Task InitializeAsync()
    {
        _context = await fixture.CreateContextAsync();
        _page = await _context.NewPageAsync();
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
    public async Task AboutPage_ShouldLoad_Successfully()
    {
        // Act
        await Page.GotoRelativeAsync("/about");

        // Assert
        await Assertions.Expect(Page).ToHaveTitleAsync(new Regex("About.*Tech Hub", RegexOptions.IgnoreCase));
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
        await Assertions.Expect(reinierSection.GetByRole(AriaRole.Link, new() { Name = "📘 GitHub", Exact = true })).ToBeVisibleAsync();
        await Assertions.Expect(reinierSection.GetByRole(AriaRole.Link, new() { Name = "💼 LinkedIn", Exact = true })).ToBeVisibleAsync();
        await Assertions.Expect(reinierSection.GetByRole(AriaRole.Link, new() { Name = "✍️ Blog", Exact = true })).ToBeVisibleAsync();

        // Rob Bos - should have GitHub, LinkedIn, and Blog
        var robSection = Page.Locator("text=Rob Bos").Locator("..");
        await Assertions.Expect(robSection.GetByRole(AriaRole.Link, new() { Name = "📘 GitHub", Exact = true })).ToBeVisibleAsync();
        await Assertions.Expect(robSection.GetByRole(AriaRole.Link, new() { Name = "💼 LinkedIn", Exact = true })).ToBeVisibleAsync();
        await Assertions.Expect(robSection.GetByRole(AriaRole.Link, new() { Name = "✍️ Blog", Exact = true })).ToBeVisibleAsync();

        // Fokko Veegens - should have GitHub and LinkedIn (Blog link exists but is placeholder)
        var fokkoSection = Page.Locator("text=Fokko Veegens").Locator("..");
        await Assertions.Expect(fokkoSection.GetByRole(AriaRole.Link, new() { Name = "📘 GitHub", Exact = true })).ToBeVisibleAsync();
        await Assertions.Expect(fokkoSection.GetByRole(AriaRole.Link, new() { Name = "💼 LinkedIn", Exact = true })).ToBeVisibleAsync();

        // Verify Fokko's blog link is a placeholder (href="#")
        var fokkoBlogLink = fokkoSection.GetByRole(AriaRole.Link, new() { Name = "✍️ Blog", Exact = true });
        var fokkoBlogHref = await fokkoBlogLink.GetAttributeAsync("href");
        Assert.Contains("#", fokkoBlogHref);
    }

    [Fact]
    public async Task AboutPage_Navigation_ShouldWorkFromHeader()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Act - Use ClickBlazorElementAsync for Blazor-enhanced navigation
        var aboutLink = Page.GetByRole(AriaRole.Link, new() { NameRegex = new Regex("About", RegexOptions.IgnoreCase) });
        await aboutLink.ClickBlazorElementAsync();

        // Assert
        await Assertions.Expect(Page).ToHaveURLAsync(new Regex("/about$", RegexOptions.IgnoreCase));
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Reinier van Maanen");
    }
}
