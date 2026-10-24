using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Repositories;

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

    // ── GetArticlesForSectionForWeekAsync ──────────────────────────────────────

    [Fact]
    public async Task GetArticlesForSectionForWeekAsync_WithMatchingItems_ReturnsGroupedArticles()
    {
        // Arrange
        await SeedContentItemAsync("test-article-1", "news", "Test Article 1",
            "https://example.com/1", sections: ["ai"],
            relevance: "high", summary: "Summary of article 1", keyTopics: ["AI", "Testing"],
            createdAt: new DateTime(2025, 3, 25, 10, 0, 0, DateTimeKind.Utc));

        // Act
        var result = await GetAllSectionsAsync();

        // Assert
        result.Should().ContainKey("ai");
        var articles = result["ai"];
        var article = articles.Should().Contain(a => a.Slug == "test-article-1").Subject;
        article.Title.Should().Be("Test Article 1");
        article.ExternalUrl.Should().Be("https://example.com/1");
        article.Relevance.Should().Be("high");
        article.Summary.Should().Be("Summary of article 1");
        article.KeyTopics.Should().Equal(["AI", "Testing"]);
        article.IsInternal.Should().BeFalse();
    }

    [Fact]
    public async Task GetArticlesForSectionForWeekAsync_WithInternalCollection_SetsIsInternalTrue()
    {
        // Arrange
        await SeedContentItemAsync("test-video-1", "videos", "Test Video 1",
            "/videos/watch?v=abc", sections: ["ai"],
            relevance: "medium",
            createdAt: new DateTime(2025, 3, 26, 12, 0, 0, DateTimeKind.Utc));

        // Act
        var result = await GetAllSectionsAsync();

        // Assert
        result.Should().ContainKey("ai");
        var article = result["ai"].First(a => a.Slug == "test-video-1");
        article.IsInternal.Should().BeTrue();
    }

    [Fact]
    public async Task GetArticlesForSectionForWeekAsync_WithMultipleSections_GroupsBySection()
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
        var result = await GetAllSectionsAsync();

        // Assert
        result.Should().ContainKey("azure");
        result.Should().ContainKey("dotnet");
        result["azure"].Should().Contain(a => a.Slug == "azure-article-1");
        result["dotnet"].Should().Contain(a => a.Slug == "dotnet-article-1");
    }

    [Fact]
    public async Task GetArticlesForSectionForWeekAsync_ReturnsTheStoredExcerpt()
    {
        await SeedContentItemAsync("excerpt-article", "news", "Excerpt Article",
            "https://example.com/excerpt", sections: ["ai"],
            excerpt: "GA ships on 1 June 2026.");

        var articles = await _sut.GetArticlesForSectionForWeekAsync("ai", _weekStart, _weekEnd, CancellationToken.None);

        articles.Should().Contain(a => a.Slug == "excerpt-article").Which.Excerpt.Should().Be("GA ships on 1 June 2026.");
    }

    [Fact]
    public async Task GetArticlesForSectionForWeekAsync_OutsideDateRange_ReturnsEmpty()
    {
        // Arrange — item created outside the queried week
        await SeedContentItemAsync("out-of-range-article", "news", "Out of Range Article",
            "https://example.com/out", sections: ["ai"],
            relevance: "high",
            createdAt: new DateTime(2025, 1, 7, 10, 0, 0, DateTimeKind.Utc));

        // Act — query a week that doesn't contain the article
        var result = await GetAllSectionsAsync(new DateOnly(2025, 3, 24), new DateOnly(2025, 3, 30));

        // Assert
        if (result.TryGetValue("ai", out var articles))
        {
            articles.Should().NotContain(a => a.Slug == "out-of-range-article");
        }
    }

    [Fact]
    public async Task GetArticlesForSectionForWeekAsync_WithNullAiMetadata_IncludesItemWithNeedsFlag()
    {
        // Arrange — no ai_metadata
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch,
                 primary_section_name, external_url, author, feed_name, tags_csv,
                 sections_bitmask, content_hash, is_ai, created_at)
            VALUES ('no-meta-article', 'news', 'No Meta Article', '', '', 1711699200,
                    'ai', 'https://example.com/no-meta', 'Author', 'Feed', '', 1, 'hash-no-meta',
                    TRUE, '2025-03-25T10:00:00Z')
            ON CONFLICT (collection_name, slug) DO NOTHING
            """);

        // Act
        var result = await GetAllSectionsAsync();

        // Assert — item is included with NeedsAiMetadata flag
        result.Should().ContainKey("ai");
        var articles = result["ai"];
        var article = articles.Should().Contain(a => a.Slug == "no-meta-article").Subject;
        article.NeedsAiMetadata.Should().BeTrue();
    }

    [Fact]
    public async Task GetArticlesForSectionForWeekAsync_WithAllRelevanceLevels_ReturnsAll()
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
        var result = await GetAllSectionsAsync();

        // Assert — repository returns all levels unfiltered
        result.Should().ContainKey("security");
        var articles = result["security"];
        articles.Should().Contain(a => a.Slug == "relevance-high");
        articles.Should().Contain(a => a.Slug == "relevance-medium");
        articles.Should().Contain(a => a.Slug == "relevance-low");
    }

    [Fact]
    public async Task GetArticlesForSectionForWeekAsync_WithSchemaVersion2Metadata_ParsesRolesAndDoesNotNeedBackfill()
    {
        // Arrange
        await SeedContentItemAsync("v2-article", "news", "V2 Article",
            "https://example.com/v2-article", sections: ["azure", "devops"],
            relevance: "medium",
            createdAt: new DateTime(2025, 3, 27, 12, 0, 0, DateTimeKind.Utc));

        var metadata = ContentItemWriteRepository.SerializeAiMetadata(new TechHub.Core.Models.ContentProcessing.RoundupMetadata
        {
            SchemaVersion = 2,
            KeyFacts = ["Fact one.", "Fact two."],
            Significance = "high",
            ContentKind = "announcement",
            Lifecycle = "ga",
            Products = ["Azure Functions"],
            SectionRoles = new Dictionary<string, string> { ["azure"] = "lead", ["devops"] = "brief" }
        });
        await _fixture.Connection.ExecuteAsync(
            "UPDATE content_items SET ai_metadata = @Metadata::jsonb WHERE slug = 'v2-article'",
            new { Metadata = metadata });

        // Act
        var articles = await _sut.GetArticlesForSectionForWeekAsync("devops", _weekStart, _weekEnd, CancellationToken.None);

        // Assert
        var article = articles.Should().ContainSingle(a => a.Slug == "v2-article").Subject;
        article.NeedsAiMetadata.Should().BeFalse();
        article.KeyFacts.Should().Equal("Fact one.", "Fact two.");
        article.Significance.Should().Be("high");
        article.Lifecycle.Should().Be("ga");
        article.Products.Should().Equal("Azure Functions");
        article.SectionRoles["devops"].Should().Be("brief");
        article.SectionRoles["azure"].Should().Be("lead");
    }

    [Fact]
    public async Task GetArticlesForSectionForWeekAsync_WithSchemaVersion1Metadata_NeedsBackfill()
    {
        // Arrange — the seed helper writes the original (version 1) summary-based metadata
        await SeedContentItemAsync("v1-article", "news", "V1 Article",
            "https://example.com/v1-article", sections: ["devops"],
            relevance: "medium", summary: "Old summary",
            createdAt: new DateTime(2025, 3, 27, 13, 0, 0, DateTimeKind.Utc));

        // Act
        var articles = await _sut.GetArticlesForSectionForWeekAsync("devops", _weekStart, _weekEnd, CancellationToken.None);

        // Assert
        var article = articles.Should().ContainSingle(a => a.Slug == "v1-article").Subject;
        article.NeedsAiMetadata.Should().BeTrue();
        article.Summary.Should().Be("Old summary");
    }

    [Fact]
    public async Task GetArticlesForSectionForWeekAsync_FiltersBySectionFlag()
    {
        // Arrange — primary section is azure, but item is also marked devops via section flags
        await SeedContentItemAsync("devops-flagged", "news", "DevOps Flagged",
            "https://example.com/devops-flagged", sections: ["azure", "devops"],
            relevance: "medium",
            createdAt: new DateTime(2025, 3, 27, 10, 0, 0, DateTimeKind.Utc));

        await SeedContentItemAsync("azure-only", "news", "Azure Only",
            "https://example.com/azure-only", sections: ["azure"],
            relevance: "medium",
            createdAt: new DateTime(2025, 3, 27, 11, 0, 0, DateTimeKind.Utc));

        // Act
        var devopsArticles = await _sut.GetArticlesForSectionForWeekAsync("devops", _weekStart, _weekEnd, CancellationToken.None);

        // Assert — selection is based on is_devops, not primary_section_name
        devopsArticles.Should().Contain(a => a.Slug == "devops-flagged");
        devopsArticles.Should().NotContain(a => a.Slug == "azure-only");
    }

    [Fact]
    public async Task GetArticlesForSectionForWeekAsync_ExcludesRoundups()
    {
        // Arrange — insert a roundup in the same week
        await SeedContentItemAsync("existing-roundup", "roundups", "Weekly Roundup",
            "/all/roundups/existing-roundup", sections: ["ai"],
            relevance: "high",
            createdAt: new DateTime(2025, 3, 28, 9, 0, 0, DateTimeKind.Utc));

        // Act
        var result = await GetAllSectionsAsync();

        // Assert — roundups are excluded to prevent circular inclusion
        if (result.TryGetValue("ai", out var articles))
        {
            articles.Should().NotContain(a => a.Slug == "existing-roundup");
        }
    }

    // ── GetPreviousRoundupContentAsync ────────────────────────────────────────

    [Fact]
    public async Task GetPreviousRoundupContentAsync_ReturnsTheRoundupPublishedOnTheMondayOfTheWeekStart()
    {
        // Arrange — roundups are stamped 09:00 on the Monday they are published.
        // The roundup published on Monday 24 March (09:00) covers the week before and is the previous one
        // when generating the week that starts on Monday 24 March.
        await _fixture.Connection.ExecuteAsync("DELETE FROM content_items WHERE collection_name = 'roundups' AND primary_section_name = 'ml'");
        await _sut.WriteRoundupAsync("ml", "weekly-ml-roundup-2025-03-17", new DateOnly(2025, 3, 17), "Two weeks back", "Older content", "intro", ["ML"]);
        await _sut.WriteRoundupAsync("ml", "weekly-ml-roundup-2025-03-24", new DateOnly(2025, 3, 24), "Last week", "Last week content", "intro", ["ML"]);
        await _sut.WriteRoundupAsync("ml", "weekly-ml-roundup-2025-03-31", new DateOnly(2025, 3, 31), "This week", "This week content", "intro", ["ML"]);

        // Act
        var previous = await _sut.GetPreviousRoundupContentAsync("ml", _weekStart, CancellationToken.None);

        // Assert
        previous.Should().Contain("Last week content");
    }

    // ── GetUpcomingDateEventsAsync ────────────────────────────────────────────

    [Fact]
    public async Task GetUpcomingDateEventsAsync_ReturnsOnlyEventsAfterTheWeekFromArticlesOfTheSectionAndWeek()
    {
        // Arrange
        await _fixture.Connection.ExecuteAsync("DELETE FROM content_items WHERE slug LIKE 'dates-%'");
        await SeedContentItemAsync("dates-in-week", "news", "Dates In Week", "https://example.com/dates-1", sections: ["security"],
            relevance: "high", createdAt: new DateTime(2025, 3, 25, 10, 0, 0, DateTimeKind.Utc));
        await SeedContentItemAsync("dates-other-section", "news", "Other Section", "https://example.com/dates-2", sections: ["ml"],
            relevance: "high", createdAt: new DateTime(2025, 3, 25, 10, 0, 0, DateTimeKind.Utc));
        await SeedContentItemAsync("dates-old-article", "news", "Old Article", "https://example.com/dates-3", sections: ["security"],
            relevance: "high", createdAt: new DateTime(2025, 2, 10, 10, 0, 0, DateTimeKind.Utc));

        async Task AddEvent(string slug, string kind, string date) => await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_timeline_events
                (collection_name, item_slug, kind, product, description, event_date, date_precision, date_source, extracted_from)
            VALUES ('news', @Slug, @Kind, 'Product', 'Something happens', @Date::date, 'day', 'explicit', 'source')
            """, new { Slug = slug, Kind = kind, Date = date });

        await AddEvent("dates-in-week", "retirement", "2025-06-01");
        await AddEvent("dates-in-week", "ga", "2025-03-28");          // inside the week: not upcoming
        await AddEvent("dates-other-section", "retirement", "2025-07-01");
        await AddEvent("dates-old-article", "retirement", "2025-08-01");

        // Act
        var events = await _sut.GetUpcomingDateEventsAsync("security", _weekStart, _weekEnd, CancellationToken.None);

        // Assert
        var ev = events.Should().ContainSingle().Subject;
        ev.Kind.Should().Be("retirement");
        ev.Date.Should().Be(new DateOnly(2025, 6, 1));
        ev.SourceTitle.Should().Be("Dates In Week");
        ev.SourceUrl.Should().Be("https://example.com/dates-1");
    }

    // ── Helpers ────────────────────────────────────────────────────────────────

    private static readonly string[] _allSections = ["ai", "azure", "dotnet", "devops", "github-copilot", "ml", "security"];

    /// <summary>Queries every section and returns the sections that have articles, keyed by section.</summary>
    private async Task<Dictionary<string, IReadOnlyList<RoundupArticle>>> GetAllSectionsAsync(DateOnly? weekStart = null, DateOnly? weekEnd = null)
    {
        var result = new Dictionary<string, IReadOnlyList<RoundupArticle>>();
        foreach (var section in _allSections)
        {
            var articles = await _sut.GetArticlesForSectionForWeekAsync(
                section, weekStart ?? _weekStart, weekEnd ?? _weekEnd, CancellationToken.None);
            if (articles.Count > 0)
            {
                result[section] = articles;
            }
        }

        return result;
    }

    private async Task SeedContentItemAsync(
        string slug,
        string collection,
        string title,
        string externalUrl,
        IReadOnlyList<string> sections,
        string relevance = "high",
        string summary = "",
        IEnumerable<string>? keyTopics = null,
        DateTime? createdAt = null,
        string excerpt = "")
    {
        var topics = keyTopics?.ToList() ?? [];

        var sectionSet = new HashSet<string>(sections, StringComparer.OrdinalIgnoreCase);
        var primarySection = sections[0];
        var isAi = sectionSet.Contains("ai");
        var isAzure = sectionSet.Contains("azure");
        var isDotnet = sectionSet.Contains("dotnet");
        var isDevops = sectionSet.Contains("devops");
        var isGhc = sectionSet.Contains("github-copilot");
        var isMl = sectionSet.Contains("ml");
        var isSecurity = sectionSet.Contains("security");

        var bitmask = 0;
        if (isAi)
        {
            bitmask |= 1;
        }

        if (isAzure)
        {
            bitmask |= 2;
        }

        if (isDotnet)
        {
            bitmask |= 4;
        }

        if (isDevops)
        {
            bitmask |= 8;
        }

        if (isGhc)
        {
            bitmask |= 16;
        }

        if (isMl)
        {
            bitmask |= 32;
        }

        if (isSecurity)
        {
            bitmask |= 64;
        }

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
            VALUES (@Slug, @Collection, @Title, '', @Excerpt, 1743278400,
                    @PrimarySection, @ExternalUrl, 'Author', 'Feed', '',
                    @IsAi, @IsAzure, @IsDotnet, @IsDevops, @IsGhc, @IsMl, @IsSecurity,
                    @Bitmask, @Hash, @AiMetadata::jsonb, @CreatedAt)
            ON CONFLICT (collection_name, slug) DO UPDATE SET
                ai_metadata = EXCLUDED.ai_metadata,
                title       = EXCLUDED.title,
                excerpt     = EXCLUDED.excerpt,
                primary_section_name = EXCLUDED.primary_section_name,
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
                Excerpt = excerpt,
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
                PrimarySection = primarySection,
                AiMetadata = aiMetadata,
                CreatedAt = ts
            });
    }

    // ── WriteRoundupAsync ─────────────────────────────────────────────────────

    [Fact]
    public async Task WriteRoundupAsync_InsertsContentItemAndProcessedUrlAndTagsExpanded()
    {
        // Arrange
        var sectionName = "ai";
        var slug = "weekly-ai-roundup-2025-04-07";
        var publishDate = new DateOnly(2025, 4, 7);
        var tags = new List<string> { "AI", "GitHub Copilot", "Roundups" };

        // Act
        await _sut.WriteRoundupAsync(
            sectionName, slug, publishDate,
            title: "Test Roundup Title",
            content: "## AI\n\nTest content.",
            introduction: "Welcome to the roundup.",
            tags: tags,
            ct: CancellationToken.None);

        // Assert — content_items
        var item = await _fixture.Connection.QueryFirstOrDefaultAsync<dynamic>(
            "SELECT title, excerpt, tags_csv, primary_section_name, collection_name FROM content_items WHERE slug = @Slug AND collection_name = 'roundups'",
            new { Slug = slug });
        ((object?)item).Should().NotBeNull("content_items row should exist after WriteRoundupAsync");
        ((string)item!.title).Should().Be("Test Roundup Title");
        ((string)item.excerpt).Should().Be("Welcome to the roundup.");
        ((string)item.tags_csv).Should().Contain(",AI,");
        ((string)item.primary_section_name).Should().Be("ai");

        // Assert — processed_urls
        var processedUrl = await _fixture.Connection.QueryFirstOrDefaultAsync<dynamic>(
            "SELECT status, slug, reason FROM processed_urls WHERE external_url = @Url",
            new { Url = $"/ai/roundups/{slug}" });
        ((object?)processedUrl).Should().NotBeNull("processed_urls row should exist after WriteRoundupAsync");
        ((string)processedUrl!.status).Should().Be("succeeded");
        ((string)processedUrl.slug).Should().Be(slug);
        ((string)processedUrl.reason).Should().Be("roundup-generated");

        // Assert — content_tags_expanded
        var tagCount = await _fixture.Connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM content_tags_expanded WHERE slug = @Slug AND collection_name = 'roundups'",
            new { Slug = slug });
        tagCount.Should().BeGreaterThan(0);

        // Verify tag expansion: "GitHub Copilot" should produce full tag + word entries
        var githubCopilotFull = await _fixture.Connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM content_tags_expanded WHERE slug = @Slug AND tag_word = 'github copilot' AND is_full_tag = true",
            new { Slug = slug });
        githubCopilotFull.Should().Be(1);

        var githubWord = await _fixture.Connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM content_tags_expanded WHERE slug = @Slug AND tag_word = 'github' AND is_full_tag = false",
            new { Slug = slug });
        githubWord.Should().Be(1);
    }
}
