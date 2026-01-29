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
}
