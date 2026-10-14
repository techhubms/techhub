using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TechHub.Api.Services;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.Admin;

namespace TechHub.Api.Tests.Services;

/// <summary>
/// Unit tests for <see cref="ContentFixerBackgroundService"/>.
/// The service is manual-trigger-only (no schedule, no enabled setting), so there is no enable/disable gating to test;
/// these tests cover the startup gate, the trigger path, job bookkeeping, cache invalidation and cancellation.
/// All synchronisation is done with <see cref="TaskCompletionSource"/> signals raised from mock callbacks.
/// </summary>
public class ContentFixerBackgroundServiceTests
{
    private const long JobId = 42;

    [Fact]
    public async Task TriggerImmediateRun_WhenItemsFixed_CompletesJobWithFixedCountAndInvalidatesCache()
    {
        // Arrange
        var startupState = CompletedStartupState();
        var jobRepo = CreateJobRepo();
        var fixer = new Mock<IContentFixerService>();
        fixer.Setup(f => f.FixAllAsync(It.IsAny<string?>(), It.IsAny<long?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateResult(tagsFixed: 3));

        // Cache invalidation is the last step of a successful run
        var runCompleted = NewSignal();
        var contentRepo = new Mock<IContentRepository>();
        contentRepo.Setup(r => r.InvalidateCachedData()).Callback(() => runCompleted.TrySetResult());

        var sut = CreateSut(startupState, jobRepo.Object, fixer.Object, contentRepo.Object);

        // Act
        await sut.StartAsync(TestContext.Current.CancellationToken);
        sut.TriggerImmediateRun();
        await WaitAsync(runCompleted);
        await sut.StopAsync(CancellationToken.None);

        // Assert
        jobRepo.Verify(r => r.CreateAsync("manual", ContentProcessingJobType.ContentCleanup, It.IsAny<CancellationToken>()), Times.Once);
        fixer.Verify(f => f.FixAllAsync(It.IsAny<string?>(), JobId, It.IsAny<CancellationToken>()), Times.Once);
        jobRepo.Verify(
            r => r.CompleteAsync(JobId, 0, 0, 0, 0, 0, 0, It.IsAny<string?>(), 3, It.IsAny<CancellationToken>()),
            Times.Once);
        contentRepo.Verify(r => r.InvalidateCachedData(), Times.Once);
    }

    [Fact]
    public async Task TriggerImmediateRun_WhenNothingFixed_CompletesJobWithoutInvalidatingCache()
    {
        // Arrange
        var startupState = CompletedStartupState();
        var firstRunCompleted = NewSignal();
        var secondRunCompleted = NewSignal();
        var completedRuns = 0;

        var jobRepo = CreateJobRepo();
        jobRepo.Setup(r => r.CompleteAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                (Interlocked.Increment(ref completedRuns) == 1 ? firstRunCompleted : secondRunCompleted).TrySetResult();
                return Task.CompletedTask;
            });
        var fixer = new Mock<IContentFixerService>();
        fixer.Setup(f => f.FixAllAsync(It.IsAny<string?>(), It.IsAny<long?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateResult(tagsFixed: 0));
        var contentRepo = new Mock<IContentRepository>();

        var sut = CreateSut(startupState, jobRepo.Object, fixer.Object, contentRepo.Object);

        // Act — the loop runs sequentially, so once the second run's job completes the first run is fully finished
        // (including the point where it would have invalidated the cache)
        await sut.StartAsync(TestContext.Current.CancellationToken);
        sut.TriggerImmediateRun();
        await WaitAsync(firstRunCompleted);
        sut.TriggerImmediateRun();
        await WaitAsync(secondRunCompleted);
        await sut.StopAsync(CancellationToken.None);

        // Assert
        jobRepo.Verify(
            r => r.CompleteAsync(JobId, 0, 0, 0, 0, 0, 0, It.IsAny<string?>(), 0, It.IsAny<CancellationToken>()),
            Times.Exactly(2));
        contentRepo.Verify(r => r.InvalidateCachedData(), Times.Never);
    }

    [Fact]
    public async Task TriggerImmediateRun_WhenFixerThrows_FailsJobAndKeepsProcessingTriggers()
    {
        // Arrange
        var startupState = CompletedStartupState();
        var jobFailed = NewSignal();
        var secondRunCompleted = NewSignal();

        var jobRepo = CreateJobRepo();
        jobRepo.Setup(r => r.FailAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                jobFailed.TrySetResult();
                return Task.CompletedTask;
            });
        jobRepo.Setup(r => r.CompleteAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                secondRunCompleted.TrySetResult();
                return Task.CompletedTask;
            });

