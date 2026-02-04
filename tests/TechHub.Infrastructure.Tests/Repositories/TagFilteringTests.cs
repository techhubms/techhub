using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;
using TechHub.Infrastructure.Data;
using TechHub.Infrastructure.Repositories;
using TechHub.TestUtilities;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Comprehensive tests for tag filtering behavior.
/// Uses pre-seeded test data from TestCollections directory.
/// Tests tag expansion, AND logic for multiple tags, pagination, and cache key uniqueness.
/// </summary>
public class TagFilteringTests : IClassFixture<DatabaseFixture<TagFilteringTests>>
{
    private readonly IContentRepository _repository;

    public TagFilteringTests(DatabaseFixture<TagFilteringTests> fixture)
    {
        // Create minimal dependencies for repository
        var cache = new MemoryCache(new MemoryCacheOptions());
        
        var mockMarkdownService = new Mock<IMarkdownService>();
        mockMarkdownService.Setup(m => m.RenderToHtml(It.IsAny<string>()))
            .Returns<string>(content => $"<p>{content}</p>");
            
        var appSettings = new AppSettings
        {
            Content = new ContentSettings
            {
                CollectionsPath = "collections",
                Sections = [],
                CollectionDisplayNames = []
            },
            BaseUrl = "https://localhost:5001"
        };

        _repository = new SqliteContentRepository(
            fixture.Connection,
            new SqliteDialect(),
            cache,
            mockMarkdownService.Object,
            Options.Create(appSettings));
    }

    /// <summary>
    /// Test: Single-word tag "AI" returns multiple matching items
    /// Why: Verifies tag expansion - "AI" tag matches items with "AI" as part of multi-word tags
    /// </summary>
    [Fact]
    public async Task SearchByTag_SingleWordTag_ReturnsMatchingItems()
    {
        // Arrange
        var request = new SearchRequest(
            take: 50,
            sections: ["all"],
            collections: ["all"],
            tags: ["ai"],
            skip: 0
        );

        // Act
        var result = await _repository.SearchAsync(request);

        // Assert - multiple items should have "ai" tag (either direct or expanded from multi-word tags)
        result.Items.Should().NotBeEmpty("AI is a common tag in test data");
        result.Items.Should().Contain(i => i.Tags.Any(t => t.Equals("AI", StringComparison.OrdinalIgnoreCase)));
    }

    /// <summary>
    /// Test: Multiple tags use AND logic - only items with ALL tags return
    /// Why: Critical for filtered search - "GitHub Copilot" AND "AI" should only match items with BOTH
    /// </summary>
    [Fact]
    public async Task SearchByTag_MultipleTags_AndLogic_ReturnsOnlyItemsWithAllTags()
    {
        // Arrange - search for items with BOTH "github copilot" AND "collaboration"
        var request = new SearchRequest(
            take: 50,
            sections: ["all"],
            collections: ["all"],
            tags: ["github copilot", "collaboration"],
            skip: 0
        );

        // Act
        var result = await _repository.SearchAsync(request);

        // Assert - all returned items must have BOTH tags
        result.Items.Should().NotBeEmpty("Test data includes items with both tags");
        foreach (var item in result.Items)
        {
            var tagSet = item.Tags.Select(t => t.ToLowerInvariant()).ToHashSet();
            tagSet.Should().Contain("github copilot", "All items must have 'GitHub Copilot' tag");
            tagSet.Should().Contain("collaboration", "All items must have 'Collaboration' tag");
        }
    }

    /// <summary>
    /// Test: Pagination with tag filters returns different results for different skip values
    /// Why: This is the BUG we fixed - cache key must include skip parameter
    /// </summary>
    [Fact]
    public async Task SearchByTag_WithPagination_ReturnsDifferentResultsForDifferentSkips()
    {
        // Arrange - search for common tag to get multiple pages
        var request1 = new SearchRequest(
            take: 2,
            sections: ["all"],
            collections: ["all"],
            tags: ["github copilot"],
            skip: 0
        );

        var request2 = new SearchRequest(
            take: 2,
            sections: ["all"],
            collections: ["all"],
            tags: ["github copilot"],
            skip: 2
        );

        // Act
        var page1 = await _repository.SearchAsync(request1);
        var page2 = await _repository.SearchAsync(request2);

        // Assert - pages should NOT be identical (the bug we fixed)
        if (page1.Items.Count > 0 && page2.Items.Count > 0)
        {
            // If both pages have results, they should be different
            var page1Slugs = page1.Items.Select(i => i.Slug).ToHashSet();
            var page2Slugs = page2.Items.Select(i => i.Slug).ToHashSet();
            page1Slugs.Should().NotBeEquivalentTo(page2Slugs,
                "Different skip values should return different items (cache key bug fix)");
        }
    }

