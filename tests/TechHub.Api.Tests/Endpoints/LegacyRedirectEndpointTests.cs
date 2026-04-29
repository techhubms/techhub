using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.Core.Models;
using TechHub.TestUtilities;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for the GET /api/legacy-redirect endpoint.
/// Tests slug resolution, section-hint preference, and error handling.
/// </summary>
public class LegacyRedirectEndpointTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly HttpClient _client;

    public LegacyRedirectEndpointTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetLegacyRedirect_ReturnsNoContent_ForNonexistentSlug()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/legacy-redirect?slug=this-slug-does-not-exist-xyz-abc-123",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task GetLegacyRedirect_ReturnsBadRequest_WhenSlugMissing()
    {
        // Act — no slug parameter at all
        var response = await _client.GetAsync(
            "/api/legacy-redirect",
            TestContext.Current.CancellationToken);

        // Assert — ASP.NET Core returns 400 for missing required query parameter
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("/api/legacy-redirect?slug=")]
    [InlineData("/api/legacy-redirect?slug=has spaces")]
    [InlineData("/api/legacy-redirect?slug=../../etc/passwd")]
    [InlineData("/api/legacy-redirect?slug=<script>alert(1)</script>")]
    public async Task GetLegacyRedirect_ReturnsBadRequest_ForInvalidSlug(string url)
    {
        var response = await _client.GetAsync(url, TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetLegacyRedirect_ReturnsBadRequest_ForInvalidSection()
    {
        var response = await _client.GetAsync(
            "/api/legacy-redirect?slug=valid-slug&section=Invalid Section!",
            TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetLegacyRedirect_ReturnsUrl_ForKnownSlug()
    {
        // Arrange — fetch a real slug from the test database (roundups link internally)
        var itemsResponse = await _client.GetAsync(
            "/api/sections/all/collections/roundups/items",
            TestContext.Current.CancellationToken);
        var items = (await itemsResponse.Content.ReadFromJsonAsync<CollectionItemsResponse>(
            TestContext.Current.CancellationToken))?.Items?.ToList();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty("test database should have roundup items");

        var testItem = items!.First();

        // Act — look up the slug (already lowercase, no date prefix)
        var response = await _client.GetAsync(
            $"/api/legacy-redirect?slug={Uri.EscapeDataString(testItem.Slug)}",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<LegacyRedirectResult>(
            TestContext.Current.CancellationToken);
        result.Should().NotBeNull();
        result!.Url.Should().NotBeNullOrEmpty();
        result.Url.Should().StartWith("/", "internal items should redirect to a path starting with /");
    }

    [Fact]
    public async Task GetLegacyRedirect_WithSectionHint_ReturnsMatchingSection()
    {
        // Arrange — fetch a real item so we know its section
        var itemsResponse = await _client.GetAsync(
            "/api/sections/all/collections/roundups/items",
            TestContext.Current.CancellationToken);
        var items = (await itemsResponse.Content.ReadFromJsonAsync<CollectionItemsResponse>(
            TestContext.Current.CancellationToken))?.Items?.ToList();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();

        var testItem = items!.First();

        // Act — request with correct section hint
        var response = await _client.GetAsync(
            $"/api/legacy-redirect?slug={Uri.EscapeDataString(testItem.Slug)}&section={Uri.EscapeDataString(testItem.PrimarySectionName)}",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<LegacyRedirectResult>(
            TestContext.Current.CancellationToken);
        result.Should().NotBeNull();
        result!.Url.Should().Contain(testItem.PrimarySectionName,
            "redirect URL should contain the preferred section when hint matches");
    }

    [Fact]
    public async Task GetLegacyRedirect_RedirectUrlContainsSlug()
    {
        // Arrange — get a known internal item
        var itemsResponse = await _client.GetAsync(
            "/api/sections/github-copilot/collections/videos/items",
            TestContext.Current.CancellationToken);
        var items = (await itemsResponse.Content.ReadFromJsonAsync<CollectionItemsResponse>(
            TestContext.Current.CancellationToken))?.Items?.ToList();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty("github-copilot section should have videos");

        var testItem = items!.First();

        // Act
        var response = await _client.GetAsync(
            $"/api/legacy-redirect?slug={Uri.EscapeDataString(testItem.Slug)}",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<LegacyRedirectResult>(
            TestContext.Current.CancellationToken);
        result!.Url.Should().EndWith(testItem.Slug,
            "the canonical URL should end with the slug");
        result.Url.Should().MatchRegex(@"^/[a-z\-]+/[a-z\-]+/",
            "URL should follow /{section}/{collection}/{slug} format");
    }
}
