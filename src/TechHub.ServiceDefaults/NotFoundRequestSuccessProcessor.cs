using System.Diagnostics;
using OpenTelemetry;

namespace TechHub.ServiceDefaults;

/// <summary>
/// OpenTelemetry processor that marks HTTP 404 responses as successful spans.
/// </summary>
/// <remarks>
/// By default, Azure Monitor treats any HTTP 4xx response as a failure (Success=false in AppRequests).
/// A 404 Not Found is semantically correct server behavior — the server correctly determined the
/// resource does not exist — not an application error. Marking 404s as failures inflates the
/// failure rate and triggers false-positive alerts when bots or users follow stale links.
///
/// This processor overrides the span status to <see cref="ActivityStatusCode.Ok"/> for 404 responses,
/// so Azure Monitor records Success=true. The ResultCode still shows 404 for filtering and analysis.
/// </remarks>
public sealed class NotFoundRequestSuccessProcessor : BaseProcessor<Activity>
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

        // http.response.status_code is the stable OTel semantic convention attribute (integer)
        // set by OpenTelemetry.Instrumentation.AspNetCore for all HTTP server spans.
        if (activity.GetTagItem("http.response.status_code") is int statusCode && statusCode == 404)
        {
            // Setting Ok explicitly overrides Azure Monitor's default "any 4xx = failure" logic.
            // ResultCode in AppRequests will still be "404" — only the Success flag changes.
            activity.SetStatus(ActivityStatusCode.Ok);
        }
    }
}
