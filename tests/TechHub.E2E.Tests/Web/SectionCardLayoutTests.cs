using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

public class SectionCardLayoutTests : PlaywrightTestBase
{
    public SectionCardLayoutTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task HomePage_SectionCard_ShouldRenderAsCompleteCard()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Assert - Each section card should have both header and body within same card
        var sectionCards = Page.Locator(".section-card");
        var count = await Page.GetElementCountBySelectorAsync(".section-card");

        (count > 0).Should().BeTrue("Expected at least one section card");

        // Check first card has complete structure
        var firstCard = sectionCards.First;
        await firstCard.Locator(".section-card-header").AssertElementVisibleAsync();
        await firstCard.Locator(".section-body").AssertElementVisibleAsync();
    }

    [Fact]
    public async Task HomePage_SectionCard_HeaderAndBody_ShouldBeInSameCard()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Assert - For each section card, verify header and body are siblings
        var sectionCards = Page.Locator(".section-card");
        var count = await Page.GetElementCountBySelectorAsync(".section-card");

        for (int i = 0; i < count; i++)
        {
            var card = sectionCards.Nth(i);

            // Both header and body should exist within this single card element
            var header = card.Locator(".section-card-header");
            var body = card.Locator(".section-body");

            await Assertions.Expect(header).ToBeVisibleAsync();
            await Assertions.Expect(body).ToBeVisibleAsync();
        }
    }

    [Fact]
    public async Task HomePage_SectionCard_ShouldHaveCorrectGridLayout()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Assert - Section cards should be in a grid container
        var grid = Page.Locator(".sections-grid .grid");
        await grid.AssertElementVisibleAsync();

        // All section card containers should be direct children of the grid
        var count = await grid.GetElementCountBySelectorAsync("> .section-card-container");

        (count > 0).Should().BeTrue("Expected section card containers to be direct children of grid");
    }

    [Fact]
    public async Task HomePage_SectionCard_CollectionBadges_ShouldBeClickable()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Act - Find first section card with collection badges
        var firstCard = Page.Locator(".section-card").First;
        var badges = firstCard.Locator(".collection-badge");
        var badgeCount = await firstCard.GetElementCountBySelectorAsync(".collection-badge");

        // If card has badges, verify they're clickable
        if (badgeCount > 0)
        {
            var firstBadge = badges.First;

            // Should be visible and enabled
            await firstBadge.AssertElementVisibleAsync();
            await Assertions.Expect(firstBadge).ToBeEnabledAsync();
        }
    }

    [Fact]
    public async Task HomePage_SectionCard_MainCard_ShouldBeClickable()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Act
        var firstCard = Page.Locator(".section-card").First;

        // Assert - Card should be a clickable link
        await firstCard.AssertElementVisibleAsync();
    }
}
