using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.Core.Models;
using TechHub.TestUtilities;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for Section API endpoints
/// Tests all 6 section endpoints with mocked file system dependencies
/// </summary>
public class SectionsEndpointsTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly HttpClient _client;

    public SectionsEndpointsTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllSections_ReturnsAllSections()
    {
        // Act
        var response = await _client.GetAsync("/api/sections");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var sections = await response.Content.ReadFromJsonAsync<List<Section>>();
        sections.Should().NotBeNull();
        sections!.Should().HaveCount(8);
        sections.Should().Contain(s => s.Name == "all");
        sections.Should().Contain(s => s.Name == "ai");
        sections.Should().Contain(s => s.Name == "github-copilot");
        sections.Should().Contain(s => s.Name == "ml");
        sections.Should().Contain(s => s.Name == "devops");
        sections.Should().Contain(s => s.Name == "azure");
        sections.Should().Contain(s => s.Name == "coding");
        sections.Should().Contain(s => s.Name == "security");
    }

    [Fact]
    public async Task GetAllSections_ReturnsCorrectStructure()
    {
        // Act
        var response = await _client.GetAsync("/api/sections");
        var sections = await response.Content.ReadFromJsonAsync<List<Section>>();

        // Assert
        var aiSection = sections!.First(s => s.Name == "ai");
        aiSection.Title.Should().Be("Artificial Intelligence");
        aiSection.Description.Should().Be("Your gateway to the AI revolution. From breakthrough announcements to practical tutorials, explore how artificial intelligence is reshaping the way we work and create.");
        aiSection.Url.Should().Be("/ai");
        aiSection.Name.Should().Be("ai");
        aiSection.Collections.Should().HaveCount(8);
        aiSection.Collections.Should().Contain(c => c.Name == "news");
        aiSection.Collections.Should().Contain(c => c.Name == "blogs");
        aiSection.Collections.Should().Contain(c => c.Name == "videos");
        aiSection.Collections.Should().Contain(c => c.Name == "community");
        aiSection.Collections.Should().Contain(c => c.Name == "genai-basics");
        aiSection.Collections.Should().Contain(c => c.Name == "genai-advanced");
        aiSection.Collections.Should().Contain(c => c.Name == "genai-applied");
        aiSection.Collections.Should().Contain(c => c.Name == "sdlc");
    }

    [Fact]
    public async Task GetSectionByName_WithValidName_ReturnsSection()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/ai");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var section = await response.Content.ReadFromJsonAsync<Section>();
        section.Should().NotBeNull();
        section!.Name.Should().Be("ai");
        section.Title.Should().Be("Artificial Intelligence");
        section.Collections.Should().HaveCount(8);
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

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty(); // AI section has items
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

        var collections = await response.Content.ReadFromJsonAsync<List<Collection>>();
        collections.Should().NotBeNull();
        collections!.Should().HaveCount(8);
        collections.Should().Contain(c => c.Name == "news");
        collections.Should().Contain(c => c.Name == "blogs");
        collections.Should().Contain(c => c.Name == "videos");
        collections.Should().Contain(c => c.Name == "community");
        collections.Should().Contain(c => c.Name == "features");
        collections.Should().Contain(c => c.Name == "vscode-updates");
        collections.Should().Contain(c => c.Name == "levels-of-enlightenment");
        collections.Should().Contain(c => c.Name == "handbook");
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

        var collection = await response.Content.ReadFromJsonAsync<Collection>();
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

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty(); // AI news collection has items
        items.Should().AllSatisfy(item =>
        {
            item.CollectionName.Should().Be("news");
        });
    }

    [Fact]
    public async Task GetSectionCollectionItems_FiltersCorrectly()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/github-copilot/collections/videos/items");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty(); // GitHub Copilot videos collection has items
        items.Should().AllSatisfy(item =>
        {
            // Videos collection includes subcollections (ghc-features, vscode-updates)
            var validCollections = new[] { "videos", "ghc-features", "vscode-updates" };
            item.CollectionName.Should().BeOneOf(validCollections);
        });
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
    public async Task GetSectionCollectionItems_GeneratesCorrectUrls_ForInternalCollections()
    {
        // Act - Use videos collection in github-copilot section (links internally, not news/blogs/community which link externally)
        var response = await _client.GetAsync("/api/sections/github-copilot/collections/videos/items");
        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();

        // Assert - Internal collections generate URLs with primary section and slug WITHOUT date prefix
        // All URL components are lowercase
        items.Should().NotBeEmpty("github-copilot section should have videos");
        items!.Should().AllSatisfy(item =>
        {
            item.GetHref().Should().MatchRegex(@"^/[a-z-]+/videos/[a-z0-9-]+$",
                "URL should include primary section, collection, and slug without date prefix");
        });
    }

    [Fact]
    public async Task GetSectionCollectionItems_ReturnsExternalUrls_ForExternalCollections()
    {
        // Act - News collection links externally
        var response = await _client.GetAsync("/api/sections/ai/collections/news/items");
        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();

        // Assert - External collections (news, blogs, community) return ExternalUrl
        items.Should().NotBeEmpty();
        items!.Should().AllSatisfy(item =>
        {
            item.LinksExternally().Should().BeTrue("news items should link externally");
            item.GetHref().Should().StartWith("https://", "external URLs should be full HTTPS URLs");
        });
    }

    [Theory]
    [InlineData("ai", "ai")]
    [InlineData("github-copilot", "github-copilot")]
    public async Task GetSectionByName_ReturnsCorrectSection(string sectionName, string expectedSection)
    {
        // Act
        var response = await _client.GetAsync($"/api/sections/{sectionName}");
        var section = await response.Content.ReadFromJsonAsync<Section>();

        // Assert
        section!.Name.Should().Be(expectedSection);
    }

    [Fact]
    public async Task GetSectionItems_ShouldNotIncludeDraftItems()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/ai/items");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();

        // Should not include draft items
        items!.Should().NotContain(item => item.Draft, "section items endpoint should filter out drafts");
    }

    [Fact]
    public async Task GetSectionCollectionItems_ShouldNotIncludeDraftItems()
    {
        // Act - News collection in AI section (our draft is ai + news)
        var response = await _client.GetAsync("/api/sections/ai/collections/news/items");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();

        // Should not include draft news items
        items!.Should().NotContain(item => item.Draft, "section collection items endpoint should filter out drafts");
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

        var sections = await response.Content.ReadFromJsonAsync<List<Section>>();
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
