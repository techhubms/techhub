using System.Diagnostics;
using OpenTelemetry;

namespace TechHub.ServiceDefaults;

/// <summary>
/// OpenTelemetry processor that marks expected, non-actionable requests as successful spans
/// so they do not inflate the failure rate or trigger false-positive Azure Monitor alerts.
/// </summary>
/// <remarks>
/// Crawlers (AhrefsBot, Googlebot, bingbot, ClaudeBot, OAI-SearchBot, FacebookBot, PetalBot, etc.)
/// follow stale links and generate expected 404s. Any user agent whose string contains "bot"
/// (case-insensitive) is treated as benign crawler traffic, not an application error.
/// The <c>ResultCode</c> in AppRequests retains the original HTTP status for filtering.
/// Only the <c>Success</c> flag is changed to suppress alert noise.
/// </remarks>
internal sealed class TelemetryNoiseProcessor : BaseProcessor<Activity>
{
    /// <inheritdoc />
    public override void OnEnd(Activity activity)
    {
        ArgumentNullException.ThrowIfNull(activity);

        // Only process server-side HTTP spans (inbound requests, not outbound HTTP client calls)
        if (activity.Kind != ActivityKind.Server)
        {
            return;
        }

        // Bot user agents following stale links — 404s are expected, not actionable failures.
        if (activity.GetTagItem("user_agent.original") is string userAgent
            && userAgent.Contains("bot", StringComparison.OrdinalIgnoreCase))
        {
            activity.SetStatus(ActivityStatusCode.Ok);
            return;
        }
    }
}
