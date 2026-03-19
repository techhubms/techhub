using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

public class AuthorsPageTests : PlaywrightTestBase
{
    public AuthorsPageTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task AuthorsPage_ShouldLoad_WithSidebarLayout()
    {
        // Act
        await Page.GotoRelativeAsync("/all/authors");

        // Assert
        await Assertions.Expect(Page).ToHaveTitleAsync("Content by Author - Tech Hub");
        await Assertions.Expect(Page.Locator("main.page-with-sidebar")).ToBeVisibleAsync();
        await Assertions.Expect(Page.Locator("aside.sidebar")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task AuthorsPage_ShouldDisplay_AuthorListInSidebar()
    {
        // Act
        await Page.GotoRelativeAsync("/all/authors");

        // Assert - Sidebar should contain author links
        var authorLinks = Page.Locator(".author-sidebar-link");
        await Assertions.Expect(authorLinks.First).ToBeVisibleAsync();

        var count = await authorLinks.CountAsync();
        count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task AuthorsPage_NoAuthorSelected_ShowsSelectPrompt()
    {
        // Act
        await Page.GotoRelativeAsync("/all/authors");

        // Assert
        await Assertions.Expect(Page.Locator(".authors-select-prompt")).ToBeVisibleAsync();
        await Assertions.Expect(Page.Locator(".authors-select-prompt")).ToContainTextAsync("Select an author");
    }

    [Fact]
    public async Task AuthorsPage_ClickAuthor_ShowsContentGrid()
    {
        // Arrange
        await Page.GotoRelativeAsync("/all/authors");

        // Get the first author link
        var firstAuthor = Page.Locator(".author-sidebar-link").First;
        await Assertions.Expect(firstAuthor).ToBeVisibleAsync();

        // Act - Click the first author
        await firstAuthor.ClickAsync();

        // Assert - Content grid or no-content message should appear (author may have 0 items)
        var contentGrid = Page.Locator(".content-grid");
        var noContent = Page.Locator(".no-content");
        var endOfContent = Page.Locator(".end-of-content");

        // Wait for either content grid, no-content, or end of content to appear
        await Assertions.Expect(contentGrid.Or(noContent).Or(endOfContent).First).ToBeVisibleAsync();

        // Select prompt should no longer be visible
        await Assertions.Expect(Page.Locator(".authors-select-prompt")).Not.ToBeVisibleAsync();
    }

    [Fact]
    public async Task AuthorsPage_ClickAuthor_HighlightsActiveAuthor()
    {
        // Arrange
        await Page.GotoRelativeAsync("/all/authors");
        var firstAuthor = Page.Locator(".author-sidebar-link").First;
        await Assertions.Expect(firstAuthor).ToBeVisibleAsync();

        // Act
        await firstAuthor.ClickAsync();

        // Assert - The clicked author should have active class
        await Assertions.Expect(Page.Locator(".author-sidebar-link.active")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task AuthorsPage_AuthorLinksShowPostCounts()
    {
        // Act
        await Page.GotoRelativeAsync("/all/authors");

        // Assert - Author links should contain post counts in parentheses
        var firstAuthor = Page.Locator(".author-sidebar-link").First;
        await Assertions.Expect(firstAuthor).ToBeVisibleAsync();

        var text = await firstAuthor.TextContentAsync();
        text.Should().NotBeNull();
        text.Should().MatchRegex(@"\(\d+\)");
    }
}
