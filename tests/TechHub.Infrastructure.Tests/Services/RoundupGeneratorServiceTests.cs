using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Services;
using TechHub.TestUtilities;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Integration tests for <see cref="RoundupGeneratorService"/>.
/// Uses a real PostgreSQL database (via Testcontainers) for DB operations
/// while mocking the AI client and section roundup repository.
/// </summary>
public class RoundupGeneratorServiceTests
    : IClassFixture<DatabaseFixture<RoundupGeneratorServiceTests>>
{
    private readonly DatabaseFixture<RoundupGeneratorServiceTests> _fixture;
    private readonly Mock<ISectionRoundupRepository> _roundupRepo = new();
    private readonly Mock<IAiCompletionClient> _aiClient = new();

    private static readonly DateOnly WeekStart = new(2025, 4, 7);    // Monday
    private static readonly DateOnly WeekEnd = new(2025, 4, 13);     // Sunday
    private static readonly string ExpectedSlug = "Weekly-AI-and-Tech-News-Roundup-2025-04-14";

    public RoundupGeneratorServiceTests(DatabaseFixture<RoundupGeneratorServiceTests> fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);
        _fixture = fixture;
    }

    private RoundupGeneratorService CreateSut(RoundupGeneratorOptions? opts = null) =>
        new(
            _roundupRepo.Object,
            _aiClient.Object,
            _fixture.Connection,
            Options.Create(opts ?? new RoundupGeneratorOptions
            {
                Enabled = true,
                RunHourUtc = 8,
                MinHighArticlesPerSection = 3,
                MinTotalArticlesPerSection = 5,
                RateLimitDelaySeconds = 0,
                MaxRetries = 1
            }),
            NullLogger<RoundupGeneratorService>.Instance);

    // ── Deduplication ─────────────────────────────────────────────────────────

    [Fact]
    public async Task GenerateAsync_WhenRoundupAlreadyExists_ReturnsFalseWithoutCallingAi()
    {
        // Arrange — pre-insert a roundup with the same slug
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch,
                 primary_section_name, external_url, author, feed_name, tags_csv,
                 sections_bitmask, content_hash)
            VALUES (@Slug, 'roundups', 'Existing Roundup', '', '', 1744588800,
                    'github-copilot', '/all/roundups/Weekly-AI-and-Tech-News-Roundup-2025-04-14',
                    'TechHub', 'TechHub', '', 127, 'existing-hash')
            ON CONFLICT (collection_name, slug) DO NOTHING
            """,
            new { Slug = ExpectedSlug });

        var sut = CreateSut();

        // Act
        var result = await sut.GenerateAsync(WeekStart, WeekEnd);

        // Assert
        result.Should().BeFalse();
        _aiClient.VerifyNoOtherCalls();
    }

    // ── No Articles ───────────────────────────────────────────────────────────

    [Fact]
    public async Task GenerateAsync_WhenNoArticles_ReturnsFalseWithoutCallingAi()
    {
        // Arrange — use a unique week to avoid deduplication from other tests
        var emptyWeekStart = new DateOnly(2025, 2, 10);
        var emptyWeekEnd = new DateOnly(2025, 2, 16);

        _roundupRepo
            .Setup(r => r.GetArticlesForWeekAsync(emptyWeekStart, emptyWeekEnd, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<string, IReadOnlyList<RoundupArticle>>());

        var sut = CreateSut();

        // Act
        var result = await sut.GenerateAsync(emptyWeekStart, emptyWeekEnd);

        // Assert
        result.Should().BeFalse();
        _aiClient.VerifyNoOtherCalls();
    }

    // ── Full Pipeline ─────────────────────────────────────────────────────────

    [Fact]
    public async Task GenerateAsync_WithArticles_WritesRoundupToDatabase()
    {
        // Arrange — use a unique week
        var uniqueWeekStart = new DateOnly(2025, 5, 5);
        var uniqueWeekEnd = new DateOnly(2025, 5, 11);
        var uniqueSlug = "Weekly-AI-and-Tech-News-Roundup-2025-05-12";

        // Ensure slug doesn't exist
        await _fixture.Connection.ExecuteAsync(
            "DELETE FROM content_items WHERE collection_name = 'roundups' AND slug = @Slug",
            new { Slug = uniqueSlug });

        var articles = new Dictionary<string, IReadOnlyList<RoundupArticle>>
        {
            ["github-copilot"] = BuildArticles("github-copilot", 4, "high")
        };

        _roundupRepo
            .Setup(r => r.GetArticlesForWeekAsync(uniqueWeekStart, uniqueWeekEnd, It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);

        // AI client returns valid responses for all steps
        SetupAiForAllSteps(
            step3Content: "## GitHub Copilot\n\nThis week GitHub Copilot gained new features.\n\n- [Article 0](https://example.com/0)\n- [Article 1](https://example.com/1)",
            step4Content: "## GitHub Copilot\n\nBuilding on last week, GitHub Copilot continues to evolve.\n\n- [Article 0](https://example.com/0)\n- [Article 1](https://example.com/1)",
            step6Content: "## GitHub Copilot\n\nGitHub Copilot gained new features.\n\n- [Article 0](https://example.com/0)\n- [Article 1](https://example.com/1)",
            step7Metadata: "{\"title\": \"AI Test Roundup\", \"tags\": [\"AI\", \"GitHub Copilot\"], \"description\": \"A concise test roundup.\", \"introduction\": \"Welcome to this week's roundup.\"}"
        );

        var sut = CreateSut();

        // Act
        var result = await sut.GenerateAsync(uniqueWeekStart, uniqueWeekEnd);

        // Assert
        result.Should().BeTrue();

        var savedRoundup = await _fixture.Connection.QueryFirstOrDefaultAsync<RoundupDbRow>(
            "SELECT slug, title, content, excerpt FROM content_items WHERE collection_name = 'roundups' AND slug = @Slug",
            new { Slug = uniqueSlug });

        savedRoundup.Should().NotBeNull();
        savedRoundup!.Slug.Should().Be(uniqueSlug);
        savedRoundup.Title.Should().NotBeNullOrWhiteSpace();
    }

    // ── Relevance Filtering ───────────────────────────────────────────────────

    [Fact]
    public async Task GenerateAsync_WithOnlyHighArticles_UsesHighRelevanceOnly()
    {
        // Arrange — section has 4 high articles (above MinHighArticlesPerSection=3)
        var uniqueWeekStart = new DateOnly(2025, 6, 2);
        var uniqueWeekEnd = new DateOnly(2025, 6, 8);

        var articles = new Dictionary<string, IReadOnlyList<RoundupArticle>>
        {
            ["ai"] = BuildArticles("ai", 4, "high")
                .Concat(BuildArticles("ai", 3, "medium", startIndex: 4))
                .ToList()
        };

        _roundupRepo
            .Setup(r => r.GetArticlesForWeekAsync(uniqueWeekStart, uniqueWeekEnd, It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);

        var capturedMessages = new List<string>();

        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<string, CancellationToken>((body, _) => capturedMessages.Add(body))
            .ReturnsAsync(OkAiResponse("## AI\n\nThis week in AI.\n\n- [Article 0](https://example.com/0)"));

        var sut = CreateSut();

        // Act — call GenerateAsync (it will fail after step 3 due to incomplete mocking, but that's OK)
        await sut.GenerateAsync(uniqueWeekStart, uniqueWeekEnd);

        // Assert — first AI call (step 3) user message should contain exactly 4 articles (all high, no medium)
        capturedMessages.Should().NotBeEmpty();
        var messageBody = System.Text.Json.JsonDocument.Parse(capturedMessages[0]);
        var userContent = messageBody.RootElement
            .GetProperty("messages")[1]
            .GetProperty("content")
            .GetString();

        // 4 high articles = 4 "ARTICLE:" entries; medium articles are excluded
        var articleCount = CountOccurrences(userContent ?? "", "ARTICLE:");
        articleCount.Should().Be(4, "filtering should include only high when count >= MinHighArticlesPerSection");
    }

    [Fact]
    public async Task GenerateAsync_WithFewHighArticles_IncludesMediumArticles()
    {
        // Arrange — section has only 1 high article (below MinHighArticlesPerSection=3)
        var uniqueWeekStart = new DateOnly(2025, 6, 9);
        var uniqueWeekEnd = new DateOnly(2025, 6, 15);

        var articles = new Dictionary<string, IReadOnlyList<RoundupArticle>>
        {
            ["azure"] = BuildArticles("azure", 1, "high")
                .Concat(BuildArticles("azure", 4, "medium", startIndex: 1))
                .ToList()
        };

        _roundupRepo
            .Setup(r => r.GetArticlesForWeekAsync(uniqueWeekStart, uniqueWeekEnd, It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);

        var capturedMessages2 = new List<string>();

        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<string, CancellationToken>((body, _) => capturedMessages2.Add(body))
            .ReturnsAsync(OkAiResponse("## Azure\n\nAzure news.\n\n- [Article](https://example.com)"));

        var sut = CreateSut();

        // Act
        await sut.GenerateAsync(uniqueWeekStart, uniqueWeekEnd);

        // Assert — should contain 5 articles total (1 high + 4 medium, capped at MinTotalArticlesPerSection=5)
        capturedMessages2.Should().NotBeEmpty();
        var doc = System.Text.Json.JsonDocument.Parse(capturedMessages2[0]);
        var userContent = doc.RootElement
            .GetProperty("messages")[1]
            .GetProperty("content")
            .GetString();

        var articleCount = CountOccurrences(userContent ?? "", "ARTICLE:");
        articleCount.Should().Be(5, "1 high + 4 medium = 5, capped at MinTotalArticlesPerSection=5");
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static IReadOnlyList<RoundupArticle> BuildArticles(
        string section,
        int count,
        string relevance,
        int startIndex = 0) =>
        Enumerable.Range(startIndex, count).Select(i => new RoundupArticle
        {
            SectionName = section,
            Title = $"Article {i}",
            ExternalUrl = $"https://example.com/{i}",
            Slug = $"article-{section}-{i}",
            CollectionName = "news",
            IsInternal = false,
            Summary = $"Summary of article {i}",
            KeyTopics = ["testing"],
            Relevance = relevance,
            TopicType = "news",
            ImpactLevel = "medium",
            TimeSensitivity = "this-week"
        }).ToList();

    private void SetupAiForAllSteps(
        string step3Content,
        string step4Content,
        string step6Content,
        string step7Metadata)
    {
        var callIndex = 0;
        var responses = new[]
        {
            OkAiResponse(step3Content),  // Step 3
            OkAiResponse(step4Content),  // Step 4
            OkAiResponse(step6Content),  // Step 6
            OkAiResponse(step7Metadata), // Step 7
        };

        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => responses[Math.Min(callIndex++, responses.Length - 1)]);
    }

    private static AiCompletionResult OkAiResponse(string content)
    {
        var serialized = System.Text.Json.JsonSerializer.Serialize(content);
        return new(IsRateLimited: false,
            ResponseBody: $$$"""{"choices":[{"message":{"content":{{{serialized}}}}}]}""");
    }

    private static int CountOccurrences(string text, string pattern)
    {
        var count = 0;
        var index = 0;
        while ((index = text.IndexOf(pattern, index, StringComparison.Ordinal)) >= 0)
        {
            count++;
            index += pattern.Length;
        }

        return count;
    }

    private sealed class RoundupDbRow
    {
        public string Slug { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string Content { get; init; } = string.Empty;
        public string Excerpt { get; init; } = string.Empty;
    }

    // ── Tag Expansion for Roundups ────────────────────────────────────────────

    [Fact]
    public async Task GenerateAsync_WithArticles_PopulatesContentTagsExpanded()
    {
        // Arrange — use a unique week
        var uniqueWeekStart = new DateOnly(2025, 7, 7);
        var uniqueWeekEnd = new DateOnly(2025, 7, 13);
        var uniqueSlug = "Weekly-AI-and-Tech-News-Roundup-2025-07-14";

        await _fixture.Connection.ExecuteAsync(
            "DELETE FROM content_items WHERE collection_name = 'roundups' AND slug = @Slug",
            new { Slug = uniqueSlug });

        var articles = new Dictionary<string, IReadOnlyList<RoundupArticle>>
        {
            ["ai"] = BuildArticles("ai", 4, "high")
        };

        _roundupRepo
            .Setup(r => r.GetArticlesForWeekAsync(uniqueWeekStart, uniqueWeekEnd, It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);

        // Use body-inspecting callback: step 7 requests contain "Return only JSON"
        var step7Json = """{"title": "Tag Test Roundup", "tags": ["AI", "Machine Learning", "Azure OpenAI"], "description": "A test.", "introduction": "Welcome."}""";
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string body, CancellationToken _) =>
                body.Contains("Return only JSON", StringComparison.Ordinal)
                    ? OkAiResponse(step7Json)
                    : OkAiResponse("## AI\n\nAI news this week.\n\n- [Article 0](https://example.com/0)"));

        var sut = CreateSut();

        // Act
        var result = await sut.GenerateAsync(uniqueWeekStart, uniqueWeekEnd);

        // Assert
        result.Should().BeTrue();

        var tagCount = await _fixture.Connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM content_tags_expanded WHERE collection_name = 'roundups' AND slug = @Slug",
            new { Slug = uniqueSlug });

        tagCount.Should().BeGreaterThanOrEqualTo(3, "roundup should have at least 3 tags written");

        var tagWords = (await _fixture.Connection.QueryAsync<string>(
            "SELECT tag_word FROM content_tags_expanded WHERE collection_name = 'roundups' AND slug = @Slug ORDER BY tag_word",
            new { Slug = uniqueSlug })).ToList();

        tagWords.Should().Contain("ai");
        tagWords.Should().Contain("machine learning");
        tagWords.Should().Contain("azure openai");
    }

    // ── Step 4: Ongoing Narrative with Previous Roundup ───────────────────────

    [Fact]
    public async Task GenerateAsync_WithPreviousRoundup_InvokesStep4WithPreviousContent()
    {
        // Arrange — insert a previous roundup then generate a new one.
        // Use far-future dates to avoid interference from other tests' roundup data.
        var previousSlug = "Weekly-AI-and-Tech-News-Roundup-2026-06-08";
        var currentWeekStart = new DateOnly(2026, 6, 8);
        var currentWeekEnd = new DateOnly(2026, 6, 14);
        var currentSlug = "Weekly-AI-and-Tech-News-Roundup-2026-06-15";

        // Clean up
        await _fixture.Connection.ExecuteAsync(
            "DELETE FROM content_items WHERE collection_name = 'roundups' AND slug IN (@Prev, @Curr)",
            new { Prev = previousSlug, Curr = currentSlug });

        // Insert a previous week's roundup with content containing an AI section.
        // Publish epoch must be < currentWeekStart converted to Brussels time.
        var brusselsZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Brussels");
        var prevPublishDt = new DateTime(2026, 6, 1, 9, 0, 0); // Monday 1 June
        var prevPublishUtc = TimeZoneInfo.ConvertTimeToUtc(prevPublishDt, brusselsZone);
        var prevEpoch = (long)prevPublishUtc.Subtract(DateTime.UnixEpoch).TotalSeconds;

        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch,
                 primary_section_name, external_url, author, feed_name, tags_csv,
                 is_ai, is_azure, is_dotnet, is_devops, is_github_copilot, is_ml, is_security,
                 sections_bitmask, content_hash)
            VALUES (@Slug, 'roundups', 'Previous Roundup', @Content, 'Previous intro', @Epoch,
                    'github-copilot', '/all/roundups/' || @Slug, 'TechHub', 'TechHub', '',
                    TRUE, TRUE, TRUE, TRUE, TRUE, TRUE, TRUE, 127, 'prev-hash')
            ON CONFLICT (collection_name, slug) DO NOTHING
            """,
            new
            {
                Slug = previousSlug,
                Content = "## AI\n\nLast week, Azure AI Foundry released new features.\n\n- [Last Week Article](https://example.com/prev)",
                Epoch = prevEpoch
            });

        var articles = new Dictionary<string, IReadOnlyList<RoundupArticle>>
        {
            ["ai"] = BuildArticles("ai", 4, "high")
        };

        _roundupRepo
            .Setup(r => r.GetArticlesForWeekAsync(currentWeekStart, currentWeekEnd, It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);

        // Capture AI calls to verify Step 4 receives previous roundup content
        var capturedBodies = new List<string>();
        var step7Metadata = """{"title": "Step 4 Test", "tags": ["AI"], "description": "Test.", "introduction": "Intro."}""";

        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<string, CancellationToken>((body, _) => capturedBodies.Add(body))
            .ReturnsAsync((string body, CancellationToken _) =>
                body.Contains("Return only JSON", StringComparison.Ordinal)
                    ? OkAiResponse(step7Metadata)
                    : OkAiResponse("## AI\n\nThis week in AI.\n\n- [Article 0](https://example.com/0)"));

        var sut = CreateSut();

        // Act
        var result = await sut.GenerateAsync(currentWeekStart, currentWeekEnd);

        // Assert
        result.Should().BeTrue();

        // Find the Step 4 call — its user message contains "PREVIOUS WEEK"
        string? step4UserContent = null;
        foreach (var body in capturedBodies)
        {
            using var doc = System.Text.Json.JsonDocument.Parse(body);
            var userMsg = doc.RootElement
                .GetProperty("messages")[1]
                .GetProperty("content")
                .GetString();
            if (userMsg?.Contains("PREVIOUS WEEK", StringComparison.Ordinal) == true)
            {
                step4UserContent = userMsg;
                break;
            }
        }

        step4UserContent.Should().NotBeNull("Step 4 should have been called with previous roundup content");
        step4UserContent.Should().Contain("Azure AI Foundry", "Step 4 should contain text from the previous roundup");
    }

    // ── Step 7: Fallback Metadata ─────────────────────────────────────────────

    [Fact]
    public async Task GenerateAsync_WhenStep7ReturnsInvalidJson_UsesFallbackMetadata()
    {
        // Arrange — use a unique week
        var uniqueWeekStart = new DateOnly(2025, 9, 1);
        var uniqueWeekEnd = new DateOnly(2025, 9, 7);
        var uniqueSlug = "Weekly-AI-and-Tech-News-Roundup-2025-09-08";

        await _fixture.Connection.ExecuteAsync(
            "DELETE FROM content_items WHERE collection_name = 'roundups' AND slug = @Slug",
            new { Slug = uniqueSlug });

        var articles = new Dictionary<string, IReadOnlyList<RoundupArticle>>
        {
            ["ai"] = BuildArticles("ai", 4, "high")
        };

        _roundupRepo
            .Setup(r => r.GetArticlesForWeekAsync(uniqueWeekStart, uniqueWeekEnd, It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);

        // Steps 3-6 return markdown, Step 7 returns invalid JSON every time
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string body, CancellationToken _) =>
                body.Contains("Return only JSON", StringComparison.Ordinal)
                    ? OkAiResponse("This is NOT valid JSON at all")
                    : OkAiResponse("## AI\n\nAI news.\n\n- [Article](https://example.com/0)"));

        var sut = CreateSut();

        // Act
        var result = await sut.GenerateAsync(uniqueWeekStart, uniqueWeekEnd);

        // Assert — should still succeed using fallback metadata
        result.Should().BeTrue();

        var savedRoundup = await _fixture.Connection.QueryFirstOrDefaultAsync<RoundupDbRow>(
            "SELECT slug, title, content, excerpt FROM content_items WHERE collection_name = 'roundups' AND slug = @Slug",
            new { Slug = uniqueSlug });

        savedRoundup.Should().NotBeNull();
        savedRoundup!.Title.Should().Contain("Weekly AI and Tech News Roundup");
    }
}
