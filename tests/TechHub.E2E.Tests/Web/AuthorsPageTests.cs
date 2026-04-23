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

        // Act + Assert — retry [click + grid/no-content visible] to cover hydration race
        var contentGrid = Page.Locator(".content-grid");
        var noContent = Page.Locator(".no-content");
        var endOfContent = Page.Locator(".end-of-content");
        await firstAuthor.ClickAndExpectAsync(async () =>
            await Assertions.Expect(contentGrid.Or(noContent).Or(endOfContent).First)
                .ToBeVisibleAsync(new() { Timeout = 2000 }));

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

        // Act + Assert — retry [click + active link visible] to cover hydration race
        var activeLink = Page.Locator(".author-sidebar-link.active");
        await firstAuthor.ClickAndExpectAsync(async () =>
            await Assertions.Expect(activeLink).ToBeVisibleAsync(
                new() { Timeout = 2000 }));
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
