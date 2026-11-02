using System.Globalization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TechHub.Core.Interfaces;
using TechHub.Core.Logging;
using TechHub.Core.Models;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Core.Validation;

namespace TechHub.Api.Endpoints;

/// <summary>
/// The dated lifecycle events (preview, GA, deprecation, ...) that the articles of a section state.
/// </summary>
public static class TimelineEndpoints
{
    private const int DefaultTake = 200;
    private const int MaxTake = 500;

    public static IEndpointRouteBuilder MapTimelineEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/sections/{sectionName}/timeline", GetSectionTimeline)
            .WithTags("Timeline")
            .WithName("GetSectionTimeline")
            .WithSummary("Get the timeline of a section")
            .WithDescription("Returns the dated events of the articles of a section, newest first, with the same event stated by several articles merged. " +
                "Optional filters: from and to (yyyy-MM-dd), kinds (comma separated event kinds) and take (1-500, default 200). Roundups are not used.")
            .Produces<IReadOnlyList<TimelineEntry>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }

    private static async Task<Results<Ok<IReadOnlyList<TimelineEntry>>, NotFound, BadRequest<string>>> GetSectionTimeline(
        string sectionName,
        [FromQuery] string? from,
        [FromQuery] string? to,
        [FromQuery] string? kinds,
        [FromQuery] int? take,
        IContentRepository contentRepository,
        ITimelineEventRepository timelineRepository,
        CancellationToken cancellationToken)
    {
        sectionName = sectionName.Sanitize();

        if (!RouteParameterValidator.IsValidNameSegment(sectionName))
        {
            return TypedResults.BadRequest("Invalid section name format.");
        }

        if (take is < 1 or > MaxTake)
        {
            return TypedResults.BadRequest($"take must be between 1 and {MaxTake}.");
        }

        if (!TryParseDate(from, out var fromDate))
        {
            return TypedResults.BadRequest("Invalid 'from' date format. Expected yyyy-MM-dd (e.g., 2026-01-15).");
        }

        if (!TryParseDate(to, out var toDate))
        {
            return TypedResults.BadRequest("Invalid 'to' date format. Expected yyyy-MM-dd (e.g., 2026-01-15).");
        }

        var kindList = (kinds ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct(StringComparer.Ordinal)
            .ToList();
        if (kindList.Any(k => !TimelineEvent.ValidKinds.Contains(k)))
        {
            return TypedResults.BadRequest($"kinds must be a comma separated list of: {string.Join(", ", TimelineEvent.ValidKinds.Order(StringComparer.Ordinal))}.");
        }

        if (fromDate > toDate)
        {
            (fromDate, toDate) = (toDate, fromDate);
        }

        if (await contentRepository.GetSectionByNameAsync(sectionName, cancellationToken) is null)
        {
            return TypedResults.NotFound();
        }

        var entries = await timelineRepository.GetTimelineAsync(new TimelineRequest(sectionName, fromDate, toDate, kindList), cancellationToken);

        // The repository returns newest first, so the cap keeps the newest entries.
        return TypedResults.Ok<IReadOnlyList<TimelineEntry>>(entries.Take(take ?? DefaultTake).ToList());
    }

    private static bool TryParseDate(string? value, out DateOnly? date)
    {
        date = null;
        if (string.IsNullOrWhiteSpace(value))
        {
            return true;
        }

        if (DateOnly.TryParseExact(value.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
        {
            date = parsed;
            return true;
        }

        return false;
    }
}
