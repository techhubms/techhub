using System.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Polly;
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
    .AddCheck<StartupHealthCheck>("startup", tags: new[] { "ready" });

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
builder.Services.Configure<ContentOptions>(builder.Configuration.GetSection("AppSettings:Content"));

// Register database connection and repository (PostgreSQL only)
var connectionString = builder.Configuration["Database:ConnectionString"]
    ?? throw new InvalidOperationException("Database:ConnectionString is required. Configure it in appsettings.json.");

builder.Services.AddSingleton<ISqlDialect, PostgresDialect>();
builder.Services.AddSingleton<IDbConnectionFactory>(_ => new PostgresConnectionFactory(connectionString));
builder.Services.AddScoped<IDbConnection>(sp => sp.GetRequiredService<IDbConnectionFactory>().CreateConnection());
builder.Services.AddTransient<IContentRepository, ContentRepository>();

// TimeProvider for testable date/time
builder.Services.AddSingleton(TimeProvider.System);

// Register singleton services - none currently

// Register transient services
builder.Services.AddTransient<IMarkdownService, MarkdownService>();
builder.Services.AddTransient<IRssService, RssService>();
builder.Services.AddTransient<IMigrationRunner, MigrationRunner>();

// RSS feed config repository (database-backed)
builder.Services.AddScoped<IRssFeedConfigRepository, RssFeedConfigRepository>();

// Custom page data repository (database-backed)
builder.Services.AddScoped<ICustomPageDataRepository, CustomPageDataRepository>();

// ─── Content Processing Pipeline ─────────────────────────────────────────────
// Configure content processing options
builder.Services.Configure<ContentProcessorOptions>(
    builder.Configuration.GetSection(ContentProcessorOptions.SectionName));
builder.Services.Configure<AiCategorizationOptions>(
    builder.Configuration.GetSection(AiCategorizationOptions.SectionName));

// Repository for job tracking (scoped — reuses the scoped IDbConnection)
builder.Services.AddScoped<IContentProcessingJobRepository, ContentProcessingJobRepository>();

// Repository for processed URL tracking (scoped — reuses the scoped IDbConnection)
builder.Services.AddScoped<IProcessedUrlRepository, ProcessedUrlRepository>();

// YouTube transcript fetcher (no HTTP client needed — uses YoutubeExplode internally)
builder.Services.AddTransient<IYouTubeTranscriptService, YouTubeTranscriptService>();

// Typed HTTP clients for external API communication with Polly resilience
builder.Services.AddHttpClient<IRssFeedClient, RssFeedClient>()
    .ConfigureHttpClient(client =>
    {
        client.DefaultRequestHeaders.UserAgent.ParseAdd("TechHub-ContentProcessor/1.0");
        client.Timeout = TimeSpan.FromSeconds(90);
    })
    .AddStandardResilienceHandler(options =>
    {
        options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(90);
        options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(30);
        options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(90);
        options.Retry.MaxRetryAttempts = 2;
        options.Retry.Delay = TimeSpan.FromSeconds(3);
        options.Retry.BackoffType = DelayBackoffType.Exponential;
    });

builder.Services.AddHttpClient<IArticleFetchClient, ArticleFetchClient>()
    .ConfigureHttpClient(client =>
    {
        client.DefaultRequestHeaders.UserAgent.ParseAdd("TechHub-ContentProcessor/1.0");
        client.Timeout = TimeSpan.FromSeconds(60);
    })
    .AddStandardResilienceHandler(options =>
    {
        options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(60);
        options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(20);
        options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(60);
        options.Retry.MaxRetryAttempts = 2;
        options.Retry.Delay = TimeSpan.FromSeconds(2);
        options.Retry.BackoffType = DelayBackoffType.Exponential;
    });

builder.Services.AddHttpClient<IYouTubeTagClient, YouTubeTagClient>()
    .ConfigureHttpClient(client =>
    {
        client.DefaultRequestHeaders.UserAgent.ParseAdd("TechHub-ContentProcessor/1.0");
        client.Timeout = TimeSpan.FromSeconds(30);
    })
    .AddStandardResilienceHandler(options =>
    {
        options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(30);
        options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(10);
        options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(30);
        options.Retry.MaxRetryAttempts = 2;
        options.Retry.Delay = TimeSpan.FromSeconds(1);
        options.Retry.BackoffType = DelayBackoffType.Exponential;
    });

