using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace TechHub.Api.Extensions;

/// <summary>
/// Extension methods for configuring OpenTelemetry distributed tracing
/// </summary>
public static class TelemetryExtensions
{
    /// <summary>
    /// Add OpenTelemetry tracing to the service collection
    /// </summary>
    public static IServiceCollection AddTelemetry(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService(
                    serviceName: "TechHub.Api",
                    serviceVersion: typeof(Program).Assembly.GetName().Version?.ToString() ?? "1.0.0",
                    serviceInstanceId: Environment.MachineName))
            .WithTracing(tracing => tracing
                .AddAspNetCoreInstrumentation(options =>
                {
                    options.RecordException = true;
                    options.Filter = httpContext =>
                    {
                        // Don't trace health check endpoints
                        return !httpContext.Request.Path.StartsWithSegments("/health");
                    };
                })
                .AddHttpClientInstrumentation()
                .AddSource("TechHub.*")
                .AddConsoleExporter());

        return services;
    }
}
