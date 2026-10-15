using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

public class NewsletterUnsubscribeTests : PlaywrightTestBase
{
    public NewsletterUnsubscribeTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task NewsletterUnsubscribe_ValidQuery_RequiresConfirmationBeforeSubmitting()
    {
        // Act
        await Page.GotoRelativeAsync("/newsletter/unsubscribe?email=scanner-test%40example.com&token=invalid-token");

        // Assert
        var button = Page.GetByRole(AriaRole.Button, new() { Name = "Unsubscribe", Exact = true });
        await Assertions.Expect(button).ToBeVisibleAsync();
        await Assertions.Expect(Page.Locator(".subscribe-alert")).ToHaveCountAsync(0);

        // Act
        await button.ClickAndExpectAsync(async () =>
            await Assertions.Expect(Page.Locator(".subscribe-alert-error"))
                .ToContainTextAsync("Invalid or expired unsubscribe link."));

        // Assert
        await Assertions.Expect(button).ToHaveCountAsync(0);
    }
}
