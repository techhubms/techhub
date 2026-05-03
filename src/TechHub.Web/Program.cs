using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Polly;
using TechHub.Core.Logging;
using TechHub.Core.Validation;
using TechHub.ServiceDefaults;
using TechHub.Web.Components;
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
// Skipped in production (AppSettings:SkipFileLogging = true) — containers log to stdout/stderr.
var skipFileLogging = builder.Configuration.GetValue<bool>("AppSettings:SkipFileLogging");
if (!skipFileLogging)
{
    // Use TECHHUB_TMP if set (devcontainer), otherwise the system temp directory
    var tmpDir = Environment.GetEnvironmentVariable("TECHHUB_TMP") ?? Path.GetTempPath();
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

// ─── Admin Authentication (Azure AD / Microsoft Entra ID) ────────────────────
// Uses Microsoft.Identity.Web (OIDC + cookie session). No password stored in config.
// Required appsettings / environment variables:
//   AzureAd:TenantId     — Directory (tenant) ID from the Entra app registration
//   AzureAd:ClientId     — Application (client) ID from the Entra app registration
//   AzureAd:ClientSecret — Client secret (set as a deployment secret, never in source)
// The admin area is accessible to any user assigned to the enterprise application.
// To restrict to specific users/groups, configure assignment required + add group claims
// to the app registration in Entra ID.
var azureAdClientId = builder.Configuration["AzureAd:ClientId"];
var isAzureAdConfigured = !string.IsNullOrEmpty(azureAdClientId);

if (isAzureAdConfigured)
{
    // Acquire a proper access token (not an id_token) for calling the API.
    // Requires an "Expose an API" scope on the Entra app registration
    // (e.g. api://<client-id>/Admin.Access) configured in AzureAd:Scopes.
    var apiScopes = builder.Configuration.GetValue<string>("AzureAd:Scopes")?.Split(' ', StringSplitOptions.RemoveEmptyEntries)
        ?? [];

    builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
        .EnableTokenAcquisitionToCallDownstreamApi(apiScopes)
        .AddInMemoryTokenCaches();

    // Always show account picker so users can switch accounts.
    // PostConfigure ensures Microsoft.Identity.Web's event handlers are already registered;
    // we chain ours before calling the existing handler to preserve token redemption.
    builder.Services.PostConfigure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
        var previousHandler = options.Events.OnRedirectToIdentityProvider;
        options.Events.OnRedirectToIdentityProvider = async context =>
        {
            context.ProtocolMessage.Prompt = "select_account";
            await previousHandler(context);
        };
    });
}
else
{
    // When Azure AD is not configured (local dev without Entra), register cookie
    // auth so the auth middleware doesn't throw. Admin pages simply won't work.
    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options => options.LoginPath = "/admin/login");
}

builder.Services.AddAuthorization();

// AddMicrosoftIdentityUI registers the /MicrosoftIdentity/Account/* controller
// which requires the OpenIdConnect scheme — only register when Azure AD is configured.
var mvcBuilder = builder.Services.AddControllersWithViews();
if (isAzureAdConfigured)
{
    mvcBuilder.AddMicrosoftIdentityUI();
}

// Section cache for immediate (flicker-free) navigation rendering
builder.Services.AddSingleton<SectionCache>();

// Background service to periodically refresh the SectionCache from the API
builder.Services.AddHostedService<SectionCacheRefreshService>();

// Hero banner cache for immediate rendering without per-request API calls
builder.Services.AddSingleton<HeroBannerCache>();

// Background service to periodically refresh the HeroBannerCache from the API
builder.Services.AddHostedService<HeroBannerCacheRefreshService>();

// HTTP context accessor for reading cookies during SSR (e.g. sidebar collapsed state)
builder.Services.AddHttpContextAccessor();

// Favicon service for base64-encoded favicon (eliminates HTTP requests)
builder.Services.AddSingleton<FaviconService>();

// Error handling service for centralized exception management
builder.Services.AddScoped<ErrorService>();

