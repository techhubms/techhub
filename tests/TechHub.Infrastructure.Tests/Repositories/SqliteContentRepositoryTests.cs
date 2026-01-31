using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;
using TechHub.Infrastructure.Repositories;
using TechHub.TestUtilities;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for SQLite content repository.
/// Inherits common repository tests from BaseContentRepositoryTests.
/// Tests SQLite-specific functionality like FTS5 full-text search.
/// Uses in-memory SQLite database with migrations applied and TestCollections seeding.
/// </summary>
public class SqliteContentRepositoryTests : BaseContentRepositoryTests, IClassFixture<DatabaseFixture<SqliteContentRepositoryTests>>
{
    private readonly DatabaseFixture<SqliteContentRepositoryTests> _fixture;
    private readonly SqliteContentRepository _repository;
    private readonly IMemoryCache _cache;

    protected override Core.Interfaces.IContentRepository Repository => _repository;

    public SqliteContentRepositoryTests(DatabaseFixture<SqliteContentRepositoryTests> fixture)
    {
        _fixture = fixture;
        _cache = new MemoryCache(new MemoryCacheOptions());

        // Create a mock markdown service for rendering
        var mockMarkdownService = new Mock<IMarkdownService>();
        mockMarkdownService.Setup(m => m.RenderToHtml(It.IsAny<string>()))
            .Returns<string>(content => $"<p>{content}</p>");
        mockMarkdownService.Setup(m => m.ProcessYouTubeEmbeds(It.IsAny<string>()))
            .Returns<string>(content => content);
        mockMarkdownService.Setup(m => m.ExtractExcerpt(It.IsAny<string>(), It.IsAny<int>()))
            .Returns<string, int>((content, _) => content.Length > 100 ? content[..100] : content);

        _repository = new SqliteContentRepository(
            _fixture.Connection,
            new Infrastructure.Data.SqliteDialect(),
            _cache,
            mockMarkdownService.Object);
    }

    public override void Dispose()
    {
        base.Dispose();
        GC.SuppressFinalize(this);
    }

    #region SQLite-Specific Tests (FTS5 Full-Text Search)

    /// <summary>
    /// Test: Full-text search finds items by unique content keyword
    /// Why: SQLite FTS5 provides fast full-text search that other repositories don't have
    /// </summary>
    [Fact(Skip = "FTS5 external content table configuration needs investigation")]
    public async Task SearchAsync_FullTextSearch_FindsMatchingItems()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: _blogs/2024-01-11-fts-test.md contains "TechHubSpecialKeyword"
        var request = new SearchRequest { Query = "TechHubSpecialKeyword", Take = 1000 };

        // Act
        var results = await Repository.SearchAsync(request);

