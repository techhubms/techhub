using Microsoft.AspNetCore.Http.HttpResults;
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
            .Produces<IEnumerable<Section>>(StatusCodes.Status200OK);

        group.MapGet("/{sectionName}", GetSectionByName)
            .WithName("GetSectionByName")
            .WithSummary("Get section by name")
            .WithDescription("Returns a single section with its collections and metadata")
            .Produces<Section>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        // Nested: items in a section
        group.MapGet("/{sectionName}/items", GetSectionItems)
            .WithName("GetSectionItems")
            .WithSummary("Get all items in a section")
            .WithDescription("Returns all content items from all collections in this section")
            .Produces<IEnumerable<ContentItem>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        // Nested: collections in a section
        group.MapGet("/{sectionName}/collections", GetSectionCollections)
            .WithName("GetSectionCollections")
            .WithSummary("Get all collections in a section")
            .WithDescription("Returns all collection references for this section")
            .Produces<IEnumerable<Collection>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        // Nested: specific collection in a section
        group.MapGet("/{sectionName}/collections/{collectionName}", GetSectionCollection)
            .WithName("GetSectionCollection")
            .WithSummary("Get collection details")
            .WithDescription("Returns details of a specific collection within this section")
            .Produces<Collection>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        // Nested: items in a specific collection within a section
        group.MapGet("/{sectionName}/collections/{collectionName}/items", GetSectionCollectionItems)
            .WithName("GetSectionCollectionItems")
            .WithSummary("Get items in a collection within a section")
            .WithDescription("Returns all content items from a specific collection in this section")
            .Produces<IEnumerable<ContentItem>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }

    /// <summary>
    /// GET /api/sections - Get all sections
    /// </summary>
    private static async Task<Ok<IEnumerable<Section>>> GetAllSections(
        ISectionRepository sectionRepository,
        CancellationToken cancellationToken)
    {
        var sections = await sectionRepository.GetAllAsync(cancellationToken);
        return TypedResults.Ok(sections.AsEnumerable());
    }

    /// <summary>
    /// GET /api/sections/{sectionName} - Get section by name
    /// </summary>
    private static async Task<Results<Ok<Section>, NotFound>> GetSectionByName(
        string sectionName,
        ISectionRepository sectionRepository,
        CancellationToken cancellationToken)
    {
        var section = await sectionRepository.GetByNameAsync(sectionName, cancellationToken);

        if (section == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(section);
    }

    /// <summary>
    /// GET /api/sections/{sectionName}/items - Get all items in a section
    /// </summary>
    private static async Task<Results<Ok<IEnumerable<ContentItem>>, NotFound>> GetSectionItems(
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
        // Exclude drafts from section content listings
        var content = await contentRepository.GetBySectionAsync(section.Name, includeDraft: false, limit: 1000, offset: 0, cancellationToken);

        return TypedResults.Ok(content.AsEnumerable());
    }

    /// <summary>
    /// GET /api/sections/{sectionName}/collections - Get all collections in a section
    /// </summary>
    private static async Task<Results<Ok<IEnumerable<Collection>>, NotFound>> GetSectionCollections(
        string sectionName,
        ISectionRepository sectionRepository,
        CancellationToken cancellationToken)
    {
        var section = await sectionRepository.GetByNameAsync(sectionName, cancellationToken);

        if (section == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(section.Collections.AsEnumerable());
    }

    /// <summary>
    /// GET /api/sections/{sectionName}/collections/{collectionName} - Get collection details
    /// </summary>
    private static async Task<Results<Ok<Collection>, NotFound>> GetSectionCollection(
        string sectionName,
        string collectionName,
        ISectionRepository sectionRepository,
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

        return TypedResults.Ok(collection);
    }

    /// <summary>
    /// GET /api/sections/{sectionName}/collections/{collectionName}/items - Get items in a collection within a section
    /// Supports optional subcollection query parameter (e.g., ?subcollection=ghc-features)
    /// </summary>
    private static async Task<Results<Ok<IEnumerable<ContentItem>>, NotFound>> GetSectionCollectionItems(
        string sectionName,
        string collectionName,
        string? subcollection,
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

        // Get content filtered by both section and collection, optionally by subcollection (exclude drafts)
        // Use SearchAsync with section filter for combined filtering
        var request = new SearchRequest
        {
            Sections = [section.Name],
            Collections = [collectionName],
            Take = 1000
        };
        var searchResults = await contentRepository.SearchAsync(request, cancellationToken);
        var sectionContent = searchResults.Items;
        
        // Further filter by subcollection if specified (SearchRequest doesn't support subcollection filtering yet)
        if (!string.IsNullOrWhiteSpace(subcollection))
        {
            sectionContent = sectionContent
                .Where(c => c.SubcollectionName?.Equals(subcollection, StringComparison.OrdinalIgnoreCase) ?? false)
                .ToList();
        }

        return TypedResults.Ok(sectionContent.AsEnumerable());
    }
}
