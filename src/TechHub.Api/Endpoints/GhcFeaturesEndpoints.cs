using TechHub.Core.Interfaces;

namespace TechHub.Api.Endpoints;

/// <summary>
/// Public endpoint for the VS Code updates page. (The features of GitHub Copilot are part of the catalog and shown on the
/// timeline of the section; they are edited on the product page of the admin, <c>/api/admin/products/{productId}/features</c>.)
/// </summary>
public static class GhcFeaturesEndpoints
{
    public static void MapGhcFeaturesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/vscode-updates", GetVscodeUpdatesPublicAsync)
            .WithTags("VscodeUpdates")
            .WithName("GetVscodeUpdatesPublic")
            .WithSummary("Get VS Code update items for the VS Code Updates page");
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
