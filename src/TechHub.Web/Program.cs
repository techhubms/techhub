using Microsoft.AspNetCore.StaticFiles;
using TechHub.Core.Logging;
using TechHub.ServiceDefaults;
using TechHub.Web.Components;
using TechHub.Web.Configuration;
using TechHub.Web.Middleware;
using TechHub.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults (OpenTelemetry, service discovery, resilience, health checks)
builder.AddServiceDefaults();

// Log environment during startup for verification
using (var loggerFactory = LoggerFactory.Create(b => b.AddConsole()))
{
    var logger = loggerFactory.CreateLogger("Startup");
    logger.LogInformation("🚀 TechHub.Web starting in {Environment} environment", builder.Environment.EnvironmentName);
}

// Configure file logging
// Skip during integration tests (AppSettings:SkipFileLogging = true)
var skipFileLogging = builder.Configuration.GetValue<bool>("AppSettings:SkipFileLogging");
if (!skipFileLogging)
{
    // Build log path: use TECHHUB_TMP if set (Docker), otherwise .tmp (local dev)
    var tmpDir = Environment.GetEnvironmentVariable("TECHHUB_TMP") ?? ".tmp";
    var logPath = Path.Combine(tmpDir, "logs", $"web-{builder.Environment.EnvironmentName.ToLowerInvariant()}.log");

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

// Configure routing to be case-insensitive for better UX
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = false; // Keep query strings as-is
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure CSS bundling and minification for production only
if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddWebOptimizer(pipeline =>
    {
        // Bundle all CSS files into a single minified bundle for production
        pipeline.AddCssBundle("/css/bundle.css", CssFiles.All);
    });
}

// Section cache for immediate (flicker-free) navigation rendering
builder.Services.AddSingleton<SectionCache>();

// Favicon service for base64-encoded favicon (eliminates HTTP requests)
builder.Services.AddSingleton<FaviconService>();

// Error handling service for centralized exception management
builder.Services.AddScoped<ErrorService>();

// Configure SignalR for Blazor Server with increased timeouts for reliability
builder.Services.AddSignalR(options =>
{
    // Increase timeouts to prevent premature disconnections during initialization
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
    options.HandshakeTimeout = TimeSpan.FromSeconds(30);
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
    // Increase max message size to handle large prerendered content during hydration
    // Default is 32KB, but with many content items the state can exceed this
    options.MaximumReceiveMessageSize = 256 * 1024; // 256KB
});

// Configure HTTP client for API with service discovery
// When running via Aspire, "https+http://api" resolves via service discovery
// When running standalone, falls back to ApiBaseUrl config (e.g., https://localhost:5001)
var apiBaseUrl = builder.Configuration["services:api:https:0"]
    ?? builder.Configuration["services:api:http:0"]
    ?? builder.Configuration["ApiBaseUrl"]
    ?? "https://localhost:5001";

// Log the resolved API base URL for debugging
builder.Logging.AddSimpleConsole();
#pragma warning disable CA2000 // Intentional: Single-use logger for startup diagnostics, disposed with app lifetime
var startupLogger = LoggerFactory.Create(b => b.AddConsole()).CreateLogger("Startup");
#pragma warning restore CA2000
startupLogger.LogInformation("🔗 Connecting to API at: {ApiBaseUrl}", apiBaseUrl);

builder.Services.AddHttpClient<TechHubApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(100); // Allow extra time beyond resilience timeout
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    // Allow invalid certificates in development (Docker uses self-signed certs)
    var handler = new SocketsHttpHandler
    {
#pragma warning disable CA5359 // Required for Docker inter-container HTTPS communication in development
        // In Docker, accept any certificate for inter-container communication
        SslOptions = new System.Net.Security.SslClientAuthenticationOptions
        {
            RemoteCertificateValidationCallback = (_, _, _, _) => true
        }
#pragma warning restore CA5359
    };
    return handler;
})
.AddStandardResilienceHandler(options =>
{
    // Configure timeouts to handle slow API startup (database seeding can take ~60s)
    // Total: 6 attempts × 10s + 5 retries × 3s = 75s, capped at 90s hard timeout
    options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(90);
    options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(10);
    options.Retry.MaxRetryAttempts = 5;
    options.Retry.Delay = TimeSpan.FromSeconds(3);
    options.Retry.BackoffType = Polly.DelayBackoffType.Constant; // Don't use exponential backoff
});
// Register interface for dependency injection (scoped to match HttpClient lifetime)
builder.Services.AddScoped<ITechHubApiClient>(sp => sp.GetRequiredService<TechHubApiClient>());

