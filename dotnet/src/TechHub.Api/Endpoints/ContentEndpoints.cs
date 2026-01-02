using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TechHub.Core.DTOs;
using TechHub.Core.Interfaces;

namespace TechHub.Api.Endpoints;

/// <summary>
/// API endpoints for advanced content filtering and search
/// </summary>
public static class ContentEndpoints
{
    /// <summary>
    /// Maps all content-related endpoints to the application
    /// </summary>
    public static IEndpointRouteBuilder MapContentEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/content")
            .WithTags("Content")
            .WithDescription("Endpoints for advanced content filtering and search");

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
    /// GET /api/content/filter - Advanced content filtering
    /// Supports filtering by: sections (category names), collections, tags, and text search
    /// Example: /api/content/filter?sections=AI,ML&collections=news,blogs&tags=copilot,azure&q=github
    /// </summary>
    private static async Task<Ok<IEnumerable<ContentItemDto>>> FilterContent(
        [FromQuery] string? sections,
        [FromQuery] string? collections,
        [FromQuery] string? tags,
        [FromQuery] string? q,
        ISectionRepository sectionRepository,
        IContentRepository contentRepository,
        CancellationToken cancellationToken)
    {
        // Start with all content
        var content = await contentRepository.GetAllAsync(cancellationToken);
        var results = content.AsEnumerable();

        // Filter by sections (category names)
        if (!string.IsNullOrWhiteSpace(sections))
        {
            var sectionNames = sections.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var allSections = await sectionRepository.GetAllAsync(cancellationToken);
            
            // Map section names to categories
            var categories = allSections
                .Where(s => sectionNames.Contains(s.Id, StringComparer.OrdinalIgnoreCase))
                .Select(s => s.Category)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            
            results = results.Where(c => c.Categories.Any(cat => categories.Contains(cat)));
        }

        // Filter by collections
        if (!string.IsNullOrWhiteSpace(collections))
        {
            var collectionNames = collections.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            results = results.Where(c => collectionNames.Contains(c.Collection, StringComparer.OrdinalIgnoreCase));
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
        return new ContentItemDto
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            Author = item.Author,
            DateEpoch = item.DateEpoch,
            DateIso = item.DateIso,
            Collection = item.Collection,
            AltCollection = item.AltCollection,
            Categories = item.Categories,
            Tags = item.Tags,
            Excerpt = item.Excerpt,
            ExternalUrl = item.ExternalUrl,
            VideoId = item.VideoId,
            Url = $"/{item.Collection}/{item.Id}" // Generate URL from collection and ID
        };
    }
}