// Circuit-scoped cache for infinite scroll state (survives enhanced navigations for back-button support)
builder.Services.AddScoped<ContentGridStateCache>();

// Domain-based branding (tech.xebia.ms vs tech.hub.ms)
builder.Services.AddScoped<BrandingService>();

// Configure SignalR for Blazor Server with increased timeouts for reliability
builder.Services.AddSignalR(options =>
{
    // Increase timeouts to prevent premature disconnections during initialization,
    // when switching between apps on mobile/tablet, or when browser throttles
    // background tabs (timers/network processing delayed in unfocused tabs).
    //
    // Relationship: each side's timeout must be > 2× the other side's keep-alive.
    //   Server KeepAliveInterval (15s) × 2 < Client serverTimeout (120s) ✓
    //   Client keepAliveInterval (15s) × 2 < Server ClientTimeout (120s) ✓
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(120);
    options.HandshakeTimeout = TimeSpan.FromSeconds(30);
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
    // Increase max message size to handle large prerendered content during hydration
    // Default is 32KB, but with many content items the state can exceed this
    options.MaximumReceiveMessageSize = 256 * 1024; // 256KB
});

// Increase disconnected circuit retention so mobile users switching apps can reconnect
builder.Services.AddOptions<Microsoft.AspNetCore.Components.Server.CircuitOptions>()
    .Configure(options =>
    {
        options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(5);
        // Surface detailed errors in Development so unhandled circuit exceptions are visible
        if (builder.Environment.IsDevelopment())
        {
            options.DetailedErrors = true;
        }
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

// Register delegating handler for forwarding auth tokens to admin API endpoints
builder.Services.AddTransient<AdminTokenDelegatingHandler>();

builder.Services.AddHttpClient<TechHubApiClient>((sp, client) =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    // Generous timeout: some admin operations (URL processing) involve AI calls that take 30-60s.
    client.Timeout = TimeSpan.FromMinutes(3);
})
.AddHttpMessageHandler<AdminTokenDelegatingHandler>()
.ConfigurePrimaryHttpMessageHandler(sp =>
{
    var handler = new SocketsHttpHandler();

    // Only bypass SSL validation in Development (Docker uses self-signed certs)
    var env = sp.GetRequiredService<IHostEnvironment>();
    if (env.IsDevelopment())
    {
#pragma warning disable CA5359 // Required for Docker inter-container HTTPS communication in development
        handler.SslOptions = new System.Net.Security.SslClientAuthenticationOptions
        {
            RemoteCertificateValidationCallback = (_, _, _, _) => true
        };
#pragma warning restore CA5359
    }

    return handler;
})
.AddResilienceHandler("web-api-retry", (pipeline, context) =>
{
    var logger = context.ServiceProvider
        .GetRequiredService<ILoggerFactory>()
        .CreateLogger("TechHub.Web.Resilience");

    // 3 fast retries for transient failures (dropped connections, brief API restarts).
    // Exponential backoff: 50 → 100 → 200ms with jitter (~350ms worst case total).
    // Retries are restricted to idempotent methods (GET, HEAD) to avoid replaying
    // non-idempotent admin operations (POST, DELETE) that could cause duplicate side effects.
    // OnRetry logs at Debug so intermediate attempts stay out of Warning/Error channels.
    // The final failure (after all retries) is logged at Error by TechHubApiClient.catch — exactly once.
    pipeline.AddRetry(new Microsoft.Extensions.Http.Resilience.HttpRetryStrategyOptions
    {
        MaxRetryAttempts = 3,
        Delay = TimeSpan.FromMilliseconds(50),
        BackoffType = DelayBackoffType.Exponential,
        UseJitter = true,
        ShouldHandle = args =>
        {
            // Only retry idempotent HTTP methods to prevent duplicate side-effects on
            // admin POST/DELETE endpoints (processing triggers, cache invalidation, etc.).
            var method = args.Outcome.Result?.RequestMessage?.Method;
            var isIdempotent = method == HttpMethod.Get || method == HttpMethod.Head;
            return new ValueTask<bool>(isIdempotent && HttpClientResiliencePredicates.IsTransient(args.Outcome));
        },
        OnRetry = args =>
        {
            logger.LogDebug(
                args.Outcome.Exception,
                "Web→API transient failure (attempt {Attempt}/3), retrying in {Delay}ms",
                args.AttemptNumber + 1,
                args.RetryDelay.TotalMilliseconds);
            return ValueTask.CompletedTask;
        },
    });
});
// Register interface for dependency injection (scoped to match HttpClient lifetime)
builder.Services.AddScoped<ITechHubApiClient>(sp => sp.GetRequiredService<TechHubApiClient>());

var app = builder.Build();

// Pre-load sections into cache at startup for flicker-free navigation
using (var scope = app.Services.CreateScope())
{
    var apiClient = scope.ServiceProvider.GetRequiredService<TechHubApiClient>();
    var sectionCache = scope.ServiceProvider.GetRequiredService<SectionCache>();
    var heroBannerCache = scope.ServiceProvider.GetRequiredService<HeroBannerCache>();

    var sections = await apiClient.GetAllSectionsAsync();
    sectionCache.Initialize(sections?.ToList() ?? []);

    try
    {
        var heroBannerData = await apiClient.GetHeroBannerDataAsync();
        heroBannerCache.Initialize(heroBannerData);
    }
    catch (HttpRequestException ex)
    {
        // Non-fatal: banner won't show until the background service refreshes the cache
        var startupLog = scope.ServiceProvider.GetRequiredService<ILogger<HeroBannerCache>>();
        startupLog.LogWarning(ex, "Failed to pre-load HeroBannerCache at startup, will retry on next background refresh");
        heroBannerCache.Initialize(null);
    }
    catch (TaskCanceledException ex)
    {
        var startupLog = scope.ServiceProvider.GetRequiredService<ILogger<HeroBannerCache>>();
        startupLog.LogWarning(ex, "Failed to pre-load HeroBannerCache at startup (timeout), will retry on next background refresh");
        heroBannerCache.Initialize(null);
    }
    catch (JsonException ex)
    {
        var startupLog = scope.ServiceProvider.GetRequiredService<ILogger<HeroBannerCache>>();
        startupLog.LogWarning(ex, "Failed to deserialize HeroBannerCache data at startup, will retry on next background refresh");
        heroBannerCache.Initialize(null);
    }
}

// Trust X-Forwarded-Proto and X-Forwarded-For from the Azure Container Apps reverse proxy.
// Without this, ASP.NET Core sees the inner HTTP request and builds OIDC redirect URIs
// with http:// instead of https://, causing AADSTS50011 redirect URI mismatch errors.
// KnownIPNetworks/KnownProxies are cleared because Azure Container Apps proxies from
// internal IPs that are not in the default loopback-only trusted list.
var forwardedHeadersOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
};
forwardedHeadersOptions.KnownIPNetworks.Clear();
forwardedHeadersOptions.KnownProxies.Clear();
app.UseForwardedHeaders(forwardedHeadersOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Security headers (XSS, clickjacking, MIME sniffing protection)
app.UseSecurityHeaders();

// ── Step 1: Fix URLs (301 redirects) ─────────────────────────────────────────
// Redirect subdomain shortcuts (e.g., ghc.xebia.ms -> /github-copilot, www.tech.hub.ms -> tech.hub.ms)
app.UseSubdomainRedirects();
// Unified URL normalization: strips .html, strips YYYY-MM-DD- date prefixes, resolves legacy
// single-segment slugs via the API — all in one pass, at most one 301 redirect per request.
app.UseUrlNormalization();
// Redirect HTTP → HTTPS
app.UseHttpsRedirection();

// ── Step 2: Serve static files before validators ──────────────────────────────
// Static files (CSS, JS, images, favicon.ico) short-circuit here so they never
// reach the route validators below. Must be before StatusCodePages so that a
// missing static file falls through to validation normally.
app.UseStaticFilesCaching();
var contentTypeProvider = new FileExtensionContentTypeProvider();
contentTypeProvider.Mappings[".jxl"] = "image/jxl";
app.UseStaticFiles(new StaticFileOptions { ContentTypeProvider = contentTypeProvider });

// ── Step 3a: Rewrite HEAD → GET ───────────────────────────────────────────────
// MapRazorComponents only registers GET endpoints. HEAD requests to valid Blazor
// routes return 405 without this rewrite. We convert HEAD to GET so routing
// succeeds; Kestrel suppresses the response body at the transport level since
// the original TCP request was HEAD.
app.Use(async (context, next) =>
{
    if (HttpMethods.IsHead(context.Request.Method))
    {
        context.Request.Method = HttpMethods.Get;
        var originalBody = context.Response.Body;
        context.Response.Body = Stream.Null;
        try
        {
            await next();
        }
        finally
        {
            context.Response.Body = originalBody;
            context.Request.Method = HttpMethods.Head;
        }
    }
    else
    {
        await next();
    }
});

app.UseAuthentication();
app.UseAuthorization();
app.UseAdminTokenValidation();
app.UseAntiforgery();

// ── Step 4: Validate URL structure ───────────────────────────────────────────
// Reject segments that contain dots, digits at start, or other characters that
// can never match a Blazor route (e.g. /config.json, /2024-probe, /ADMIN).
// Paths starting with _ (Blazor internals: /_blazor, /_framework) are allowed.
app.UseInvalidRouteSegmentFilter();

// ── Step 5: Validate section/collection existence ────────────────────────────
// Done in MainLayout.razor (runs before @Body renders, covers both HTTP requests
// and Blazor soft-navigation). No middleware needed here.

// MapStaticAssets for optimized static assets (fingerprinting, compression, ETags)
app.MapStaticAssets();

app.MapControllers();

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
    var xml = await apiClient.GetCollectionRssFeedAsync("roundups", "all", ct);
    return Results.Content(xml, "application/rss+xml; charset=utf-8");
})
.WithName("GetRoundupsRssFeed")
.WithSummary("RSS feed for roundups collection")
.ExcludeFromDescription();

