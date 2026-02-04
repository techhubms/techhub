using TechHub.Core.Interfaces;
using TechHub.Core.Models;

namespace TechHub.Api.Endpoints;

/// <summary>
/// API endpoints for RSS feeds
/// </summary>
public static class RssEndpoints
{
    /// <summary>
    /// Maps all RSS feed endpoints to the application
    /// </summary>
    public static IEndpointRouteBuilder MapRssEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/rss")
            .WithTags("RSS")
            .WithDescription("RSS 2.0 feed endpoints for content syndication");

        // Everything feed (all content)
        group.MapGet("/all", GetAllContentFeed)
            .WithName("GetAllContentRssFeed")
            .WithSummary("Get RSS feed for all content")
            .WithDescription("Returns RSS 2.0 feed containing all content from all sections")
            .Produces(StatusCodes.Status200OK, contentType: "application/rss+xml");

        // Section-specific feed
        group.MapGet("/{sectionName}", GetSectionFeed)
            .WithName("GetSectionRssFeed")
            .WithSummary("Get RSS feed for a section")
            .WithDescription("Returns RSS 2.0 feed for content in the specified section")
            .Produces(StatusCodes.Status200OK, contentType: "application/rss+xml")
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }

    /// <summary>
    /// Gets RSS feed containing all content from all sections
    /// </summary>
    private static async Task<IResult> GetAllContentFeed(
        IContentRepository contentRepository,
        IRssService rssService)
    {
        // Get only the 50 most recent items for RSS feed (standard RSS practice)
        var searchResult = await contentRepository.SearchAsync(new SearchRequest(
            take: 50,
            sections: ["all"],
            collections: ["all"],
            tags: []
        ));

        // Create a virtual "Everything" section for the feed with a dummy collection
        var dummyCollection = new Core.Models.Collection(
            name: "all",
            title: "All Content",
            url: "/all",
            description: "All content from Tech Hub",
            displayName: "All Content");

        var everythingSection = new Core.Models.Section(
            name: "all",
            title: "Everything",
            description: "All content from Tech Hub",
            url: "/",
            collections: [dummyCollection]);

        var channel = await rssService.GenerateSectionFeedAsync(everythingSection, searchResult.Items);
        var xml = rssService.SerializeToXml(channel);

        return Results.Content(xml, "application/rss+xml; charset=utf-8");
    }

    /// <summary>
    /// Gets RSS feed for a specific section
    /// </summary>
    private static async Task<IResult> GetSectionFeed(
        string sectionName,
        IContentRepository contentRepository,
        IRssService rssService)
    {
        var section = await contentRepository.GetSectionByNameAsync(sectionName);
        if (section is null)
        {
            return Results.NotFound();
        }

        // Get content for this section using SearchAsync
        // RSS feeds should exclude draft content and show only 50 most recent items
        var searchResult = await contentRepository.SearchAsync(new SearchRequest(
            take: 50,
            sections: [section.Name],
            collections: ["all"],
            tags: []
        ));

        var channel = await rssService.GenerateSectionFeedAsync(section, searchResult.Items);
        var xml = rssService.SerializeToXml(channel);

        return Results.Content(xml, "application/rss+xml; charset=utf-8");
    }
}
