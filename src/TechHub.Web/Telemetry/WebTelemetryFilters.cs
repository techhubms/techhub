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
    /// Combined filter for the Web OTel trace pipeline.
    /// Returns <c>false</c> (suppress trace) for Blazor disconnect and bot requests.
    /// </summary>
    internal static bool ShouldTrace(HttpContext httpContext)
        => !IsBlazorDisconnectRequest(httpContext.Request.Path)
           && !IsBotRequest(httpContext.Request.Headers.UserAgent.ToString());
}
