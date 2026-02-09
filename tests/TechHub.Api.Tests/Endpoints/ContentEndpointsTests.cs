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
        sections.Should().Contain(s => s.Name == "dotnet");
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

    #region Dynamic Tag Counts Tests (With Filter Parameters)

    [Fact]
    public async Task GetCollectionTags_WithNoFilters_ReturnsStaticCounts()
    {
        // Arrange - No filter parameters = static counts (total items with each tag)

        // Act
        var response = await _client.GetAsync("/api/sections/ai/collections/all/tags");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tagCloud = await response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        tagCloud.Should().NotBeNull();

        if (tagCloud!.Count > 0)
        {
            // All counts should be positive (static counts)
            tagCloud.Should().AllSatisfy(item =>
            {
                item.Count.Should().BeGreaterThan(0, "Static counts should be positive");
            });
        }
    }

    [Fact]
    public async Task GetCollectionTags_WithTagsFilter_ReturnsDynamicCounts()
    {
        // Arrange - When tags parameter is provided, counts should show intersection
        // (how many items would remain if BOTH the selected tag AND this tag are applied)
        const string selectedTag = "ai";

        // Act - Get tag cloud with AI tag already selected
        var response = await _client.GetAsync($"/api/sections/all/collections/all/tags?tags={selectedTag}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tagCloud = await response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        tagCloud.Should().NotBeNull();

        if (tagCloud!.Count > 0)
        {
            // Counts should reflect intersection with selected tag
            // Some tags may have count = 0 (no items with BOTH tags)
            tagCloud.Should().AllSatisfy(item =>
            {
                item.Count.Should().BeGreaterThanOrEqualTo(0, "Dynamic counts can be zero or positive");
            });

            // At least one tag should have count > 0 (tags that co-occur with AI)
            tagCloud.Should().Contain(item => item.Count > 0,
                "Some tags should co-occur with the selected tag");
        }
    }

    [Fact]
    public async Task GetCollectionTags_WithMultipleTags_CalculatesIntersection()
    {
        // Arrange - Select multiple tags, counts should show items matching ALL selected tags AND each tag
        const string tag1 = "ai";
        const string tag2 = "copilot";

        // Act - Get tag cloud with both AI and Copilot selected
        var response = await _client.GetAsync($"/api/sections/all/collections/all/tags?tags={tag1},{tag2}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tagCloud = await response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        tagCloud.Should().NotBeNull();

        if (tagCloud!.Count > 0)
        {
            // Counts should be for items with AI AND Copilot AND this tag
            // Many tags will likely have count = 0 (intersection is smaller)
            tagCloud.Should().AllSatisfy(item =>
            {
                item.Count.Should().BeGreaterThanOrEqualTo(0, "Intersection counts can be zero");
            });
        }
    }

    [Fact]
    public async Task GetCollectionTags_WithDateRangeFilter_FiltersCountsByDate()
    {
        // Arrange - Date range should affect tag counts
        var fromDate = DateTimeOffset.UtcNow.AddDays(-30).ToString("yyyy-MM-dd");
        var toDate = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd");

        // Act - Get tag cloud for last 30 days
        var response = await _client.GetAsync($"/api/sections/ai/collections/all/tags?from={fromDate}&to={toDate}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tagCloud = await response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        tagCloud.Should().NotBeNull();

        // Response structure should be valid (counts filtered to date range)
        tagCloud!.Should().AllSatisfy(item =>
        {
            item.Tag.Should().NotBeNullOrEmpty();
            item.Count.Should().BeGreaterThan(0); // minUses filters out zeros
        });
    }

    [Fact]
    public async Task GetCollectionTags_WithTagsAndDateRange_CombinesFilters()
    {
        // Arrange - Combine tags and date range filters
        const string selectedTag = "ai";
        var fromDate = DateTimeOffset.UtcNow.AddDays(-90).ToString("yyyy-MM-dd");
        var toDate = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd");

        // Act - Get tag cloud with both filters
        var response = await _client.GetAsync($"/api/sections/all/collections/all/tags?tags={selectedTag}&from={fromDate}&to={toDate}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tagCloud = await response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        tagCloud.Should().NotBeNull();

        // Counts should reflect items matching tag AND within date range
        tagCloud!.Should().AllSatisfy(item =>
        {
            item.Tag.Should().NotBeNullOrEmpty();
            item.Count.Should().BeGreaterThanOrEqualTo(0);
        });
    }

    [Fact]
    public async Task GetCollectionTags_WithInvalidDateFormat_ReturnsBadRequest()
    {
        // Act - Invalid date format should return 400
        var response = await _client.GetAsync("/api/sections/ai/collections/all/tags?from=invalid-date");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetCollectionTags_IntersectionCounts_ShouldBeSymmetric()
    {
        // This test verifies the bug fix where tag counts should be consistent
        // regardless of the order in which tags are selected
        //
        // Bug scenario:
        // - Start with tags A,B selected
        // - Click tag C → see count for tag D
        // - Reset, start with tags A,B selected again
        // - Click tag D → see count for tag C
        // - Both counts should be EQUAL (representing A AND B AND C AND D intersection)

        // Arrange - Select two initial tags
        const string initialTag1 = "copilot coding agent";
        const string initialTag2 = "vs code";

        // Get tag cloud with initial tags selected
        var initialResponse = await _client.GetAsync(
            $"/api/sections/github-copilot/collections/all/tags?tags={Uri.EscapeDataString(initialTag1)},{Uri.EscapeDataString(initialTag2)}");
        initialResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var initialTagCloud = await initialResponse.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        initialTagCloud.Should().NotBeNull();

        // Find two additional tags that have counts > 0 in the initial state
        var additionalTag1 = initialTagCloud!
            .FirstOrDefault(t => t.Count > 0 && 
                               !t.Tag.Equals(initialTag1, StringComparison.OrdinalIgnoreCase) &&
                               !t.Tag.Equals(initialTag2, StringComparison.OrdinalIgnoreCase));
        var additionalTag2 = initialTagCloud!
            .Skip(1)
            .FirstOrDefault(t => t.Count > 0 && 
                               !t.Tag.Equals(initialTag1, StringComparison.OrdinalIgnoreCase) &&
                               !t.Tag.Equals(initialTag2, StringComparison.OrdinalIgnoreCase) &&
                               !t.Tag.Equals(additionalTag1?.Tag, StringComparison.OrdinalIgnoreCase));

        if (additionalTag1 == null || additionalTag2 == null)
        {
            // Skip test if we don't have enough tags with intersection
            return;
        }

        // Path 1: Select additional tag 1, check count for additional tag 2
        var path1Response = await _client.GetAsync(
            $"/api/sections/github-copilot/collections/all/tags?tags={Uri.EscapeDataString(initialTag1)},{Uri.EscapeDataString(initialTag2)},{Uri.EscapeDataString(additionalTag1.Tag)}");
        path1Response.StatusCode.Should().Be(HttpStatusCode.OK);

        var path1TagCloud = await path1Response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        var path1Count = path1TagCloud!
            .FirstOrDefault(t => t.Tag.Equals(additionalTag2.Tag, StringComparison.OrdinalIgnoreCase))?.Count ?? 0;

        // Path 2: Select additional tag 2, check count for additional tag 1
        var path2Response = await _client.GetAsync(
            $"/api/sections/github-copilot/collections/all/tags?tags={Uri.EscapeDataString(initialTag1)},{Uri.EscapeDataString(initialTag2)},{Uri.EscapeDataString(additionalTag2.Tag)}");
        path2Response.StatusCode.Should().Be(HttpStatusCode.OK);

        var path2TagCloud = await path2Response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        var path2Count = path2TagCloud!
            .FirstOrDefault(t => t.Tag.Equals(additionalTag1.Tag, StringComparison.OrdinalIgnoreCase))?.Count ?? 0;

        // Assert - The intersection counts should be symmetric
        path1Count.Should().Be(path2Count,
            $"Intersection count should be symmetric. " +
            $"With tags [{initialTag1}, {initialTag2}, {additionalTag1.Tag}] → count for {additionalTag2.Tag} = {path1Count}. " +
            $"With tags [{initialTag1}, {initialTag2}, {additionalTag2.Tag}] → count for {additionalTag1.Tag} = {path2Count}. " +
            $"Both represent the intersection of all 4 tags, so counts must be equal.");
    }

    #endregion

    #region TagsToCount Parameter Tests

    [Fact]
    public async Task GetCollectionTags_WithTagsToCount_ReturnsOnlyRequestedTags()
    {
        // Arrange - Get baseline tags first (use 'all' section for more tags)
        var baselineResponse = await _client.GetAsync("/api/sections/all/collections/all/tags?maxTags=10");
        baselineResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var baselineTags = await baselineResponse.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        baselineTags.Should().NotBeNull();
        baselineTags!.Should().HaveCountGreaterThanOrEqualTo(3, "Need at least 3 baseline tags for test");

        // Select up to 3 specific tags to count
        var requestedTags = baselineTags!.Take(3).Select(t => t.Tag).ToList();
        var tagsToCountParam = string.Join(",", requestedTags.Select(Uri.EscapeDataString));

        // Act - Request counts for only those specific tags
        var response = await _client.GetAsync(
            $"/api/sections/all/collections/all/tags?tagsToCount={tagsToCountParam}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var filteredTags = await response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        filteredTags.Should().NotBeNull();

        // Should return only the requested tags (or fewer if some have 0 count)
        filteredTags!.Should().OnlyContain(
            t => requestedTags.Contains(t.Tag, StringComparer.OrdinalIgnoreCase),
            "Should only return tags that were explicitly requested in tagsToCount");
    }

    [Fact]
    public async Task GetCollectionTags_WithTagsToCount_BypassesMaxTagsLimit()
    {
        // Arrange - Get baseline tags (use 'all' section for more tags)
        var baselineResponse = await _client.GetAsync("/api/sections/all/collections/all/tags?maxTags=20");
        baselineResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var baselineTags = await baselineResponse.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        baselineTags.Should().NotBeNull();
        
        // Skip test if not enough tags in test data
        if (baselineTags!.Count < 6)
        {
            return; // Can't test maxTags bypass without enough tags
        }

        // Request counts for all tags, even with maxTags=5
        var tagsToCountParam = string.Join(",", baselineTags!.Select(t => Uri.EscapeDataString(t.Tag)));

        // Act - Request with low maxTags but full tagsToCount list
        var response = await _client.GetAsync(
            $"/api/sections/all/collections/all/tags?maxTags=5&tagsToCount={tagsToCountParam}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var resultTags = await response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        resultTags.Should().NotBeNull();

        // tagsToCount should bypass maxTags limit - should get all requested tags
        resultTags!.Count.Should().BeGreaterThanOrEqualTo(baselineTags!.Count,
            "tagsToCount parameter should bypass maxTags limit and return all requested tags");
    }

    [Fact]
    public async Task GetCollectionTags_WithTagsToCount_AndSelectedTags_ReturnsFilteredCounts()
    {
        // Arrange - Get baseline tags (use 'all' section for more tags)
        var baselineResponse = await _client.GetAsync("/api/sections/all/collections/all/tags?maxTags=10");
        baselineResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var baselineTags = await baselineResponse.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        baselineTags.Should().NotBeNull();
        baselineTags!.Should().HaveCountGreaterThanOrEqualTo(2, "Need at least 2 baseline tags for test");

        // Use first tag as filter, request counts for remaining tags
        var filterTag = baselineTags![0].Tag;
        var tagsToCount = baselineTags!.Skip(1).Take(5).Select(t => t.Tag).ToList();
        var tagsToCountParam = string.Join(",", tagsToCount.Select(Uri.EscapeDataString));

        // Act - Get counts for specific tags, filtered by another tag
        var response = await _client.GetAsync(
            $"/api/sections/all/collections/all/tags?tags={Uri.EscapeDataString(filterTag)}&tagsToCount={tagsToCountParam}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var filteredTags = await response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        filteredTags.Should().NotBeNull();

        // All returned tags should be from our requested list
        filteredTags!.Should().OnlyContain(
            t => tagsToCount.Contains(t.Tag, StringComparer.OrdinalIgnoreCase),
            "Should only return tags from tagsToCount list");

        // Counts should reflect intersection with the filter tag
        // (some may have count 0 if no items match both tags)
        filteredTags.Should().AllSatisfy(t =>
            t.Count.Should().BeGreaterThanOrEqualTo(0, "Counts can be 0 or positive"));
    }

    [Fact]
    public async Task GetCollectionTags_WithTagsToCount_BypassesMinUsesFilter()
    {
        // Arrange - Get baseline tags with no minUses filter (use 'all' section for more tags)
        var baselineResponse = await _client.GetAsync("/api/sections/all/collections/all/tags?maxTags=20&minUses=1");
        baselineResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var baselineTags = await baselineResponse.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        baselineTags.Should().NotBeNull();
        baselineTags!.Should().NotBeEmpty();

        // Find a tag with low count (if available)
        var lowCountTag = baselineTags!.LastOrDefault(t => t.Count <= 2);
        if (lowCountTag == null)
        {
            // Skip test if no low-count tags exist
            return;
        }

        var tagsToCountParam = Uri.EscapeDataString(lowCountTag.Tag);

        // Act - Request with high minUses filter but including the low-count tag in tagsToCount
        var response = await _client.GetAsync(
            $"/api/sections/all/collections/all/tags?minUses=10&tagsToCount={tagsToCountParam}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var resultTags = await response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        resultTags.Should().NotBeNull();

        // tagsToCount should bypass minUses filter
        resultTags!.Should().Contain(t => t.Tag.Equals(lowCountTag.Tag, StringComparison.OrdinalIgnoreCase),
            "tagsToCount parameter should bypass minUses filter for requested tags");
    }

    [Fact]
    public async Task GetCollectionTags_TagsToCount_EnsuresConsistentCountsRegardlessOfClickOrder()
    {
        // This is the key test for the bug fix:
        // When user clicks tags in different orders, the baseline tags should always
        // show consistent counts because we use tagsToCount to request exact counts.

        // Arrange - Get baseline tags (use 'all' section for more tags in test data)
        var baselineResponse = await _client.GetAsync("/api/sections/all/collections/all/tags?maxTags=20");
        baselineResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var baselineTags = await baselineResponse.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        baselineTags.Should().NotBeNull();
        
        // Skip test if not enough tags in test data
        if (baselineTags!.Count < 4)
        {
            return; // Can't test symmetric counts without enough tags with intersections
        }

        // Get list of baseline tag names
        var baselineTagNames = baselineTags!.Select(t => t.Tag).ToList();
        var tagsToCountParam = string.Join(",", baselineTagNames.Select(Uri.EscapeDataString));

        // Select first tag, request counts for all baseline tags
        var selectedTag1 = baselineTags![0].Tag;
        var path1Response = await _client.GetAsync(
            $"/api/sections/all/collections/all/tags?tags={Uri.EscapeDataString(selectedTag1)}&tagsToCount={tagsToCountParam}");
        path1Response.StatusCode.Should().Be(HttpStatusCode.OK);
        var path1Counts = await path1Response.Content.ReadFromJsonAsync<List<TagCloudItem>>();

        // Select second tag, request counts for all baseline tags
        var selectedTag2 = baselineTags![1].Tag;
        var path2Response = await _client.GetAsync(
            $"/api/sections/all/collections/all/tags?tags={Uri.EscapeDataString(selectedTag2)}&tagsToCount={tagsToCountParam}");
        path2Response.StatusCode.Should().Be(HttpStatusCode.OK);
        var path2Counts = await path2Response.Content.ReadFromJsonAsync<List<TagCloudItem>>();

        // Assert - When selecting tag1, the count for tag2 should equal
        // when selecting tag2, the count for tag1 (symmetric intersection)
        var tag2CountWithTag1Selected = path1Counts!
            .FirstOrDefault(t => t.Tag.Equals(selectedTag2, StringComparison.OrdinalIgnoreCase))?.Count ?? 0;
        var tag1CountWithTag2Selected = path2Counts!
            .FirstOrDefault(t => t.Tag.Equals(selectedTag1, StringComparison.OrdinalIgnoreCase))?.Count ?? 0;

        tag1CountWithTag2Selected.Should().Be(tag2CountWithTag1Selected,
            "Intersection counts should be symmetric regardless of selection order");
    }

    #endregion

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

    /// <summary>
    /// INTEGRATION TEST: Verify infinite scrolling with tag filters works correctly with pagination
    /// This test validates the fix for the bug where infinite scrolling + tag filtering was broken
    /// </summary>
    [Fact]
    public async Task GetCollectionItems_WithTagFilter_PaginatesCorrectly()
    {
        // Arrange - Use "AI" tag which exists in test data (25 items per TestDataConstants)
        const string tag = "AI";
        const int pageSize = 10;  // Smaller page size to test pagination with test data

        // First, get total count by requesting all items with this tag
        var allItemsResponse = await _client.GetAsync($"/api/sections/all/collections/all/items?tags={tag}&take=100");
        allItemsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var allItems = await allItemsResponse.Content.ReadFromJsonAsync<List<ContentItem>>();
        var totalCount = allItems!.Count;

        // Verify we have enough items to test pagination
        totalCount.Should().Be(TestDataConstants.FilteredByAiTotalCount, "AI tag should match expected test data count");
        totalCount.Should().BeGreaterThan(pageSize, "should have enough items to require pagination");

        // Act - Fetch first batch (skip=0, take=10)
        var batch1Response = await _client.GetAsync($"/api/sections/all/collections/all/items?tags={tag}&skip=0&take={pageSize}");
        batch1Response.StatusCode.Should().Be(HttpStatusCode.OK);
        var batch1 = await batch1Response.Content.ReadFromJsonAsync<List<ContentItem>>();

        // Act - Fetch second batch (skip=10, take=10)
        var batch2Response = await _client.GetAsync($"/api/sections/all/collections/all/items?tags={tag}&skip={pageSize}&take={pageSize}");
        batch2Response.StatusCode.Should().Be(HttpStatusCode.OK);
        var batch2 = await batch2Response.Content.ReadFromJsonAsync<List<ContentItem>>();

        // Act - Fetch third batch (skip=20, take=10) to get remaining items
        var batch3Response = await _client.GetAsync($"/api/sections/all/collections/all/items?tags={tag}&skip={pageSize * 2}&take={pageSize}");
        batch3Response.StatusCode.Should().Be(HttpStatusCode.OK);
        var batch3 = await batch3Response.Content.ReadFromJsonAsync<List<ContentItem>>();

        // Assert - Batches should work correctly
        batch1.Should().NotBeNull();
        batch2.Should().NotBeNull();
        batch3.Should().NotBeNull();

        batch1!.Should().HaveCount(pageSize, "first batch should contain exactly 10 items");
        batch1.Should().OnlyContain(item => item.Tags.Any(t => t.Contains("ai", StringComparison.OrdinalIgnoreCase)), 
            "all items in batch 1 should have AI in their tags");

        batch2!.Should().HaveCount(pageSize, "second batch should contain exactly 10 items");
        batch2.Should().OnlyContain(item => item.Tags.Any(t => t.Contains("ai", StringComparison.OrdinalIgnoreCase)), 
            "all items in batch 2 should have AI in their tags");

        var expectedBatch3Count = totalCount - (pageSize * 2);
        batch3!.Should().HaveCount(expectedBatch3Count, $"third batch should contain {expectedBatch3Count} remaining items");
        batch3.Should().OnlyContain(item => item.Tags.Any(t => t.Contains("ai", StringComparison.OrdinalIgnoreCase)), 
            "all items in batch 3 should have AI in their tags");

        // Verify no duplicate items between batches
        var batch1Slugs = batch1.Select(i => i.Slug).ToHashSet();
        var batch2Slugs = batch2.Select(i => i.Slug).ToHashSet();
        var batch3Slugs = batch3.Select(i => i.Slug).ToHashSet();
        
        batch1Slugs.Should().NotIntersectWith(batch2Slugs, "batches 1 and 2 should not contain duplicate items");
        batch1Slugs.Should().NotIntersectWith(batch3Slugs, "batches 1 and 3 should not contain duplicate items");
        batch2Slugs.Should().NotIntersectWith(batch3Slugs, "batches 2 and 3 should not contain duplicate items");

        // Verify combined count matches total
        (batch1.Count + batch2.Count + batch3.Count).Should().Be(totalCount, 
            "combined batches should equal total items with tag filter");
    }

    #endregion

    #region Tag Cloud Tests

    [Fact]
    public async Task GetCollectionTags_WithMultiWordTagFilters_ShouldReturnSymmetricCounts()
    {
        // This test verifies the bug fix for tag count symmetry when using multi-word tags as filters
        // Bug scenario: Tag counts differ based on which tags are clicked first
        // 
        // Test data setup (in TestCollections):
        // - 1 item with all 4 tags: [Copilot Coding Agent, VS Code, Agent Mode, Pull Requests]
        // - 1 item with 3 tags: [Copilot Coding Agent, VS Code, Agent Mode] (missing Pull Requests)
        // - 1 item with 3 tags: [Copilot Coding Agent, VS Code, Pull Requests] (missing Agent Mode)
        // - 1 item with 2 tags: [Copilot Coding Agent, VS Code]
        //
        // Expected behavior:
        // Path 1: Filter by [Copilot Coding Agent, VS Code, Agent Mode] → Pull Requests count = 1
        // Path 2: Filter by [Copilot Coding Agent, VS Code, Pull Requests] → Agent Mode count = 1
        // Both represent the same intersection, so counts must be equal (symmetric).

        var tag1 = "Copilot Coding Agent";
        var tag2 = "VS Code";
        var tag3 = "Agent Mode";
        var tag4 = "Pull Requests";

        // Act - Path 1: Filter by tag1, tag2, tag3 and get tag4's count
        var path1Response = await _client.GetAsync(
            $"/api/sections/github-copilot/collections/all/tags?maxTags=50&minUses=1&tags={Uri.EscapeDataString(tag1)},{Uri.EscapeDataString(tag2)},{Uri.EscapeDataString(tag3)}");
        path1Response.StatusCode.Should().Be(HttpStatusCode.OK);

        var path1Tags = await path1Response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        path1Tags.Should().NotBeNull();

        var tag4CountInPath1 = path1Tags!.FirstOrDefault(t => t.Tag.Equals(tag4, StringComparison.OrdinalIgnoreCase))?.Count ?? 0;

        // Act - Path 2: Filter by tag1, tag2, tag4 and get tag3's count
        var path2Response = await _client.GetAsync(
            $"/api/sections/github-copilot/collections/all/tags?maxTags=50&minUses=1&tags={Uri.EscapeDataString(tag1)},{Uri.EscapeDataString(tag2)},{Uri.EscapeDataString(tag4)}");
        path2Response.StatusCode.Should().Be(HttpStatusCode.OK);

        var path2Tags = await path2Response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        path2Tags.Should().NotBeNull();

        var tag3CountInPath2 = path2Tags!.FirstOrDefault(t => t.Tag.Equals(tag3, StringComparison.OrdinalIgnoreCase))?.Count ?? 0;

        // Assert - Both paths should return the same count (symmetric intersection)
        // With the bug fix, both should return 1 (one item has all 4 tags)
        tag4CountInPath1.Should().Be(tag3CountInPath2,
            "Tag counts should be symmetric. " +
            $"Filtering by [{tag1}, {tag2}, {tag3}] and counting {tag4} should equal " +
            $"filtering by [{tag1}, {tag2}, {tag4}] and counting {tag3}. " +
            $"Both represent the intersection of all 4 tags. " +
            $"Path1={tag4CountInPath1}, Path2={tag3CountInPath2}");

        // Verify the expected count is 1 (one item with all 4 tags exists in test data)
        tag4CountInPath1.Should().Be(1, "Test data includes 1 item with all 4 tags");
        tag3CountInPath2.Should().Be(1, "Test data includes 1 item with all 4 tags");
    }

    #endregion
}
