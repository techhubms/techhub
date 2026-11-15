using Microsoft.AspNetCore.Http.HttpResults;
using TechHub.Core.Interfaces;
using TechHub.Core.Logging;
using TechHub.Core.Models;
using TechHub.Core.Validation;

namespace TechHub.Api.Endpoints;

/// <summary>
/// Public endpoints of the GitHub Copilot pages: the features of the catalog that have content (the features page) and the
/// VS Code updates page. (Features are edited on the product page of the admin, <c>/api/admin/products/{productId}/features</c>.)
/// </summary>
public static class GhcFeaturesEndpoints
{
    public static void MapGhcFeaturesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/sections/{sectionName}/features", GetSectionFeaturesAsync)
            .WithTags("Features")
            .WithName("GetSectionFeatures")
            .WithSummary("Get the features of a section that have content")
            .WithDescription("Returns the features of the catalog product that has the tag of the section as its name, and of its child products, " +
                "that have content: a video or article that an admin linked, that a dated event names, or that lists the feature. " +
                "Features without content are left out. Newest release first, features without a release date last. " +
                "The list is empty when the tag of the section is no product.")
            .Produces<IReadOnlyList<SectionFeature>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        app.MapGet("/api/vscode-updates", GetVscodeUpdatesPublicAsync)
            .WithTags("VscodeUpdates")
            .WithName("GetVscodeUpdatesPublic")
            .WithSummary("Get VS Code update items for the VS Code Updates page");
    }

    private static async Task<Results<Ok<IReadOnlyList<SectionFeature>>, NotFound, BadRequest<string>>> GetSectionFeaturesAsync(
        string sectionName,
        IContentRepository contentRepository,
        IGhcFeatureRepository repo,
        CancellationToken ct)
    {
        sectionName = sectionName.Sanitize();

        if (!RouteParameterValidator.IsValidNameSegment(sectionName))
        {
            return TypedResults.BadRequest("Invalid section name format.");
        }

        var section = await contentRepository.GetSectionByNameAsync(sectionName, ct);
        if (section is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(await repo.GetFeaturesWithContentAsync(section.Tag, ct));
    }

    private static async Task<IResult> GetVscodeUpdatesPublicAsync(
        IGhcFeatureRepository repo,
        CancellationToken ct)
    {
        // Hard-coded upper bound of 100 items. This endpoint has no pagination parameters
        // and is designed for the VS Code Updates page which shows a curated set of items.
        // If this ever needs to scale beyond 100, add a limit/offset query parameter.
        var (items, _) = await repo.GetVscodeUpdateItemsAsync(offset: 0, pageSize: 100, search: null, ct);
        return Results.Ok(items);
    }
}
