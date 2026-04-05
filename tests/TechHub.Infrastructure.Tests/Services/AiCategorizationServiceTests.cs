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
/// Unit tests for <see cref="AiCategorizationService"/>.
/// Mocks the AI completion client to verify parsing, slug generation,
/// skip handling, roundup metadata extraction, and error resilience.
/// </summary>
public class AiCategorizationServiceTests
{
    private readonly Mock<IAiCompletionClient> _aiClient = new();

    private AiCategorizationService CreateSut(AiCategorizationOptions? opts = null)
    {
        var options = opts ?? new AiCategorizationOptions
        {
            MaxRetries = 1,
            RateLimitDelaySeconds = 0,
            MaxContentLength = 200_000
        };

        return new AiCategorizationService(
            _aiClient.Object,
            Options.Create(options),
            NullLogger<AiCategorizationService>.Instance);
    }

    private static RawFeedItem CreateRawItem(
        string url = "https://example.com/article",
        string title = "Test Article") => new()
    {
        Title = title,
        ExternalUrl = url,
        PublishedAt = new DateTimeOffset(2025, 6, 15, 12, 0, 0, TimeSpan.Zero),
        FeedItemData = "A test description",
        FeedName = "Test Feed",
        CollectionName = "blogs"
    };

    private static string WrapInAiResponse(string jsonContent)
    {
        var serialized = JsonSerializer.Serialize(jsonContent);
        return $$$"""{"choices":[{"message":{"content":{{{serialized}}}}}]}""";
    }

    // ── Included Item Parsing ─────────────────────────────────────────────────

