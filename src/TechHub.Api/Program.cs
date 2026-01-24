using TechHub.Api.Endpoints;
using TechHub.Api.Middleware;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Logging;
using TechHub.Core.Services;
using TechHub.Infrastructure.Repositories;
using TechHub.Infrastructure.Services;
using TechHub.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults (OpenTelemetry, service discovery, resilience, health checks)
builder.AddServiceDefaults();

// Log environment during startup for verification
using (var loggerFactory = LoggerFactory.Create(b => b.AddConsole()))
{
    var logger = loggerFactory.CreateLogger("Startup");
    logger.LogInformation("🚀 TechHub.Api starting in {Environment} environment", builder.Environment.EnvironmentName);
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

// Note: OpenTelemetry is configured by AddServiceDefaults()

// Add memory caching
builder.Services.AddMemoryCache();

// Configure AppSettings from appsettings.json
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Configure FilteringOptions from appsettings.json
builder.Services.Configure<FilteringOptions>(builder.Configuration.GetSection("AppSettings:Filtering"));

// Register repositories and services
builder.Services.AddSingleton<IMarkdownService, MarkdownService>();
builder.Services.AddSingleton<ISectionRepository, ConfigurationBasedSectionRepository>();
builder.Services.AddTransient<IContentRepository, FileBasedContentRepository>();
builder.Services.AddSingleton<IRssService, RssService>();
builder.Services.AddSingleton<ISectionMappingService, SectionMappingService>();

// Register filtering services
builder.Services.AddSingleton<ITagCloudService, TagCloudService>();
builder.Services.AddSingleton<ITagMatchingService, TagMatchingService>();

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

// Configure the HTTP request pipeline

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

// Map API endpoints
app.MapSectionsEndpoints();
app.MapContentEndpoints();
app.MapCustomPagesEndpoints();
app.MapRssEndpoints();
app.MapTagEndpoints();

// Map Aspire default health check endpoints (/health and /alive)
app.MapDefaultEndpoints();

app.Run();
