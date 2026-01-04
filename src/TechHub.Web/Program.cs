using TechHub.Web.Components;
using TechHub.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure HTTP client for API with resilience policies
var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5029";
builder.Services.AddHttpClient<TechHubApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
})
.AddStandardResilienceHandler(options =>
{
    // Retry configuration: 3 attempts with exponential backoff
    options.Retry.MaxRetryAttempts = 3;
    options.Retry.BackoffType = Polly.DelayBackoffType.Exponential;
    options.Retry.UseJitter = true;
    
    // Circuit breaker: open after 5 failures in 30 seconds
    options.CircuitBreaker.FailureRatio = 0.5;
    options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(30);
    options.CircuitBreaker.MinimumThroughput = 5;
    options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(5);
    
    // Total timeout for the entire request (including retries)
    options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(60);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

// Configure static files with browser caching
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Cache static assets for 1 year (images, fonts, etc.)
        // CSS and JS files should use cache busting via MapStaticAssets()
        var path = ctx.File.PhysicalPath ?? ctx.Context.Request.Path.Value ?? string.Empty;
        
        // Images, fonts, and other media - cache for 1 year
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
            // Cache for 1 year, can be cached by browsers and CDNs
            ctx.Context.Response.Headers.CacheControl = "public,max-age=31536000,immutable";
        }
        // CSS and JS files - let MapStaticAssets handle versioning
        else if (path.EndsWith(".css", StringComparison.OrdinalIgnoreCase) ||
                 path.EndsWith(".js", StringComparison.OrdinalIgnoreCase))
        {
            // Cache for 1 year with versioning (MapStaticAssets adds fingerprint)
            ctx.Context.Response.Headers.CacheControl = "public,max-age=31536000,immutable";
        }
        else
        {
            // Default: cache for 1 hour
            ctx.Context.Response.Headers.CacheControl = "public,max-age=3600";
        }
    }
});

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
