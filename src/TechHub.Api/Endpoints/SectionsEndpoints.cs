using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;

namespace TechHub.Api.Endpoints;

/// <summary>
/// Unified API endpoints for sections, collections, content items, and tags.
/// All content access goes through the section hierarchy.
/// </summary>
public static class SectionsEndpoints
{
    /// <summary>
    /// Default number of items to return when no limit is specified.
    /// </summary>
    private const int DefaultPageSize = 20;

    /// <summary>
    /// Maximum number of items that can be requested in a single call.
    /// </summary>
    private const int MaxPageSize = 50;

    /// <summary>
    /// Maps all section-related endpoints to the application.
    /// 
    /// Hierarchy:
    /// - /api/sections                                              → List all sections
    /// - /api/sections/{sectionName}                                → Get section metadata
    /// - /api/sections/{sectionName}/collections                    → List collections in section
    /// - /api/sections/{sectionName}/collections/{collectionName}   → Get collection metadata
    /// - /api/sections/{sectionName}/collections/{collectionName}/items  → Get items (use "all" for section-level)
    /// - /api/sections/{sectionName}/collections/{collectionName}/tags   → Get tag cloud (use "all" for section-level)
    /// - /api/sections/{sectionName}/collections/{collectionName}/{slug} → Get single item detail
    /// </summary>
    public static IEndpointRouteBuilder MapSectionsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/sections")
            .WithTags("Sections")
            .WithDescription("Unified API for browsing sections, collections, content items, and tags");

        // ============================================================
        // Section-level endpoints
        // ============================================================

        group.MapGet("/", GetAllSections)
            .WithName("GetAllSections")
            .WithSummary("Get all sections")
            .WithDescription("Returns all sections with their collections and metadata")
            .Produces<IEnumerable<Section>>(StatusCodes.Status200OK);

        group.MapGet("/{sectionName}", GetSectionByName)
            .WithName("GetSectionByName")
            .WithSummary("Get section by name")
            .WithDescription("Returns a single section with its collections and metadata")
            .Produces<Section>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{sectionName}/collections", GetSectionCollections)
            .WithName("GetSectionCollections")
            .WithSummary("Get all collections in a section")
            .WithDescription("Returns all collection references for this section")
            .Produces<IEnumerable<Collection>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        // ============================================================
        // Collection-level endpoints
        // ============================================================

        group.MapGet("/{sectionName}/collections/{collectionName}", GetSectionCollection)
            .WithName("GetSectionCollection")
            .WithSummary("Get collection details")
            .WithDescription("Returns details of a specific collection within this section")
            .Produces<Collection>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{sectionName}/collections/{collectionName}/items", GetCollectionItems)
            .WithName("GetCollectionItems")
            .WithSummary("Get items in a collection")
            .WithDescription($"Returns content items from a collection with optional filtering. " +
                $"Supports: take (default={DefaultPageSize}, max={MaxPageSize}), skip, q (search), tags, subcollection, lastDays, includeDraft.")
            .Produces<IEnumerable<ContentItem>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{sectionName}/collections/{collectionName}/tags", GetCollectionTags)
            .WithName("GetCollectionTags")
            .WithSummary("Get tag cloud for a collection")
            .WithDescription("Returns top tags with usage counts for tag cloud rendering")
            .Produces<IReadOnlyList<TagCloudItem>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        // ============================================================
        // Content item detail endpoint
        // ============================================================

