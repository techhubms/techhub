using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for Admin pages.
/// Without authentication, admin pages show the login form inline (via AdminLayout's AuthorizeView).
/// The login form is rendered within Blazor, so GotoRelativeAsync works for all admin pages.
/// </summary>
public class AdminPageTests : PlaywrightTestBase
{
    public AdminPageTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    // ── Login Page ────────────────────────────────────────────────────────

    [Fact]
    public async Task AdminLoginPage_ShouldLoad_Successfully()
    {
        // Act
        await Page.GotoRelativeAsync("/admin/login");

        // Assert
        await Assertions.Expect(Page).ToHaveTitleAsync("Admin Sign-in — TechHub");
    }

    [Fact]
    public async Task AdminLoginPage_ShouldDisplay_SignInHeading()
    {
        // Act
        await Page.GotoRelativeAsync("/admin/login");

        // Assert
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "TechHub Admin");
    }

    // ── Auth Gate ──────────────────────────────────────────────────────────
    // Without authentication, admin pages render the login form inline.
    // All tests use GotoRelativeAsync — Blazor initializes normally.

    [Fact]
    public async Task AdminDashboard_WithoutAuth_ShowsLoginForm()
    {
        // Act
        await Page.GotoRelativeAsync("/admin");

        // Assert — should show login form inline instead of page content
        await Assertions.Expect(Page).ToHaveTitleAsync("Admin Sign-in — TechHub");
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "TechHub Admin");
    }

    [Fact]
    public async Task AdminFeeds_WithoutAuth_ShowsLoginForm()
    {
        // Act
        await Page.GotoRelativeAsync("/admin/feeds");

        // Assert
        await Assertions.Expect(Page).ToHaveTitleAsync("Admin Sign-in — TechHub");
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "TechHub Admin");
    }

    [Fact]
    public async Task AdminStatistics_WithoutAuth_ShowsLoginForm()
    {
        // Act
        await Page.GotoRelativeAsync("/admin/statistics");

        // Assert
        await Assertions.Expect(Page).ToHaveTitleAsync("Admin Sign-in — TechHub");
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "TechHub Admin");
    }

    [Fact]
    public async Task AdminProcessedUrls_WithoutAuth_ShowsLoginForm()
    {
        // Act
        await Page.GotoRelativeAsync("/admin/processed-urls");

        // Assert
        await Assertions.Expect(Page).ToHaveTitleAsync("Admin Sign-in — TechHub");
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "TechHub Admin");
    }

    [Fact]
    public async Task AdminDashboard_WithoutAuth_StaysOnRequestedUrl()
    {
        // Act — navigate to /admin without auth
        await Page.GotoRelativeAsync("/admin");

        // Assert — should stay on /admin, not redirect away
        Page.Url.Should().Contain("/admin");
        Page.Url.Should().NotContain("login.microsoftonline.com",
            "admin pages should render login inline instead of redirecting externally");
    }
}
