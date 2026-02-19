using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;
using TechHub.Infrastructure.Repositories;
using TechHub.TestUtilities;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for tag cloud generation across all repository types.
/// Tests that section and collection titles are correctly excluded from tag clouds.
/// </summary>
public class TagCloudTests : IClassFixture<DatabaseFixture<TagCloudTests>>
{
    private readonly ContentRepository _repository;

    public TagCloudTests(DatabaseFixture<TagCloudTests> fixture)
    {
        var cache = new MemoryCache(new MemoryCacheOptions());

        var mockMarkdownService = new Mock<IMarkdownService>();
        mockMarkdownService.Setup(m => m.RenderToHtml(It.IsAny<string>()))
            .Returns<string>(content => $"<p>{content}</p>");
        mockMarkdownService.Setup(m => m.ProcessYouTubeEmbeds(It.IsAny<string>()))
            .Returns<string>(content => content);
        mockMarkdownService.Setup(m => m.ExtractExcerpt(It.IsAny<string>(), It.IsAny<int>()))
            .Returns<string, int>((content, _) => content.Length > 100 ? content[..100] : content);

        // Load real AppSettings from appsettings.json - includes section/collection configuration
        var appSettings = ConfigurationHelper.LoadAppSettings();

        _repository = new ContentRepository(
            fixture.Connection,
            new Infrastructure.Data.PostgresDialect(),
            cache,
            mockMarkdownService.Object,
            Options.Create(appSettings));
    }

    /// <summary>
    /// Test: Tag cloud should exclude section titles from results
    /// Why: Section titles like "AI", "GitHub Copilot" are auto-added to every item
    ///      in those sections for search purposes, but shouldn't appear in tag clouds
    /// </summary>
    [Fact]
    public async Task GetTagCountsAsync_ExcludesSectionTitles()
    {
        // Arrange
        var request = new TagCountsRequest(
            sectionName: "all",
            collectionName: "all",
            minUses: 1,
            maxTags: 50,
            dateFrom: null,
            dateTo: null
        );

        // Act
        var tagCounts = await _repository.GetTagCountsAsync(request, TestContext.Current.CancellationToken);

        // Assert
        tagCounts.Should().NotContain(t => t.Tag.Equals("AI", StringComparison.OrdinalIgnoreCase),
            "Section title 'AI' should be excluded from tag cloud");
        tagCounts.Should().NotContain(t => t.Tag.Equals("Artificial Intelligence", StringComparison.OrdinalIgnoreCase),
            "Section title 'Artificial Intelligence' should be excluded from tag cloud");
        tagCounts.Should().NotContain(t => t.Tag.Equals("GitHub Copilot", StringComparison.OrdinalIgnoreCase),
            "Section title 'GitHub Copilot' should be excluded from tag cloud");
        tagCounts.Should().NotContain(t => t.Tag.Equals("Machine Learning", StringComparison.OrdinalIgnoreCase),
            "Section title 'Machine Learning' should be excluded from tag cloud");
        tagCounts.Should().NotContain(t => t.Tag.Equals(".NET", StringComparison.OrdinalIgnoreCase),
            "Section title '.NET' should be excluded from tag cloud");
    }

