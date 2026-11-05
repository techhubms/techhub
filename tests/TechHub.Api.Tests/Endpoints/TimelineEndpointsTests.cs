using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Api.Tests.TestSupport;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for GET /api/sections/{sectionName}/timeline: the dated events of the articles of a section.
/// Every test seeds its own article (in the AI section) and product name, so tests are independent of each other.
/// </summary>
public class TimelineEndpointsTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly HttpClient _client;
    private readonly TechHubIntegrationTestApiFactory _factory;

    public TimelineEndpointsTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;
        _client = factory.CreateClient();
    }

    private static string NewProduct() => $"Timeline API Product {Guid.NewGuid():N}";

    private static TimelineEvent Event(string product, string feature, string kind = "ga", string date = "2026-03-10") => new()
    {
        Kind = kind,
        Product = product,
        Feature = feature,
        Description = $"{kind} of {feature}",
        Date = DateOnly.Parse(date, CultureInfo.InvariantCulture),
        DatePrecision = "day",
        DateSource = "explicit"
    };

    private async Task SeedAsync(string product, params TimelineEvent[] events)
    {
        var slug = $"timeline-api-{Guid.NewGuid():N}";
        await ContentItemSeeder.InsertAsync(_factory.Services, "news", slug, $"https://example.com/{slug}", title: $"Timeline API article about {product}");

        await using var scope = _factory.Services.CreateAsyncScope();
        var timeline = scope.ServiceProvider.GetRequiredService<ITimelineEventRepository>();
        await timeline.ReplaceEventsAsync("news", slug, events, ITimelineEventRepository.ExtractedFromSource, ITimelineEventRepository.CurrentPromptVersion, TestContext.Current.CancellationToken);
    }

    private async Task<List<TimelineEntry>> GetEntriesAsync(string product, string query = "")
    {
        var entries = await _client.GetFromJsonAsync<List<TimelineEntry>>($"/api/sections/ai/timeline{query}", TestContext.Current.CancellationToken);
        return entries!.Where(e => e.Product == product).ToList();
    }

    [Fact]
    public async Task GetTimeline_ReturnsTheEventsOfTheSectionNewestFirst()
    {
        var product = NewProduct();
        await SeedAsync(product, Event(product, "Older", date: "2026-01-05"), Event(product, "Newer", date: "2026-04-20"));

        var entries = await GetEntriesAsync(product);

        entries.Select(e => e.Feature).Should().Equal("Newer", "Older");
        entries[0].Date.Should().Be(new DateOnly(2026, 4, 20));
        entries[0].Kind.Should().Be("ga");
        entries[0].DatePrecision.Should().Be("day");
        entries[0].Sources.Should().ContainSingle().Which.Title.Should().Be($"Timeline API article about {product}");
    }

    [Fact]
    public async Task GetTimeline_FiltersByDateRangeAndKinds()
    {
        var product = NewProduct();
        await SeedAsync(product,
            Event(product, "A", "ga", "2026-01-10"),
            Event(product, "B", "ga", "2026-03-10"),
            Event(product, "C", "retirement", "2026-03-20"),
            Event(product, "D", "ga", "2026-06-10"));

        var inRange = await GetEntriesAsync(product, "?from=2026-02-01&to=2026-04-30");
        var kinds = await GetEntriesAsync(product, "?kinds=retirement,deprecation");

        inRange.Select(e => e.Feature).Should().Equal("C", "B");
        kinds.Select(e => e.Feature).Should().Equal("C");
    }

    [Fact]
    public async Task GetTimeline_WithTake_ReturnsTheNewestEntriesOnly()
    {
        var product = NewProduct();
        await SeedAsync(product, Event(product, "Shared", date: "9998-12-30"));
        await SeedAsync(product, Event(product, "Shared", date: "9998-12-30"));
        await SeedAsync(product, Event(product, "Older", date: "9998-12-29"));

        var response = await _client.GetFromJsonAsync<List<TimelineEntry>>("/api/sections/ai/timeline?take=1", TestContext.Current.CancellationToken);

        var selected = response.Should().ContainSingle().Subject;
        selected.Product.Should().Be(product);
        selected.Feature.Should().Be("Shared");
        selected.Sources.Should().HaveCount(2, "limiting distinct entries must retain every source for the selected event");
    }

    [Fact]
    public async Task GetTimeline_ForAnUnknownSection_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/sections/no-such-section/timeline", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("?from=yesterday")]
    [InlineData("?to=2026-13-45")]
    [InlineData("?kinds=ga,not-a-kind")]
    [InlineData("?take=0")]
    [InlineData("?take=501")]
    public async Task GetTimeline_WithInvalidInput_ReturnsBadRequest(string query)
    {
        var response = await _client.GetAsync($"/api/sections/ai/timeline{query}", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetTimeline_IsPublic()
    {
        var response = await _client.GetAsync("/api/sections/ai/timeline", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
