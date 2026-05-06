using Microsoft.Extensions.Diagnostics.HealthChecks;
using TechHub.Core.Models;

namespace TechHub.Web.Services;

/// <summary>
/// Readiness health check for <see cref="SectionCache"/>.
/// Reports <see cref="HealthCheckResult.Unhealthy"/> until the cache has been populated
/// from the API at least once. Container Apps uses <c>/health</c> as its readiness probe,
/// so a cold instance will not receive traffic until this check passes.
/// </summary>
public class SectionCacheHealthCheck(SectionCache sectionCache) : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(sectionCache.IsReady
            ? HealthCheckResult.Healthy()
            : HealthCheckResult.Unhealthy("SectionCache has not been populated from the API yet."));
    }
}
