using System.Diagnostics;
using OpenTelemetry;

namespace TechHub.Web.Telemetry;

/// <summary>
/// OpenTelemetry activity processor that suppresses <em>failed</em> Blazor Server hub method
/// activities from the trace export pipeline.
///
/// <para>
/// When the client has already disconnected (navigated away, closed the tab), the server still
/// attempts to push a component update, which fails because the circuit is gone. This produces
/// a <c>ComponentHub/UpdateRootComponents</c> activity with
/// <see cref="ActivityStatusCode.Error"/> — no HTTP status code, so Application Insights
/// records it as <c>success = false, resultCode = 0</c>. These are structurally expected and
/// have no diagnostic value; they are a normal side-effect of Blazor Server's server-driven
/// rendering model.
/// </para>
///
/// <para>
/// Successful hub invocations (circuit still connected, update delivered) are left untouched
/// so they remain visible in Application Insights for performance analysis.
/// </para>
///
/// <para>
/// This processor clears <see cref="ActivityTraceFlags.Recorded"/> in <c>OnEnd</c> only for
/// error-status activities, so the Azure Monitor <c>BatchExportProcessor</c> does not queue
/// them for export. Consistent with the approach in
/// <see cref="WebTelemetryFilters.SuppressIfClientError"/> for 404/405 HTTP responses.
/// </para>
/// </summary>
internal sealed class BlazorHubNoiseSuppressor : BaseProcessor<Activity>
{
    // All Blazor Server circuit hub invocations share this prefix in their display name.
    // Examples:
    //   Microsoft.AspNetCore.Components.Server.ComponentHub/UpdateRootComponents
    //   Microsoft.AspNetCore.Components.Server.ComponentHub/BeginInvokeDotNetFromJS
    private const string ComponentHubPrefix = "Microsoft.AspNetCore.Components.Server.ComponentHub";

    public override void OnEnd(Activity activity)
    {
        if (activity.DisplayName.StartsWith(ComponentHubPrefix, StringComparison.Ordinal)
            && activity.Status == ActivityStatusCode.Error)
        {
            activity.ActivityTraceFlags &= ~ActivityTraceFlags.Recorded;
        }
    }
}
