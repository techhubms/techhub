using TechHub.Core.Models;
using TechHub.Core.Models.Admin;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Web.Services;

/// <summary>
/// Interface for Tech Hub API client.
/// Enables mocking in unit tests — always inject this interface, never the concrete class.
/// </summary>
internal interface ITechHubApiClient
{
    // ================================================================
    // Section endpoints
    // ================================================================

    /// <summary>
    /// Get all sections.
    /// GET /api/sections
    /// </summary>
    Task<IEnumerable<Section>?> GetAllSectionsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a specific section by name.
    /// GET /api/sections/{sectionName}
    /// </summary>
    Task<Section?> GetSectionAsync(string sectionName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all collections in a section.
    /// GET /api/sections/{sectionName}/collections
    /// </summary>
    Task<IEnumerable<Collection>?> GetSectionCollectionsAsync(
        string sectionName,
        CancellationToken cancellationToken = default);

    // ================================================================
    // Collection endpoints
    // ================================================================

    /// <summary>
    /// Get a specific collection in a section.
    /// GET /api/sections/{sectionName}/collections/{collectionName}
    /// </summary>
    Task<Collection?> GetCollectionAsync(
        string sectionName,
        string collectionName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get content items for a section/collection with optional filtering.
    /// GET /api/sections/{sectionName}/collections/{collectionName}/items
    /// </summary>
    Task<CollectionItemsResponse?> GetCollectionItemsAsync(
        string sectionName,
        string collectionName,
        int? take = null,
        int? skip = null,
        string? query = null,
        string? tags = null,
        string? subcollection = null,
        int? lastDays = null,
        string? fromDate = null,
        string? toDate = null,
        bool includeDraft = false,
        string? types = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get tag cloud for a collection.
    /// GET /api/sections/{sectionName}/collections/{collectionName}/tags
    /// </summary>
    Task<IReadOnlyList<TagCloudItem>?> GetCollectionTagsAsync(
        string sectionName,
        string collectionName,
        int? maxTags = null,
        int? minUses = null,
        int? lastDays = null,
        List<string>? selectedTags = null,
        List<string>? tagsToCount = null,
        string? fromDate = null,
        string? toDate = null,
        string? searchQuery = null,
        CancellationToken cancellationToken = default);

    // ================================================================
    // Content item endpoints
    // ================================================================

    /// <summary>
    /// Get content item detail by section, collection, and slug.
    /// GET /api/sections/{sectionName}/collections/{collectionName}/{slug}
    /// </summary>
    Task<ContentItemDetail?> GetContentDetailAsync(
        string sectionName,
        string collectionName,
        string slug,
        CancellationToken cancellationToken = default);

    // ================================================================
    // Convenience methods
    // ================================================================

    /// <summary>
    /// Get the latest items across all sections (for homepage sidebar).
    /// </summary>
    Task<IEnumerable<ContentItem>?> GetLatestItemsAsync(
        int count = 10,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the latest roundup item (for homepage sidebar).
    /// </summary>
    Task<ContentItem?> GetLatestRoundupAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get GitHub Copilot feature videos (subcollection=ghc-features), including drafts.
    /// </summary>
    Task<IEnumerable<ContentItem>?> GetGhcFeaturesAsync(CancellationToken cancellationToken = default);

    // ================================================================
    // Tag cloud endpoint (unified scope)
    // ================================================================

    /// <summary>
    /// Get tag cloud for specified scope.
    /// Uses /api/sections/{sectionName}/collections/{collectionName}/tags endpoint.
    /// Pass "all" as collectionName for section-level tag cloud.
    /// Supports dynamic counts via selectedTags and date range parameters.
    /// </summary>
    Task<IReadOnlyList<TagCloudItem>?> GetTagCloudAsync(
        string sectionName,
        string collectionName,
        int? maxTags = null,
        int? minUses = null,
        int? lastDays = null,
        List<string>? selectedTags = null,
        List<string>? tagsToCount = null,
        string? fromDate = null,
        string? toDate = null,
        string? searchQuery = null,
        CancellationToken cancellationToken = default);

    // ================================================================
    // RSS feed endpoints
    // ================================================================

    /// <summary>
    /// Get RSS feed for all content.
    /// </summary>
    Task<string> GetAllContentRssFeedAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get RSS feed for a section.
    /// </summary>
    Task<string> GetSectionRssFeedAsync(string sectionName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get RSS feed for a collection.
    /// </summary>
    Task<string> GetCollectionRssFeedAsync(
        string collectionName,
        string sectionName = "all",
        CancellationToken cancellationToken = default);

    // ================================================================
    // Custom page data endpoints
    // ================================================================

    /// <summary>
    /// Get DX Space page data.
    /// GET /api/custom-pages/dx-space
    /// </summary>
    Task<DXSpacePageData?> GetDXSpaceDataAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get GitHub Copilot Handbook page data.
    /// GET /api/custom-pages/github-copilot-handbook
    /// </summary>
    Task<HandbookPageData?> GetHandbookDataAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get GitHub Copilot Levels page data.
    /// GET /api/custom-pages/github-copilot-levels
    /// </summary>
    Task<LevelsPageData?> GetLevelsDataAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get GitHub Copilot Features page data.
    /// GET /api/custom-pages/github-copilot-features
    /// </summary>
    Task<FeaturesPageData?> GetFeaturesDataAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get GenAI Basics page data.
    /// GET /api/custom-pages/gen-ai-basics
    /// </summary>
    Task<GenAIPageData?> GetGenAIBasicsDataAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get GenAI Advanced page data.
    /// GET /api/custom-pages/gen-ai-advanced
    /// </summary>
    Task<GenAIPageData?> GetGenAIAdvancedDataAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get GenAI Applied page data.
    /// GET /api/custom-pages/gen-ai-applied
    /// </summary>
    Task<GenAIPageData?> GetGenAIAppliedDataAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get AI SDLC page data.
    /// GET /api/custom-pages/ai-sdlc
    /// </summary>
    Task<SDLCPageData?> GetSDLCDataAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get GitHub Copilot ToolTips page data.
    /// GET /api/custom-pages/github-copilot-tooltips
    /// </summary>
    Task<ToolTipsPageData?> GetToolTipsDataAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get GitHub Copilot Getting Started page data.
    /// GET /api/custom-pages/github-copilot-getting-started
    /// </summary>
    Task<GettingStartedPageData?> GetGettingStartedDataAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get hero banner data.
    /// GET /api/custom-pages/hero-banner
    /// </summary>
    Task<HeroBannerData?> GetHeroBannerDataAsync(CancellationToken cancellationToken = default);

    // ================================================================
    // Sitemap endpoint
    // ================================================================

    /// <summary>
    /// Get XML sitemap from the API.
    /// GET /api/sitemap
    /// </summary>
    Task<string> GetSitemapAsync(CancellationToken cancellationToken = default);

    // ================================================================
    // Author endpoints
    // ================================================================

    /// <summary>
    /// Get all authors with their content item counts.
    /// GET /api/authors
    /// </summary>
    Task<IReadOnlyList<AuthorSummary>?> GetAuthorsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get content items for a specific author.
    /// GET /api/authors/{authorName}/items
    /// </summary>
    Task<CollectionItemsResponse?> GetAuthorItemsAsync(
        string authorName,
        int? take = null,
        int? skip = null,
        CancellationToken cancellationToken = default);

    // ================================================================
    // Admin endpoints
    // ================================================================

    /// <summary>
    /// Trigger an immediate content processing run.
    /// POST /api/admin/processing/trigger
    /// </summary>
    Task TriggerContentProcessingAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Trigger an immediate roundup generation run.
    /// POST /api/admin/roundup/trigger
    /// </summary>
    Task TriggerRoundupGenerationAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Trigger a bulk content fix run (tags, authors, markdown, AI metadata backfill).
    /// POST /api/admin/content-fixer/trigger
    /// </summary>
    Task TriggerContentFixerAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancel the currently running background job.
    /// POST /api/admin/processing/cancel
    /// </summary>
    Task CancelRunningJobAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get recent content processing job history.
    /// GET /api/admin/processing/jobs
    /// </summary>
    Task<IReadOnlyList<ContentProcessingJob>> GetProcessingJobsAsync(
        int count = 20,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a specific content processing job by ID.
    /// GET /api/admin/processing/jobs/{id}
    /// </summary>
    Task<ContentProcessingJob?> GetProcessingJobByIdAsync(
        long id,
        CancellationToken cancellationToken = default);

    // ================================================================
    // RSS Feed config endpoints
    // ================================================================

    /// <summary>
    /// Get all RSS feed configurations.
    /// GET /api/admin/feeds
    /// </summary>
    Task<IReadOnlyList<FeedConfig>> GetFeedConfigsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a specific RSS feed config by ID.
    /// GET /api/admin/feeds/{id}
    /// </summary>
    Task<FeedConfig?> GetFeedConfigByIdAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a new RSS feed config.
    /// POST /api/admin/feeds
    /// </summary>
    Task<FeedConfig> CreateFeedConfigAsync(FeedConfig config, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update an existing RSS feed config.
    /// PUT /api/admin/feeds/{id}
    /// </summary>
    Task<FeedConfig> UpdateFeedConfigAsync(FeedConfig config, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete an RSS feed config.
    /// DELETE /api/admin/feeds/{id}
    /// </summary>
    Task DeleteFeedConfigAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get database statistics for the admin dashboard.
    /// GET /api/admin/statistics
    /// </summary>
    Task<DatabaseStatistics?> GetDatabaseStatisticsAsync(CancellationToken cancellationToken = default);

    // ================================================================
    // Processed URLs endpoints
    // ================================================================

    /// <summary>
    /// Get a paginated list of processed URLs with optional filters.
    /// GET /api/admin/processed-urls
    /// </summary>
    Task<PagedResult<ProcessedUrlListItem>> GetProcessedUrlsAsync(
        int page = 1,
        int pageSize = 100,
        string? status = null,
        string? search = null,
        string? feedName = null,
        string? collectionName = null,
        long? jobId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a specific processed URL so it can be retried.
    /// DELETE /api/admin/processed-urls?url={url}
    /// </summary>
    Task<bool> DeleteProcessedUrlAsync(string url, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete all failed processed URL records.
    /// DELETE /api/admin/processed-urls/failed
    /// </summary>
    Task<int> DeleteAllFailedProcessedUrlsAsync(CancellationToken cancellationToken = default);

    // ================================================================
    // Admin – Custom page data endpoints
    // ================================================================

    /// <summary>
    /// List all custom page entries (key, description, last updated).
    /// GET /api/admin/custom-pages
    /// </summary>
    Task<IReadOnlyList<CustomPageEntry>> GetCustomPageEntriesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a single custom page entry with its raw JSON.
    /// GET /api/admin/custom-pages/{key}
    /// </summary>
    Task<CustomPageEntry?> GetCustomPageEntryAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update the raw JSON for a custom page.
    /// PUT /api/admin/custom-pages/{key}
    /// </summary>
    Task<CustomPageEntry> UpdateCustomPageAsync(string key, string jsonData, CancellationToken cancellationToken = default);

    // ================================================================
    // Admin – Content item ai_metadata endpoints
    // ================================================================

    /// <summary>
    /// Get the ai_metadata JSON for a content item by its primary key.
    /// GET /api/admin/content-items/ai-metadata?collection={collection}&amp;slug={slug}
    /// </summary>
    Task<ContentItemAiMetadataResult?> GetContentItemAiMetadataAsync(string collectionName, string slug, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update the ai_metadata JSON for a content item by its primary key.
    /// PUT /api/admin/content-items/ai-metadata?collection={collection}&amp;slug={slug}
    /// </summary>
    Task UpdateContentItemAiMetadataAsync(string collectionName, string slug, string aiMetadata, CancellationToken cancellationToken = default);

    // ================================================================
    // Admin – Content item editing endpoints
    // ================================================================

    /// <summary>
    /// Get all editable fields for a content item by its primary key.
    /// GET /api/admin/content-items/edit-data?collection={collection}&amp;slug={slug}
    /// </summary>
    Task<ContentItemEditData?> GetContentItemEditDataAsync(string collectionName, string slug, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update all editable fields for a content item by its primary key.
    /// PUT /api/admin/content-items/edit-data?collection={collection}&amp;slug={slug}
    /// </summary>
    Task UpdateContentItemEditDataAsync(string collectionName, string slug, ContentItemEditData editData, CancellationToken cancellationToken = default);

    // ================================================================
    // Admin – Content items listing endpoints
    // ================================================================

    /// <summary>
    /// Get a paginated list of content items with optional filters.
    /// GET /api/admin/content-items
    /// </summary>
    Task<PagedResult<ContentItemListItem>> GetContentItemsAsync(
        int page = 1,
        int pageSize = 100,
        string? search = null,
        string? collectionName = null,
        string? feedName = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a content item by its primary key (cascades to processed_urls).
    /// DELETE /api/admin/content-items?collection={collection}&amp;slug={slug}
    /// </summary>
    Task<bool> DeleteContentItemAsync(string collectionName, string slug, CancellationToken cancellationToken = default);

    // ================================================================
    // Admin – Background job settings endpoints
    // ================================================================

    /// <summary>
    /// Get all background job settings.
    /// GET /api/admin/job-settings
    /// </summary>
    Task<IReadOnlyList<BackgroundJobSetting>> GetJobSettingsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Update the enabled state for a background job.
    /// PUT /api/admin/job-settings/{jobName}
    /// </summary>
    Task UpdateJobSettingAsync(string jobName, bool enabled, CancellationToken cancellationToken = default);

    // ================================================================
    // Admin – Cache management endpoints
    // ================================================================

    /// <summary>
    /// Invalidate all server-side caches.
    /// POST /api/admin/cache/invalidate
    /// </summary>
    Task InvalidateCachesAsync(CancellationToken cancellationToken = default);

    // ================================================================
    // Admin – Content review endpoints
    // ================================================================

    /// <summary>
    /// Get content reviews filtered by status.
    /// GET /api/admin/reviews
    /// </summary>
    Task<IReadOnlyList<ContentReview>> GetContentReviewsAsync(
        string? status = null,
        int limit = 100,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get review summary counts (pending/approved/rejected).
    /// GET /api/admin/reviews/summary
    /// </summary>
    Task<ContentReviewSummary> GetContentReviewSummaryAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Approve a single review and apply the change.
    /// POST /api/admin/reviews/{id}/approve
    /// </summary>
    Task<bool> ApproveContentReviewAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reject a single review without applying the change.
    /// POST /api/admin/reviews/{id}/reject
    /// </summary>
    Task<bool> RejectContentReviewAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Approve all pending reviews and apply changes.
    /// POST /api/admin/reviews/approve-all
    /// </summary>
    Task<int> ApproveAllContentReviewsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Reject all pending reviews.
    /// POST /api/admin/reviews/reject-all
    /// </summary>
    Task<int> RejectAllContentReviewsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Update the fixed value of a pending review.
    /// PUT /api/admin/reviews/{id}
    /// </summary>
    Task<bool> UpdateContentReviewFixedValueAsync(long id, string fixedValue, CancellationToken cancellationToken = default);

    // ================================================================
    // Admin – Content preview endpoint
    // ================================================================

    /// <summary>
    /// Render markdown to HTML for preview.
    /// POST /api/admin/content-items/preview-markdown
    /// </summary>
    Task<string> PreviewMarkdownAsync(string markdown, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update the subscription plans, GHES support flag, and draft status for a ghc-features video.
    /// PUT /api/admin/ghc-features/{slug}/plans
    /// </summary>
    Task UpdateGhcFeaturePlansAsync(string slug, IReadOnlyList<string> plans, bool ghesSupport, bool draft, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a ghc-features video from the database.
    /// DELETE /api/admin/ghc-features/{slug}
    /// Returns false when the item was not found (HTTP 404).
    /// </summary>
    Task<bool> DeleteGhcFeatureAsync(string slug, CancellationToken cancellationToken = default);

    /// <summary>
    /// Process a single URL ad-hoc, outside the RSS pipeline.
    /// POST /api/admin/urls/process
    /// Returns null when the URL already exists (HTTP 409).
    /// </summary>
    Task<AdHocUrlProcessResult?> ProcessAdHocUrlAsync(AdHocUrlProcessRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Fetch the page title for a URL.
    /// GET /api/admin/urls/title?url={url}
    /// Returns null when the title could not be extracted.
    /// </summary>
    Task<string?> FetchUrlTitleAsync(string url, CancellationToken cancellationToken = default);

    // ================================================================
    // Legacy redirect endpoint
    // ================================================================

    /// <summary>
    /// Look up a legacy slug and return the canonical URL to redirect to, or null if not found.
    /// GET /api/legacy-redirect?slug={slug}&amp;section={sectionHint}
    /// </summary>
    Task<LegacyRedirectResult?> GetLegacyRedirectAsync(
        string slug,
        string? sectionHint = null,
        CancellationToken cancellationToken = default);
}
