using System.Data;
using TechHub.Api.Endpoints;
using TechHub.Api.HealthChecks;
using TechHub.Api.Middleware;
using TechHub.Api.Services;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Logging;
using TechHub.Infrastructure.Data;
using TechHub.Infrastructure.Repositories;
using TechHub.Infrastructure.Services;
using TechHub.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults (OpenTelemetry, service discovery, resilience, health checks)
builder.AddServiceDefaults();

// Register startup state tracking service as singleton
builder.Services.AddSingleton<StartupStateService>();

// Add custom health check that waits for startup operations to complete
builder.Services.AddHealthChecks()
    .AddCheck<StartupHealthCheck>("startup", tags: ["ready"]);

// Log environment during startup for verification
using (var loggerFactory = LoggerFactory.Create(b => b.AddConsole()))
{
    var logger = loggerFactory.CreateLogger("Startup");
    logger.LogInformation("🚀 TechHub.Api starting in {Environment} environment", builder.Environment.EnvironmentName);
}

// Configure file logging
// Skip during integration tests (AppSettings:SkipFileLogging = true)
var skipFileLogging = builder.Configuration.GetValue<bool>("AppSettings:SkipFileLogging");
if (!skipFileLogging)
{
    // Build log path: use TECHHUB_TMP if set (Docker), otherwise .tmp (local dev)
    var tmpDir = Environment.GetEnvironmentVariable("TECHHUB_TMP") ?? ".tmp";
    var logPath = Path.Combine(tmpDir, "logs", $"api-{builder.Environment.EnvironmentName.ToLowerInvariant()}.log");

    // Parse log levels from configuration
    var logLevels = new Dictionary<string, LogLevel>();
    var logLevelSection = builder.Configuration.GetSection("Logging:File:LogLevel");
    foreach (var child in logLevelSection.GetChildren())
    {
        if (Enum.TryParse<LogLevel>(child.Value, ignoreCase: true, out var level))
        {
            logLevels[child.Key] = level;
        }
    }

    // FileLoggerProvider is registered with DI and disposed by framework
#pragma warning disable CA2000
    builder.Logging.AddProvider(new FileLoggerProvider(logPath, logLevels));
#pragma warning restore CA2000
}

// Add services to the container
builder.Services.AddOpenApi();

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Title = "Tech Hub API",
        Version = "v1",
        Description = "API for Tech Hub content and sections"
    });
});

// Add memory caching
builder.Services.AddMemoryCache();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<TagCloudOptions>(builder.Configuration.GetSection("AppSettings:TagCloud"));
builder.Services.Configure<ApiOptions>(builder.Configuration.GetSection("AppSettings:Api"));
builder.Services.Configure<RssOptions>(builder.Configuration.GetSection("AppSettings:Rss"));
builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection("Database"));
builder.Services.Configure<ContentSyncOptions>(builder.Configuration.GetSection("ContentSync"));
builder.Services.Configure<ContentOptions>(builder.Configuration.GetSection("AppSettings:Content"));

// Register database connection and repository (PostgreSQL only)
var connectionString = builder.Configuration["Database:ConnectionString"]
    ?? throw new InvalidOperationException("Database:ConnectionString is required. Configure it in appsettings.json.");

builder.Services.AddSingleton<ISqlDialect, PostgresDialect>();
builder.Services.AddSingleton<IDbConnectionFactory>(_ => new PostgresConnectionFactory(connectionString));
builder.Services.AddScoped<IDbConnection>(sp => sp.GetRequiredService<IDbConnectionFactory>().CreateConnection());
builder.Services.AddTransient<IContentRepository, ContentRepository>();

// Register singleton services - none currently

// Register transient services
builder.Services.AddTransient<IMarkdownService, MarkdownService>();
builder.Services.AddTransient<IRssService, RssService>();
builder.Services.AddTransient<IContentSyncService, ContentSyncService>();
builder.Services.AddTransient<IMigrationRunner, MigrationRunner>();

// Register startup background service that runs migrations and content sync
// after Kestrel starts, so health endpoints are reachable during startup
builder.Services.AddHostedService<StartupBackgroundService>();

// Add CORS
builder.Services.AddCors(options =>
{
    var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
        ?? ["https://localhost:5003"];

    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(allowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

// Global exception handler (must be first)
app.UseMiddleware<ExceptionHandlerMiddleware>();

// Request logging (after exception handler to capture all requests)
app.UseMiddleware<RequestLoggingMiddleware>();

// Enable Swagger in Development environment
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Tech Hub API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseCors();

// Map API endpoints
app.MapContentEndpoints();
app.MapCustomPagesEndpoints();
app.MapRssEndpoints();

// Map Aspire default health check endpoints (/health and /alive)
app.MapDefaultEndpoints();

await app.RunAsync();
