using TechHub.Core.Interfaces;

namespace TechHub.Api.Endpoints;

/// <summary>
/// API endpoints for RSS feeds
/// </summary>
internal static class RssEndpoints
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

        // Collection-specific feed
        group.MapGet("/collection/{collectionName}", GetCollectionFeed)
            .WithName("GetCollectionRssFeed")
            .WithSummary("Get RSS feed for a collection")
            .WithDescription("Returns RSS 2.0 feed for content in the specified collection (e.g., roundups)")
            .Produces(StatusCodes.Status200OK, contentType: "application/rss+xml")
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }

    /// <summary>
    /// Gets RSS feed containing all content from all sections
    /// </summary>
    private static async Task<IResult> GetAllContentFeed(
        ISectionRepository sectionRepository,
        IContentRepository contentRepository,
        IRssService rssService)
    {
        // Get all content items (exclude drafts from RSS feeds)
        var allItems = await contentRepository.GetAllAsync(includeDraft: false);

        // Create a virtual "Everything" section for the feed
        var everythingSection = new Core.Models.Section
        {
            Name = "all",
            Title = "Everything",
            Url = "/",
            Description = "All content from Tech Hub",
            Collections = []
        };

        var channel = await rssService.GenerateSectionFeedAsync(everythingSection, allItems);
        var xml = rssService.SerializeToXml(channel);

        return Results.Content(xml, "application/rss+xml; charset=utf-8");
    }

    /// <summary>
    /// Gets RSS feed for a specific section
    /// </summary>
    private static async Task<IResult> GetSectionFeed(
        string sectionName,
        ISectionRepository sectionRepository,
        IContentRepository contentRepository,
        IRssService rssService)
    {
        var section = await sectionRepository.GetByNameAsync(sectionName);
        if (section is null)
        {
            return Results.NotFound();
        }

        // Get content for this section using the section name (lowercase identifier)
        // RSS feeds should exclude draft content
        var items = await contentRepository.GetBySectionAsync(section.Name, includeDraft: false);
        var channel = await rssService.GenerateSectionFeedAsync(section, items);
        var xml = rssService.SerializeToXml(channel);

        return Results.Content(xml, "application/rss+xml; charset=utf-8");
    }

    /// <summary>
    /// Gets RSS feed for a specific collection
    /// </summary>
    private static async Task<IResult> GetCollectionFeed(
        string collectionName,
        ISectionRepository sectionRepository,
        IContentRepository contentRepository,
        IRssService rssService)
    {
        // Get all sections to find if this collection exists
        var sections = await sectionRepository.GetAllAsync();
        var collection = sections
            .SelectMany(s => s.Collections)
            .FirstOrDefault(c => c.Name.Equals(collectionName, StringComparison.OrdinalIgnoreCase));

        if (collection is null)
        {
            return Results.NotFound();
        }

        var items = await contentRepository.GetByCollectionAsync(collectionName, includeDraft: false);
        var channel = await rssService.GenerateCollectionFeedAsync(collectionName, items);
        var xml = rssService.SerializeToXml(channel);

        return Results.Content(xml, "application/rss+xml; charset=utf-8");
    }
}
