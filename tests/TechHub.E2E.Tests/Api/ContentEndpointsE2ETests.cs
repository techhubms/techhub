using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.Core.Models;

namespace TechHub.E2E.Tests.Api;

/// <summary>
/// End-to-end tests for Content API endpoints
/// Tests verify production integration with PostgreSQL, real markdown files, and full HTTP pipeline
/// Coverage:
/// - Section endpoints: GET /api/sections, GET /api/sections/{name}
/// - Collection items: GET /api/sections/{section}/collections/{collection}/items (with pagination and filtering)
/// - Content detail: GET /api/sections/{section}/collections/{collection}/{slug}
/// - Tag cloud: GET /api/sections/{section}/collections/{collection}/tags
/// - Custom pages: GET /api/custom-pages/{pageName}
/// </summary>
[Collection("API E2E Tests")]
public class ContentEndpointsE2ETests
{
    private readonly HttpClient _client;

    public ContentEndpointsE2ETests(ApiCollectionFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        _client = fixture.Factory.CreateClient();
    }

    [Fact]
    public async Task GetAllSections_ReturnsRealSections()
    {
        // Act
        var response = await _client.GetAsync("/api/sections", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var sections = await response.Content.ReadFromJsonAsync<List<Section>>(TestContext.Current.CancellationToken);
        sections.Should().NotBeNull();
        sections!.Should().NotBeEmpty();

        // Verify expected sections from appsettings.json
        sections.Should().Contain(s => s.Name == "ai");
        sections.Should().Contain(s => s.Name == "github-copilot");
        sections.Should().Contain(s => s.Name == "azure");
        sections.Should().Contain(s => s.Name == "ml");
        sections.Should().Contain(s => s.Name == "dotnet");
        sections.Should().Contain(s => s.Name == "devops");
        sections.Should().Contain(s => s.Name == "security");

        // All sections should have collections
        sections.Should().AllSatisfy(s => s.Collections.Should().NotBeEmpty());
    }

    [Fact]
    public async Task GetSectionByName_ReturnsRealSection()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/ai", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var section = await response.Content.ReadFromJsonAsync<Section>(TestContext.Current.CancellationToken);
        section.Should().NotBeNull();
        section!.Name.Should().Be("ai");
        section.Title.Should().Be("Artificial Intelligence");
        section.Collections.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetSectionByName_InvalidSection_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/nonexistent", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAllSections_CaseSensitive_ExactMatchRequired()
    {
        // Act - Try with different casing
        var upperResponse = await _client.GetAsync("/api/sections/AI", TestContext.Current.CancellationToken);
        var mixedResponse = await _client.GetAsync("/api/sections/GitHub-Copilot", TestContext.Current.CancellationToken);

        // Assert - Section names are case-sensitive in URLs
        upperResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        mixedResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetCollectionItems_ReturnsRealContentWithPagination()
    {
        // Act - Get first page of AI news items
        var response = await _client.GetAsync("/api/sections/ai/collections/news/items?take=10", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<CollectionItemsResponse>(TestContext.Current.CancellationToken);
        result.Should().NotBeNull();
        var items = result!.Items;
        items.Should().NotBeEmpty();
        items.Should().HaveCountLessThanOrEqualTo(10, "take parameter should limit results");

        // Verify items have expected structure
        items.Should().AllSatisfy(item =>
        {
            item.Slug.Should().NotBeNullOrEmpty();
            item.Title.Should().NotBeNullOrEmpty();
            item.CollectionName.Should().Be("news");
            item.Tags.Should().NotBeNull();
        });
    }

    [Fact]
    public async Task GetCollectionItems_WithSkipAndTake_ReturnsPaginatedResults()
    {
        // Act - Get page with skip offset to test pagination with larger datasets
        var response = await _client.GetAsync("/api/sections/all/collections/all/items?skip=500&take=20", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<CollectionItemsResponse>(TestContext.Current.CancellationToken);
        result.Should().NotBeNull();
        var items = result!.Items;
        items.Should().HaveCountLessThanOrEqualTo(20, "take parameter should limit results");

        // With 4000+ items in production, skip=500 should return results
        items.Should().NotBeEmpty("production dataset should have items beyond offset 500");

        // Verify pagination doesn't break data structure
        items.Should().AllSatisfy(item =>
        {
            item.Slug.Should().NotBeNullOrEmpty();
            item.Title.Should().NotBeNullOrEmpty();
            item.Tags.Should().NotBeNull();
        });
    }

    [Fact]
    public async Task GetCollectionItems_WithTagFilter_ReturnsFilteredContent()
    {
        // Act - Filter by tags (using common tags that should exist in real data)
        var response = await _client.GetAsync("/api/sections/github-copilot/collections/all/items?tags=GitHub%20Copilot&take=20", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<CollectionItemsResponse>(TestContext.Current.CancellationToken);
        result.Should().NotBeNull();
        var items = result!.Items;
        items.Should().NotBeEmpty("GitHub Copilot section should have items with the tag");

        // All items should contain the filter tag (tags are case-sensitive)
        items.Should().AllSatisfy(item =>
        {
            item.Tags.Should().Contain("GitHub Copilot", "all items should match the tag filter");
        });
    }

    [Fact]
    public async Task GetContentDetail_ReturnsRealContentWithRenderedHtml()
    {
        // Arrange - Get a real item first (using roundups which link internally)
        var itemsResponse = await _client.GetAsync("/api/sections/all/collections/roundups/items?take=1", TestContext.Current.CancellationToken);
        var itemsResult = await itemsResponse.Content.ReadFromJsonAsync<CollectionItemsResponse>(TestContext.Current.CancellationToken);
        itemsResult.Should().NotBeNull();
        itemsResult!.Items.Should().NotBeEmpty();
        var testItem = itemsResult.Items.First();

        // Act - Get content detail
        var response = await _client.GetAsync($"/api/sections/all/collections/roundups/{testItem.Slug}", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var detail = await response.Content.ReadFromJsonAsync<ContentItemDetail>(TestContext.Current.CancellationToken);
        detail.Should().NotBeNull();
        detail!.Slug.Should().Be(testItem.Slug);
        detail.Title.Should().NotBeNullOrEmpty();
        detail.RenderedHtml.Should().NotBeNullOrEmpty("content should be rendered to HTML");
        detail.RenderedHtml.Should().Contain("<", "rendered content should contain HTML tags");
        detail.Author.Should().NotBeNullOrEmpty();
        detail.DateEpoch.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetContentDetail_ExternalCollection_ReturnsNotFound()
    {
        // Arrange - Get a news item (external collection)
        var itemsResponse = await _client.GetAsync("/api/sections/ai/collections/news/items?take=1", TestContext.Current.CancellationToken);
        var itemsResult = await itemsResponse.Content.ReadFromJsonAsync<CollectionItemsResponse>(TestContext.Current.CancellationToken);
        itemsResult.Should().NotBeNull();
        itemsResult!.Items.Should().NotBeEmpty();
        var newsItem = itemsResult.Items.First();

        // Act - Try to get detail for external content
        var response = await _client.GetAsync($"/api/sections/ai/collections/news/{newsItem.Slug}", TestContext.Current.CancellationToken);

        // Assert - External collections should return 404 since they link to original sources
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetCollectionTags_ReturnsRealTagCloudData()
    {
        // Act - Get tag cloud for AI section (using 'all' collection for section-wide tags)
        var response = await _client.GetAsync("/api/sections/ai/collections/all/tags?maxTags=20", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tagCloud = await response.Content.ReadFromJsonAsync<List<TagCloudItem>>(TestContext.Current.CancellationToken);
        tagCloud.Should().NotBeNull();
        tagCloud!.Should().NotBeEmpty("AI section should have tags");

        // Verify tag cloud structure
        tagCloud.Should().AllSatisfy(item =>
        {
            item.Tag.Should().NotBeNullOrEmpty();
            item.Count.Should().BeGreaterThan(0);
            item.Size.Should().BeOneOf(TagSize.Small, TagSize.Medium, TagSize.Large);
        });

        // Verify tags are sorted by count descending
        for (int i = 0; i < tagCloud.Count - 1; i++)
        {
            tagCloud[i].Count.Should().BeGreaterThanOrEqualTo(tagCloud[i + 1].Count,
                "tags should be sorted by count descending");
        }
    }

    [Fact]
    public async Task GetCustomPage_DXSpace_ReturnsStructuredData()
    {
        // Act
        var response = await _client.GetAsync("/api/custom-pages/dx-space", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var data = await response.Content.ReadFromJsonAsync<DXSpacePageData>(TestContext.Current.CancellationToken);
        data.Should().NotBeNull();
        data!.Title.Should().Be("Developer Experience Space");
        data.Dora.Should().NotBeNull();
        data.Dora.Metrics.Should().HaveCount(4, "DORA should have 4 metrics");
        data.Space.Should().NotBeNull();
        data.Space.Dimensions.Should().HaveCount(5, "SPACE should have 5 dimensions");
        data.DevEx.Should().NotBeNull();
        data.BestPractices.Should().NotBeNull();
    }
}