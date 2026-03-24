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
}
