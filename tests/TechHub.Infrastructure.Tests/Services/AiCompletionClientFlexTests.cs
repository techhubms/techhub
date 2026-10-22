using System.Net;
using System.Text.Json;
using Azure.Core;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Infrastructure.Services;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Tests for the Flex service tier handling and per-call usage logging of <see cref="AiCompletionClient"/>.
/// </summary>
public class AiCompletionClientFlexTests
{
    private const string FlexBody = """{"model":"gpt-6-luna","messages":[],"service_tier":"flex"}""";
    private const string StandardBody = """{"model":"gpt-5.2","messages":[]}""";
    private const string OkResponse = """{"choices":[]}""";

    private static AiCategorizationOptions FastOptions(
        TimeSpan? maxWait = null,
        bool fallback = true) => new()
        {
            Endpoint = "https://ai.example.com/",
            FlexMaxWait = maxWait ?? TimeSpan.FromMilliseconds(200),
            FlexMinRetryDelay = TimeSpan.FromMilliseconds(5),
            FlexMaxRetryDelay = TimeSpan.FromMilliseconds(20),
            FlexFallbackToDefaultTier = fallback
        };

    private static AiCompletionClient CreateClient(
        HttpMessageHandler handler,
        AiCategorizationOptions options,
        ILogger<AiCompletionClient>? logger = null)
    {
        return new AiCompletionClient(
            new HttpClient(handler),
            Options.Create(options),
            logger ?? NullLogger<AiCompletionClient>.Instance,
            new StubTokenCredential());
    }

    private static string RequestedTier(string body)
    {
        using var doc = JsonDocument.Parse(body);
        return doc.RootElement.TryGetProperty("service_tier", out var tier) ? tier.GetString()! : "default";
    }

    [Fact]
    public async Task SendCompletionAsync_FlexRateLimitedThenSuccess_RetriesOnFlex()
    {
        // Arrange
        var bodies = new List<string>();
        var calls = 0;
        using var handler = new RecordingHandler(bodies, _ => ++calls < 3
            ? new HttpResponseMessage(HttpStatusCode.TooManyRequests)
            : new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(OkResponse) });
        var client = CreateClient(handler, FastOptions());

        // Act
        var result = await client.SendCompletionAsync(FlexBody);

