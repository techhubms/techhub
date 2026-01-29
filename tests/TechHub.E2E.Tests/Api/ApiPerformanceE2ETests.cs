using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.Core.Models;

namespace TechHub.E2E.Tests.Api;

/// <summary>
/// Performance-focused E2E tests for ALL API endpoints.
/// Each test validates:
/// 1. Response completeness (correct data structure and content)
/// 2. Response time ≤ 200ms (WITHOUT caching - measures actual query performance)
/// 
/// These tests run FIRST in the E2E suite to validate query optimization.
/// NO warmup calls - we measure the first request to ensure queries are fast.
/// Cache is a final optimization, not a requirement for performance.
/// </summary>
[Collection("API E2E Tests")]
public class ApiPerformanceE2ETests
{
    private readonly HttpClient _client;
    private const int MaxResponseTimeMs = 200;

    public ApiPerformanceE2ETests(ApiCollectionFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        _client = fixture.Factory.CreateClient();
    }

    #region Helper Methods

    private static async Task<(HttpResponseMessage Response, long ElapsedMs)> MeasureRequestAsync(
        Func<Task<HttpResponseMessage>> requestFunc)
    {
        var sw = Stopwatch.StartNew();
        var response = await requestFunc();
        sw.Stop();
        return (response, sw.ElapsedMilliseconds);
    }

    private static void AssertPerformance(long elapsedMs, string endpointName)
    {
        if (elapsedMs > MaxResponseTimeMs)
        {
            throw new Exception(
                $"Performance degradation detected in {endpointName}: " +
                $"{elapsedMs}ms > {MaxResponseTimeMs}ms threshold. " +
                $"This indicates missing indexes, inefficient queries, or lack of caching. " +
                $"Review query execution plan and add appropriate indexes.");
        }
    }

    #endregion

    #region Content Endpoints

    [Fact]
    public async Task GetContent_Search_ReturnsCompleteResults_WithinPerformanceThreshold()
    {
        // Measured call
        var (response, elapsed) = await MeasureRequestAsync(
            () => _client.GetAsync("/api/content/search?take=5"));

        // Assert response completeness
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<SearchResults<ContentItem>>();
        result.Should().NotBeNull();
        result!.Items.Should().NotBeEmpty();
        result.Items.Should().HaveCountLessThanOrEqualTo(5);
        result.Items.Should().AllSatisfy(item =>
        {
            item.Slug.Should().NotBeNullOrEmpty();
            item.Title.Should().NotBeNullOrEmpty();
            item.CollectionName.Should().NotBeNullOrEmpty();
        });

        // Assert performance
        AssertPerformance(elapsed, "GET /api/content/search");
    }

    [Fact]
    public async Task GetContent_SearchWithFilters_ReturnsCompleteResults_WithinPerformanceThreshold()
    {
        // Measured call
        var (response, elapsed) = await MeasureRequestAsync(
            () => _client.GetAsync("/api/content/search?sections=ai&take=10"));

        // Assert completeness
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<SearchResults<ContentItem>>();
        result.Should().NotBeNull();
        result!.Items.Should().NotBeEmpty();

        // Assert performance
        AssertPerformance(elapsed, "GET /api/content/search (with filters)");
    }

    [Fact]
    public async Task GetContentDetail_ReturnsCompleteItem_WithinPerformanceThreshold()
    {
        // First, get a content item to test with
        var searchResponse = await _client.GetAsync("/api/content/search?take=1");
        var searchResult = await searchResponse.Content.ReadFromJsonAsync<SearchResults<ContentItem>>();
        var item = searchResult!.Items.First();
        var url = $"/api/content/{item.PrimarySectionName}/{item.CollectionName}/{item.Slug}";
        // Measured call
        var (response, elapsed) = await MeasureRequestAsync(() => _client.GetAsync(url));

        // Assert completeness
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var detail = await response.Content.ReadFromJsonAsync<ContentItem>();
        detail.Should().NotBeNull();
        detail!.Slug.Should().Be(item.Slug);
        detail.Title.Should().NotBeNullOrEmpty();
        // Note: Content is [JsonIgnore] - raw markdown is not serialized
        // RenderedHtml would be set if API does markdown rendering, but currently API returns raw content
        // Just verify the item was returned with expected properties
        detail.Tags.Should().NotBeNull();

        // Assert performance
        AssertPerformance(elapsed, "GET /api/content/{section}/{collection}/{slug}");
    }

