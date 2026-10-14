using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TechHub.Core.Models;
using TechHub.TestUtilities.Builders;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Services;

/// <summary>
/// Tests for <see cref="SectionCacheHealthCheck"/> — readiness signal based on <see cref="SectionCache.IsReady"/>.
/// </summary>
public class SectionCacheHealthCheckTests
{
    [Fact]
    public async Task CheckHealthAsync_CacheNeverPopulated_ReturnsUnhealthyWithExplanation()
    {
        // Arrange
        var sut = new SectionCacheHealthCheck(SectionCacheBuilder.Empty());

        // Act
        var result = await sut.CheckHealthAsync(new HealthCheckContext(), TestContext.Current.CancellationToken);

        // Assert
        result.Status.Should().Be(HealthStatus.Unhealthy);
        result.Description.Should().Be("SectionCache has not been populated from the API yet.");
    }

    [Fact]
    public async Task CheckHealthAsync_CachePopulated_ReturnsHealthy()
    {
        // Arrange
        var sut = new SectionCacheHealthCheck(A.SectionCache.WithSections("ai").Build());

        // Act
        var result = await sut.CheckHealthAsync(new HealthCheckContext(), TestContext.Current.CancellationToken);

        // Assert
        result.Status.Should().Be(HealthStatus.Healthy);
        result.Description.Should().BeNull();
    }

    [Fact]
    public async Task CheckHealthAsync_CacheInitializedWithNoSections_ReturnsUnhealthy()
    {
        // Arrange
        var cache = new SectionCache();
        cache.Initialize([]);
        var sut = new SectionCacheHealthCheck(cache);

        // Act
        var result = await sut.CheckHealthAsync(new HealthCheckContext(), TestContext.Current.CancellationToken);

        // Assert
        result.Status.Should().Be(HealthStatus.Unhealthy);
    }

    [Fact]
    public async Task CheckHealthAsync_CachePopulatedAfterFirstCheck_TurnsHealthy()
    {
        // Arrange
        var cache = new SectionCache();
        var sut = new SectionCacheHealthCheck(cache);
        var before = await sut.CheckHealthAsync(new HealthCheckContext(), TestContext.Current.CancellationToken);
        var populated = A.SectionCache.WithSections("ai").Build();
        cache.Initialize(populated.Sections);

        // Act
        var after = await sut.CheckHealthAsync(new HealthCheckContext(), TestContext.Current.CancellationToken);

        // Assert
        before.Status.Should().Be(HealthStatus.Unhealthy);
        after.Status.Should().Be(HealthStatus.Healthy);
    }

    [Fact]
    public async Task CheckHealthAsync_CancelledToken_StillReturnsResultWithoutThrowing()
    {
        // Arrange
        var sut = new SectionCacheHealthCheck(SectionCacheBuilder.Empty());
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        // Act
        var result = await sut.CheckHealthAsync(new HealthCheckContext(), cts.Token);

        // Assert
        result.Status.Should().Be(HealthStatus.Unhealthy);
    }
}
