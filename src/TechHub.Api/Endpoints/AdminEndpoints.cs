using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using TechHub.Api.Services;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Logging;
using TechHub.Core.Models.Admin;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Services;
using TechHub.Infrastructure.Services.ContentProcessing;

namespace TechHub.Api.Endpoints;

/// <summary>
/// Admin API endpoints for content processing management and RSS feed configuration.
/// Protected by Azure AD bearer token authentication. The Web front-end forwards
/// the authenticated user's token after OIDC sign-in.
/// When Azure AD is not configured (local dev), the AdminOnly policy allows all requests.
/// </summary>
public static partial class AdminEndpoints
{
    public static void MapAdminEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/admin")
            .WithTags("Admin")
            .RequireAuthorization("AdminOnly")
            .RequireRateLimiting("api-admin");

        // ── Processing jobs ──────────────────────────────────────────────────

        group.MapPost("/processing/trigger", TriggerProcessingAsync)
            .WithName("TriggerContentProcessing")
            .WithSummary("Trigger an immediate content processing run");

        group.MapPost("/roundup/trigger", TriggerRoundupAsync)
            .WithName("TriggerRoundupGeneration")
            .WithSummary("Trigger an immediate roundup generation run");

        group.MapPost("/roundup/regenerate", RegenerateRoundupAsync)
            .WithName("RegenerateRoundup")
            .WithSummary("Delete an existing roundup and re-run generation for its week");

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

        group.MapPost("/content-items/apply-transcript", ApplyTranscriptAsync)
            .WithName("ApplyTranscript")
            .WithSummary("Apply a manually provided transcript to an existing video content item, regenerating AI content");

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

        // ── GHC feature plans (legacy routes removed — now managed via /api/ghc-features) ──

        group.MapDelete("/ghc-features/{slug}", DeleteGhcFeatureAsync)
            .WithName("AdminDeleteGhcFeature")
            .WithSummary("Delete a ghc-features video from the database");

        // ── VS Code updates ──────────────────────────────────────────────────

        group.MapGet("/vscode-updates", GetVscodeUpdatesAsync)
            .WithName("GetVscodeUpdates")
            .WithSummary("Get a paginated list of VS Code Update items");

        group.MapPost("/vscode-updates", AddVscodeUpdateAsync)
            .WithName("AddVscodeUpdate")
            .WithSummary("Register an existing content item as a VS Code Update");

        group.MapDelete("/vscode-updates", RemoveVscodeUpdateAsync)
            .WithName("RemoveVscodeUpdate")
            .WithSummary("Remove a content item from the VS Code Updates list (does not delete the content item)");

        // ── Ad-hoc URL processing ────────────────────────────────────────────

        group.MapPost("/urls/process", ProcessAdHocUrlAsync)
            .WithName("ProcessAdHocUrl")
            .WithSummary("Process a single URL outside the RSS pipeline");

        group.MapGet("/urls/title", FetchUrlTitleAsync)
            .WithName("FetchUrlTitle")
            .WithSummary("Fetch the page title for a given URL");
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

    private static async Task<IResult> RegenerateRoundupAsync(
        IContentRepository contentRepo,
        RoundupGeneratorBackgroundService roundupService,
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

        if (!string.Equals(collection, "roundups", StringComparison.OrdinalIgnoreCase))
        {
            return Results.BadRequest("Regeneration is only supported for the 'roundups' collection.");
        }

        var match = RoundupSlugRegex().Match(slug);
        if (!match.Success || !DateOnly.TryParse(match.Groups[2].Value, out var publishDate))
        {
            return Results.BadRequest("The slug does not match the expected roundup format 'weekly-{section}-roundup-YYYY-MM-DD'.");
        }

        // publishDate is the Monday after the week ends (weekEnd + 1 day).
        var weekEnd = publishDate.AddDays(-1);
        var weekStart = weekEnd.AddDays(-6);

        // Delete the existing roundup so the generator produces a fresh one.
        // Ignore the return value — if it didn't exist, generation will simply create it.
        await contentRepo.DeleteContentItemAsync(collection, slug, ct);
        contentRepo.InvalidateCachedData();

        roundupService.TriggerRegenerateRun(weekStart, weekEnd);
        return Results.Accepted("/api/admin/processing/jobs", new { message = $"Roundup regeneration triggered for week {weekStart}\u2013{weekEnd}" });
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
        };

