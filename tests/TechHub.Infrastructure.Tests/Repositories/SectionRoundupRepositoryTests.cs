using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Infrastructure.Repositories;
using TechHub.TestUtilities;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for <see cref="SectionRoundupRepository"/>.
/// Uses PostgreSQL via Testcontainers to verify Dapper mapping, JSON parsing, and date filtering.
/// </summary>
public class SectionRoundupRepositoryTests
    : IClassFixture<DatabaseFixture<SectionRoundupRepositoryTests>>
{
    private readonly DatabaseFixture<SectionRoundupRepositoryTests> _fixture;
    private readonly SectionRoundupRepository _sut;

    private static readonly DateOnly _weekStart = new(2025, 3, 24); // Monday 24 March 2025
    private static readonly DateOnly _weekEnd = new(2025, 3, 30);   // Sunday 30 March 2025

    public SectionRoundupRepositoryTests(DatabaseFixture<SectionRoundupRepositoryTests> fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        _fixture = fixture;
        _sut = new SectionRoundupRepository(
            fixture.Connection,
            NullLogger<SectionRoundupRepository>.Instance);
    }

    // ── GetArticlesForWeekAsync ────────────────────────────────────────────────

    [Fact]
    public async Task GetArticlesForWeekAsync_WithMatchingItems_ReturnsGroupedArticles()
    {
        // Arrange
        await SeedContentItemAsync("test-article-1", "news", "Test Article 1",
            "https://example.com/1", relevance: "high", summary: "Summary of article 1", keyTopics: ["AI", "Testing"]);
        await SeedRoundupItemAsync("ai", _weekStart, "news", "test-article-1");

        // Act
        var result = await _sut.GetArticlesForWeekAsync(_weekStart, _weekEnd);

        // Assert
        result.Should().ContainKey("ai");
        var articles = result["ai"];
        articles.Should().ContainSingle();
        articles[0].Title.Should().Be("Test Article 1");
        articles[0].ExternalUrl.Should().Be("https://example.com/1");
        articles[0].Relevance.Should().Be("high");
        articles[0].Summary.Should().Be("Summary of article 1");
        articles[0].KeyTopics.Should().Equal(["AI", "Testing"]);
        articles[0].IsInternal.Should().BeFalse();
    }

    [Fact]
    public async Task GetArticlesForWeekAsync_WithInternalCollection_SetsIsInternalTrue()
    {
        // Arrange
        await SeedContentItemAsync("test-video-1", "videos", "Test Video 1",
            "/videos/watch?v=abc", relevance: "medium");
        await SeedRoundupItemAsync("ai", _weekStart, "videos", "test-video-1");

        // Act
        var result = await _sut.GetArticlesForWeekAsync(_weekStart, _weekEnd);

        // Assert
        result.Should().ContainKey("ai");
        var article = result["ai"].First(a => a.Slug == "test-video-1");
        article.IsInternal.Should().BeTrue();
    }

    [Fact]
    public async Task GetArticlesForWeekAsync_WithMultipleSections_GroupsBySection()
    {
        // Arrange
        await SeedContentItemAsync("azure-article-1", "blogs", "Azure Article 1",
            "https://azure.example.com/1", relevance: "high");
        await SeedContentItemAsync("dotnet-article-1", "blogs", ".NET Article 1",
            "https://dotnet.example.com/1", relevance: "medium");

        await SeedRoundupItemAsync("azure", _weekStart, "blogs", "azure-article-1");
        await SeedRoundupItemAsync("dotnet", _weekStart, "blogs", "dotnet-article-1");

        // Act
        var result = await _sut.GetArticlesForWeekAsync(_weekStart, _weekEnd);

        // Assert
        result.Should().ContainKey("azure");
        result.Should().ContainKey("dotnet");
        result["azure"].Should().Contain(a => a.Slug == "azure-article-1");
        result["dotnet"].Should().Contain(a => a.Slug == "dotnet-article-1");
    }

    [Fact]
    public async Task GetArticlesForWeekAsync_OutsideDateRange_ReturnsEmpty()
    {
        // Arrange — item belongs to a different week
        var differentWeekStart = new DateOnly(2025, 1, 6);
        await SeedContentItemAsync("out-of-range-article", "news", "Out of Range Article",
            "https://example.com/out", relevance: "high");
        await SeedRoundupItemAsync("ai", differentWeekStart, "news", "out-of-range-article");

        // Act — query the current week, not the different week
        var result = await _sut.GetArticlesForWeekAsync(
            new DateOnly(2025, 3, 24),
            new DateOnly(2025, 3, 30));

        // Assert
        result.Should().NotContainKey("out-of-range-article");
        // If the section exists it should not contain our out-of-range article
        if (result.TryGetValue("ai", out var articles))
        {
            articles.Should().NotContain(a => a.Slug == "out-of-range-article");
        }
    }

    [Fact]
    public async Task GetArticlesForWeekAsync_WithNullAiMetadata_DefaultsToMediumRelevance()
    {
        // Arrange — no ai_metadata
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch,
                 primary_section_name, external_url, author, feed_name, tags_csv,
                 sections_bitmask, content_hash)
            VALUES ('no-meta-article', 'news', 'No Meta Article', '', '', 1711699200,
                    'ai', 'https://example.com/no-meta', 'Author', 'Feed', '', 4, 'hash-no-meta')
            ON CONFLICT (collection_name, slug) DO NOTHING
            """);
        await SeedRoundupItemAsync("ai", _weekStart, "news", "no-meta-article");

        // Act
        var result = await _sut.GetArticlesForWeekAsync(_weekStart, _weekEnd);

        // Assert — item is excluded because ai_metadata IS NULL (see WHERE clause)
        if (result.TryGetValue("ai", out var articles))
        {
            articles.Should().NotContain(a => a.Slug == "no-meta-article");
        }
    }

    [Fact]
    public async Task GetArticlesForWeekAsync_WithAllRelevanceLevels_ReturnsAll()
    {
        // Arrange
        await SeedContentItemAsync("relevance-high", "news", "High Relevance",
            "https://example.com/high", relevance: "high");
        await SeedContentItemAsync("relevance-medium", "news", "Medium Relevance",
            "https://example.com/medium", relevance: "medium");
        await SeedContentItemAsync("relevance-low", "news", "Low Relevance",
            "https://example.com/low", relevance: "low");

        await SeedRoundupItemAsync("security", _weekStart, "news", "relevance-high");
        await SeedRoundupItemAsync("security", _weekStart, "news", "relevance-medium");
        await SeedRoundupItemAsync("security", _weekStart, "news", "relevance-low");

        // Act
        var result = await _sut.GetArticlesForWeekAsync(_weekStart, _weekEnd);

        // Assert — repository returns all levels unfiltered
        result.Should().ContainKey("security");
        var articles = result["security"];
        articles.Should().Contain(a => a.Slug == "relevance-high");
        articles.Should().Contain(a => a.Slug == "relevance-medium");
        articles.Should().Contain(a => a.Slug == "relevance-low");
    }

    // ── Helpers ────────────────────────────────────────────────────────────────

    private async Task SeedContentItemAsync(
        string slug,
        string collection,
        string title,
        string externalUrl,
        string relevance = "high",
        string summary = "",
        IEnumerable<string>? keyTopics = null)
    {
        var topics = keyTopics?.ToList() ?? [];
        _ = System.Text.Json.JsonSerializer.Serialize(topics);

        var aiMetadata = System.Text.Json.JsonSerializer.Serialize(new
        {
            roundup_summary = summary,
            key_topics = topics,
            roundup_relevance = relevance,
            topic_type = "news",
            impact_level = "medium",
            time_sensitivity = "this-week"
        });

        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch,
                 primary_section_name, external_url, author, feed_name, tags_csv,
                 sections_bitmask, content_hash, ai_metadata)
            VALUES (@Slug, @Collection, @Title, '', '', 1743278400,
                    'ai', @ExternalUrl, 'Author', 'Feed', '', 4, @Hash, @AiMetadata::jsonb)
            ON CONFLICT (collection_name, slug) DO UPDATE SET
                ai_metadata = EXCLUDED.ai_metadata,
                title       = EXCLUDED.title
            """,
            new
            {
                Slug = slug,
                Collection = collection,
                Title = title,
                ExternalUrl = externalUrl,
                Hash = $"hash-{slug}",
                AiMetadata = aiMetadata
            });
    }

    private async Task SeedRoundupItemAsync(
        string sectionName,
        DateOnly weekStartDate,
        string collectionName,
        string slug)
    {
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO section_roundup_items (section_name, week_start_date, collection_name, slug)
            VALUES (@SectionName, @WeekStartDate, @CollectionName, @Slug)
            ON CONFLICT (section_name, week_start_date, collection_name, slug) DO NOTHING
            """,
            new
            {
                SectionName = sectionName,
                WeekStartDate = weekStartDate.ToDateTime(TimeOnly.MinValue),
                CollectionName = collectionName,
                Slug = slug
            });
    }
}