    [Fact]
    public async Task CategorizeAsync_WithIncludedItem_ReturnsProcessedItem()
    {
        // Arrange
        var aiJson = """
            {
                "title": "Azure AI Foundry Update",
                "excerpt": "New features in Azure AI Foundry",
                "collection": "news",
                "sections": ["ai", "azure"],
                "tags": ["azure", "ai-foundry"],
                "author": "Microsoft",
                "primary_section": "ai",
                "content": "Full markdown content here.",
                "explanation": "Included: relevant Azure AI content",
                "roundup": {
                    "summary": "Azure AI Foundry gets new features.",
                    "key_topics": ["AI Foundry", "Azure"],
                    "relevance": "high",
                    "topic_type": "announcement",
                    "impact_level": "high",
                    "time_sensitivity": "immediate"
                }
            }
            """;
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, WrapInAiResponse(aiJson)));

        var sut = CreateSut();

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert
        result.Item.Should().NotBeNull();
        result.Item!.Title.Should().Be("Azure AI Foundry Update");
        result.Item.Excerpt.Should().Be("New features in Azure AI Foundry");
        result.Item.CollectionName.Should().Be("blogs");
        result.Item.Sections.Should().BeEquivalentTo(["ai", "azure"]);
        result.Item.Tags.Should().BeEquivalentTo(["azure", "ai-foundry"]);
        result.Item.PrimarySectionName.Should().Be("ai");
        result.Item.Content.Should().Be("Full markdown content here.");
        result.Explanation.Should().Contain("Included");
    }

    // ── Roundup Metadata Extraction ───────────────────────────────────────────

    [Fact]
    public async Task CategorizeAsync_WithRoundupMetadata_ExtractsAllFields()
    {
        // Arrange
        var aiJson = """
            {
                "title": "Copilot Studio GA",
                "excerpt": "Copilot Studio is now generally available",
                "collection": "news",
                "sections": ["ai", "github-copilot"],
                "tags": ["copilot"],
                "primary_section": "github-copilot",
                "content": "Content",
                "explanation": "Included",
                "roundup": {
                    "summary": "Copilot Studio reaches GA with new agent capabilities.",
                    "key_topics": ["Copilot Studio", "Agents"],
                    "relevance": "high",
                    "topic_type": "ga-release",
                    "impact_level": "high",
                    "time_sensitivity": "immediate"
                }
            }
            """;
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, WrapInAiResponse(aiJson)));

        var sut = CreateSut();

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert
        result.Item.Should().NotBeNull();
        var meta = result.Item!.RoundupMetadata;
        meta.Should().NotBeNull();
        meta!.Summary.Should().Be("Copilot Studio reaches GA with new agent capabilities.");
        meta.KeyTopics.Should().BeEquivalentTo(["Copilot Studio", "Agents"]);
        meta.Relevance.Should().Be("high");
        meta.TopicType.Should().Be("ga-release");
        meta.ImpactLevel.Should().Be("high");
        meta.TimeSensitivity.Should().Be("immediate");
    }

    [Fact]
    public async Task CategorizeAsync_WithoutRoundupMetadata_ReturnsNullRoundupMetadata()
    {
        // Arrange — no "roundup" property
        var aiJson = """
            {
                "title": "Some Article",
                "excerpt": "An article",
                "collection": "blogs",
                "sections": ["dotnet"],
                "tags": ["csharp"],
                "primary_section": "dotnet",
                "content": "Content",
                "explanation": "Included"
            }
            """;
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, WrapInAiResponse(aiJson)));

        var sut = CreateSut();

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert
        result.Item.Should().NotBeNull();
        result.Item!.RoundupMetadata.Should().BeNull();
    }

    // ── Excluded Item Handling ──────────────────────────────────────────────────

    [Fact]
    public async Task CategorizeAsync_WithExplanationOnly_ReturnsNullItem()
    {
        // Arrange — AI returns Option B (explanation only, no content fields)
        var aiJson = """
            {
                "explanation": "Content excluded: sales pitch without educational value"
            }
            """;
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, WrapInAiResponse(aiJson)));

        var sut = CreateSut();

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert — legitimate AI exclusion (Option B), not a failure
        result.Item.Should().BeNull();
        result.IsFailure.Should().BeFalse();
        result.Explanation.Should().Contain("sales pitch");
    }

    [Fact]
    public async Task CategorizeAsync_WithNoTitleAndNoSections_ReturnsNullItem()
    {
        // Arrange — AI returns explanation only (Option B response), no content fields
        var aiJson = """
            {
                "explanation": "Content excluded: not relevant to developer audience"
            }
            """;
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, WrapInAiResponse(aiJson)));

        var sut = CreateSut();

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert — legitimate AI exclusion (Option B), not a failure
        result.Item.Should().BeNull();
        result.IsFailure.Should().BeFalse();
        result.Explanation.Should().Contain("not relevant");
    }

    // ── Slug Generation ───────────────────────────────────────────────────────

    [Fact]
    public async Task CategorizeAsync_GeneratesDatePrefixedSlug()
    {
        // Arrange
        var aiJson = """
            {
                "title": "What's New in .NET 10?",
                "excerpt": "Overview of .NET 10 features",
                "collection": "blogs",
                "sections": ["dotnet"],
                "tags": ["dotnet"],
                "primary_section": "dotnet",
                "content": "Content",
                "explanation": "Included"
            }
            """;
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, WrapInAiResponse(aiJson)));

        var sut = CreateSut();
        var item = CreateRawItem(); // PublishedAt = 2025-06-15

        // Act
        var result = await sut.CategorizeAsync(item, CancellationToken.None);

        // Assert — slug starts with the date prefix and contains sanitized title
        result.Item.Should().NotBeNull();
        result.Item!.Slug.Should().StartWith("2025-06-15-");
        result.Item.Slug.Should().Contain("what-s-new-in-net-10");
    }

    // ── Content Hash ──────────────────────────────────────────────────────────

    [Fact]
    public async Task CategorizeAsync_ComputesContentHash()
    {
        // Arrange
        var aiJson = """
            {
                "title": "Hash Test",
                "excerpt": "Testing hash",
                "collection": "blogs",
                "sections": ["ai"],
                "tags": ["ai-hash"],
                "primary_section": "ai",
                "content": "Some content",
                "explanation": "Included"
            }
            """;
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, WrapInAiResponse(aiJson)));

        var sut = CreateSut();

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert
        result.Item.Should().NotBeNull();
        result.Item!.ContentHash.Should().NotBeNullOrWhiteSpace();
        result.Item.ContentHash.Should().HaveLength(64); // SHA-256 hex string
    }

    // ── JSON Wrapped in Markdown Code Fence ───────────────────────────────────

    [Fact]
    public async Task CategorizeAsync_HandlesJsonWrappedInCodeFence()
    {
        // Arrange — AI sometimes wraps JSON in ```json ... ```
        var wrappedJson = """
            ```json
            {
                "title": "Code Fence Article",
                "excerpt": "Wrapped in fence",
                "collection": "news",
                "sections": ["azure"],
                "tags": ["azure"],
                "primary_section": "azure",
                "content": "Content",
                "explanation": "Included"
            }
            ```
            """;
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, WrapInAiResponse(wrappedJson)));

        var sut = CreateSut();

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert
        result.Item.Should().NotBeNull();
        result.Item!.Title.Should().Be("Code Fence Article");
    }

    // ── Error Handling ────────────────────────────────────────────────────────

    [Fact]
    public async Task CategorizeAsync_WhenAiReturnsEmptyContent_ReturnsNullItemWithFinishReason()
    {
        // Arrange — AI returns empty content string with unknown finish_reason
        var response = """{"choices":[{"message":{"content":""},"finish_reason":"stop"}]}""";
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, response));

        var sut = CreateSut();

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert — failure: AI returned no content, explanation includes the finish_reason
        result.Item.Should().BeNull();
        result.IsFailure.Should().BeTrue();
        result.Explanation.Should().Contain("empty", because: "should describe the empty response");
        result.Explanation.Should().Contain("stop", because: "should include the finish_reason for diagnostics");
    }

    [Fact]
    public async Task CategorizeAsync_WhenFinishReasonIsLengthWithEmptyContent_ShowsFinishReasonAndUsage()
    {
        // Arrange — AI returned finish_reason: length but content is empty (model ran out of tokens before producing output)
        var response = """{"choices":[{"message":{"content":null},"finish_reason":"length"}],"usage":{"prompt_tokens":3800,"completion_tokens":0,"total_tokens":3800}}""";
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, response));

        var sut = CreateSut();

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert — explanation includes the finish_reason and token usage so you can see what went wrong
        result.Item.Should().BeNull();
        result.IsFailure.Should().BeTrue();
        result.Explanation.Should().Contain("empty", because: "content was empty");
        result.Explanation.Should().Contain("length", because: "finish_reason should be included verbatim");
        result.Explanation.Should().Contain("3800", because: "token usage should be included for diagnostics");
    }

    [Fact]
    public async Task CategorizeAsync_WhenFinishReasonIsLength_IncludesTokenUsageInExplanation()
    {
        // Arrange — AI response has non-empty content but finish_reason is "length" (truncated), with usage info
        var response = """{"choices":[{"message":{"content":"{ \"title\": \"Trunca"},"finish_reason":"length"}],"usage":{"prompt_tokens":3800,"completion_tokens":200,"total_tokens":4000}}""";
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, response));

        var sut = CreateSut();

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert — should include finish_reason and token usage info for diagnostics
        result.Item.Should().BeNull();
        result.IsFailure.Should().BeTrue();
        result.Explanation.Should().Contain("incomplete", because: "non-stop finish_reason means the response is incomplete");
        result.Explanation.Should().Contain("length", because: "finish_reason should be included verbatim");
        result.Explanation.Should().Contain("3800", because: "prompt token count helps diagnose why length was exceeded");
        result.Explanation.Should().Contain("200", because: "completion token count shows how much was generated before cutoff");
    }

    [Fact]
    public async Task CategorizeAsync_WhenFinishReasonIsContentFilter_ReturnsContentFilterFailure()
    {
        // Arrange — Azure OpenAI returns 200 OK but finish_reason is "content_filter" with null content
        var response = """{"choices":[{"message":{"content":null},"finish_reason":"content_filter"}]}""";
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, response));

        var sut = CreateSut();

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert — explanation includes the finish_reason so you can see it was content_filter
        result.Item.Should().BeNull();
        result.IsFailure.Should().BeTrue();
        result.Explanation.Should().Contain("content_filter", because: "finish_reason should be included verbatim in the explanation");
    }

    [Fact]
    public async Task CategorizeAsync_WhenFinishReasonIsLength_ReturnsIncompleteFailure()
    {
        // Arrange — AI response was truncated due to max_completion_tokens
        var response = """{"choices":[{"message":{"content":"{ \"title\": \"Trunca"},"finish_reason":"length"}]}""";
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, response));

        var sut = CreateSut();

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert — explanation includes finish_reason verbatim
        result.Item.Should().BeNull();
        result.IsFailure.Should().BeTrue();
        result.Explanation.Should().Contain("incomplete", because: "non-stop finish_reason means the response is incomplete");
        result.Explanation.Should().Contain("length", because: "finish_reason should appear verbatim");
    }

    [Fact]
    public async Task CategorizeAsync_WhenFinishReasonIsContentFilterWithEmptyString_ReturnsContentFilterFailure()
    {
        // Arrange — finish_reason content_filter with empty string content (not null)
        var response = """{"choices":[{"message":{"content":""},"finish_reason":"content_filter"}]}""";
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, response));

        var sut = CreateSut();

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert — explanation includes the finish_reason verbatim
        result.Item.Should().BeNull();
        result.IsFailure.Should().BeTrue();
        result.Explanation.Should().Contain("content_filter", because: "finish_reason should appear in the explanation");
    }

    [Fact]
    public async Task CategorizeAsync_WhenFinishReasonIsStop_ProcessesNormally()
    {
        // Arrange — normal response with finish_reason "stop"
        var aiJson = """
            {
                "title": "Normal Article",
                "excerpt": "Normal excerpt",
                "collection": "blogs",
                "sections": ["ai"],
                "tags": ["ai-test"],
                "primary_section": "ai",
                "content": "Normal content",
                "explanation": "Included"
            }
            """;
        var serialized = JsonSerializer.Serialize(aiJson);
        var response = $$$"""{"choices":[{"message":{"content":{{{serialized}}}},"finish_reason":"stop"}]}""";
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, response));

        var sut = CreateSut();

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert — should process normally when finish_reason is "stop"
        result.Item.Should().NotBeNull();
        result.Item!.Title.Should().Be("Normal Article");
    }

    [Fact]
    public async Task CategorizeAsync_WhenAiReturnsInvalidJson_ReturnsNullItem()
    {
        // Arrange — response body contains non-JSON text
        var invalidJson = "This is not valid JSON at all";
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, WrapInAiResponse(invalidJson)));

        var sut = CreateSut();

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert — failure: response was not valid JSON
        result.Item.Should().BeNull();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task CategorizeAsync_WhenAllRetriesFail_ThrowsOnLastAttempt()
    {
        // Arrange — HTTP exception on every attempt
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("Service unavailable"));

        // With MaxRetries=1, the single attempt throws and is not caught
        var sut = CreateSut(new AiCategorizationOptions { MaxRetries = 1, RateLimitDelaySeconds = 0 });

        // Act & Assert — exception propagates on the last attempt
        var act = () => sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Fact]
    public async Task CategorizeAsync_WhenTimeoutOnNonFinalAttempt_Retries()
    {
        // Arrange — first call times out, second succeeds
        var aiJson = """
            {
                "title": "Timeout Retry Article",
                "excerpt": "After timeout retry",
                "collection": "news",
                "sections": ["ai"],
                "tags": ["ai-retry"],
                "primary_section": "ai",
                "content": "Content",
                "explanation": "Included"
            }
            """;

        var callCount = 0;
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() =>
            {
                callCount++;
                if (callCount == 1)
                {
                    throw new TaskCanceledException("The operation was canceled.", new TimeoutException());
                }

                return new AiCompletionResult(false, WrapInAiResponse(aiJson));
            });

        var sut = CreateSut(new AiCategorizationOptions { MaxRetries = 3, RateLimitDelaySeconds = 0 });

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert
        result.Item.Should().NotBeNull();
        result.Item!.Title.Should().Be("Timeout Retry Article");
        callCount.Should().Be(2);
    }

    [Fact]
    public async Task CategorizeAsync_WhenAllRetriesRateLimited_ReturnsNullItem()
    {
        // Arrange — every attempt rate-limited
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(true, null));

        var sut = CreateSut(new AiCategorizationOptions { MaxRetries = 2, RateLimitDelaySeconds = 0 });

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert — failure: exhausted retries, returns null with explanation
        result.Item.Should().BeNull();
        result.IsFailure.Should().BeTrue();
        result.Explanation.Should().Contain("failed after");
    }

    // ── Rate Limiting ─────────────────────────────────────────────────────────

    [Fact]
    public async Task CategorizeAsync_WhenRateLimited_RetriesAndSucceeds()
    {
        // Arrange — first call rate-limited, second succeeds
        var aiJson = """
            {
                "title": "Rate Limited Article",
                "excerpt": "After retry",
                "collection": "news",
                "sections": ["ai"],
                "tags": ["ai-rate"],
                "primary_section": "ai",
                "content": "Content",
                "explanation": "Included"
            }
            """;

        var callCount = 0;
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() =>
            {
                callCount++;
                return callCount == 1
                    ? new AiCompletionResult(true, null)
                    : new AiCompletionResult(false, WrapInAiResponse(aiJson));
            });

        var sut = CreateSut(new AiCategorizationOptions { MaxRetries = 3, RateLimitDelaySeconds = 0 });

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert
        result.Item.Should().NotBeNull();
        result.Item!.Title.Should().Be("Rate Limited Article");
        callCount.Should().Be(2);
    }

    // ── Content Filter ────────────────────────────────────────────────────────

    [Fact]
    public async Task CategorizeAsync_WhenContextLengthExceeded_ReturnsNullItemWithExplanation()
    {
        // Arrange — AI client returns context length exceeded result
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, null, ContentFilterMessage: "Content too large for AI model context window"));

        var sut = CreateSut(new AiCategorizationOptions { MaxRetries = 3, RateLimitDelaySeconds = 0 });

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert — failure: content exceeded context window
        result.Item.Should().BeNull();
        result.IsFailure.Should().BeTrue();
        result.Explanation.Should().Contain("too large");
        _aiClient.Verify(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    // ── Content Filter ────────────────────────────────────────────────────────

    [Fact]
    public async Task CategorizeAsync_WhenContentFilterTriggered_ReturnsNullItemWithExplanation()
    {
        // Arrange — AI client returns content filter result
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, null, ContentFilterMessage: "Content blocked by Azure AI content filter"));

        var sut = CreateSut(new AiCategorizationOptions { MaxRetries = 3, RateLimitDelaySeconds = 0 });

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert — failure: content filter blocked the request
        result.Item.Should().BeNull();
        result.IsFailure.Should().BeTrue();
        result.Explanation.Should().Contain("content filter");
        _aiClient.Verify(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    // ── User Prompt Construction ──────────────────────────────────────────────

    [Fact]
    public async Task CategorizeAsync_IncludesFeedTagsInPrompt()
    {
        // Arrange
        var aiJson = """
            { "explanation": "Excluded" }
            """;
        string? capturedBody = null;
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<string, CancellationToken>((body, _) => capturedBody = body)
            .ReturnsAsync(new AiCompletionResult(false, WrapInAiResponse(aiJson)));

        var sut = CreateSut();
        var item = new RawFeedItem
        {
            Title = "Tagged Article",
            ExternalUrl = "https://example.com/tagged",
            PublishedAt = DateTimeOffset.UtcNow,
            FeedName = "Feed",
            CollectionName = "blogs",
            FeedTags = ["azure", "copilot", "security"]
        };

        // Act
        await sut.CategorizeAsync(item, CancellationToken.None);

        // Assert — user content should include feed tags
        capturedBody.Should().NotBeNull();
        using var doc = JsonDocument.Parse(capturedBody!);
        var userContent = doc.RootElement
            .GetProperty("messages")[1]
            .GetProperty("content")
            .GetString();
        userContent.Should().Contain("FEED TAGS: azure, copilot, security");
    }

    // ── Validation: Incomplete AI Responses ───────────────────────────────────

    [Theory]
    [InlineData("", "An excerpt", """["ai"]""", """["ai-tag"]""", "Content", "missing title")]
    [InlineData("Title", "", """["ai"]""", """["ai-tag"]""", "Content", "missing excerpt")]
    [InlineData("Title", "An excerpt", """["ai"]""", """["ai-tag"]""", "", "missing content")]
    [InlineData("Title", "An excerpt", """[]""", """["ai-tag"]""", "Content", "missing sections")]
    [InlineData("Title", "An excerpt", """["ai"]""", """[]""", "Content", "missing tags")]
    [InlineData("Title", "An excerpt", """["invalid-section"]""", """["tag"]""", "Content", "invalid sections")]
    [InlineData("Title", "An excerpt", """["ai", "fake"]""", """["tag"]""", "Content", "mix of valid and invalid sections")]
    public async Task CategorizeAsync_WithMissingRequiredField_ReturnsNullItem(
        string title, string excerpt, string sectionsJson,
        string tagsJson, string content, string scenario)
    {
        // Arrange — AI returns a response with one or more required fields missing or invalid
        var aiJson = $$"""
            {
                "title": "{{title}}",
                "excerpt": "{{excerpt}}",
                "sections": {{sectionsJson}},
                "tags": {{tagsJson}},
                "primary_section": "ai",
                "content": "{{content}}",
                "explanation": "Included: relevant content"
            }
            """;
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, WrapInAiResponse(aiJson)));

        var sut = CreateSut();

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert — failure: incomplete AI response is a failure, not a skip
        result.Item.Should().BeNull(scenario);
        result.IsFailure.Should().BeTrue(scenario);
        result.Explanation.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task CategorizeAsync_WithAllRequiredFieldsFilled_ReturnsItem()
    {
        // Arrange — sanity check: a fully valid response still works
        var aiJson = """
            {
                "title": "Valid Article",
                "excerpt": "A great excerpt",
                "collection": "blogs",
                "sections": ["dotnet"],
                "tags": ["csharp"],
                "primary_section": "dotnet",
                "content": "Full content with substance",
                "explanation": "Included: relevant .NET content about C# patterns"
            }
            """;
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, WrapInAiResponse(aiJson)));

        var sut = CreateSut();

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert — valid item should still be returned
        result.Item.Should().NotBeNull();
        result.Item!.Title.Should().Be("Valid Article");
        result.Item.Content.Should().Be("Full content with substance");
    }

    // ── Primary Section Passthrough ───────────────────────────────────────────

    [Fact]
    public async Task CategorizeAsync_PassesThroughAiPrimarySection()
    {
        // Arrange — AI returns primary_section, it should be used as-is
        var aiJson = """
            {
                "title": "Azure ML Article",
                "excerpt": "About Azure ML",
                "collection": "blogs",
                "sections": ["azure", "ml"],
                "tags": ["machine-learning"],
                "primary_section": "ml",
                "content": "Content about ML on Azure",
                "explanation": "Included"
            }
            """;
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, WrapInAiResponse(aiJson)));

        var sut = CreateSut();

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert — AI's primary section choice should be preserved
        result.Item.Should().NotBeNull();
        result.Item!.PrimarySectionName.Should().Be("ml");
    }

    [Fact]
    public async Task CategorizeAsync_WithNoPrimarySection_RejectsItem()
    {
        // Arrange — AI omits primary_section
        var aiJson = """
            {
                "title": "No Primary",
                "excerpt": "Missing primary",
                "collection": "blogs",
                "sections": ["ai", "azure"],
                "tags": ["cloud"],
                "content": "Content",
                "explanation": "Included"
            }
            """;
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, WrapInAiResponse(aiJson)));

        var sut = CreateSut();

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert — failure: missing primary_section
        result.Item.Should().BeNull();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task CategorizeAsync_WithInvalidPrimarySection_RejectsItem()
    {
        // Arrange — AI returns an invalid primary_section not in the known list
        var aiJson = """
            {
                "title": "Invalid Primary",
                "excerpt": "Bad primary section",
                "collection": "blogs",
                "sections": ["ai"],
                "tags": ["cloud"],
                "primary_section": "made-up-section",
                "content": "Content",
                "explanation": "Included"
            }
            """;
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, WrapInAiResponse(aiJson)));

        var sut = CreateSut();

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert — failure: invalid primary_section
        result.Item.Should().BeNull();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task CategorizeAsync_WithPrimarySectionNotInSections_RejectsItem()
    {
        // Arrange — AI returns primary_section that is valid but not in sections list
        var aiJson = """
            {
                "title": "Mismatched Primary",
                "excerpt": "Primary not in sections",
                "collection": "blogs",
                "sections": ["ai", "azure"],
                "tags": ["cloud"],
                "primary_section": "dotnet",
                "content": "Content",
                "explanation": "Included"
            }
            """;
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, WrapInAiResponse(aiJson)));

        var sut = CreateSut();

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert — failure: primary_section not in sections array
        result.Item.Should().BeNull();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task CategorizeAsync_WithWhitespaceOnlyTags_ReturnsNullItem()
    {
        // Arrange — AI returns tags that are all whitespace
        var aiJson = """
            {
                "title": "Whitespace Tags",
                "excerpt": "An excerpt",
                "collection": "blogs",
                "sections": ["ai"],
                "tags": ["  ", "", " "],
                "primary_section": "ai",
                "content": "Content",
                "explanation": "Included"
            }
            """;
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, WrapInAiResponse(aiJson)));

        var sut = CreateSut();

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert — failure: whitespace-only tags
        result.Item.Should().BeNull();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task CategorizeAsync_WithExcerptOnly_NoContent_RejectsItem()
    {
        // Arrange — excerpt is present but content is empty
        var aiJson = """
            {
                "title": "Excerpt Only",
                "excerpt": "A solid excerpt",
                "collection": "videos",
                "sections": ["ai"],
                "tags": ["azure-ai"],
                "primary_section": "ai",
                "content": "",
                "explanation": "Included"
            }
            """;
        _aiClient
            .Setup(c => c.SendCompletionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AiCompletionResult(false, WrapInAiResponse(aiJson)));

        var sut = CreateSut();

        // Act
        var result = await sut.CategorizeAsync(CreateRawItem(), CancellationToken.None);

        // Assert — failure: no fallbacks, missing content means rejection
        result.Item.Should().BeNull();
        result.IsFailure.Should().BeTrue();
    }
}
