using Polly;
using Polly.Retry;

namespace TechHub.Infrastructure.Resilience;

/// <summary>
/// Polly resilience policies for HTTP clients and external dependencies
/// </summary>
public static class PollyPolicies
{
    /// <summary>
    /// Retry policy for HTTP requests with exponential backoff
    /// Retries 3 times with 2s, 4s, 8s delays
    /// </summary>
    public static ResiliencePipeline<HttpResponseMessage> GetHttpRetryPolicy()
    {
        return new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddRetry(new RetryStrategyOptions<HttpResponseMessage>
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(2),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .HandleResult(response => !response.IsSuccessStatusCode)
                    .Handle<HttpRequestException>()
                    .Handle<TimeoutException>()
            })
            .AddTimeout(TimeSpan.FromSeconds(30))
            .Build();
    }

    /// <summary>
    /// Retry policy for file operations with linear backoff
    /// Retries 2 times with 1s delays
    /// </summary>
    public static ResiliencePipeline GetFileRetryPolicy()
    {
        return new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 2,
                Delay = TimeSpan.FromSeconds(1),
                BackoffType = DelayBackoffType.Linear,
                UseJitter = false,
                ShouldHandle = new PredicateBuilder()
                    .Handle<IOException>()
                    .Handle<UnauthorizedAccessException>()
            })
            .Build();
    }

    /// <summary>
    /// Retry policy for cache operations (fail fast)
    /// Single retry with minimal delay
    /// </summary>
    public static ResiliencePipeline GetCacheRetryPolicy()
    {
        return new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 1,
                Delay = TimeSpan.FromMilliseconds(100),
                BackoffType = DelayBackoffType.Constant,
                UseJitter = false,
                ShouldHandle = new PredicateBuilder()
                    .Handle<InvalidOperationException>()
            })
            .Build();
    }
}
