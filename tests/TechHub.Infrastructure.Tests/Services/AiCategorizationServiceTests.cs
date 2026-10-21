using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Services.ContentProcessing;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Unit tests for <see cref="AiCategorizationService"/>: the classify step (strict JSON) and the rewrite step (markdown).
/// </summary>
public class AiCategorizationServiceTests
{
    private const string ClassifyDeployment = "test-classification";
    private const string RewriteDeployment = "test-rewrite";

    private readonly Mock<IAiCompletionClient> _aiClient = new();
    private readonly List<string> _requests = [];

    private static readonly AiCategorizationOptions _defaultOptions = new()
    {
        MaxRetries = 1,
        RateLimitDelaySeconds = 0,
        Profiles = new AiProfilesOptions
        {
            Classification = new AiProfileOptions { DeploymentName = ClassifyDeployment, ServiceTier = "flex", ReasoningEffort = "low" },
            Rewrite = new AiProfileOptions { DeploymentName = RewriteDeployment, ServiceTier = "flex", ReasoningEffort = "low" },
            RoundupNewsWriter = new AiProfileOptions { DeploymentName = "w" },
            RoundupMetadata = new AiProfileOptions { DeploymentName = "m" }
        }
    };

    private AiCategorizationService CreateSut(AiCategorizationOptions? options = null) => new(
        _aiClient.Object,
        Options.Create(options ?? _defaultOptions),
        NullLogger<AiCategorizationService>.Instance);

    private static RawFeedItem CreateRawItem(string url = "https://example.com/article", bool interactive = false) => new()
    {
        Title = "Test Article",
        ExternalUrl = url,
        PublishedAt = new DateTimeOffset(2026, 3, 14, 23, 30, 0, TimeSpan.Zero),
        FeedItemData = "A test description",
        FeedName = "Test Feed",
        CollectionName = "blogs",
        FullContent = "Full article text",
        IsInteractive = interactive
    };

    private static string Wrap(string content, string finishReason = "stop")
    {
        var serialized = JsonSerializer.Serialize(content);
        return $$$"""{"choices":[{"finish_reason":"{{{finishReason}}}","message":{"content":{{{serialized}}}}}]}""";
    }

    private static string ClassificationJson(
        string? sectionsJson = null,
        string primary = "ai",
        string? rolesJson = null,
        string? keyFactsJson = null,
        string significance = "medium",
        string? eventsJson = null,
        string title = "Azure OpenAI Basics",
        string? excerptJson = null)
    {
        sectionsJson ??= """["ai","azure"]""";
        rolesJson ??= """[{"section":"ai","role":"lead"},{"section":"azure","role":"brief"}]""";
        keyFactsJson ??= """["Feature X is generally available.","It costs 5 dollars."]""";
        eventsJson ??= "[]";
        excerptJson ??= "\"Jane explains the basics.\"";

        return $$"""
            {
              "included": true,
              "explanation": "Set ai (primary), azure because it covers Azure OpenAI.",
              "title": "{{title}}",
              "author": "Jane Smith",
              "sections": {{sectionsJson}},
              "primary_section": "{{primary}}",
              "tags": ["Azure OpenAI Service", "C#"],
              "excerpt": {{excerptJson}},
              "section_roles": {{rolesJson}},
              "key_facts": {{keyFactsJson}},
              "significance": "{{significance}}",
              "content_kind": "tutorial",
              "lifecycle": "ga",
              "products": ["Azure OpenAI Service"],
              "timeline_events": {{eventsJson}}
            }
            """;
    }

    private const string ExcludedJson = """
        {"included":false,"explanation":"Sales Pitches. Promotional content.","title":null,"author":null,"sections":[],
         "primary_section":null,"tags":[],"excerpt":null,"section_roles":[],"key_facts":[],"significance":null,
         "content_kind":null,"lifecycle":null,"products":[],"timeline_events":[]}
        """;

