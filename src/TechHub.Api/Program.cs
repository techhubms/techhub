using System.Data;
using TechHub.Api.Endpoints;
using TechHub.Api.Middleware;
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

// Note: OpenTelemetry is configured by AddServiceDefaults()

// Add memory caching
builder.Services.AddMemoryCache();

// Configure AppSettings from appsettings.json
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Configure FilteringOptions from appsettings.json
builder.Services.Configure<FilteringOptions>(builder.Configuration.GetSection("AppSettings:Filtering"));

// Configure DatabaseOptions from appsettings.json
builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection("Database"));

// Configure ContentSyncOptions from appsettings.json
builder.Services.Configure<ContentSyncOptions>(builder.Configuration.GetSection("ContentSync"));
builder.Services.Configure<ContentOptions>(builder.Configuration.GetSection("AppSettings:Content"));

// Register database connection and repository based on configured provider
var databaseProvider = builder.Configuration["Database:Provider"] ?? "SQLite";
var connectionString = builder.Configuration["Database:ConnectionString"] ?? "Data Source=techhub.db";

if (databaseProvider.Equals("SQLite", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddSingleton<ISqlDialect, SqliteDialect>();
    
    // Register connection factory for thread-safe concurrent operations
    builder.Services.AddSingleton<IDbConnectionFactory>(_ => new SqliteConnectionFactory(connectionString));
    
    // Register singleton connection for services that don't need concurrency
    builder.Services.AddSingleton<IDbConnection>(sp => sp.GetRequiredService<IDbConnectionFactory>().CreateConnection());
    
    builder.Services.AddSingleton<IContentRepository, SqliteContentRepository>();
}
else if (databaseProvider.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddSingleton<ISqlDialect, PostgresDialect>();
    
    // Register connection factory for thread-safe concurrent operations
    builder.Services.AddSingleton<IDbConnectionFactory>(_ => new PostgresConnectionFactory(connectionString));
    
    // Register singleton connection for services that don't need concurrency
    builder.Services.AddSingleton<IDbConnection>(sp => sp.GetRequiredService<IDbConnectionFactory>().CreateConnection());
    
    // TODO: Create PostgresContentRepository
    throw new NotImplementedException("PostgreSQL repository not yet implemented");
}
else
{
    throw new InvalidOperationException($"Unsupported database provider: {databaseProvider}");
}

// Register services
builder.Services.AddSingleton<IMarkdownService, MarkdownService>();
builder.Services.AddSingleton<ISectionRepository, ConfigurationBasedSectionRepository>();
builder.Services.AddSingleton<IRssService, RssService>();
builder.Services.AddSingleton<IContentSyncService, ContentSyncService>();
builder.Services.AddSingleton<MigrationRunner>();

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
app.MapSectionsEndpoints();
app.MapContentEndpoints();
app.MapCustomPagesEndpoints();
app.MapRssEndpoints();
app.MapTagEndpoints();

// Map Aspire default health check endpoints (/health and /alive)
app.MapDefaultEndpoints();

// Run database migrations
var migrationRunner = app.Services.GetRequiredService<MigrationRunner>();
await migrationRunner.RunMigrationsAsync();
app.Logger.LogInformation("✅ Database migrations completed");

// Synchronize content from markdown files to database
var contentSyncService = app.Services.GetRequiredService<IContentSyncService>();
var syncResult = await contentSyncService.SyncAsync();
app.Logger.LogInformation("✅ Content sync: {Added} added, {Updated} updated, {Deleted} deleted, {Unchanged} unchanged ({Duration}ms)",
    syncResult.Added, syncResult.Updated, syncResult.Deleted, syncResult.Unchanged, syncResult.Duration.TotalMilliseconds);

// Log database record counts for verification
await LogDatabaseRecordCountsAsync(app);

// Preload all content into cache BEFORE starting server
// This blocks server startup until cache is warm, ensuring blazingly fast first request
var contentRepository = app.Services.GetRequiredService<IContentRepository>();
var loadedItems = await contentRepository.InitializeAsync();
app.Logger.LogInformation("✅ Repository initialized with {Count} content items", loadedItems.Count);

await app.RunAsync();

// Logs record counts for all database tables for verification and debugging.
static async Task LogDatabaseRecordCountsAsync(WebApplication app)
{
    using var connection = app.Services.GetRequiredService<IDbConnectionFactory>().CreateConnection();
    
    var tables = new (string TableName, string Description)[]
    {
        ("content_items", "content items"),
        ("content_tags", "tags"),
        ("content_sections", "section mappings"),
        ("content_plans", "plan mappings"),
        ("content_tags_expanded", "expanded tags"),
        ("collections", "collections")
    };

    app.Logger.LogInformation("📊 Database record counts:");
    
    foreach (var (tableName, description) in tables)
    {
        try
        {
            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT COUNT(*) FROM {tableName}";
            var count = Convert.ToInt32(command.ExecuteScalar(), System.Globalization.CultureInfo.InvariantCulture);
            app.Logger.LogInformation("   - {TableName}: {Count} {Description}", tableName, count, description);
        }
        catch (Exception)
        {
            // Table might not exist (e.g., before migrations run) - skip logging
        }
    }
}
