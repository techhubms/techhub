using Microsoft.Playwright;

namespace TechHub.E2E.Tests.Helpers;

/// <summary>
/// CDP-based network throttling for Playwright Chromium tests.
/// Used to simulate slower CI runner network conditions when running E2E tests locally.
///
/// Activate via environment variable:
///   E2E_NETWORK_THROTTLE=fast3g     → Fast 3G (562 Kbps down, 150ms latency)
///   E2E_NETWORK_THROTTLE=slow3g     → Slow 3G (400 Kbps down, 400ms latency)
///   E2E_NETWORK_THROTTLE=regular4g  → Regular 4G (4 Mbps down, 20ms latency)
///   E2E_NETWORK_THROTTLE=ci         → Simulates CI runner conditions (CPU throttle only, no network throttle)
///   E2E_NETWORK_THROTTLE=wan        → Simulates remote deployment latency (150ms latency, unlimited bandwidth)
///                                     Reproduces PR preview / staging E2E conditions where the GitHub runner
///                                     targets a remote Azure Container App over WAN. SignalR WebSocket
///                                     round-trips and JS interop calls each pay this latency cost.
///
/// The "ci" profile also applies CPU throttling to better simulate resource-constrained CI runners.
/// The "wan" profile is the closest simulation of PR preview E2E conditions.
/// </summary>
public static class NetworkThrottling
{
    private const string EnvVar = "E2E_NETWORK_THROTTLE";

    /// <summary>
    /// Gets the throttle profile name from the environment variable, or null if not set.
    /// </summary>
    public static string? GetConfiguredProfile() =>
        Environment.GetEnvironmentVariable(EnvVar);

    /// <summary>
    /// Applies network throttling to the page if the E2E_NETWORK_THROTTLE environment variable is set.
    /// Returns the CDP session (caller must dispose), or null if no throttling was applied.
    /// </summary>
    public static async Task<ICDPSession?> ApplyIfConfiguredAsync(IPage page)
    {
        var profileName = GetConfiguredProfile();
        if (string.IsNullOrEmpty(profileName))
        {
            return null;
        }

        var profile = GetProfile(profileName);
        if (profile == null)
        {
            return null;
        }

        var cdpSession = await page.Context.NewCDPSessionAsync(page);

        if (profile.DownloadThroughputBytesPerSecond >= 0)
        {
            await cdpSession.SendAsync("Network.emulateNetworkConditions", new Dictionary<string, object>
            {
                ["offline"] = false,
                ["downloadThroughput"] = profile.DownloadThroughputBytesPerSecond,
                ["uploadThroughput"] = profile.UploadThroughputBytesPerSecond,
                ["latency"] = profile.LatencyMs,
            });
        }

        if (profile.CpuThrottleRate > 1)
        {
            await cdpSession.SendAsync("Emulation.setCPUThrottlingRate", new Dictionary<string, object>
            {
                ["rate"] = profile.CpuThrottleRate,
            });
        }

        return cdpSession;
    }

    private static ThrottleProfile? GetProfile(string name) => name.ToLowerInvariant() switch
    {
        "slow3g" => new ThrottleProfile(
            DownloadThroughputBytesPerSecond: 400 * 1024 / 8,   // 400 Kbps
            UploadThroughputBytesPerSecond: 400 * 1024 / 8,     // 400 Kbps
            LatencyMs: 400,
            CpuThrottleRate: 1),
        "fast3g" => new ThrottleProfile(
            DownloadThroughputBytesPerSecond: 562 * 1024 / 8,   // 562 Kbps
            UploadThroughputBytesPerSecond: 562 * 1024 / 8,     // 562 Kbps
            LatencyMs: 150,
            CpuThrottleRate: 1),
        "regular4g" => new ThrottleProfile(
            DownloadThroughputBytesPerSecond: 4 * 1024 * 1024 / 8, // 4 Mbps
            UploadThroughputBytesPerSecond: 3 * 1024 * 1024 / 8,   // 3 Mbps
            LatencyMs: 20,
            CpuThrottleRate: 1),
        "ci" => new ThrottleProfile(
            DownloadThroughputBytesPerSecond: -1,                       // No network throttle (CI is localhost)
            UploadThroughputBytesPerSecond: -1,                        // No network throttle
            LatencyMs: 0,
            CpuThrottleRate: 2),                                       // 2x CPU slowdown only
        "wan" => new ThrottleProfile(
            DownloadThroughputBytesPerSecond: -1,                       // No bandwidth limit
            UploadThroughputBytesPerSecond: -1,                        // No bandwidth limit
            LatencyMs: 150,                                            // 150ms simulates GitHub runner → remote Azure Container App
            CpuThrottleRate: 1),                                       // No CPU throttle (CI runners are fast)
        _ => null,
    };

    private sealed record ThrottleProfile(
        int DownloadThroughputBytesPerSecond,
        int UploadThroughputBytesPerSecond,
        int LatencyMs,
        int CpuThrottleRate);
}
