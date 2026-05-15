using System.Diagnostics;

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
    /// Returns <c>true</c> for Blazor circuit reconnection attempts (<c>/_blazor?id=...</c>).
    /// These occur when a client tries to resume a server-side Blazor circuit that no longer
    /// exists (e.g. after a container restart). The requests always return 404 and have no
    /// diagnostic value; filtering them prevents normal replica-restart churn from inflating
    /// the <c>requests/failed</c> metric.
    /// </summary>
    internal static bool IsBlazorCircuitReconnectRequest(PathString path, IQueryCollection query)
        => path.Equals("/_blazor", StringComparison.OrdinalIgnoreCase)
           && query.ContainsKey("id");

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
           && !IsBlazorCircuitReconnectRequest(httpContext.Request.Path, httpContext.Request.Query)
           && !IsApiProbeRequest(httpContext.Request.Path)
           && !IsBotRequest(httpContext.Request.Headers.UserAgent.ToString());

    /// <summary>
    /// Response enricher that suppresses structural-noise 4xx activities from the trace export
    /// pipeline by clearing <see cref="ActivityTraceFlags.Recorded"/>.
    /// Called from <c>EnrichWithHttpResponse</c> — after the response is written but before
    /// the Activity is stopped — so the flag is cleared before the Azure Monitor
    /// <c>BatchExportProcessor.OnEnd</c> checks it and decides whether to queue for export.
    /// <para>
    /// Only 404 (page not found) and 405 (method not allowed) are suppressed — both are
    /// generated exclusively by bots, scanners, and stale links, not by real application errors.
    /// Other 4xx codes are retained: 429 (rate limit) indicates a scraping or DDoS attack
    /// worth alerting on; 401/403 can reveal real authorization bugs.
    /// HTTP 5xx responses (genuine server errors) are always left untouched.
    /// </para>
    /// </summary>
    internal static void SuppressIfClientError(Activity activity, HttpResponse response)
    {
        if (response.StatusCode is 404 or 405)
        {
            // Clearing the Recorded flag prevents the Azure Monitor BatchExportProcessor
            // from adding this activity to its export queue when its OnEnd fires.
            activity.ActivityTraceFlags &= ~ActivityTraceFlags.Recorded;
        }
    }
}
