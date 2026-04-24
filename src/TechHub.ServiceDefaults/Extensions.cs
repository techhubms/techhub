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
            })
            .WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation(options =>
                       {
                           // Filter out health probe requests from telemetry.
                           // Container Apps fires /alive (liveness) and /health (readiness)
                           // every 10-30s × 2 replicas × 2 services, generating ~2.9 GB/month
                           // in AppRequests with no diagnostic value.
                           options.Filter = httpContext =>
                               !IsHealthProbeRequest(httpContext.Request.Path);
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