    /// <summary>
    /// Test: Tag cloud should exclude collection display names from results
    /// Why: Collection names like "Blogs", "Videos" are auto-added to every item
    ///      in those collections for search purposes, but shouldn't appear in tag clouds
    /// </summary>
    [Fact]
    public async Task GetTagCountsAsync_ExcludesCollectionDisplayNames()
    {
        // Arrange
        var request = new TagCountsRequest(
            sectionName: "all",
            collectionName: "all",
            minUses: 1,
            maxTags: 50,
            dateFrom: null,
            dateTo: null
        );

        // Act
        var tagCounts = await _repository.GetTagCountsAsync(request, TestContext.Current.CancellationToken);

        // Assert
        tagCounts.Should().NotContain(t => t.Tag.Equals("Blogs", StringComparison.OrdinalIgnoreCase),
            "Collection name 'Blogs' should be excluded from tag cloud");
        tagCounts.Should().NotContain(t => t.Tag.Equals("Videos", StringComparison.OrdinalIgnoreCase),
            "Collection name 'Videos' should be excluded from tag cloud");
        tagCounts.Should().NotContain(t => t.Tag.Equals("News", StringComparison.OrdinalIgnoreCase),
            "Collection name 'News' should be excluded from tag cloud");
        tagCounts.Should().NotContain(t => t.Tag.Equals("Community", StringComparison.OrdinalIgnoreCase),
            "Collection name 'Community' should be excluded from tag cloud");
        tagCounts.Should().NotContain(t => t.Tag.Equals("Community Posts", StringComparison.OrdinalIgnoreCase),
            "Collection display name 'Community Posts' should be excluded from tag cloud");
        tagCounts.Should().NotContain(t => t.Tag.Equals("Roundups", StringComparison.OrdinalIgnoreCase),
            "Collection name 'Roundups' should be excluded from tag cloud");
    }

    /// <summary>
    /// Test: Tag cloud should include content-specific tags
    /// Why: Only actual content tags (not structural tags) should appear
    /// </summary>
    [Fact]
    public async Task GetTagCountsAsync_IncludesContentTags()
    {
        // Arrange
        var request = new TagCountsRequest(
            sectionName: "all",
            collectionName: "all",
            minUses: 1,
            maxTags: 50,
            dateFrom: null,
            dateTo: null
        );

        // Act
        var tagCounts = await _repository.GetTagCountsAsync(request, TestContext.Current.CancellationToken);

        // Assert - should contain actual content tags from test data
        tagCounts.Should().NotBeEmpty("Tag cloud should contain content tags");
        // These are actual tags from the test markdown files (not section/collection names)
        tagCounts.Should().Contain(t => t.Tag.Equals("Test Tag", StringComparison.OrdinalIgnoreCase) ||
                                       t.Tag.Equals("Code Review", StringComparison.OrdinalIgnoreCase) ||
                                       t.Tag.Equals("Copilot", StringComparison.OrdinalIgnoreCase),
            "Tag cloud should include actual content tags");
    }

    /// <summary>
    /// Test: TagsToCount query returns display names from MAX(tag_display).
    /// Why: The unified query uses MAX(tag_display) for proper casing from the database,
    ///      so display names are always correct regardless of the caller's input casing.
    ///      Results also include popular tags filling up to maxTags.
    /// </summary>
    [Fact]
    public async Task GetTagCountsAsync_WithTagsToCount_ReturnsProperDisplayNames()
    {
        // Arrange - request counts for specific tags (casing doesn't matter, DB provides display names)
        var request = new TagCountsRequest(
            sectionName: "all",
            collectionName: "all",
            minUses: 1,
            maxTags: 50,
            dateFrom: null,
            dateTo: null,
            tags: null,
            tagsToCount: ["code review", "collaboration"]  // lowercase input
        );

        // Act
        var tagCounts = await _repository.GetTagCountsAsync(request, TestContext.Current.CancellationToken);

        // Assert - display names come from MAX(tag_display), always proper-cased
        tagCounts.Should().NotBeEmpty("Should return counts for requested tags plus popular tags");

        var codeReview = tagCounts.FirstOrDefault(t => t.Tag.Equals("Code Review", StringComparison.OrdinalIgnoreCase));
        codeReview.Should().NotBeNull("Should return a result for 'Code Review'");
        codeReview!.Tag.Should().Be("Code Review", "Tag display name should come from MAX(tag_display) with proper casing");

        var collaboration = tagCounts.FirstOrDefault(t => t.Tag.Equals("Collaboration", StringComparison.OrdinalIgnoreCase));
        collaboration.Should().NotBeNull("Should return a result for 'Collaboration'");
        collaboration!.Tag.Should().Be("Collaboration", "Tag display name should come from MAX(tag_display) with proper casing");
    }

