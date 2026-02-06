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
    private readonly DatabaseContentRepository _repository;

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

        _repository = new DatabaseContentRepository(
            fixture.Connection,
            new Infrastructure.Data.SqliteDialect(),
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
        var tagCounts = await _repository.GetTagCountsAsync(request);

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
        var tagCounts = await _repository.GetTagCountsAsync(request);

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
        var tagCounts = await _repository.GetTagCountsAsync(request);

        // Assert - should contain actual content tags from test data
        tagCounts.Should().NotBeEmpty("Tag cloud should contain content tags");
        // These are actual tags from the test markdown files (not section/collection names)
        tagCounts.Should().Contain(t => t.Tag.Equals("Test Tag", StringComparison.OrdinalIgnoreCase) ||
                                       t.Tag.Equals("Code Review", StringComparison.OrdinalIgnoreCase) ||
                                       t.Tag.Equals("Copilot", StringComparison.OrdinalIgnoreCase),
            "Tag cloud should include actual content tags");
    }
}