        group.MapGet("/{sectionName}/collections/{collectionName}/{slug}", GetContentDetail)
            .WithName("GetContentDetail")
            .WithSummary("Get content item detail")
            .WithDescription("Returns full content item including rendered HTML for content pages")
            .Produces<ContentItemDetail>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }

    // ================================================================
    // Section-level endpoint implementations
    // ================================================================

    /// <summary>
    /// GET /api/sections - Get all sections
    /// </summary>
    private static async Task<Ok<IEnumerable<Section>>> GetAllSections(
        IContentRepository contentRepository,
        CancellationToken cancellationToken)
    {
        var sections = await contentRepository.GetAllSectionsAsync(cancellationToken);
        return TypedResults.Ok(sections.AsEnumerable());
    }

    /// <summary>
    /// GET /api/sections/{sectionName} - Get section by name
    /// </summary>
    private static async Task<Results<Ok<Section>, NotFound>> GetSectionByName(
        string sectionName,
        IContentRepository contentRepository,
        CancellationToken cancellationToken)
    {
        var section = await contentRepository.GetSectionByNameAsync(sectionName, cancellationToken);

        if (section == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(section);
    }

    /// <summary>
    /// GET /api/sections/{sectionName}/collections - Get all collections in a section
    /// </summary>
    private static async Task<Results<Ok<IEnumerable<Collection>>, NotFound>> GetSectionCollections(
        string sectionName,
        IContentRepository contentRepository,
        CancellationToken cancellationToken)
    {
        var section = await contentRepository.GetSectionByNameAsync(sectionName, cancellationToken);

        if (section == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(section.Collections.AsEnumerable());
    }

    // ================================================================
    // Collection-level endpoint implementations
    // ================================================================

    /// <summary>
    /// GET /api/sections/{sectionName}/collections/{collectionName} - Get collection details
    /// </summary>
    private static async Task<Results<Ok<Collection>, NotFound>> GetSectionCollection(
        string sectionName,
        string collectionName,
        IContentRepository contentRepository,
        CancellationToken cancellationToken)
    {
        var section = await contentRepository.GetSectionByNameAsync(sectionName, cancellationToken);

        if (section == null)
        {
            return TypedResults.NotFound();
        }

        var collection = section.Collections.FirstOrDefault(c =>
            c.Name.Equals(collectionName, StringComparison.OrdinalIgnoreCase));

        if (collection == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(collection);
    }

    /// <summary>
    /// GET /api/sections/{sectionName}/collections/{collectionName}/items - Get items in a collection
    /// Supports "all" as a virtual collection name - returns all items in the section.
    /// </summary>
    private static async Task<Results<Ok<IEnumerable<ContentItem>>, NotFound>> GetCollectionItems(
        string sectionName,
        string collectionName,
        [FromQuery] int take = DefaultPageSize,
        [FromQuery] int skip = 0,
        [FromQuery] string? q = null,
        [FromQuery] string? tags = null,
        [FromQuery] string? subcollection = null,
        [FromQuery] int? lastDays = null,
        [FromQuery] bool includeDraft = false,
        IContentRepository contentRepository = default!,
        CancellationToken cancellationToken = default)
    {
        // Verify section exists
        var section = await contentRepository.GetSectionByNameAsync(sectionName, cancellationToken);
        if (section == null)
        {
            return TypedResults.NotFound();
        }

        // "all" is a virtual collection - returns all items in section
        var isAllCollection = string.Equals(collectionName, "all", StringComparison.OrdinalIgnoreCase);

        // Verify collection exists (skip for virtual "all" collection)
        if (!isAllCollection)
        {
            var hasCollection = section.Collections.Any(c =>
                c.Name.Equals(collectionName, StringComparison.OrdinalIgnoreCase));

            if (!hasCollection)
            {
                return TypedResults.NotFound();
            }
        }

        // Enforce pagination limits
        var limit = Math.Clamp(take, 1, MaxPageSize);
        var offset = Math.Max(skip, 0);

        // If filtering by query or tags, use SearchAsync
        if (!string.IsNullOrWhiteSpace(q) || !string.IsNullOrWhiteSpace(tags))
        {
            var request = new SearchRequest
            {
                Query = q,
                Sections = [section.Name],
                Collections = isAllCollection ? null : [collectionName],
                Tags = string.IsNullOrWhiteSpace(tags)
                    ? null
                    : tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
                DateFrom = lastDays.HasValue
                    ? DateTimeOffset.UtcNow.AddDays(-lastDays.Value)
                    : null,
                Take = limit
            };

            var searchResults = await contentRepository.SearchAsync(request, cancellationToken);
            return TypedResults.Ok(searchResults.Items.Skip(offset).AsEnumerable());
        }

        // Simple collection query - use indexed GetBySectionAsync
        // For "all" virtual collection, pass null to get all items in section
        var content = await contentRepository.GetBySectionAsync(
            section.Name,
            limit: limit,
            offset: offset,
            collectionName: isAllCollection ? null : collectionName,
            subcollectionName: subcollection,
            includeDraft: includeDraft,
            ct: cancellationToken);

        return TypedResults.Ok(content.AsEnumerable());
    }

    /// <summary>
    /// GET /api/sections/{sectionName}/collections/{collectionName}/tags - Get tag cloud for a collection
    /// Supports "all" as a virtual collection name - falls back to section-level tags.
    /// </summary>
    private static async Task<Results<Ok<IReadOnlyList<TagCloudItem>>, NotFound>> GetCollectionTags(
        string sectionName,
        string collectionName,
        [FromQuery] int? maxTags = null,
        [FromQuery] int? minUses = null,
        [FromQuery] int? lastDays = null,
        IContentRepository contentRepository = default!,
        IOptions<FilteringOptions> filteringOptions = default!,
        CancellationToken cancellationToken = default)
    {
        // Verify section exists
        var section = await contentRepository.GetSectionByNameAsync(sectionName, cancellationToken);
        if (section == null)
        {
            return TypedResults.NotFound();
        }

        // "all" is a virtual collection - use section scope instead
        var isAllCollection = string.Equals(collectionName, "all", StringComparison.OrdinalIgnoreCase);

        // Verify collection exists (skip for virtual "all" collection)
        if (!isAllCollection)
        {
            var hasCollection = section.Collections.Any(c =>
                c.Name.Equals(collectionName, StringComparison.OrdinalIgnoreCase));

            if (!hasCollection)
            {
                return TypedResults.NotFound();
            }
        }

        var options = filteringOptions.Value;

        // Calculate date filter if LastDays is specified
        DateTimeOffset? dateFrom = (lastDays ?? options.TagCloud.DefaultDateRangeDays) > 0
            ? DateTimeOffset.UtcNow.AddDays(-(lastDays ?? options.TagCloud.DefaultDateRangeDays))
            : null;

        // Determine section/collection filters
        // "all" is a virtual section/collection - means no filter
        string? sectionFilter = !isAllCollection &&
                                !string.IsNullOrWhiteSpace(sectionName) &&
                                !sectionName.Equals("all", StringComparison.OrdinalIgnoreCase)
            ? sectionName
            : null;

        string? collectionFilter = !isAllCollection &&
                                   !string.IsNullOrWhiteSpace(collectionName)
            ? collectionName
            : null;

        // Get top N tag counts from repository
        var tagCounts = await contentRepository.GetTagCountsAsync(
            dateFrom: dateFrom,
            dateTo: null,
            sectionName: sectionFilter,
            collectionName: collectionFilter,
            maxTags: maxTags ?? options.TagCloud.DefaultMaxTags,
            minUses: minUses ?? options.TagCloud.MinimumTagUses,
            ct: cancellationToken);

        if (tagCounts.Count == 0)
        {
            return TypedResults.Ok<IReadOnlyList<TagCloudItem>>([]);
        }

        // Apply quantile-based sizing to top N tags
        var tagCloud = ApplyQuantileSizing([.. tagCounts], options.TagCloud.QuantilePercentiles);

        return TypedResults.Ok(tagCloud);
    }

    /// <summary>
    /// Apply quantile-based sizing to tag cloud items.
    /// Top 25% = Large, Middle 50% = Medium, Bottom 25% = Small.
    /// </summary>
    private static IReadOnlyList<TagCloudItem> ApplyQuantileSizing(
        List<TagWithCount> sortedTags,
        QuantilePercentilesOptions quantileOptions)
    {
        if (sortedTags.Count == 0)
        {
            return [];
        }

        var smallToMedium = quantileOptions.SmallToMedium;
        var mediumToLarge = quantileOptions.MediumToLarge;

        var totalTags = sortedTags.Count;
        var smallThreshold = (int)Math.Ceiling(totalTags * smallToMedium);
        var largeThreshold = (int)Math.Ceiling(totalTags * mediumToLarge);

        var result = new List<TagCloudItem>();

        for (int i = 0; i < sortedTags.Count; i++)
        {
            var tag = sortedTags[i];
            TagSize size;

            // Top 25% (0-25% index) = Large
            // Middle 50% (25%-75% index) = Medium
            // Bottom 25% (75%-100% index) = Small
            if (i < smallThreshold)
            {
                size = TagSize.Large;
            }
            else if (i < largeThreshold)
            {
                size = TagSize.Medium;
            }
            else
            {
                size = TagSize.Small;
            }

            result.Add(new TagCloudItem
            {
                Tag = tag.Tag,
                Count = tag.Count,
                Size = size
            });
        }

        return result;
    }

    // ================================================================
    // Content item detail endpoint implementation
    // ================================================================

    /// <summary>
    /// GET /api/sections/{sectionName}/collections/{collectionName}/{slug} - Get content item detail
    /// Returns ContentItemDetail with full rendered HTML for content pages.
    /// </summary>
    private static async Task<Results<Ok<ContentItemDetail>, NotFound>> GetContentDetail(
        string sectionName,
        string collectionName,
        string slug,
        IContentRepository contentRepository,
        CancellationToken cancellationToken)
    {
        // Verify section exists
        var section = await contentRepository.GetSectionByNameAsync(sectionName, cancellationToken);
        if (section == null)
        {
            return TypedResults.NotFound();
        }

        // Verify collection exists in this section
        var hasCollection = section.Collections.Any(c =>
            c.Name.Equals(collectionName, StringComparison.OrdinalIgnoreCase));

        if (!hasCollection)
        {
            return TypedResults.NotFound();
        }

        // Get the content item detail by collection and slug
        var item = await contentRepository.GetBySlugAsync(collectionName, slug, includeDraft: false, cancellationToken);

        if (item == null)
        {
            return TypedResults.NotFound();
        }

        // Check if this content item has an external URL - if so, return 404 when accessed via internal route
        if (item.LinksExternally())
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(item);
    }
}
