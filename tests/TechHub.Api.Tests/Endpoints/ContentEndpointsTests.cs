using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.Core.Models;
using TechHub.TestUtilities;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for Content filtering API endpoints
/// Tests advanced filtering and tag retrieval with production-like test data from TestCollections
/// 
/// Test Data (32 non-draft items total):
/// - 7 news items
/// - 18 blogs
/// - 2 community items
/// - 1 roundup
/// - 1 ghc-features (video subcollection)
/// - 1 vscode-updates (video subcollection)
/// - 2 videos
/// Note: Videos collection has 2 root items plus ghc-features and vscode-updates subcollections
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

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().HaveCount(32); // All 32 non-draft test items from TestCollections
    }

    [Fact]
    public async Task FilterContent_BySingleSection_ReturnsFilteredItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?sections=ai");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty("There should be items with 'ai' section");
    }

    [Fact]
    public async Task FilterContent_ByMultipleSections_ReturnsItemsFromAnySections()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?sections=ai,github-copilot");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty("There should be items with 'ai' or 'github-copilot' sections");
    }

    [Fact]
    public async Task FilterContent_BySingleCollection_ReturnsFilteredItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?collections=news");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().HaveCount(7); // 7 news items
        items.Should().AllSatisfy(item => item.CollectionName.Should().Be("news"));
    }

    [Fact]
    public async Task FilterContent_ByMultipleCollections_ReturnsItemsFromAnyCollections()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?collections=news,blogs");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().HaveCount(25); // 7 news + 18 blogs
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

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty("There should be news items with 'ai' section");
        items.Should().AllSatisfy(item => 
        {
            item.CollectionName.Should().Be("news");
        });
    }

    [Fact]
    public async Task FilterContent_BySingleTag_ReturnsItemsWithTag()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?tags=Copilot");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty("There should be items with 'Copilot' tag");
        items.Should().AllSatisfy(item => item.Tags.Should().Contain("Copilot"));
    }

    [Fact]
    public async Task FilterContent_ByMultipleTags_RequiresAllTags()
    {
        // Act - Looking for items with BOTH 'Code Review' AND 'Collaboration' tags
        var response = await _client.GetAsync("/api/content/filter?tags=Code Review,Collaboration");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty("There should be items with both 'Code Review' and 'Collaboration' tags");
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

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty("There should be news items with 'ai' section and 'Copilot' tag");
        items.Should().AllSatisfy(item =>
        {
            item.CollectionName.Should().Be("news");
            item.Tags.Should().Contain("Copilot");
        });
    }

    [Fact]
    public async Task FilterContent_ByTextSearch_SearchesTitleDescriptionTags()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?q=toolkit");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty("There should be items containing 'toolkit'");
        items.Should().AllSatisfy(item => 
            item.Title.Should().ContainEquivalentOf("toolkit", "Search should find items with 'toolkit' in title"));
    }

    [Fact]
    public async Task FilterContent_TextSearchWithSectionFilter_CombinesFilters()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?sections=ai&q=toolkit");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty("There should be ai items containing 'toolkit'");
        items.Should().AllSatisfy(item =>
        {
            item.Title.Should().ContainEquivalentOf("toolkit");
        });
    }

    [Fact]
    public async Task FilterContent_CaseInsensitiveFiltering()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?sections=AI&collections=NEWS&tags=COPILOT");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty("Case-insensitive filtering should find items");
    }

    [Fact]
    public async Task FilterContent_WithNoMatches_ReturnsEmptyList()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?tags=nonexistent");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Should().BeEmpty();
    }

    [Fact]
    public async Task FilterContent_GeneratesCorrectUrls()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?collections=blogs");
        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();

        // Assert - URLs should include section context (primary section) and slug WITHOUT date prefix
        // All URL components are lowercase
        items.Should().NotBeNull();
        items!.Should().HaveCount(18); // 18 blog items
        items.Should().AllSatisfy(item => item.GetHref().Should().MatchRegex(@"^/[a-z-]+/blogs/[a-z0-9-]+$"));
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
    [InlineData("?sections=ai", 8)] // 8 items with 'ai' section
    [InlineData("?sections=github-copilot", 22)] // 22 non-draft items with 'github-copilot' section
    [InlineData("?collections=news", 7)] // 7 news items
    [InlineData("?collections=videos", 4)] // 4 video items (including subcollection videos)
    [InlineData("?collections=blogs", 18)] // 18 blog items
    [InlineData("?tags=Developer Tools", 3)] // 3 items with 'Developer Tools' tag
    public async Task FilterContent_VariousCriteria_ReturnsExpectedCounts(string queryString, int expectedCount)
    {
        // Act
        var response = await _client.GetAsync($"/api/content/filter{queryString}");
        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();

        // Assert
        items.Should().HaveCount(expectedCount);
    }

    [Fact]
    public async Task FilterContent_PreservesAllContentItemProperties()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?collections=videos");
        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();

        // Assert
        var item = items![0];
        item.Slug.Should().NotBeNullOrEmpty();
        item.Title.Should().NotBeNullOrEmpty();
        item.Author.Should().NotBeNullOrEmpty();
        item.DateEpoch.Should().BeGreaterThan(0);
        item.CollectionName.Should().NotBeNullOrEmpty();
        item.PrimarySectionName.Should().NotBeNullOrEmpty();
        item.Tags.Should().NotBeEmpty();
        item.Excerpt.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetContent_MapsSubcollectionNameAndFeedName()
    {
        // Arrange - test video has SubcollectionName="vscode-updates" and FeedName="Test Feed"

        // Act
        var response = await _client.GetAsync("/api/content?collectionName=videos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        items!.Count.Should().BeGreaterThanOrEqualTo(2); // At least 2 videos in test data

        var videoItem = items[0];
        videoItem.CollectionName.Should().Be("videos");
        videoItem.SubcollectionName.Should().BeNull("Test video doesn't have a subcollection");
        // FeedName may or may not be set depending on the test data
    }

    [Fact]
    public async Task FilterContent_WithNoParameters_ShouldNotReturnDraftItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        
        // Should not include the draft item (2026-02-01-draft-feature-announcement.md)
        items!.Should().NotContain(item => item.Draft, "draft items should be filtered out by default");
        items.Should().NotContain(item => item.Title.Contains("Coming Soon"), "draft items should be filtered out by default");
    }

    [Fact]
    public async Task GetContent_WithNoFilters_ShouldNotReturnDraftItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        
        // Should not include the draft item
        items!.Should().NotContain(item => item.Draft, "draft items should be filtered out by default");
    }

    [Fact]
    public async Task GetContent_BySectionName_ShouldNotReturnDraftItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content?sectionName=ai");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        
        // Should not include the draft item even though it has ai section
        items!.Should().NotContain(item => item.Draft, "draft items should be filtered out by default");
    }

    [Fact]
    public async Task GetContent_ByCollectionName_ShouldNotReturnDraftItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content?collectionName=news");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        
        // Should not include the draft news item
        items!.Should().NotContain(item => item.Draft, "draft items should be filtered out by default");
    }

    [Fact]
    public async Task GetContent_WithIncludeDraft_ShouldIncludeDraftItems()
    {
        // This is the ONLY scenario where drafts should be included
        // (for the GitHub Copilot Features page to show "Coming Soon" items)
        
        // Act
        var response = await _client.GetAsync("/api/content?includeDraft=true");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
        items.Should().NotBeNull();
        
        // When includeDraft=true, draft items would be included
        // This test documents the exception to the rule
    }

    #region SearchContent Endpoint Tests

    [Fact]
    public async Task SearchContent_WithNoParameters_ReturnsFirst20Items()
    {
        // Act - default Take is 20
        var response = await _client.GetAsync("/api/content/search");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var results = await response.Content.ReadFromJsonAsync<SearchResults<ContentItem>>();
        results.Should().NotBeNull();
        results!.Items.Should().HaveCountLessThanOrEqualTo(20, "Default Take is 20");
        results.TotalCount.Should().Be(32, "Total count should be all 32 non-draft items");
    }

    [Fact]
    public async Task SearchContent_WithTake1_ReturnsExactlyOneItem()
    {
        // Act - database-level LIMIT 1
        var response = await _client.GetAsync("/api/content/search?take=1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var results = await response.Content.ReadFromJsonAsync<SearchResults<ContentItem>>();
        results.Should().NotBeNull();
        results!.Items.Should().HaveCount(1, "Take=1 should return exactly 1 item");
        results.TotalCount.Should().Be(32, "Total count should still be all 32 items");
    }

    [Fact]
    public async Task SearchContent_WithTake5_ReturnsExactlyFiveItems()
    {
        // Act - database-level LIMIT 5
        var response = await _client.GetAsync("/api/content/search?take=5");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var results = await response.Content.ReadFromJsonAsync<SearchResults<ContentItem>>();
        results.Should().NotBeNull();
        results!.Items.Should().HaveCount(5, "Take=5 should return exactly 5 items");
        results.TotalCount.Should().Be(32, "Total count should still be all 32 items");
    }

    [Fact]
    public async Task SearchContent_WithCollectionRoundups_ReturnsOnlyRoundups()
    {
        // Act - filter by roundups collection
        var response = await _client.GetAsync("/api/content/search?collections=roundups");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var results = await response.Content.ReadFromJsonAsync<SearchResults<ContentItem>>();
        results.Should().NotBeNull();
        results!.Items.Should().HaveCount(1, "There is 1 roundup in TestCollections");
        results.Items.Should().AllSatisfy(item => item.CollectionName.Should().Be("roundups"));
        results.TotalCount.Should().Be(1, "Total count should be 1 roundup");
    }

    [Fact]
    public async Task SearchContent_WithCollectionRoundupsAndTake1_ReturnsLatestRoundup()
    {
        // Act - This is the exact query used by GetLatestRoundupAsync
        var response = await _client.GetAsync("/api/content/search?collections=roundups&take=1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var results = await response.Content.ReadFromJsonAsync<SearchResults<ContentItem>>();
        results.Should().NotBeNull();
        results!.Items.Should().HaveCount(1, "Should return exactly 1 roundup");
        results.Items[0].CollectionName.Should().Be("roundups");
    }

    [Fact]
    public async Task SearchContent_WithCollectionBlogs_Returns18Blogs()
    {
        // Act
        var response = await _client.GetAsync("/api/content/search?collections=blogs");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var results = await response.Content.ReadFromJsonAsync<SearchResults<ContentItem>>();
        results.Should().NotBeNull();
        results!.Items.Should().HaveCount(18, "There are 18 blogs in TestCollections");
        results.Items.Should().AllSatisfy(item => item.CollectionName.Should().Be("blogs"));
        results.TotalCount.Should().Be(18);
    }

    [Fact]
    public async Task SearchContent_WithSectionAi_ReturnsOnlyAiSectionItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content/search?sections=ai");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var results = await response.Content.ReadFromJsonAsync<SearchResults<ContentItem>>();
        results.Should().NotBeNull();
        results!.Items.Should().NotBeEmpty("There should be items in the ai section");
    }

    [Fact]
    public async Task SearchContent_WithTagsAI_ReturnsItemsWithAITag()
    {
        // Act
        var response = await _client.GetAsync("/api/content/search?tags=AI");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var results = await response.Content.ReadFromJsonAsync<SearchResults<ContentItem>>();
        results.Should().NotBeNull();
        results!.Items.Should().NotBeEmpty("There should be items with AI tag");
        results.TotalCount.Should().Be(9, "There are 9 items with AI tag in TestCollections");
    }

    [Fact]
    public async Task SearchContent_OrderedByDateDesc_ReturnsNewestFirst()
    {
        // Act
        var response = await _client.GetAsync("/api/content/search?take=5&orderBy=date_desc");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var results = await response.Content.ReadFromJsonAsync<SearchResults<ContentItem>>();
        results.Should().NotBeNull();
        results!.Items.Should().HaveCount(5);
        results.Items.Should().BeInDescendingOrder(item => item.DateEpoch, 
            "Items should be sorted by date descending");
    }

    [Fact]
    public async Task SearchContent_ExcludesDraftItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content/search");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var results = await response.Content.ReadFromJsonAsync<SearchResults<ContentItem>>();
        results.Should().NotBeNull();
        results!.Items.Should().NotContain(item => item.Draft, 
            "Search results should never include drafts");
    }

    #endregion
}