    private void SetupResponses(params string[] responses)
    {
        var queue = new Queue<string>(responses);
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns<string, CancellationToken>((json, _) =>
            {
                _requests.Add(json);
                return Task.FromResult(new AiCompletionResult(false, queue.Dequeue()));
            });
    }

    private static JsonElement Parse(string json) => JsonDocument.Parse(json).RootElement.Clone();

    private static bool HasServiceTier(string requestJson) => Parse(requestJson).TryGetProperty("service_tier", out _);

    // ── Call plan ─────────────────────────────────────────────────────────────

    [Fact]
    public async Task CategorizeAsync_ExcludedItem_MakesExactlyOneCallOnTheClassificationDeployment()
    {
        SetupResponses(Wrap(ExcludedJson));

        var result = await CreateSut().CategorizeAsync(CreateRawItem());

        result.Item.Should().BeNull();
        result.IsFailure.Should().BeFalse();
        result.Explanation.Should().StartWith("Sales Pitches");
        _requests.Should().ContainSingle();
        Parse(_requests[0]).GetProperty("model").GetString().Should().Be(ClassifyDeployment);
    }

    [Fact]
    public async Task CategorizeAsync_IncludedItem_CallsClassificationThenRewrite()
    {
        SetupResponses(Wrap(ClassificationJson()), Wrap("Rewritten body."));

        var result = await CreateSut().CategorizeAsync(CreateRawItem());

        result.IsFailure.Should().BeFalse();
        _requests.Should().HaveCount(2);
        Parse(_requests[0]).GetProperty("model").GetString().Should().Be(ClassifyDeployment);
        Parse(_requests[1]).GetProperty("model").GetString().Should().Be(RewriteDeployment);
        result.Item!.Content.Should().Be("Rewritten body.");
        result.Item.Title.Should().Be("Azure OpenAI Basics");
        result.Item.Slug.Should().Be("azure-openai-basics");
        result.Item.Excerpt.Should().Be("Jane explains the basics.");
        result.Item.PrimarySectionName.Should().Be("ai");
    }

    [Fact]
    public async Task ClassifyAsync_IncludedItem_MakesOneCallAndNeverRewrites()
    {
        SetupResponses(Wrap(ClassificationJson()));

        var result = await CreateSut().ClassifyAsync(CreateRawItem());

        result.Item.Should().NotBeNull();
        result.Item!.Content.Should().BeEmpty();
        _requests.Should().ContainSingle();
    }

    [Fact]
    public async Task CategorizeAsync_RewriteFailure_FailsTheWholeItem()
    {
        SetupResponses(Wrap(ClassificationJson()), Wrap("partial", finishReason: "length"));

        var result = await CreateSut().CategorizeAsync(CreateRawItem());

        result.IsFailure.Should().BeTrue();
        result.Item.Should().BeNull();
        result.Explanation.Should().StartWith("Rewrite failed");
    }

    [Fact]
    public async Task CategorizeAsync_RewriteReturnsEmpty_FailsTheWholeItem()
    {
        SetupResponses(Wrap(ClassificationJson()), Wrap("   "));

        var result = await CreateSut().CategorizeAsync(CreateRawItem());

        result.IsFailure.Should().BeTrue();
        result.Item.Should().BeNull();
    }

    [Fact]
    public async Task CategorizeAsync_RewriteWrappedInMarkdownFence_UnwrapsIt()
    {
        SetupResponses(Wrap(ClassificationJson()), Wrap("```markdown\n## Heading\n\nText\n```"));

        var result = await CreateSut().CategorizeAsync(CreateRawItem());

        result.Item!.Content.Should().Be("## Heading\n\nText");
    }

    [Fact]
    public async Task CategorizeAsync_ContentHashIsComputedOverTheRewrittenContent()
    {
        SetupResponses(Wrap(ClassificationJson()), Wrap("Body one."));
        var first = await CreateSut().CategorizeAsync(CreateRawItem());
        _requests.Clear();
        SetupResponses(Wrap(ClassificationJson()), Wrap("Body two."));

        var second = await CreateSut().CategorizeAsync(CreateRawItem());

        first.Item!.ContentHash.Should().NotBe(second.Item!.ContentHash);
    }

