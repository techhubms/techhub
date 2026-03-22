using TechHub.Api.Services;
using TechHub.Core.Interfaces;

namespace TechHub.Api.Endpoints;

/// <summary>
/// Admin API endpoints for content processing management.
/// Accessible only from the Web front-end which sits behind cookie authentication.
/// Additional network-level security is provided by the Container Apps private endpoint.
/// </summary>
public static class AdminEndpoints
{
    public static void MapAdminEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/admin")
            .WithTags("Admin")
            .WithSummary("Admin operations (protected by network isolation + web cookie auth)");

        // POST /api/admin/processing/trigger
        group.MapPost("/processing/trigger", TriggerProcessingAsync)
            .WithName("TriggerContentProcessing")
            .WithSummary("Trigger an immediate content processing run");

        // GET /api/admin/processing/jobs
        group.MapGet("/processing/jobs", GetJobsAsync)
            .WithName("GetProcessingJobs")
            .WithSummary("Get recent content processing job history");

        // GET /api/admin/processing/jobs/{id}
        group.MapGet("/processing/jobs/{id:long}", GetJobByIdAsync)
            .WithName("GetProcessingJobById")
            .WithSummary("Get a specific content processing job by ID");
    }

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
}
