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

// Configure file logging when path is configured in appsettings
// Skip during integration tests (AppSettings:SkipFileLogging = true)
var skipFileLogging = builder.Configuration.GetValue<bool>("AppSettings:SkipFileLogging");
var logPath = builder.Configuration["Logging:File:Path"];
if (!skipFileLogging && !string.IsNullOrEmpty(logPath))
{
    // FileLoggerProvider is registered with DI and disposed by framework
#pragma warning disable CA2000
    builder.Logging.AddProvider(new FileLoggerProvider(logPath));
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
});

// Configure HTTP client for API with service discovery
// When running via Aspire, "https+http://api" resolves via service discovery
// When running standalone, falls back to ApiBaseUrl config (e.g., https://localhost:5001)
var apiBaseUrl = builder.Configuration["services:api:https:0"]
    ?? builder.Configuration["services:api:http:0"]
    ?? builder.Configuration["ApiBaseUrl"]
    ?? "https://localhost:5001";

builder.Services.AddHttpClient<TechHubApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});
// Register interface for dependency injection (scoped to match HttpClient lifetime)
builder.Services.AddScoped<ITechHubApiClient>(sp => sp.GetRequiredService<TechHubApiClient>());
// Note: Resilience handler is already configured by AddServiceDefaults()

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
    var xml = await apiClient.GetCollectionRssFeedAsync("roundups", ct);
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