builder.Services.AddHttpClient<IAiCompletionClient, AiCompletionClient>()
    .ConfigureHttpClient(client =>
    {
        client.Timeout = TimeSpan.FromSeconds(120);
    })
    .AddStandardResilienceHandler(options =>
    {
        // No Polly-level retries — AiCategorizationService handles retries with rate limit awareness
        options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(120);
        options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(120);
        options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(240);
        options.Retry.MaxRetryAttempts = 1;
        options.Retry.ShouldHandle = _ => ValueTask.FromResult(false);
    });

// Processing services (depend on typed clients above, not on HttpClient directly)
builder.Services.AddTransient<IRssFeedIngestionService, RssFeedIngestionService>();
builder.Services.AddTransient<IArticleContentService, ArticleContentService>();
builder.Services.AddTransient<IYouTubeTagService, YouTubeTagService>();
builder.Services.AddTransient<IAiCategorizationService, AiCategorizationService>();

// Admin services
builder.Services.AddTransient<DatabaseStatisticsService>();

// Scoped orchestrator — creates its own scope per call to run in IHostedService
builder.Services.AddScoped<ContentProcessingService>();

// Background service — singleton, schedules processing runs
builder.Services.AddSingleton<ContentProcessingBackgroundService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<ContentProcessingBackgroundService>());

// ─── Roundup Generator Pipeline ──────────────────────────────────────────────
builder.Services.Configure<RoundupGeneratorOptions>(
    builder.Configuration.GetSection(RoundupGeneratorOptions.SectionName));
builder.Services.AddScoped<ISectionRoundupRepository, SectionRoundupRepository>();
builder.Services.AddScoped<IRoundupGeneratorService, RoundupGeneratorService>();
builder.Services.AddSingleton<RoundupGeneratorBackgroundService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<RoundupGeneratorBackgroundService>());

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

// ─── Admin Authentication (Azure AD JWT Bearer) ─────────────────────────────
// When AzureAd:ClientId is configured, admin endpoints require a valid bearer token
// issued by Azure AD (forwarded by the Web front-end). When not configured (local dev),
// the AdminOnly policy allows all requests so admin endpoints remain accessible.
var azureAdClientId = builder.Configuration["AzureAd:ClientId"];
if (!string.IsNullOrEmpty(azureAdClientId))
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
}
else
{
    // Register base auth services so UseAuthentication() middleware doesn't throw
    builder.Services.AddAuthentication();
}

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        if (!string.IsNullOrEmpty(azureAdClientId))
        {
            policy.RequireAuthenticatedUser();

            // Validate that the access token contains the expected API scope.
            // The scope is configured via AzureAd:Scopes (e.g. "Admin.Access").
            // Microsoft.Identity.Web normalizes scope claims so only the short
            // name (without the api:// prefix) appears in the "scp" claim.
            var requiredScope = builder.Configuration.GetValue<string>("AzureAd:Scopes");
            if (!string.IsNullOrEmpty(requiredScope))
            {
                policy.RequireScope(requiredScope.Split(' ', StringSplitOptions.RemoveEmptyEntries));
            }
        }
        else
        {
            // When Azure AD is not configured (local dev), allow all requests
            policy.RequireAssertion(_ => true);
        }
    });
});

var app = builder.Build();

// Global exception handler (must be first)
app.UseMiddleware<ExceptionHandlerMiddleware>();

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
app.UseAuthentication();
app.UseAuthorization();

// Map API endpoints
app.MapContentEndpoints();
app.MapCustomPagesEndpoints();
app.MapRssEndpoints();
app.MapSitemapEndpoints();
app.MapAuthorEndpoints();
app.MapAdminEndpoints();

// Map Aspire default health check endpoints (/health and /alive)
app.MapDefaultEndpoints();

await app.RunAsync();
