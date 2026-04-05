using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Api.Services;
using TechHub.Core.Configuration;

namespace TechHub.Api.Tests.Services;

/// <summary>
/// Unit tests for <see cref="RoundupGeneratorBackgroundService"/>.
/// Validates the scheduling logic that computes the delay until the next Monday run.
/// </summary>
public class RoundupGeneratorBackgroundServiceTests
{
    private RoundupGeneratorBackgroundService CreateSut(int runHourUtc = 8) =>
        new(
            Mock.Of<IServiceProvider>(),
            Options.Create(new RoundupGeneratorOptions { RunHourUtc = runHourUtc }),
            new StartupStateService(),
            NullLogger<RoundupGeneratorBackgroundService>.Instance);

    // ── ComputeDelayUntilNextRun ──────────────────────────────────────────────

    [Fact]
    public void ComputeDelayUntilNextRun_OnMondayBeforeRunHour_ReturnsDelayUntilSameDay()
    {
        // Arrange — Monday at 6:00 AM UTC, run hour is 8:00 AM
        var nowUtc = new DateTime(2025, 3, 24, 6, 0, 0, DateTimeKind.Utc); // Monday

        var sut = CreateSut(runHourUtc: 8);

        // Act
        var delay = sut.ComputeDelayUntilNextRun(nowUtc);

        // Assert — should fire in 2 hours (same day)
        delay.Should().BeCloseTo(TimeSpan.FromHours(2), TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void ComputeDelayUntilNextRun_OnMondayAfterRunHour_ReturnsDelayUntilNextMonday()
    {
        // Arrange — Monday at 10:00 AM UTC, run hour already passed
        var nowUtc = new DateTime(2025, 3, 24, 10, 0, 0, DateTimeKind.Utc); // Monday

        var sut = CreateSut(runHourUtc: 8);

        // Act
        var delay = sut.ComputeDelayUntilNextRun(nowUtc);

        // Assert — should be approximately 7 days - 2 hours = 166 hours
        delay.Should().BeCloseTo(TimeSpan.FromHours(166), TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void ComputeDelayUntilNextRun_OnTuesday_ReturnsDelayUntilNextMonday()
    {
        // Arrange — Tuesday at 12:00 PM UTC
        var nowUtc = new DateTime(2025, 3, 25, 12, 0, 0, DateTimeKind.Utc); // Tuesday

        var sut = CreateSut(runHourUtc: 8);

        // Act
        var delay = sut.ComputeDelayUntilNextRun(nowUtc);

        // Assert — 6 days minus 4 hours = 140 hours
        delay.Should().BeCloseTo(TimeSpan.FromHours(140), TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void ComputeDelayUntilNextRun_OnSunday_ReturnsDelayUntilTomorrow()
    {
        // Arrange — Sunday at 6:00 AM UTC
        var nowUtc = new DateTime(2025, 3, 30, 6, 0, 0, DateTimeKind.Utc); // Sunday

        var sut = CreateSut(runHourUtc: 8);

        // Act
        var delay = sut.ComputeDelayUntilNextRun(nowUtc);

        // Assert — 1 day + 2 hours = 26 hours
        delay.Should().BeCloseTo(TimeSpan.FromHours(26), TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void ComputeDelayUntilNextRun_NeverReturnsNegativeDelay()
    {
        // Arrange — even if something is slightly off, delay should be >= 0
        var nowUtc = new DateTime(2025, 3, 24, 8, 0, 0, DateTimeKind.Utc); // Monday exactly at run time

        var sut = CreateSut(runHourUtc: 8);

        // Act
        var delay = sut.ComputeDelayUntilNextRun(nowUtc);

        // Assert — no negative delays
        delay.Should().BeGreaterThanOrEqualTo(TimeSpan.Zero);
    }

    [Fact]
    public void ComputeDelayUntilNextRun_RespectsConfiguredRunHour()
    {
        // Arrange — Monday at midnight UTC, run hour is 6
        var nowUtc = new DateTime(2025, 3, 24, 0, 0, 0, DateTimeKind.Utc); // Monday midnight

        var sut = CreateSut(runHourUtc: 6);

        // Act
        var delay = sut.ComputeDelayUntilNextRun(nowUtc);

        // Assert — should be 6 hours
        delay.Should().BeCloseTo(TimeSpan.FromHours(6), TimeSpan.FromSeconds(1));
    }
}
