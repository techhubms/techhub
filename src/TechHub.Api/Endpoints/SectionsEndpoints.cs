using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.DTOs;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;

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

        group.MapGet("/{sectionName}", GetSectionByName)
            .WithName("GetSectionByName")
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
        IOptions<AppSettings> appSettings,
        CancellationToken cancellationToken)
    {
        var sections = await sectionRepository.GetAllAsync(cancellationToken);
        var displayNames = appSettings.Value.Content.CollectionDisplayNames;

        var sectionDtos = sections.Select(s => new SectionDto
        {
            Name = s.Name,
            Title = s.Title,
            Description = s.Description,
            Url = s.Url,
            BackgroundImage = s.BackgroundImage,
            Collections = [.. s.Collections.Select(c => MapCollectionToDto(c, displayNames))]
        });

        return TypedResults.Ok(sectionDtos);
    }

    /// <summary>
    /// GET /api/sections/{sectionName} - Get section by name
    /// </summary>
    private static async Task<Results<Ok<SectionDto>, NotFound>> GetSectionByName(
        string sectionName,
        ISectionRepository sectionRepository,
        IOptions<AppSettings> appSettings,
        CancellationToken cancellationToken)
    {
        var section = await sectionRepository.GetByNameAsync(sectionName, cancellationToken);

        if (section == null)
        {
            return TypedResults.NotFound();
        }

        var displayNames = appSettings.Value.Content.CollectionDisplayNames;
        var sectionDto = new SectionDto
        {
            Name = section.Name,
            Title = section.Title,
            Description = section.Description,
            Url = section.Url,
            BackgroundImage = section.BackgroundImage,
            Collections = [.. section.Collections.Select(c => MapCollectionToDto(c, displayNames))]
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
        var section = await sectionRepository.GetByNameAsync(sectionName, cancellationToken);
        if (section == null)
        {
            return TypedResults.NotFound();
        }

        // Get all content for this section (filter by section.Name which matches ContentItem.SectionNames)
        var content = await contentRepository.GetBySectionAsync(section.Name, cancellationToken);
        var contentDtos = content.Select(MapContentToDto);

        return TypedResults.Ok(contentDtos);
    }

    /// <summary>
    /// GET /api/sections/{sectionName}/collections - Get all collections in a section
    /// </summary>
    private static async Task<Results<Ok<IEnumerable<CollectionReferenceDto>>, NotFound>> GetSectionCollections(
        string sectionName,
        ISectionRepository sectionRepository,
        IOptions<AppSettings> appSettings,
        CancellationToken cancellationToken)
    {
        var section = await sectionRepository.GetByNameAsync(sectionName, cancellationToken);

        if (section == null)
        {
            return TypedResults.NotFound();
        }

        var displayNames = appSettings.Value.Content.CollectionDisplayNames;
        var collectionDtos = section.Collections.Select(c => MapCollectionToDto(c, displayNames));

        return TypedResults.Ok(collectionDtos);
    }

    /// <summary>
    /// GET /api/sections/{sectionName}/collections/{collectionName} - Get collection details
    /// </summary>
    private static async Task<Results<Ok<CollectionReferenceDto>, NotFound>> GetSectionCollection(
        string sectionName,
        string collectionName,
        ISectionRepository sectionRepository,
        IOptions<AppSettings> appSettings,
        CancellationToken cancellationToken)
    {
        var section = await sectionRepository.GetByNameAsync(sectionName, cancellationToken);

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

        var displayNames = appSettings.Value.Content.CollectionDisplayNames;
        var collectionDto = MapCollectionToDto(collection, displayNames);

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
        var section = await sectionRepository.GetByNameAsync(sectionName, cancellationToken);
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

        // Get content filtered by both section and collection
        var allContent = await contentRepository.GetByCollectionAsync(collectionName, cancellationToken);
        var sectionContent = allContent
            .Where(c => c.SectionNames.Contains(section.Name, StringComparer.OrdinalIgnoreCase))
            .Select(MapContentToDto);

        return TypedResults.Ok(sectionContent);
    }

    /// <summary>
    /// Map ContentItem entity to ContentItemDto
    /// </summary>
    private static ContentItemDto MapContentToDto(Core.Models.ContentItem item)
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
            SectionNames = item.SectionNames,
            PrimarySection = TechHub.Core.Helpers.SectionPriorityHelper.GetPrimarySectionName(item.SectionNames, item.CollectionName),
            Tags = item.Tags,
            Excerpt = item.Excerpt,
            ExternalUrl = item.ExternalUrl,
            ViewingMode = item.ViewingMode,
            Url = $"/{primarySectionUrl.ToLowerInvariant()}/{item.CollectionName.ToLowerInvariant()}/{item.Slug.ToLowerInvariant()}"
        };
    }

    /// <summary>
    /// Helper method to map CollectionReference to DTO with display name from configuration
    /// </summary>
    private static CollectionReferenceDto MapCollectionToDto(CollectionReference collection, Dictionary<string, string> displayNames)
    {
        // Look up display name from configuration, fallback to Title if not found
        var displayName = displayNames.TryGetValue(collection.Name.ToLowerInvariant(), out var name)
            ? name
            : collection.Title;

        return new CollectionReferenceDto
        {
            Name = collection.Name,
            Title = collection.Title,
            Url = collection.Url,
            Description = collection.Description,
            DisplayName = displayName,
            IsCustom = collection.IsCustom
        };
    }
}
