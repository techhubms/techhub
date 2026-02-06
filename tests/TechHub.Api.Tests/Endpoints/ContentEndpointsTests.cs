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
public class ContentEndpointsTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly HttpClient _client;

    public ContentEndpointsTests(TechHubIntegrationTestApiFactory factory)
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
        // Act - Use collections/all endpoint to get all items in a section
        var response = await _client.GetAsync("/api/sections/ai/collections/all/items");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty(); // AI section has items
    }

    [Fact]
    public async Task GetSectionItems_WithInvalidSection_ReturnsNotFound()
    {
        // Act - Use collections/all endpoint for invalid section
        var response = await _client.GetAsync("/api/sections/invalid/collections/all/items");

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
        // Act - Use collections/all endpoint to get all items in a section
        var response = await _client.GetAsync("/api/sections/ai/collections/all/items");

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

    #region Tag Cloud Endpoint Tests

    [Fact]
    public async Task GetCollectionTags_WithValidParameters_ReturnsTagCloud()
    {
        // Arrange - Use AI collection with "all" to get section-wide tags (more likely to have results)

        // Act
        var response = await _client.GetAsync("/api/sections/ai/collections/all/tags");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tagCloud = await response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        tagCloud.Should().NotBeNull();

        // Tag cloud may be empty if no content matches the default filters (lastDays, minUses)
        // The important thing is the endpoint returns successfully
        if (tagCloud!.Count > 0)
        {
            // Verify tag cloud structure when tags are present
            tagCloud.Should().AllSatisfy(item =>
            {
                item.Tag.Should().NotBeNullOrEmpty("Tag should have a name");
                item.Count.Should().BeGreaterThan(0, "Tag count should be positive");
                item.Size.Should().BeOneOf(TagSize.Small, TagSize.Medium, TagSize.Large);
            });

            // Verify tags are sorted by count descending (most popular first)
            for (int i = 0; i < tagCloud.Count - 1; i++)
            {
                tagCloud[i].Count.Should().BeGreaterThanOrEqualTo(tagCloud[i + 1].Count,
                    "Tags should be sorted by count descending");
            }
        }
    }

    [Fact]
    public async Task GetCollectionTags_WithAllCollection_ReturnsSectionTagCloud()
    {
        // Arrange - Use "all" collection to get section-wide tag cloud

        // Act
        var response = await _client.GetAsync("/api/sections/github-copilot/collections/all/tags");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tagCloud = await response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        tagCloud.Should().NotBeNull();
        tagCloud!.Should().NotBeEmpty("GitHub Copilot section should have tags across all collections");
    }

    [Fact]
    public async Task GetCollectionTags_WithInvalidSection_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/invalid-section/collections/news/tags");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetCollectionTags_WithInvalidCollection_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/ai/collections/invalid-collection/tags");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetCollectionTags_WithMaxTagsParameter_RespectsLimit()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/ai/collections/news/tags?maxTags=5");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tagCloud = await response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        tagCloud.Should().NotBeNull();
        tagCloud!.Should().HaveCountLessThanOrEqualTo(5, "maxTags parameter should limit results");
    }

    [Fact]
    public async Task GetCollectionTags_WithMinUsesParameter_FiltersLowCountTags()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/ai/collections/news/tags?minUses=3");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tagCloud = await response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        tagCloud.Should().NotBeNull();

        // Verify all tags meet minimum usage threshold
        tagCloud!.Should().AllSatisfy(item =>
        {
            item.Count.Should().BeGreaterThanOrEqualTo(3, "All tags should meet minimum usage threshold");
        });
    }

    [Fact]
    public async Task GetCollectionTags_WithLastDaysParameter_FiltersOldContent()
    {
        // Act - Request tags from last 30 days only
        var response = await _client.GetAsync("/api/sections/ai/collections/news/tags?lastDays=30");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tagCloud = await response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        tagCloud.Should().NotBeNull();

        // Tag cloud should only include tags from recent content
        // The exact tags will vary, but structure should be valid
        tagCloud!.Should().AllSatisfy(item =>
        {
            item.Tag.Should().NotBeNullOrEmpty();
            item.Count.Should().BeGreaterThan(0);
        });
    }

    [Fact]
    public async Task GetCollectionTags_WithMultipleParameters_CombinesFilters()
    {
        // Act - Combine multiple filtering parameters
        var response = await _client.GetAsync("/api/sections/github-copilot/collections/videos/tags?maxTags=3&minUses=2&lastDays=90");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tagCloud = await response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        tagCloud.Should().NotBeNull();
        tagCloud!.Should().HaveCountLessThanOrEqualTo(3, "maxTags should be respected");
        tagCloud!.Should().AllSatisfy(item =>
        {
            item.Count.Should().BeGreaterThanOrEqualTo(2, "minUses should be respected");
        });
    }

    #endregion

    #region Content Detail Endpoint Tests

    [Fact]
    public async Task GetContentDetail_WithValidSlug_ReturnsContentWithRenderedHtml()
    {
        // Arrange - Use roundups collection which links internally (not externally like news/blogs/community)
        var itemsResponse = await _client.GetAsync("/api/sections/all/collections/roundups/items");
        var items = await itemsResponse.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();

        var testItem = items!.First();
        var slug = testItem.Slug;

        // Act
        var response = await _client.GetAsync($"/api/sections/all/collections/roundups/{slug}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var detail = await response.Content.ReadFromJsonAsync<ContentItemDetail>();
        detail.Should().NotBeNull();
        detail!.Slug.Should().Be(slug);
        detail.Title.Should().NotBeNullOrEmpty();
        detail.RenderedHtml.Should().NotBeNullOrEmpty("Content should be rendered to HTML");
        detail.RenderedHtml.Should().Contain("<", "Rendered content should contain HTML tags");
    }

    [Fact]
    public async Task GetContentDetail_WithInvalidSection_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/invalid-section/collections/news/test-slug");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetContentDetail_WithInvalidCollection_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/ai/collections/invalid-collection/test-slug");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetContentDetail_WithInvalidSlug_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/ai/collections/news/non-existent-slug-12345");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetContentDetail_IncludesMetadata()
    {
        // Arrange - Use roundups collection which links internally (not externally like news/blogs/community)
        var itemsResponse = await _client.GetAsync("/api/sections/all/collections/roundups/items");
        var items = await itemsResponse.Content.ReadFromJsonAsync<List<ContentItem>>();
        var testItem = items!.First();

        // Act
        var response = await _client.GetAsync($"/api/sections/all/collections/roundups/{testItem.Slug}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var detail = await response.Content.ReadFromJsonAsync<ContentItemDetail>();
        detail.Should().NotBeNull();

        // Verify all metadata is populated
        detail!.Slug.Should().NotBeNullOrEmpty();
        detail.Title.Should().NotBeNullOrEmpty();
        detail.Author.Should().NotBeNullOrEmpty();
        detail.DateEpoch.Should().BeGreaterThan(0);
        detail.CollectionName.Should().Be("roundups");
        detail.Tags.Should().NotBeNull();
        detail.Excerpt.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetContentDetail_ShouldNotReturnDraftContent()
    {
        // This test verifies that draft content is not accessible via the detail endpoint
        // We can't easily test this without knowing specific draft slugs,
        // but we can verify that any returned content is not marked as draft

        // Arrange - Use roundups collection which links internally (not externally like news/blogs/community)
        var itemsResponse = await _client.GetAsync("/api/sections/all/collections/roundups/items");
        var items = await itemsResponse.Content.ReadFromJsonAsync<List<ContentItem>>();
        var testItem = items!.First();

        // Act
        var response = await _client.GetAsync($"/api/sections/all/collections/roundups/{testItem.Slug}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var detail = await response.Content.ReadFromJsonAsync<ContentItemDetail>();
        detail.Should().NotBeNull();
        detail!.Draft.Should().BeFalse("Detail endpoint should not return draft content");
    }

    [Fact]
    public async Task GetContentDetail_WithExternalUrl_ReturnsNotFound()
    {
        // External collections (news, blogs, community) link to original sources,
        // so detail endpoint returns 404 since there's no internal content to display

        // Arrange - Get a news item (external collection)
        var itemsResponse = await _client.GetAsync("/api/sections/ai/collections/news/items");
        var items = await itemsResponse.Content.ReadFromJsonAsync<List<ContentItem>>();
        var testItem = items!.First();

        // Verify it's actually an external item
        testItem.LinksExternally().Should().BeTrue("News items should link externally");

        // Act - Try to access detail endpoint
        var response = await _client.GetAsync($"/api/sections/ai/collections/news/{testItem.Slug}");

        // Assert - Should return 404 since external items don't have detail pages
        response.StatusCode.Should().Be(HttpStatusCode.NotFound,
            "External collections should return 404 for detail endpoint since they link to original sources");
    }

    #endregion
}
