using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Azure.Core;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Polly.CircuitBreaker;
using TechHub.Core.Interfaces;
using TechHub.Infrastructure.Services;

namespace TechHub.Api.Tests.Services;

public class AiCompletionResilienceTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private const int MinimumThroughput = 100;
    private const string StandardBody = """{"model":"test-model","messages":[]}""";
    private const string FlexBody = """{"model":"test-model","messages":[],"service_tier":"flex"}""";
    private const string OkResponse = """{"choices":[]}""";
    private readonly TechHubIntegrationTestApiFactory _factory;

    public AiCompletionResilienceTests(TechHubIntegrationTestApiFactory factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task SendCompletionAsync_SustainedRateLimiting_AllowsFlexRetriesAndStandardFallback(bool retryOnFlex)
    {
        // Arrange: exceed the default breaker throughput using the registered pipeline.
        var flexAttempts = 0;
        var standardAttempts = 0;
        using var handler = new StubHandler(body =>
        {
            using var request = JsonDocument.Parse(body);
            if (!request.RootElement.TryGetProperty("service_tier", out _))
            {
                return ++standardAttempts <= MinimumThroughput
                    ? new HttpResponseMessage(HttpStatusCode.TooManyRequests)
                    : new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(OkResponse) };
            }

            var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests);
            if (++flexAttempts > (retryOnFlex ? 1 : 0))
            {
                response.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromMinutes(1));
            }

            return response;
        });
        await using var factory = CreateFactory(handler);
        var client = factory.Services.GetRequiredService<IAiCompletionClient>();
        for (var i = 0; i < MinimumThroughput; i++)
        {
            var limited = await client.SendCompletionAsync(StandardBody, TestContext.Current.CancellationToken);
            limited.IsRateLimited.Should().BeTrue();
        }

        // Act
        var result = await client.SendCompletionAsync(FlexBody, TestContext.Current.CancellationToken);

        // Assert
        result.IsRateLimited.Should().BeFalse();
        result.ResponseBody.Should().Be(OkResponse);
        flexAttempts.Should().Be(retryOnFlex ? 2 : 1);
        standardAttempts.Should().Be(MinimumThroughput + 1);
    }

    [Theory]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.RequestTimeout)]
    [InlineData(null)]
    public async Task SendCompletionAsync_OtherTransientFailures_StillOpenCircuit(HttpStatusCode? status)
    {
        // Arrange
        var attempts = 0;
        using var handler = new StubHandler(_ =>
        {
            attempts++;
            return status.HasValue
                ? new HttpResponseMessage(status.Value)
                : throw new HttpRequestException("Simulated transport failure");
        });
        await using var factory = CreateFactory(handler);
        var client = factory.Services.GetRequiredService<IAiCompletionClient>();
        for (var i = 0; i < MinimumThroughput; i++)
        {
            var send = () => client.SendCompletionAsync(StandardBody, TestContext.Current.CancellationToken);
            await send.Should().ThrowAsync<HttpRequestException>();
        }

        // Act
        var blocked = () => client.SendCompletionAsync(StandardBody, TestContext.Current.CancellationToken);

        // Assert
        await blocked.Should().ThrowAsync<BrokenCircuitException>();
        attempts.Should().Be(MinimumThroughput);
    }

    private WebApplicationFactory<Program> CreateFactory(HttpMessageHandler handler)
    {
        return _factory.WithWebHostBuilder(builder =>
        {
            builder.UseSetting("AiCategorization:Endpoint", "https://ai.example.com/");
            builder.UseSetting("AiCategorization:FlexMaxWait", "00:00:10");
            builder.UseSetting("AiCategorization:FlexMinRetryDelay", "00:00:00.001");
            builder.UseSetting("AiCategorization:FlexMaxRetryDelay", "00:00:00.001");
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<TokenCredential>(new StubTokenCredential());
                services.AddHttpClient<IAiCompletionClient, AiCompletionClient>()
                    .ConfigurePrimaryHttpMessageHandler(() => handler);
            });
        });
    }

    private sealed class StubHandler : HttpMessageHandler
    {
        private readonly Func<string, HttpResponseMessage> _respond;

        public StubHandler(Func<string, HttpResponseMessage> respond)
        {
            _respond = respond;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var body = await request.Content!.ReadAsStringAsync(cancellationToken);
            return _respond(body);
        }
    }

    private sealed class StubTokenCredential : TokenCredential
    {
        public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
            => throw new NotSupportedException();

        public override ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
            => ValueTask.FromResult(new AccessToken("test-token", DateTimeOffset.MaxValue));
    }
}
