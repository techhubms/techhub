using TechHub.Core.Configuration;
using TechHub.Core.Logging;
using TechHub.Web.Components;
using TechHub.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure file logging for Development and Test environments
// Skip during integration tests (AppSettings:SkipFileLogging = true)
var skipFileLogging = builder.Configuration.GetValue<bool>("AppSettings:SkipFileLogging");
if (!skipFileLogging && (builder.Environment.IsDevelopment() || builder.Environment.IsEnvironment("Test")))
{
    var logPath = builder.Configuration["Logging:File:Path"];
    if (!string.IsNullOrEmpty(logPath))
    {
        builder.Logging.AddProvider(new FileLoggerProvider(logPath));
    }
}

// Enable static web assets for non-Development environments (e.g., Test)
// Required for E2E tests which run in Test environment
if (!builder.Environment.IsDevelopment())
{
    builder.WebHost.UseStaticWebAssets();
}

// Configure application settings
builder.Services.Configure<WebAppSettings>(
    builder.Configuration.GetSection("AppSettings"));

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
if (!builder.Environment.IsDevelopment() && !builder.Environment.IsEnvironment("Test"))
{
    builder.Services.AddWebOptimizer(pipeline =>
    {
        // Bundle all CSS files into a single minified bundle for production
        pipeline.AddCssBundle("/css/bundle.css",
            "css/design-tokens.css",
            "css/base.css",
            "css/layout.css",
            "css/components/sidebar.css",
            "css/components/page-container.css",
            "css/components/buttons.css",
            "css/components/navigation.css",
            "css/components/loading.css",
            "css/components/forms.css",
            "css/utilities.css"
        );
    });
}

// Section cache for immediate (flicker-free) navigation rendering
builder.Services.AddSingleton<SectionCache>();

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

// Configure HTTP client for API with resilience policies
var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5029";
builder.Services.AddHttpClient<TechHubApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
})
.AddStandardResilienceHandler(options =>
{
    // Retry configuration: 2 attempts with exponential backoff starting at 1 second
    options.Retry.MaxRetryAttempts = 2;
    options.Retry.BackoffType = Polly.DelayBackoffType.Exponential;
    options.Retry.UseJitter = true;
    options.Retry.Delay = TimeSpan.FromSeconds(1);  // Base delay before exponential backoff

    // Circuit breaker: open after 50% failures in 30 seconds (min 8 requests)
    options.CircuitBreaker.FailureRatio = 0.5;
    options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(30);
    options.CircuitBreaker.MinimumThroughput = 8;  // Increased to avoid premature triggering
    options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(10);

    // Total timeout for the entire request (including retries)
    options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(60);
});

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
if (!app.Environment.IsDevelopment() && !app.Environment.IsEnvironment("Test"))
{
    app.UseWebOptimizer();
}

// Configure static files with environment-appropriate caching
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        var path = ctx.File.PhysicalPath ?? ctx.Context.Request.Path.Value ?? string.Empty;
        var isDevelopment = app.Environment.IsDevelopment();

        // Images, fonts, and other media
        if (path.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith(".webp", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith(".svg", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith(".ico", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith(".woff", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith(".woff2", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith(".eot", StringComparison.OrdinalIgnoreCase))
        {
            // Cache images and fonts aggressively (1 year) in both dev and production
            // Images don't change frequently and should be cached to avoid unnecessary network requests
            ctx.Context.Response.Headers.CacheControl = "public,max-age=31536000,immutable";
        }
        // CSS and JS files
        else if (path.EndsWith(".css", StringComparison.OrdinalIgnoreCase) ||
                 path.EndsWith(".js", StringComparison.OrdinalIgnoreCase))
        {
            // Development: no-cache (always validate), Production: 1 year with versioning
            ctx.Context.Response.Headers.CacheControl = isDevelopment
                ? "no-cache"
                : "public,max-age=31536000,immutable";
        }
        else
        {
            // Default: 1 hour
            ctx.Context.Response.Headers.CacheControl = "public,max-age=3600";
        }
    }
});

app.UseAntiforgery();

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

// Health check endpoint for monitoring and E2E test server readiness
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTimeOffset.UtcNow }))
    .ExcludeFromDescription();

await app.RunAsync();
