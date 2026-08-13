using Microsoft.Extensions.Diagnostics.HealthChecks;
using TechHub.Core.Models;

namespace TechHub.Web.Services;

/// <summary>
/// Readiness health check for <see cref="SectionCache"/>.
/// Reports <see cref="HealthCheckResult.Unhealthy"/> until the cache has been populated
/// from the API at least once. App Service uses <c>/health</c> as its health-check probe,
/// but on this single-instance Basic plan it never removes the instance from rotation for
/// a failing probe — it only replaces the instance after a full continuous hour of
/// failures (see docs/health-checks.md). This check mainly surfaces a monitoring signal.
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