    /// <summary>
    /// Test: Case-insensitive tag matching works
    /// Why: Tags should match regardless of case (AI, ai, Ai all match)
    /// </summary>
    [Fact]
    public async Task SearchByTag_CaseInsensitive_ReturnsMatches()
    {
        // Arrange - search with uppercase
        var requestUpper = new SearchRequest(
            take: 50,
            sections: ["all"],
            collections: ["all"],
            tags: ["GITHUB COPILOT"],
            skip: 0
        );

        var requestLower = new SearchRequest(
            take: 50,
            sections: ["all"],
            collections: ["all"],
            tags: ["github copilot"],
            skip: 0
        );

        // Act
        var resultUpper = await _repository.SearchAsync(requestUpper);
        var resultLower = await _repository.SearchAsync(requestLower);

        // Assert - case should not matter
        resultUpper.Items.Count.Should().Be(resultLower.Items.Count,
            "Tag search should be case-insensitive");
        var upperSlugs = resultUpper.Items.Select(i => i.Slug).OrderBy(s => s).ToList();
        var lowerSlugs = resultLower.Items.Select(i => i.Slug).OrderBy(s => s).ToList();
        upperSlugs.Should().BeEquivalentTo(lowerSlugs);
    }

    /// <summary>
    /// Test: Results are ordered by date descending (newest first)
    /// Why: Users expect newest content first
    /// </summary>
    [Fact]
    public async Task SearchByTag_OrderedByDate_NewestFirst()
    {
        // Arrange
        var request = new SearchRequest(
            take: 50,
            sections: ["all"],
            collections: ["all"],
            tags: ["ai"],
            skip: 0
        );

        // Act
        var result = await _repository.SearchAsync(request);

        // Assert - items should be ordered by date descending
        result.Items.Should().NotBeEmpty();
        var dates = result.Items.Select(i => i.DateEpoch).ToList();
        dates.Should().BeInDescendingOrder("Results should be ordered by date descending");
    }

    /// <summary>
    /// Test: Tag filtering with section filter returns only items in that section
    /// Why: Section + tag combination is common UI pattern
    /// </summary>
    [Fact]
    public async Task SearchByTag_WithSectionFilter_ReturnsOnlyFromSection()
    {
        // Arrange - search for tags in specific section
        var request = new SearchRequest(
            take: 50,
            sections: ["github-copilot"],
            collections: ["all"],
            tags: ["github copilot"],
            skip: 0
        );

        // Act
        var result = await _repository.SearchAsync(request);

        // Assert - all items should be in the github-copilot section
        result.Items.Should().NotBeEmpty("Test data has GitHub Copilot items");
        foreach (var item in result.Items)
        {
            item.Sections.Should().Contain("github-copilot",
                "All items should be in github-copilot section");
        }
    }

    /// <summary>
    /// Test: Non-existent tag returns empty results
    /// Why: Graceful handling of invalid searches
    /// </summary>
    [Fact]
    public async Task SearchByTag_NonExistentTag_ReturnsEmpty()
    {
        // Arrange - search for tag that doesn't exist
        var request = new SearchRequest(
            take: 50,
            sections: ["all"],
            collections: ["all"],
            tags: ["this-tag-definitely-does-not-exist-in-test-data-xyz123"],
            skip: 0
        );

        // Act
        var result = await _repository.SearchAsync(request);

        // Assert
        result.Items.Should().BeEmpty("Non-existent tag should return no results");
    }

    /// <summary>
    /// Test: Cache returns same results for identical requests
    /// Why: Verify caching is working correctly
    /// </summary>
    [Fact]
    public async Task SearchByTag_CachedRequest_ReturnsSameResults()
    {
        // Arrange
        var request = new SearchRequest(
            take: 5,
            sections: ["all"],
            collections: ["all"],
            tags: ["collaboration"],
            skip: 0
        );

        // Act - call twice
        var result1 = await _repository.SearchAsync(request);
        var result2 = await _repository.SearchAsync(request);

        // Assert - results should be identical
        result1.Items.Count.Should().Be(result2.Items.Count,
            "Cached requests should return same count");
        var slugs1 = result1.Items.Select(i => i.Slug).ToList();
        var slugs2 = result2.Items.Select(i => i.Slug).ToList();
        slugs1.Should().BeEquivalentTo(slugs2,
            "Cached requests should return same items in same order");
    }
}
