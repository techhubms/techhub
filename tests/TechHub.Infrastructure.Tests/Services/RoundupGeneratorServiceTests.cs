using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Services.RoundupGeneration;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Tests for <see cref="RoundupGeneratorService"/>.
/// Uses real step class instances with mocked AI client and repository.
/// </summary>
public class RoundupGeneratorServiceTests
{
    private readonly Mock<ISectionRoundupRepository> _roundupRepo = new();
    private readonly Mock<IAiCompletionClient> _aiClient = new();
    private readonly Mock<IAiCategorizationService> _aiCategorizationService = new();
    private readonly Mock<IContentItemWriteRepository> _writeRepo = new();

    private static readonly DateOnly _weekStart = new(2025, 4, 7);    // Monday
    private static readonly DateOnly _weekEnd = new(2025, 4, 13);     // Sunday
    private static readonly string _expectedSlug = "weekly-ai-and-tech-news-roundup-2025-04-14";

    private static readonly AppSettings _testAppSettings = new()
    {
        BaseUrl = "https://test.example.com",
        Content = new ContentSettings
        {
            CollectionsPath = "collections",
            Sections = new Dictionary<string, SectionConfig>
            {
                ["github-copilot"] = new()
                {
                    Title = "GitHub Copilot",
                    Description = "GitHub Copilot section",
                    Url = "/github-copilot",
                    Tag = "github-copilot",
                    Order = 1,
                    Collections = new Dictionary<string, CollectionConfig>
                    {
                        ["news"] = new() { Title = "News", Url = "/news", Description = "News" }
                    }
                },
                ["ai"] = new()
                {
                    Title = "AI",
                    Description = "AI section",
                    Url = "/ai",
                    Tag = "ai",
                    Order = 2,
                    Collections = new Dictionary<string, CollectionConfig>
                    {
                        ["news"] = new() { Title = "News", Url = "/news", Description = "News" }
                    }
                },
                ["azure"] = new()
                {
                    Title = "Azure",
                    Description = "Azure section",
                    Url = "/azure",
                    Tag = "azure",
                    Order = 3,
                    Collections = new Dictionary<string, CollectionConfig>
                    {
                        ["news"] = new() { Title = "News", Url = "/news", Description = "News" }
                    }
                }
            }
        }
    };

    private static readonly RoundupGeneratorOptions _defaultOptions = new()
    {
        RunHourUtc = 8,
        MinArticlesPerSection = 10,
        RateLimitDelaySeconds = 0,
        MaxRetries = 1
    };

    private RoundupGeneratorService CreateSut(RoundupGeneratorOptions? opts = null)
    {
        var options = opts ?? _defaultOptions;
        var appSettingsOptions = Options.Create(_testAppSettings);

        var aiHelper = new RoundupAiHelper(
            _aiClient.Object,
            options,
            NullLogger<RoundupAiHelper>.Instance);

        var relevanceFilter = new RoundupRelevanceFilter(
            options,
            appSettingsOptions);

        var newsWriter = new RoundupNewsWriter(
            aiHelper,
            appSettingsOptions,
            options,
            NullLogger<RoundupNewsWriter>.Instance);

        var narrativeEnhancer = new RoundupNarrativeEnhancer(
            aiHelper,
            _roundupRepo.Object,
            appSettingsOptions,
            options);

        var condenser = new RoundupCondenser(
            aiHelper,
            options,
            NullLogger<RoundupCondenser>.Instance);

        var metadataGenerator = new RoundupMetadataGenerator(
            aiHelper,
            options,
            NullLogger<RoundupMetadataGenerator>.Instance);

        var contentFixerMock = new Mock<IContentFixerService>();
        contentFixerMock.Setup(s => s.RepairMarkdown(It.IsAny<string>())).Returns((string c) => c);

        return new RoundupGeneratorService(
            _roundupRepo.Object,
            contentFixerMock.Object,
            _aiCategorizationService.Object,
            _writeRepo.Object,
            relevanceFilter,
            newsWriter,
            narrativeEnhancer,
            condenser,
            metadataGenerator,
            options,
            NullLogger<RoundupGeneratorService>.Instance);
    }

    // ── Deduplication ─────────────────────────────────────────────────────────

