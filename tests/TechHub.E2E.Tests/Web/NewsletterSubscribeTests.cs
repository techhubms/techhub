using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

public class NewsletterSubscribeTests : PlaywrightTestBase
{
    public NewsletterSubscribeTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task NewsletterSubscribePage_ShouldLoad_Successfully()
    {
        // Act
        await Page.GotoRelativeAsync("/newsletter/subscribe");

        // Assert - page title and URL
        await Assertions.Expect(Page).ToHaveTitleAsync(new System.Text.RegularExpressions.Regex("Subscribe"));
        Page.Url.Should().NotContain("/not-found", "newsletter/subscribe must be a valid page");
        Page.Url.Should().NotContain("not-found", "newsletter/subscribe must resolve to its own page");
    }

    [Fact]
    public async Task NewsletterSubscribePage_ShouldDisplay_SubscribeForm()
    {
        // Act
        await Page.GotoRelativeAsync("/newsletter/subscribe");

        // Assert - heading
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Subscribe to TechHub newsletters");

        // Email and display name inputs
        await Assertions.Expect(Page.Locator("input").First).ToBeVisibleAsync();

        // Subscribe button
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Button, "Subscribe");
    }

    [Fact]
    public async Task NewsletterSubscribePage_ShouldDisplay_SectionCheckboxes()
    {
        // Act
        await Page.GotoRelativeAsync("/newsletter/subscribe");

        // Assert - section checkboxes for weekly and daily
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Weekly roundup per section");
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Daily overview per section (every morning at 9:00)");

        // At least one checkbox should be visible (requires sections to load)
        var checkboxCount = await Page.Locator("input[type='checkbox']").CountAsync();
        checkboxCount.Should().BeGreaterThan(0, "subscribe page should show section checkboxes");
    }
}
