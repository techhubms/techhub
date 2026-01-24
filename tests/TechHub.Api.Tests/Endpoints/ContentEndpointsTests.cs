using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.Core.DTOs;
using TechHub.TestUtilities;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for Content filtering API endpoints
/// Tests advanced filtering and tag retrieval with production-like test data from TestCollections
/// 
/// Test Data (8 items total):
/// - 3 news items: Agentic Memory (ai, github-copilot), .NET 10 Networking (coding, security), Commit Review (devops)
/// - 2 blogs: From Tool to Teammate (ai, github-copilot), Azure Cost (azure)
/// - 1 video: Hands-On Lab (ai, coding, github-copilot)
/// - 1 community: AI Toolkit (ai, azure, coding, github-copilot)
/// - 1 roundup: Weekly AI and Tech (ai, github-copilot, ml, azure, coding, devops, security)
/// </summary>
public class ContentEndpointsTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly HttpClient _client;

    public ContentEndpointsTests(TechHubIntegrationTestApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task FilterContent_WithNoParameters_ReturnsAllContent()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().HaveCount(8); // All 8 test items from TestCollections
    }

    [Fact]
    public async Task FilterContent_BySingleSection_ReturnsFilteredItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?sections=ai");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().HaveCount(5); // 5 items with 'ai' section
        items.Should().AllSatisfy(item => item.SectionNames.Should().Contain("ai"));
    }

    [Fact]
    public async Task FilterContent_ByMultipleSections_ReturnsItemsFromAnySections()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?sections=ai,github-copilot");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().HaveCount(5); // Items with either 'ai' OR 'github-copilot' (same items, overlap)
    }

    [Fact]
    public async Task FilterContent_BySingleCollection_ReturnsFilteredItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?collections=news");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().HaveCount(3); // 3 news items
        items.Should().AllSatisfy(item => item.CollectionName.Should().Be("news"));
    }

    [Fact]
    public async Task FilterContent_ByMultipleCollections_ReturnsItemsFromAnyCollections()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?collections=news,blogs");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().HaveCount(5); // 3 news + 2 blogs
        items.Should().AllSatisfy(item =>
            new[] { "news", "blogs" }.Should().Contain(item.CollectionName));
    }

    [Fact]
    public async Task FilterContent_BySectionAndCollection_AppliesAndLogic()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?sections=ai&collections=news");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().HaveCount(1); // Only 1 news item with 'ai' section (Agentic Memory)
        items![0].CollectionName.Should().Be("news");
        items[0].SectionNames.Should().Contain("ai");
    }

    [Fact]
    public async Task FilterContent_BySingleTag_ReturnsItemsWithTag()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?tags=Copilot");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().HaveCount(1); // Only 1 item with exact 'Copilot' tag
        items.Should().AllSatisfy(item => item.Tags.Should().Contain("Copilot"));
    }

    [Fact]
    public async Task FilterContent_ByMultipleTags_RequiresAllTags()
    {
        // Act - Looking for items with BOTH 'Code Review' AND 'Collaboration' tags
        var response = await _client.GetAsync("/api/content/filter?tags=Code Review,Collaboration");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().HaveCount(2); // 2 items have both tags (Agentic Memory + From Tool to Teammate)
        items.Should().AllSatisfy(item => 
        {
            item.Tags.Should().Contain("Code Review");
            item.Tags.Should().Contain("Collaboration");
        });
    }

    [Fact]
    public async Task FilterContent_ComplexFilter_CombinesAllCriteria()
    {
        // Act - AI section + news collection + Copilot tag
        var response = await _client.GetAsync("/api/content/filter?sections=ai&collections=news&tags=Copilot");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().HaveCount(1); // Agentic Memory for GitHub Copilot
        items![0].CollectionName.Should().Be("news");
        items[0].SectionNames.Should().Contain("ai");
        items[0].Tags.Should().Contain("Copilot");
    }

    [Fact]
    public async Task FilterContent_ByTextSearch_SearchesTitleDescriptionTags()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?q=toolkit");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().HaveCount(1); // AI Toolkit article
        items![0].Title.Should().Contain("Toolkit");
    }

    [Fact]
    public async Task FilterContent_TextSearchWithSectionFilter_CombinesFilters()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?sections=ai&q=toolkit");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().HaveCount(1); // AI Toolkit article
        items![0].SectionNames.Should().Contain("ai");
        items[0].Title.Should().Contain("Toolkit");
    }

    [Fact]
    public async Task FilterContent_CaseInsensitiveFiltering()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?sections=AI&collections=NEWS&tags=COPILOT");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().HaveCount(1); // Agentic Memory (case-insensitive)
    }

    [Fact]
    public async Task FilterContent_WithNoMatches_ReturnsEmptyList()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?tags=nonexistent");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().BeEmpty();
    }

    [Fact]
    public async Task FilterContent_GeneratesCorrectUrls()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?collections=blogs");
        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();

        // Assert - URLs should include section context (primary section) and slug
        items.Should().NotBeNull();
        items!.Should().HaveCount(2);
        items.Should().AllSatisfy(item => item.Url.Should().MatchRegex(@"^/[a-z-]+/blogs/20[0-9]{2}-[0-9]{2}-[0-9]{2}-.+$"));
    }

    [Fact]
    public async Task GetAllTags_ReturnsAllUniqueTags()
    {
        // Act
        var response = await _client.GetAsync("/api/tags/all");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<AllTagsResponse>();
        result.Should().NotBeNull();
        result!.Tags.Should().HaveCountGreaterThan(0);
        result.Tags.Select(t => t.Tag).Should().Contain("Copilot");
        result.Tags.Select(t => t.Tag).Should().Contain("Developer Tools");
        result.Tags.Select(t => t.Tag).Should().Contain("VS Code");
        result.Tags.Select(t => t.Tag).Should().OnlyHaveUniqueItems();
        result.Tags.All(t => t.Count > 0).Should().BeTrue("all tags should have counts");
    }

    [Theory]
    [InlineData("?sections=ai", 5)] // 5 items with 'ai' section
    [InlineData("?sections=github-copilot", 5)] // 5 items with 'github-copilot' section (includes roundup)
    [InlineData("?collections=news", 3)] // 3 news items
    [InlineData("?collections=videos", 1)] // 1 video item
    [InlineData("?collections=blogs", 2)] // 2 blog items
    [InlineData("?tags=Developer Tools", 3)] // 3 items with 'Developer Tools' tag
    public async Task FilterContent_VariousCriteria_ReturnsExpectedCounts(string queryString, int expectedCount)
    {
        // Act
        var response = await _client.GetAsync($"/api/content/filter{queryString}");
        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();

        // Assert
        items.Should().HaveCount(expectedCount);
    }

    [Fact]
    public async Task FilterContent_PreservesAllContentItemProperties()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?collections=videos");
        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();

        // Assert
        var item = items![0];
        item.Slug.Should().NotBeNullOrEmpty();
        item.Title.Should().NotBeNullOrEmpty();
        item.Author.Should().NotBeNullOrEmpty();
        item.DateEpoch.Should().BeGreaterThan(0);
        item.DateIso.Should().NotBeNullOrEmpty();
        item.CollectionName.Should().NotBeNullOrEmpty();
        item.SectionNames.Should().NotBeEmpty();
        item.Tags.Should().NotBeEmpty();
        item.Excerpt.Should().NotBeNullOrEmpty();
        item.Url.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetContent_MapsSubcollectionNameAndFeedName()
    {
        // Arrange - test video has SubcollectionName="vscode-updates" and FeedName="Test Feed"

        // Act
        var response = await _client.GetAsync("/api/content?collectionName=videos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().HaveCount(1); // Only 1 video in test data

        var videoItem = items[0];
        videoItem.CollectionName.Should().Be("videos");
        videoItem.SubcollectionName.Should().BeNull("Test video doesn't have a subcollection");
        videoItem.FeedName.Should().Be("Visual Studio Code YouTube", "FeedName should be mapped from ContentItem");
    }
}
