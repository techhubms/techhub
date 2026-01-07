using TechHub.Web.Components;
using TechHub.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Enable static web assets for non-Development environments (e.g., Test)
// Required for E2E tests which run in Test environment
if (!builder.Environment.IsDevelopment())
{
    builder.WebHost.UseStaticWebAssets();
}

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Section cache for immediate (flicker-free) navigation rendering
builder.Services.AddSingleton<SectionCache>();

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
    
    try
    {
        var sections = await apiClient.GetAllSectionsAsync();
        sectionCache.Initialize(sections?.ToList() ?? []);
    }
    catch (Exception ex)
    {
        // Log error but continue - navigation will show without sections
        app.Logger.LogError(ex, "Failed to pre-load sections at startup");
    }
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
            // Development: 1 hour, Production: 1 year
            ctx.Context.Response.Headers.CacheControl = isDevelopment
                ? "public,max-age=3600"
                : "public,max-age=31536000,immutable";
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

await app.RunAsync();
