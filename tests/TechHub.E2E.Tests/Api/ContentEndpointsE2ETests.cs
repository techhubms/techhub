using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.Core.DTOs;

namespace TechHub.E2E.Tests.Api;

/// <summary>
/// End-to-end tests for Content API endpoints
/// Tests: GET /api/content, GET /api/content/filter
/// </summary>
public class ContentEndpointsE2ETests(ApiTestFactory factory) : IClassFixture<ApiTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    #region GET /api/content - Basic Retrieval

    [Fact]
    public async Task GetAllContent_ReturnsRealContent()
    {
        // Act
        var response = await _client.GetAsync("/api/content");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        content.Should().NotBeNull();
        content.Should().NotBeEmpty();

        // Verify content has expected structure
        content!.Should().AllSatisfy(item =>
        {
            item.Slug.Should().NotBeNullOrEmpty();
            item.Title.Should().NotBeNullOrEmpty();
            item.CollectionName.Should().NotBeNullOrEmpty();
            item.DateEpoch.Should().BeGreaterThan(0);
            item.Url.Should().NotBeNullOrEmpty();
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

    [Fact]
    public async Task GetContentByCollection_News_ReturnsNewsItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content?collectionName=news");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items!.Should().AllSatisfy(item => item.CollectionName.Should().Be("news"));
    }

    [Fact]
    public async Task GetContentByCollection_Videos_ReturnsVideoItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content?collectionName=videos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items.Should().AllSatisfy(item => item.CollectionName.Should().Be("videos"));
    }

    [Fact]
    public async Task GetContentByCollection_Blogs_ReturnsBlogItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content?collectionName=blogs");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items.Should().AllSatisfy(item => item.CollectionName.Should().Be("blogs"));
    }

    [Fact]
    public async Task GetContentByCollection_Community_ReturnsCommunityItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content?collectionName=community");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items.Should().AllSatisfy(item => item.CollectionName.Should().Be("community"));
    }

    [Fact]
    public async Task GetContentByCollection_Roundups_ReturnsRoundupItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content?collectionName=roundups");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items.Should().AllSatisfy(item => item.CollectionName.Should().Be("roundups"));
    }

    #endregion

    #region GET /api/content - Section Filtering

    [Fact]
    public async Task GetContentBySection_AI_ReturnsAIItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content?sectionName=ai");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items.Should().AllSatisfy(item => item.SectionNames.Should().Contain("ai"));
    }

    [Fact]
    public async Task GetContentBySection_GitHubCopilot_ReturnsCopilotItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content?sectionName=github-copilot");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items.Should().AllSatisfy(item => item.SectionNames.Should().Contain("github-copilot"));
    }

    [Fact]
    public async Task GetContentBySection_Azure_ReturnsAzureItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content?sectionName=azure");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items.Should().AllSatisfy(item => item.SectionNames.Should().Contain("azure"));
    }

    #endregion

    #region GET /api/content/filter - Advanced Filtering

    [Fact]
    public async Task FilterContent_BySection_ReturnsFilteredItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?sections=ai");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items.Should().AllSatisfy(item => item.SectionNames.Should().Contain("ai"));
    }

    [Fact]
    public async Task FilterContent_BySectionAndCollection_ReturnsFilteredItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?sections=github-copilot&collections=news");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items.Should().AllSatisfy(item =>
        {
            item.CollectionName.Should().Be("news");
            item.SectionNames.Should().Contain("github-copilot");
        });
    }

    [Fact]
    public async Task FilterContent_ByMultipleSections_ReturnsItemsFromAnySections()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?sections=ai,azure");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items!.Should().AllSatisfy(item =>
        {
            var hasAI = item.SectionNames.Contains("ai");
            var hasAzure = item.SectionNames.Contains("azure");
            (hasAI || hasAzure).Should().BeTrue("each item should have at least one of the filtered sections");
        });
    }

    [Fact]
    public async Task SearchContent_ByQuery_ReturnsMatchingItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?q=copilot");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();

        // Items should contain "copilot" in title, description, or tags
        items.Should().AllSatisfy(item =>
        {
            var containsInTitle = item.Title.Contains("copilot", StringComparison.OrdinalIgnoreCase);
            var containsInDescription = item.Description.Contains("copilot", StringComparison.OrdinalIgnoreCase);
            var containsInTags = item.Tags.Any(tag => tag.Contains("copilot", StringComparison.OrdinalIgnoreCase));

            (containsInTitle || containsInDescription || containsInTags).Should().BeTrue();
        });
    }

    #endregion

    #region Performance Tests

    [Fact]
    public async Task GetAllContent_RespondsQuickly_DueToInMemoryCache()
    {
        // Warm up cache
        await _client.GetAsync("/api/content");

        // Act - Second request should be very fast (from cache)
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var response = await _client.GetAsync("/api/content");
        stopwatch.Stop();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Should respond in less than 100ms from cache
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(100);
    }

    [Fact]
    public async Task FilterContent_RespondsQuickly_DueToInMemoryCache()
    {
        // Warm up cache
        await _client.GetAsync("/api/content/filter?sections=ai");

        // Act - Second request should be very fast (from cache)
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var response = await _client.GetAsync("/api/content/filter?sections=ai");
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
        var response = await _client.GetAsync("/api/content");
        var content = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();

        // Assert
        content!.Should().AllSatisfy(item =>
        {
            item.Slug.Should().NotBeNullOrEmpty();
            item.Title.Should().NotBeNullOrEmpty();
            item.Description.Should().NotBeNullOrEmpty();
            item.Author.Should().NotBeNullOrEmpty();
            item.DateEpoch.Should().BeGreaterThan(0);
            item.DateIso.Should().NotBeNullOrEmpty();
            item.CollectionName.Should().NotBeNullOrEmpty();
            item.SectionNames.Should().NotBeEmpty();
            item.Tags.Should().NotBeEmpty();
            item.Url.Should().NotBeNullOrEmpty();
        });
    }

    [Fact]
    public async Task AllContent_HasValidDateFormats()
    {
        // Act
        var response = await _client.GetAsync("/api/content");
        var content = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();

        // Assert
        content!.Should().AllSatisfy(item =>
        {
            // DateEpoch should be a reasonable Unix timestamp
            item.DateEpoch.Should().BeGreaterThan(1000000000); // After 2001
            item.DateEpoch.Should().BeLessThan(2000000000); // Before 2033

            // DateIso should be parseable
            DateTimeOffset.TryParse(item.DateIso, out _).Should().BeTrue();
        });
    }

    [Fact]
    public async Task AllContent_HasValidUrls()
    {
        // Act
        var response = await _client.GetAsync("/api/content");
        var content = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();

        // Assert
        content!.Should().AllSatisfy(item =>
        {
            // URL should start with / and contain the collection and slug (lowercased)
            item.Url.Should().StartWith("/");
            item.Url.Should().Contain(item.CollectionName.ToLowerInvariant());
            item.Url.Should().Contain(item.Slug.ToLowerInvariant());
        });
    }

    #endregion
}
