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
    /// </summary>
    Task<ContentItem?> GetBySlugAsync(
        string collectionName,
        string slug,
        bool includeDraft = false,
        CancellationToken ct = default);

    /// <summary>
    /// Get all content items across all collections.
    /// </summary>
    Task<IReadOnlyList<ContentItem>> GetAllAsync(
        bool includeDraft = false,
        CancellationToken ct = default);

    /// <summary>
    /// Get all content items in a specific collection.
    /// For "videos", returns all video subcollections (ghc-features, vscode-updates).
    /// </summary>
    Task<IReadOnlyList<ContentItem>> GetByCollectionAsync(
        string collectionName,
        bool includeDraft = false,
        CancellationToken ct = default);

    /// <summary>
    /// Get all content items in a specific section.
    /// </summary>
    Task<IReadOnlyList<ContentItem>> GetBySectionAsync(
        string sectionName,
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
    /// Get related articles based on tag overlap (Phase 1) or semantic similarity (Phase 2).
    /// Returns articles ranked by shared tags.
    /// </summary>
    Task<IReadOnlyList<ContentItem>> GetRelatedAsync(
        string articleId,
        int count = 5,
        CancellationToken ct = default);
}
