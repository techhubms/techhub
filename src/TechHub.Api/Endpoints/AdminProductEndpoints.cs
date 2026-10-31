using Microsoft.AspNetCore.Mvc;
using TechHub.Core.Interfaces;
using TechHub.Core.Logging;
using TechHub.Core.Models.Admin;

namespace TechHub.Api.Endpoints;

/// <summary>
/// Admin API endpoints for the product catalog: products and aliases, the review queue of product names the AI
/// returned that are not in the catalog yet, and the names that are not products.
/// Mapped on the <c>/api/admin</c> group, so the AdminOnly policy applies.
/// </summary>
internal static class AdminProductEndpoints
{
    private const int MaxSearchLength = 200;

    public static void MapAdminProductEndpoints(this RouteGroupBuilder group)
    {
        ArgumentNullException.ThrowIfNull(group);

        group.MapGet("/products", GetProductsAsync)
            .WithName("GetProducts")
            .WithSummary("Get the product catalog with aliases and the number of linked articles and dates")
            .Produces<IReadOnlyList<ProductSummary>>();

        group.MapGet("/products/{id:long}", GetProductAsync)
            .WithName("GetProduct")
            .WithSummary("Get a product with its aliases, linked articles and linked dates")
            .Produces<ProductDetail>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/products", CreateProductAsync)
            .WithName("CreateProduct")
            .WithSummary("Add a product to the catalog")
            .Produces<ProductDetail>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict);

        group.MapPut("/products/{id:long}", RenameProductAsync)
            .WithName("RenameProduct")
            .WithSummary("Rename a product; the old name becomes an alias")
            .Produces<ProductDetail>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict);

        group.MapDelete("/products/{id:long}", DeleteProductAsync)
            .WithName("DeleteProduct")
            .WithSummary("Delete a product; its articles and dates become unmatched")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/products/{id:long}/aliases", AddAliasAsync)
            .WithName("AddProductAlias")
            .WithSummary("Add an alias to a product")
            .Produces<ProductDetail>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict);

        group.MapDelete("/products/{id:long}/aliases/{aliasId:long}", RemoveAliasAsync)
            .WithName("RemoveProductAlias")
            .WithSummary("Remove an alias from a product")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/products/{id:long}/merge", MergeProductAsync)
            .WithName("MergeProduct")
            .WithSummary("Merge a product into another product")
            .Produces<ProductDetail>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/products/unmatched", GetUnmatchedAsync)
            .WithName("GetUnmatchedProductNames")
            .WithSummary("Get the product names the AI returned that are not in the catalog yet")
            .Produces<IReadOnlyList<UnmatchedProductName>>();

        group.MapGet("/products/ignored", GetIgnoredAsync)
            .WithName("GetIgnoredProductNames")
            .WithSummary("Get the names that are marked as not being a product")
            .Produces<IReadOnlyList<string>>();

        group.MapPost("/products/ignored", IgnoreNameAsync)
            .WithName("IgnoreProductName")
            .WithSummary("Mark a name as not being a product")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict);

        group.MapDelete("/products/ignored", UnignoreNameAsync)
            .WithName("UnignoreProductName")
            .WithSummary("Put an ignored name back in the review queue")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetProductsAsync(
        IProductRepository repository,
        CancellationToken ct,
        string? search = null)
    {
        search = search?.Trim().Sanitize();
        if (search is { Length: > MaxSearchLength })
        {
            return Results.BadRequest($"Search can be at most {MaxSearchLength} characters.");
        }

        return Results.Ok(await repository.GetProductsAsync(search, ct));
    }

    private static async Task<IResult> GetProductAsync(long id, IProductRepository repository, CancellationToken ct)
    {
        var product = await repository.GetDetailAsync(id, ct);
        return product is null ? Results.NotFound() : Results.Ok(product);
    }

    private static async Task<IResult> CreateProductAsync(
        ProductNameRequest request,
        IProductRepository repository,
        CancellationToken ct)
    {
        var result = await repository.CreateAsync(request.Name?.Trim().Sanitize() ?? string.Empty, ct);
        if (!result.Succeeded)
        {
            return ToErrorResult(result);
        }

        var created = await repository.GetDetailAsync(result.ProductId!.Value, ct);
        return Results.Created($"/api/admin/products/{result.ProductId}", created);
    }

    private static async Task<IResult> RenameProductAsync(
        long id,
        ProductNameRequest request,
        IProductRepository repository,
        CancellationToken ct)
    {
        var result = await repository.RenameAsync(id, request.Name?.Trim().Sanitize() ?? string.Empty, ct);
        return await ToDetailResultAsync(result, repository, ct);
    }

    private static async Task<IResult> DeleteProductAsync(long id, IProductRepository repository, CancellationToken ct)
    {
        var result = await repository.DeleteAsync(id, ct);
        return result.Succeeded ? Results.NoContent() : ToErrorResult(result);
    }

    private static async Task<IResult> AddAliasAsync(
        long id,
        ProductAliasRequest request,
        IProductRepository repository,
        CancellationToken ct)
    {
        var result = await repository.AddAliasAsync(id, request.Alias?.Trim().Sanitize() ?? string.Empty, ct);
        return await ToDetailResultAsync(result, repository, ct);
    }

    private static async Task<IResult> RemoveAliasAsync(
        long id,
        long aliasId,
        IProductRepository repository,
        CancellationToken ct)
    {
        var result = await repository.RemoveAliasAsync(id, aliasId, ct);
        return result.Succeeded ? Results.NoContent() : ToErrorResult(result);
    }

    private static async Task<IResult> MergeProductAsync(
        long id,
        ProductMergeRequest request,
        IProductRepository repository,
        CancellationToken ct)
    {
        var result = await repository.MergeAsync(id, request.TargetId, ct);
        return await ToDetailResultAsync(result, repository, ct);
    }

    private static async Task<IResult> GetUnmatchedAsync(IProductRepository repository, CancellationToken ct) =>
        Results.Ok(await repository.GetUnmatchedNamesAsync(ct));

    private static async Task<IResult> GetIgnoredAsync(IProductRepository repository, CancellationToken ct) =>
        Results.Ok(await repository.GetIgnoredNamesAsync(ct));

    private static async Task<IResult> IgnoreNameAsync(
        ProductNameRequest request,
        IProductRepository repository,
        CancellationToken ct)
    {
        var result = await repository.IgnoreNameAsync(request.Name?.Trim().Sanitize() ?? string.Empty, ct);
        return result.Succeeded ? Results.NoContent() : ToErrorResult(result);
    }

    private static async Task<IResult> UnignoreNameAsync(
        [FromQuery] string? name,
        IProductRepository repository,
        CancellationToken ct)
    {
        var result = await repository.UnignoreNameAsync(name?.Trim().Sanitize() ?? string.Empty, ct);
        return result.Succeeded ? Results.NoContent() : ToErrorResult(result);
    }

    private static async Task<IResult> ToDetailResultAsync(
        ProductOperationResult result,
        IProductRepository repository,
        CancellationToken ct)
    {
        if (!result.Succeeded)
        {
            return ToErrorResult(result);
        }

        var detail = await repository.GetDetailAsync(result.ProductId!.Value, ct);
        return detail is null ? Results.NotFound() : Results.Ok(detail);
    }

    private static IResult ToErrorResult(ProductOperationResult result) => result.Status switch
    {
        ProductOperationStatus.NotFound => Results.NotFound(result.Message),
        ProductOperationStatus.Conflict => Results.Conflict(result.Message),
        _ => Results.BadRequest(result.Message)
    };
}