    [Fact]
    public async Task GenerateAsync_WhenRoundupAlreadyExists_ReturnsAlreadyExistsWithoutCallingAi()
    {
        // Arrange — mock that the roundup already exists
        _roundupRepo
            .Setup(r => r.RoundupExistsAsync(_expectedSlug, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var sut = CreateSut();

        // Act
        var result = await sut.GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        // Assert
        result.Result.Should().Be(RoundupGenerationResult.AlreadyExists);
        result.Slug.Should().BeNull();
        _aiClient.VerifyNoOtherCalls();
    }

    // ── No Articles ───────────────────────────────────────────────────────────

    [Fact]
    public async Task GenerateAsync_WhenNoArticles_ReturnsNoArticlesWithoutCallingAi()
    {
        // Arrange — use a unique week to avoid deduplication from other tests
        var emptyWeekStart = new DateOnly(2025, 2, 10);
        var emptyWeekEnd = new DateOnly(2025, 2, 16);

        _roundupRepo
            .Setup(r => r.GetArticlesForWeekAsync(emptyWeekStart, emptyWeekEnd, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<string, IReadOnlyList<RoundupArticle>>());

        var sut = CreateSut();

        // Act
        var result = await sut.GenerateAsync(emptyWeekStart, emptyWeekEnd, ct: TestContext.Current.CancellationToken);

        // Assert
        result.Result.Should().Be(RoundupGenerationResult.NoArticles);
        result.Slug.Should().BeNull();
        _aiClient.VerifyNoOtherCalls();
    }

    // ── Full Pipeline ─────────────────────────────────────────────────────────

    [Fact]
    public async Task GenerateAsync_WithArticles_WritesRoundupToDatabase()
    {
        // Arrange — use a unique week
        var uniqueWeekStart = new DateOnly(2025, 5, 5);
        var uniqueWeekEnd = new DateOnly(2025, 5, 11);
        var uniqueSlug = "weekly-ai-and-tech-news-roundup-2025-05-12";

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
        var result = await sut.GenerateAsync(uniqueWeekStart, uniqueWeekEnd, ct: TestContext.Current.CancellationToken);

        // Assert
        result.Result.Should().Be(RoundupGenerationResult.Generated);
        result.Slug.Should().Be(uniqueSlug);

        _roundupRepo.Verify(r => r.WriteRoundupAsync(
            It.Is<string>(s => s == uniqueSlug),
            It.IsAny<DateOnly>(),
            It.Is<string>(s => !string.IsNullOrWhiteSpace(s)),
            It.Is<string>(s => !string.IsNullOrWhiteSpace(s)),
            It.Is<string>(s => !string.IsNullOrWhiteSpace(s)),
            It.Is<string>(s => !string.IsNullOrWhiteSpace(s)),
            It.IsAny<IReadOnlyList<string>>(),
            It.IsAny<long?>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    // ── Relevance Filtering ───────────────────────────────────────────────────

    [Fact]
    public async Task GenerateAsync_WithOnlyHighArticles_UsesHighRelevanceOnly()
    {
        // Arrange — section has 12 high articles (above MinArticlesPerSection=10)
        var uniqueWeekStart = new DateOnly(2025, 6, 2);
        var uniqueWeekEnd = new DateOnly(2025, 6, 8);

        var articles = new Dictionary<string, IReadOnlyList<RoundupArticle>>
        {
            ["ai"] = BuildArticles("ai", 12, "high")
                .Concat(BuildArticles("ai", 5, "medium", startIndex: 12))
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

        // Act — call GenerateAsync (it will fail after step 1 due to incomplete mocking, but that's OK)
        await sut.GenerateAsync(uniqueWeekStart, uniqueWeekEnd, ct: TestContext.Current.CancellationToken);

        // Assert — first AI call (step 1) user message should contain exactly 12 articles (all high, no medium)
        capturedMessages.Should().NotBeEmpty();
        var messageBody = System.Text.Json.JsonDocument.Parse(capturedMessages[0]);
        var userContent = messageBody.RootElement
            .GetProperty("messages")[1]
            .GetProperty("content")
            .GetString();

        // 12 high articles = 12 "ARTICLE:" entries; medium articles are excluded
        var articleCount = CountOccurrences(userContent ?? "", "ARTICLE:");
        articleCount.Should().Be(12, "filtering should include only high when count >= MinArticlesPerSection");
    }

    [Fact]
    public async Task GenerateAsync_WithFewHighArticles_IncludesMediumArticlesRankedByImportance()
    {
        // Arrange — section has only 3 high articles (below MinArticlesPerSection=10)
        var uniqueWeekStart = new DateOnly(2025, 6, 9);
        var uniqueWeekEnd = new DateOnly(2025, 6, 15);

        var articles = new Dictionary<string, IReadOnlyList<RoundupArticle>>
        {
            ["azure"] = BuildArticles("azure", 3, "high")
                .Concat(BuildArticles("azure", 12, "medium", startIndex: 3))
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
        await sut.GenerateAsync(uniqueWeekStart, uniqueWeekEnd, ct: TestContext.Current.CancellationToken);

        // Assert — should contain 10 articles total (3 high + 7 medium to reach MinArticlesPerSection=10)
        capturedMessages2.Should().NotBeEmpty();
        var doc = System.Text.Json.JsonDocument.Parse(capturedMessages2[0]);
        var userContent = doc.RootElement
            .GetProperty("messages")[1]
            .GetProperty("content")
            .GetString();

        var articleCount = CountOccurrences(userContent ?? "", "ARTICLE:");
        articleCount.Should().Be(10, "3 high + 7 medium = 10, filling up to MinArticlesPerSection=10");
    }

    // ── Tag Expansion for Roundups ────────────────────────────────────────────

    [Fact]
    public async Task GenerateAsync_WithArticles_PopulatesContentTagsExpanded()
    {
        // Arrange — use a unique week
        var uniqueWeekStart = new DateOnly(2025, 7, 7);
        var uniqueWeekEnd = new DateOnly(2025, 7, 13);

        var articles = new Dictionary<string, IReadOnlyList<RoundupArticle>>
        {
            ["ai"] = BuildArticles("ai", 4, "high")
        };

        _roundupRepo
            .Setup(r => r.GetArticlesForWeekAsync(uniqueWeekStart, uniqueWeekEnd, It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);

        // Use body-inspecting callback: step 7 requests contain "Return only JSON"
        var step7Json = """{"title": "Tag Test Roundup", "tags": ["AI", "ML", "Azure OpenAI"], "description": "A test.", "introduction": "Welcome."}""";
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string body, CancellationToken _) =>
                body.Contains("Return only JSON", StringComparison.Ordinal)
                    ? OkAiResponse(step7Json)
                    : OkAiResponse("## AI\n\nAI news this week.\n\n- [Article 0](https://example.com/0)"));

        var sut = CreateSut();

        // Act
        var result = await sut.GenerateAsync(uniqueWeekStart, uniqueWeekEnd, ct: TestContext.Current.CancellationToken);

        // Assert
        result.Result.Should().Be(RoundupGenerationResult.Generated);

        _roundupRepo.Verify(r => r.WriteRoundupAsync(
            It.IsAny<string>(),
            It.IsAny<DateOnly>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.Is<IReadOnlyList<string>>(tags =>
                tags.Contains("AI") &&
                tags.Contains("ML") &&
                tags.Contains("Azure OpenAI")),
            It.IsAny<long?>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    // ── Step 4: Ongoing Narrative with Previous Roundup ───────────────────────

    [Fact]
    public async Task GenerateAsync_WithPreviousRoundup_InvokesStep4WithPreviousContent()
    {
        // Arrange — mock previous roundup content
        var currentWeekStart = new DateOnly(2026, 6, 8);
        var currentWeekEnd = new DateOnly(2026, 6, 14);

        _roundupRepo
            .Setup(r => r.GetPreviousRoundupContentAsync(currentWeekStart, It.IsAny<CancellationToken>()))
            .ReturnsAsync("## AI\n\nLast week, Azure AI Foundry released new features.\n\n- [Last Week Article](https://example.com/prev)");

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
        var result = await sut.GenerateAsync(currentWeekStart, currentWeekEnd, ct: TestContext.Current.CancellationToken);

        // Assert
        result.Result.Should().Be(RoundupGenerationResult.Generated);

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
        var result = await sut.GenerateAsync(uniqueWeekStart, uniqueWeekEnd, ct: TestContext.Current.CancellationToken);

        // Assert — should still succeed using fallback metadata
        result.Result.Should().Be(RoundupGenerationResult.Generated);

        _roundupRepo.Verify(r => r.WriteRoundupAsync(
            It.IsAny<string>(),
            It.IsAny<DateOnly>(),
            It.Is<string>(title => title.Contains("Weekly AI and Tech News Roundup")),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<IReadOnlyList<string>>(),
            It.IsAny<long?>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    // ── Content Generation Failure ────────────────────────────────────────────

    [Fact]
    public async Task GenerateAsync_WhenAllAiCallsFail_ReturnsContentGenerationFailed()
    {
        // Arrange — AI returns null for everything (simulates total AI outage)
        var uniqueWeekStart = new DateOnly(2025, 11, 3);
        var uniqueWeekEnd = new DateOnly(2025, 11, 9);

        var articles = new Dictionary<string, IReadOnlyList<RoundupArticle>>
        {
            ["ai"] = BuildArticles("ai", 4, "high")
        };

        _roundupRepo
            .Setup(r => r.GetArticlesForWeekAsync(uniqueWeekStart, uniqueWeekEnd, It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);

        // AI returns null for all calls (extraction fails)
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(IsRateLimited: false, ResponseBody: null));

        var sut = CreateSut();

        // Act
        var result = await sut.GenerateAsync(uniqueWeekStart, uniqueWeekEnd, ct: TestContext.Current.CancellationToken);

        // Assert — should fail, NOT write to database
        result.Result.Should().Be(RoundupGenerationResult.ContentGenerationFailed);
        result.Slug.Should().BeNull();
        _roundupRepo.Verify(r => r.WriteRoundupAsync(
            It.IsAny<string>(), It.IsAny<DateOnly>(), It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<IReadOnlyList<string>>(), It.IsAny<long?>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GenerateAsync_WhenStep1ProducesEmptyContent_ReturnsContentGenerationFailed()
    {
        // Arrange — AI returns empty strings (technically "succeeds" but produces nothing)
        var uniqueWeekStart = new DateOnly(2025, 11, 10);
        var uniqueWeekEnd = new DateOnly(2025, 11, 16);

        var articles = new Dictionary<string, IReadOnlyList<RoundupArticle>>
        {
            ["ai"] = BuildArticles("ai", 4, "high")
        };

        _roundupRepo
            .Setup(r => r.GetArticlesForWeekAsync(uniqueWeekStart, uniqueWeekEnd, It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);

        // AI returns empty content for step 1
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(OkAiResponse(""));

        var sut = CreateSut();

        // Act
        var result = await sut.GenerateAsync(uniqueWeekStart, uniqueWeekEnd, ct: TestContext.Current.CancellationToken);

        // Assert — should fail, NOT write empty content to database
        result.Result.Should().Be(RoundupGenerationResult.ContentGenerationFailed);
        result.Slug.Should().BeNull();
        _roundupRepo.Verify(r => r.WriteRoundupAsync(
            It.IsAny<string>(), It.IsAny<DateOnly>(), It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<IReadOnlyList<string>>(), It.IsAny<long?>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GenerateAsync_WhenContentGenerated_WritesNonEmptyContent()
    {
        // Arrange — verify that a successful pipeline writes non-empty, non-whitespace content
        var uniqueWeekStart = new DateOnly(2025, 12, 1);
        var uniqueWeekEnd = new DateOnly(2025, 12, 7);

        var articles = new Dictionary<string, IReadOnlyList<RoundupArticle>>
        {
            ["ai"] = BuildArticles("ai", 4, "high")
        };

        _roundupRepo
            .Setup(r => r.GetArticlesForWeekAsync(uniqueWeekStart, uniqueWeekEnd, It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);

        SetupAiForAllSteps(
            step3Content: "## AI\n\nAI had a busy week with multiple announcements.\n\n- [Article 0](https://example.com/0)",
            step4Content: "## AI\n\nBuilding on last week, AI continues to evolve.\n\n- [Article 0](https://example.com/0)",
            step6Content: "## AI\n\nAI had a busy week.\n\n- [Article 0](https://example.com/0)",
            step7Metadata: """{"title": "Test Roundup", "tags": ["AI"], "description": "A test roundup.", "introduction": "Welcome to this week's roundup covering the latest in AI."}"""
        );

        var sut = CreateSut();

        // Act
        var result = await sut.GenerateAsync(uniqueWeekStart, uniqueWeekEnd, ct: TestContext.Current.CancellationToken);

        // Assert — content written must contain actual section content (not just boilerplate)
        result.Result.Should().Be(RoundupGenerationResult.Generated);

        _roundupRepo.Verify(r => r.WriteRoundupAsync(
            It.IsAny<string>(),
            It.IsAny<DateOnly>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.Is<string>(content =>
                content.Contains("## AI") &&
                content.Contains("## This Week's Overview") &&
                content.Contains("<!--excerpt_end-->")),
            It.IsAny<string>(),
            It.IsAny<IReadOnlyList<string>>(),
            It.IsAny<long?>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    // ── AI Metadata Backfill Progress ─────────────────────────────────────────

    [Fact]
    public async Task GenerateAsync_WithBackfillNeeded_ReportsProgressAfterEachItemIsProcessed()
    {
        // Arrange — 2 articles with NeedsAiMetadata = true
        var uniqueWeekStart = new DateOnly(2025, 10, 6);
        var uniqueWeekEnd = new DateOnly(2025, 10, 12);

        var articles = new Dictionary<string, IReadOnlyList<RoundupArticle>>
        {
            ["ai"] = BuildArticlesNeedingBackfill("ai", 2)
                .Concat(BuildArticles("ai", 4, "high", startIndex: 2))
                .ToList()
        };

        _roundupRepo
            .Setup(r => r.GetArticlesForWeekAsync(uniqueWeekStart, uniqueWeekEnd, It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);

        _aiCategorizationService
            .Setup(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategorizationResult
            {
                Explanation = "ok",
                Item = new ProcessedContentItem
                {
                    Slug = "backfill-result",
                    Title = "Backfilled",
                    Excerpt = "excerpt",
                    DateEpoch = 0,
                    CollectionName = "news",
                    ExternalUrl = "https://example.com/backfill",
                    FeedName = "Test Feed",
                    PrimarySectionName = "ai",
                    ContentHash = "abc123",
                    RoundupMetadata = new RoundupMetadata
                    {
                        Summary = "Summary",
                        Relevance = "high",
                        TopicType = "news",
                        ImpactLevel = "medium",
                        TimeSensitivity = "this-week"
                    }
                }
            });

        SetupAiForAllSteps(
            step3Content: "## AI\n\nAI news.\n\n- [Article 2](https://example.com/2)\n- [Article 3](https://example.com/3)",
            step4Content: "## AI\n\nAI news.\n\n- [Article 2](https://example.com/2)",
            step6Content: "## AI\n\nAI news.\n\n- [Article 2](https://example.com/2)",
            step7Metadata: "{\"title\": \"Backfill Test\", \"tags\": [\"AI\"], \"description\": \"Test.\", \"introduction\": \"Intro.\"}"
        );

        var progressMessages = new List<string>();
        var progress = new Progress<string>(progressMessages.Add);

        var sut = CreateSut();

        // Act
        await sut.GenerateAsync(uniqueWeekStart, uniqueWeekEnd, progress, ct: TestContext.Current.CancellationToken);

        // Assert — per-item progress messages appear AFTER each item is processed
        var backfillMessages = progressMessages
            .Where(m => m.StartsWith("AI metadata backfill:", StringComparison.Ordinal) && m.Contains('/') && m.Contains('\u2014'))
            .ToList();

        backfillMessages.Should().HaveCount(2, "one progress message per backfilled item");
        backfillMessages[0].Should().Contain("1/2", "first item is reported as 1/2 after it completes");
        backfillMessages[1].Should().Contain("2/2", "second item is reported as 2/2 after it completes");

        // Neither message should appear with doneCount=0
        progressMessages.Should().NotContain(m => m.Contains("0/2"));
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

    private static IReadOnlyList<RoundupArticle> BuildArticlesNeedingBackfill(string section, int count) =>
        Enumerable.Range(0, count).Select(i => new RoundupArticle
        {
            SectionName = section,
            Title = $"Backfill Article {i}",
            ExternalUrl = $"https://example.com/backfill-{i}",
            Slug = $"backfill-{section}-{i}",
            CollectionName = "news",
            IsInternal = false,
            NeedsAiMetadata = true,
            Content = $"Content of backfill article {i}",
            FeedName = "Test Feed",
            DateEpoch = 1_700_000_000 + i
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
}