        // Assert
        result.IsRateLimited.Should().BeFalse();
        result.ResponseBody.Should().Be(OkResponse);
        bodies.Should().HaveCount(3);
        bodies.Should().OnlyContain(b => RequestedTier(b) == "flex");
    }

    [Fact]
    public async Task SendCompletionAsync_FlexRetryAfterBeyondMaxRetryDelay_IsNotShortened()
    {
        // Arrange: the server asks for 3s, longer than FlexMaxRetryDelay (20ms) and FlexMaxWait (200ms).
        var bodies = new List<string>();
        using var handler = new RecordingHandler(bodies, request =>
        {
            if (RequestedTier(request) != "flex")
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(OkResponse) };
            }

            var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests);
            response.Headers.TryAddWithoutValidation("Retry-After", "3");
            return response;
        });
        var client = CreateClient(handler, FastOptions());

        // Act
        var result = await client.SendCompletionAsync(FlexBody, TestContext.Current.CancellationToken);

        // Assert: waiting would exceed FlexMaxWait, so there is no second Flex attempt before the fallback.
        result.ResponseBody.Should().Be(OkResponse);
        bodies.Should().HaveCount(2);
        RequestedTier(bodies[0]).Should().Be("flex");
        RequestedTier(bodies[1]).Should().Be("default");
    }

    [Fact]
    public async Task SendCompletionAsync_FlexStaysRateLimited_FallsBackToStandardTier()
    {
        // Arrange
        var bodies = new List<string>();
        using var handler = new RecordingHandler(bodies, request =>
            RequestedTier(request) == "flex"
                ? new HttpResponseMessage(HttpStatusCode.TooManyRequests)
                : new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(OkResponse) });
        var client = CreateClient(handler, FastOptions(maxWait: TimeSpan.FromMilliseconds(40)));

        // Act
        var result = await client.SendCompletionAsync(FlexBody);

        // Assert
        result.ResponseBody.Should().Be(OkResponse);
        bodies.Should().HaveCountGreaterThan(1);
        RequestedTier(bodies[^1]).Should().Be("default");
        bodies.Take(bodies.Count - 1).Should().OnlyContain(b => RequestedTier(b) == "flex");
    }

    [Fact]
    public async Task SendCompletionAsync_FallbackKeepsTheRestOfTheRequestUnchanged()
    {
        // Arrange
        var bodies = new List<string>();
        using var handler = new RecordingHandler(bodies, request =>
            RequestedTier(request) == "flex"
                ? new HttpResponseMessage(HttpStatusCode.TooManyRequests)
                : new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(OkResponse) });
        var client = CreateClient(handler, FastOptions(maxWait: TimeSpan.FromMilliseconds(20)));

        // Act
        await client.SendCompletionAsync(FlexBody);

        // Assert
        using var doc = JsonDocument.Parse(bodies[^1]);
        doc.RootElement.GetProperty("model").GetString().Should().Be("gpt-6-luna");
        doc.RootElement.TryGetProperty("messages", out _).Should().BeTrue();
    }

    [Fact]
    public async Task SendCompletionAsync_FlexStaysRateLimitedWithoutFallback_ReturnsRateLimited()
    {
        // Arrange
        var bodies = new List<string>();
        using var handler = new RecordingHandler(bodies, _ => new HttpResponseMessage(HttpStatusCode.TooManyRequests));
        var client = CreateClient(handler, FastOptions(maxWait: TimeSpan.FromMilliseconds(40), fallback: false));

        // Act
        var result = await client.SendCompletionAsync(FlexBody);

        // Assert
        result.IsRateLimited.Should().BeTrue();
        bodies.Should().OnlyContain(b => RequestedTier(b) == "flex");
    }

    [Fact]
    public async Task SendCompletionAsync_StandardTierRateLimited_ReturnsImmediatelyWithoutRetry()
    {
        // Arrange
        var bodies = new List<string>();
        using var handler = new RecordingHandler(bodies, _ => new HttpResponseMessage(HttpStatusCode.TooManyRequests));
        var client = CreateClient(handler, FastOptions());

        // Act
        var result = await client.SendCompletionAsync(StandardBody);

        // Assert
        result.IsRateLimited.Should().BeTrue();
        bodies.Should().ContainSingle();
    }

    [Fact]
    public async Task SendCompletionAsync_FlexContentFilter_DoesNotRetry()
    {
        // Arrange
        var bodies = new List<string>();
        using var handler = new RecordingHandler(bodies, _ => new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("""{"error":{"code":"content_filter","message":"blocked"}}""")
        });
        var client = CreateClient(handler, FastOptions());

        // Act
        var result = await client.SendCompletionAsync(FlexBody);

        // Assert
        result.ContentFilterMessage.Should().NotBeNull();
        bodies.Should().ContainSingle();
    }

    [Fact]
    public async Task SendCompletionAsync_Success_LogsUsageAndServedTier()
    {
        // Arrange
        const string Response = """
            {
              "model": "gpt-6-luna-2026-09-22",
              "service_tier": "flex",
              "choices": [],
              "usage": {
                "prompt_tokens": 1000,
                "completion_tokens": 700,
                "prompt_tokens_details": { "cached_tokens": 640 },
                "completion_tokens_details": { "reasoning_tokens": 500 }
              }
            }
            """;
        var logger = new CapturingLogger();
        using var handler = new RecordingHandler([], _ => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(Response) });
        var client = CreateClient(handler, FastOptions(), logger);

        // Act
        await client.SendCompletionAsync(FlexBody);

        // Assert
        var message = logger.Messages.Should().ContainSingle(m => m.Contains("AI usage", StringComparison.Ordinal)).Subject;
        message.Should().Contain("gpt-6-luna");
        message.Should().Contain("flex");
        message.Should().Contain("1000");
        message.Should().Contain("640");
        message.Should().Contain("700");
        message.Should().Contain("500");
    }

    [Fact]
    public async Task SendCompletionAsync_SuccessWithoutUsage_DoesNotThrow()
    {
        // Arrange
        using var handler = new RecordingHandler([], _ => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("not json") });
        var client = CreateClient(handler, FastOptions());

        // Act
        var result = await client.SendCompletionAsync(StandardBody);

        // Assert
        result.ResponseBody.Should().Be("not json");
    }

    private sealed class RecordingHandler : HttpMessageHandler
    {
        private readonly List<string> _bodies;
        private readonly Func<string, HttpResponseMessage> _responder;

        public RecordingHandler(List<string> bodies, Func<string, HttpResponseMessage> responder)
        {
            _bodies = bodies;
            _responder = responder;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var body = await request.Content!.ReadAsStringAsync(cancellationToken);
            _bodies.Add(body);
            return _responder(body);
        }
    }

    private sealed class StubTokenCredential : TokenCredential
    {
        public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
            => throw new NotSupportedException();

        public override ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
            => ValueTask.FromResult(new AccessToken("test-token", DateTimeOffset.MaxValue));
    }

    private sealed class CapturingLogger : ILogger<AiCompletionClient>
    {
        public List<string> Messages { get; } = [];

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            => Messages.Add(formatter(state, exception));
    }
}
