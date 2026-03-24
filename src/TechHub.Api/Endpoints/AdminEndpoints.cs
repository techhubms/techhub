using TechHub.Api.Services;
using TechHub.Core.Interfaces;
using TechHub.Core.Logging;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Api.Endpoints;

/// <summary>
/// Admin API endpoints for content processing management and RSS feed configuration.
/// Protected by Azure AD bearer token authentication. The Web front-end forwards
/// the authenticated user's token after OIDC sign-in.
/// When Azure AD is not configured (local dev), the AdminOnly policy allows all requests.
/// </summary>
public static class AdminEndpoints
{
    public static void MapAdminEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/admin")
            .WithTags("Admin")
            .RequireAuthorization("AdminOnly");

        // ── Processing jobs ──────────────────────────────────────────────────

        group.MapPost("/processing/trigger", TriggerProcessingAsync)
            .WithName("TriggerContentProcessing")
            .WithSummary("Trigger an immediate content processing run");

        group.MapGet("/processing/jobs", GetJobsAsync)
            .WithName("GetProcessingJobs")
            .WithSummary("Get recent content processing job history");

        group.MapGet("/processing/jobs/{id:long}", GetJobByIdAsync)
            .WithName("GetProcessingJobById")
            .WithSummary("Get a specific content processing job by ID");

        // ── RSS Feed configuration ───────────────────────────────────────────

        group.MapGet("/feeds", GetFeedsAsync)
            .WithName("GetRssFeedConfigs")
            .WithSummary("Get all RSS feed configurations");

        group.MapGet("/feeds/{id:long}", GetFeedByIdAsync)
            .WithName("GetRssFeedConfigById")
            .WithSummary("Get a specific RSS feed configuration by ID");

        group.MapPost("/feeds", CreateFeedAsync)
            .WithName("CreateRssFeedConfig")
            .WithSummary("Create a new RSS feed configuration");

        group.MapPut("/feeds/{id:long}", UpdateFeedAsync)
            .WithName("UpdateRssFeedConfig")
            .WithSummary("Update an existing RSS feed configuration");

        group.MapDelete("/feeds/{id:long}", DeleteFeedAsync)
            .WithName("DeleteRssFeedConfig")
            .WithSummary("Delete an RSS feed configuration");
    }

    // ── Processing handlers ──────────────────────────────────────────────────

    private static IResult TriggerProcessingAsync(
        ContentProcessingBackgroundService backgroundService)
    {
        backgroundService.TriggerImmediateRun();
        return Results.Accepted("/api/admin/processing/jobs", new { message = "Processing run triggered" });
    }

    private static async Task<IResult> GetJobsAsync(
        IContentProcessingJobRepository jobRepo,
        CancellationToken ct,
        int count = 20)
    {
        var jobs = await jobRepo.GetRecentAsync(Math.Clamp(count, 1, 100), ct);
        return Results.Ok(jobs);
    }

    private static async Task<IResult> GetJobByIdAsync(
        long id,
        IContentProcessingJobRepository jobRepo,
        CancellationToken ct)
    {
        var job = await jobRepo.GetByIdAsync(id, ct);
        return job is null ? Results.NotFound() : Results.Ok(job);
    }

    // ── RSS Feed handlers ────────────────────────────────────────────────────

    private static async Task<IResult> GetFeedsAsync(
        IRssFeedConfigRepository feedRepo,
        CancellationToken ct)
    {
        var feeds = await feedRepo.GetAllAsync(ct);
        return Results.Ok(feeds);
    }

    private static async Task<IResult> GetFeedByIdAsync(
        long id,
        IRssFeedConfigRepository feedRepo,
        CancellationToken ct)
    {
        var feed = await feedRepo.GetByIdAsync(id, ct);
        return feed is null ? Results.NotFound() : Results.Ok(feed);
    }

    private static async Task<IResult> CreateFeedAsync(
        FeedConfigRequest request,
        IRssFeedConfigRepository feedRepo,
        CancellationToken ct)
    {
        var name = request.Name?.Trim().Sanitize() ?? string.Empty;
        var url = request.Url?.Trim().Sanitize() ?? string.Empty;
        var outputDir = request.OutputDir?.Trim().Sanitize() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(outputDir))
        {
            return Results.BadRequest("Name, Url and OutputDir are required.");
        }

        if (!Uri.TryCreate(url, UriKind.Absolute, out var parsedUri)
            || (parsedUri.Scheme != "http" && parsedUri.Scheme != "https"))
        {
            return Results.BadRequest("Url must be a valid absolute HTTP(S) URL.");
        }

        var feed = new FeedConfig
        {
            Name = name,
            Url = url,
            OutputDir = outputDir,
            Enabled = request.Enabled
        };

        var id = await feedRepo.CreateAsync(feed, ct);
        var created = new FeedConfig
        {
            Id = id,
            Name = name,
            Url = url,
            OutputDir = outputDir,
            Enabled = request.Enabled
        };
        return Results.Created($"/api/admin/feeds/{id}", created);
    }

    private static async Task<IResult> UpdateFeedAsync(
        long id,
        FeedConfigRequest request,
        IRssFeedConfigRepository feedRepo,
        CancellationToken ct)
    {
        var existing = await feedRepo.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return Results.NotFound();
        }

        var name = request.Name?.Trim().Sanitize() ?? string.Empty;
        var url = request.Url?.Trim().Sanitize() ?? string.Empty;
        var outputDir = request.OutputDir?.Trim().Sanitize() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(outputDir))
        {
            return Results.BadRequest("Name, Url and OutputDir are required.");
        }

        if (!Uri.TryCreate(url, UriKind.Absolute, out var parsedUri)
            || (parsedUri.Scheme != "http" && parsedUri.Scheme != "https"))
        {
            return Results.BadRequest("Url must be a valid absolute HTTP(S) URL.");
        }

        var feed = new FeedConfig
        {
            Id = id,
            Name = name,
            Url = url,
            OutputDir = outputDir,
            Enabled = request.Enabled
        };

        var updated = await feedRepo.UpdateAsync(feed, ct);
        return updated ? Results.Ok(feed) : Results.NotFound();
    }

    private static async Task<IResult> DeleteFeedAsync(
        long id,
        IRssFeedConfigRepository feedRepo,
        CancellationToken ct)
    {
        var deleted = await feedRepo.DeleteAsync(id, ct);
        return deleted ? Results.NoContent() : Results.NotFound();
    }
}

/// <summary>DTO for creating/updating RSS feed configurations.</summary>
public sealed class FeedConfigRequest
{
    public string? Name { get; init; }
    public string? Url { get; init; }
    public string? OutputDir { get; init; }
    public bool Enabled { get; init; } = true;
}
