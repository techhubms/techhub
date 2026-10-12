using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace TechHub.Web.MCP;

/// <summary>
/// Custom telemetry for the MCP server. Protocol-level spans and metrics (method, tool name, duration)
/// are emitted by the MCP SDK itself; this adds content-level detail such as cache effectiveness per section.
/// </summary>
internal static class McpTelemetry
{
    public const string Name = "TechHub.Mcp";
    public const string RequestsMetricName = "techhub.mcp.content.requests";

    public static readonly ActivitySource ActivitySource = new(Name);

    private static readonly Meter _meter = new(Name);

    private static readonly Counter<long> _requests = _meter.CreateCounter<long>(
        RequestsMetricName,
        unit: "{request}",
        description: "Latest-content requests served by the MCP server, by section and cache result.");

    private static readonly Histogram<double> _duration = _meter.CreateHistogram<double>(
        "techhub.mcp.content.duration",
        unit: "ms",
        description: "Time spent producing a latest-content response, by section and cache result.");

    public static void RecordRequest(string sectionName, string result, TimeSpan elapsed)
    {
        var tags = new TagList
        {
            { "techhub.section", sectionName },
            { "techhub.result", result },
        };

        _requests.Add(1, tags);
        _duration.Record(elapsed.TotalMilliseconds, tags);
    }
}