        // Assert
        results.Items.Should().NotBeEmpty("FTS should find item with unique keyword");
        results.Items.Should().Contain(item => item.Slug == "fts-test",
            "Should find the fts-test article by its unique content");
    }

    #endregion

    #region SQLite-Specific Tests (Denormalized Tags)

    /// <summary>
    /// Test: tags_csv column is populated during sync
    /// Why: Allows in-memory tag parsing without joins
    /// </summary>
    [Fact]
    public async Task GetBySlugAsync_TagsCsvColumn_IsPopulated()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: 2024-01-07-github-copilot.md has tags

        // Act - query database directly to verify tags_csv
        var result = await _fixture.Connection.QuerySingleOrDefaultAsync<string>(
            "SELECT tags_csv FROM content_items WHERE slug = @Slug AND collection_name = @Collection",
            new { Slug = "github-copilot", Collection = "blogs" });

        // Assert
        result.Should().NotBeNull("tags_csv should be populated");
        result.Should().Contain(",GitHub Copilot,", "tags should be comma-delimited with leading/trailing commas");
        result.Should().Contain(",AI,", "should contain AI tag");
    }

    /// <summary>
    /// Test: content_tags_expanded table is populated with word expansions
    /// Why: Enables substring matching (e.g., "AI" matches "AI Engineering")
    /// </summary>
    [Fact]
    public async Task ContentTagsExpanded_WordExpansion_IsPopulated()
    {
        // Arrange - data already seeded from TestCollections
        // Expected: Tags like "GitHub Copilot" are expanded to words: "github", "copilot"

        // Act - query expanded tags
        var expandedWords = await _fixture.Connection.QueryAsync<string>(
            "SELECT DISTINCT tag_word FROM content_tags_expanded WHERE slug = @Slug AND collection_name = @Collection ORDER BY tag_word",
            new { Slug = "github-copilot", Collection = "blogs" });

        // Assert
        expandedWords.Should().NotBeEmpty("expanded tags should exist");
        expandedWords.Should().Contain("github", "multi-word tag 'GitHub Copilot' should expand to 'github'");
        expandedWords.Should().Contain("copilot", "multi-word tag 'GitHub Copilot' should expand to 'copilot'");
        expandedWords.Should().Contain("ai", "single-word tag 'AI' should be included");
    }

    /// <summary>
    /// Test: content_tags_expanded has denormalized date_epoch
    /// Why: Enables filtering by tag and date without joining to content_items
    /// </summary>
    [Fact]
    public async Task ContentTagsExpanded_DateEpoch_IsDenormalized()
    {
        // Arrange - data already seeded

        // Act - query expanded tags with date
        var result = await _fixture.Connection.QuerySingleOrDefaultAsync<long>(
            "SELECT date_epoch FROM content_tags_expanded WHERE slug = @Slug AND collection_name = @Collection LIMIT 1",
            new { Slug = "github-copilot", Collection = "blogs" });

        // Assert
        result.Should().BeGreaterThan(0, "date_epoch should be populated");
        // 2024-01-07 = 1704585600 (approximate)
        result.Should().BeInRange(1704500000, 1705000000, "date should match article date");
    }

    /// <summary>
    /// Test: content_tags_expanded has denormalized section boolean columns
    /// Why: Enables filtering by section without joining to content_sections
    /// </summary>
    [Fact]
    public async Task ContentTagsExpanded_SectionBooleans_ArePopulated()
    {
        // Arrange - data already seeded
        // Expected: Article is in "github-copilot" section only

        // Act - query section booleans
        var result = await _fixture.Connection.QuerySingleAsync<dynamic>(
            @"SELECT is_ai, is_github_copilot, is_ml, is_devops, is_azure, is_coding, is_security 
              FROM content_tags_expanded 
              WHERE slug = @Slug AND collection_name = @Collection 
              LIMIT 1",
            new { Slug = "github-copilot", Collection = "blogs" });

        // Assert
        ((int)result.is_github_copilot).Should().Be(1, "article should be in 'github-copilot' section");
        ((int)result.is_ai).Should().Be(0, "article should NOT be in 'ai' section");
        ((int)result.is_devops).Should().Be(0, "article should NOT be in 'devops' section");
    }

    /// <summary>
    /// Test: content_items has denormalized section boolean columns
    /// Why: Enables zero-join filtering by section on main table
    /// </summary>
    [Fact]
    public async Task ContentItems_SectionBooleans_ArePopulated()
    {
        // Arrange - data already seeded
        // Expected: Article is in "github-copilot" section only

        // Act - query section booleans from content_items
        var result = await _fixture.Connection.QuerySingleAsync<dynamic>(
            @"SELECT is_ai, is_github_copilot, is_ml, is_devops, is_azure, is_coding, is_security 
              FROM content_items 
              WHERE slug = @Slug AND collection_name = @Collection",
            new { Slug = "github-copilot", Collection = "blogs" });

        // Assert
        ((int)result.is_github_copilot).Should().Be(1, "article should be in 'github-copilot' section");
        ((int)result.is_ai).Should().Be(0, "article should NOT be in 'ai' section");
        ((int)result.is_devops).Should().Be(0, "article should NOT be in 'devops' section");
    }

    #endregion
}
