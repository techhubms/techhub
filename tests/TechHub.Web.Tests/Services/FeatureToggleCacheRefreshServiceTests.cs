using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TechHub.Core.Models.FeatureToggles;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Services;

public class FeatureToggleCacheRefreshServiceTests
{
    [Fact]
    public async Task ExecuteAsync_MalformedJsonResponse_LogsAndKeepsCachedToggles()
    {
        var api = new Mock<ITechHubApiClient>();
        api.Setup(client => client.GetFeatureTogglesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new JsonException("Malformed JSON"));
        using var provider = new ServiceCollection()
            .AddSingleton(api.Object)
            .BuildServiceProvider();
        var cache = new FeatureToggleCache();
        cache.Initialize([new FeatureToggle
        {
            Key = "timeline",
            Mode = FeatureToggleMode.Everyone,
            Description = "Timeline"
        }]);
        var logger = new CapturingLogger<FeatureToggleCacheRefreshService>();
        var service = new FeatureToggleCacheRefreshService(
            provider.GetRequiredService<IServiceScopeFactory>(),
            cache,
            logger);

        await service.StartAsync(TestContext.Current.CancellationToken);
        var exception = await logger.WarningLogged.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);

        exception.Should().BeOfType<JsonException>();
        cache.GetMode("timeline").Should().Be(FeatureToggleMode.Everyone);
        service.ExecuteTask.Should().NotBeNull();
        service.ExecuteTask!.IsCompleted.Should().BeFalse();
        api.Verify(client => client.GetFeatureTogglesAsync(It.IsAny<CancellationToken>()), Times.Once);

        await service.StopAsync(TestContext.Current.CancellationToken);
    }

    private sealed class CapturingLogger<T> : ILogger<T>
    {
        private readonly TaskCompletionSource<Exception?> _warningLogged = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public Task<Exception?> WarningLogged => _warningLogged.Task;

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (logLevel == LogLevel.Warning)
            {
                _warningLogged.TrySetResult(exception);
            }
        }
    }
}
