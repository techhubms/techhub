using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TechHub.Core.DTOs;
using TechHub.Core.Interfaces;
using TechHub.Infrastructure.Services;

namespace TechHub.Api.Endpoints;

/// <summary>
/// Endpoints for tag cloud and tag filtering operations
/// </summary>
public static class TagEndpoints
{
    public static void MapTagEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tags").WithTags("Tags");

        group.MapGet("/cloud", GetTagCloud)
            .WithName("GetTagCloud")
            .WithSummary("Get tag cloud for specified scope")
            .WithDescription("Returns top tags with usage counts and size categories for tag cloud rendering")
            .Produces<IReadOnlyList<TagCloudItem>>()
            .Produces<ProblemDetails>(400);

        group.MapGet("/all", GetAllTags)
            .WithName("GetAllTags")
            .WithSummary("Get all tags for specified scope")
            .WithDescription("Returns all tags with usage counts for dropdown filter")
            .Produces<AllTagsResponse>();
    }

    private static async Task<IResult> GetTagCloud(
        [FromQuery(Name = "scope")] string scope,
        [FromQuery(Name = "section")] string? section,
        [FromQuery(Name = "collection")] string? collection,
        [FromQuery(Name = "contentId")] string? contentId,
        [FromQuery(Name = "maxTags")] int? maxTags,
        [FromQuery(Name = "minUses")] int? minUses,
        [FromQuery(Name = "lastDays")] int? lastDays,
        ITagCloudService tagCloudService,
        IOptions<FilteringOptions> filteringOptions,
        CancellationToken cancellationToken)
    {
        // Parse scope
        if (!Enum.TryParse<TagCloudScope>(scope, ignoreCase: true, out var tagCloudScope))
        {
            return Results.BadRequest(new ProblemDetails
            {
                Title = "Invalid scope",
                Detail = $"The scope '{scope}' is not valid. Valid values: Homepage, Section, Collection, Content",
                Status = 400
            });
        }

        // Validate required parameters for scope
        if (tagCloudScope == TagCloudScope.Section && string.IsNullOrWhiteSpace(section))
        {
            return Results.BadRequest(new ProblemDetails
            {
                Title = "Section required",
                Detail = "Section name is required for Section scope",
                Status = 400
            });
        }

        if (tagCloudScope == TagCloudScope.Collection &&
            (string.IsNullOrWhiteSpace(section) || string.IsNullOrWhiteSpace(collection)))
        {
            return Results.BadRequest(new ProblemDetails
            {
                Title = "Section and collection required",
                Detail = "Both section and collection names are required for Collection scope",
                Status = 400
            });
        }

        if (tagCloudScope == TagCloudScope.Content && string.IsNullOrWhiteSpace(contentId))
        {
            return Results.BadRequest(new ProblemDetails
            {
                Title = "Content ID required",
                Detail = "Content ID is required for Content scope",
                Status = 400
            });
        }

        // Get defaults from configuration
        var config = filteringOptions.Value.TagCloud;

        // Create request
        var request = new TagCloudRequest
        {
            Scope = tagCloudScope,
            SectionName = section,
            CollectionName = collection,
            ContentItemId = contentId,
            MaxTags = maxTags ?? config.DefaultMaxTags,
            MinUses = minUses ?? config.MinimumTagUses,
            LastDays = lastDays ?? config.DefaultDateRangeDays
        };

        // Get tag cloud
        var tagCloud = await tagCloudService.GetTagCloudAsync(request, cancellationToken);

        return Results.Ok(tagCloud);
    }

    private static async Task<IResult> GetAllTags(
        [FromQuery(Name = "section")] string? section,
        [FromQuery(Name = "collection")] string? collection,
        ITagCloudService tagCloudService,
        CancellationToken cancellationToken)
    {
        var response = await tagCloudService.GetAllTagsAsync(section, collection, cancellationToken);
        return Results.Ok(response);
    }
}
