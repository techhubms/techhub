using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Api.Services;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Services.ContentProcessing;

namespace TechHub.Api.Tests.Services;

public class ContentProcessingBackgroundServiceTests
{
    [Fact]
    public async Task TriggerImmediateRun_WhenDisabled_ExecutesManualRun()
    {
        // Arrange — disabled mode, so the service loops waiting for manual triggers
        var startupState = new StartupStateService();
        startupState.MarkStartupCompleted(); // bypass startup wait

        var mockJobRepo = new Mock<IContentProcessingJobRepository>();
        mockJobRepo.Setup(r => r.CreateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1L);

        var mockJobSettingRepo = new Mock<IBackgroundJobSettingRepository>();
        mockJobSettingRepo.Setup(r => r.IsEnabledAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var services = new ServiceCollection();
        services.AddScoped(_ => CreateMockProcessingService(mockJobRepo.Object));
        services.AddScoped(_ => mockJobSettingRepo.Object);
        services.AddScoped(_ => Mock.Of<IContentRepository>());

        var serviceProvider = services.BuildServiceProvider();

        var options = new ContentProcessorOptions { BrowserUserAgent = "TestAgent/1.0", IntervalMinutes = 60 };

        var sut = new ContentProcessingBackgroundService(
            serviceProvider,
            Options.Create(options),
            startupState,
            Mock.Of<ILogger<ContentProcessingBackgroundService>>());

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        // Act — start the service and trigger a manual run
        await sut.StartAsync(cts.Token);
        await Task.Delay(200, cts.Token);
        sut.TriggerImmediateRun();
        await Task.Delay(500, cts.Token);
        await sut.StopAsync(CancellationToken.None);

        // Assert — CreateAsync should have been called with "manual" trigger
        mockJobRepo.Verify(
            r => r.CreateAsync("manual", It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task TriggerImmediateRun_WhenEnabled_ExecutesManualRun()
    {
        // Arrange — enabled mode, service runs on schedule + manual triggers
        var startupState = new StartupStateService();
        startupState.MarkStartupCompleted();

        var mockJobRepo = new Mock<IContentProcessingJobRepository>();
        mockJobRepo.Setup(r => r.CreateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1L);

        var mockJobSettingRepo = new Mock<IBackgroundJobSettingRepository>();
        mockJobSettingRepo.Setup(r => r.IsEnabledAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var services = new ServiceCollection();
        services.AddScoped(_ => CreateMockProcessingService(mockJobRepo.Object));
        services.AddScoped(_ => mockJobSettingRepo.Object);
        services.AddScoped(_ => Mock.Of<IContentRepository>());

        var serviceProvider = services.BuildServiceProvider();

        var options = new ContentProcessorOptions { BrowserUserAgent = "TestAgent/1.0", IntervalMinutes = 60 };

        var sut = new ContentProcessingBackgroundService(
            serviceProvider,
            Options.Create(options),
            startupState,
            Mock.Of<ILogger<ContentProcessingBackgroundService>>());

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        // Act
        await sut.StartAsync(cts.Token);
        // Wait for the initial scheduled run to finish
        await Task.Delay(500, cts.Token);
        sut.TriggerImmediateRun();
        await Task.Delay(500, cts.Token);
        await sut.StopAsync(CancellationToken.None);

        // Assert — one scheduled + one manual
        mockJobRepo.Verify(
            r => r.CreateAsync("scheduled", It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
        mockJobRepo.Verify(
            r => r.CreateAsync("manual", It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void CancelCurrentRun_WhenNoRunInProgress_ReturnsFalse()
    {
        // Arrange
        var startupState = new StartupStateService();
        var services = new ServiceCollection().BuildServiceProvider();
        var sut = new ContentProcessingBackgroundService(
            services,
            Options.Create(new ContentProcessorOptions { BrowserUserAgent = "TestAgent/1.0" }),
            startupState,
            Mock.Of<ILogger<ContentProcessingBackgroundService>>());

        // Act
        var result = sut.CancelCurrentRun();

        // Assert — no run in progress, so nothing to cancel
        result.Should().BeFalse();
    }

    [Fact]
    public async Task CancelCurrentRun_WhenRunInProgress_CancelsAndReturnsTrue()
    {
        // Arrange — disabled mode so it only runs on manual trigger
        var startupState = new StartupStateService();
        startupState.MarkStartupCompleted();

        var runStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        var mockJobRepo = new Mock<IContentProcessingJobRepository>();
        mockJobRepo.Setup(r => r.CreateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1L);
        mockJobRepo.Setup(r => r.AbortJobAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var mockJobSettingRepo = new Mock<IBackgroundJobSettingRepository>();
        mockJobSettingRepo.Setup(r => r.IsEnabledAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Create a processing service that blocks until cancelled
        var feedRepo = new Mock<IRssFeedConfigRepository>();
        feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>()))
            .Returns(async (CancellationToken ct) =>
            {
                runStarted.TrySetResult();
                await Task.Delay(Timeout.Infinite, ct);
                return (IReadOnlyList<FeedConfig>)[];
            });

        var services = new ServiceCollection();
        services.AddScoped(_ => new ContentProcessingService(
            Mock.Of<IRssFeedIngestionService>(),
            Mock.Of<IArticleContentService>(),
            Mock.Of<IAiCategorizationService>(),
            Mock.Of<IYouTubeTagService>(),
            Mock.Of<IContentItemWriteRepository>(),
            mockJobRepo.Object,
            Mock.Of<IProcessedUrlRepository>(),
            feedRepo.Object,
            Mock.Of<IContentFixerService>(),
            TimeProvider.System,
            Options.Create(new ContentProcessorOptions { BrowserUserAgent = "TestAgent/1.0" }),
            Mock.Of<ILogger<ContentProcessingService>>()));
        services.AddScoped(_ => mockJobSettingRepo.Object);
        services.AddScoped(_ => Mock.Of<IContentRepository>());

        var serviceProvider = services.BuildServiceProvider();
        var options = new ContentProcessorOptions { BrowserUserAgent = "TestAgent/1.0", IntervalMinutes = 60 };

        var sut = new ContentProcessingBackgroundService(
            serviceProvider,
            Options.Create(options),
            startupState,
            Mock.Of<ILogger<ContentProcessingBackgroundService>>());

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        // Act — start the service, trigger a run, wait for it to start, then cancel
        await sut.StartAsync(cts.Token);
        await Task.Delay(200, cts.Token);
        sut.TriggerImmediateRun();

        // Wait for the run to actually start (blocks on GetEnabledAsync)
        await runStarted.Task.WaitAsync(cts.Token);
        await Task.Delay(100, cts.Token);

        var cancelled = sut.CancelCurrentRun();
        await Task.Delay(500, cts.Token);
        await sut.StopAsync(CancellationToken.None);

        // Assert
        cancelled.Should().BeTrue();
        mockJobRepo.Verify(
            r => r.AbortJobAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task RunOnceAsync_InvalidatesContentCache_AfterSuccessfulRun()
    {
        // Arrange
        var startupState = new StartupStateService();
        startupState.MarkStartupCompleted();

        var mockJobRepo = new Mock<IContentProcessingJobRepository>();
        mockJobRepo.Setup(r => r.CreateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1L);

        var mockJobSettingRepo = new Mock<IBackgroundJobSettingRepository>();
        mockJobSettingRepo.Setup(r => r.IsEnabledAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var mockContentRepo = new Mock<IContentRepository>();

        var services = new ServiceCollection();
        services.AddScoped(_ => CreateMockProcessingService(mockJobRepo.Object));
        services.AddScoped(_ => mockJobSettingRepo.Object);
        services.AddScoped(_ => mockContentRepo.Object);

        var serviceProvider = services.BuildServiceProvider();
        var options = new ContentProcessorOptions { BrowserUserAgent = "TestAgent/1.0", IntervalMinutes = 60 };

        var sut = new ContentProcessingBackgroundService(
            serviceProvider,
            Options.Create(options),
            startupState,
            Mock.Of<ILogger<ContentProcessingBackgroundService>>());

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        // Act — trigger a manual run and wait for it to complete
        await sut.StartAsync(cts.Token);
        await Task.Delay(200, cts.Token);
        sut.TriggerImmediateRun();
        await Task.Delay(500, cts.Token);
        await sut.StopAsync(CancellationToken.None);

        // Assert — cache should have been invalidated after the run
        mockContentRepo.Verify(r => r.InvalidateCachedData(), Times.Once);
    }

    /// <summary>
    /// Creates a <see cref="ContentProcessingService"/> with all dependencies mocked,
    /// using the provided job repository mock to verify calls.
    /// </summary>
    private static ContentProcessingService CreateMockProcessingService(
        IContentProcessingJobRepository jobRepo)
    {
        var feedRepo = new Mock<IRssFeedConfigRepository>();
        feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        return new ContentProcessingService(
            Mock.Of<IRssFeedIngestionService>(),
            Mock.Of<IArticleContentService>(),
            Mock.Of<IAiCategorizationService>(),
            Mock.Of<IYouTubeTagService>(),
            Mock.Of<IContentItemWriteRepository>(),
            jobRepo,
            Mock.Of<IProcessedUrlRepository>(),
            feedRepo.Object,
            Mock.Of<IContentFixerService>(),
            TimeProvider.System,
            Options.Create(new ContentProcessorOptions { BrowserUserAgent = "TestAgent/1.0" }),
            Mock.Of<ILogger<ContentProcessingService>>());
    }
}
