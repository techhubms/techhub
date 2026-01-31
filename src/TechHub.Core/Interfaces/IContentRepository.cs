using TechHub.Core.Models;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Repository interface for content operations with database backing.
/// Replaces filesystem-based repository with database-backed implementation.
/// </summary>
public interface IContentRepository
{
    // ==================== Initialization ====================

    /// <summary>
    /// Initialize repository (database query after content sync completes).
    /// Called once at startup after content sync.
    /// </summary>
    Task<IReadOnlyList<ContentItem>> InitializeAsync(CancellationToken ct = default);

    // ==================== Existing Methods (Database-Backed) ====================

    /// <summary>
    /// Get a single content item by slug and collection.
    /// Returns ContentItemDetail which includes the full markdown content for rendering.
    /// </summary>
    Task<ContentItemDetail?> GetBySlugAsync(
        string collectionName,
        string slug,
        bool includeDraft = false,
        CancellationToken ct = default);

    /// <summary>
    /// Get all content items across all collections.
    /// </summary>
    Task<IReadOnlyList<ContentItem>> GetAllAsync(
        int limit,
        int offset = 0,
        bool includeDraft = false,
        CancellationToken ct = default);

    /// <summary>
    /// Get all content items in a specific collection.
    /// Optionally filter by subcollection (e.g., "ghc-features", "vscode-updates").
    /// </summary>
    Task<IReadOnlyList<ContentItem>> GetByCollectionAsync(
        string collectionName,
        int limit,
        string? subcollectionName = null,
        int offset = 0,
        bool includeDraft = false,
        CancellationToken ct = default);

    /// <summary>
    /// Get all content items in a specific section.
    /// Optionally filter by collection and subcollection.
    /// </summary>
    Task<IReadOnlyList<ContentItem>> GetBySectionAsync(
        string sectionName,
        int limit,
        int offset = 0,
        string? collectionName = null,
        string? subcollectionName = null,
        bool includeDraft = false,
        CancellationToken ct = default);

    // ==================== New Search Methods ====================

    /// <summary>
    /// Search content with filters, facets, and pagination.
    /// Supports full-text search, tag filtering, date ranges, and keyset pagination.
    /// </summary>
    Task<SearchResults<ContentItem>> SearchAsync(
        SearchRequest request,
        CancellationToken ct = default);

    /// <summary>
    /// Get facet counts for filtered content.
    /// Returns counts for tags, collections, sections within the filtered scope.
    /// </summary>
    Task<FacetResults> GetFacetsAsync(
        FacetRequest request,
        CancellationToken ct = default);

    /// <summary>
    /// Get related articles based on tag overlap.
    /// Returns articles ranked by number of shared tags (descending).
    /// </summary>
    /// <param name="sourceTags">Tags from the source article (caller already has these)</param>
    /// <param name="excludeSlug">Slug of the source article to exclude from results</param>
    /// <param name="count">Maximum number of related articles to return</param>
    /// <param name="ct">Cancellation token</param>
    Task<IReadOnlyList<ContentItem>> GetRelatedAsync(
        IReadOnlyList<string> sourceTags,
        string excludeSlug,
        int count,
        CancellationToken ct = default);

    /// <summary>
    /// Get tag counts with optional filtering by date range, section, and collection.
    /// Returns top N tags (sorted by count descending) above minUses threshold.
    /// Results are cached - very fast for repeated calls with same filters.
    /// </summary>
    Task<IReadOnlyList<TagWithCount>> GetTagCountsAsync(
        DateTimeOffset? dateFrom = null,
        DateTimeOffset? dateTo = null,
        string? sectionName = null,
        string? collectionName = null,
        int? maxTags = null,
        int minUses = 1,
        CancellationToken ct = default);
}