    #endregion

    #region Section Endpoints

    [Fact]
    public async Task GetAllSections_ReturnsCompleteSections_WithinPerformanceThreshold()
    {
        // Measured call
        var (response, elapsed) = await MeasureRequestAsync(
            () => _client.GetAsync("/api/sections"));

        // Assert completeness
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var sections = await response.Content.ReadFromJsonAsync<List<Section>>();
        sections.Should().NotBeNull();
        sections!.Should().NotBeEmpty();
        sections.Should().AllSatisfy(s =>
        {
            s.Name.Should().NotBeNullOrEmpty();
            s.Title.Should().NotBeNullOrEmpty();
            s.Collections.Should().NotBeNull();
        });

        // Assert performance
        AssertPerformance(elapsed, "GET /api/sections");
    }

    [Fact]
    public async Task GetSectionByName_ReturnsCompleteSection_WithinPerformanceThreshold()
    {
        // Measured call
        var (response, elapsed) = await MeasureRequestAsync(
            () => _client.GetAsync("/api/sections/github-copilot"));

        // Assert completeness
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var section = await response.Content.ReadFromJsonAsync<Section>();
        section.Should().NotBeNull();
        section!.Name.Should().Be("github-copilot");
        section.Title.Should().NotBeNullOrEmpty();
        section.Collections.Should().NotBeEmpty();

        // Assert performance
        AssertPerformance(elapsed, "GET /api/sections/{name}");
    }

    [Fact]
    public async Task GetSectionItems_ReturnsCompleteItems_WithinPerformanceThreshold()
    {
        // Measured call
        var (response, elapsed) = await MeasureRequestAsync(
            () => _client.GetAsync("/api/sections/ai/items"));

        // Assert completeness
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();

        // Assert performance
        AssertPerformance(elapsed, "GET /api/sections/{name}/items");
    }

    [Fact]
    public async Task GetSectionCollections_ReturnsCompleteCollections_WithinPerformanceThreshold()
    {
        // Measured call
        var (response, elapsed) = await MeasureRequestAsync(
            () => _client.GetAsync("/api/sections/ai/collections"));

        // Assert completeness
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var collections = await response.Content.ReadFromJsonAsync<List<Collection>>();
        collections.Should().NotBeNull();
        collections!.Should().NotBeEmpty();
        collections.Should().AllSatisfy(c =>
        {
            c.Name.Should().NotBeNullOrEmpty();
            c.Title.Should().NotBeNullOrEmpty();
        });

        // Assert performance
        AssertPerformance(elapsed, "GET /api/sections/{name}/collections");
    }

    [Fact]
    public async Task GetSectionCollection_ReturnsCompleteCollection_WithinPerformanceThreshold()
    {
        // Measured call
        var (response, elapsed) = await MeasureRequestAsync(
            () => _client.GetAsync("/api/sections/ai/collections/blogs"));

        // Assert completeness
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var collection = await response.Content.ReadFromJsonAsync<Collection>();
        collection.Should().NotBeNull();
        collection!.Name.Should().Be("blogs");

        // Assert performance
        AssertPerformance(elapsed, "GET /api/sections/{name}/collections/{collection}");
    }

    [Fact]
    public async Task GetSectionCollectionItems_ReturnsCompleteItems_WithinPerformanceThreshold()
    {
        // Measured call
        var (response, elapsed) = await MeasureRequestAsync(
            () => _client.GetAsync("/api/sections/ai/collections/blogs/items"));

        // Assert completeness
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        // May be empty if no blogs in AI section, but should be fast

        // Assert performance
        AssertPerformance(elapsed, "GET /api/sections/{name}/collections/{collection}/items");
    }

    #endregion

    #region Tag Endpoints

