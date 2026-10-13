using System.ComponentModel;
using ModelContextProtocol.Server;
using TechHub.Core.Models;

namespace TechHub.Web.MCP;

/// <summary>
/// Builds the read-only MCP tools from <see cref="SectionCache"/>: one latest-content tool per configured section
/// (the aggregate "all" section becomes the cross-section <c>get_latest_content</c> tool) plus <c>get_roundup_content</c>,
/// so the MCP surface follows the configured sections instead of a second hard-coded list.
/// The tool list is rebuilt only when the cache publishes a new section snapshot.
/// </summary>
internal sealed class ContentToolCatalog
{
    internal const string GetRoundupContentToolName = "get_roundup_content";

    internal const string PageDescription = "Page number to retrieve, starting at 1 (maximum 100). Each page contains up to 10 items.";

    private const string AllSectionName = "all";

    // The 90-day default window comes from the content API's DefaultDateRangeDays setting.
    private const string ResultDescription =
        " Returns items newest first, 10 per page, along with page, pageSize, totalPages, totalCount and generatedAt " +
        "(the server's current time, so you can judge how fresh the data is). " +
        "Only the last 90 days are included unless you pass days or since; use contentType=roundups for the weekly summaries. " +
        "Each item has title, author, date (ISO 8601), collection, excerpt and a Tech Hub url. " +
        "Items only carry an excerpt, never the full text. Always link users to the Tech Hub url: it shows the summary and a button " +
        "to open the original article. For items with collection roundups pass the url to get_roundup_content to read the full roundup. " +
        "Use the page parameter to retrieve further pages.";

    // Optional parameters use non-nullable types with sentinel defaults: nullable types emit ["integer","null"] schemas
    // that single-type clients (e.g. Gemini) reject.
    private const int DaysNotSet = -1;

    internal const string DaysDescription =
        "Only include items from the last N days (0-3650). Omit it to use the default of 90 days when since is not given; 0 removes the date window.";

    internal const string SinceDescription =
        "Only include items published on or after this date, formatted yyyy-MM-dd. Takes precedence over days.";

    internal const string ContentTypeDescription =
        "Comma-separated content types to include: news, blogs, videos, community, roundups.";

    internal const string TagsDescription =
        "Comma-separated tags; items must have all of them, for example 'MCP,Microsoft Agent Framework'.";

    internal const string QueryDescription =
        "Keyword search across title, description and tags (maximum 200 characters).";

    internal const string IncludeTagsDescription =
        "Include the tags of every item. Off by default to keep results small.";

    private readonly SectionCache _sectionCache;
    private readonly IServiceProvider _services;
    private Snapshot? _snapshot;

    public ContentToolCatalog(SectionCache sectionCache, IServiceProvider services)
    {
        _sectionCache = sectionCache;
        _services = services;
    }

    public static string GetToolName(string sectionName) =>
        IsAllSection(sectionName)
            ? "get_latest_content"
            : $"get_latest_{sectionName.Replace('-', '_')}_content";

    public IReadOnlyList<McpServerTool> GetTools()
    {
        var sections = _sectionCache.Sections;
        var snapshot = Volatile.Read(ref _snapshot);
        if (snapshot is not null && ReferenceEquals(snapshot.Sections, sections))
        {
            return snapshot.Tools;
        }

        var tools = sections
            .Select(CreateLatestContentTool)
            .Append(CreateRoundupContentTool())
            .ToList();

        Volatile.Write(ref _snapshot, new Snapshot(sections, tools));
        return tools;
    }

    private static bool IsAllSection(string sectionName) =>
        string.Equals(sectionName, AllSectionName, StringComparison.OrdinalIgnoreCase);

    private McpServerTool CreateLatestContentTool(Section section)
    {
        var sectionName = section.Name;
        var isAll = IsAllSection(sectionName);

        var description = isAll
            ? "Gets the latest content across all sections on Tech Hub in one list: news, blogs, videos, community posts and roundups. " +
              "Each item appears once, even if it belongs to several sections, so prefer this over combining the per-section tools."
            : $"Gets the latest {section.Title} content on Tech Hub: news, blogs, videos, community posts and roundups. " +
              "Items can belong to several sections; use get_latest_content to avoid duplicates across sections.";

        return McpServerTool.Create(
            (LatestContentProvider provider,
                [Description(PageDescription)] int page = 1,
                [Description(DaysDescription)] int days = DaysNotSet,
                [Description(SinceDescription)] string since = "",
                [Description(ContentTypeDescription)] string contentType = "",
                [Description(TagsDescription)] string tags = "",
                [Description(QueryDescription)] string query = "",
                [Description(IncludeTagsDescription)] bool includeTags = false,
                CancellationToken cancellationToken = default)
                => provider.GetLatestAsync(
                    sectionName,
                    new ContentFilter
                    {
                        Page = page,
                        Days = days == DaysNotSet ? null : days,
                        Since = since,
                        ContentTypes = contentType,
                        Tags = tags,
                        Query = query,
                        IncludeTags = includeTags,
                    },
                    cancellationToken),
            new McpServerToolCreateOptions
            {
                Name = GetToolName(sectionName),
                Title = isAll ? "Latest content across all sections" : $"Latest {section.Title} content",
                Description = description + ResultDescription,
                ReadOnly = true,
                Idempotent = true,
                Destructive = false,
                OpenWorld = false,
                Services = _services,
            });
    }

    private McpServerTool CreateRoundupContentTool() =>
        McpServerTool.Create(
            (RoundupContentProvider provider,
                [Description("The Tech Hub roundup url, as returned by a latest-content tool with contentType=roundups.")] string url,
                CancellationToken cancellationToken = default)
                => provider.GetAsync(url, cancellationToken),
            new McpServerToolCreateOptions
            {
                Name = GetRoundupContentToolName,
                Title = "Roundup content",
                Description = "Gets the full content (HTML) of a Tech Hub weekly roundup, an editorial summary of the week, by its url. " +
                    "Use it when a latest-content result has collection roundups and its excerpt is not enough. " +
                    "Find roundup urls with a latest-content tool and contentType=roundups. Only roundup urls are accepted.",
                ReadOnly = true,
                Idempotent = true,
                Destructive = false,
                OpenWorld = false,
                Services = _services,
            });

    private sealed record Snapshot(IReadOnlyList<Section> Sections, IReadOnlyList<McpServerTool> Tools);
}
