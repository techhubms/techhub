using TechHub.Core.Models;
using TechHub.Core.Models.Admin;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Repository interface for content operations with database backing.
/// Replaces filesystem-based repository with database-backed implementation.
/// </summary>
public interface IContentRepository
{
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

    /// <summary>
    /// Get all published content items that have a real detail page on the site.
    /// Excludes items from collections that link externally (news, blogs, community).
    /// Used for XML sitemap generation.
    /// </summary>
    Task<IReadOnlyList<SitemapItem>> GetSitemapItemsAsync(CancellationToken ct = default);

    /// <summary>
    /// Get all known authors with their published content item counts.
    /// Returns authors sorted alphabetically by name.
    /// Only includes authors with at least one published (non-draft) content item.
    /// </summary>
    Task<IReadOnlyList<AuthorSummary>> GetAuthorsAsync(CancellationToken ct = default);

    // ==================== Admin Methods ====================

    /// <summary>
    /// Get the ai_metadata JSON for a content item identified by its primary key.
    /// Returns null if no item with that key exists.
    /// </summary>
    Task<ContentItemAiMetadataResult?> GetAiMetadataAsync(string collectionName, string slug, CancellationToken ct = default);

    /// <summary>
    /// Update the ai_metadata JSON column for a content item identified by its primary key.
    /// Returns true if found and updated, false if not found.
    /// </summary>
    Task<bool> UpdateAiMetadataAsync(string collectionName, string slug, string aiMetadata, CancellationToken ct = default);

    /// <summary>
    /// Get all editable fields for a content item identified by its primary key.
    /// Returns null if no item with that key exists.
    /// </summary>
    Task<ContentItemEditData?> GetEditDataAsync(string collectionName, string slug, CancellationToken ct = default);

    /// <summary>
    /// Update the editable fields of a content item identified by its primary key.
    /// Returns true if found and updated, false if not found.
    /// </summary>
    Task<bool> UpdateEditDataAsync(string collectionName, string slug, ContentItemEditData editData, CancellationToken ct = default);

    /// <summary>
    /// Gets a paged list of content items for the admin listing page with optional filters.
    /// </summary>
    Task<PagedResult<ContentItemListItem>> GetContentItemsPagedAsync(
        int offset,
        int limit,
        string? search = null,
        string? collectionName = null,
        string? feedName = null,
        CancellationToken ct = default);

    /// <summary>
    /// Deletes a content item by its primary key (collection_name, slug).
    /// FK cascades automatically delete the associated processed_urls and content_tags_expanded rows.
    /// Returns true if a row was deleted, false if not found.
    /// </summary>
    Task<bool> DeleteContentItemAsync(string collectionName, string slug, CancellationToken ct = default);

    /// <summary>
    /// Updates the subscription plans, GHES support flag, and draft status for a ghc-features content item.
    /// Identified by slug alone (all ghc-features items share the "videos" collection).
    /// Returns true if found and updated, false if not found.
    /// </summary>
    Task<bool> UpdateGhcFeaturePlansAsync(string slug, IReadOnlyList<string> plans, bool ghesSupport, bool draft, CancellationToken ct = default);

    /// <summary>
    /// Invalidates all cached content data (search results, slugs, sitemaps, etc.).
    /// Call after mutations that change content_items (delete, update) to ensure
    /// subsequent queries return fresh data from the database.
    /// </summary>
    void InvalidateCachedData();

    /// <summary>
    /// Find a content item by a legacy slug for redirect purposes.
    /// Tries an exact slug match first, then falls back to stripping a YYYY-MM-DD- date prefix.
    /// When sectionHint is provided, items in that section are preferred.
    /// Among multiple matches the most recently published item wins.
    /// For items in externally-linking collections (news, blogs, community) the returned URL
    /// is the external source URL. For all other items it is the internal path.
    /// Returns null if no matching published item is found.
    /// </summary>
    Task<LegacyRedirectResult?> FindByLegacySlugAsync(
        string slug,
        string? sectionHint = null,
        CancellationToken ct = default);
}
