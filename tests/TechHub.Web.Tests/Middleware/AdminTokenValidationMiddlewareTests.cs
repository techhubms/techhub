using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Moq;
using TechHub.Web.Middleware;

namespace TechHub.Web.Tests.Middleware;

/// <summary>
/// Tests for AdminTokenValidationMiddleware — re-authenticates admin users whose MSAL token cache is empty
/// by issuing an OIDC challenge before Blazor starts rendering.
/// </summary>
public class AdminTokenValidationMiddlewareTests
{
    private const string Scope = "api://test-client-id/Admin.Access";

    [Theory]
    [InlineData("/")]
    [InlineData("/ai")]
    [InlineData("/ai/videos/my-article")]
    [InlineData("/about")]
    [InlineData("/_blazor/negotiate")]
    public async Task InvokeAsync_NonAdminPath_PassesThroughWithoutAcquiringToken(string path)
    {
        // Arrange
        var harness = CreateHarness(path: path);

        // Act
        await harness.Middleware.InvokeAsync(harness.Context);

        // Assert
        harness.NextCalled().Should().BeTrue();
        harness.VerifyTokenAcquisitionNeverCalled();
        harness.VerifyNoChallenge();
    }

    [Theory]
    [InlineData("/admin/login")]
    [InlineData("/admin/login/callback")]
    [InlineData("/ADMIN/LOGIN")]
    [InlineData("/admin/logout")]
    [InlineData("/Admin/Logout")]
    public async Task InvokeAsync_LoginAndLogoutPaths_AreSkipped(string path)
    {
        // Arrange
        var harness = CreateHarness(path: path);

        // Act
        await harness.Middleware.InvokeAsync(harness.Context);

        // Assert
        harness.NextCalled().Should().BeTrue();
        harness.VerifyTokenAcquisitionNeverCalled();
        harness.VerifyNoChallenge();
    }

