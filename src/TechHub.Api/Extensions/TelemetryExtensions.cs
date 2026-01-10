using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace TechHub.Api.Extensions;

/// <summary>
/// Extension methods for configuring OpenTelemetry distributed tracing
/// </summary>
internal static class TelemetryExtensions
{
    /// <summary>
    /// Add OpenTelemetry tracing to the service collection
    /// </summary>
    public static IServiceCollection AddTelemetry(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        var builder = services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService(
                    serviceName: "TechHub.Api",
                    serviceVersion: typeof(Program).Assembly.GetName().Version?.ToString() ?? "1.0.0",
                    serviceInstanceId: Environment.MachineName))
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.RecordException = true;
                        options.Filter = httpContext =>
                        {
                            var path = httpContext.Request.Path.Value ?? string.Empty;

                            // Don't trace health checks
                            if (path.StartsWith("/health", StringComparison.OrdinalIgnoreCase))
                                return false;

                            // Don't trace common browser requests that return 404
                            if (path.EndsWith("/favicon.ico", StringComparison.OrdinalIgnoreCase) ||
                                path.EndsWith(".map", StringComparison.OrdinalIgnoreCase))
                                return false;

                            return true;
                        };
                    })
                    .AddHttpClientInstrumentation()
                    .AddSource("TechHub.*");

                // NOTE: Console exporter intentionally not added to reduce log noise.
                // Traces are collected and exported to Azure Monitor/App Insights if configured.

                var appInsightsEndpoint = configuration["ApplicationInsights:Endpoint"];
                if (!string.IsNullOrWhiteSpace(appInsightsEndpoint))
                {
                    tracing.AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(appInsightsEndpoint);
                    });
                }
            });

        return services;
    }
}
