using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;

namespace TechHub.Api.Endpoints;

/// <summary>
/// API endpoints for advanced content filtering and search
/// </summary>
internal static class ContentEndpoints
{
    /// <summary>
    /// Maps all content-related endpoints to the application
    /// </summary>
    public static IEndpointRouteBuilder MapContentEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/content")
            .WithTags("Content")
            .WithDescription("Endpoints for advanced content filtering and search");

        // Get content by section and collection
        group.MapGet("", GetContent)
            .WithName("GetContent")
            .WithSummary("Get content by section and collection")
            .WithDescription("Get all content items for a specific section and collection. Example: /api/content?sectionName=AI&collectionName=news")
            .Produces<IEnumerable<ContentItem>>(StatusCodes.Status200OK);

        // Get individual content detail
        group.MapGet("/{sectionName}/{collectionName}/{slug}", GetContentDetail)
            .WithName("GetContentDetail")
            .WithSummary("Get content detail")
            .WithDescription("Get detailed content item by section name, collection name, and content slug")
            .Produces<ContentItem>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        // Advanced filtering endpoint
        group.MapGet("/filter", FilterContent)
            .WithName("FilterContent")
            .WithSummary("Advanced content filtering")
            .WithDescription("Filter content by multiple criteria: sections, collections, tags, search query. Example: /api/content/filter?sections=ai,ml&collections=news,blogs&tags=copilot,azure")
            .Produces<IEnumerable<ContentItem>>(StatusCodes.Status200OK);

        // Database-optimized search endpoint with pagination
        group.MapGet("/search", SearchContent)
            .WithName("SearchContent")
            .WithSummary("Database-optimized content search")
            .WithDescription("Search content with database-level filtering and pagination. Supports: sections, collections, tags, date range, take limit. Example: /api/content/search?collections=roundups&take=1")
            .Produces<SearchResults<ContentItem>>(StatusCodes.Status200OK);

