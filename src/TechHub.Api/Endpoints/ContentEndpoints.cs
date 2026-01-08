using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TechHub.Core.DTOs;
using TechHub.Core.Interfaces;

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

        // Get content by category and collection
        group.MapGet("", GetContent)
            .WithName("GetContent")
            .WithSummary("Get content by category and collection")
            .WithDescription("Get all content items for a specific category and collection. Example: /api/content?category=ai&collectionName=news")
            .Produces<IEnumerable<ContentItemDto>>(StatusCodes.Status200OK);

        // Get individual content detail
        group.MapGet("/{sectionName}/{collectionName}/{slug}", GetContentDetail)
            .WithName("GetContentDetail")
            .WithSummary("Get content detail")
            .WithDescription("Get detailed content item by section name, collection name, and content slug")
            .Produces<ContentItemDetailDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        // Advanced filtering endpoint
        group.MapGet("/filter", FilterContent)
            .WithName("FilterContent")
            .WithSummary("Advanced content filtering")
            .WithDescription("Filter content by multiple criteria: sections, collections, tags, search query. Example: /api/content/filter?sections=ai,ml&collections=news,blogs&tags=copilot,azure")
            .Produces<IEnumerable<ContentItemDto>>(StatusCodes.Status200OK);

        // Get all tags for autocomplete/filtering UI
        group.MapGet("/tags", GetAllTags)
            .WithName("GetAllTags")
            .WithSummary("Get all tags")
            .WithDescription("Returns all unique tags across all content")
            .Produces<IEnumerable<string>>(StatusCodes.Status200OK);

        return endpoints;
    }

    /// <summary>
    /// GET /api/content?category={category}&collectionName={collectionName} - Get content by category and collection
    /// </summary>
    private static async Task<Ok<IEnumerable<ContentItemDto>>> GetContent(
        [FromQuery] string? category,
        [FromQuery] string? collectionName,
        IContentRepository contentRepository,
        CancellationToken cancellationToken)
    {
        // Use targeted repository methods for better database performance
        IReadOnlyList<Core.Models.ContentItem> content;
        
        if (!string.IsNullOrWhiteSpace(category) && !string.IsNullOrWhiteSpace(collectionName))
        {
            // Both filters: get by collection first (smaller dataset), then filter by category
            var collectionContent = await contentRepository.GetByCollectionAsync(collectionName, cancellationToken);
            content = collectionContent
                .Where(c => c.Categories.Contains(category, StringComparer.OrdinalIgnoreCase))
                .ToList();
        }
        else if (!string.IsNullOrWhiteSpace(category))
        {
            // Category only
            content = await contentRepository.GetByCategoryAsync(category, cancellationToken);
        }
        else if (!string.IsNullOrWhiteSpace(collectionName))
        {
            // Collection only
            content = await contentRepository.GetByCollectionAsync(collectionName, cancellationToken);
        }
        else
        {
            // No filters: get all content
            content = await contentRepository.GetAllAsync(cancellationToken);
        }

        var contentDtos = content.Select(MapToDto);
        return TypedResults.Ok(contentDtos);
    }

    /// <summary>
    /// GET /api/content/{sectionName}/{collectionName}/{slug} - Get individual content detail
    /// </summary>
    private static async Task<Results<Ok<ContentItemDetailDto>, NotFound>> GetContentDetail(
        string sectionName,
        string collectionName,
        string slug,
        ISectionRepository sectionRepository,
        IContentRepository contentRepository,
        IMarkdownService markdownService,
        CancellationToken cancellationToken)
    {
        // Get the section to find the category
        var section = await sectionRepository.GetByNameAsync(sectionName, cancellationToken);
        if (section == null)
        {
            return TypedResults.NotFound();
        }

        // Get the specific content item by collection and slug (database-friendly approach)
        var item = await contentRepository.GetBySlugAsync(collectionName, slug, cancellationToken);
        
        if (item == null)
        {
            return TypedResults.NotFound();
        }

        // Validate that the item belongs to the requested section
        // "All" section accepts all content, specific sections only accept matching categories
        var isValidForSection = section.Category.Equals("All", StringComparison.OrdinalIgnoreCase) ||
                                item.Categories.Contains(section.Category, StringComparer.OrdinalIgnoreCase);
        
        if (!isValidForSection)
        {
            return TypedResults.NotFound();
        }

        // Convert to detail DTO with full content HTML
        var detailDto = new ContentItemDetailDto
        {
            Slug = item.Slug,
            Title = item.Title,
            Description = item.Description,
            Author = item.Author,
            DateEpoch = item.DateEpoch,
            DateIso = item.DateIso,
            CollectionName = item.CollectionName,
            AltCollection = item.AltCollection,
            Categories = item.Categories,
            PrimarySection = TechHub.Core.Helpers.SectionPriorityHelper.GetPrimarySectionName(item.Categories, item.CollectionName),
            Tags = item.Tags,
            Excerpt = item.Excerpt,
            RenderedHtml = item.RenderedHtml,
            ExternalUrl = item.ExternalUrl,
            VideoId = item.VideoId,
            ViewingMode = item.ViewingMode,
            Url = $"/{sectionName}/{collectionName}/{slug}"
        };

        return TypedResults.Ok(detailDto);
    }

    /// <summary>
    /// GET /api/content/filter - Advanced content filtering
    /// Supports filtering by: sections (category names), collections, tags, and text search
    /// Example: /api/content/filter?sections=AI,ML&collections=news,blogs&tags=copilot,azure&q=github
    /// </summary>
    /// <remarks>
    /// NOTE: This endpoint loads all content for complex filtering. 
    /// Current implementation is file-based with caching, so this is acceptable.
    /// When migrating to a database, consider:
    /// 1. Building dynamic LINQ queries for better performance
    /// 2. Using a search index (e.g., Azure Cognitive Search, Elasticsearch) for text search
    /// 3. Implementing pagination for large result sets
    /// </remarks>
    private static async Task<Ok<IEnumerable<ContentItemDto>>> FilterContent(
        [FromQuery] string? sections,
        [FromQuery] string? collections,
        [FromQuery] string? tags,
        [FromQuery] string? q,
        ISectionRepository sectionRepository,
        IContentRepository contentRepository,
        CancellationToken cancellationToken)
    {
        // Start with all content (acceptable for file-based implementation with caching)
        var content = await contentRepository.GetAllAsync(cancellationToken);
        var results = content.AsEnumerable();

        // Filter by sections (category names)
        if (!string.IsNullOrWhiteSpace(sections))
        {
            var sectionNames = sections.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var allSections = await sectionRepository.GetAllAsync(cancellationToken);
            
            // Map section names to categories
            var categories = allSections
                .Where(s => sectionNames.Contains(s.Name, StringComparer.OrdinalIgnoreCase))
                .Select(s => s.Category)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            
            results = results.Where(c => c.Categories.Any(cat => categories.Contains(cat)));
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
                (c.Description?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                c.Tags.Any(tag => tag.Contains(query, StringComparison.OrdinalIgnoreCase)));
        }

        var contentDtos = results.Select(MapToDto);
        return TypedResults.Ok(contentDtos);
    }

    /// <summary>
    /// GET /api/content/tags - Get all tags
    /// </summary>
    private static async Task<Ok<IEnumerable<string>>> GetAllTags(
        IContentRepository contentRepository,
        CancellationToken cancellationToken)
    {
        var tags = await contentRepository.GetAllTagsAsync(cancellationToken);
        return TypedResults.Ok(tags.AsEnumerable());
    }

    /// <summary>
    /// Map ContentItem entity to ContentItemDto
    /// </summary>
    private static ContentItemDto MapToDto(Core.Models.ContentItem item)
    {
        var primarySectionUrl = TechHub.Core.Helpers.SectionPriorityHelper.GetPrimarySectionUrl(item.Categories, item.CollectionName);
        
        return new ContentItemDto
        {
            Slug = item.Slug,
            Title = item.Title,
            Description = item.Description,
            Author = item.Author,
            DateEpoch = item.DateEpoch,
            DateIso = item.DateIso,
            CollectionName = item.CollectionName,
            AltCollection = item.AltCollection,
            Categories = item.Categories,
            PrimarySection = TechHub.Core.Helpers.SectionPriorityHelper.GetPrimarySectionName(item.Categories, item.CollectionName),
            Tags = item.Tags,
            Excerpt = item.Excerpt,
            ExternalUrl = item.ExternalUrl,
            VideoId = item.VideoId,
            ViewingMode = item.ViewingMode,
            Url = $"/{primarySectionUrl.ToLowerInvariant()}/{item.CollectionName.ToLowerInvariant()}/{item.Slug.ToLowerInvariant()}"
        };
    }
}
