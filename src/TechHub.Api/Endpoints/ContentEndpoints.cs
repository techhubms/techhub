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
public static class ContentEndpoints
{

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
    public static IEndpointRouteBuilder MapContentEndpoints(this IEndpointRouteBuilder endpoints)
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
            .WithDescription("Returns content items from a collection with optional filtering. " +
                "Supports: take (default configured in appsettings), skip, q (search), tags, subcollection, lastDays, from, to, includeDraft.")
            .Produces<IEnumerable<ContentItem>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);

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
    private static async Task<Results<Ok<CollectionItemsResponse>, NotFound, BadRequest<string>>> GetCollectionItems(
        string sectionName,
        string collectionName,
        IOptions<ApiOptions> apiOptions,
        IOptions<TagCloudOptions> tagCloudOptions,
        IContentRepository contentRepository,
        [FromQuery] int? take = null,
        [FromQuery] int skip = 0,
        [FromQuery] string? q = null,
        [FromQuery] string? tags = null,
        [FromQuery] string? subcollection = null,
        [FromQuery] int? lastDays = null,
        [FromQuery] string? from = null,
        [FromQuery] string? to = null,
        [FromQuery] bool includeDraft = false,
        CancellationToken cancellationToken = default)
    {
        // Verify section exists
        var section = await contentRepository.GetSectionByNameAsync(sectionName, cancellationToken);
        if (section == null)
        {
            return TypedResults.NotFound();
        }

        // "all" is a virtual collection - verify it exists unless it's "all"
        var isAllCollection = string.Equals(collectionName, "all", StringComparison.OrdinalIgnoreCase);
        if (!isAllCollection)
        {
            var hasCollection = section.Collections.Any(c =>
                c.Name.Equals(collectionName, StringComparison.OrdinalIgnoreCase));

            if (!hasCollection)
            {
                return TypedResults.NotFound();
            }
        }

        // Enforce pagination limits using configured values
        var options = apiOptions.Value;
        var limit = Math.Clamp(take ?? options.DefaultPageSize, 1, options.MaxPageSize);
        var offset = Math.Max(skip, 0);

        // Parse explicit date range parameters (from/to take precedence over lastDays)
        DateTimeOffset? dateFrom = null;
        DateTimeOffset? dateTo = null;

        if (!string.IsNullOrWhiteSpace(from))
        {
            if (!DateTimeOffset.TryParse(from, null, System.Globalization.DateTimeStyles.AssumeUniversal, out var parsedFrom))
            {
                return TypedResults.BadRequest($"Invalid 'from' date format: {from}. Expected ISO 8601 format (e.g., 2024-01-15).");
            }

            dateFrom = parsedFrom;
        }

        if (!string.IsNullOrWhiteSpace(to))
        {
            if (!DateTimeOffset.TryParse(to, null, System.Globalization.DateTimeStyles.AssumeUniversal, out var parsedTo))
            {
                return TypedResults.BadRequest($"Invalid 'to' date format: {to}. Expected ISO 8601 format (e.g., 2024-01-15).");
            }

            dateTo = parsedTo;
        }

        // Fall back to lastDays if from/to not specified, then to configured default
        if (dateFrom == null && dateTo == null && (lastDays ?? tagCloudOptions.Value.DefaultDateRangeDays) > 0)
        {
            dateFrom = DateTimeOffset.UtcNow.AddDays(-(lastDays ?? tagCloudOptions.Value.DefaultDateRangeDays));
        }

        // Defensive: swap dates if from > to (can happen due to client race conditions)
        if (dateFrom.HasValue && dateTo.HasValue && dateFrom > dateTo)
        {
            (dateFrom, dateTo) = (dateTo, dateFrom);
        }

        // Build search request - repository will handle "all" as no filter
        var request = new SearchRequest(
            take: limit,
            skip: offset,
            query: q,
            sections: [section.Name],
            collections: [collectionName],  // Pass "all" through, repo handles it
            tags: string.IsNullOrWhiteSpace(tags)
                ? []
                : tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
            subcollection: subcollection,
            dateFrom: dateFrom,
            dateTo: dateTo,
            includeDraft: includeDraft
        );

        var content = await contentRepository.SearchAsync(request, cancellationToken);
        return TypedResults.Ok(new CollectionItemsResponse(content.Items, content.TotalCount));
    }

    /// <summary>
    /// GET /api/sections/{sectionName}/collections/{collectionName}/tags - Get tag cloud for a collection
    /// Supports "all" as a virtual collection name - falls back to section-level tags.
    /// Supports dynamic tag counts based on current filter state (tags, date range).
    /// </summary>
    private static async Task<Results<Ok<IReadOnlyList<TagCloudItem>>, NotFound, BadRequest<string>>> GetCollectionTags(
        string sectionName,
        string collectionName,
        [FromQuery] int? maxTags = null,
        [FromQuery] int? minUses = null,
        [FromQuery] int? lastDays = null,
        [FromQuery] string? tags = null,
        [FromQuery] string? tagsToCount = null,
        [FromQuery] string? from = null,
        [FromQuery] string? to = null,
        [FromQuery] string? q = null,
        IContentRepository contentRepository = default!,
        IOptions<TagCloudOptions> tagCloudOptions = default!,
        CancellationToken cancellationToken = default)
    {
        // Verify section exists
        var section = await contentRepository.GetSectionByNameAsync(sectionName, cancellationToken);
        if (section == null)
        {
            return TypedResults.NotFound();
        }

        // "all" is a virtual collection - verify it exists unless it's "all"
        var isAllCollection = string.Equals(collectionName, "all", StringComparison.OrdinalIgnoreCase);
        if (!isAllCollection)
        {
            var hasCollection = section.Collections.Any(c =>
                c.Name.Equals(collectionName, StringComparison.OrdinalIgnoreCase));

            if (!hasCollection)
            {
                return TypedResults.NotFound();
            }
        }

        var options = tagCloudOptions.Value;

        // Parse date range parameters (from/to take precedence over lastDays)
        DateTimeOffset? dateFrom = null;
        DateTimeOffset? dateTo = null;

        if (!string.IsNullOrWhiteSpace(from))
        {
            if (!DateTimeOffset.TryParse(from, null, System.Globalization.DateTimeStyles.AssumeUniversal, out var parsedFrom))
            {
                return TypedResults.BadRequest($"Invalid 'from' date format: {from}. Expected ISO 8601 format (e.g., 2024-01-15).");
            }

            dateFrom = parsedFrom;
        }

        if (!string.IsNullOrWhiteSpace(to))
        {
            if (!DateTimeOffset.TryParse(to, null, System.Globalization.DateTimeStyles.AssumeUniversal, out var parsedTo))
            {
                return TypedResults.BadRequest($"Invalid 'to' date format: {to}. Expected ISO 8601 format (e.g., 2024-01-15).");
            }

            dateTo = parsedTo;
        }

        // Fall back to lastDays if from/to not specified
        if (dateFrom == null && dateTo == null && (lastDays ?? options.DefaultDateRangeDays) > 0)
        {
            dateFrom = DateTimeOffset.UtcNow.AddDays(-(lastDays ?? options.DefaultDateRangeDays));
        }

        // Defensive: swap dates if from > to (can happen due to client race conditions)
        if (dateFrom.HasValue && dateTo.HasValue && dateFrom > dateTo)
        {
            (dateFrom, dateTo) = (dateTo, dateFrom);
        }

        // Parse selected tags filter (for dynamic counts)
        List<string>? selectedTags = null;
        if (!string.IsNullOrWhiteSpace(tags))
        {
            selectedTags = [.. tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)];
        }

        // Parse tagsToCount filter (for getting counts of specific baseline tags)
        List<string>? parsedTagsToCount = null;
        if (!string.IsNullOrWhiteSpace(tagsToCount))
        {
            parsedTagsToCount = [.. tagsToCount.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)];
        }

        // Get top N tag counts from repository - repository will handle "all" as no filter
        var tagCounts = await contentRepository.GetTagCountsAsync(
            new TagCountsRequest(
                sectionName: sectionName,
                collectionName: collectionName,
                minUses: minUses ?? options.MinimumTagUses,
                maxTags: maxTags ?? options.DefaultMaxTags,
                dateFrom: dateFrom,
                dateTo: dateTo,
                tags: selectedTags,
                tagsToCount: parsedTagsToCount,
                searchQuery: string.IsNullOrWhiteSpace(q) ? null : q.Trim()
            ),
            cancellationToken);

        if (tagCounts.Count == 0)
        {
            return TypedResults.Ok<IReadOnlyList<TagCloudItem>>([]);
        }

        // Apply quantile-based sizing to top N tags
        var tagCloud = ApplyQuantileSizing([.. tagCounts], options.QuantilePercentiles);

        return TypedResults.Ok(tagCloud);
    }

    /// <summary>
    /// Apply quantile-based sizing to tag cloud items using count value thresholds.
    /// Tags with high counts get Large size, low counts get Small, middle counts get Medium.
    /// When counts are very similar, most tags will have the same size.
    /// When the quantile algorithm produces fewer than 3 distinct size groups, sizes are
    /// normalized: 1 group → all Medium, 2 groups → Medium + Small, 3 groups → Large + Medium + Small.
    /// </summary>
    public static IReadOnlyList<TagCloudItem> ApplyQuantileSizing(
        List<TagWithCount> sortedTags,
        QuantilePercentilesOptions quantileOptions)
    {
        ArgumentNullException.ThrowIfNull(sortedTags);
        ArgumentNullException.ThrowIfNull(quantileOptions);

        if (sortedTags.Count == 0)
        {
            return [];
        }

        // For 1-2 tags, use consistent sizing
        if (sortedTags.Count <= 2)
        {
            return [.. sortedTags.Select(tag => new TagCloudItem
            {
                Tag = tag.Tag,
                Count = tag.Count,
                Size = TagSize.Medium
            })];
        }

        // Calculate quantile thresholds based on COUNT VALUES, not positions
        // This ensures tags with similar counts get similar sizes
        var counts = sortedTags.Select(t => t.Count).ToList();

        // Calculate thresholds at quantile positions
        // Position 25% in descending list = boundary between Large and Medium (HIGH count)
        var highThresholdIndex = (int)Math.Ceiling(counts.Count * quantileOptions.SmallToMedium);
        var highThresholdCount = counts[Math.Min(highThresholdIndex, counts.Count - 1)];

        // Position 75% in descending list = boundary between Medium and Small (LOW count)
        var lowThresholdIndex = (int)Math.Ceiling(counts.Count * quantileOptions.MediumToLarge);
        var lowThresholdCount = counts[Math.Min(lowThresholdIndex, counts.Count - 1)];

        var result = new List<TagCloudItem>();

        foreach (var tag in sortedTags)
        {
            TagSize size;

            // Assign size based on count VALUE thresholds (not position)
            if (tag.Count >= highThresholdCount)
            {
                size = TagSize.Large;
            }
            else if (tag.Count >= lowThresholdCount)
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

        // Normalize sizes based on how many distinct size groups the quantile algorithm produced.
        // When data has limited variation, the algorithm may collapse into fewer groups.
        // The percentile thresholds shift based on the number of groups:
        // 1 group:  all tags get Medium (regular size - avoids misleading emphasis)
        // 2 groups: recalculate with single 50th percentile threshold → Medium + Small
        // 3 groups: keep Large + Medium + Small as-is (original 25th/75th thresholds)
        var distinctSizes = result.Select(t => t.Size).Distinct().ToList();

        if (distinctSizes.Count == 1)
        {
            // All tags ended up in the same group → normalize to Medium
            return [.. result.Select(t => t with { Size = TagSize.Medium })];
        }

        if (distinctSizes.Count == 2)
        {
            // Recalculate with a single threshold at the 50th percentile (median)
            // for a balanced split between Medium and Small
            var medianIndex = (int)Math.Ceiling(counts.Count * 0.5);
            var medianThreshold = counts[Math.Min(medianIndex, counts.Count - 1)];

            return [.. sortedTags.Select(tag => new TagCloudItem
            {
                Tag = tag.Tag,
                Count = tag.Count,
                Size = tag.Count >= medianThreshold ? TagSize.Medium : TagSize.Small
            })];
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