        var id = await feedRepo.CreateAsync(feed, ct);
        var created = new FeedConfig
        {
            Id = id,
            Name = name,
            Url = url,
            OutputDir = outputDir,
            Enabled = request.Enabled,
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
        long? jobId = null,
        string? sectionName = null,
        bool primarySectionOnly = false)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 500);
        status = status?.Trim().Sanitize();
        search = search?.Trim().Sanitize();
        feedName = feedName?.Trim().Sanitize();
        collectionName = collectionName?.Trim().Sanitize();
        sectionName = sectionName?.Trim().Sanitize();

        // Validate status filter
        if (!string.IsNullOrEmpty(status) && status is not "succeeded" and not "skipped" and not "failed")
        {
            return Results.BadRequest("Status must be 'succeeded', 'skipped', or 'failed'.");
        }

        var offset = (page - 1) * pageSize;
        var result = await repo.GetPagedAsync(offset, pageSize, status, search, feedName, collectionName, jobId, sectionName, primarySectionOnly, ct);
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

    // ── Apply transcript handler ─────────────────────────────────────────────

    private static async Task<IResult> ApplyTranscriptAsync(
        ApplyTranscriptRequest request,
        IContentRepository contentRepo,
        IAiCategorizationService aiService,
        IContentFixerService contentFixer,
        CancellationToken ct,
        string? collection = null,
        string? slug = null)
    {
        if (string.IsNullOrWhiteSpace(collection) || string.IsNullOrWhiteSpace(slug))
        {
            return Results.BadRequest("The 'collection' and 'slug' query parameters are required.");
        }

        if (!string.Equals(collection.Trim(), "videos", StringComparison.OrdinalIgnoreCase))
        {
            return Results.BadRequest("Transcripts can only be applied to video items (collection must be 'videos').");
        }

        if (string.IsNullOrWhiteSpace(request.Transcript))
        {
            return Results.BadRequest("Transcript is required.");
        }

        if (request.Transcript.Length > MaxTranscriptLength)
        {
            return Results.BadRequest($"Transcript is too long. Maximum allowed length is {MaxTranscriptLength:N0} characters (received {request.Transcript.Length:N0}).");
        }

        collection = collection.Trim().Sanitize();
        slug = slug.Trim().Sanitize();

        var existing = await contentRepo.GetEditDataAsync(collection, slug, ct);
        if (existing is null)
        {
            return Results.NotFound();
        }

        if (string.IsNullOrWhiteSpace(existing.ExternalUrl))
        {
            return Results.BadRequest("Content item does not have an associated external URL.");
        }

        // Ensure the item is actually a YouTube video before proceeding
        var isYouTubeUrl = existing.ExternalUrl.Contains("youtube.com", StringComparison.OrdinalIgnoreCase)
            || existing.ExternalUrl.Contains("youtu.be", StringComparison.OrdinalIgnoreCase);
        if (!isYouTubeUrl)
        {
            return Results.BadRequest("Transcripts can only be applied to YouTube video items.");
        }

        // Build a synthetic RawFeedItem using the existing item's metadata and the provided transcript
        var raw = new RawFeedItem
        {
            Title = existing.Title,
            ExternalUrl = existing.ExternalUrl,
            PublishedAt = DateTimeOffset.FromUnixTimeSeconds(existing.DateEpoch),
            FeedName = existing.FeedName ?? "TechHub",
            CollectionName = collection,
            FeedLevelAuthor = existing.Author,
            FeedTags = existing.Tags.ToList(),
            FullContent = request.Transcript.Trim(),
            SkipSalesPitchCheck = true
        };

        CategorizationResult categorizationResult;
        try
        {
            categorizationResult = await aiService.CategorizeAsync(raw, ct);
        }
        catch (Exception ex) when (ex is HttpRequestException or System.Text.Json.JsonException or InvalidOperationException or TimeoutException)
        {
            return Results.Problem($"AI categorization failed: {ex.Message.Sanitize()}", statusCode: 502);
        }

        if (categorizationResult.IsFailure || categorizationResult.Item is null)
        {
            return Results.Problem(
                categorizationResult.IsFailure
                    ? $"AI categorization failed: {categorizationResult.Explanation}"
                    : $"AI determined this content should be excluded: {categorizationResult.Explanation}",
                statusCode: 422);
        }

        var processed = categorizationResult.Item;

        // Repair markdown
        if (!string.IsNullOrWhiteSpace(processed.Content))
        {
            processed = processed.WithContent(contentFixer.RepairMarkdown(processed.Content));
        }

        // Ensure section-derived tags are present
        processed = processed.WithTags(TagNormalizer.EnsureSectionTags(processed.Tags, processed.Sections));
        processed = processed.WithTags(TagNormalizer.NormalizeTags(processed.Tags));

        // Build updated edit data, preserving the existing slug/collection/date
        string? newAiMetadata = existing.AiMetadata;
        if (processed.RoundupMetadata is not null)
        {
            newAiMetadata = System.Text.Json.JsonSerializer.Serialize(new
            {
                roundup_summary = processed.RoundupMetadata.Summary,
                key_topics = processed.RoundupMetadata.KeyTopics,
                roundup_relevance = processed.RoundupMetadata.Relevance,
                topic_type = processed.RoundupMetadata.TopicType,
                impact_level = processed.RoundupMetadata.ImpactLevel,
                time_sensitivity = processed.RoundupMetadata.TimeSensitivity
            });
        }

        var updatedEditData = new ContentItemEditData
        {
            CollectionName = collection,
            Slug = slug,
            DateEpoch = existing.DateEpoch,
            Title = processed.Title,
            Author = processed.Author ?? existing.Author,
            Excerpt = processed.Excerpt,
            Content = processed.Content,
            PrimarySectionName = processed.PrimarySectionName,
            FeedName = existing.FeedName,
            Tags = processed.Tags,
            Sections = processed.Sections,
            AiMetadata = newAiMetadata,
            ExternalUrl = existing.ExternalUrl
        };

        var updated = await contentRepo.UpdateEditDataAsync(collection, slug, updatedEditData, ct);
        if (!updated)
        {
            return Results.NotFound();
        }

        contentRepo.InvalidateCachedData();
        return Results.Ok(updatedEditData);
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
        string? feedName = null,
        string? sectionName = null,
        bool primarySectionOnly = false)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 500);
        search = search?.Trim().Sanitize();
        collectionName = collectionName?.Trim().Sanitize();
        feedName = feedName?.Trim().Sanitize();
        sectionName = sectionName?.Trim().Sanitize();

        var offset = (page - 1) * pageSize;
        var result = await contentRepo.GetContentItemsPagedAsync(offset, pageSize, search, collectionName, feedName, sectionName, primarySectionOnly, ct);
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

    private static async Task<IResult> DeleteGhcFeatureAsync(
        string slug,
        IContentRepository contentRepo,
        CancellationToken ct)
    {
        slug = slug.Trim().Sanitize();

        var deleted = await contentRepo.DeleteContentItemAsync("videos", slug, ct);
        if (!deleted)
        {
            return Results.NotFound();
        }

        contentRepo.InvalidateCachedData();
        return Results.NoContent();
    }

    // ── VS Code updates handlers ─────────────────────────────────────────────

    private static async Task<IResult> GetVscodeUpdatesAsync(
        IGhcFeatureRepository ghcRepo,
        CancellationToken ct,
        int page = 1,
        int pageSize = 100,
        string? search = null)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 500);
        search = search?.Trim().Sanitize();

        var offset = (page - 1) * pageSize;
        var (items, totalCount) = await ghcRepo.GetVscodeUpdateItemsAsync(offset, pageSize, search, ct);
        return Results.Ok(new { Items = items, TotalCount = totalCount });
    }

    private static async Task<IResult> AddVscodeUpdateAsync(
        VscodeUpdateRequest request,
        IGhcFeatureRepository ghcRepo,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.CollectionName) || string.IsNullOrWhiteSpace(request.Slug))
        {
            return Results.BadRequest("CollectionName and Slug are required.");
        }

        await ghcRepo.AddVscodeUpdateItemAsync(request.CollectionName.Trim().Sanitize(), request.Slug.Trim().Sanitize(), ct);
        return Results.NoContent();
    }

    private static async Task<IResult> RemoveVscodeUpdateAsync(
        IGhcFeatureRepository ghcRepo,
        CancellationToken ct,
        string? collection = null,
        string? slug = null)
    {
        if (string.IsNullOrWhiteSpace(collection) || string.IsNullOrWhiteSpace(slug))
        {
            return Results.BadRequest("The 'collection' and 'slug' query parameters are required.");
        }

        var removed = await ghcRepo.RemoveVscodeUpdateItemAsync(collection.Trim().Sanitize(), slug.Trim().Sanitize(), ct);
        return removed ? Results.NoContent() : Results.NotFound();
    }