app.MapGet("/{sectionName}/feed.xml", async (string sectionName, TechHubApiClient apiClient, CancellationToken ct) =>
{
    if (!RouteParameterValidator.IsValidNameSegment(sectionName))
    {
        return Results.BadRequest("Invalid section name format.");
    }

    var xml = await apiClient.GetSectionRssFeedAsync(sectionName, ct);
    return Results.Content(xml, "application/rss+xml; charset=utf-8");
})
.WithName("GetSectionRssFeed")
.WithSummary("RSS feed for a section")
.ExcludeFromDescription();

// Map Aspire default health check endpoints (/health and /alive)
app.MapDefaultEndpoints();

// Sitemap proxy — fetches from API and serves from the canonical web domain
app.MapGet("/sitemap.xml", async (TechHubApiClient apiClient, CancellationToken ct) =>
{
    var xml = await apiClient.GetSitemapAsync(ct);
    return Results.Content(xml, "application/xml; charset=utf-8");
})
.WithName("GetSitemap")
.WithSummary("XML sitemap")
.ExcludeFromDescription();

// ─── Admin Logout Endpoint ────────────────────────────────────────────────────
// Microsoft.Identity.Web provides /MicrosoftIdentity/Account/SignIn and /SignOut
// built-in. We expose /admin/logout as a stable alias for the logout button.
// Signs out of OIDC first (issues the end_session request to Azure AD), then
// clears the local cookie. The OIDC sign-out redirects to "/" on completion.
// Uses POST to prevent CSRF / prefetch-triggered logout (antiforgery validated).
app.MapPost("/admin/logout", async (HttpContext context) =>
{
    // OIDC sign-out must happen first — it issues the end_session_endpoint redirect.
    // Cookie sign-out is handled by the OIDC post-logout flow automatically.
    await context.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme,
        new AuthenticationProperties { RedirectUri = "/" });
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
})
.WithName("AdminLogout")
.ExcludeFromDescription();

await app.RunAsync();
