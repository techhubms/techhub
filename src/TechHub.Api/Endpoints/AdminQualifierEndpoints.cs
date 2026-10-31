using TechHub.Core.Interfaces;
using TechHub.Core.Logging;
using TechHub.Core.Models.Admin;

namespace TechHub.Api.Endpoints;

/// <summary>
/// Admin API endpoints for the vocabulary of plans and surfaces: the qualifiers, their aliases and the names the AI
/// returned that are not in the vocabulary yet. Mapped on the <c>/api/admin</c> group, so the AdminOnly policy applies.
/// </summary>
internal static class AdminQualifierEndpoints
{
    public static void MapAdminQualifierEndpoints(this RouteGroupBuilder group)
    {
        ArgumentNullException.ThrowIfNull(group);

        group.MapGet("/qualifiers", GetQualifiersAsync)
            .WithName("GetQualifiers")
            .WithSummary("Get the plans and surfaces with their aliases and usage counts")
            .Produces<IReadOnlyList<ProductQualifier>>();

        group.MapGet("/qualifiers/unmatched", GetUnmatchedAsync)
            .WithName("GetUnmatchedQualifierNames")
            .WithSummary("Get the plan and surface names the AI returned that are not in the vocabulary yet")
            .Produces<IReadOnlyList<UnmatchedQualifierName>>();

        group.MapPost("/qualifiers", CreateQualifierAsync)
            .WithName("CreateQualifier")
            .WithSummary("Add a plan or surface")
            .Produces<ProductQualifier>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict);

        group.MapPut("/qualifiers/{id:long}", RenameQualifierAsync)
            .WithName("RenameQualifier")
            .WithSummary("Rename a plan or surface; the old name becomes an alias")
            .Produces<ProductQualifier>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict);

        group.MapDelete("/qualifiers/{id:long}", DeleteQualifierAsync)
            .WithName("DeleteQualifier")
            .WithSummary("Delete a plan or surface and its aliases")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/qualifiers/{id:long}/aliases", AddAliasAsync)
            .WithName("AddQualifierAlias")
            .WithSummary("Add an alias to a plan or surface")
            .Produces<ProductQualifier>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict);

        group.MapDelete("/qualifiers/{id:long}/aliases/{aliasId:long}", RemoveAliasAsync)
            .WithName("RemoveQualifierAlias")
            .WithSummary("Remove an alias from a plan or surface")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetQualifiersAsync(IProductQualifierRepository repository, CancellationToken ct) =>
        Results.Ok(await repository.GetQualifiersAsync(ct));

    private static async Task<IResult> GetUnmatchedAsync(IProductQualifierRepository repository, CancellationToken ct) =>
        Results.Ok(await repository.GetUnmatchedNamesAsync(ct));

    private static async Task<IResult> CreateQualifierAsync(
        QualifierCreateRequest request,
        IProductQualifierRepository repository,
        CancellationToken ct)
    {
        var result = await repository.CreateAsync(
            request.Kind?.Trim().Sanitize() ?? string.Empty,
            request.Name?.Trim().Sanitize() ?? string.Empty,
            ct);
        if (!result.Succeeded)
        {
            return ToErrorResult(result);
        }

        var created = await FindAsync(result.ProductId!.Value, repository, ct);
        return Results.Created($"/api/admin/qualifiers/{result.ProductId}", created);
    }

    private static async Task<IResult> RenameQualifierAsync(
        long id,
        ProductNameRequest request,
        IProductQualifierRepository repository,
        CancellationToken ct)
    {
        var result = await repository.RenameAsync(id, request.Name?.Trim().Sanitize() ?? string.Empty, ct);
        return await ToQualifierResultAsync(result, repository, ct);
    }

    private static async Task<IResult> DeleteQualifierAsync(long id, IProductQualifierRepository repository, CancellationToken ct)
    {
        var result = await repository.DeleteAsync(id, ct);
        return result.Succeeded ? Results.NoContent() : ToErrorResult(result);
    }

    private static async Task<IResult> AddAliasAsync(
        long id,
        ProductAliasRequest request,
        IProductQualifierRepository repository,
        CancellationToken ct)
    {
        var result = await repository.AddAliasAsync(id, request.Alias?.Trim().Sanitize() ?? string.Empty, ct);
        return await ToQualifierResultAsync(result, repository, ct);
    }

    private static async Task<IResult> RemoveAliasAsync(
        long id,
        long aliasId,
        IProductQualifierRepository repository,
        CancellationToken ct)
    {
        var result = await repository.RemoveAliasAsync(id, aliasId, ct);
        return result.Succeeded ? Results.NoContent() : ToErrorResult(result);
    }

    private static async Task<ProductQualifier?> FindAsync(long id, IProductQualifierRepository repository, CancellationToken ct) =>
        (await repository.GetQualifiersAsync(ct)).FirstOrDefault(q => q.Id == id);

    private static async Task<IResult> ToQualifierResultAsync(
        ProductOperationResult result,
        IProductQualifierRepository repository,
        CancellationToken ct)
    {
        if (!result.Succeeded)
        {
            return ToErrorResult(result);
        }

        var qualifier = await FindAsync(result.ProductId!.Value, repository, ct);
        return qualifier is null ? Results.NotFound() : Results.Ok(qualifier);
    }

    private static IResult ToErrorResult(ProductOperationResult result) => result.Status switch
    {
        ProductOperationStatus.NotFound => Results.NotFound(result.Message),
        ProductOperationStatus.Conflict => Results.Conflict(result.Message),
        _ => Results.BadRequest(result.Message)
    };
}
