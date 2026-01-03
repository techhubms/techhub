using Microsoft.AspNetCore.Http.HttpResults;
using TechHub.Core.DTOs;
using TechHub.Core.Interfaces;

namespace TechHub.Api.Endpoints;

/// <summary>
/// API endpoints for sections - top-level organizational units
/// </summary>
internal static class SectionsEndpoints
{
    /// <summary>
    /// Maps all section-related endpoints to the application
    /// </summary>
    public static IEndpointRouteBuilder MapSectionsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/sections")
            .WithTags("Sections")
            .WithDescription("Endpoints for browsing content sections");

        // Base section endpoints
        group.MapGet("/", GetAllSections)
            .WithName("GetAllSections")
            .WithSummary("Get all sections")
            .WithDescription("Returns all sections with their collections and metadata")
            .Produces<IEnumerable<SectionDto>>(StatusCodes.Status200OK);

        group.MapGet("/{sectionName}", GetSectionById)
            .WithName("GetSectionById")
            .WithSummary("Get section by name")
            .WithDescription("Returns a single section with its collections and metadata")
            .Produces<SectionDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        // Nested: items in a section
        group.MapGet("/{sectionName}/items", GetSectionItems)
            .WithName("GetSectionItems")
            .WithSummary("Get all items in a section")
            .WithDescription("Returns all content items from all collections in this section")
            .Produces<IEnumerable<ContentItemDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        // Nested: collections in a section
        group.MapGet("/{sectionName}/collections", GetSectionCollections)
            .WithName("GetSectionCollections")
            .WithSummary("Get all collections in a section")
            .WithDescription("Returns all collection references for this section")
            .Produces<IEnumerable<CollectionReferenceDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        // Nested: specific collection in a section
        group.MapGet("/{sectionName}/collections/{collectionName}", GetSectionCollection)
            .WithName("GetSectionCollection")
            .WithSummary("Get collection details")
            .WithDescription("Returns details of a specific collection within this section")
            .Produces<CollectionReferenceDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        // Nested: items in a specific collection within a section
        group.MapGet("/{sectionName}/collections/{collectionName}/items", GetSectionCollectionItems)
            .WithName("GetSectionCollectionItems")
            .WithSummary("Get items in a collection within a section")
            .WithDescription("Returns all content items from a specific collection in this section")
            .Produces<IEnumerable<ContentItemDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }

    /// <summary>
    /// GET /api/sections - Get all sections
    /// </summary>
    private static async Task<Ok<IEnumerable<SectionDto>>> GetAllSections(
        ISectionRepository sectionRepository,
        CancellationToken cancellationToken)
    {
        var sections = await sectionRepository.GetAllAsync(cancellationToken);
        
        var sectionDtos = sections.Select(s => new SectionDto
        {
            Id = s.Id,
            Title = s.Title,
            Description = s.Description,
            Url = s.Url,
            Category = s.Category,
            BackgroundImage = s.BackgroundImage,
            Collections = s.Collections.Select(c => new CollectionReferenceDto
            {
                Title = c.Title,
                Collection = c.Collection,
                Url = c.Url,
                Description = c.Description,
                IsCustom = c.IsCustom
            }).ToList()
        });

        return TypedResults.Ok(sectionDtos);
    }

    /// <summary>
    /// GET /api/sections/{sectionName} - Get section by name
    /// </summary>
    private static async Task<Results<Ok<SectionDto>, NotFound>> GetSectionById(
        string sectionName,
        ISectionRepository sectionRepository,
        CancellationToken cancellationToken)
    {
        var section = await sectionRepository.GetByIdAsync(sectionName, cancellationToken);
        
        if (section == null)
        {
            return TypedResults.NotFound();
        }

        var sectionDto = new SectionDto
        {
            Id = section.Id,
            Title = section.Title,
            Description = section.Description,
            Url = section.Url,
            Category = section.Category,
            BackgroundImage = section.BackgroundImage,
            Collections = section.Collections.Select(c => new CollectionReferenceDto
            {
                Title = c.Title,
                Collection = c.Collection,
                Url = c.Url,
                Description = c.Description,
                IsCustom = c.IsCustom
            }).ToList()
        };

        return TypedResults.Ok(sectionDto);
    }

    /// <summary>
    /// GET /api/sections/{sectionName}/items - Get all items in a section
    /// </summary>
    private static async Task<Results<Ok<IEnumerable<ContentItemDto>>, NotFound>> GetSectionItems(
        string sectionName,
        ISectionRepository sectionRepository,
        IContentRepository contentRepository,
        CancellationToken cancellationToken)
    {
        // Verify section exists
        var section = await sectionRepository.GetByIdAsync(sectionName, cancellationToken);
        if (section == null)
        {
            return TypedResults.NotFound();
        }

        // Get all content with this section's category
        var content = await contentRepository.GetByCategoryAsync(section.Category, cancellationToken);
        var contentDtos = content.Select(MapContentToDto);
        
        return TypedResults.Ok(contentDtos);
    }

    /// <summary>
    /// GET /api/sections/{sectionName}/collections - Get all collections in a section
    /// </summary>
    private static async Task<Results<Ok<IEnumerable<CollectionReferenceDto>>, NotFound>> GetSectionCollections(
        string sectionName,
        ISectionRepository sectionRepository,
        CancellationToken cancellationToken)
    {
        var section = await sectionRepository.GetByIdAsync(sectionName, cancellationToken);
        
        if (section == null)
        {
            return TypedResults.NotFound();
        }

        var collectionDtos = section.Collections.Select(c => new CollectionReferenceDto
        {
            Title = c.Title,
            Collection = c.Collection,
            Url = c.Url,
            Description = c.Description,
            IsCustom = c.IsCustom
        });

        return TypedResults.Ok(collectionDtos);
    }

    /// <summary>
    /// GET /api/sections/{sectionName}/collections/{collectionName} - Get collection details
    /// </summary>
    private static async Task<Results<Ok<CollectionReferenceDto>, NotFound>> GetSectionCollection(
        string sectionName,
        string collectionName,
        ISectionRepository sectionRepository,
        CancellationToken cancellationToken)
    {
        var section = await sectionRepository.GetByIdAsync(sectionName, cancellationToken);
        
        if (section == null)
        {
            return TypedResults.NotFound();
        }

        var collection = section.Collections.FirstOrDefault(c => 
            c.Collection.Equals(collectionName, StringComparison.OrdinalIgnoreCase));
        
        if (collection == null)
        {
            return TypedResults.NotFound();
        }

        var collectionDto = new CollectionReferenceDto
        {
            Title = collection.Title,
            Collection = collection.Collection,
            Url = collection.Url,
            Description = collection.Description,
            IsCustom = collection.IsCustom
        };

        return TypedResults.Ok(collectionDto);
    }

    /// <summary>
    /// GET /api/sections/{sectionName}/collections/{collectionName}/items - Get items in a collection within a section
    /// </summary>
    private static async Task<Results<Ok<IEnumerable<ContentItemDto>>, NotFound>> GetSectionCollectionItems(
        string sectionName,
        string collectionName,
        ISectionRepository sectionRepository,
        IContentRepository contentRepository,
        CancellationToken cancellationToken)
    {
        // Verify section exists
        var section = await sectionRepository.GetByIdAsync(sectionName, cancellationToken);
        if (section == null)
        {
            return TypedResults.NotFound();
        }

        // Verify collection exists in this section
        var hasCollection = section.Collections.Any(c => 
            c.Collection.Equals(collectionName, StringComparison.OrdinalIgnoreCase));
        
        if (!hasCollection)
        {
            return TypedResults.NotFound();
        }

        // Get content filtered by both category and collection
        var allContent = await contentRepository.GetByCollectionAsync(collectionName, cancellationToken);
        var sectionContent = allContent
            .Where(c => c.Categories.Contains(section.Category, StringComparer.OrdinalIgnoreCase))
            .Select(MapContentToDto);
        
        return TypedResults.Ok(sectionContent);
    }

    /// <summary>
    /// Map ContentItem entity to ContentItemDto
    /// </summary>
    private static ContentItemDto MapContentToDto(Core.Models.ContentItem item)
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
            Url = $"/{item.Collection}/{item.Id}"
        };
    }
}
