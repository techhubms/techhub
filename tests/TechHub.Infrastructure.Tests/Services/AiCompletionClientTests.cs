using System.Net;
using System.Net.Http.Headers;
using Azure.Core;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Infrastructure.Services;

namespace TechHub.Infrastructure.Tests.Services;

public class AiCompletionClientTests
{
    [Fact]
    public void Constructor_NullHttpClient_ThrowsArgumentNullException()
    {
        // Act
        var act = () => new AiCompletionClient(null!, Options.Create(new AiCategorizationOptions()), NullLogger<AiCompletionClient>.Instance);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("httpClient");
    }

    [Fact]
    public void Constructor_NullOptions_ThrowsArgumentNullException()
    {
        // Arrange
        using var httpClient = new HttpClient();

        // Act
        var act = () => new AiCompletionClient(httpClient, null!, NullLogger<AiCompletionClient>.Instance);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("options");
    }

    [Fact]
    public void Constructor_NullLogger_ThrowsArgumentNullException()
    {
        // Arrange
        using var httpClient = new HttpClient();

        // Act
        var act = () => new AiCompletionClient(httpClient, Options.Create(new AiCategorizationOptions()), null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("logger");
    }

    [Fact]
    public async Task SendCompletionAsync_Success_SendsAuthenticatedJsonRequestAndReturnsBody()
    {
        // Arrange
        const string JsonBody = """{"messages":[]}""";
        const string ResponseBody = """{"choices":[]}""";
        var credential = new StubTokenCredential();
        Task<string>? sentBody = null;
        using var handler = new StubHttpMessageHandler(request =>
        {
            request.Method.Should().Be(HttpMethod.Post);
            request.RequestUri.Should().Be("https://ai.example.com/openai/deployments/test-model/chat/completions?api-version=2025-04-01-preview");
            request.Headers.Authorization.Should().Be(new AuthenticationHeaderValue("Bearer", "test-token"));
            request.Content!.Headers.ContentType!.MediaType.Should().Be("application/json");
            sentBody = request.Content.ReadAsStringAsync(CancellationToken.None);
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(ResponseBody) };
        });
        using var httpClient = new HttpClient(handler);
        var client = CreateClient(httpClient, credential);
        using var cts = new CancellationTokenSource();

        // Act
        var result = await client.SendCompletionAsync(JsonBody, cts.Token);

        // Assert
        result.IsRateLimited.Should().BeFalse();
        result.ResponseBody.Should().Be(ResponseBody);
        result.RetryAfterSeconds.Should().BeNull();
        result.ContentFilterMessage.Should().BeNull();
        (await sentBody!).Should().Be(JsonBody);
        credential.Scopes.Should().Equal("https://cognitiveservices.azure.com/.default");
        credential.CancellationToken.Should().Be(cts.Token);
        handler.RequestedUrls.Should().ContainSingle();
    }

