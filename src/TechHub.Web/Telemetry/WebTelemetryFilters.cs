namespace TechHub.Web.Telemetry;

/// <summary>
/// OpenTelemetry trace filter helpers specific to the Blazor frontend.
/// These are passed as an additional filter to <c>AddServiceDefaults</c> in Program.cs
/// and are not shared with the API, which has no Blazor and receives no bot traffic.
/// </summary>
internal static class WebTelemetryFilters
{
    /// <summary>
    /// Returns <c>true</c> for the Blazor SignalR disconnect endpoint.
    /// <c>/_blazor/disconnect</c> produces structurally expected 499s (client closed) and
    /// 400s (circuit already gone) during page unload. Real Blazor connectivity failures
    /// appear on <c>/_blazor/negotiate</c> or the SignalR WebSocket, not here.
    /// </summary>
    internal static bool IsBlazorDisconnectRequest(PathString path)
        => path.StartsWithSegments("/_blazor/disconnect", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Returns <c>true</c> if the User-Agent indicates a bot crawler.
    /// Bots following stale links generate expected 404s with no diagnostic value.
    /// </summary>
    internal static bool IsBotRequest(string userAgent)
        => !string.IsNullOrEmpty(userAgent)
           && userAgent.Contains("bot", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Returns <c>true</c> for requests whose path starts with <c>/api</c>.
    /// The web frontend never serves <c>/api/...</c> routes — these are scanners probing
    /// for a REST API on the same host. Suppressed here (not in <see cref="TechHub.Core.Security.ProbeDetector"/>
    /// which is shared with the API service that legitimately handles <c>/api/...</c> traffic).
    /// </summary>
    internal static bool IsApiProbeRequest(PathString path)
        => path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Combined filter for the Web OTel trace pipeline.
    /// Returns <c>false</c> (suppress trace) for Blazor disconnect, bot requests, and /api probes.
    /// </summary>
    internal static bool ShouldTrace(HttpContext httpContext)
        => !IsBlazorDisconnectRequest(httpContext.Request.Path)
           && !IsApiProbeRequest(httpContext.Request.Path)
           && !IsBotRequest(httpContext.Request.Headers.UserAgent.ToString());
}
