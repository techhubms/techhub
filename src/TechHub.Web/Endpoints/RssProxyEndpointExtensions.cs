using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using TechHub.Core.Models;
using TechHub.Core.Validation;
using TechHub.Web.Services;

namespace TechHub.Web.Endpoints;

internal static class RssProxyEndpointExtensions
{
    public static void MapSectionRssFeedEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{sectionName}/roundups/feed.xml", async (
            string sectionName,
            SectionCache sectionCache,
            TechHubApiClient apiClient,
            CancellationToken ct) =>
        {
            if (!RouteParameterValidator.IsValidNameSegment(sectionName))
            {
                return Results.BadRequest("Invalid section name format.");
            }

            // Crawlers append /feed.xml to arbitrary article slugs. Answer 404 here instead of calling
            // the API (404 -> HttpRequestException -> 500), which would count as two failed requests.
            if (sectionCache.IsReady && sectionCache.GetSectionByName(sectionName) is null)
            {
                return Results.NotFound();
            }

            try
            {
                var xml = await apiClient.GetCollectionRssFeedAsync("roundups", sectionName, ct);
                return Results.Content(xml, "application/rss+xml; charset=utf-8");
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return Results.NotFound();
            }
        })
        .WithName("GetRoundupsRssFeed")
        .WithSummary("RSS feed for section roundups collection")
        .ExcludeFromDescription()
        .RequireRateLimiting("web-rss");

        app.MapGet("/{sectionName}/feed.xml", async (
            string sectionName,
            SectionCache sectionCache,
            TechHubApiClient apiClient,
            CancellationToken ct) =>
        {
            if (!RouteParameterValidator.IsValidNameSegment(sectionName))
            {
                return Results.BadRequest("Invalid section name format.");
            }

            if (sectionCache.IsReady && sectionCache.GetSectionByName(sectionName) is null)
            {
                return Results.NotFound();
            }

            try
            {
                var xml = await apiClient.GetSectionRssFeedAsync(sectionName, ct);
                return Results.Content(xml, "application/rss+xml; charset=utf-8");
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return Results.NotFound();
            }
        })
        .WithName("GetSectionRssFeed")
        .WithSummary("RSS feed for a section")
        .ExcludeFromDescription()
        .RequireRateLimiting("web-rss");
    }
}