    [Theory]
    [InlineData(null, null, null)]
    [InlineData("Retry-After", "42", 42)]
    [InlineData("Retry-After", "0", 0)]
    [InlineData("Retry-After", "invalid", null)]
    [InlineData("x-ratelimit-reset-requests", "12", 12)]
    [InlineData("x-ratelimit-timeremaining", "15", 15)]
    [InlineData("retry-after-ms", "2500", 2)]
    [InlineData("retry-after-ms", "500", 1)]
    [InlineData("x-ratelimit-reset-requests", "invalid", null)]
    [InlineData("x-ratelimit-reset-requests", "0", null)]
    [InlineData("x-ratelimit-timeremaining", "-1", null)]
    [InlineData("retry-after-ms", "0", null)]
    public async Task SendCompletionAsync_RateLimited_ParsesRetryHeaders(string? header, string? value, int? expectedSeconds)
    {
        // Arrange
        using var handler = new StubHttpMessageHandler(_ =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests);
            if (header is not null)
            {
                response.Headers.TryAddWithoutValidation(header, value);
            }
            return response;
        });
        using var httpClient = new HttpClient(handler);
        var client = CreateClient(httpClient);

        // Act
        var result = await client.SendCompletionAsync("{}", CancellationToken.None);

        // Assert
        result.IsRateLimited.Should().BeTrue();
        result.ResponseBody.Should().BeNull();
        result.RetryAfterSeconds.Should().Be(expectedSeconds);
        result.ContentFilterMessage.Should().BeNull();
        handler.RequestedUrls.Should().ContainSingle();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(-1)]
    public async Task SendCompletionAsync_RetryAfterDate_ReturnsPositiveWaitOrNull(int daysFromNow)
    {
        // Arrange
        var retryDate = DateTimeOffset.FromUnixTimeSeconds(DateTimeOffset.UtcNow.AddDays(daysFromNow).ToUnixTimeSeconds());
        using var handler = new StubHttpMessageHandler(_ =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests);
            response.Headers.RetryAfter = new RetryConditionHeaderValue(retryDate);
            return response;
        });
        using var httpClient = new HttpClient(handler);
        var client = CreateClient(httpClient);
        var maximumWait = (int)(retryDate - DateTimeOffset.UtcNow).TotalSeconds;

        // Act
        var result = await client.SendCompletionAsync("{}", CancellationToken.None);

        // Assert
        result.IsRateLimited.Should().BeTrue();
        if (daysFromNow > 0)
        {
            result.RetryAfterSeconds.Should().NotBeNull();
            result.RetryAfterSeconds!.Value.Should().BeInRange(
                (int)(retryDate - DateTimeOffset.UtcNow).TotalSeconds, maximumWait);
        }
        else
        {
            result.RetryAfterSeconds.Should().BeNull();
        }
    }

    [Theory]
    [InlineData("7", "12", "15", 7)]
    [InlineData(null, "12", "15", 12)]
    [InlineData("invalid", "invalid", "15", 15)]
    [InlineData(null, "0", "-1", 2)]
    public async Task SendCompletionAsync_MultipleRetryHeaders_UsesFirstValidHeader(
        string? retryAfter, string? resetRequests, string? timeRemaining, int expectedSeconds)
    {
        // Arrange
        using var handler = new StubHttpMessageHandler(_ =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests);
            response.Headers.TryAddWithoutValidation("Retry-After", retryAfter);
            response.Headers.TryAddWithoutValidation("x-ratelimit-reset-requests", resetRequests);
            response.Headers.TryAddWithoutValidation("x-ratelimit-timeremaining", timeRemaining);
            response.Headers.TryAddWithoutValidation("retry-after-ms", "2000");
            return response;
        });
        using var httpClient = new HttpClient(handler);
        var client = CreateClient(httpClient);

        // Act
        var result = await client.SendCompletionAsync("{}", CancellationToken.None);

        // Assert
        result.RetryAfterSeconds.Should().Be(expectedSeconds);
    }

    [Theory]
    [InlineData("content_filter")]
    [InlineData("Content Filter")]
    [InlineData("ResponsibleAIPolicyViolation")]
    [InlineData("JAILBREAK")]
    [InlineData("context_length_exceeded")]
    [InlineData("Too Many Tokens")]
    public async Task SendCompletionAsync_NonRetryableBadRequest_ReturnsDetailedMessage(string code)
    {
        // Arrange
        var body = $$$"""{"error":{"code":"{{{code}}}","message":"Request rejected"}}""";
        var prefix = code is "context_length_exceeded" or "Too Many Tokens"
            ? "Content too large for AI model context window"
            : "Content blocked by Azure AI content filter";
        using var handler = new StubHttpMessageHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(body) });
        using var httpClient = new HttpClient(handler);
        var client = CreateClient(httpClient);

        // Act
        var result = await client.SendCompletionAsync("{}", CancellationToken.None);

        // Assert
        result.IsRateLimited.Should().BeFalse();
        result.ResponseBody.Should().BeNull();
        result.RetryAfterSeconds.Should().BeNull();
        result.ContentFilterMessage.Should().Be($"{prefix} — {code}: Request rejected");
    }

    [Theory]
    [InlineData("""{"error":{"message":"content_filter blocked"}}""", "content_filter blocked")]
    [InlineData("""{"error":{"code":"","message":"content_filter blocked"}}""", "content_filter blocked")]
    [InlineData("""{"error":{"code":"content_filter"}}""", """{"error":{"code":"content_filter"}}""")]
    [InlineData("""{"error":{"code":"content_filter","message":" "}}""", """{"error":{"code":"content_filter","message":" "}}""")]
    [InlineData("""{"message":"content_filter blocked"}""", """{"message":"content_filter blocked"}""")]
    [InlineData("content_filter: not JSON", "content_filter: not JSON")]
    public async Task SendCompletionAsync_ContentFilter_ExtractsMessageOrFallsBackToBody(string body, string expectedDetail)
    {
        // Arrange
        using var handler = new StubHttpMessageHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(body) });
        using var httpClient = new HttpClient(handler);
        var client = CreateClient(httpClient);

        // Act
        var result = await client.SendCompletionAsync("{}", CancellationToken.None);

        // Assert
        result.ContentFilterMessage.Should().Be($"Content blocked by Azure AI content filter — {expectedDetail}");
    }

    [Fact]
    public async Task SendCompletionAsync_LongNonJsonError_TruncatesFallbackMessage()
    {
        // Arrange
        var body = "content_filter: " + new string('x', 250);
        using var handler = new StubHttpMessageHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(body) });
        using var httpClient = new HttpClient(handler);
        var client = CreateClient(httpClient);

        // Act
        var result = await client.SendCompletionAsync("{}", CancellationToken.None);

        // Assert
        result.ContentFilterMessage.Should().Be($"Content blocked by Azure AI content filter — {body[..200]}…");
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest, "")]
    [InlineData(HttpStatusCode.BadRequest, """{"error":{"message":"Invalid deployment"}}""")]
    [InlineData(HttpStatusCode.Unauthorized, "content_filter")]
    [InlineData(HttpStatusCode.InternalServerError, "context_length_exceeded")]
    public async Task SendCompletionAsync_UnhandledError_ThrowsHttpRequestException(HttpStatusCode status, string body)
    {
        // Arrange
        using var handler = new StubHttpMessageHandler(_ =>
            new HttpResponseMessage(status) { Content = new StringContent(body) });
        using var httpClient = new HttpClient(handler);
        var client = CreateClient(httpClient);

        // Act
        var act = () => client.SendCompletionAsync("{}", CancellationToken.None);

        // Assert
        (await act.Should().ThrowAsync<HttpRequestException>()).Which.StatusCode.Should().Be(status);
    }

    [Fact]
    public void Constructor_TypedHttpClientRegistrationWithoutCredential_ResolvesClient()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        services.Configure<AiCategorizationOptions>(_ => { });
        services.AddHttpClient<TechHub.Core.Interfaces.IAiCompletionClient, AiCompletionClient>();
        using var provider = services.BuildServiceProvider();

        // Act
        var client = provider.GetRequiredService<TechHub.Core.Interfaces.IAiCompletionClient>();

        // Assert
        client.Should().BeOfType<AiCompletionClient>();
    }

    private static AiCompletionClient CreateClient(HttpClient httpClient, TokenCredential? credential = null)
    {
        return new AiCompletionClient(httpClient, Options.Create(new AiCategorizationOptions
        {
            Endpoint = "https://ai.example.com/",
            DeploymentName = "test-model",
            ApiVersion = "2025-04-01-preview"
        }), NullLogger<AiCompletionClient>.Instance, credential ?? new StubTokenCredential());
    }

    private sealed class StubTokenCredential : TokenCredential
    {
        public string[]? Scopes { get; private set; }
        public CancellationToken CancellationToken { get; private set; }

        public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public override ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            Scopes = requestContext.Scopes;
            CancellationToken = cancellationToken;
            return ValueTask.FromResult(new AccessToken("test-token", DateTimeOffset.MaxValue));
        }
    }
}
