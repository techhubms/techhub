using Microsoft.Extensions.Diagnostics.HealthChecks;
using TechHub.Api.Services;

namespace TechHub.Api.HealthChecks;

/// <summary>
/// Health check that verifies all startup operations have completed.
/// Reports unhealthy until migrations and data seeding finish.
/// </summary>
public class StartupHealthCheck : IHealthCheck
{
    private readonly StartupStateService _startupState;

    public StartupHealthCheck(StartupStateService startupState)
    {
        _startupState = startupState ?? throw new ArgumentNullException(nameof(startupState));
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        if (!_startupState.IsStartupCompleted)
        {
            return Task.FromResult(
                HealthCheckResult.Unhealthy("Startup operations have not completed yet"));
        }

        return Task.FromResult(
            HealthCheckResult.Healthy("All startup operations completed successfully"));
    }
}
