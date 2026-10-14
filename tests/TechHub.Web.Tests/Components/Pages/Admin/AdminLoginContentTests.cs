using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Web.Components.Pages.Admin;

namespace TechHub.Web.Tests.Components.Pages.Admin;

/// <summary>
/// Tests for AdminLoginContent — returnUrl handling must never allow redirecting outside /admin or off-site.
/// </summary>
public class AdminLoginContentTests : BunitContext
{
    private const string SignInPrefix = "http://localhost/MicrosoftIdentity/Account/SignIn?redirectUri=";

    public AdminLoginContentTests()
    {
        JSInterop.SetupVoid("markScriptsReady");
    }

    [Fact]
    public void Render_AzureAdConfigured_ShowsSignInButton()
    {
        // Arrange
        RegisterConfiguration(azureAdConfigured: true);

        // Act
        var cut = Render<AdminLoginContent>();

        // Assert
        cut.Find("button.admin-login-btn-link").TextContent.Should().Contain("Sign in with Microsoft");
        cut.FindAll(".admin-alert-error").Should().BeEmpty();
    }

    [Fact]
    public void Render_AzureAdNotConfigured_ShowsConfigurationWarningInsteadOfButton()
    {
        // Arrange
        RegisterConfiguration(azureAdConfigured: false);

        // Act
        var cut = Render<AdminLoginContent>();

        // Assert
        cut.FindAll("button.admin-login-btn-link").Should().BeEmpty();
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Azure AD is not configured");
    }

    [Fact]
    public void Render_ErrorQueryParameter_ShowsSignInFailedAlert()
    {
        // Arrange
        RegisterConfiguration(azureAdConfigured: true);
        NavigateTo("/admin/login?error=access_denied");

        // Act
        var cut = Render<AdminLoginContent>();

        // Assert
        cut.Find(".admin-alert-error[role='alert']").TextContent.Should().Contain("Sign-in failed");
    }

    [Theory]
    [InlineData("/admin/feeds", "/admin/feeds")]
    [InlineData("/admin", "/admin")]
    [InlineData("/ADMIN/Reviews", "/ADMIN/Reviews")]
    public void SignIn_LocalAdminReturnUrl_IsUsedAsRedirectUri(string returnUrl, string expectedRedirectUri)
    {
        // Arrange
        RegisterConfiguration(azureAdConfigured: true);
        NavigateTo("/admin/login?returnUrl=" + Uri.EscapeDataString(returnUrl));

        // Act
        var redirectUri = ClickSignInAndGetRedirectUri();

        // Assert
        redirectUri.Should().Be(expectedRedirectUri);
    }

    [Theory]
    [InlineData("/dashboard")]
    [InlineData("/")]
    [InlineData("/about")]
    [InlineData("https://evil.com/steal")]
    [InlineData("https://evil.com")]
    [InlineData("//evil.com/path")]
    [InlineData("//evil.com/admin")]
    public void SignIn_NonAdminOrExternalReturnUrl_FallsBackToAdmin(string returnUrl)
    {
        // Arrange
        RegisterConfiguration(azureAdConfigured: true);
        NavigateTo("/admin/login?returnUrl=" + Uri.EscapeDataString(returnUrl));

        // Act
        var redirectUri = ClickSignInAndGetRedirectUri();

        // Assert
        redirectUri.Should().Be("/admin");
    }

    [Fact]
    public void SignIn_AbsoluteUrlWithAdminPath_UsesPathOnly()
    {
        // Arrange
        RegisterConfiguration(azureAdConfigured: true);
        NavigateTo("/admin/login?returnUrl=" + Uri.EscapeDataString("https://evil.com/admin/reviews?x=1"));

        // Act
        var redirectUri = ClickSignInAndGetRedirectUri();

        // Assert
        redirectUri.Should().Be("/admin/reviews");
        redirectUri.Should().NotContain("evil.com");
    }

    [Fact]
    public void SignIn_DoubleEncodedReturnUrl_IsDecodedToAdminPath()
    {
        // Arrange
        RegisterConfiguration(azureAdConfigured: true);
        NavigateTo("/admin/login?returnUrl=" + Uri.EscapeDataString(Uri.EscapeDataString("/admin/feeds")));

        // Act
        var redirectUri = ClickSignInAndGetRedirectUri();

        // Assert
        redirectUri.Should().Be("/admin/feeds");
    }

    [Theory]
    [InlineData("/admin/feeds", "/admin/feeds")]
    [InlineData("/admin/reviews", "/admin/reviews")]
    [InlineData("/admin/login", "/admin")]
    [InlineData("/about", "/admin")]
    public void SignIn_NoReturnUrl_UsesCurrentAdminPage(string currentPath, string expectedRedirectUri)
    {
        // Arrange
        RegisterConfiguration(azureAdConfigured: true);
        NavigateTo(currentPath);

        // Act
        var redirectUri = ClickSignInAndGetRedirectUri();

        // Assert
        redirectUri.Should().Be(expectedRedirectUri);
    }

    private void RegisterConfiguration(bool azureAdConfigured)
    {
        var values = new Dictionary<string, string?>();
        if (azureAdConfigured)
        {
            values["AzureAd:ClientId"] = "test-client-id";
        }

        Services.AddSingleton<IConfiguration>(new ConfigurationBuilder().AddInMemoryCollection(values).Build());
    }

    private void NavigateTo(string relativeUri)
    {
        Services.GetRequiredService<NavigationManager>().NavigateTo(relativeUri);
    }

    private string ClickSignInAndGetRedirectUri()
    {
        var navigation = Services.GetRequiredService<NavigationManager>();
        var cut = Render<AdminLoginContent>();

        cut.Find("button.admin-login-btn-link").Click();

        navigation.Uri.Should().StartWith(SignInPrefix);
        return Uri.UnescapeDataString(navigation.Uri[SignInPrefix.Length..]);
    }
}