#pragma warning disable CA1812 // Instantiated by ASP.NET Core JSON binding
    private sealed class VscodeUpdateRequest
    {
        public required string CollectionName { get; init; }
        public required string Slug { get; init; }
    }
#pragma warning restore CA1812

    // ── Ad-hoc URL processing handlers ───────────────────────────────────────

    /// <summary>Maximum transcript length (characters) accepted by transcript endpoints.</summary>
    private const int MaxTranscriptLength = 50_000;

    private static readonly HashSet<string> _validCollectionNames =
        ["blogs", "news", "videos", "community"];

    private static async Task<IResult> ProcessAdHocUrlAsync(
        AdHocUrlProcessRequest request,
        ContentProcessingService processingService,
        IContentRepository contentRepo,
        CancellationToken ct)
    {
        // Validate URL
        if (string.IsNullOrWhiteSpace(request.Url)
            || !Uri.TryCreate(request.Url.Trim(), UriKind.Absolute, out var uri)
            || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            return Results.BadRequest("A valid HTTP or HTTPS URL is required.");
        }

        // Validate collection
        var collection = request.CollectionName?.Trim().ToLowerInvariant() ?? string.Empty;
        if (!_validCollectionNames.Contains(collection))
        {
            return Results.BadRequest(
                $"Invalid collection '{request.CollectionName}'. Valid collections: {string.Join(", ", _validCollectionNames)}.");
        }

        var sanitizedUrl = request.Url.Trim().Sanitize();
        var feedName = !string.IsNullOrWhiteSpace(request.FeedName)
            ? request.FeedName.Trim().Sanitize()
            : "TechHub";
        var titleHint = !string.IsNullOrWhiteSpace(request.TitleHint)
            ? request.TitleHint.Trim().Sanitize()
            : null;
        var transcript = !string.IsNullOrWhiteSpace(request.Transcript)
            ? request.Transcript.Trim()
            : null;
        var authorOverride = !string.IsNullOrWhiteSpace(request.AuthorOverride)
            ? request.AuthorOverride.Trim().Sanitize()
            : null;

        if (transcript is not null && transcript.Length > MaxTranscriptLength)
        {
            return Results.BadRequest($"Transcript is too long. Maximum allowed length is {MaxTranscriptLength:N0} characters (received {transcript.Length:N0}).");
        }

        var result = await processingService.ProcessSingleAsync(
            sanitizedUrl,
            collection,
            feedName,
            subcollectionName: null,
            titleHint,
            transcript,
            authorOverride,
            ct);

        if (result is null)
        {
            return Results.Conflict(new { message = "This URL has already been processed." });
        }

        contentRepo.InvalidateCachedData();

        return result.Outcome switch
        {
            AdHocUrlProcessOutcome.Added => Results.Ok(result),
            AdHocUrlProcessOutcome.Skipped => Results.Ok(result),
            _ => Results.UnprocessableEntity(result)
        };
    }

    // ── URL title fetch handler ──────────────────────────────────────────────

    private static async Task<IResult> FetchUrlTitleAsync(
        [FromQuery] string url,
        IArticleFetchClient fetchClient,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(url)
            || !Uri.TryCreate(url.Trim(), UriKind.Absolute, out var uri)
            || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            return Results.BadRequest("A valid HTTP or HTTPS URL is required.");
        }

        string? html;
        try
        {
            html = await fetchClient.FetchHtmlAsync(url.Trim(), ct);
        }
        catch (HttpRequestException)
        {
            return Results.Ok(new { title = (string?)null });
        }

        if (string.IsNullOrWhiteSpace(html))
        {
            return Results.Ok(new { title = (string?)null });
        }

        // Extract <title> from HTML
        var titleMatch = TitleRegex().Match(html);
        var title = titleMatch.Success
            ? System.Net.WebUtility.HtmlDecode(titleMatch.Groups[1].Value).Trim()
            : null;

        return Results.Ok(new { title });
    }

    [GeneratedRegex(@"<title[^>]*>(.*?)</title>", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private static partial Regex TitleRegex();

    [GeneratedRegex(@"^weekly-(.+)-roundup-(\d{4}-\d{2}-\d{2})$")]
    private static partial Regex RoundupSlugRegex();
}
