using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TechHub.Web.Services;

/// <summary>
/// Verifies Web can reach the API over the network by calling the API's DB-agnostic
/// <c>/alive</c> endpoint. Included in <c>/health</c> (not tagged "live") so a broken
/// Web→API network path (e.g. VNet/DNS/firewall misconfiguration) is surfaced instead
/// of silently masked by SectionCache/HeroBannerCache serving stale data indefinitely.
/// Deliberately checks <c>/alive</c> rather than <c>/health</c> — an API-side DB outage
/// is the API's own health concern and shouldn't also flip Web unhealthy.
/// </summary>
public class ApiHealthCheck(HttpClient httpClient, ILogger<ApiHealthCheck> logger) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await httpClient.GetAsync("/alive", cancellationToken);
            return response.IsSuccessStatusCode
                ? HealthCheckResult.Healthy()
                : HealthCheckResult.Unhealthy($"API returned {(int)response.StatusCode} {response.StatusCode}");
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            logger.LogWarning(ex, "API connectivity health check failed");
            return HealthCheckResult.Unhealthy("Unable to reach API", ex);
        }
    }
}