var app = builder.Build();

// Pre-load sections into cache at startup for flicker-free navigation
using (var scope = app.Services.CreateScope())
{
    var apiClient = scope.ServiceProvider.GetRequiredService<TechHubApiClient>();
    var sectionCache = scope.ServiceProvider.GetRequiredService<SectionCache>();

    var sections = await apiClient.GetAllSectionsAsync();
    sectionCache.Initialize(sections?.ToList() ?? []);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Reject old date-prefixed URLs (YYYY-MM-DD-slug) before routing
app.UseRejectDatePrefixedSlugs();

// Status code pages middleware: converts 404 status codes to /not-found page
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

// Enable WebOptimizer middleware for CSS bundling/minification in production
// Note: MapStaticAssets() handles compression/fingerprinting, WebOptimizer provides bundling
if (!app.Environment.IsDevelopment())
{
    app.UseWebOptimizer();
}

app.UseAntiforgery();

// Static file caching: Our middleware sets proper cache headers for all static files
// Place BEFORE MapStaticAssets so our OnStarting callback runs AFTER theirs (LIFO order)
app.UseStaticFilesCaching();

// UseStaticFiles for custom content types (e.g., .jxl)
// MapStaticAssets handles the serving with fingerprinting, compression, ETags
var contentTypeProvider = new FileExtensionContentTypeProvider();
contentTypeProvider.Mappings[".jxl"] = "image/jxl";
app.UseStaticFiles(new StaticFileOptions { ContentTypeProvider = contentTypeProvider });

// MapStaticAssets for optimized static assets (fingerprinting, compression, ETags)
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Page timing endpoint - receives client-side performance metrics
app.MapPost("/api/page-timing", (PageTimingMetrics metrics, ILogger<Program> logger) =>
{
    logger.LogInformation(
        "Page {Page} loaded: DNS={DnsMs}ms, TCP={TcpMs}ms, Request={RequestMs}ms, Response={ResponseMs}ms, " +
        "DOMParse={DomParseMs}ms, TimeToInteractive={TimeToInteractiveMs}ms, DOMReady={DomReadyMs}ms, TotalLoad={TotalLoadMs}ms",
        metrics.Page, metrics.Dns, metrics.Tcp, metrics.Request, metrics.Response,
        metrics.DomParse, metrics.TimeToInteractive, metrics.DomReady, metrics.TotalLoad
    );
    return Results.Ok();
})
.WithName("LogPageTiming")
.DisableAntiforgery()  // Allow POST without antiforgery token
.ExcludeFromDescription();

// RSS feed proxy endpoints - serve feeds from same domain as website
app.MapGet("/all/feed.xml", async (TechHubApiClient apiClient, CancellationToken ct) =>
{
    var xml = await apiClient.GetAllContentRssFeedAsync(ct);
    return Results.Content(xml, "application/rss+xml; charset=utf-8");
})
.WithName("GetAllContentRssFeed")
.WithSummary("RSS feed for all content")
.ExcludeFromDescription();

// Special handling for /all/roundups/feed.xml before general section route
app.MapGet("/all/roundups/feed.xml", async (TechHubApiClient apiClient, CancellationToken ct) =>
{
    var xml = await apiClient.GetCollectionRssFeedAsync("roundups", "all", ct);
    return Results.Content(xml, "application/rss+xml; charset=utf-8");
})
.WithName("GetRoundupsRssFeed")
.WithSummary("RSS feed for roundups collection")
.ExcludeFromDescription();

app.MapGet("/{sectionName}/feed.xml", async (string sectionName, TechHubApiClient apiClient, CancellationToken ct) =>
{
    var xml = await apiClient.GetSectionRssFeedAsync(sectionName, ct);
    return Results.Content(xml, "application/rss+xml; charset=utf-8");
})
.WithName("GetSectionRssFeed")
.WithSummary("RSS feed for a section")
.ExcludeFromDescription();

// Map Aspire default health check endpoints (/health and /alive)
app.MapDefaultEndpoints();

await app.RunAsync();

// Page timing metrics DTO
internal record PageTimingMetrics(
    string Page,
    int Dns,
    int Tcp,
    int Request,
    int Response,
    int DomParse,
    int TimeToInteractive,
    int DomReady,
    int TotalLoad
);
