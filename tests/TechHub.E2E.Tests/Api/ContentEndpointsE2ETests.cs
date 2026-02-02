using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.Core.Models;

namespace TechHub.E2E.Tests.Api;

/// <summary>
/// End-to-end tests for Content API endpoints.
/// Tests: GET /api/sections/{sectionName}/collections/{collectionName}/items
/// 
/// API Pattern:
/// - Use "all" as sectionName for cross-section queries
/// - Use "all" as collectionName for all collections within a section
/// - Both "all" values can be combined for all content
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

    #region GET /api/sections/all/collections/all/items - All Content

    [Fact]
    public async Task GetAllContent_ReturnsRealContent()
    {
        // Act - Use "all" section and "all" collection to get all content
        var response = await _client.GetAsync("/api/sections/all/collections/all/items");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        content.Should().NotBeNull();
        content.Should().NotBeEmpty();

        // Verify content has expected structure
        content!.Should().AllSatisfy(item =>
        {
            item.Slug.Should().NotBeNullOrEmpty();
            item.Title.Should().NotBeNullOrEmpty();
            item.CollectionName.Should().NotBeNullOrEmpty();
            item.DateEpoch.Should().BeGreaterThan(0);
        });

        // Content should be sorted by date descending (newest first)
        for (int i = 0; i < content!.Count - 1; i++)
        {
            var currentItem = content[i];
            var nextItem = content[i + 1];
            if (currentItem is not null && nextItem is not null)
            {
                currentItem.DateEpoch.Should().BeGreaterThanOrEqualTo(nextItem.DateEpoch);
            }
        }
    }

    #endregion

    #region GET /api/sections/all/collections/{collectionName}/items - Collection Filtering

    [Fact]
    public async Task GetContentByCollection_News_ReturnsNewsItems()
    {
        // Act - Use "all" section to get news from all sections
        var response = await _client.GetAsync("/api/sections/all/collections/news/items");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items!.Should().AllSatisfy(item => item.CollectionName.Should().Be("news"));
    }

    [Fact]
    public async Task GetContentByCollection_Videos_ReturnsVideoItems()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/all/collections/videos/items");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items.Should().AllSatisfy(item => item.CollectionName.Should().Be("videos"));
    }

    [Fact]
    public async Task GetContentByCollection_Blogs_ReturnsBlogItems()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/all/collections/blogs/items");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items.Should().AllSatisfy(item => item.CollectionName.Should().Be("blogs"));
    }

    [Fact]
    public async Task GetContentByCollection_Community_ReturnsCommunityItems()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/all/collections/community/items");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items.Should().AllSatisfy(item => item.CollectionName.Should().Be("community"));
    }

    [Fact]
    public async Task GetContentByCollection_Roundups_ReturnsRoundupItems()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/all/collections/roundups/items");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items.Should().AllSatisfy(item => item.CollectionName.Should().Be("roundups"));
    }

    #endregion

    #region GET /api/sections/{sectionName}/collections/all/items - Section Filtering

    [Fact]
    public async Task GetContentBySection_AI_ReturnsAIItems()
    {
        // Act - Use "all" collection to get all items in AI section
        var response = await _client.GetAsync("/api/sections/ai/collections/all/items");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetContentBySection_GitHubCopilot_ReturnsCopilotItems()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/github-copilot/collections/all/items");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetContentBySection_Azure_ReturnsAzureItems()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/azure/collections/all/items");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
    }

    #endregion

    #region GET /api/sections/{sectionName}/collections/{collectionName}/items - Combined Filtering

    [Fact]
    public async Task GetContent_BySectionAndCollection_ReturnsFilteredItems()
    {
        // Act - Get news items from GitHub Copilot section only
        var response = await _client.GetAsync("/api/sections/github-copilot/collections/news/items");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items.Should().AllSatisfy(item =>
        {
            item.CollectionName.Should().Be("news");
        });
    }

    [Fact]
    public async Task SearchContent_ByQuery_ReturnsMatchingItems()
    {
        // Act - Search for "copilot" in all content
        var response = await _client.GetAsync("/api/sections/all/collections/all/items?q=copilot");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();

        // Items should contain "copilot" in title, excerpt, or tags
        items.Should().AllSatisfy(item =>
        {
            var containsInTitle = item.Title.Contains("copilot", StringComparison.OrdinalIgnoreCase);
            var containsInExcerpt = item.Excerpt.Contains("copilot", StringComparison.OrdinalIgnoreCase);
            var containsInTags = item.Tags.Any(tag => tag.Contains("copilot", StringComparison.OrdinalIgnoreCase));

            (containsInTitle || containsInExcerpt || containsInTags).Should().BeTrue();
        });
    }

    #endregion

    #region Performance Tests

    [Fact]
    public async Task GetAllContent_RespondsQuickly_DueToInMemoryCache()
    {
        // Warm up cache
        await _client.GetAsync("/api/sections/all/collections/all/items");

        // Act - Second request should be very fast (from cache)
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var response = await _client.GetAsync("/api/sections/all/collections/all/items");
        stopwatch.Stop();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Should respond in less than 500ms from cache (generous to avoid flaky tests on loaded systems)
        // The actual response is typically <50ms but we use a higher threshold for reliability
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(500,
            "cached response should be fast, even under system load");
    }

    [Fact]
    public async Task FilterContent_RespondsQuickly_DueToInMemoryCache()
    {
        // Warm up cache
        await _client.GetAsync("/api/sections/ai/collections/all/items");

        // Act - Second request should be very fast (from cache)
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var response = await _client.GetAsync("/api/sections/ai/collections/all/items");
        stopwatch.Stop();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Should respond in less than 100ms from cache
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(100);
    }

    #endregion

    #region Data Validation Tests

    [Fact]
    public async Task AllContent_HasRequiredFields()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/all/collections/all/items");
        var content = await response.Content.ReadFromJsonAsync<List<ContentItem>>();

        // Assert
        content!.Should().AllSatisfy(item =>
        {
            item.Slug.Should().NotBeNullOrEmpty();
            item.Title.Should().NotBeNullOrEmpty();
            item.Author.Should().NotBeNullOrEmpty();
            item.DateEpoch.Should().BeGreaterThan(0);
            item.CollectionName.Should().NotBeNullOrEmpty();
            item.Tags.Should().NotBeEmpty();
        });
    }

    [Fact]
    public async Task AllContent_HasValidDateFormats()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/all/collections/all/items");
        var content = await response.Content.ReadFromJsonAsync<List<ContentItem>>();

        // Assert
        content!.Should().AllSatisfy(item =>
        {
            // DateEpoch should be a reasonable Unix timestamp
            item.DateEpoch.Should().BeGreaterThan(1000000000); // After 2001
            item.DateEpoch.Should().BeLessThan(2000000000); // Before 2033
        });
    }

    [Fact]
    public async Task AllContent_HasValidUrls()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/all/collections/all/items");
        var content = await response.Content.ReadFromJsonAsync<List<ContentItem>>();

        // Assert
        content!.Should().AllSatisfy(item =>
        {
            var href = item.GetHref();

            // External content (news, blogs, community) links to original source
            if (item.LinksExternally())
            {
                href.Should().StartWith("http", $"External item '{item.Slug}' should have full URL");
            }
            else
            {
                // Internal content (videos, roundups, custom) links within site
                // URL should start with / and contain the collection name and slug (lowercased)
                href.Should().StartWith("/");
                href.Should().Contain(item.CollectionName.ToLowerInvariant(),
                    $"URL '{href}' should contain collection '{item.CollectionName}' (not subcollection '{item.SubcollectionName}')");
                href.Should().Contain(item.Slug.ToLowerInvariant());
            }
        });
    }

    #endregion
}
