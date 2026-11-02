using TechHub.Core.Interfaces;

namespace TechHub.Api.Endpoints;

/// <summary>
/// Public endpoints for the features of GitHub Copilot shown on the features page (features of the GitHub Copilot product in
/// the catalog) and for the VS Code updates. The features are edited on the product page of the admin
/// (<c>/api/admin/products/{productId}/features</c>).
/// </summary>
public static class GhcFeaturesEndpoints
{
    public static void MapGhcFeaturesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/ghc-features")
            .WithTags("GhcFeatures");

        group.MapGet("/", GetAllFeaturesAsync)
            .WithName("GetGhcFeatures")
            .WithSummary("Get all GitHub Copilot features with their content links");

        // ── VS Code Updates (public) ─────────────────────────────────────────
        app.MapGet("/api/vscode-updates", GetVscodeUpdatesPublicAsync)
            .WithTags("VscodeUpdates")
            .WithName("GetVscodeUpdatesPublic")
            .WithSummary("Get VS Code update items for the VS Code Updates page");
    }

    private static async Task<IResult> GetAllFeaturesAsync(
        IGhcFeatureRepository repo,
        CancellationToken ct)
    {
        var features = await repo.GetAllFeaturesAsync(ct);
        return Results.Ok(features);
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
