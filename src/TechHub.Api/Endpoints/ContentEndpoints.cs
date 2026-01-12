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

        // Get content by section and collection
        group.MapGet("", GetContent)
            .WithName("GetContent")
            .WithSummary("Get content by section and collection")
            .WithDescription("Get all content items for a specific section and collection. Example: /api/content?sectionName=AI&collectionName=news")
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
    /// GET /api/content?sectionName={sectionName}&collectionName={collectionName} - Get content by section and collection
    /// </summary>
    private static async Task<Ok<IEnumerable<ContentItemDto>>> GetContent(
        [FromQuery] string? sectionName,
        [FromQuery] string? collectionName,
        IContentRepository contentRepository,
        CancellationToken cancellationToken)
    {
        // Use targeted repository methods for better database performance
        IReadOnlyList<Core.Models.ContentItem> content;

        if (!string.IsNullOrWhiteSpace(sectionName) && !string.IsNullOrWhiteSpace(collectionName))
        {
            // Both filters: get by collection first (smaller dataset), then filter by section title
            var collectionContent = await contentRepository.GetByCollectionAsync(collectionName, cancellationToken);
            content = [.. collectionContent.Where(c => c.SectionNames.Contains(sectionName, StringComparer.OrdinalIgnoreCase))];
        }
        else if (!string.IsNullOrWhiteSpace(sectionName))
        {
            // Section filter only
            content = await contentRepository.GetBySectionAsync(sectionName, cancellationToken);
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
        // Get the section data
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
        // "All" section accepts all content, specific sections only accept matching section names
        // ContentItem.SectionNames contains lowercase section names (e.g., "ai", "github-copilot")
        var isValidForSection = section.Name.Equals("all", StringComparison.OrdinalIgnoreCase) ||
                                item.SectionNames.Contains(section.Name, StringComparer.OrdinalIgnoreCase);

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
            SectionNames = item.SectionNames,
            PrimarySection = TechHub.Core.Helpers.SectionPriorityHelper.GetPrimarySectionName(item.SectionNames, item.CollectionName),
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
    /// Supports filtering by: sections (section names), collections, tags, and text search
    /// Example: /api/content/filter?sections=ai,github-copilot&collections=news,blogs&tags=copilot,azure&q=github
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

        // Filter by sections (section names in content Sections property)
        if (!string.IsNullOrWhiteSpace(sections))
        {
            var sectionNames = sections.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var allSections = await sectionRepository.GetAllAsync(cancellationToken);

            // Get section titles (what's stored in content Sections property) from section names (URL slugs)
            var validSectionTitles = allSections
                .Where(s => sectionNames.Contains(s.Name, StringComparer.OrdinalIgnoreCase))
                .Select(s => s.Title)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            results = results.Where(c => c.SectionNames.Any(sectionTitle => validSectionTitles.Contains(sectionTitle)));
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
        var primarySectionUrl = TechHub.Core.Helpers.SectionPriorityHelper.GetPrimarySectionUrl(item.SectionNames, item.CollectionName);

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
            SectionNames = item.SectionNames,
            PrimarySection = TechHub.Core.Helpers.SectionPriorityHelper.GetPrimarySectionName(item.SectionNames, item.CollectionName),
            Tags = item.Tags,
            Excerpt = item.Excerpt,
            ExternalUrl = item.ExternalUrl,
            VideoId = item.VideoId,
            ViewingMode = item.ViewingMode,
            Url = $"/{primarySectionUrl.ToLowerInvariant()}/{item.CollectionName.ToLowerInvariant()}/{item.Slug.ToLowerInvariant()}"
        };
    }
}