    /// <summary>
    /// Test: TagsToCount filters out excluded tags (section/collection titles, high-frequency terms).
    /// Why: Content items may have tags like "GitHub Copilot" or "AI" which are section titles.
    ///      These structural tags should be excluded from the tag cloud even when passed via tagsToCount.
    /// </summary>
    [Fact]
    public async Task GetTagCountsAsync_WithTagsToCount_ExcludesStructuralTags()
    {
        // Arrange - include structural tags that should be excluded
        var request = new TagCountsRequest(
            sectionName: "all",
            collectionName: "all",
            minUses: 1,
            maxTags: 50,
            dateFrom: null,
            dateTo: null,
            tags: null,
            tagsToCount: ["AI", "Code Review", "GitHub Copilot", "Microsoft"]
        );

        // Act
        var tagCounts = await _repository.GetTagCountsAsync(request, TestContext.Current.CancellationToken);

        // Assert - structural/excluded tags should not be in results
        tagCounts.Should().NotContain(t => t.Tag.Equals("AI", StringComparison.OrdinalIgnoreCase),
            "Section title 'AI' should be excluded");
        tagCounts.Should().NotContain(t => t.Tag.Equals("GitHub Copilot", StringComparison.OrdinalIgnoreCase),
            "Section title 'GitHub Copilot' should be excluded");
        tagCounts.Should().NotContain(t => t.Tag.Equals("Microsoft", StringComparison.OrdinalIgnoreCase),
            "High-frequency tag 'Microsoft' should be excluded");

        // Non-excluded tag should still be present
        tagCounts.Should().Contain(t => t.Tag.Equals("Code Review", StringComparison.OrdinalIgnoreCase),
            "Content tag 'Code Review' should be included");
    }

    /// <summary>
    /// Test: TagsToCount returns specific tags PLUS popular tags to fill up to maxTags.
    /// Why: The unified query returns all requested tags with real counts and fills
    ///      remaining slots with popular tags, so the tag cloud is always full.
    /// </summary>
    [Fact]
    public async Task GetTagCountsAsync_WithTagsToCount_IncludesPopularTagsFill()
    {
        // Arrange - request 2 specific tags with maxTags=10
        var request = new TagCountsRequest(
            sectionName: "all",
            collectionName: "all",
            minUses: 1,
            maxTags: 10,
            dateFrom: null,
            dateTo: null,
            tags: null,
            tagsToCount: ["Code Review", "Collaboration"]
        );

        // Act
        var tagCounts = await _repository.GetTagCountsAsync(request, TestContext.Current.CancellationToken);

        // Assert - should have the 2 specific tags plus popular tags filling up to maxTags
        tagCounts.Should().Contain(t => t.Tag.Equals("Code Review", StringComparison.OrdinalIgnoreCase),
            "Requested tag should be in results");
        tagCounts.Should().Contain(t => t.Tag.Equals("Collaboration", StringComparison.OrdinalIgnoreCase),
            "Requested tag should be in results");

        // Should have more than just the 2 requested tags (popular fill)
        tagCounts.Should().HaveCountGreaterThan(2,
            "Should include popular tags filling up to maxTags beyond the 2 requested tags");
    }

    /// <summary>
    /// Test: Top tags query path also returns proper display names.
    /// Why: Validates the existing MAX(tag_display) approach works correctly.
    /// </summary>
    [Fact]
    public async Task GetTagCountsAsync_TopTags_ReturnsProperDisplayNames()
    {
        // Arrange - standard top tags request (no TagsToCount)
        var request = new TagCountsRequest(
            sectionName: "all",
            collectionName: "all",
            minUses: 1,
            maxTags: 50,
            dateFrom: null,
            dateTo: null
        );

        // Act
        var tagCounts = await _repository.GetTagCountsAsync(request, TestContext.Current.CancellationToken);

        // Assert - no tag should be all lowercase (display names always have proper casing)
        tagCounts.Should().NotBeEmpty();
        foreach (var tag in tagCounts)
        {
            // Every tag should have at least one uppercase letter (proper display name)
            tag.Tag.Should().MatchRegex("[A-Z]",
                $"Tag '{tag.Tag}' should have proper casing (not all lowercase)");
        }
    }
}
