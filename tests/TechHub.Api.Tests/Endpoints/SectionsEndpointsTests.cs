using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.Core.DTOs;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for Section API endpoints
/// Tests all 6 section endpoints with mocked file system dependencies
/// </summary>
public class SectionsEndpointsTests : IClassFixture<TechHubApiFactory>
{
    private readonly TechHubApiFactory _factory;
    private readonly HttpClient _client;

    public SectionsEndpointsTests(TechHubApiFactory factory)
    {
        _factory = factory;
        _factory.SetupDefaultSections();
        _factory.SetupDefaultContent();
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetAllSections_ReturnsAllSections()
    {
        // Act
        var response = await _client.GetAsync("/api/sections");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var sections = await response.Content.ReadFromJsonAsync<List<SectionDto>>();
        sections.Should().NotBeNull();
        sections!.Should().HaveCount(2);
        sections.Should().Contain(s => s.Name == "ai");
        sections.Should().Contain(s => s.Name == "github-copilot");
    }

    [Fact]
    public async Task GetAllSections_ReturnsCorrectStructure()
    {
        // Act
        var response = await _client.GetAsync("/api/sections");
        var sections = await response.Content.ReadFromJsonAsync<List<SectionDto>>();

        // Assert
        var aiSection = sections!.First(s => s.Name == "ai");
        aiSection.Title.Should().Be("AI");
        aiSection.Description.Should().Be("Artificial Intelligence resources");
        aiSection.Url.Should().Be("/ai");
        aiSection.Name.Should().Be("ai");
        aiSection.BackgroundImage.Should().Be("/assets/section-backgrounds/ai.jpg");
        aiSection.Collections.Should().HaveCount(2);
        aiSection.Collections.Should().Contain(c => c.Name == "news");
        aiSection.Collections.Should().Contain(c => c.Name == "blogs");
    }

    [Fact]
    public async Task GetSectionByName_WithValidName_ReturnsSection()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/ai");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var section = await response.Content.ReadFromJsonAsync<SectionDto>();
        section.Should().NotBeNull();
        section!.Name.Should().Be("ai");
        section.Title.Should().Be("AI");
        section.Collections.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetSectionByName_WithInvalidName_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/invalid");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetSectionItems_WithValidSection_ReturnsItems()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/ai/items");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().HaveCount(2); // 2 items with AI section
        items.Should().AllSatisfy(item => item.SectionNames.Should().Contain("ai"));
    }

    [Fact]
    public async Task GetSectionItems_WithInvalidSection_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/invalid/items");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetSectionCollections_WithValidSection_ReturnsCollections()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/github-copilot/collections");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var collections = await response.Content.ReadFromJsonAsync<List<CollectionReferenceDto>>();
        collections.Should().NotBeNull();
        collections!.Should().HaveCount(2);
        collections.Should().Contain(c => c.Name == "news");
        collections.Should().Contain(c => c.Name == "videos");
    }

    [Fact]
    public async Task GetSectionCollections_WithInvalidSection_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/invalid/collections");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetSectionCollection_WithValidParameters_ReturnsCollection()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/ai/collections/news");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var collection = await response.Content.ReadFromJsonAsync<CollectionReferenceDto>();
        collection.Should().NotBeNull();
        collection!.Name.Should().Be("news");
        collection.Title.Should().Be("News");
        collection.Url.Should().Be("/ai/news");
    }

    [Fact]
    public async Task GetSectionCollection_WithInvalidSection_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/invalid/collections/news");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetSectionCollection_WithInvalidCollection_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/ai/collections/invalid");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetSectionCollectionItems_WithValidParameters_ReturnsItems()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/ai/collections/news/items");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().HaveCount(1); // 1 AI news item
        items![0].CollectionName.Should().Be("news");
        items[0].SectionNames.Should().Contain("ai");
    }

    [Fact]
    public async Task GetSectionCollectionItems_FiltersCorrectly()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/github-copilot/collections/videos/items");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().HaveCount(1);
        items![0].CollectionName.Should().Be("videos");
        items[0].SectionNames.Should().Contain("github-copilot");
    }

    [Fact]
    public async Task GetSectionCollectionItems_WithInvalidSection_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/invalid/collections/news/items");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetSectionCollectionItems_WithInvalidCollection_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/ai/collections/invalid/items");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetSectionCollectionItems_GeneratesCorrectUrls()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/ai/collections/news/items");
        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();

        // Assert - URLs should include section context and be lowercase 
        items![0].Url.Should().Be("/ai/news/2024-01-15-ai-news-1");
    }

    [Theory]
    [InlineData("ai", "ai")]
    [InlineData("github-copilot", "github-copilot")]
    public async Task GetSectionByName_ReturnsCorrectSection(string sectionName, string expectedSection)
    {
        // Act
        var response = await _client.GetAsync($"/api/sections/{sectionName}");
        var section = await response.Content.ReadFromJsonAsync<SectionDto>();

        // Assert
        section!.Name.Should().Be(expectedSection);
    }

    /// <summary>
    /// INTEGRATION TEST: Verify API can serialize sections correctly through the HTTP layer
    /// Why: Ensures the entire request/response pipeline works, including JSON serialization
    /// This catches issues that might not show up in unit tests but appear when using Swagger/HTTP clients
    /// </summary>
    [Fact]
    public async Task GetAllSections_SerializesCorrectlyThroughHttpPipeline()
    {
        // This test verifies the complete HTTP request/response cycle works correctly
        // The same serialization path is used by Swagger UI

        // Act: Make HTTP request to get sections
        var response = await _client.GetAsync("/api/sections");

        // Assert: Should return success
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Verify Content-Type is JSON
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");

        // Verify we can deserialize the response
        var jsonString = await response.Content.ReadAsStringAsync();
        jsonString.Should().NotBeEmpty();

        var sections = await response.Content.ReadFromJsonAsync<List<SectionDto>>();
        sections.Should().NotBeNull();
        sections.Should().NotBeEmpty();

        // Verify the structure matches what Swagger expects
        foreach (var section in sections!)
        {
            section.Name.Should().NotBeNullOrEmpty();
            section.Title.Should().NotBeNullOrEmpty();
            section.Description.Should().NotBeNullOrEmpty();
            section.Collections.Should().NotBeNull();
        }
    }
}