    [Fact]
    public async Task GetTagCloud_Homepage_ReturnsCompleteTags_WithinPerformanceThreshold()
    {
        // Measured call
        var (response, elapsed) = await MeasureRequestAsync(
            () => _client.GetAsync("/api/tags/cloud?scope=Homepage&maxTags=20&minUses=1&lastDays=90"));

        // Assert completeness
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var tagCloud = await response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        tagCloud.Should().NotBeNull();
        tagCloud!.Should().NotBeEmpty();
        tagCloud.Should().HaveCountLessThanOrEqualTo(20);
        tagCloud.Should().AllSatisfy(tag =>
        {
            tag.Tag.Should().NotBeNullOrEmpty();
            tag.Count.Should().BeGreaterThan(0);
            Enum.IsDefined(typeof(TagSize), tag.Size).Should().BeTrue();
        });

        // Assert performance - THIS WAS THE SLOW ONE!
        AssertPerformance(elapsed, "GET /api/tags/cloud (Homepage)");
    }

    [Fact]
    public async Task GetTagCloud_Section_ReturnsCompleteTags_WithinPerformanceThreshold()
    {
        // Measured call
        var (response, elapsed) = await MeasureRequestAsync(
            () => _client.GetAsync("/api/tags/cloud?scope=Section&section=github-copilot"));

        // Assert completeness
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var tagCloud = await response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        tagCloud.Should().NotBeNull();
        tagCloud!.Should().NotBeEmpty();

        // Assert performance
        AssertPerformance(elapsed, "GET /api/tags/cloud (Section)");
    }

    [Fact]
    public async Task GetAllTags_ReturnsCompleteTags_WithinPerformanceThreshold()
    {
        // Measured call
        var (response, elapsed) = await MeasureRequestAsync(
            () => _client.GetAsync("/api/tags/all"));

        // Assert completeness
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<AllTagsResponse>();
        result.Should().NotBeNull();
        result!.Tags.Should().NotBeEmpty();
        result.Tags.Should().AllSatisfy(tag =>
        {
            tag.Tag.Should().NotBeNullOrEmpty();
            tag.Count.Should().BeGreaterThan(0);
        });

        // Assert performance
        AssertPerformance(elapsed, "GET /api/tags/all");
    }

    #endregion

    #region RSS Endpoints

    [Fact]
    public async Task GetAllContentFeed_ReturnsValidRSS_WithinPerformanceThreshold()
    {
        // Measured call
        var (response, elapsed) = await MeasureRequestAsync(
            () => _client.GetAsync("/api/rss/all"));

        // Assert completeness
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/rss+xml");
        var rss = await response.Content.ReadAsStringAsync();
        rss.Should().Contain("<?xml");
        rss.Should().Contain("<rss");
        rss.Should().Contain("<channel>");

        // Assert performance
        AssertPerformance(elapsed, "GET /api/rss/all");
    }

    [Fact]
    public async Task GetSectionFeed_ReturnsValidRSS_WithinPerformanceThreshold()
    {
        // Measured call
        var (response, elapsed) = await MeasureRequestAsync(
            () => _client.GetAsync("/api/rss/github-copilot"));

        // Assert completeness
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/rss+xml");
        var rss = await response.Content.ReadAsStringAsync();
        rss.Should().Contain("<?xml");
        rss.Should().Contain("GitHub Copilot");

        // Assert performance
        AssertPerformance(elapsed, "GET /api/rss/{section}");
    }

    [Fact]
    public async Task GetCollectionFeed_ReturnsValidRSS_WithinPerformanceThreshold()
    {
        // Measured call
        var (response, elapsed) = await MeasureRequestAsync(
            () => _client.GetAsync("/api/rss/collection/blogs"));

        // Assert completeness
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/rss+xml");

        // Assert performance
        AssertPerformance(elapsed, "GET /api/rss/collection/{name}");
    }

    #endregion

    #region Custom Pages Endpoints

    [Fact]
    public async Task GetDXSpaceData_ReturnsCompleteData_WithinPerformanceThreshold()
    {
        // Measured call
        var (response, elapsed) = await MeasureRequestAsync(
            () => _client.GetAsync("/api/custom-pages/dx-space"));

        // Assert completeness
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var data = await response.Content.ReadFromJsonAsync<DXSpacePageData>();
        data.Should().NotBeNull();
        data!.Title.Should().NotBeNullOrEmpty();

        // Assert performance
        AssertPerformance(elapsed, "GET /api/custom-pages/dx-space");
    }

