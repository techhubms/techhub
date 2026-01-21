using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.Core.DTOs;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for Content filtering API endpoints
/// Tests advanced filtering and tag retrieval with mocked dependencies
/// </summary>
public class ContentEndpointsTests : IClassFixture<TechHubApiFactory>
{
    private readonly TechHubApiFactory _factory;
    private readonly HttpClient _client;

    public ContentEndpointsTests(TechHubApiFactory factory)
    {
        _factory = factory;
        _factory.SetupDefaultSections();
        _factory.SetupDefaultContent();
        _client = _factory.CreateClient();
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
        items!.Should().HaveCount(4); // All 4 test items
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
        items!.Should().HaveCount(2); // 2 AI items
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
        items!.Should().HaveCount(4); // All items (2 AI + 2 GitHub Copilot)
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
        items!.Should().HaveCount(2); // 2 news items
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
        items!.Should().HaveCount(3); // 2 news + 1 blog
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
        items!.Should().HaveCount(1); // Only AI news item
        items![0].CollectionName.Should().Be("news");
        items[0].SectionNames.Should().Contain("ai");
    }

    [Fact]
    public async Task FilterContent_BySingleTag_ReturnsItemsWithTag()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?tags=github copilot");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().HaveCount(3); // 3 items with github copilot tag
        items.Should().AllSatisfy(item => item.Tags.Should().Contain("github copilot"));
    }

    [Fact]
    public async Task FilterContent_ByMultipleTags_RequiresAllTags()
    {
        // Act - Looking for items with BOTH github copilot AND azure tags
        var response = await _client.GetAsync("/api/content/filter?tags=github copilot,azure");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().HaveCount(1); // Only 1 item has both tags
        items![0].Tags.Should().Contain("github copilot");
        items[0].Tags.Should().Contain("azure");
    }

    [Fact]
    public async Task FilterContent_ComplexFilter_CombinesAllCriteria()
    {
        // Act - AI section + news collection + github copilot tag
        var response = await _client.GetAsync("/api/content/filter?sections=ai&collections=news&tags=github copilot");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().HaveCount(1);
        items![0].Slug.Should().Be("2024-01-15-ai-news-1");
        items[0].CollectionName.Should().Be("news");
        items[0].SectionNames.Should().Contain("ai");
        items[0].Tags.Should().Contain("github copilot");
    }

    [Fact]
    public async Task FilterContent_ByTextSearch_SearchesTitleDescriptionTags()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?q=video");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().HaveCount(1); // 1 item with "video" in title/description
        items![0].Title.Should().Contain("Video");
    }

    [Fact]
    public async Task FilterContent_TextSearchWithSectionFilter_CombinesFilters()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?sections=github-copilot&q=vs code");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().HaveCount(1);
        items![0].SectionNames.Should().Contain("github-copilot");
        items[0].Tags.Should().Contain("vs code");
    }

    [Fact]
    public async Task FilterContent_CaseInsensitiveFiltering()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?sections=AI&collections=NEWS&tags=GITHUB COPILOT");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().HaveCount(1); // Case-insensitive matching should work
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

        // Assert - URLs should include section context (primary section based on Sections property) and be lowercase 
        items![0].Url.Should().Be("/ai/blogs/2024-01-16-ai-blog-1");
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
        result.Tags.Select(t => t.Tag).Should().Contain("github copilot");
        result.Tags.Select(t => t.Tag).Should().Contain("ai");
        result.Tags.Select(t => t.Tag).Should().Contain("vs code");
        result.Tags.Select(t => t.Tag).Should().OnlyHaveUniqueItems();
        result.Tags.All(t => t.Count > 0).Should().BeTrue("all tags should have counts");
    }

    [Theory]
    [InlineData("?sections=ai", 2)]
    [InlineData("?sections=github-copilot", 2)]
    [InlineData("?collections=news", 2)]
    [InlineData("?collections=videos", 1)]
    [InlineData("?tags=news", 2)]
    [InlineData("?tags=productivity", 1)]
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
        videoItem.SubcollectionName.Should().Be("vscode-updates", "SubcollectionName should be mapped from ContentItem");
        videoItem.FeedName.Should().Be("Test Feed", "FeedName should be mapped from ContentItem");
    }
}
