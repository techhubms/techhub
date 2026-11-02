using TechHub.Core.Interfaces;
using TechHub.Core.Logging;
using TechHub.Core.Models.Admin;

namespace TechHub.Api.Endpoints;

/// <summary>
/// Admin API endpoints for the curated features of a product: the features with their aliases, and the feature names of
/// its dates that are not curated yet. Mapped on the <c>/api/admin</c> group, so the AdminOnly policy applies.
/// </summary>
internal static class AdminFeatureEndpoints
{
    public static void MapAdminFeatureEndpoints(this RouteGroupBuilder group)
    {
        ArgumentNullException.ThrowIfNull(group);

        group.MapGet("/products/{productId:long}/features", GetOverviewAsync)
            .WithName("GetProductFeatures")
            .WithSummary("Get the features of a product with aliases, and the feature names of its dates that are not curated yet")
            .Produces<ProductFeatureOverview>();

        group.MapPost("/products/{productId:long}/features", CreateFeatureAsync)
            .WithName("CreateProductFeature")
            .WithSummary("Add a feature to a product")
            .Produces<ProductFeatureOverview>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict);

        group.MapPut("/products/{productId:long}/features/{id:long}", RenameFeatureAsync)
            .WithName("RenameProductFeature")
            .WithSummary("Rename a feature; the old name becomes an alias")
            .Produces<ProductFeatureOverview>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict);

        group.MapDelete("/products/{productId:long}/features/{id:long}", DeleteFeatureAsync)
            .WithName("DeleteProductFeature")
            .WithSummary("Delete a feature and its aliases")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/products/{productId:long}/features/{id:long}/details", UpdateDetailsAsync)
            .WithName("UpdateProductFeatureDetails")
            .WithSummary("Set the description and the release date of a feature")
            .Produces<ProductFeatureOverview>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/products/{productId:long}/features/{id:long}/content", AddContentAsync)
            .WithName("AddProductFeatureContent")
            .WithSummary("Link a content item (video, post) to a feature, optionally as its thumbnail")
            .Produces<ProductFeatureOverview>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/products/{productId:long}/features/{id:long}/content", RemoveContentAsync)
            .WithName("RemoveProductFeatureContent")
            .WithSummary("Remove the link between a feature and a content item")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/products/{productId:long}/features/{id:long}/thumbnail", SetThumbnailAsync)
            .WithName("SetProductFeatureThumbnail")
            .WithSummary("Make a linked content item the thumbnail of a feature")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/products/{productId:long}/features/{id:long}/aliases", AddAliasAsync)
            .WithName("AddProductFeatureAlias")
            .WithSummary("Add an alias to a feature")
            .Produces<ProductFeatureOverview>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict);

        group.MapDelete("/products/{productId:long}/features/{id:long}/aliases/{aliasId:long}", RemoveAliasAsync)
            .WithName("RemoveProductFeatureAlias")
            .WithSummary("Remove an alias from a feature")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetOverviewAsync(long productId, IProductFeatureRepository repository, CancellationToken ct) =>
        Results.Ok(await repository.GetOverviewAsync(productId, ct));

    private static async Task<IResult> CreateFeatureAsync(
        long productId,
        ProductNameRequest request,
        IProductFeatureRepository repository,
        CancellationToken ct)
    {
        var result = await repository.CreateAsync(productId, request.Name?.Trim().Sanitize() ?? string.Empty, ct);
        if (!result.Succeeded)
        {
            return ToErrorResult(result);
        }

        return Results.Created($"/api/admin/products/{productId}/features/{result.ProductId}", await repository.GetOverviewAsync(productId, ct));
    }

    private static async Task<IResult> RenameFeatureAsync(
        long productId,
        long id,
        ProductNameRequest request,
        IProductFeatureRepository repository,
        CancellationToken ct)
    {
        var result = await repository.RenameAsync(productId, id, request.Name?.Trim().Sanitize() ?? string.Empty, ct);
        return await ToOverviewResultAsync(result, productId, repository, ct);
    }

    private static async Task<IResult> DeleteFeatureAsync(long productId, long id, IProductFeatureRepository repository, CancellationToken ct)
    {
        var result = await repository.DeleteAsync(productId, id, ct);
        return result.Succeeded ? Results.NoContent() : ToErrorResult(result);
    }

    private static async Task<IResult> UpdateDetailsAsync(
        long productId,
        long id,
        FeatureDetailsRequest request,
        IProductFeatureRepository repository,
        CancellationToken ct)
    {
        var result = await repository.UpdateDetailsAsync(productId, id, request.Description, request.ReleaseDate, ct);
        return await ToOverviewResultAsync(result, productId, repository, ct);
    }

    private static async Task<IResult> AddContentAsync(
        long productId,
        long id,
        FeatureContentRequest request,
        IProductFeatureRepository repository,
        CancellationToken ct)
    {
        var result = await repository.AddContentAsync(productId, id, request.CollectionName?.Trim() ?? string.Empty, request.ItemSlug?.Trim() ?? string.Empty, request.IsThumbnail, ct);
        return await ToOverviewResultAsync(result, productId, repository, ct);
    }

    private static async Task<IResult> RemoveContentAsync(
        long productId,
        long id,
        IProductFeatureRepository repository,
        CancellationToken ct,
        string? collection = null,
        string? itemSlug = null)
    {
        if (string.IsNullOrWhiteSpace(collection) || string.IsNullOrWhiteSpace(itemSlug))
        {
            return Results.BadRequest("The 'collection' and 'itemSlug' query parameters are required.");
        }

        var result = await repository.RemoveContentAsync(productId, id, collection.Trim(), itemSlug.Trim(), ct);
        return result.Succeeded ? Results.NoContent() : ToErrorResult(result);
    }

    private static async Task<IResult> SetThumbnailAsync(
        long productId,
        long id,
        IProductFeatureRepository repository,
        CancellationToken ct,
        string? collection = null,
        string? itemSlug = null)
    {
        if (string.IsNullOrWhiteSpace(collection) || string.IsNullOrWhiteSpace(itemSlug))
        {
            return Results.BadRequest("The 'collection' and 'itemSlug' query parameters are required.");
        }

        var result = await repository.SetThumbnailAsync(productId, id, collection.Trim(), itemSlug.Trim(), ct);
        return result.Succeeded ? Results.NoContent() : ToErrorResult(result);
    }

    private static async Task<IResult> AddAliasAsync(
        long productId,
        long id,
        ProductAliasRequest request,
        IProductFeatureRepository repository,
        CancellationToken ct)
    {
        var result = await repository.AddAliasAsync(productId, id, request.Alias?.Trim().Sanitize() ?? string.Empty, ct);
        return await ToOverviewResultAsync(result, productId, repository, ct);
    }

    private static async Task<IResult> RemoveAliasAsync(
        long productId,
        long id,
        long aliasId,
        IProductFeatureRepository repository,
        CancellationToken ct)
    {
        var result = await repository.RemoveAliasAsync(productId, id, aliasId, ct);
        return result.Succeeded ? Results.NoContent() : ToErrorResult(result);
    }

    private static async Task<IResult> ToOverviewResultAsync(
        ProductOperationResult result,
        long productId,
        IProductFeatureRepository repository,
        CancellationToken ct) =>
        result.Succeeded ? Results.Ok(await repository.GetOverviewAsync(productId, ct)) : ToErrorResult(result);

    private static IResult ToErrorResult(ProductOperationResult result) => result.Status switch
    {
        ProductOperationStatus.NotFound => Results.NotFound(result.Message),
        ProductOperationStatus.Conflict => Results.Conflict(result.Message),
        _ => Results.BadRequest(result.Message)
    };
}