    [Fact]
    public async Task GetHandbookData_ReturnsCompleteData_WithinPerformanceThreshold()
    {
        // Measured call
        var (response, elapsed) = await MeasureRequestAsync(
            () => _client.GetAsync("/api/custom-pages/handbook"));

        // Assert completeness
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var data = await response.Content.ReadFromJsonAsync<HandbookPageData>();
        data.Should().NotBeNull();
        data!.Title.Should().NotBeNullOrEmpty();

        // Assert performance
        AssertPerformance(elapsed, "GET /api/custom-pages/handbook");
    }

    [Fact]
    public async Task GetLevelsData_ReturnsCompleteData_WithinPerformanceThreshold()
    {
        // Measured call
        var (response, elapsed) = await MeasureRequestAsync(
            () => _client.GetAsync("/api/custom-pages/levels"));

        // Assert completeness
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var data = await response.Content.ReadFromJsonAsync<LevelsPageData>();
        data.Should().NotBeNull();
        data!.Title.Should().NotBeNullOrEmpty();

        // Assert performance
        AssertPerformance(elapsed, "GET /api/custom-pages/levels");
    }

    [Fact]
    public async Task GetFeaturesData_ReturnsCompleteData_WithinPerformanceThreshold()
    {
        // Measured call
        var (response, elapsed) = await MeasureRequestAsync(
            () => _client.GetAsync("/api/custom-pages/features"));

        // Assert completeness
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var data = await response.Content.ReadFromJsonAsync<FeaturesPageData>();
        data.Should().NotBeNull();
        data!.Title.Should().NotBeNullOrEmpty();

        // Assert performance
        AssertPerformance(elapsed, "GET /api/custom-pages/features");
    }

    [Fact]
    public async Task GetGenAIBasicsData_ReturnsCompleteData_WithinPerformanceThreshold()
    {
        // Measured call
        var (response, elapsed) = await MeasureRequestAsync(
            () => _client.GetAsync("/api/custom-pages/genai-basics"));

        // Assert completeness
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var data = await response.Content.ReadFromJsonAsync<GenAIPageData>();
        data.Should().NotBeNull();
        data!.Title.Should().NotBeNullOrEmpty();

        // Assert performance
        AssertPerformance(elapsed, "GET /api/custom-pages/genai-basics");
    }

    [Fact]
    public async Task GetGenAIAdvancedData_ReturnsCompleteData_WithinPerformanceThreshold()
    {
        // Measured call
        var (response, elapsed) = await MeasureRequestAsync(
            () => _client.GetAsync("/api/custom-pages/genai-advanced"));

        // Assert completeness
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var data = await response.Content.ReadFromJsonAsync<GenAIPageData>();
        data.Should().NotBeNull();
        data!.Title.Should().NotBeNullOrEmpty();

        // Assert performance
        AssertPerformance(elapsed, "GET /api/custom-pages/genai-advanced");
    }

    [Fact]
    public async Task GetGenAIAppliedData_ReturnsCompleteData_WithinPerformanceThreshold()
    {
        // Measured call
        var (response, elapsed) = await MeasureRequestAsync(
            () => _client.GetAsync("/api/custom-pages/genai-applied"));

        // Assert completeness
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var data = await response.Content.ReadFromJsonAsync<GenAIPageData>();
        data.Should().NotBeNull();
        data!.Title.Should().NotBeNullOrEmpty();

        // Assert performance
        AssertPerformance(elapsed, "GET /api/custom-pages/genai-applied");
    }

    [Fact]
    public async Task GetSDLCData_ReturnsCompleteData_WithinPerformanceThreshold()
    {
        // Measured call
        var (response, elapsed) = await MeasureRequestAsync(
            () => _client.GetAsync("/api/custom-pages/sdlc"));

        // Assert completeness
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var data = await response.Content.ReadFromJsonAsync<SDLCPageData>();
        data.Should().NotBeNull();
        data!.Title.Should().NotBeNullOrEmpty();

        // Assert performance
        AssertPerformance(elapsed, "GET /api/custom-pages/sdlc");
    }

    #endregion
}

