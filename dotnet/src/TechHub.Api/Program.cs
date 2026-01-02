using TechHub.Api.Extensions;
using TechHub.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddOpenApi();

// Add OpenTelemetry tracing
builder.Services.AddTelemetry(builder.Configuration, builder.Environment);

// Add CORS
builder.Services.AddCors(options =>
{
    var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
        ?? ["https://localhost:7001"];

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

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTimeOffset.UtcNow }))
    .WithName("HealthCheck");

// Sample endpoint (will be replaced by actual endpoints)
app.MapGet("/api/ping", () => Results.Ok(new { message = "pong", timestamp = DateTimeOffset.UtcNow }))
    .WithName("Ping");

app.Run();
