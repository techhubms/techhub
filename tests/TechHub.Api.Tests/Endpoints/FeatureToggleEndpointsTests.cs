using System.Net;
using System.Net.Http.Json;
using System.Xml.Linq;
using FluentAssertions;
using TechHub.Core.Models.FeatureToggles;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for the feature toggle endpoints and for what the toggles do to the sitemap.
/// The tests change the seeded <c>timeline</c> toggle, so this class has its own factory (and database) and each test
/// starts from the seeded state.
/// </summary>
public class FeatureToggleEndpointsTests : IClassFixture<TechHubIntegrationTestApiFactory>, IAsyncLifetime
{
    private static readonly XNamespace _sitemapNs = "http://www.sitemaps.org/schemas/sitemap/0.9";

    private readonly HttpClient _client;

    public FeatureToggleEndpointsTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _client = factory.CreateClient();
    }

    public async ValueTask InitializeAsync() => await SetModeAsync(FeatureKeys.Timeline, "Admins");

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    // ── GET /api/feature-toggles ─────────────────────────────────────────────

    [Fact]
    public async Task GetFeatureToggles_SeededDatabase_TimelineIsForAdmins()
    {
        // Act
        var response = await _client.GetAsync("/api/feature-toggles", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var toggles = await response.Content.ReadFromJsonAsync<IReadOnlyList<FeatureToggle>>(TestContext.Current.CancellationToken);
        var timeline = toggles.Should().ContainSingle(t => t.Key == FeatureKeys.Timeline).Subject;
        timeline.Mode.Should().Be(FeatureToggleMode.Admins);
        timeline.Description.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetFeatureToggles_ModeIsSerializedAsText()
    {
        // Act
        var json = await _client.GetStringAsync("/api/feature-toggles", TestContext.Current.CancellationToken);

        // Assert
        json.Should().Contain("\"Admins\"");
    }

    // ── PUT /api/admin/feature-toggles/{featureKey} ──────────────────────────

    [Theory]
    [InlineData("Disabled", FeatureToggleMode.Disabled)]
    [InlineData("Everyone", FeatureToggleMode.Everyone)]
    [InlineData("Admins", FeatureToggleMode.Admins)]
    public async Task UpdateFeatureToggle_ValidMode_IsStoredAndReturnedByGet(string mode, FeatureToggleMode expected)
    {
        // Act
        var response = await PutModeAsync(FeatureKeys.Timeline, $"{{\"mode\":\"{mode}\"}}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var toggles = await _client.GetFromJsonAsync<IReadOnlyList<FeatureToggle>>("/api/feature-toggles", TestContext.Current.CancellationToken);
        toggles!.Single(t => t.Key == FeatureKeys.Timeline).Mode.Should().Be(expected);
    }

    [Fact]
    public async Task UpdateFeatureToggle_UnknownFeature_ReturnsNotFound()
    {
        // Act
        var response = await PutModeAsync("no-such-feature", "{\"mode\":\"Everyone\"}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("{\"mode\":\"Sometimes\"}")]
    [InlineData("{\"mode\":7}")]
    public async Task UpdateFeatureToggle_InvalidMode_ReturnsBadRequestAndKeepsTheMode(string body)
    {
        // Act
        var response = await PutModeAsync(FeatureKeys.Timeline, body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var toggles = await _client.GetFromJsonAsync<IReadOnlyList<FeatureToggle>>("/api/feature-toggles", TestContext.Current.CancellationToken);
        toggles!.Single(t => t.Key == FeatureKeys.Timeline).Mode.Should().Be(FeatureToggleMode.Admins);
    }

    [Fact]
    public async Task UpdateFeatureToggle_InvalidKey_ReturnsBadRequest()
    {
        // Act
        var response = await PutModeAsync("Not%20Valid!", "{\"mode\":\"Everyone\"}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // ── Sitemap ──────────────────────────────────────────────────────────────

    [Theory]
    [InlineData("Disabled", false)]
    [InlineData("Admins", false)]
    [InlineData("Everyone", true)]
    public async Task GetSitemap_TimelinePage_IsListedOnlyWhenTheFeatureIsForEveryone(string mode, bool listed)
    {
        // Arrange
        await SetModeAsync(FeatureKeys.Timeline, mode);

        // Act
        var xml = await _client.GetStringAsync("/api/sitemap", TestContext.Current.CancellationToken);

        // Assert
        var locations = XDocument.Parse(xml).Descendants(_sitemapNs + "loc").Select(l => l.Value).ToList();
        locations.Any(l => l.EndsWith("/timeline", StringComparison.Ordinal)).Should().Be(listed);
        locations.Should().Contain(l => l.EndsWith("/github-copilot/roundups", StringComparison.Ordinal), "pages without a feature are always listed");
    }

    private async Task SetModeAsync(string featureKey, string mode)
    {
        var response = await PutModeAsync(featureKey, $"{{\"mode\":\"{mode}\"}}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    private async Task<HttpResponseMessage> PutModeAsync(string featureKey, string jsonBody)
    {
        using var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");
        return await _client.PutAsync($"/api/admin/feature-toggles/{featureKey}", content, TestContext.Current.CancellationToken);
    }
}
