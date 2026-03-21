using System.Data;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using TechHub.ContentProcessor.Options;
using TechHub.ContentProcessor.Services;
using TechHub.ContentProcessor.Workers;
using TechHub.Core.Interfaces;
using TechHub.Infrastructure.Data;

var builder = new HostApplicationBuilder(args);

// ─── Logging ────────────────────────────────────────────────────────────────
builder.Logging.AddConsole();

// ─── OpenTelemetry ───────────────────────────────────────────────────────────
var otelEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing.AddHttpClientInstrumentation();

        if (!string.IsNullOrWhiteSpace(otelEndpoint))
        {
            tracing.AddOtlpExporter();
        }
    })
    .WithMetrics(metrics =>
    {
        metrics.AddRuntimeInstrumentation()
               .AddHttpClientInstrumentation();

        if (!string.IsNullOrWhiteSpace(otelEndpoint))
        {
            metrics.AddOtlpExporter();
        }
    });

// ─── Configuration options ────────────────────────────────────────────────────
builder.Services.Configure<ContentProcessorOptions>(
    builder.Configuration.GetSection(ContentProcessorOptions.SectionName));

builder.Services.Configure<AiCategorizationOptions>(
    builder.Configuration.GetSection(AiCategorizationOptions.SectionName));

// ─── Database ─────────────────────────────────────────────────────────────────
var connectionString = builder.Configuration["Database:ConnectionString"]
    ?? throw new InvalidOperationException("Database:ConnectionString is required.");

builder.Services.AddSingleton<ISqlDialect, PostgresDialect>();
builder.Services.AddSingleton<IDbConnectionFactory>(_ => new PostgresConnectionFactory(connectionString));
builder.Services.AddScoped<IDbConnection>(sp => sp.GetRequiredService<IDbConnectionFactory>().CreateConnection());

// ─── Content processor services ───────────────────────────────────────────────
builder.Services.AddHttpClient<RssFeedIngestionService>()
    .ConfigureHttpClient(client =>
    {
        client.DefaultRequestHeaders.UserAgent.ParseAdd("TechHub-ContentProcessor/1.0");
        client.Timeout = TimeSpan.FromSeconds(60);
    });

builder.Services.AddHttpClient<ArticleContentService>()
    .ConfigureHttpClient(client =>
    {
        client.DefaultRequestHeaders.UserAgent.ParseAdd("TechHub-ContentProcessor/1.0 (content-fetch)");
        client.Timeout = TimeSpan.FromSeconds(30);
    });

builder.Services.AddHttpClient<AiCategorizationService>()
    .ConfigureHttpClient(client =>
    {
        client.Timeout = TimeSpan.FromSeconds(120);
    });

builder.Services.AddScoped<ContentDatabaseWriter>();

// ─── Background worker ────────────────────────────────────────────────────────
builder.Services.AddHostedService<ContentProcessorWorker>();

var host = builder.Build();

host.Run();
