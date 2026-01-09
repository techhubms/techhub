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
            await _page.CloseAsync();
        
        if (_context != null)
            await _context.CloseAsync();
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
        await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Reinier van Maanen" })).ToBeVisibleAsync();
        await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Rob Bos" })).ToBeVisibleAsync();
        await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Fokko Veegens" })).ToBeVisibleAsync();
    }

    [Fact]
    public async Task AboutPage_ShouldDisplay_TeamMemberPhotos()
    {
        // Act
        await Page.GotoRelativeAsync("/about");
        
        // Assert - Check for team member photos
        var reinierPhoto = Page.GetByAltText(new Regex("Reinier", RegexOptions.IgnoreCase));
        var robPhoto = Page.GetByAltText(new Regex("Rob", RegexOptions.IgnoreCase));
        var fokkoPhoto = Page.GetByAltText(new Regex("Fokko", RegexOptions.IgnoreCase));

        await Assertions.Expect(reinierPhoto).ToBeVisibleAsync();
        await Assertions.Expect(robPhoto).ToBeVisibleAsync();
        await Assertions.Expect(fokkoPhoto).ToBeVisibleAsync();
    }

    [Fact]
    public async Task AboutPage_ShouldDisplay_SocialLinks()
    {
        // Act
        await Page.GotoRelativeAsync("/about");
        
        // Assert - Check for social media links (GitHub, LinkedIn, Blog)
        var githubLinks = Page.GetByRole(AriaRole.Link, new() { NameRegex = new Regex("GitHub", RegexOptions.IgnoreCase) });
        var linkedInLinks = Page.GetByRole(AriaRole.Link, new() { NameRegex = new Regex("LinkedIn", RegexOptions.IgnoreCase) });
        
        // Should have at least one GitHub and LinkedIn link for team members
        await Assertions.Expect(githubLinks.First).ToBeVisibleAsync();
        await Assertions.Expect(linkedInLinks.First).ToBeVisibleAsync();
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
        await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Reinier van Maanen" })).ToBeVisibleAsync();
    }
}
