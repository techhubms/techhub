using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TechHub.Api.Services;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.Admin;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Api.Tests.Services;

/// <summary>
/// Unit tests for the cancellation handling of <see cref="TimelineBackfillBackgroundService"/>.
/// Synchronisation uses <see cref="TaskCompletionSource"/> signals raised from mock callbacks.
/// </summary>
public class TimelineBackfillBackgroundServiceTests
{
    private const long JobId = 7;

    [Fact]
    public void CancelCurrentRun_WhenNoRunInProgress_ReturnsFalse()
    {
        var sut = CreateSut(CreateJobRepo().Object, new Mock<ITimelineBackfillService>().Object);

        sut.CancelCurrentRun().Should().BeFalse();
    }

    [Fact]
    public async Task CancelCurrentRun_WhileJobCreationIsPending_DoesNotStopTheServiceAndTheNextRunWorks()
    {
        // Arrange: the first CreateAsync blocks until the run is cancelled, the second one succeeds
        var createStarted = NewSignal();
        var secondRunCompleted = NewSignal();
        var createCalls = 0;

        var jobRepo = new Mock<IContentProcessingJobRepository>();
        jobRepo.Setup(r => r.CreateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(async (string _, string _, CancellationToken ct) =>
            {
                if (Interlocked.Increment(ref createCalls) == 1)
                {
                    createStarted.TrySetResult();
                    await Task.Delay(Timeout.Infinite, ct);
                }

                return JobId;
            });
        jobRepo.Setup(r => r.CompleteAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                secondRunCompleted.TrySetResult();
                return Task.CompletedTask;
            });

        var backfill = new Mock<ITimelineBackfillService>();
        backfill.Setup(b => b.RunAsync(It.IsAny<TimelineBackfillRequest>(), It.IsAny<Func<string, Task>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TimelineBackfillResult());

        var sut = CreateSut(jobRepo.Object, backfill.Object);

        // Act
        await sut.StartAsync(TestContext.Current.CancellationToken);
        sut.TriggerImmediateRun(new TimelineBackfillRequest());
        await WaitAsync(createStarted);
        var cancelled = sut.CancelCurrentRun();

        // The service must survive the cancellation: a following run is still picked up
        TriggerWhenAccepted(sut);
        await WaitAsync(secondRunCompleted);

        // Assert
        cancelled.Should().BeTrue();
        sut.ExecuteTask.Should().NotBeNull();
        sut.ExecuteTask!.IsCompleted.Should().BeFalse("an admin cancellation must not terminate the background service");

        await sut.StopAsync(CancellationToken.None);
    }

    [Fact]
    public async Task CancelCurrentRun_RacingWithCompletion_NeverThrows()
    {
        // Arrange: runs that finish immediately while another thread keeps cancelling
        var runCompleted = new SemaphoreSlim(0);
        var jobRepo = CreateJobRepo();
        jobRepo.Setup(r => r.CompleteAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                runCompleted.Release();
                return Task.CompletedTask;
            });
        jobRepo.Setup(r => r.AbortJobAsync(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                runCompleted.Release();
                return Task.CompletedTask;
            });

        var backfill = new Mock<ITimelineBackfillService>();
        backfill.Setup(b => b.RunAsync(It.IsAny<TimelineBackfillRequest>(), It.IsAny<Func<string, Task>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TimelineBackfillResult());

        var sut = CreateSut(jobRepo.Object, backfill.Object);
        using var stopCancelling = new CancellationTokenSource();
        Exception? cancelFailure = null;

        var canceller = Task.Run(() =>
        {
            try
            {
                while (!stopCancelling.IsCancellationRequested)
                {
                    sut.CancelCurrentRun();
                }
            }
            catch (ObjectDisposedException ex)
            {
                cancelFailure = ex;
            }
        }, TestContext.Current.CancellationToken);

        // Act
        await sut.StartAsync(TestContext.Current.CancellationToken);
        for (var i = 0; i < 200; i++)
        {
            TriggerWhenAccepted(sut);
            (await runCompleted.WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken)).Should().BeTrue();
        }

        await stopCancelling.CancelAsync();
        await canceller;
        await sut.StopAsync(CancellationToken.None);

        // Assert
        cancelFailure.Should().BeNull();
    }

    private static void TriggerWhenAccepted(TimelineBackfillBackgroundService sut)
    {
        var deadline = DateTime.UtcNow.AddSeconds(10);
        while (!sut.TriggerImmediateRun(new TimelineBackfillRequest()))
        {
            DateTime.UtcNow.Should().BeBefore(deadline, "the previous trigger must be consumed by the service");
            Thread.Sleep(1);
        }
    }

    private static TaskCompletionSource NewSignal() => new(TaskCreationOptions.RunContinuationsAsynchronously);

    private static Task WaitAsync(TaskCompletionSource signal) =>
        signal.Task.WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken);

    private static Mock<IContentProcessingJobRepository> CreateJobRepo()
    {
        var jobRepo = new Mock<IContentProcessingJobRepository>();
        jobRepo.Setup(r => r.CreateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(JobId);
        return jobRepo;
    }

    private static TimelineBackfillBackgroundService CreateSut(IContentProcessingJobRepository jobRepo, ITimelineBackfillService backfill)
    {
        var startupState = new StartupStateService();
        startupState.MarkStartupCompleted();

        var services = new ServiceCollection();
        services.AddScoped(_ => jobRepo);
        services.AddScoped(_ => backfill);

        return new TimelineBackfillBackgroundService(
            services.BuildServiceProvider(),
            startupState,
            Mock.Of<ILogger<TimelineBackfillBackgroundService>>());
    }
}
