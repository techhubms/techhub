using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace TechHub.ServiceDefaults;

/// <summary>
/// Extension methods for configuring common .NET Aspire service defaults.
/// </summary>
public static class ServiceDefaultsExtensions
{
    private static readonly string[] _liveHealthCheckTags = ["live"];
    /// <summary>
    /// Adds service defaults for Aspire orchestration including:
    /// - OpenTelemetry (logging, metrics, tracing)
    /// - Service discovery
    /// - HTTP client resilience
    /// - Health checks
    /// </summary>
    public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ConfigureOpenTelemetry();

        builder.AddDefaultHealthChecks();

        builder.Services.AddServiceDiscovery();

        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            // Turn on service discovery by default
            http.AddServiceDiscovery();

            // NOTE: Do NOT add AddStandardResilienceHandler() here.
            // Each typed HttpClient configures its own resilience with custom timeouts.
            // A global handler would add a second resilience pipeline with default 30s AttemptTimeout,
            // overriding the per-client configuration.
        });

        return builder;
    }

    /// <summary>
    /// Configures OpenTelemetry for logging, metrics, and tracing with OTLP export.
    /// </summary>
    public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation()
                       .AddHttpClientInstrumentation();
                // Runtime instrumentation (.AddRuntimeInstrumentation()) intentionally omitted:
                // GC, thread pool, and assembly metrics generated ~8 GB/month in AppMetrics
                // and AppPerformanceCounters with minimal operational value.

                // Drop high-volume metrics with no operational value for this app.
                // Only metrics generating >5k rows/day are targeted; everything else is left as-is
                // to avoid over-engineering the blocklist.

                // Tracks connection pool state (active/idle) per backend IP. Azure services rotate
                // IPs constantly, creating a new row per IP (~10x fan-out). http.client.active_requests
                // covers any connection pressure scenario worth alerting on.
                metrics.AddView("http.client.open_connections", MetricStreamConfiguration.Drop);

                // Fires for every Blazor component on every SSR page render (~20 components/request).
                // Pure render profiling data; no production health or alerting value.
                metrics.AddView("aspnetcore.components.update_parameters.duration", MetricStreamConfiguration.Drop);
            })
            .WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation(options =>
                       {
                           // Filter requests that have no diagnostic value and would inflate
                           // App Insights data volume:
                           //   - /health and /alive: Container Apps liveness/readiness probes,
                           //     fired every 10-30s × 2 replicas × 2 services (~2.9 GB/month).
                           //   - /_blazor/disconnect: browser's fire-and-forget goodbye message
                           //     on page unload. 499 (client closed) and 400 (circuit already
                           //     gone) are structurally expected. Real Blazor connectivity
                           //     failures appear on /_blazor/negotiate or the SignalR WebSocket,
                           //     not on the disconnect endpoint.
                           //   - Scanner/attacker probe paths: rejected by InvalidRouteSegment-
                           //     Middleware before Blazor runs; have no diagnostic value.
                           //   - Bot crawlers: follow stale links and generate expected 404s.
                           //     User-Agent containing "bot" (case-insensitive) is treated as
                           //     benign crawler traffic. Filtering here (not in a processor)
                           //     means no Activity is ever created — zero data cost.
                           options.Filter = httpContext =>
                               !IsHealthProbeRequest(httpContext.Request.Path) &&
                               !IsBlazorDisconnectRequest(httpContext.Request.Path) &&
                               !IsProbeRequest(httpContext.Request.Path) &&
                               !IsBotRequest(httpContext.Request.Headers.UserAgent.ToString());

                           // Fix client.address to reflect the real client IP after the
                           // ForwardedHeaders middleware has updated RemoteIpAddress from
                           // X-Forwarded-For. The OTel SDK captures client.address at activity
                           // start — before ForwardedHeaders runs — so without this override
                           // all requests appear to originate from the Container Apps NAT IP
                           // (which geo-resolves to Gävle, Sweden Central).
                           options.EnrichWithHttpResponse = (activity, httpResponse) =>
                           {
                               var ip = httpResponse.HttpContext.Connection.RemoteIpAddress;
                               if (ip != null)
                               {
                                   activity.SetTag("client.address", ip.MapToIPv4().ToString());
                               }
                           };
                       })
                       .AddHttpClientInstrumentation();
            });

        builder.AddOpenTelemetryExporters();

        return builder;
    }

    internal static bool IsHealthProbeRequest(PathString path)
    {
        return path.Equals("/health", StringComparison.OrdinalIgnoreCase)
            || path.Equals("/alive", StringComparison.OrdinalIgnoreCase);
    }

    internal static bool IsBlazorDisconnectRequest(PathString path)
    {
        return path.StartsWithSegments("/_blazor/disconnect", StringComparison.OrdinalIgnoreCase);
    }

    internal static bool IsBotRequest(string userAgent)
    {
        return !string.IsNullOrEmpty(userAgent)
            && userAgent.Contains("bot", StringComparison.OrdinalIgnoreCase);
    }

    // Mirror of InvalidRouteSegmentMiddleware._probeExtensions / _probePathSubstrings.
    // Duplicated here because ServiceDefaults cannot reference TechHub.Web.
    // Keep these two sets in sync with the middleware.
    private static readonly HashSet<string> _probeExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".php", ".asp", ".aspx", ".cfm", ".cgi", ".pl", ".py", ".rb", ".jsp",
        ".env", ".htaccess", ".htpasswd",
        ".bak", ".backup", ".old", ".orig", ".swp",
        ".exe", ".dll", ".sh", ".bat", ".cmd",
        ".sql",
        ".pem", ".key", ".crt", ".p12", ".pfx",
        ".zip", ".tar", ".gz", ".rar", ".7z",
    };

    private static readonly string[] _probePathSubstrings =
    [
        "wp-admin", "wp-content", "wp-includes", "wp-login",
        "xmlrpc",
        "phpmyadmin",
        "cgi-bin",
        "actuator",
    ];

    /// <summary>
    /// Returns <c>true</c> if <paramref name="path"/> matches a known scanner or
    /// attacker probe pattern. Used to suppress telemetry for these requests entirely
    /// (no Activity span created) so they do not appear in Azure Monitor / App Insights.
    /// </summary>
    internal static bool IsProbeRequest(PathString path)
    {
        var value = path.Value;
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        foreach (var probe in _probePathSubstrings)
        {
            if (value.Contains(probe, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        var lastDot = value.LastIndexOf('.');
        if (lastDot < 0)
        {
            return false;
        }

        var ext = value[lastDot..];

        if (ext.Equals(".xml", StringComparison.OrdinalIgnoreCase))
        {
            return !value.EndsWith("/feed.xml", StringComparison.OrdinalIgnoreCase)
                && !value.Equals("/sitemap.xml", StringComparison.OrdinalIgnoreCase);
        }

        return _probeExtensions.Contains(ext);
    }

    private static IHostApplicationBuilder AddOpenTelemetryExporters(this IHostApplicationBuilder builder)
    {
        var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

        if (useOtlpExporter)
        {
            builder.Services.AddOpenTelemetry().UseOtlpExporter();
        }

        if (!string.IsNullOrWhiteSpace(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
        {
            builder.Services.AddOpenTelemetry().UseAzureMonitor();
        }

        return builder;
    }

    /// <summary>
    /// Adds default health checks for liveness and readiness probes.
    /// </summary>
    public static IHostApplicationBuilder AddDefaultHealthChecks(this IHostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.AddHealthChecks()
            // Liveness check: verifies the app runtime is responsive and not resource-starved.
            // Intentionally does NOT check external dependencies (DB, APIs) — a DB outage
            // should not trigger container restarts (restart storm).
            .AddCheck("self", () =>
            {
                // Check GC isn't in a catastrophic state (e.g., out of memory pressure)
                var gcInfo = GC.GetGCMemoryInfo();
                var memoryUsagePercent = gcInfo.HighMemoryLoadThresholdBytes > 0
                    ? (double)gcInfo.MemoryLoadBytes / gcInfo.HighMemoryLoadThresholdBytes * 100
                    : 0;

                if (memoryUsagePercent > 95)
                {
                    return HealthCheckResult.Unhealthy(
                        $"Memory pressure critical: {memoryUsagePercent:F0}% of threshold");
                }

                return HealthCheckResult.Healthy();
            }, _liveHealthCheckTags);

        return builder;
    }

    /// <summary>
    /// Maps health check endpoints to the application.
    /// </summary>
    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        // Adding health checks endpoints to applications in non-development environments has security implications.
        // See https://aka.ms/dotnet/aspire/healthchecks for details before enabling in production.

        // All health checks must pass for app to be considered ready to accept traffic
        app.MapHealthChecks("/health");

        // Only health checks tagged with the "live" tag must pass for app to be considered alive
        app.MapHealthChecks("/alive", new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("live")
        });

        return app;
    }
}
