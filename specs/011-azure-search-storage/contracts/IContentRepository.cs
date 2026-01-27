using TechHub.Core.Models;
using TechHub.Core.Models;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Repository interface for content operations with database backing.
/// Extends existing filesystem-based interface with search and faceting capabilities.
/// </summary>
public interface IContentRepository
{
    // ==================== Initialization ====================
    
    /// <summary>
    /// Initialize repository (replaces file loading with database query).
    /// Called once at startup after content sync completes.
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
    /// </summary>
    /// <param name="collectionName">
    /// Collection name (e.g., "videos", "ghc-features", "blogs").
    /// For "videos", returns all video subcollections (ghc-features, vscode-updates).
    /// </param>
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
    /// Supports:
    /// - Full-text search (PostgreSQL tsvector, SQLite FTS5)
    /// - Tag filtering with subset matching (e.g., "AI" matches "Azure AI")
    /// - Section and collection filtering
    /// - Date range filtering
    /// - Keyset pagination (cursor-based for infinite scroll)
    /// - Facet counts for dynamic filtering
    /// </summary>
    Task<SearchResults<ContentItem>> SearchAsync(
        SearchRequest request,
        CancellationToken ct = default);
    
    /// <summary>
    /// Get facet counts for filtered content.
    /// Returns counts for tags, collections, sections within the filtered scope.
    /// Used to show "how many items would match if I click this tag".
    /// </summary>
    Task<FacetResults> GetFacetsAsync(
        FacetRequest request,
        CancellationToken ct = default);
    
    /// <summary>
    /// Get related articles based on tag overlap (Phase 1) or semantic similarity (Phase 2).
    /// Returns articles ranked by shared tags in Phase 1.
    /// </summary>
    Task<IReadOnlyList<ContentItem>> GetRelatedAsync(
        string articleId,
        int count = 5,
        CancellationToken ct = default);
}
