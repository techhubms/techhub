using TechHub.Api.Services;
using TechHub.Core.Interfaces;
using TechHub.Core.Logging;
using TechHub.Core.Models.Admin;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Services;

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

        group.MapPost("/roundup/trigger", TriggerRoundupAsync)
            .WithName("TriggerRoundupGeneration")
            .WithSummary("Trigger an immediate roundup generation run");

        group.MapPost("/content-fixer/trigger", TriggerContentFixerAsync)
            .WithName("TriggerContentFixer")
            .WithSummary("Trigger a bulk content fix run (tags, authors, markdown)");

        group.MapPost("/processing/cancel", CancelRunningJobAsync)
            .WithName("CancelRunningJob")
            .WithSummary("Cancel the currently running background job");

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

        // ── Database statistics ──────────────────────────────────────────────

        group.MapGet("/statistics", GetStatisticsAsync)
            .WithName("GetDatabaseStatistics")
            .WithSummary("Get database statistics for the admin dashboard");

        // ── Processed URLs ───────────────────────────────────────────────────

        group.MapGet("/processed-urls", GetProcessedUrlsAsync)
            .WithName("GetProcessedUrls")
            .WithSummary("Get a paginated list of processed URLs with optional filters");

        group.MapDelete("/processed-urls", DeleteProcessedUrlAsync)
            .WithName("DeleteProcessedUrl")
            .WithSummary("Delete a processed URL and its associated content item so it can be retried");

        group.MapDelete("/processed-urls/failed", DeleteAllFailedUrlsAsync)
            .WithName("DeleteAllFailedUrls")
            .WithSummary("Delete all failed processed URL records");

        // ── Custom page data ─────────────────────────────────────────────────

        group.MapGet("/custom-pages", GetCustomPagesAsync)
            .WithName("GetCustomPages")
            .WithSummary("List all custom page entries (key, description, last updated)");

        group.MapGet("/custom-pages/{key}", GetCustomPageByKeyAsync)
            .WithName("GetCustomPageByKey")
            .WithSummary("Get the raw JSON for a custom page by key");

        group.MapPut("/custom-pages/{key}", UpdateCustomPageAsync)
            .WithName("UpdateCustomPage")
            .WithSummary("Update the raw JSON for a custom page");

        // ── Content item editing ─────────────────────────────────────────────

        group.MapGet("/content-items/ai-metadata", GetContentItemAiMetadataAsync)
            .WithName("GetContentItemAiMetadata")
            .WithSummary("Get the ai_metadata JSON for a content item by primary key");

        group.MapPut("/content-items/ai-metadata", UpdateContentItemAiMetadataAsync)
            .WithName("UpdateContentItemAiMetadata")
            .WithSummary("Update the ai_metadata JSON for a content item by primary key");

        group.MapGet("/content-items/edit-data", GetContentItemEditDataAsync)
            .WithName("GetContentItemEditData")
            .WithSummary("Get all editable fields for a content item by primary key");

        group.MapPut("/content-items/edit-data", UpdateContentItemEditDataAsync)
            .WithName("UpdateContentItemEditData")
            .WithSummary("Update all editable fields for a content item by primary key");

        group.MapGet("/content-items", GetContentItemsPagedAsync)
            .WithName("GetContentItemsPaged")
            .WithSummary("Get a paginated list of content items with optional filters");

        group.MapDelete("/content-items", DeleteContentItemAsync)
            .WithName("DeleteContentItem")
            .WithSummary("Delete a content item by collection name and slug (cascades to processed_urls)");

        // ── Background job settings ──────────────────────────────────────────

        group.MapGet("/job-settings", GetJobSettingsAsync)
            .WithName("GetBackgroundJobSettings")
            .WithSummary("Get all background job settings");

        group.MapPut("/job-settings/{jobName}", UpdateJobSettingAsync)
            .WithName("UpdateBackgroundJobSetting")
            .WithSummary("Update the enabled state for a background job");

        // ── Cache management ─────────────────────────────────────────────────

        group.MapPost("/cache/invalidate", InvalidateCacheAsync)
            .WithName("InvalidateAllCaches")
            .WithSummary("Invalidate all server-side caches (content, sections, etc.)");

        // ── Content reviews ──────────────────────────────────────────────────

        group.MapGet("/reviews", GetReviewsAsync)
            .WithName("GetContentReviews")
            .WithSummary("Get content reviews filtered by status");

        group.MapGet("/reviews/summary", GetReviewSummaryAsync)
            .WithName("GetContentReviewSummary")
            .WithSummary("Get summary counts of pending/approved/rejected reviews");

        group.MapPost("/reviews/{id:long}/approve", ApproveReviewAsync)
            .WithName("ApproveContentReview")
            .WithSummary("Approve a single review and apply the change");

        group.MapPost("/reviews/{id:long}/reject", RejectReviewAsync)
            .WithName("RejectContentReview")
            .WithSummary("Reject a single review without applying the change");

        group.MapPost("/reviews/approve-all", ApproveAllReviewsAsync)
            .WithName("ApproveAllContentReviews")
            .WithSummary("Approve all pending reviews and apply changes");

        group.MapPost("/reviews/reject-all", RejectAllReviewsAsync)
            .WithName("RejectAllContentReviews")
            .WithSummary("Reject all pending reviews");

        group.MapPut("/reviews/{id:long}", UpdateReviewFixedValueAsync)
            .WithName("UpdateContentReviewFixedValue")
            .WithSummary("Update the fixed value of a pending review");

        // ── Content preview ──────────────────────────────────────────────────

        group.MapPost("/content-items/preview-markdown", PreviewMarkdownAsync)
            .WithName("PreviewMarkdown")
            .WithSummary("Render raw markdown to HTML for preview");
    }

    // ── Processing handlers ──────────────────────────────────────────────────

    private static IResult TriggerProcessingAsync(
        ContentProcessingBackgroundService backgroundService)
    {
        backgroundService.TriggerImmediateRun();
        return Results.Accepted("/api/admin/processing/jobs", new { message = "Processing run triggered" });
    }

    private static IResult TriggerRoundupAsync(
        RoundupGeneratorBackgroundService roundupService)
    {
        roundupService.TriggerImmediateRun();
        return Results.Accepted("/api/admin/processing/jobs", new { message = "Roundup generation triggered" });
    }

    private static IResult TriggerContentFixerAsync(
        ContentFixerBackgroundService contentFixerService)
    {
        contentFixerService.TriggerImmediateRun();
        return Results.Accepted(value: new { message = "Content fixer run triggered" });
    }

    private static IResult CancelRunningJobAsync(
        ContentProcessingBackgroundService backgroundService,
        RoundupGeneratorBackgroundService roundupService,
        ContentFixerBackgroundService contentFixerService)
    {
        var cancelled = backgroundService.CancelCurrentRun() | roundupService.CancelCurrentRun() | contentFixerService.CancelCurrentRun();
        if (!cancelled)
        {
            return Results.Ok(new { message = "No running job to cancel" });
        }

        return Results.Ok(new { message = "Cancellation requested" });
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
            Enabled = request.Enabled,
            TranscriptMandatory = request.TranscriptMandatory
        };

        var id = await feedRepo.CreateAsync(feed, ct);
        var created = new FeedConfig
        {
            Id = id,
            Name = name,
            Url = url,
            OutputDir = outputDir,
            Enabled = request.Enabled,
            TranscriptMandatory = request.TranscriptMandatory
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
            Enabled = request.Enabled,
            TranscriptMandatory = request.TranscriptMandatory
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

    // ── Statistics handlers ──────────────────────────────────────────────────

    private static async Task<IResult> GetStatisticsAsync(
        DatabaseStatisticsService statsService,
        CancellationToken ct)
    {
        var stats = await statsService.GetStatisticsAsync(ct);
        return Results.Ok(stats);
    }

    // ── Processed URL handlers ───────────────────────────────────────────────

    private static async Task<IResult> GetProcessedUrlsAsync(
        IProcessedUrlRepository repo,
        CancellationToken ct,
        int page = 1,
        int pageSize = 100,
        string? status = null,
        string? search = null,
        string? feedName = null,
        string? collectionName = null,
        long? jobId = null)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 500);
        status = status?.Trim().Sanitize();
        search = search?.Trim().Sanitize();
        feedName = feedName?.Trim().Sanitize();
        collectionName = collectionName?.Trim().Sanitize();

        // Validate status filter
        if (!string.IsNullOrEmpty(status) && status is not "succeeded" and not "skipped" and not "failed")
        {
            return Results.BadRequest("Status must be 'succeeded', 'skipped', or 'failed'.");
        }

        var offset = (page - 1) * pageSize;
        var result = await repo.GetPagedAsync(offset, pageSize, status, search, feedName, collectionName, jobId, ct);
        return Results.Ok(result);
    }

    private static async Task<IResult> DeleteProcessedUrlAsync(
        IProcessedUrlRepository repo,
        IContentRepository contentRepo,
        CancellationToken ct,
        string? url = null)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return Results.BadRequest("The 'url' query parameter is required.");
        }

        var deleted = await repo.DeleteByUrlAsync(url, ct);
        if (!deleted)
        {
            return Results.NotFound();
        }

        contentRepo.InvalidateCachedData();
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteAllFailedUrlsAsync(
        IProcessedUrlRepository repo,
        CancellationToken ct)
    {
        var count = await repo.DeleteAllFailedAsync(ct);
        return Results.Ok(new { deleted = count });
    }

    // ── Custom page data handlers ────────────────────────────────────────────

    private static async Task<IResult> GetCustomPagesAsync(
        ICustomPageDataRepository repo,
        CancellationToken ct)
    {
        var pages = await repo.GetAllAsync(ct);
        return Results.Ok(pages.Select(p => new
        {
            p.Key,
            p.Description,
            p.UpdatedAt
        }));
    }

    private static async Task<IResult> GetCustomPageByKeyAsync(
        string key,
        ICustomPageDataRepository repo,
        CancellationToken ct)
    {
        key = key.Trim().Sanitize();
        var entry = await repo.GetByKeyAsync(key, ct);
        return entry is null ? Results.NotFound() : Results.Ok(entry);
    }

    private static async Task<IResult> UpdateCustomPageAsync(
        string key,
        CustomPageUpdateRequest request,
        ICustomPageDataRepository repo,
        CancellationToken ct)
    {
        key = key.Trim().Sanitize();

        if (string.IsNullOrWhiteSpace(request.JsonData))
        {
            return Results.BadRequest("JsonData is required.");
        }

        // Validate that it is well-formed JSON
        try
        {
            System.Text.Json.JsonDocument.Parse(request.JsonData);
        }
        catch (System.Text.Json.JsonException)
        {
            return Results.BadRequest("JsonData must be valid JSON.");
        }

        var existing = await repo.GetByKeyAsync(key, ct);
        if (existing is null)
        {
            return Results.NotFound();
        }

        await repo.UpsertAsync(key, existing.Description, request.JsonData, ct);
        var updated = await repo.GetByKeyAsync(key, ct);
        return Results.Ok(updated);
    }

    // ── Content item ai_metadata handlers ────────────────────────────────────

    private static async Task<IResult> GetContentItemAiMetadataAsync(
        IContentRepository contentRepo,
        CancellationToken ct,
        string? collection = null,
        string? slug = null)
    {
        if (string.IsNullOrWhiteSpace(collection) || string.IsNullOrWhiteSpace(slug))
        {
            return Results.BadRequest("The 'collection' and 'slug' query parameters are required.");
        }

        collection = collection.Trim().Sanitize();
        slug = slug.Trim().Sanitize();
        var item = await contentRepo.GetAiMetadataAsync(collection, slug, ct);
        return item is null ? Results.NotFound() : Results.Ok(item);
    }

    private static async Task<IResult> UpdateContentItemAiMetadataAsync(
        ContentItemMetadataRequest request,
        IContentRepository contentRepo,
        CancellationToken ct,
        string? collection = null,
        string? slug = null)
    {
        if (string.IsNullOrWhiteSpace(collection) || string.IsNullOrWhiteSpace(slug))
        {
            return Results.BadRequest("The 'collection' and 'slug' query parameters are required.");
        }

        collection = collection.Trim().Sanitize();
        slug = slug.Trim().Sanitize();

        if (string.IsNullOrWhiteSpace(request.AiMetadata))
        {
            return Results.BadRequest("AiMetadata is required.");
        }

        try
        {
            System.Text.Json.JsonDocument.Parse(request.AiMetadata);
        }
        catch (System.Text.Json.JsonException)
        {
            return Results.BadRequest("AiMetadata must be valid JSON.");
        }

        var updated = await contentRepo.UpdateAiMetadataAsync(collection, slug, request.AiMetadata, ct);
        if (!updated)
        {
            return Results.NotFound();
        }

        contentRepo.InvalidateCachedData();
        return Results.NoContent();
    }

    private static async Task<IResult> GetContentItemEditDataAsync(
        IContentRepository contentRepo,
        CancellationToken ct,
        string? collection = null,
        string? slug = null)
    {
        if (string.IsNullOrWhiteSpace(collection) || string.IsNullOrWhiteSpace(slug))
        {
            return Results.BadRequest("The 'collection' and 'slug' query parameters are required.");
        }

        collection = collection.Trim().Sanitize();
        slug = slug.Trim().Sanitize();
        var item = await contentRepo.GetEditDataAsync(collection, slug, ct);
        return item is null ? Results.NotFound() : Results.Ok(item);
    }

    private static async Task<IResult> UpdateContentItemEditDataAsync(
        ContentItemEditData request,
        IContentRepository contentRepo,
        CancellationToken ct,
        string? collection = null,
        string? slug = null)
    {
        if (string.IsNullOrWhiteSpace(collection) || string.IsNullOrWhiteSpace(slug))
        {
            return Results.BadRequest("The 'collection' and 'slug' query parameters are required.");
        }

        collection = collection.Trim().Sanitize();
        slug = slug.Trim().Sanitize();

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return Results.BadRequest("Title is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Author))
        {
            return Results.BadRequest("Author is required.");
        }

        if (string.IsNullOrWhiteSpace(request.PrimarySectionName))
        {
            return Results.BadRequest("PrimarySectionName is required.");
        }

        if (request.Tags.Count == 0)
        {
            return Results.BadRequest("At least one tag is required.");
        }

        if (request.Sections.Count == 0)
        {
            return Results.BadRequest("At least one section is required.");
        }

        if (request.DateEpoch <= 0)
        {
            return Results.BadRequest("A valid date is required.");
        }

        // Validate ai_metadata JSON if provided
        if (!string.IsNullOrWhiteSpace(request.AiMetadata))
        {
            try
            {
                System.Text.Json.JsonDocument.Parse(request.AiMetadata);
            }
            catch (System.Text.Json.JsonException)
            {
                return Results.BadRequest("AiMetadata must be valid JSON.");
            }
        }

        var updated = await contentRepo.UpdateEditDataAsync(collection, slug, request, ct);
        if (!updated)
        {
            return Results.NotFound();
        }

        contentRepo.InvalidateCachedData();
        return Results.NoContent();
    }

    // ── Background job settings handlers ─────────────────────────────────────

    private static async Task<IResult> GetJobSettingsAsync(
        IBackgroundJobSettingRepository repo,
        CancellationToken ct)
    {
        var settings = await repo.GetAllAsync(ct);
        return Results.Ok(settings);
    }

    private static async Task<IResult> UpdateJobSettingAsync(
        string jobName,
        JobSettingUpdateRequest request,
        IBackgroundJobSettingRepository repo,
        CancellationToken ct)
    {
        jobName = jobName.Trim().Sanitize();
        var updated = await repo.SetEnabledAsync(jobName, request.Enabled, ct);
        return updated ? Results.NoContent() : Results.NotFound();
    }

    // ── Cache management handlers ────────────────────────────────────────────

    private static IResult InvalidateCacheAsync(
        IContentRepository contentRepo)
    {
        contentRepo.InvalidateCachedData();
        return Results.Ok(new { message = "All caches invalidated" });
    }

    // ── Content review handlers ──────────────────────────────────────────────

    private static async Task<IResult> GetReviewsAsync(
        IContentReviewRepository reviewRepo,
        CancellationToken ct,
        string? status = null,
        int limit = 100)
    {
        status = status?.Trim().Sanitize();

        if (!string.IsNullOrEmpty(status) && status is not "pending" and not "approved" and not "rejected")
        {
            return Results.BadRequest("Status must be 'pending', 'approved', or 'rejected'.");
        }

        limit = Math.Clamp(limit, 1, 1000);
        var reviews = await reviewRepo.GetByStatusAsync(status, limit, ct);
        return Results.Ok(reviews);
    }

    private static async Task<IResult> GetReviewSummaryAsync(
        IContentReviewRepository reviewRepo,
        CancellationToken ct)
    {
        var summary = await reviewRepo.GetSummaryAsync(ct);
        return Results.Ok(summary);
    }

    private static async Task<IResult> ApproveReviewAsync(
        long id,
        IContentReviewRepository reviewRepo,
        IContentRepository contentRepo,
        CancellationToken ct)
    {
        var approved = await reviewRepo.ApproveAsync(id, ct);
        if (!approved)
        {
            return Results.NotFound();
        }

        contentRepo.InvalidateCachedData();
        return Results.Ok(new { message = "Review approved and change applied" });
    }

    private static async Task<IResult> RejectReviewAsync(
        long id,
        IContentReviewRepository reviewRepo,
        CancellationToken ct)
    {
        var rejected = await reviewRepo.RejectAsync(id, ct);
        return rejected ? Results.Ok(new { message = "Review rejected" }) : Results.NotFound();
    }

    private static async Task<IResult> ApproveAllReviewsAsync(
        IContentReviewRepository reviewRepo,
        IContentRepository contentRepo,
        CancellationToken ct)
    {
        var count = await reviewRepo.ApproveAllAsync(ct);
        if (count > 0)
        {
            contentRepo.InvalidateCachedData();
        }

        return Results.Ok(new { approved = count });
    }

    private static async Task<IResult> RejectAllReviewsAsync(
        IContentReviewRepository reviewRepo,
        CancellationToken ct)
    {
        var count = await reviewRepo.RejectAllAsync(ct);
        return Results.Ok(new { rejected = count });
    }

    private static async Task<IResult> UpdateReviewFixedValueAsync(
        long id,
        ReviewFixedValueRequest request,
        IContentReviewRepository reviewRepo,
        CancellationToken ct)
    {
        if (request.FixedValue is null)
        {
            return Results.BadRequest("FixedValue is required.");
        }

        var updated = await reviewRepo.UpdateFixedValueAsync(id, request.FixedValue, ct);
        return updated ? Results.Ok(new { message = "Review updated" }) : Results.NotFound();
    }

    // ── Content preview handlers ─────────────────────────────────────────────

    private static IResult PreviewMarkdownAsync(
        MarkdownPreviewRequest request,
        IMarkdownService markdownService)
    {
        if (string.IsNullOrWhiteSpace(request.Markdown))
        {
            return Results.BadRequest("Markdown content is required.");
        }

        var processed = markdownService.ProcessYouTubeEmbeds(request.Markdown);
        var html = markdownService.RenderToHtml(processed);
        return Results.Ok(new { html });
    }

    // ── Content items listing handlers ───────────────────────────────────────

    private static async Task<IResult> GetContentItemsPagedAsync(
        IContentRepository contentRepo,
        CancellationToken ct,
        int page = 1,
        int pageSize = 100,
        string? search = null,
        string? collectionName = null,
        string? feedName = null)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 500);
        search = search?.Trim().Sanitize();
        collectionName = collectionName?.Trim().Sanitize();
        feedName = feedName?.Trim().Sanitize();

        var offset = (page - 1) * pageSize;
        var result = await contentRepo.GetContentItemsPagedAsync(offset, pageSize, search, collectionName, feedName, ct);
        return Results.Ok(result);
    }

    private static async Task<IResult> DeleteContentItemAsync(
        IContentRepository contentRepo,
        CancellationToken ct,
        string? collection = null,
        string? slug = null)
    {
        if (string.IsNullOrWhiteSpace(collection) || string.IsNullOrWhiteSpace(slug))
        {
            return Results.BadRequest("The 'collection' and 'slug' query parameters are required.");
        }

        collection = collection.Trim().Sanitize();
        slug = slug.Trim().Sanitize();

        var deleted = await contentRepo.DeleteContentItemAsync(collection, slug, ct);
        if (!deleted)
        {
            return Results.NotFound();
        }

        contentRepo.InvalidateCachedData();
        return Results.NoContent();
    }
}

/// <summary>DTO for creating/updating RSS feed configurations.</summary>
public sealed class FeedConfigRequest
{
    public string? Name { get; init; }
    public string? Url { get; init; }
    public string? OutputDir { get; init; }
    public bool Enabled { get; init; } = true;
    public bool TranscriptMandatory { get; init; }
}

/// <summary>DTO for updating custom page raw JSON.</summary>
public sealed class CustomPageUpdateRequest
{
    public string? JsonData { get; init; }
}

/// <summary>DTO for updating a content item's ai_metadata JSON.</summary>
public sealed class ContentItemMetadataRequest
{
    public string? AiMetadata { get; init; }
}

/// <summary>DTO for updating a background job's enabled state.</summary>
public sealed class JobSettingUpdateRequest
{
    public bool Enabled { get; init; }
}

/// <summary>DTO for rendering markdown to HTML preview.</summary>
public sealed class MarkdownPreviewRequest
{
    public string? Markdown { get; init; }
}

/// <summary>DTO for updating a review's fixed value.</summary>
public sealed class ReviewFixedValueRequest
{
    public string? FixedValue { get; init; }
}