        var fixer = new Mock<IContentFixerService>();
        fixer.SetupSequence(f => f.FixAllAsync(It.IsAny<string?>(), It.IsAny<long?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("boom"))
            .ReturnsAsync(CreateResult(tagsFixed: 0));

        var sut = CreateSut(startupState, jobRepo.Object, fixer.Object, new Mock<IContentRepository>().Object);

        // Act
        await sut.StartAsync(TestContext.Current.CancellationToken);
        sut.TriggerImmediateRun();
        await WaitAsync(jobFailed);
        sut.TriggerImmediateRun();
        await WaitAsync(secondRunCompleted);
        await sut.StopAsync(CancellationToken.None);

        // Assert — the failure is recorded on the job and does not stop the loop
        jobRepo.Verify(
            r => r.FailAsync(JobId, 0, 0, 0, 1, 0, 0, It.Is<string?>(s => s != null && s.Contains("boom", StringComparison.Ordinal)), 0, It.IsAny<CancellationToken>()),
            Times.Once);
        jobRepo.Verify(
            r => r.CompleteAsync(JobId, 0, 0, 0, 0, 0, 0, It.IsAny<string?>(), 0, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task TriggerImmediateRun_BeforeStartupCompletes_WaitsForStartup()
    {
        // Arrange — startup has not completed yet
        var startupState = new StartupStateService();
        var fixerStarted = NewSignal();
        var startupCompletedWhenFixerStarted = false;

        var fixer = new Mock<IContentFixerService>();
        fixer.Setup(f => f.FixAllAsync(It.IsAny<string?>(), It.IsAny<long?>(), It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                startupCompletedWhenFixerStarted = startupState.IsStartupCompleted;
                fixerStarted.TrySetResult();
                return Task.FromResult(CreateResult(tagsFixed: 0));
            });

        var sut = CreateSut(startupState, CreateJobRepo().Object, fixer.Object, new Mock<IContentRepository>().Object);

        // Act
        await sut.StartAsync(TestContext.Current.CancellationToken);
        sut.TriggerImmediateRun();
        startupState.MarkStartupCompleted();
        await WaitAsync(fixerStarted);
        await sut.StopAsync(CancellationToken.None);

        // Assert
        startupCompletedWhenFixerStarted.Should().BeTrue("the fixer must not run before startup (migrations) has completed");
    }

    [Fact]
    public void CancelCurrentRun_WhenNoRunInProgress_ReturnsFalse()
    {
        // Arrange
        var sut = CreateSut(
            new StartupStateService(),
            CreateJobRepo().Object,
            new Mock<IContentFixerService>().Object,
            new Mock<IContentRepository>().Object);

        // Act
        var result = sut.CancelCurrentRun();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task CancelCurrentRun_WhenRunInProgress_AbortsJobAndReturnsTrue()
    {
        // Arrange
        var startupState = CompletedStartupState();
        var runStarted = NewSignal();
        var jobAborted = NewSignal();

        var jobRepo = CreateJobRepo();
        jobRepo.Setup(r => r.AbortJobAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                jobAborted.TrySetResult();
                return Task.CompletedTask;
            });

        // The fixer blocks until the run is cancelled
        var fixer = new Mock<IContentFixerService>();
        fixer.Setup(f => f.FixAllAsync(It.IsAny<string?>(), It.IsAny<long?>(), It.IsAny<CancellationToken>()))
            .Returns(async (string? _, long? _, CancellationToken ct) =>
            {
                runStarted.TrySetResult();
                await Task.Delay(Timeout.Infinite, ct);
                return CreateResult(tagsFixed: 0);
            });

        var sut = CreateSut(startupState, jobRepo.Object, fixer.Object, new Mock<IContentRepository>().Object);

        // Act — the run's cancellation source exists before the fixer is invoked, so cancelling is safe once it started
        await sut.StartAsync(TestContext.Current.CancellationToken);
        sut.TriggerImmediateRun();
        await WaitAsync(runStarted);
        var cancelled = sut.CancelCurrentRun();
        await WaitAsync(jobAborted);
        await sut.StopAsync(CancellationToken.None);

        // Assert
        cancelled.Should().BeTrue();
        jobRepo.Verify(
            r => r.AbortJobAsync(JobId, 0, 0, 0, 0, 0, 0, It.IsAny<string?>(), 0, It.IsAny<CancellationToken>()),
            Times.Once);
        jobRepo.Verify(
            r => r.FailAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    private static StartupStateService CompletedStartupState()
    {
        var startupState = new StartupStateService();
        startupState.MarkStartupCompleted();
        return startupState;
    }

    private static TaskCompletionSource NewSignal() => new(TaskCreationOptions.RunContinuationsAsynchronously);

    private static Task WaitAsync(TaskCompletionSource signal) =>
        signal.Task.WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken);

    private static ContentFixerResult CreateResult(int tagsFixed) => new()
    {
        TagsScanned = 10,
        TagsFixed = tagsFixed,
        AuthorsFixed = 0,
        MarkdownScanned = 10,
        MarkdownFixed = 0,
        ValidationIssues = 0
    };

    private static Mock<IContentProcessingJobRepository> CreateJobRepo()
    {
        var jobRepo = new Mock<IContentProcessingJobRepository>();
        jobRepo.Setup(r => r.CreateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(JobId);
        return jobRepo;
    }

    private static ContentFixerBackgroundService CreateSut(
        StartupStateService startupState,
        IContentProcessingJobRepository jobRepo,
        IContentFixerService fixer,
        IContentRepository contentRepo)
    {
        var services = new ServiceCollection();
        services.AddScoped(_ => jobRepo);
        services.AddScoped(_ => fixer);
        services.AddScoped(_ => contentRepo);

        return new ContentFixerBackgroundService(
            services.BuildServiceProvider(),
            startupState,
            Mock.Of<ILogger<ContentFixerBackgroundService>>());
    }
}
