using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TechHub.Api.Tests;

/// <summary>
/// Regression tests for the middleware ordering of forwarded headers and HTTPS redirection.
/// Behind the Azure App Service proxy the request arrives over HTTP with X-Forwarded-Proto: https;
/// forwarded headers must be applied first, otherwise HTTPS redirection loops forever.
/// </summary>
public class HttpsRedirectionTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly HttpClient _client;

    public HttpsRedirectionTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _client = factory
            .WithWebHostBuilder(builder => builder.UseSetting("HTTPS_PORT", "443"))
            .CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
    }

    [Fact]
    public async Task Request_WithForwardedProtoHttps_IsNotRedirected()
    {
        // Arrange
        using var request = new HttpRequestMessage(HttpMethod.Get, "/");
        request.Headers.Add("X-Forwarded-Proto", "https");

        // Act
        using var response = await _client.SendAsync(request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Request_OverPlainHttp_IsRedirectedToHttps()
    {
        // Act
        using var response = await _client.GetAsync("/", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.TemporaryRedirect, HttpStatusCode.PermanentRedirect);
        response.Headers.Location?.Scheme.Should().Be("https");
    }
}