    // ── Request shape ─────────────────────────────────────────────────────────

    [Fact]
    public async Task ClassifyAsync_SendsStrictJsonSchemaAndFlexTier()
    {
        SetupResponses(Wrap(ExcludedJson));

        await CreateSut().ClassifyAsync(CreateRawItem());

        var body = Parse(_requests[0]);
        body.GetProperty("service_tier").GetString().Should().Be("flex");
        body.GetProperty("reasoning_effort").GetString().Should().Be("low");
        body.TryGetProperty("temperature", out _).Should().BeFalse();
        var format = body.GetProperty("response_format");
        format.GetProperty("type").GetString().Should().Be("json_schema");
        format.GetProperty("json_schema").GetProperty("strict").GetBoolean().Should().BeTrue();
    }

    [Fact]
    public async Task ClassifyAsync_SchemaListsEveryPropertyAsRequired()
    {
        SetupResponses(Wrap(ExcludedJson));

        await CreateSut().ClassifyAsync(CreateRawItem());

        var schema = Parse(_requests[0]).GetProperty("response_format").GetProperty("json_schema").GetProperty("schema");
        var properties = schema.GetProperty("properties").EnumerateObject().Select(p => p.Name).ToList();
        var required = schema.GetProperty("required").EnumerateArray().Select(e => e.GetString()).ToList();
        required.Should().BeEquivalentTo(properties);
        schema.GetProperty("additionalProperties").GetBoolean().Should().BeFalse();
    }

    [Fact]
    public async Task CategorizeAsync_RewriteRequestHasNoResponseFormat()
    {
        SetupResponses(Wrap(ClassificationJson()), Wrap("Body."));

        await CreateSut().CategorizeAsync(CreateRawItem());

        Parse(_requests[1]).TryGetProperty("response_format", out _).Should().BeFalse();
    }

    [Fact]
    public async Task CategorizeAsync_RewritePromptContainsClassificationResultAndSource()
    {
        SetupResponses(Wrap(ClassificationJson()), Wrap("Body."));

        await CreateSut().CategorizeAsync(CreateRawItem());

        var userMessage = Parse(_requests[1]).GetProperty("messages")[1].GetProperty("content").GetString();
        userMessage.Should().Contain("TITLE: Azure OpenAI Basics");
        userMessage.Should().Contain("EXCERPT: Jane explains the basics.");
        userMessage.Should().Contain("Full article text");
    }

    [Fact]
    public async Task CategorizeAsync_InteractiveItem_UsesStandardTierForBothCalls()
    {
        SetupResponses(Wrap(ClassificationJson()), Wrap("Body."));

        await CreateSut().CategorizeAsync(CreateRawItem(interactive: true));

        _requests.Should().HaveCount(2);
        _requests.Should().OnlyContain(r => !HasServiceTier(r));
    }

    [Fact]
    public async Task CategorizeAsync_BackgroundItem_UsesFlexTierForBothCalls()
    {
        SetupResponses(Wrap(ClassificationJson()), Wrap("Body."));

        await CreateSut().CategorizeAsync(CreateRawItem());

        _requests.Should().OnlyContain(r => Parse(r).GetProperty("service_tier").GetString() == "flex");
    }

    // ── Validation ────────────────────────────────────────────────────────────

    [Fact]
    public async Task ClassifyAsync_PrimarySectionNotInSections_Fails()
    {
        SetupResponses(Wrap(ClassificationJson(primary: "security")));

        var result = await CreateSut().ClassifyAsync(CreateRawItem());

        result.IsFailure.Should().BeTrue();
        result.Explanation.Should().Contain("not in sections");
    }

    [Fact]
    public async Task ClassifyAsync_InvalidSection_Fails()
    {
        SetupResponses(Wrap(ClassificationJson(sectionsJson: """["ai","cooking"]""")));

        var result = await CreateSut().ClassifyAsync(CreateRawItem());

        result.IsFailure.Should().BeTrue();
        result.Explanation.Should().Contain("invalid sections");
    }

