using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;

namespace TechHub.Api.Endpoints;

/// <summary>
/// API endpoints for browsing content by author.
/// </summary>
public static class AuthorEndpoints
{
    /// <summary>
    /// Maximum author name length accepted in route parameters.
    /// Prevents excessively long values from reaching the repository.
    /// </summary>
    private const int MaxAuthorNameLength = 200;

    /// <summary>
    /// Maps all author-related endpoints to the application.
    ///
    /// - /api/authors                      → List all authors with item counts
    /// - /api/authors/{authorName}/items   → List content items for a specific author
    /// </summary>
    public static IEndpointRouteBuilder MapAuthorEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/authors")
            .WithTags("Authors")
            .WithDescription("Endpoints for browsing content by author");

        group.MapGet("/", GetAllAuthors)
            .WithName("GetAllAuthors")
            .WithSummary("Get all authors")
            .WithDescription("Returns all authors with their published content item counts, sorted alphabetically")
            .Produces<IReadOnlyList<AuthorSummary>>(StatusCodes.Status200OK);

        group.MapGet("/{authorName}/items", GetAuthorItems)
            .WithName("GetAuthorItems")
            .WithSummary("Get content items for an author")
            .WithDescription("Returns paginated content items attributed to the specified author. " +
                "Supports: take (default configured in appsettings), skip.")
            .Produces<CollectionItemsResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }

    /// <summary>
    /// Validates an author name from a route parameter.
    /// Author names may contain any printable characters except path separators.
    /// </summary>
    private static bool IsValidAuthorName(string? value)
    {
        if (string.IsNullOrEmpty(value) || value.Length > MaxAuthorNameLength)
        {
            return false;
        }

        // Reject path traversal and null bytes
        return !value.Contains('/', StringComparison.Ordinal)
            && !value.Contains('\\', StringComparison.Ordinal)
            && !value.Contains('\0', StringComparison.Ordinal);
    }

    /// <summary>
    /// GET /api/authors - Get all authors with content item counts.
    /// </summary>
    private static async Task<Ok<IReadOnlyList<AuthorSummary>>> GetAllAuthors(
        IContentRepository contentRepository,
        CancellationToken cancellationToken)
    {
        var authors = await contentRepository.GetAuthorsAsync(cancellationToken);
        return TypedResults.Ok(authors);
    }

    /// <summary>
    /// GET /api/authors/{authorName}/items - Get content items for a specific author.
    /// </summary>
    private static async Task<Results<Ok<CollectionItemsResponse>, BadRequest<string>, NotFound>> GetAuthorItems(
        string authorName,
        IOptions<ApiOptions> apiOptions,
        IContentRepository contentRepository,
        [FromQuery] int? take = null,
        [FromQuery] int skip = 0,
        CancellationToken cancellationToken = default)
    {
        // Validate author name
        if (!IsValidAuthorName(authorName))
        {
            return TypedResults.BadRequest("Invalid author name format.");
        }

        var decodedAuthorName = Uri.UnescapeDataString(authorName);

        // Enforce pagination limits
        var options = apiOptions.Value;
        var limit = Math.Clamp(take ?? options.DefaultPageSize, 1, options.MaxPageSize);
        var offset = Math.Max(skip, 0);

        // Check that the author exists; use the stored name for case-exact SQL matching
        var authors = await contentRepository.GetAuthorsAsync(cancellationToken);
        var matchedAuthor = authors.FirstOrDefault(a =>
            string.Equals(a.Name, decodedAuthorName, StringComparison.OrdinalIgnoreCase));
        if (matchedAuthor == null)
        {
            return TypedResults.NotFound();
        }

        // Use the stored author name (exact case) so the SQL filter matches precisely
        var request = new SearchRequest(
            take: limit,
            skip: offset,
            sections: ["all"],
            collections: ["all"],
            tags: [],
            author: matchedAuthor.Name);

        var content = await contentRepository.SearchAsync(request, cancellationToken);
        return TypedResults.Ok(new CollectionItemsResponse(content.Items, content.TotalCount));
    }
}
