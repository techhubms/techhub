using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace TechHub.Web.Tests.Endpoints;

/// <summary>
/// Tests for <c>POST /admin/logout</c> in Program.cs — requires a valid antiforgery token, signs out of OIDC
/// (redirecting to "/") and clears the local cookie.
/// The OIDC scheme is a recording fake, so no Entra ID round-trip happens.
/// </summary>
public class AdminLogoutEndpointTests(DevelopmentWebFixture fixture) : IClassFixture<DevelopmentWebFixture>
{
    [Fact]
    public async Task Post_Logout_SignsOutOfOidcWithRedirectToRoot()
    {
        // Arrange
        var before = fixture.SignOuts.RedirectUris.Count;
        using var client = fixture.CreateClient("198.51.100.1");
        using var request = CreateLogoutRequest();

        // Act
        using var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        fixture.SignOuts.RedirectUris.Skip(before).Should().Equal("/");
    }

    [Fact]
    public async Task Post_Logout_ClearsLocalAuthenticationCookie()
    {
        // Arrange
        using var client = fixture.CreateClient("198.51.100.2");
        using var request = CreateLogoutRequest(extraCookie: ".AspNetCore.Cookies=stale-session");

        // Act
        using var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

        // Assert
        response.Headers.TryGetValues("Set-Cookie", out var setCookies).Should().BeTrue();
        setCookies.Should().Contain(c => c.StartsWith(".AspNetCore.Cookies=;", StringComparison.Ordinal)
            && c.Contains("expires=Thu, 01 Jan 1970", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task Post_LogoutTwice_SignsOutEachTimeWithoutFailing()
    {
        // Arrange
        var before = fixture.SignOuts.RedirectUris.Count;
        using var client = fixture.CreateClient("198.51.100.3");

        // Act
        using var firstRequest = CreateLogoutRequest();
        using var first = await client.SendAsync(firstRequest, TestContext.Current.CancellationToken);
        using var secondRequest = CreateLogoutRequest();
        using var second = await client.SendAsync(secondRequest, TestContext.Current.CancellationToken);

        // Assert
        first.StatusCode.Should().Be(HttpStatusCode.OK);
        second.StatusCode.Should().Be(HttpStatusCode.OK);
        fixture.SignOuts.RedirectUris.Skip(before).Should().Equal("/", "/");
    }

    [Fact]
    public async Task Post_LogoutWithoutAntiforgeryToken_IsRejectedWithoutSigningOut()
    {
        // Arrange
        var before = fixture.SignOuts.RedirectUris.Count;
        using var client = fixture.CreateClient("198.51.100.5");
        using var content = new StringContent(string.Empty);

        // Act
        using var response = await client.PostAsync("/admin/logout", content, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        fixture.SignOuts.RedirectUris.Count.Should().Be(before);
        response.Headers.Contains("Set-Cookie").Should().BeFalse();
    }

    [Theory]
    [InlineData("Cookie")]
    [InlineData("RequestVerificationToken")]
    public async Task Post_LogoutWithInvalidAntiforgeryToken_IsRejectedWithoutSigningOut(string headerName)
    {
        // Arrange
        var before = fixture.SignOuts.RedirectUris.Count;
        using var client = fixture.CreateClient("198.51.100.6");
        using var request = CreateLogoutRequest();
        request.Headers.Remove(headerName);
        request.Headers.Add(headerName, headerName == "Cookie"
            ? ".AspNetCore.Cookies=stale-session"
            : "invalid-token");

        // Act
        using var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        fixture.SignOuts.RedirectUris.Count.Should().Be(before);
        response.Headers.Contains("Set-Cookie").Should().BeFalse();
    }

    [Fact]
    public async Task Post_LogoutWithoutOidc_ClearsLocalCookieAndRedirectsToRoot()
    {
        // Arrange
        await using var factory = new TechHubWebFactory("Development");
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            BaseAddress = new Uri("https://localhost")
        });
        factory.Services.GetRequiredService<IAuthenticationSchemeProvider>()
            .RemoveScheme(OpenIdConnectDefaults.AuthenticationScheme);
        using var request = CreateLogoutRequest(
            extraCookie: ".AspNetCore.Cookies=stale-session", services: factory.Services);

        // Act
        using var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location.Should().Be(new Uri("/", UriKind.Relative));
        response.Headers.TryGetValues("Set-Cookie", out var setCookies).Should().BeTrue();
        setCookies.Should().Contain(c => c.StartsWith(".AspNetCore.Cookies=;", StringComparison.Ordinal)
            && c.Contains("expires=Thu, 01 Jan 1970", StringComparison.OrdinalIgnoreCase));
        factory.SignOuts.RedirectUris.Should().BeEmpty();
    }

    [Theory]
    [InlineData("PUT")]
    [InlineData("DELETE")]
    [InlineData("PATCH")]
    public async Task Send_NonPostNonGetMethod_IsRejectedWithoutSigningOut(string method)
    {
        // Arrange
        var before = fixture.SignOuts.RedirectUris.Count;
        using var client = fixture.CreateClient("198.51.100.4");
        using var request = new HttpRequestMessage(new HttpMethod(method), "/admin/logout");

        // Act
        using var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.MethodNotAllowed);
        fixture.SignOuts.RedirectUris.Count.Should().Be(before);
    }

    private HttpRequestMessage CreateLogoutRequest(string? extraCookie = null, IServiceProvider? services = null)
    {
        services ??= fixture.Services;
        var httpContext = new DefaultHttpContext { RequestServices = services };
        var tokens = services.GetRequiredService<IAntiforgery>().GetAndStoreTokens(httpContext);
        var antiforgeryCookie = httpContext.Response.Headers.SetCookie.ToString().Split(';')[0];

        var request = new HttpRequestMessage(HttpMethod.Post, "/admin/logout")
        {
            Content = new StringContent(string.Empty)
        };
        request.Headers.Add("Cookie", extraCookie is null ? antiforgeryCookie : $"{antiforgeryCookie}; {extraCookie}");
        request.Headers.Add(tokens.HeaderName!, tokens.RequestToken);
        return request;
    }
}