    [Fact]
    public async Task ClassifyAsync_NoKeyFacts_Fails()
    {
        SetupResponses(Wrap(ClassificationJson(keyFactsJson: "[]")));

        var result = await CreateSut().ClassifyAsync(CreateRawItem());

        result.IsFailure.Should().BeTrue();
        result.Explanation.Should().Contain("key facts");
    }

    [Fact]
    public async Task ClassifyAsync_InvalidSignificance_Fails()
    {
        SetupResponses(Wrap(ClassificationJson(significance: "critical")));

        var result = await CreateSut().ClassifyAsync(CreateRawItem());

        result.IsFailure.Should().BeTrue();
        result.Explanation.Should().Contain("significance");
    }

    [Fact]
    public async Task ClassifyAsync_EmptyExcerpt_Fails()
    {
        SetupResponses(Wrap(ClassificationJson(excerptJson: "null")));

        var result = await CreateSut().ClassifyAsync(CreateRawItem());

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task ClassifyAsync_ExcludedWithoutExplanation_Fails()
    {
        SetupResponses(Wrap("""{"included":false,"explanation":""}"""));

        var result = await CreateSut().ClassifyAsync(CreateRawItem());

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task ClassifyAsync_MissingIncludedFlag_Fails()
    {
        SetupResponses(Wrap("""{"explanation":"x"}"""));

        var result = await CreateSut().ClassifyAsync(CreateRawItem());

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task ClassifyAsync_InvalidJson_Fails()
    {
        SetupResponses(Wrap("this is not json"));

        var result = await CreateSut().ClassifyAsync(CreateRawItem());

        result.IsFailure.Should().BeTrue();
    }

    // ── Section roles ─────────────────────────────────────────────────────────

    [Fact]
    public async Task ClassifyAsync_PrimarySectionIsAlwaysLeadEvenWhenAiSaysBrief()
    {
        SetupResponses(Wrap(ClassificationJson(rolesJson: """[{"section":"ai","role":"brief"},{"section":"azure","role":"brief"}]""")));

        var result = await CreateSut().ClassifyAsync(CreateRawItem());

        var roles = result.Item!.RoundupMetadata!.SectionRoles;
        roles["ai"].Should().Be("lead");
        roles["azure"].Should().Be("brief");
    }

    [Fact]
    public async Task ClassifyAsync_SectionWithoutRole_DefaultsToLead()
    {
        SetupResponses(Wrap(ClassificationJson(rolesJson: """[{"section":"ai","role":"lead"}]""")));

        var result = await CreateSut().ClassifyAsync(CreateRawItem());

        result.Item!.RoundupMetadata!.SectionRoles["azure"].Should().Be("lead");
    }

    [Fact]
    public async Task ClassifyAsync_RoleForSectionOutsideSections_Fails()
    {
        SetupResponses(Wrap(ClassificationJson(rolesJson: """[{"section":"security","role":"lead"}]""")));

        var result = await CreateSut().ClassifyAsync(CreateRawItem());

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task ClassifyAsync_InvalidRole_Fails()
    {
        SetupResponses(Wrap(ClassificationJson(rolesJson: """[{"section":"ai","role":"headline"}]""")));

        var result = await CreateSut().ClassifyAsync(CreateRawItem());

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task ClassifyAsync_StoresSchemaVersion2Metadata()
    {
        SetupResponses(Wrap(ClassificationJson()));

        var result = await CreateSut().ClassifyAsync(CreateRawItem());

        var meta = result.Item!.RoundupMetadata!;
        meta.SchemaVersion.Should().Be(RoundupMetadata.CurrentSchemaVersion);
        meta.KeyFacts.Should().HaveCount(2);
        meta.Significance.Should().Be("medium");
        meta.ContentKind.Should().Be("tutorial");
        meta.Lifecycle.Should().Be("ga");
        meta.Products.Should().Equal("Azure OpenAI Service");
    }

    // ── Timeline events ───────────────────────────────────────────────────────

    [Fact]
    public async Task ClassifyAsync_PublicationDatedEvent_UsesBrusselsPublicationDate()
    {
        const string Events = """
            [{"kind":"ga","product":"Azure OpenAI Service","feature":null,"description":"Becomes GA",
              "date_source":"publication","date":null,"date_precision":null,"quote":"is now generally available"}]
            """;
        SetupResponses(Wrap(ClassificationJson(eventsJson: Events)));

        var result = await CreateSut().ClassifyAsync(CreateRawItem());

        // Published 14 March 23:30 UTC, which is already 15 March in Brussels.
        var e = result.Item!.TimelineEvents.Should().ContainSingle().Subject;
        e.Date.Should().Be(new DateOnly(2026, 3, 15));
        e.DateSource.Should().Be("publication");
    }

    [Fact]
    public async Task ClassifyAsync_EventWithBadDate_IsSkippedButItemSucceeds()
    {
        const string Events = """
            [{"kind":"ga","product":"X","feature":null,"description":"d","date_source":"explicit","date":"soon","date_precision":"day","quote":null}]
            """;
        SetupResponses(Wrap(ClassificationJson(eventsJson: Events)));

        var result = await CreateSut().ClassifyAsync(CreateRawItem());

        result.IsFailure.Should().BeFalse();
        result.Item!.TimelineEvents.Should().BeEmpty();
    }

    // ── Author ────────────────────────────────────────────────────────────────

    [Fact]
    public async Task ClassifyAsync_AuthorOverrideWinsOverAiAuthor()
    {
        SetupResponses(Wrap(ClassificationJson()));
        var raw = new RawFeedItem
        {
            Title = "t",
            ExternalUrl = "https://example.com/a",
            PublishedAt = DateTimeOffset.UtcNow,
            FeedName = "f",
            CollectionName = "blogs",
            AuthorOverride = "Override Author"
        };

        var result = await CreateSut().ClassifyAsync(raw);

        result.Item!.Author.Should().Be("Override Author");
    }

    [Fact]
    public async Task ClassifyAsync_NoAiAuthor_FallsBackToFeedLevelAuthor()
    {
        SetupResponses(Wrap(ClassificationJson().Replace("\"author\": \"Jane Smith\"", "\"author\": null", StringComparison.Ordinal)));
        var raw = new RawFeedItem
        {
            Title = "t",
            ExternalUrl = "https://example.com/a",
            PublishedAt = DateTimeOffset.UtcNow,
            FeedName = "f",
            CollectionName = "blogs",
            FeedLevelAuthor = "Feed Author"
        };

        var result = await CreateSut().ClassifyAsync(raw);

        result.Item!.Author.Should().Be("Feed Author");
    }

    // ── Transport failures ────────────────────────────────────────────────────

    [Fact]
    public async Task ClassifyAsync_FinishReasonLength_Fails()
    {
        SetupResponses(Wrap(ExcludedJson, finishReason: "length"));

        var result = await CreateSut().ClassifyAsync(CreateRawItem());

        result.IsFailure.Should().BeTrue();
        result.Explanation.Should().Contain("incomplete");
    }

    [Fact]
    public async Task ClassifyAsync_EmptyContent_Fails()
    {
        SetupResponses("""{"choices":[{"finish_reason":"stop","message":{"content":""}}]}""");

        var result = await CreateSut().ClassifyAsync(CreateRawItem());

        result.IsFailure.Should().BeTrue();
        result.Explanation.Should().Contain("empty");
    }

    [Fact]
    public async Task ClassifyAsync_ContentFilter_FailsWithoutRetry()
    {
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, null, ContentFilterMessage: "blocked"));

        var result = await CreateSut(new AiCategorizationOptions { MaxRetries = 3, RateLimitDelaySeconds = 0, Profiles = _defaultOptions.Profiles })
            .ClassifyAsync(CreateRawItem());

        result.IsFailure.Should().BeTrue();
        result.Explanation.Should().Be("blocked");
        _aiClient.Verify(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ClassifyAsync_RateLimitedThenSuccess_Retries()
    {
        var calls = 0;
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => ++calls == 1
                ? new AiCompletionResult(true, null, RetryAfterSeconds: 0)
                : new AiCompletionResult(false, Wrap(ExcludedJson)));

        var result = await CreateSut(new AiCategorizationOptions { MaxRetries = 3, RateLimitDelaySeconds = 0, Profiles = _defaultOptions.Profiles })
            .ClassifyAsync(CreateRawItem());

        result.IsFailure.Should().BeFalse();
        calls.Should().Be(2);
    }

    [Fact]
    public async Task ClassifyAsync_AllRetriesRateLimited_Fails()
    {
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(true, null, RetryAfterSeconds: 0));

        var result = await CreateSut(new AiCategorizationOptions { MaxRetries = 2, RateLimitDelaySeconds = 0, Profiles = _defaultOptions.Profiles })
            .ClassifyAsync(CreateRawItem());

        result.IsFailure.Should().BeTrue();
        result.Explanation.Should().Contain("failed after 2 attempts");
    }

    [Fact]
    public async Task ClassifyAsync_HttpFailureOnLastAttempt_Throws()
    {
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("boom"));

        var act = () => CreateSut().ClassifyAsync(CreateRawItem());

        await act.Should().ThrowAsync<HttpRequestException>();
    }

    // ── Prompts ───────────────────────────────────────────────────────────────

    [Fact]
    public void BuildUserPrompt_WithSkipSalesPitchCheck_IncludesOverride()
    {
        var item = new RawFeedItem
        {
            Title = "t",
            ExternalUrl = "https://example.com/a",
            PublishedAt = DateTimeOffset.UtcNow,
            FeedName = "f",
            CollectionName = "blogs",
            SkipSalesPitchCheck = true
        };

        AiCategorizationService.BuildUserPrompt(item).Should().Contain("PROCESSING_OVERRIDE: SkipSalesPitchCheck=true");
    }

    [Fact]
    public void BuildUserPrompt_WithoutSkipSalesPitchCheck_OmitsOverride()
    {
        AiCategorizationService.BuildUserPrompt(CreateRawItem()).Should().NotContain("PROCESSING_OVERRIDE");
    }

    [Fact]
    public void BuildUserPrompt_YouTubeItem_LabelsContentAsTranscript()
    {
        var item = new RawFeedItem
        {
            Title = "t",
            ExternalUrl = "https://www.youtube.com/watch?v=abc",
            PublishedAt = DateTimeOffset.UtcNow,
            FeedName = "f",
            CollectionName = "videos",
            FullContent = "spoken words"
        };

        AiCategorizationService.BuildUserPrompt(item).Should().Contain("TRANSCRIPT:");
    }

    [Fact]
    public void BuildUserPrompt_IncludesFeedTags()
    {
        var item = new RawFeedItem
        {
            Title = "t",
            ExternalUrl = "https://example.com/a",
            PublishedAt = DateTimeOffset.UtcNow,
            FeedName = "f",
            CollectionName = "blogs",
            FeedTags = ["Azure", "AI"]
        };

        AiCategorizationService.BuildUserPrompt(item).Should().Contain("FEED TAGS: Azure, AI");
    }

    [Theory]
    [InlineData("plain text", "plain text")]
    [InlineData("```markdown\nBody\n```", "Body")]
    [InlineData("```md\nBody\n```", "Body")]
    [InlineData("Intro\n```csharp\ncode\n```\nOutro", "Intro\n```csharp\ncode\n```\nOutro")]
    public void UnwrapMarkdownFence_OnlyRemovesAFenceAroundTheWholeAnswer(string input, string expected)
    {
        AiCategorizationService.UnwrapMarkdownFence(input).Should().Be(expected);
    }
}
