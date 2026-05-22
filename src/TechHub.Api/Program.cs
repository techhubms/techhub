using System.Data;
using System.Net;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Npgsql;
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
using TechHub.Infrastructure.Services.ContentProcessing;
using TechHub.Infrastructure.Services.RoundupGeneration;
using TechHub.ServiceDefaults;
var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults (OpenTelemetry, service discovery, resilience, health checks)
builder.AddServiceDefaults();

// Register startup state tracking service as singleton
builder.Services.AddSingleton<StartupStateService>();

// Add custom health check that waits for startup operations to complete
builder.Services.AddHealthChecks()
    .AddCheck<StartupHealthCheck>("startup", tags: new[] { "ready" })
    .AddCheck<DatabaseHealthCheck>("database", tags: new[] { "ready" });

// Log environment during startup for verification
using (var loggerFactory = LoggerFactory.Create(b => b.AddConsole()))
{
    var logger = loggerFactory.CreateLogger("Startup");
    logger.LogInformation("🚀 TechHub.Api starting in {Environment} environment", builder.Environment.EnvironmentName);
}

// Configure file logging
// Skipped in production (AppSettings:SkipFileLogging = true) — containers log to stdout/stderr.
var skipFileLogging = builder.Configuration.GetValue<bool>("AppSettings:SkipFileLogging");
if (!skipFileLogging)
{
    // Use TECHHUB_TMP if set (devcontainer), otherwise the system temp directory
    var tmpDir = Environment.GetEnvironmentVariable("TECHHUB_TMP") ?? Path.GetTempPath();
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

// When running with a managed identity (e.g. PR preview containers that use a per-PR
// user-assigned identity), acquire PostgreSQL access tokens via DefaultAzureCredential
// instead of a password. Set Database:UseEntraAuth=true to opt in.
var useEntraAuth = builder.Configuration.GetValue<bool>("Database:UseEntraAuth");

// Register NpgsqlDataSource as singleton — it manages the connection pool internally.
// All connections are acquired from this pool via OpenConnection/OpenConnectionAsync.
// DI owns the lifetime and disposes the data source on application shutdown.
builder.Services.AddSingleton(_ => NpgsqlDataSourceFactory.Create(connectionString, useEntraAuth));

builder.Services.AddSingleton<ISqlDialect, PostgresDialect>();
builder.Services.AddSingleton<IDbConnectionFactory>(sp =>
    new PostgresConnectionFactory(sp.GetRequiredService<NpgsqlDataSource>()));
builder.Services.AddScoped<IDbConnection>(sp =>
    sp.GetRequiredService<NpgsqlDataSource>().OpenConnection());
builder.Services.AddTransient<IContentRepository, ContentRepository>();
builder.Services.AddTransient<IGhcFeatureRepository, GhcFeatureRepository>();
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

// Background job settings repository (database-backed)
builder.Services.AddScoped<IBackgroundJobSettingRepository, BackgroundJobSettingRepository>();

// ─── Content Processing Pipeline ─────────────────────────────────────────────
// Configure content processing options (fails at startup if BrowserUserAgent is missing)
builder.Services.AddOptionsWithValidateOnStart<ContentProcessorOptions>()
    .Bind(builder.Configuration.GetSection(ContentProcessorOptions.SectionName))
    .Validate(o => !string.IsNullOrWhiteSpace(o.BrowserUserAgent),
        "ContentProcessor:BrowserUserAgent must be configured. Set it in appsettings.json or Key Vault.");
builder.Services.Configure<AiCategorizationOptions>(
    builder.Configuration.GetSection(AiCategorizationOptions.SectionName));

// Repository for job tracking (scoped — reuses the scoped IDbConnection)
builder.Services.AddScoped<IContentProcessingJobRepository, ContentProcessingJobRepository>();

// Repository for processed URL tracking (scoped — reuses the scoped IDbConnection)
builder.Services.AddScoped<IProcessedUrlRepository, ProcessedUrlRepository>();

// Typed HTTP clients for external API communication with Polly resilience
builder.Services.AddHttpClient<IRssFeedClient, RssFeedClient>()
    .ConfigureHttpClient(client =>
    {
        client.DefaultRequestHeaders.UserAgent.ParseAdd(
            "Mozilla/5.0 (compatible; TechHub-ContentProcessor/1.0; +https://tech.hub.ms)");
        client.DefaultRequestHeaders.Accept.ParseAdd(
            "application/rss+xml,application/atom+xml,application/xml;q=0.9,*/*;q=0.8");
        client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US,en;q=0.9");
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
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        // Disable auto-redirect so ArticleFetchClient can handle HTTPS→HTTP scheme-downgrade
        // redirects explicitly (e.g. mindbyte.nl redirects www.mindbyte.nl → mindbyte.nl over HTTP).
        AllowAutoRedirect = false
    })
    .ConfigureHttpClient((sp, client) =>
    {
        var options = sp.GetRequiredService<IOptions<ContentProcessorOptions>>().Value;
        client.DefaultRequestHeaders.UserAgent.ParseAdd(options.BrowserUserAgent);
        client.DefaultRequestHeaders.Accept.ParseAdd(
            "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
        client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US,en;q=0.9");
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
    .ConfigureHttpClient((sp, client) =>
    {
        var options = sp.GetRequiredService<IOptions<ContentProcessorOptions>>().Value;
        client.DefaultRequestHeaders.UserAgent.ParseAdd(options.BrowserUserAgent);
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

// Content review repository — scoped (reuses the scoped IDbConnection)
builder.Services.AddScoped<IContentReviewRepository, ContentReviewRepository>();

// Content fixer service — scoped (reuses the scoped IDbConnection)
builder.Services.AddScoped<IContentFixerService, ContentFixerService>();

// Scoped orchestrator — creates its own scope per call to run in IHostedService
builder.Services.AddScoped<ContentProcessingService>();

// Background service — singleton, schedules processing runs
builder.Services.AddSingleton<ContentProcessingBackgroundService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<ContentProcessingBackgroundService>());

// Content item write repository (scoped — reuses the scoped IDbConnection)
builder.Services.AddScoped<IContentItemWriteRepository, ContentItemWriteRepository>();

// Content fixer background service — manual trigger only, no periodic schedule
builder.Services.AddSingleton<ContentFixerBackgroundService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<ContentFixerBackgroundService>());

// ─── Roundup Generator Pipeline ──────────────────────────────────────────────
builder.Services.Configure<RoundupGeneratorOptions>(
    builder.Configuration.GetSection(RoundupGeneratorOptions.SectionName));
builder.Services.AddScoped<ISectionRoundupRepository, SectionRoundupRepository>();
builder.Services.AddRoundupGeneration();
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

            // The API scope is part of the app registration's public contract,
            // defined by Manage-EntraId.ps1 and not environment-specific. Entra ID
            // places the short scope name (without the "api://<client-id>/" prefix)
            // in the JWT "scp" claim, so we validate against that directly.
            policy.RequireScope("Admin.Access");
        }
        else
        {
            // When Azure AD is not configured (local dev), allow all requests
            policy.RequireAssertion(_ => true);
        }
    });
});