    [Theory]
    [InlineData("/admin")]
    [InlineData("/admin/reviews")]
    [InlineData("/ADMIN/Feeds")]
    public async Task InvokeAsync_AnonymousUserOnAdminPath_PassesThroughWithoutAcquiringToken(string path)
    {
        // Arrange
        var harness = CreateHarness(path: path, authenticated: false);

        // Act
        await harness.Middleware.InvokeAsync(harness.Context);

        // Assert
        harness.NextCalled().Should().BeTrue("anonymous users are handled by [Authorize] on the page components");
        harness.VerifyTokenAcquisitionNeverCalled();
        harness.VerifyNoChallenge();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task InvokeAsync_NoScopesConfigured_PassesThroughWithoutAcquiringToken(string? scopes)
    {
        // Arrange
        var harness = CreateHarness(scopes: scopes);

        // Act
        await harness.Middleware.InvokeAsync(harness.Context);

        // Assert
        harness.NextCalled().Should().BeTrue("Azure AD is not configured (local development)");
        harness.VerifyTokenAcquisitionNeverCalled();
        harness.VerifyNoChallenge();
    }

    [Fact]
    public async Task InvokeAsync_TokenAcquisitionNotRegistered_PassesThrough()
    {
        // Arrange
        var harness = CreateHarness(registerTokenAcquisition: false);

        // Act
        await harness.Middleware.InvokeAsync(harness.Context);

        // Assert
        harness.NextCalled().Should().BeTrue("without ITokenAcquisition there is nothing to validate");
        harness.VerifyNoChallenge();
    }

    [Fact]
    public async Task InvokeAsync_ValidToken_CallsNextWithoutChallenge()
    {
        // Arrange
        var harness = CreateHarness();
        harness.SetupTokenSuccess();

        // Act
        await harness.Middleware.InvokeAsync(harness.Context);

        // Assert
        harness.NextCalled().Should().BeTrue();
        harness.VerifyNoChallenge();
    }

    [Fact]
    public async Task InvokeAsync_ValidToken_RequestsConfiguredScopes()
    {
        // Arrange
        IEnumerable<string>? requestedScopes = null;
        var harness = CreateHarness(scopes: "api://a/One api://a/Two");
        harness.TokenAcquisition!
            .Setup(t => t.GetAccessTokenForUserAsync(
                It.IsAny<IEnumerable<string>>(),
                It.IsAny<string?>(), It.IsAny<string?>(),
                It.IsAny<ClaimsPrincipal?>(), It.IsAny<TokenAcquisitionOptions?>()))
            .Callback<IEnumerable<string>, string?, string?, ClaimsPrincipal?, TokenAcquisitionOptions?>(
                (s, _, _, _, _) => requestedScopes = s.ToArray())
            .ReturnsAsync("token");

        // Act
        await harness.Middleware.InvokeAsync(harness.Context);

        // Assert
        requestedScopes.Should().Equal("api://a/One", "api://a/Two");
    }

    [Fact]
    public async Task InvokeAsync_MsalUiRequired_ChallengesWithOidcAndDoesNotCallNext()
    {
        // Arrange
        var harness = CreateHarness();
        harness.SetupTokenThrows(new MsalUiRequiredException("no_tokens_found", "Token cache is empty"));

        // Act
        await harness.Middleware.InvokeAsync(harness.Context);

        // Assert
        harness.NextCalled().Should().BeFalse();
        harness.VerifyChallenged(OpenIdConnectDefaults.AuthenticationScheme);
    }

    [Fact]
    public async Task InvokeAsync_MicrosoftIdentityWebChallengeUser_ChallengesWithOidcAndDoesNotCallNext()
    {
        // Arrange
        var harness = CreateHarness();
        harness.SetupTokenThrows(new MicrosoftIdentityWebChallengeUserException(
            new MsalUiRequiredException("invalid_grant", "Consent required"),
            [Scope]));

        // Act
        await harness.Middleware.InvokeAsync(harness.Context);

        // Assert
        harness.NextCalled().Should().BeFalse();
        harness.VerifyChallenged(OpenIdConnectDefaults.AuthenticationScheme);
    }

    [Theory]
    [InlineData("/admin/reviews", null, "/admin/reviews")]
    [InlineData("/admin/content-items", "?page=3&search=copilot", "/admin/content-items?page=3&search=copilot")]
    [InlineData("/admin", "?x=1", "/admin?x=1")]
    public async Task InvokeAsync_Challenge_PreservesPathAndQueryAsRedirectUri(string path, string? query, string expectedRedirectUri)
    {
        // Arrange
        var harness = CreateHarness(path: path, query: query);
        harness.SetupTokenThrows(new MsalUiRequiredException("no_tokens_found", "Token cache is empty"));

        // Act
        await harness.Middleware.InvokeAsync(harness.Context);

        // Assert
        harness.ChallengeProperties.Should().NotBeNull();
        harness.ChallengeProperties!.RedirectUri.Should().Be(expectedRedirectUri);
    }

    [Fact]
    public async Task InvokeAsync_UnexpectedTokenException_Propagates()
    {
        // Arrange
        var harness = CreateHarness();
        harness.SetupTokenThrows(new InvalidOperationException("boom"));

        // Act
        var act = () => harness.Middleware.InvokeAsync(harness.Context);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        harness.NextCalled().Should().BeFalse();
        harness.VerifyNoChallenge();
    }

    [Fact]
    public void Constructor_NullNext_Throws()
    {
        // Act
        var act = () => new AdminTokenValidationMiddleware(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    private static Harness CreateHarness(
        string path = "/admin/reviews",
        string? query = null,
        bool authenticated = true,
        string? scopes = Scope,
        bool registerTokenAcquisition = true)
    {
        var configValues = new Dictionary<string, string?>();
        if (scopes != null)
        {
            configValues["AzureAd:Scopes"] = scopes;
        }

        var configuration = new ConfigurationBuilder().AddInMemoryCollection(configValues).Build();
        var authService = new Mock<IAuthenticationService>();
        AuthenticationProperties? challengeProperties = null;
        string? challengeScheme = null;
        var challengeCount = 0;
        authService
            .Setup(a => a.ChallengeAsync(It.IsAny<HttpContext>(), It.IsAny<string?>(), It.IsAny<AuthenticationProperties?>()))
            .Callback<HttpContext, string?, AuthenticationProperties?>((_, scheme, props) =>
            {
                challengeScheme = scheme;
                challengeProperties = props;
                challengeCount++;
            })
            .Returns(Task.CompletedTask);

        var tokenAcquisition = registerTokenAcquisition ? new Mock<ITokenAcquisition>() : null;

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddSingleton(authService.Object);
        if (tokenAcquisition != null)
        {
            services.AddSingleton(tokenAcquisition.Object);
        }

        var wasCalled = false;
        RequestDelegate next = _ =>
        {
            wasCalled = true;
            return Task.CompletedTask;
        };

        var context = new DefaultHttpContext
        {
            RequestServices = services.BuildServiceProvider()
        };
        context.Request.Path = path;
        if (query != null)
        {
            context.Request.QueryString = new QueryString(query);
        }

        if (authenticated)
        {
            context.User = new ClaimsPrincipal(new ClaimsIdentity("TestAuth"));
        }

        return new Harness(
            new AdminTokenValidationMiddleware(next),
            context,
            tokenAcquisition,
            () => wasCalled,
            () => challengeProperties,
            () => challengeScheme,
            () => challengeCount);
    }

    private sealed class Harness(
        AdminTokenValidationMiddleware middleware,
        HttpContext context,
        Mock<ITokenAcquisition>? tokenAcquisition,
        Func<bool> nextCalled,
        Func<AuthenticationProperties?> challengeProperties,
        Func<string?> challengeScheme,
        Func<int> challengeCount)
    {
        public AdminTokenValidationMiddleware Middleware { get; } = middleware;

        public HttpContext Context { get; } = context;

        public Mock<ITokenAcquisition>? TokenAcquisition { get; } = tokenAcquisition;

        public Func<bool> NextCalled { get; } = nextCalled;

        public AuthenticationProperties? ChallengeProperties => challengeProperties();

        public void SetupTokenSuccess()
        {
            TokenAcquisition!
                .Setup(t => t.GetAccessTokenForUserAsync(
                    It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string?>(), It.IsAny<string?>(),
                    It.IsAny<ClaimsPrincipal?>(), It.IsAny<TokenAcquisitionOptions?>()))
                .ReturnsAsync("token");
        }

        public void SetupTokenThrows(Exception exception)
        {
            TokenAcquisition!
                .Setup(t => t.GetAccessTokenForUserAsync(
                    It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string?>(), It.IsAny<string?>(),
                    It.IsAny<ClaimsPrincipal?>(), It.IsAny<TokenAcquisitionOptions?>()))
                .ThrowsAsync(exception);
        }

        public void VerifyTokenAcquisitionNeverCalled()
        {
            TokenAcquisition!.Verify(
                t => t.GetAccessTokenForUserAsync(
                    It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string?>(), It.IsAny<string?>(),
                    It.IsAny<ClaimsPrincipal?>(), It.IsAny<TokenAcquisitionOptions?>()),
                Times.Never);
        }

        public void VerifyNoChallenge() => challengeCount().Should().Be(0);

        public void VerifyChallenged(string expectedScheme)
        {
            challengeCount().Should().Be(1);
            challengeScheme().Should().Be(expectedScheme);
        }
    }
}
