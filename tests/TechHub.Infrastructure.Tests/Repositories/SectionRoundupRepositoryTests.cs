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
            "https://example.com/1", sections: ["ai"],
            relevance: "high", summary: "Summary of article 1", keyTopics: ["AI", "Testing"],
            createdAt: new DateTime(2025, 3, 25, 10, 0, 0, DateTimeKind.Utc));

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
            "/videos/watch?v=abc", sections: ["ai"],
            relevance: "medium",
            createdAt: new DateTime(2025, 3, 26, 12, 0, 0, DateTimeKind.Utc));

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
        // Arrange — article belongs to azure only, other to dotnet only
        await SeedContentItemAsync("azure-article-1", "blogs", "Azure Article 1",
            "https://azure.example.com/1", sections: ["azure"],
            relevance: "high",
            createdAt: new DateTime(2025, 3, 25, 8, 0, 0, DateTimeKind.Utc));
        await SeedContentItemAsync("dotnet-article-1", "blogs", ".NET Article 1",
            "https://dotnet.example.com/1", sections: ["dotnet"],
            relevance: "medium",
            createdAt: new DateTime(2025, 3, 26, 8, 0, 0, DateTimeKind.Utc));

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
        // Arrange — item created outside the queried week
        await SeedContentItemAsync("out-of-range-article", "news", "Out of Range Article",
            "https://example.com/out", sections: ["ai"],
            relevance: "high",
            createdAt: new DateTime(2025, 1, 7, 10, 0, 0, DateTimeKind.Utc));

        // Act — query a week that doesn't contain the article
        var result = await _sut.GetArticlesForWeekAsync(
            new DateOnly(2025, 3, 24),
            new DateOnly(2025, 3, 30));

        // Assert
        if (result.TryGetValue("ai", out var articles))
        {
            articles.Should().NotContain(a => a.Slug == "out-of-range-article");
        }
    }

    [Fact]
    public async Task GetArticlesForWeekAsync_WithNullAiMetadata_ExcludesItem()
    {
        // Arrange — no ai_metadata
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch,
                 primary_section_name, external_url, author, feed_name, tags_csv,
                 sections_bitmask, content_hash, is_ai, created_at)
            VALUES ('no-meta-article', 'news', 'No Meta Article', '', '', 1711699200,
                    'ai', 'https://example.com/no-meta', 'Author', 'Feed', '', 4, 'hash-no-meta',
                    TRUE, '2025-03-25T10:00:00Z')
            ON CONFLICT (collection_name, slug) DO NOTHING
            """);

        // Act
        var result = await _sut.GetArticlesForWeekAsync(_weekStart, _weekEnd);

        // Assert — item is excluded because ai_metadata IS NULL
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
            "https://example.com/high", sections: ["security"],
            relevance: "high",
            createdAt: new DateTime(2025, 3, 24, 8, 0, 0, DateTimeKind.Utc));
        await SeedContentItemAsync("relevance-medium", "news", "Medium Relevance",
            "https://example.com/medium", sections: ["security"],
            relevance: "medium",
            createdAt: new DateTime(2025, 3, 25, 8, 0, 0, DateTimeKind.Utc));
        await SeedContentItemAsync("relevance-low", "news", "Low Relevance",
            "https://example.com/low", sections: ["security"],
            relevance: "low",
            createdAt: new DateTime(2025, 3, 26, 8, 0, 0, DateTimeKind.Utc));

        // Act
        var result = await _sut.GetArticlesForWeekAsync(_weekStart, _weekEnd);

        // Assert — repository returns all levels unfiltered
        result.Should().ContainKey("security");
        var articles = result["security"];
        articles.Should().Contain(a => a.Slug == "relevance-high");
        articles.Should().Contain(a => a.Slug == "relevance-medium");
        articles.Should().Contain(a => a.Slug == "relevance-low");
    }

    [Fact]
    public async Task GetArticlesForWeekAsync_WithMultipleSectionFlags_ExpandsIntoEachSection()
    {
        // Arrange — one article belongs to both "ai" and "github-copilot"
        await SeedContentItemAsync("multi-section-article", "news", "Multi-Section Article",
            "https://example.com/multi", sections: ["ai", "github-copilot"],
            relevance: "high",
            createdAt: new DateTime(2025, 3, 27, 10, 0, 0, DateTimeKind.Utc));

        // Act
        var result = await _sut.GetArticlesForWeekAsync(_weekStart, _weekEnd);

        // Assert — article appears in both sections
        result.Should().ContainKey("ai");
        result.Should().ContainKey("github-copilot");
        result["ai"].Should().Contain(a => a.Slug == "multi-section-article");
        result["github-copilot"].Should().Contain(a => a.Slug == "multi-section-article");
    }

    [Fact]
    public async Task GetArticlesForWeekAsync_ExcludesRoundups()
    {
        // Arrange — insert a roundup in the same week
        await SeedContentItemAsync("existing-roundup", "roundups", "Weekly Roundup",
            "/all/roundups/existing-roundup", sections: ["ai"],
            relevance: "high",
            createdAt: new DateTime(2025, 3, 28, 9, 0, 0, DateTimeKind.Utc));

        // Act
        var result = await _sut.GetArticlesForWeekAsync(_weekStart, _weekEnd);

        // Assert — roundups are excluded to prevent circular inclusion
        if (result.TryGetValue("ai", out var articles))
        {
            articles.Should().NotContain(a => a.Slug == "existing-roundup");
        }
    }

    // ── Helpers ────────────────────────────────────────────────────────────────

    private async Task SeedContentItemAsync(
        string slug,
        string collection,
        string title,
        string externalUrl,
        IReadOnlyList<string> sections,
        string relevance = "high",
        string summary = "",
        IEnumerable<string>? keyTopics = null,
        DateTime? createdAt = null)
    {
        var topics = keyTopics?.ToList() ?? [];

        var sectionSet = new HashSet<string>(sections, StringComparer.OrdinalIgnoreCase);
        var isAi = sectionSet.Contains("ai");
        var isAzure = sectionSet.Contains("azure");
        var isDotnet = sectionSet.Contains("dotnet");
        var isDevops = sectionSet.Contains("devops");
        var isGhc = sectionSet.Contains("github-copilot");
        var isMl = sectionSet.Contains("ml");
        var isSecurity = sectionSet.Contains("security");

        var bitmask = 0;
        if (isAi) bitmask |= 1;
        if (isAzure) bitmask |= 2;
        if (isDotnet) bitmask |= 4;
        if (isDevops) bitmask |= 8;
        if (isGhc) bitmask |= 16;
        if (isMl) bitmask |= 32;
        if (isSecurity) bitmask |= 64;

        var aiMetadata = System.Text.Json.JsonSerializer.Serialize(new
        {
            roundup_summary = summary,
            key_topics = topics,
            roundup_relevance = relevance,
            topic_type = "news",
            impact_level = "medium",
            time_sensitivity = "this-week"
        });

        var ts = createdAt ?? new DateTime(2025, 3, 25, 10, 0, 0, DateTimeKind.Utc);

        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch,
                 primary_section_name, external_url, author, feed_name, tags_csv,
                 is_ai, is_azure, is_dotnet, is_devops, is_github_copilot, is_ml, is_security,
                 sections_bitmask, content_hash, ai_metadata, created_at)
            VALUES (@Slug, @Collection, @Title, '', '', 1743278400,
                    'ai', @ExternalUrl, 'Author', 'Feed', '',
                    @IsAi, @IsAzure, @IsDotnet, @IsDevops, @IsGhc, @IsMl, @IsSecurity,
                    @Bitmask, @Hash, @AiMetadata::jsonb, @CreatedAt)
            ON CONFLICT (collection_name, slug) DO UPDATE SET
                ai_metadata = EXCLUDED.ai_metadata,
                title       = EXCLUDED.title,
                is_ai       = EXCLUDED.is_ai,
                is_azure    = EXCLUDED.is_azure,
                is_dotnet   = EXCLUDED.is_dotnet,
                is_devops   = EXCLUDED.is_devops,
                is_github_copilot = EXCLUDED.is_github_copilot,
                is_ml       = EXCLUDED.is_ml,
                is_security = EXCLUDED.is_security,
                sections_bitmask = EXCLUDED.sections_bitmask,
                created_at  = EXCLUDED.created_at
            """,
            new
            {
                Slug = slug,
                Collection = collection,
                Title = title,
                ExternalUrl = externalUrl,
                IsAi = isAi,
                IsAzure = isAzure,
                IsDotnet = isDotnet,
                IsDevops = isDevops,
                IsGhc = isGhc,
                IsMl = isMl,
                IsSecurity = isSecurity,
                Bitmask = bitmask,
                Hash = $"hash-{slug}",
                AiMetadata = aiMetadata,
                CreatedAt = ts
            });
    }
}