// Rate limiting: defense-in-depth for the API (primary rate limiting is on the Web layer)
// Disabled in IntegrationTest environment to avoid throttling test suites that run many
// requests from a single IP within a short window.
// Loopback exemptions are scoped to Development/IntegrationTest so that a spoofed
// X-Forwarded-For: 127.0.0.1 cannot bypass rate limiting in production
// (UseForwardedHeaders runs before UseRateLimiter).
var isIntegrationTest = builder.Environment.IsEnvironment("IntegrationTest");
var isLoopbackExempt = builder.Environment.IsDevelopment() || isIntegrationTest;
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.OnRejected = async (context, token) =>
    {
        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            context.HttpContext.Response.Headers.RetryAfter =
                ((int)retryAfter.TotalSeconds).ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        await context.HttpContext.Response.WriteAsync("Rate limit exceeded. Please retry later.", token);
    };

    // Public content endpoints: generous limit (defense against runaway loops or future architecture changes).
    // Loopback connections (localhost) are exempt — they are always local dev or CI integration test
    // runners, not external clients. Loopback exemption is restricted to Development and
    // IntegrationTest: UseForwardedHeaders rewrites RemoteIpAddress before UseRateLimiter
    // runs, so a spoofed X-Forwarded-For: 127.0.0.1 could otherwise bypass rate limiting.
    options.AddPolicy("api-public", context =>
    {
        if (isIntegrationTest)
        {
            return RateLimitPartition.GetNoLimiter("no-limit");
        }

        var ip = context.Connection.RemoteIpAddress;
        if (isLoopbackExempt && ip != null && IPAddress.IsLoopback(ip))
        {
            return RateLimitPartition.GetNoLimiter("loopback");
        }

        return RateLimitPartition.GetSlidingWindowLimiter(
            partitionKey: ip?.ToString() ?? "unknown",
            factory: _ => new SlidingWindowRateLimiterOptions
            {
                PermitLimit = 200,
                Window = TimeSpan.FromMinutes(1),
                SegmentsPerWindow = 6,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 10
            });
    });

    // Admin endpoints: per-user limit for authenticated requests (generous for legitimate admin
    // work), strict IP-based limit for unauthenticated requests (brute-force protection).
    // Loopback exemption is restricted to Development/IntegrationTest for the same reason as api-public.
    // NOTE: UseRateLimiter() must run after UseAuthentication() so context.User is populated.
    options.AddPolicy("api-admin", context =>
    {
        if (isIntegrationTest)
        {
            return RateLimitPartition.GetNoLimiter("no-limit");
        }

        var ip = context.Connection.RemoteIpAddress;
        if (isLoopbackExempt && ip != null && IPAddress.IsLoopback(ip))
        {
            return RateLimitPartition.GetNoLimiter("loopback");
        }

        // Prefer the Azure AD object-ID claim so each admin user gets their own bucket.
        // Fall back to IP when the request is unauthenticated (keeps the strict limit that
        // deters auth brute-forcing).
        var userId = context.User.FindFirst("oid")?.Value
            ?? context.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;

        if (userId is not null)
        {
            return RateLimitPartition.GetSlidingWindowLimiter(
                partitionKey: $"user:{userId}",
                factory: _ => new SlidingWindowRateLimiterOptions
                {
                    PermitLimit = 200,
                    Window = TimeSpan.FromMinutes(1),
                    SegmentsPerWindow = 6,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 10
                });
        }

        // Unauthenticated — keep a strict IP limit to deter auth brute-forcing.
        return RateLimitPartition.GetSlidingWindowLimiter(
            partitionKey: ip?.ToString() ?? "unknown",
            factory: _ => new SlidingWindowRateLimiterOptions
            {
                PermitLimit = 30,
                Window = TimeSpan.FromMinutes(1),
                SegmentsPerWindow = 6,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            });
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

// Trust X-Forwarded-For from the Azure Container Apps reverse proxy so rate limiting
// partitions by real client IP rather than the proxy/NAT address.
// KnownIPNetworks/KnownProxies are cleared because the proxy IPs are internal and dynamic.
var forwardedHeadersOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
};
forwardedHeadersOptions.KnownIPNetworks.Clear();
forwardedHeadersOptions.KnownProxies.Clear();
app.UseForwardedHeaders(forwardedHeadersOptions);

// Authentication must run before rate limiting so the api-admin policy can read
// the Azure AD OID claim and partition by user rather than by IP.
app.UseAuthentication();
app.UseRateLimiter();
app.UseAuthorization();

// Map API endpoints
app.MapContentEndpoints();
app.MapCustomPagesEndpoints();
app.MapRssEndpoints();
app.MapSitemapEndpoints();
app.MapAuthorEndpoints();
app.MapGhcFeaturesEndpoints();
app.MapAdminEndpoints();

// Map Aspire default health check endpoints (/health and /alive)
app.MapDefaultEndpoints();

await app.RunAsync();
