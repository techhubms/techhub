using Microsoft.AspNetCore.Http.HttpResults;
using TechHub.Core.Interfaces;
using TechHub.Core.Logging;
using TechHub.Core.Models.FeatureToggles;
using TechHub.Core.Validation;

namespace TechHub.Api.Endpoints;

/// <summary>DTO for changing who can use a feature.</summary>
public sealed class FeatureToggleUpdateRequest
{
    public FeatureToggleMode? Mode { get; init; }
}

/// <summary>
/// The feature toggles. Reading them is public: the Web app reads them to decide what it shows to a visitor.
/// Changing them is admin-only.
/// </summary>
public static class FeatureToggleEndpoints
{
    public static IEndpointRouteBuilder MapFeatureToggleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/feature-toggles", GetFeatureToggles)
            .WithTags("FeatureToggles")
            .WithName("GetFeatureToggles")
            .WithSummary("Get the feature toggles")
            .WithDescription("Returns for every feature who can use it: nobody (disabled), admins or everyone. The Web app decides per visitor.")
            .Produces<IReadOnlyList<FeatureToggle>>(StatusCodes.Status200OK);

        return endpoints;
    }

    /// <summary>Maps the endpoints that change a feature toggle on the <c>/api/admin</c> group, so the AdminOnly policy applies.</summary>
    internal static void MapAdminFeatureToggleEndpoints(this RouteGroupBuilder group)
    {
        ArgumentNullException.ThrowIfNull(group);

        group.MapPut("/feature-toggles/{featureKey}", UpdateFeatureToggle)
            .WithName("UpdateFeatureToggle")
            .WithSummary("Set who can use a feature: disabled, admins or everyone")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static async Task<Ok<IReadOnlyList<FeatureToggle>>> GetFeatureToggles(
        IFeatureToggleRepository repository,
        CancellationToken cancellationToken)
    {
        return TypedResults.Ok(await repository.GetAllAsync(cancellationToken));
    }

    private static async Task<Results<NoContent, NotFound, BadRequest<string>>> UpdateFeatureToggle(
        string featureKey,
        FeatureToggleUpdateRequest request,
        IFeatureToggleRepository repository,
        CancellationToken cancellationToken)
    {
        featureKey = featureKey.Trim().Sanitize();

        if (!RouteParameterValidator.IsValidNameSegment(featureKey))
        {
            return TypedResults.BadRequest("Invalid feature key format.");
        }

        if (request.Mode is not { } mode || !Enum.IsDefined(mode))
        {
            return TypedResults.BadRequest("Mode must be one of: Disabled, Admins, Everyone.");
        }

        var updated = await repository.SetModeAsync(featureKey, mode, cancellationToken);
        return updated ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}
