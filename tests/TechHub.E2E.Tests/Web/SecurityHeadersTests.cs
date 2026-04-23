using FluentAssertions;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests verifying security headers are present on all responses.
/// These headers provide defense-in-depth against XSS, clickjacking,
/// MIME sniffing, and other common web attacks.
/// </summary>
public class SecurityHeadersTests : PlaywrightTestBase
{
    public SecurityHeadersTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task HomePage_ShouldHave_XContentTypeOptions()
    {
        // Act
        var response = await Page.GotoAsync($"{BlazorHelpers.BaseUrl}/");

        // Assert
        response.Should().NotBeNull();
        var header = await response!.HeaderValueAsync("x-content-type-options");
        header.Should().Be("nosniff");
    }

    [Fact]
    public async Task HomePage_ShouldHave_XFrameOptions()
    {
        // Act
        var response = await Page.GotoAsync($"{BlazorHelpers.BaseUrl}/");

        // Assert
        response.Should().NotBeNull();
        var header = await response!.HeaderValueAsync("x-frame-options");
        header.Should().Be("DENY");
    }

    [Fact]
    public async Task HomePage_ShouldHave_ReferrerPolicy()
    {
        // Act
        var response = await Page.GotoAsync($"{BlazorHelpers.BaseUrl}/");

        // Assert
        response.Should().NotBeNull();
        var header = await response!.HeaderValueAsync("referrer-policy");
        header.Should().Be("strict-origin-when-cross-origin");
    }

    [Fact]
    public async Task HomePage_ShouldHave_PermissionsPolicy()
    {
        // Act
        var response = await Page.GotoAsync($"{BlazorHelpers.BaseUrl}/");

        // Assert
        response.Should().NotBeNull();
        var header = await response!.HeaderValueAsync("permissions-policy");
        header.Should().Be("interest-cohort=()");
    }

    [Fact]
    public async Task SectionPage_ShouldHave_SecurityHeaders()
    {
        // Act - verify security headers are present on non-homepage routes too
        var response = await Page.GotoAsync($"{BlazorHelpers.BaseUrl}/github-copilot");

        // Assert
        response.Should().NotBeNull();
        (await response!.HeaderValueAsync("x-content-type-options")).Should().Be("nosniff");
        (await response!.HeaderValueAsync("x-frame-options")).Should().Be("DENY");
        (await response!.HeaderValueAsync("referrer-policy")).Should().Be("strict-origin-when-cross-origin");
    }
}
