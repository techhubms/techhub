using System.Text.Json;
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
/// Tests for <see cref="RoundupGeneratorService"/>: split into lead and brief articles, one writer call and one
/// metadata call per section, completeness check, and the code-built "Also relevant" and "Dates to know" blocks.
/// Uses real step classes with a mocked AI client and repository.
/// </summary>
public class RoundupGeneratorServiceTests
{
    private const string WriterDeployment = "test-roundup-writer";
    private const string MetadataDeployment = "test-roundup-metadata";
    private const string MetadataJson = """{"title": "Weekly AI Roundup", "tags": ["AI"], "introduction": "Intro."}""";

    private readonly Mock<ISectionRoundupRepository> _roundupRepo = new();
    private readonly Mock<IAiCompletionClient> _aiClient = new();
    private readonly Mock<IAiCategorizationService> _aiCategorizationService = new();
    private readonly Mock<IContentItemWriteRepository> _writeRepo = new();

    private readonly List<(string Model, string UserMessage)> _aiCalls = [];
    private readonly List<string> _aiRequests = [];
    private readonly List<(string Section, string Slug, string Title, string Content, IReadOnlyList<string> Tags)> _written = [];

    public RoundupGeneratorServiceTests()
    {
        // Sections without a specific setup have no articles.
        _roundupRepo
            .Setup(r => r.GetArticlesForSectionForWeekAsync(It.IsAny<string>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        _writeRepo
            .Setup(w => w.UpdateAiMetadataIfUnchangedAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimelineEventReplacement>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
    }

    private static readonly DateOnly _weekStart = new(2025, 4, 7);    // Monday
    private static readonly DateOnly _weekEnd = new(2025, 4, 13);     // Sunday

    private static readonly AppSettings _testAppSettings = new()
    {
        BaseUrl = "https://test.example.com",
        Content = new ContentSettings
        {
            CollectionsPath = "collections",
            Sections = new Dictionary<string, SectionConfig>
            {
                ["github-copilot"] = Section("GitHub Copilot", "GitHub Copilot", 1),
                ["ai"] = Section("Artificial Intelligence", "AI", 2),
                ["azure"] = Section("Microsoft Azure", "Azure", 3)
            }
        }
    };

    private static readonly RoundupGeneratorOptions _defaultOptions = new()
    {
        RunHourUtc = 8,
        RateLimitDelaySeconds = 0,
        MaxRetries = 1
    };

    private static SectionConfig Section(string title, string tag, int order) => new()
    {
        Title = title,
        Description = $"{title} section",
        Url = $"/{tag.ToLowerInvariant()}",
        Tag = tag,
        Order = order,
        Collections = new Dictionary<string, CollectionConfig>
        {
            ["news"] = new() { Title = "News", Url = "/news", Description = "News" }
        }
    };

    private RoundupGeneratorService CreateSut()
    {
        var appSettingsOptions = Options.Create(_testAppSettings);

        var aiHelper = new RoundupAiHelper(
            _aiClient.Object,
            _defaultOptions,
            new AiCategorizationOptions
            {
                Profiles = new AiProfilesOptions
                {
                    RoundupNewsWriter = new AiProfileOptions { DeploymentName = WriterDeployment },
                    RoundupMetadata = new AiProfileOptions { DeploymentName = MetadataDeployment }
                }
            },
            NullLogger<RoundupAiHelper>.Instance);

        var newsWriter = new RoundupNewsWriter(aiHelper, appSettingsOptions, _defaultOptions, NullLogger<RoundupNewsWriter>.Instance);
        var metadataGenerator = new RoundupMetadataGenerator(aiHelper, _defaultOptions, appSettingsOptions, NullLogger<RoundupMetadataGenerator>.Instance);

        var contentFixerMock = new Mock<IContentFixerService>();
        contentFixerMock.Setup(s => s.RepairMarkdown(It.IsAny<string>())).Returns((string c) => c);

        _roundupRepo
            .Setup(r => r.GetUpcomingDateEventsAsync(It.IsAny<string>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        _roundupRepo
            .Setup(r => r.WriteRoundupAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateOnly>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<IReadOnlyList<string>>(), It.IsAny<long?>(), It.IsAny<CancellationToken>()))
            .Callback((string section, string slug, DateOnly _, string title, string content, string _, IReadOnlyList<string> tags, long? _, CancellationToken _)
                => _written.Add((section, slug, title, content, tags)))
            .Returns(Task.CompletedTask);

        return new RoundupGeneratorService(
            _roundupRepo.Object,
            contentFixerMock.Object,
            _aiCategorizationService.Object,
            _writeRepo.Object,
            newsWriter,
            metadataGenerator,
            appSettingsOptions,
            NullLogger<RoundupGeneratorService>.Instance);
    }

    private static RoundupArticle Article(
        string section,
        int index,
        string role = "lead",
        string significance = "medium",
        string primary = "",
        string[]? products = null,
        string[]? facts = null) => new()
        {
            SectionName = section,
            PrimarySectionName = string.IsNullOrEmpty(primary) ? section : primary,
            Title = $"Article {index}",
            ExternalUrl = $"https://example.com/{index}",
            Slug = $"article-{index}",
            CollectionName = "news",
            Significance = significance,
            Products = products ?? ["Product"],
            KeyFacts = facts ?? [$"Fact of article {index}."],
            SectionRoles = new Dictionary<string, string> { [section] = role }
        };

    private void SetupArticles(string section, params RoundupArticle[] articles) =>
        _roundupRepo
            .Setup(r => r.GetArticlesForSectionForWeekAsync(section, _weekStart, _weekEnd, It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);

    /// <summary>
    /// The writer returns <paramref name="writerContent"/>; the metadata step returns <paramref name="metadataJson"/>.
    /// </summary>
    private void SetupAi(Func<string, string>? writer = null, string metadataJson = MetadataJson)
    {
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns<string, CancellationToken>((json, _) =>
            {
                _aiRequests.Add(json);
                using var doc = JsonDocument.Parse(json);
                var model = doc.RootElement.GetProperty("model").GetString()!;
                var user = doc.RootElement.GetProperty("messages")[1].GetProperty("content").GetString()!;
                _aiCalls.Add((model, user));

                var content = model == MetadataDeployment
                    ? metadataJson
                    : (writer ?? DefaultWriter)(user);

                var serialized = JsonSerializer.Serialize(content);
                return Task.FromResult(new AiCompletionResult(false, $$$"""{"choices":[{"message":{"content":{{{serialized}}}}}]}"""));
            });
    }

    /// <summary>Writes one topic that links every article in the user message.</summary>
    private static string DefaultWriter(string user)
    {
        var links = user.Split('\n')
            .Where(l => l.StartsWith("LINK: ", StringComparison.Ordinal))
            .Select(l => "- " + l["LINK: ".Length..]);
        return "## Topic\n\nStory text.\n\n" + string.Join("\n", links);
    }

    // ── Early exits ───────────────────────────────────────────────────────────

    [Fact]
    public async Task GenerateAsync_WhenRoundupAlreadyExists_ReturnsAlreadyExistsWithoutCallingAi()
    {
        SetupArticles("ai", Article("ai", 1));
        _roundupRepo.Setup(r => r.RoundupExistsAsync("ai", "weekly-ai-roundup-2025-04-14", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var result = await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        result.Result.Should().Be(RoundupGenerationResult.AlreadyExists);
        _aiCalls.Should().BeEmpty();
        _aiClient.Verify(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GenerateAsync_WhenNoArticles_ReturnsNoArticlesWithoutCallingAi()
    {
        var result = await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        result.Result.Should().Be(RoundupGenerationResult.NoArticles);
        _aiClient.Verify(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    // ── Call plan ─────────────────────────────────────────────────────────────

    [Fact]
    public async Task GenerateAsync_OneSection_MakesExactlyOneWriterCallAndOneMetadataCall()
    {
        SetupArticles("ai", Article("ai", 1), Article("ai", 2));
        SetupAi();

        var result = await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        result.Result.Should().Be(RoundupGenerationResult.Generated);
        result.Slug.Should().Be("weekly-ai-roundup-2025-04-14");
        _aiCalls.Select(c => c.Model).Should().Equal(WriterDeployment, MetadataDeployment);
    }

    [Fact]
    public async Task GenerateAsync_TwoSections_MakesTwoCallsPerSection()
    {
        SetupArticles("ai", Article("ai", 1));
        SetupArticles("azure", Article("azure", 2));
        SetupAi();

        var result = await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        result.GeneratedCount.Should().Be(2);
        _aiCalls.Should().HaveCount(4);
        _aiCalls.Count(c => c.Model == WriterDeployment).Should().Be(2);
        _aiCalls.Count(c => c.Model == MetadataDeployment).Should().Be(2);
        _written.Select(w => w.Section).Should().BeEquivalentTo("ai", "azure");
    }

    [Fact]
    public async Task GenerateAsync_MetadataRequestUsesStrictJsonSchema()
    {
        SetupArticles("ai", Article("ai", 1));
        SetupAi();

        await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        var metadataRequest = _aiRequests.Select(r => JsonDocument.Parse(r).RootElement)
            .Single(r => r.GetProperty("model").GetString() == MetadataDeployment);
        metadataRequest.GetProperty("response_format").GetProperty("json_schema").GetProperty("strict").GetBoolean().Should().BeTrue();
    }

    // ── Lead and brief ────────────────────────────────────────────────────────

    [Fact]
    public async Task GenerateAsync_BriefArticle_IsNotSentToTheWriterButListedUnderAlsoRelevant()
    {
        SetupArticles("ai", Article("ai", 3));
        SetupArticles("azure",
            Article("azure", 1),
            Article("azure", 2, role: "brief", primary: "ai", facts: ["Brief fact"]));
        SetupAi();

        await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        var writerMessage = _aiCalls.Single(c => c.Model == WriterDeployment && c.UserMessage.Contains("Article 1", StringComparison.Ordinal)).UserMessage;
        writerMessage.Should().Contain("Article 1");
        writerMessage.Should().NotContain("Article 2");

        var content = _written.Single(w => w.Section == "azure").Content;
        content.Should().Contain("## Also relevant this week");
        content.Should().Contain("- [Article 2](https://example.com/2): Brief fact.");
        content.Should().Contain("[Artificial Intelligence roundup](/ai/roundups/weekly-ai-roundup-2025-04-14)");
    }

    [Fact]
    public async Task GenerateAsync_PrimaryRoundupFailsToGenerate_DoesNotLinkTheFullStory()
    {
        SetupArticles("ai", Article("ai", 3));
        SetupArticles("azure",
            Article("azure", 1),
            Article("azure", 2, role: "brief", primary: "ai", facts: ["Brief fact"]));
        SetupAi(writer: user => user.Contains("Article 3", StringComparison.Ordinal) ? string.Empty : DefaultWriter(user));

        var result = await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        result.Result.Should().Be(RoundupGenerationResult.Generated);
        var azure = _written.Should().ContainSingle().Which;
        azure.Section.Should().Be("azure");
        azure.Content.Should().Contain("- [Article 2](https://example.com/2): Brief fact.");
        azure.Content.Should().NotContain("Full story");
        azure.Content.Should().NotContain("/ai/roundups/");
    }

    [Fact]
    public async Task GenerateAsync_PrimaryRoundupAlreadyExists_StillLinksTheFullStory()
    {
        SetupArticles("ai", Article("ai", 3));
        SetupArticles("azure",
            Article("azure", 1),
            Article("azure", 2, role: "brief", primary: "ai", facts: ["Brief fact"]));
        _roundupRepo.Setup(r => r.RoundupExistsAsync("ai", "weekly-ai-roundup-2025-04-14", It.IsAny<CancellationToken>())).ReturnsAsync(true);
        SetupAi();

        await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        _written.Should().ContainSingle().Which.Content.Should().Contain("/ai/roundups/weekly-ai-roundup-2025-04-14");
    }

    [Fact]
    public async Task GenerateAsync_PrimarySectionIsLeadEvenWhenRoleSaysBrief()
    {
        SetupArticles("ai", Article("ai", 1, role: "brief"));
        SetupAi();

        await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        _aiCalls.Single(c => c.Model == WriterDeployment).UserMessage.Should().Contain("Article 1");
        _written.Single().Content.Should().NotContain("Also relevant this week");
    }

    [Fact]
    public async Task GenerateAsync_ArticleWithoutRoleForTheSection_IsTreatedAsLead()
    {
        var legacy = Article("ai", 1) with { SectionRoles = new Dictionary<string, string>() };
        SetupArticles("ai", legacy);
        SetupAi();

        await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        _aiCalls.Single(c => c.Model == WriterDeployment).UserMessage.Should().Contain("Article 1");
    }

    [Fact]
    public async Task GenerateAsync_OnlyBriefArticles_SkipsWriterAndStillWritesRoundup()
    {
        SetupArticles("azure", Article("azure", 1, role: "brief", primary: "ai"));
        SetupAi();

        var result = await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        result.Result.Should().Be(RoundupGenerationResult.Generated);
        _aiCalls.Select(c => c.Model).Should().Equal(MetadataDeployment);
        _written.Single().Content.Should().Contain("## Also relevant this week");
    }

    [Fact]
    public async Task GenerateAsync_OrdersWriterInputBySignificanceThenProduct()
    {
        SetupArticles("ai",
            Article("ai", 1, significance: "low", products: ["Alpha"]),
            Article("ai", 2, significance: "high", products: ["Zeta"]),
            Article("ai", 3, significance: "high", products: ["Beta"]),
            Article("ai", 4, significance: "medium", products: ["Alpha"]));
        SetupAi();

        await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        var message = _aiCalls.Single(c => c.Model == WriterDeployment).UserMessage;
        var order = new[] { "Article 3", "Article 2", "Article 4", "Article 1" }.Select(t => message.IndexOf($"ARTICLE: {t}", StringComparison.Ordinal)).ToList();
        order.Should().BeInAscendingOrder().And.OnlyContain(i => i >= 0);
    }

    [Fact]
    public async Task GenerateAsync_WriterInputUsesKeyFactsNotOldSummaries()
    {
        SetupArticles("ai", Article("ai", 1, facts: ["It ships in version 3.2.", "It costs 5 dollars."]));
        SetupAi();

        await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        var message = _aiCalls.Single(c => c.Model == WriterDeployment).UserMessage;
        message.Should().Contain("KEY FACTS:").And.Contain("- It ships in version 3.2.").And.Contain("- It costs 5 dollars.");
        message.Should().NotContain("SUMMARY:");
    }

    // ── Completeness ──────────────────────────────────────────────────────────

    [Fact]
    public async Task GenerateAsync_WhenWriterLeavesOutALead_AppendsItToOtherNews()
    {
        SetupArticles("ai", Article("ai", 1), Article("ai", 2));
        SetupAi(writer: _ => "## Topic\n\nStory.\n\n- [Article 1](https://example.com/1)\n\n## Other Artificial Intelligence News\n\nMisc.\n\n- [Old](https://example.com/old)");

        await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        var content = _written.Single().Content;
        content.Should().Contain("[Article 2](https://example.com/2)");
        content.IndexOf("[Article 2]", StringComparison.Ordinal).Should().BeGreaterThan(content.IndexOf("## Other Artificial Intelligence News", StringComparison.Ordinal));
    }

    [Fact]
    public async Task GenerateAsync_WhenWriterHasNoOtherNewsSection_CreatesOne()
    {
        SetupArticles("ai", Article("ai", 1), Article("ai", 2));
        SetupAi(writer: _ => "## Topic\n\nStory.\n\n- [Article 1](https://example.com/1)");

        await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        var content = _written.Single().Content;
        content.Should().Contain("## Other Artificial Intelligence News");
        content.Should().Contain("[Article 2](https://example.com/2)");
    }

    // ── Continuity ────────────────────────────────────────────────────────────

    [Fact]
    public async Task GenerateAsync_WithPreviousRoundup_SendsOnlyItsHeadingsToTheWriter()
    {
        SetupArticles("ai", Article("ai", 1));
        _roundupRepo
            .Setup(r => r.GetPreviousRoundupContentAsync("ai", _weekStart, It.IsAny<CancellationToken>()))
            .ReturnsAsync("Intro\n\n## This Week's Overview\n\n- toc\n\n## Agent Mode Preview\n\nLong paragraph that must not be sent.\n\n### Details\n\nMore text.");
        SetupAi();

        await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        var message = _aiCalls.Single(c => c.Model == WriterDeployment).UserMessage;
        message.Should().Contain("LAST WEEK'S HEADINGS");
        message.Should().Contain("## Agent Mode Preview").And.Contain("### Details");
        message.Should().NotContain("Long paragraph that must not be sent");
        message.Should().NotContain("This Week's Overview");
    }

    [Fact]
    public async Task GenerateAsync_WithoutPreviousRoundup_SendsNoHeadingsBlock()
    {
        SetupArticles("ai", Article("ai", 1));
        SetupAi();

        await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        _aiCalls.Single(c => c.Model == WriterDeployment).UserMessage.Should().NotContain("LAST WEEK'S HEADINGS");
    }

    // ── Dates to know ─────────────────────────────────────────────────────────

    [Fact]
    public async Task GenerateAsync_WithUpcomingEvents_AddsDatesToKnowBlockBuiltByCode()
    {
        SetupArticles("ai", Article("ai", 1));
        SetupAi();
        var sut = CreateSut();
        _roundupRepo
            .Setup(r => r.GetUpcomingDateEventsAsync("ai", _weekStart, _weekEnd, It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new RoundupDateEvent { Kind = "retirement", Product = "Azure OpenAI", Feature = "v1 API", Description = "Is shut down", Date = new DateOnly(2025, 6, 1), DatePrecision = "day", SourceTitle = "Src A", SourceUrl = "https://example.com/a" },
                new RoundupDateEvent { Kind = "ga", Product = "GitHub Copilot", Description = "Becomes generally available", Date = new DateOnly(2025, 7, 1), DatePrecision = "quarter", SourceTitle = "Src B", SourceUrl = "https://example.com/b" }
            ]);

        await sut.GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        var content = _written.Single().Content;
        content.Should().Contain("## Dates to know").And.Contain("### Deadlines").And.Contain("### Coming up");
        content.Should().Contain("**1 June 2025**: Azure OpenAI v1 API: Is shut down. ([Src A](https://example.com/a))");
        content.Should().Contain("**Q3 2025**: GitHub Copilot: Becomes generally available. ([Src B](https://example.com/b))");
    }

    [Fact]
    public async Task GenerateAsync_WithoutUpcomingEvents_HasNoDatesToKnowBlock()
    {
        SetupArticles("ai", Article("ai", 1));
        SetupAi();

        await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        _written.Single().Content.Should().NotContain("Dates to know");
    }

    // ── Final content ─────────────────────────────────────────────────────────

    [Fact]
    public async Task GenerateAsync_TableOfContentsIsBuiltByCodeAndIncludesTheCodeBuiltBlocks()
    {
        SetupArticles("azure", Article("azure", 1), Article("azure", 2, role: "brief", primary: "ai"));
        SetupAi();

        await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        var content = _written.Single().Content;
        content.Should().Contain("<!--excerpt_end-->").And.Contain("## This Week's Overview");
        content.Should().Contain("- [Topic](#topic)").And.Contain("- [Also relevant this week](#also-relevant-this-week)");
        content.Should().StartWith("Intro.");
    }

    [Fact]
    public async Task GenerateAsync_WritesTitleAndTagsFromMetadataAndAddsSectionTag()
    {
        SetupArticles("ai", Article("ai", 1));
        SetupAi(metadataJson: """{"title": "My Title", "tags": ["Agents"], "introduction": "Hello."}""");

        await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        var written = _written.Single();
        written.Title.Should().Be("My Title");
        written.Tags.Should().Contain("Agents").And.Contain("AI");
    }

    // ── Failures ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task GenerateAsync_WhenWriterCallFails_ReturnsContentGenerationFailedAndWritesNothing()
    {
        SetupArticles("ai", Article("ai", 1));
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(IsRateLimited: true, ResponseBody: null));

        var result = await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        result.Result.Should().Be(RoundupGenerationResult.ContentGenerationFailed);
        _written.Should().BeEmpty();
    }

    [Fact]
    public async Task GenerateAsync_WhenMetadataIsInvalidJson_UsesFallbackMetadata()
    {
        SetupArticles("ai", Article("ai", 1));
        SetupAi(metadataJson: "not json at all");

        var result = await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        result.Result.Should().Be(RoundupGenerationResult.Generated);
        _written.Single().Title.Should().Contain("Roundup");
    }

    // ── Metadata backfill ─────────────────────────────────────────────────────

    [Fact]
    public async Task GenerateAsync_WithBackfillNeeded_ClassifiesEachItemOnceNeverRewritesAndReportsProgress()
    {
        var needing = Enumerable.Range(0, 2).Select(i => Article("ai", 10 + i) with { NeedsAiMetadata = true, Content = "text", FeedName = "Feed", DateEpoch = 1_700_000_000 }).ToArray();
        SetupArticles("ai", [.. needing, Article("ai", 1)]);

        _aiCategorizationService
            .Setup(s => s.ClassifyAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
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
                        SchemaVersion = RoundupMetadata.CurrentSchemaVersion,
                        KeyFacts = ["A fact."],
                        Significance = "high",
                        SectionRoles = new Dictionary<string, string> { ["ai"] = "lead" }
                    }
                }
            });
        SetupAi();

        var progressMessages = new List<string>();
        var progress = new SynchronousProgress<string>(progressMessages.Add);

        await CreateSut().GenerateAsync(_weekStart, _weekEnd, progress, ct: TestContext.Current.CancellationToken);

        var perItem = progressMessages.Where(m => m.StartsWith("AI metadata backfill:", StringComparison.Ordinal) && m.Contains('\u2014')).ToList();
        perItem.Should().HaveCount(2);
        perItem[0].Should().Contain("1/2");
        perItem[1].Should().Contain("2/2");

        _aiCategorizationService.Verify(s => s.ClassifyAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        _aiCategorizationService.Verify(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()), Times.Never);
        _writeRepo.Verify(w => w.UpdateAiMetadataIfUnchangedAsync("news", It.IsAny<string>(), It.Is<string>(j => j.Contains("\"schema_version\":2")), It.IsAny<TimelineEventReplacement>(), It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GenerateAsync_BackfillKeepsTheTimelineEventsOfTheClassification()
    {
        var needing = Article("ai", 10) with { NeedsAiMetadata = true, Content = "text", FeedName = "Feed", DateEpoch = 1_700_000_000, Revision = 4242 };
        SetupArticles("ai", needing, Article("ai", 1));
        var timelineEvent = new TimelineEvent
        {
            Kind = "retirement",
            Product = "Azure OpenAI",
            Description = "Is shut down",
            Date = new DateOnly(2026, 6, 1),
            DatePrecision = "day",
            DateSource = "explicit"
        };
        _aiCategorizationService
            .Setup(s => s.ClassifyAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategorizationResult
            {
                Explanation = "ok",
                Item = new ProcessedContentItem
                {
                    Slug = "article-10",
                    Title = "t",
                    Excerpt = "e",
                    DateEpoch = 0,
                    CollectionName = "news",
                    ExternalUrl = "https://example.com/10",
                    FeedName = "Feed",
                    PrimarySectionName = "ai",
                    ContentHash = "h",
                    RoundupMetadata = new RoundupMetadata { SchemaVersion = 2, KeyFacts = ["A fact."], SectionRoles = new Dictionary<string, string> { ["ai"] = "lead" } },
                    TimelineEvents = [timelineEvent]
                }
            });
        SetupAi();

        await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        _writeRepo.Verify(w => w.UpdateAiMetadataIfUnchangedAsync(
            "news", "article-10", It.IsAny<string>(),
            It.Is<TimelineEventReplacement>(t => t.Events.Count == 1 && t.Events[0].Kind == "retirement"
                && t.ExtractedFrom == ITimelineEventRepository.ExtractedFromStoredContent
                && t.PromptVersion == ITimelineEventRepository.CurrentPromptVersion),
            4242L,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GenerateAsync_BackfillTimeoutWithoutRunCancellation_CountsAsFailedAndContinuesWithTheNextArticle()
    {
        var needing = Enumerable.Range(0, 2).Select(i => Article("ai", 10 + i) with { NeedsAiMetadata = true, Content = "text", FeedName = "Feed", DateEpoch = 1_700_000_000 }).ToArray();
        SetupArticles("ai", [.. needing, Article("ai", 1)]);
        var calls = 0;
        _aiCategorizationService
            .Setup(s => s.ClassifyAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .Returns(() => ++calls == 1
                ? throw new TaskCanceledException("The request was canceled due to the configured HttpClient.Timeout")
                : Task.FromResult(new CategorizationResult { Explanation = "no metadata", Item = null }));
        SetupAi();

        await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        _aiCategorizationService.Verify(s => s.ClassifyAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GenerateAsync_BackfillSkipsTheArticleWhenItChangedDuringClassification()
    {
        var needing = Article("ai", 10) with { NeedsAiMetadata = true, Content = "text", FeedName = "Feed", DateEpoch = 1_700_000_000, Revision = 4242 };
        SetupArticles("ai", needing, Article("ai", 1));
        _writeRepo
            .Setup(w => w.UpdateAiMetadataIfUnchangedAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimelineEventReplacement>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _aiCategorizationService
            .Setup(s => s.ClassifyAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategorizationResult
            {
                Explanation = "ok",
                Item = new ProcessedContentItem
                {
                    Slug = "article-10",
                    Title = "t",
                    Excerpt = "e",
                    DateEpoch = 0,
                    CollectionName = "news",
                    ExternalUrl = "https://example.com/10",
                    FeedName = "Feed",
                    PrimarySectionName = "ai",
                    ContentHash = "h",
                    RoundupMetadata = new RoundupMetadata { SchemaVersion = 2, KeyFacts = ["A stale fact."], SectionRoles = new Dictionary<string, string> { ["ai"] = "lead" } }
                }
            });
        SetupAi();

        await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        _writeRepo.Verify(w => w.UpdateAiMetadataIfUnchangedAsync("news", "article-10", It.IsAny<string>(), It.IsAny<TimelineEventReplacement>(), 4242L, It.IsAny<CancellationToken>()), Times.Once);
        _aiRequests.Should().NotContain(r => r.Contains("A stale fact."), "metadata classified from outdated text must not be used");
    }

    [Fact]
    public async Task GenerateAsync_BackfillPrependsTheExcerptToTheClassifiedText()
    {
        var needing = Article("ai", 10) with { NeedsAiMetadata = true, Excerpt = "GA ships on 1 June 2026.", Content = "body text", FeedName = "Feed", DateEpoch = 1_700_000_000 };
        SetupArticles("ai", needing, Article("ai", 1));
        RawFeedItem? classified = null;
        _aiCategorizationService
            .Setup(s => s.ClassifyAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .Callback<RawFeedItem, CancellationToken>((item, _) => classified = item)
            .ReturnsAsync(new CategorizationResult { Explanation = "no metadata", Item = null });
        SetupAi();

        await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        classified.Should().NotBeNull();
        classified!.FullContent.Should().Be("GA ships on 1 June 2026.\n\nbody text");
    }

    [Fact]
    public async Task GenerateAsync_WhenBackfillClassificationFails_StillGeneratesTheRoundup()
    {
        SetupArticles("ai", Article("ai", 1) with { NeedsAiMetadata = true, Content = "text" });
        _aiCategorizationService
            .Setup(s => s.ClassifyAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("boom"));
        SetupAi();

        var result = await CreateSut().GenerateAsync(_weekStart, _weekEnd, ct: TestContext.Current.CancellationToken);

        result.Result.Should().Be(RoundupGenerationResult.Generated);
    }
}

/// <summary>
/// Synchronous <see cref="IProgress{T}"/> that invokes the callback inline,
/// avoiding the thread-pool dispatch that <see cref="Progress{T}"/> uses
/// when there is no <see cref="SynchronizationContext"/>.
/// </summary>
sealed file class SynchronousProgress<T>(Action<T> handler) : IProgress<T>
{
    public void Report(T value) => handler(value);
}
