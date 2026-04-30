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
    public async Task GetLegacyRedirect_ForNonRoundupSlug_ReturnsCorrectSectionUrl()
    {
        // Arrange — videos are internal items whose URL includes the section name.
        var itemsResponse = await _client.GetAsync(
            "/api/sections/github-copilot/collections/videos/items",
            TestContext.Current.CancellationToken);
        var items = (await itemsResponse.Content.ReadFromJsonAsync<CollectionItemsResponse>(
            TestContext.Current.CancellationToken))?.Items?.ToList();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty("github-copilot section should have videos");

        var testItem = items!.First();

        // Act — pass a section hint so the preferred section is reflected in the URL
        var response = await _client.GetAsync(
            $"/api/legacy-redirect?slug={Uri.EscapeDataString(testItem.Slug)}&section={Uri.EscapeDataString(testItem.PrimarySectionName)}",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<LegacyRedirectResult>(
            TestContext.Current.CancellationToken);
        result.Should().NotBeNull();
        result!.Url.Should().EndWith(testItem.Slug, "canonical URL should end with the slug");
        result.Url.Should().Contain(testItem.PrimarySectionName,
            "URL should reflect the preferred section from the hint");
        result.Url.Should().MatchRegex(@"^/[a-z\-]+/[a-z\-]+/",
            "URL should follow /{section}/{collection}/{slug} format");
    }

    [Fact]
    public async Task GetLegacyRedirect_ForRoundupSlug_ReturnsRoundupPath()
    {
        // Arrange — fetch a real roundup slug from the DB, skipping the special-cased one.
        var itemsResponse = await _client.GetAsync(
            "/api/sections/all/collections/roundups/items",
            TestContext.Current.CancellationToken);
        var items = (await itemsResponse.Content.ReadFromJsonAsync<CollectionItemsResponse>(
            TestContext.Current.CancellationToken))?.Items?.ToList();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty("test database should have roundup items");

        // Skip the special-cased slug which is handled by a separate test
        var testItem = items!.First(i =>
            !i.Slug.Equals("weekly-ai-and-tech-news-roundup", StringComparison.OrdinalIgnoreCase));

        // Act
        var response = await _client.GetAsync(
            $"/api/legacy-redirect?slug={Uri.EscapeDataString(testItem.Slug)}",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<LegacyRedirectResult>(
            TestContext.Current.CancellationToken);
        result.Should().NotBeNull();
        result!.Url.Should().StartWith("/all/roundups/",
            "roundups always resolve to /all/roundups/{slug} regardless of section hint");
        result.Url.Should().EndWith(testItem.Slug, "canonical URL should end with the slug");
    }

    [Fact]
    public async Task GetLegacyRedirect_WeeklyRoundupSlug_ReturnsLatestRoundup()
    {
        // Act — legacy slug used as the shared permalink for all weekly roundups on the old site.
        // The special case bypasses normal slug lookup and returns the latest roundup instead.
        var response = await _client.GetAsync(
            "/api/legacy-redirect?slug=Weekly-AI-and-Tech-News-Roundup",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK,
            "the weekly roundup slug should redirect to the latest roundup, not return 404");

        var result = await response.Content.ReadFromJsonAsync<LegacyRedirectResult>(
            TestContext.Current.CancellationToken);
        result.Should().NotBeNull();
        result!.Url.Should().StartWith("/all/roundups/",
            "roundups always live at /all/roundups/{slug}");
    }
}