        return endpoints;
    }

    /// <summary>
    /// GET /api/content?sectionName={sectionName}&amp;collectionName={collectionName} - Get content by section and collection
    /// </summary>
    private static async Task<Ok<IEnumerable<ContentItem>>> GetContent(
        [FromQuery] string? sectionName,
        [FromQuery] string? collectionName,
        [FromQuery] bool? ghcFeature,
        IContentRepository contentRepository,
        CancellationToken cancellationToken)
    {
        // Special case: When requesting GitHub Copilot features, include drafts
        // This allows the features page to show "Coming Soon" items
        var includeDraft = ghcFeature == true;

        // Use targeted repository methods for better database performance
        IReadOnlyList<Core.Models.ContentItem> content;

        if (!string.IsNullOrWhiteSpace(sectionName) && !string.IsNullOrWhiteSpace(collectionName))
        {
            // Both filters: get by section and filter by collection
            var sectionContent = await contentRepository.GetBySectionAsync(sectionName, includeDraft, cancellationToken);
            content = [.. sectionContent.Where(c => c.CollectionName.Equals(collectionName, StringComparison.OrdinalIgnoreCase))];

            // Filter by ghc_feature if specified
            if (ghcFeature.HasValue)
            {
                content = [.. content.Where(c => c.GhcFeature == ghcFeature.Value)];
            }
        }
        else if (!string.IsNullOrWhiteSpace(sectionName))
        {
            // Section filter only
            content = await contentRepository.GetBySectionAsync(sectionName, includeDraft, cancellationToken);

            // Filter by ghc_feature if specified
            if (ghcFeature.HasValue)
            {
                content = [.. content.Where(c => c.GhcFeature == ghcFeature.Value)];
            }
        }
        else if (!string.IsNullOrWhiteSpace(collectionName))
        {
            // Collection only - include drafts if requesting ghc features
            content = await contentRepository.GetByCollectionAsync(collectionName, includeDraft, cancellationToken);

            // Filter by ghc_feature if specified
            if (ghcFeature.HasValue)
            {
                content = [.. content.Where(c => c.GhcFeature == ghcFeature.Value)];
            }
        }
        else
        {
            // No filters: get all content (exclude drafts unless ghcFeature=true)
            content = await contentRepository.GetAllAsync(includeDraft, cancellationToken);

            // Filter by ghc_feature if specified
            if (ghcFeature.HasValue)
            {
                content = [.. content.Where(c => c.GhcFeature == ghcFeature.Value)];
            }
        }

        return TypedResults.Ok(content.AsEnumerable());
    }

    /// <summary>
    /// GET /api/content/{sectionName}/{collectionName}/{slug} - Get individual content detail
    /// </summary>
    private static async Task<Results<Ok<ContentItem>, NotFound>> GetContentDetail(
        string sectionName,
        string collectionName,
        string slug,
        ISectionRepository sectionRepository,
        IContentRepository contentRepository,
        CancellationToken cancellationToken)
    {
        // Get the section data
        var section = await sectionRepository.GetByNameAsync(sectionName, cancellationToken);
        if (section == null)
        {
            return TypedResults.NotFound();
        }

        // Get the specific content item by collection and slug (database-friendly approach)
        // Exclude drafts unless this is a preview/admin request
        var item = await contentRepository.GetBySlugAsync(collectionName, slug, includeDraft: false, cancellationToken);

        if (item == null)
        {
            return TypedResults.NotFound();
        }

        // Validate that the item belongs to the requested section
        // "All" section accepts all content, specific sections only accept matching section names
        // ContentItem.SectionNames contains lowercase section names (e.g., "ai", "github-copilot")
        var isValidForSection = section.Name.Equals("all", StringComparison.OrdinalIgnoreCase) ||
                                item.SectionNames.Contains(section.Name, StringComparer.OrdinalIgnoreCase);

        if (!isValidForSection)
        {
            return TypedResults.NotFound();
        }

        // Return the content item directly - repository already populates all fields including Url
        return TypedResults.Ok(item);
    }

    /// <summary>
    /// GET /api/content/filter - Advanced content filtering
    /// Supports filtering by: sections (section names), collections, tags, and text search
    /// Example: /api/content/filter?sections=ai,github-copilot&amp;collections=news,blogs&amp;tags=copilot,azure&amp;q=github
    /// </summary>
    /// <remarks>
    /// NOTE: This endpoint loads all content for complex filtering. 
    /// Current implementation is file-based with caching, so this is acceptable.
    /// When migrating to a database, consider:
    /// 1. Building dynamic LINQ queries for better performance
    /// 2. Using a search index (e.g., Azure Cognitive Search, Elasticsearch) for text search
    /// 3. Implementing pagination for large result sets
    /// </remarks>
    private static async Task<Ok<IEnumerable<ContentItem>>> FilterContent(
        [FromQuery] string? sections,
        [FromQuery] string? collections,
        [FromQuery] string? tags,
        [FromQuery] string? q,
        IContentRepository contentRepository,
        CancellationToken cancellationToken)
    {
        // Start with all content (exclude drafts, acceptable for file-based implementation with caching)
        var content = await contentRepository.GetAllAsync(includeDraft: false, cancellationToken);
        var results = content.AsEnumerable();

        // Filter by sections (section names are already lowercase identifiers in SectionNames)
        if (!string.IsNullOrWhiteSpace(sections))
        {
            var sectionNames = sections.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            results = results.Where(c => c.SectionNames.Any(sectionName =>
                sectionNames.Contains(sectionName, StringComparer.OrdinalIgnoreCase)));
        }

        // Filter by collections
        if (!string.IsNullOrWhiteSpace(collections))
        {
            var collectionNames = collections.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            results = results.Where(c => collectionNames.Contains(c.CollectionName, StringComparer.OrdinalIgnoreCase));
        }

        // Filter by tags (content must have ALL specified tags)
        if (!string.IsNullOrWhiteSpace(tags))
        {
            var tagList = tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            results = results.Where(c => tagList.All(tag =>
                c.Tags.Contains(tag, StringComparer.OrdinalIgnoreCase)));
        }

        // Filter by text search query
        if (!string.IsNullOrWhiteSpace(q))
        {
            var query = q.ToLowerInvariant();
            results = results.Where(c =>
                c.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                c.Excerpt.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                c.Tags.Any(tag => tag.Contains(query, StringComparison.OrdinalIgnoreCase)));
        }

        return TypedResults.Ok(results);
    }

    /// <summary>
    /// GET /api/content/search - Database-optimized search with pagination
    /// Uses repository's SearchAsync for database-level filtering and limiting.
    /// Example: /api/content/search?collections=roundups&amp;take=1
    /// </summary>
    private static async Task<Ok<SearchResults<ContentItem>>> SearchContent(
        [FromQuery] string? sections,
        [FromQuery] string? collections,
        [FromQuery] string? tags,
        [FromQuery] string? q,
        [FromQuery] int? take,
        [FromQuery] int? lastDays,
        [FromQuery] string? orderBy,
        IContentRepository contentRepository,
        CancellationToken cancellationToken)
    {
        // Build the search request with database-level filtering
        var request = new SearchRequest
        {
            Query = q,
            Tags = string.IsNullOrWhiteSpace(tags)
                ? null
                : tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
            Sections = string.IsNullOrWhiteSpace(sections)
                ? null
                : sections.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
            Collections = string.IsNullOrWhiteSpace(collections)
                ? null
                : collections.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
            DateFrom = lastDays.HasValue
                ? DateTimeOffset.UtcNow.AddDays(-lastDays.Value)
                : null,
            Take = take ?? 20,
            OrderBy = orderBy ?? "date_desc"
        };

        var results = await contentRepository.SearchAsync(request, cancellationToken);
        return TypedResults.Ok(results);
    }
}
