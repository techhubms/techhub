using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for the real "AdminOnly" authorization policy.
/// Azure AD is configured (ClientId set) and the JWT bearer scheme is replaced by a header-driven
/// test scheme, so the policy itself (authenticated user + Admin.Access scope) is what is under test.
/// </summary>
public class AdminAuthorizationTests : IClassFixture<AdminAuthorizationTests.AzureAdEnabledApiFactory>
{
    private const string TestScheme = "TestAuth";
    private const string AuthHeader = "X-Test-Auth";
    private const string ScopeHeader = "X-Test-Scope";

    private readonly HttpClient _client;

    public AdminAuthorizationTests(AzureAdEnabledApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _client = factory.CreateClient();
    }

    public static TheoryData<string, string> AdminRoutes => new()
    {
        { "GET", "/api/admin/feeds" },
        { "GET", "/api/admin/statistics" },
        { "POST", "/api/admin/cache/invalidate" },
        { "GET", "/api/admin/newsletter/subscribers" },
        { "PUT", "/api/ghc-features/some-feature" },
        { "DELETE", "/api/ghc-features/some-feature" },
        { "POST", "/api/ghc-features/some-feature/content-links" },
        { "DELETE", "/api/ghc-features/some-feature/content-links" },
        { "PUT", "/api/ghc-features/some-feature/thumbnail" }
    };

    public static TheoryData<string> PublicRoutes => new()
    {
        "/api/sections",
        "/api/ghc-features/",
        "/api/vscode-updates",
        "/api/newsletter/sections"
    };

    [Theory]
    [MemberData(nameof(AdminRoutes))]
    public async Task AdminRoute_WithoutToken_ReturnsUnauthorized(string method, string route)
    {
        // Arrange
        using var request = new HttpRequestMessage(new HttpMethod(method), route);

        // Act
        var response = await _client.SendAsync(request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [MemberData(nameof(AdminRoutes))]
    public async Task AdminRoute_AuthenticatedWithoutAdminScope_ReturnsForbidden(string method, string route)
    {
        // Arrange
        using var request = new HttpRequestMessage(new HttpMethod(method), route);
        request.Headers.Add(AuthHeader, "true");
        request.Headers.Add(ScopeHeader, "Some.OtherScope");

        // Act
        var response = await _client.SendAsync(request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Theory]
    [InlineData("/api/admin/feeds")]
    [InlineData("/api/admin/statistics")]
    [InlineData("/api/admin/newsletter/subscribers")]
    public async Task AdminRoute_WithAdminScope_ReturnsOk(string route)
    {
        // Arrange
        using var request = new HttpRequestMessage(HttpMethod.Get, route);
        request.Headers.Add(AuthHeader, "true");
        request.Headers.Add(ScopeHeader, "Admin.Access");

        // Act
        var response = await _client.SendAsync(request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [MemberData(nameof(PublicRoutes))]
    public async Task PublicRoute_WithAzureAdConfigured_RemainsAnonymous(string route)
    {
        // Act
        var response = await _client.GetAsync(route, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    /// <summary>
    /// Integration factory with Azure AD configured and a header-driven authentication scheme.
    /// </summary>
    public sealed class AzureAdEnabledApiFactory : TechHubIntegrationTestApiFactory
    {
        protected override void ConfigureTestSpecificServices(IWebHostBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);
            base.ConfigureTestSpecificServices(builder);

            builder.UseSetting("AzureAd:ClientId", "00000000-0000-0000-0000-000000000001");
            builder.UseSetting("AzureAd:TenantId", "00000000-0000-0000-0000-000000000002");

            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication(TestScheme)
                    .AddScheme<AuthenticationSchemeOptions, HeaderAuthHandler>(TestScheme, _ => { });
            });
        }
    }

    private sealed class HeaderAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public HeaderAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(AuthHeader))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var claims = new List<Claim> { new(ClaimTypes.Name, "test-admin") };
            var scope = Request.Headers[ScopeHeader].ToString();
            if (!string.IsNullOrEmpty(scope))
            {
                claims.Add(new Claim("scp", scope));
                claims.Add(new Claim("http://schemas.microsoft.com/identity/claims/scope", scope));
            }

            var identity = new ClaimsIdentity(claims, TestScheme);
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), TestScheme);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
