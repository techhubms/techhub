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

    // ==================== Search Methods ====================

    /// <summary>
    /// Search content with filters, facets, and pagination.
    /// Supports full-text search, tag filtering, date ranges, and keyset pagination.
    /// 
    /// Examples:
    /// - All items: SearchAsync(new SearchRequest { Take = 20 })
    /// - By collection: SearchAsync(new SearchRequest { Collections = ["blogs"], Take = 20 })
    /// - By section: SearchAsync(new SearchRequest { Sections = ["github-copilot"], Take = 20 })
    /// - By section + collection: SearchAsync(new SearchRequest { Sections = ["ai"], Collections = ["blogs"], Take = 20 })
    /// - With tags: SearchAsync(new SearchRequest { Tags = ["copilot"], Take = 20 })
    /// - Full-text search: SearchAsync(new SearchRequest { Query = "ai foundry", Take = 20 })
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
    /// Get tag counts with optional filtering.
    /// Returns top N tags (sorted by count descending) above minUses threshold.
    /// Results are cached - very fast for repeated calls with same filters.
    /// </summary>
    /// <example>
    /// <code>
    /// // Get top 20 most used tags
    /// var popularTags = await repo.GetTagCountsAsync(new TagCountsRequest
    /// {
    ///     MaxTags = 20,
    ///     MinUses = 5
    /// });
    ///
    /// // Get tags for specific section
    /// var aiTags = await repo.GetTagCountsAsync(new TagCountsRequest
    /// {
    ///     SectionName = "ai",
    ///     MaxTags = 50
    /// });
    ///
    /// // Get recent tags
    /// var recentTags = await repo.GetTagCountsAsync(new TagCountsRequest
    /// {
    ///     DateFrom = DateTimeOffset.UtcNow.AddMonths(-3),
    ///     MinUses = 2
    /// });
    /// </code>
    /// </example>
    Task<IReadOnlyList<TagWithCount>> GetTagCountsAsync(
        TagCountsRequest request,
        CancellationToken ct = default);

    // ==================== Section Methods ====================

    /// <summary>
    /// Get all sections defined in configuration.
    /// Sections define the main navigation areas (AI, GitHub Copilot, Security, etc.).
    /// </summary>
    Task<IReadOnlyList<Section>> GetAllSectionsAsync(CancellationToken ct = default);

    /// <summary>
    /// Get a single section by name.
    /// Returns null if section doesn't exist.
    /// </summary>
    Task<Section?> GetSectionByNameAsync(string name, CancellationToken ct = default);
}
