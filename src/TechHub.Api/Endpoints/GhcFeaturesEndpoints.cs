using TechHub.Core.Interfaces;

namespace TechHub.Api.Endpoints;

/// <summary>
/// Public endpoints for GitHub Copilot feature data (ghc_features table).
/// </summary>
public static class GhcFeaturesEndpoints
{
    public static void MapGhcFeaturesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/ghc-features")
            .WithTags("GhcFeatures")
            .RequireRateLimiting("api-public");

        group.MapGet("/", GetAllFeaturesAsync)
            .WithName("GetGhcFeatures")
            .WithSummary("Get all GitHub Copilot features with their content links");

        group.MapPut("/{slug}", UpsertFeatureAsync)
            .WithName("UpsertGhcFeature")
            .WithSummary("Create or update a GitHub Copilot feature")
            .RequireAuthorization("AdminOnly");

        group.MapDelete("/{slug}", DeleteFeatureAsync)
            .WithName("DeleteGhcFeature")
            .WithSummary("Delete a GitHub Copilot feature")
            .RequireAuthorization("AdminOnly");

        group.MapPost("/{slug}/content-links", AddContentLinkAsync)
            .WithName("AddGhcFeatureContentLink")
            .WithSummary("Add a content link to a GitHub Copilot feature")
            .RequireAuthorization("AdminOnly");

        group.MapDelete("/{slug}/content-links", RemoveContentLinkAsync)
            .WithName("RemoveGhcFeatureContentLink")
            .WithSummary("Remove a content link from a GitHub Copilot feature")
            .RequireAuthorization("AdminOnly");

        group.MapPut("/{slug}/thumbnail", SetThumbnailAsync)
            .WithName("SetGhcFeatureThumbnail")
            .WithSummary("Set the thumbnail content link for a GitHub Copilot feature")
            .RequireAuthorization("AdminOnly");

        // ── VS Code Updates (public) ──────────────────────────────────────────
        app.MapGet("/api/vscode-updates", GetVscodeUpdatesPublicAsync)
            .WithTags("VscodeUpdates")
            .WithName("GetVscodeUpdatesPublic")
            .WithSummary("Get VS Code update items for the VS Code Updates page")
            .RequireRateLimiting("api-public");
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

    private static async Task<IResult> UpsertFeatureAsync(
        string slug,
        TechHub.Core.Models.GhcFeature feature,
        IGhcFeatureRepository repo,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(feature);
        if (!string.Equals(slug, feature.Slug, StringComparison.Ordinal))
        {
            return Results.BadRequest("The slug in the URL must match the slug in the request body.");
        }

        if (string.IsNullOrWhiteSpace(feature.Title))
        {
            return Results.BadRequest("Title is required.");
        }

        if (feature.Plans == null)
        {
            return Results.BadRequest("Plans must not be null. Provide an empty array [] if no plans apply.");
        }

        await repo.UpsertFeatureAsync(feature, ct);
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteFeatureAsync(
        string slug,
        IGhcFeatureRepository repo,
        CancellationToken ct)
    {
        var deleted = await repo.DeleteFeatureAsync(slug, ct);
        return deleted ? Results.NoContent() : Results.NotFound();
    }

    private static async Task<IResult> AddContentLinkAsync(
        string slug,
        ContentLinkRequest request,
        IGhcFeatureRepository repo,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.CollectionName) || string.IsNullOrWhiteSpace(request.ItemSlug))
        {
            return Results.BadRequest("CollectionName and ItemSlug are required.");
        }

        var added = await repo.AddContentLinkAsync(
            slug,
            request.CollectionName.Trim(),
            request.ItemSlug.Trim(),
            request.IsThumbnail,
            request.SortOrder,
            ct);

        return added ? Results.NoContent() : Results.NotFound();
    }

    private static async Task<IResult> RemoveContentLinkAsync(
        string slug,
        IGhcFeatureRepository repo,
        CancellationToken ct,
        string? collection = null,
        string? itemSlug = null)
    {
        if (string.IsNullOrWhiteSpace(collection) || string.IsNullOrWhiteSpace(itemSlug))
        {
            return Results.BadRequest("The 'collection' and 'itemSlug' query parameters are required.");
        }

        var removed = await repo.RemoveContentLinkAsync(slug, collection.Trim(), itemSlug.Trim(), ct);
        return removed ? Results.NoContent() : Results.NotFound();
    }

    private static async Task<IResult> SetThumbnailAsync(
        string slug,
        IGhcFeatureRepository repo,
        CancellationToken ct,
        string? collection = null,
        string? itemSlug = null)
    {
        if (string.IsNullOrWhiteSpace(collection) || string.IsNullOrWhiteSpace(itemSlug))
        {
            return Results.BadRequest("The 'collection' and 'itemSlug' query parameters are required.");
        }

        var updated = await repo.SetThumbnailAsync(slug, collection.Trim(), itemSlug.Trim(), ct);
        return updated ? Results.NoContent() : Results.NotFound();
    }

    /// <summary>Request body for adding a content link to a GHC feature.</summary>
#pragma warning disable CA1812 // Instantiated by ASP.NET Core JSON binding
    private sealed class ContentLinkRequest
    {
        public required string CollectionName { get; init; }
        public required string ItemSlug { get; init; }
        public bool IsThumbnail { get; init; }
        public int SortOrder { get; init; }
    }
#pragma warning restore CA1812
}

