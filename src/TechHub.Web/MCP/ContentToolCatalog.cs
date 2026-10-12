using System.ComponentModel;
using ModelContextProtocol.Server;
using TechHub.Core.Models;

namespace TechHub.Web.MCP;

/// <summary>
/// Builds one read-only MCP tool per configured section from <see cref="SectionCache"/>, so the MCP surface
/// follows the configured sections instead of a second hard-coded list. The aggregate "all" section is
/// intentionally not exposed. The tool list is rebuilt only when the cache publishes a new section snapshot.
/// </summary>
internal sealed class ContentToolCatalog
{
    internal const string PageDescription = "Page number to retrieve, starting at 1 (maximum 100). Each page contains up to 10 items.";

    private const string ResultDescription =
        " Returns the newest items first, 10 per page, along with page, pageSize, totalPages and totalCount. " +
        "Use the page parameter to retrieve further pages. Each item's url links to the item on Tech Hub.";

    private readonly SectionCache _sectionCache;
    private readonly IServiceProvider _services;
    private Snapshot _snapshot = new([], []);

    public ContentToolCatalog(SectionCache sectionCache, IServiceProvider services)
    {
        _sectionCache = sectionCache;
        _services = services;
    }

    public static string GetToolName(string sectionName) =>
        $"get_latest_{sectionName.Replace('-', '_')}_content";

    public IReadOnlyList<McpServerTool> GetTools()
    {
        var sections = _sectionCache.Sections;
        var snapshot = Volatile.Read(ref _snapshot);
        if (ReferenceEquals(snapshot.Sections, sections))
        {
            return snapshot.Tools;
        }

        var tools = sections
            .Where(s => !string.Equals(s.Name, "all", StringComparison.OrdinalIgnoreCase))
            .Select(CreateTool)
            .ToList();

        Volatile.Write(ref _snapshot, new Snapshot(sections, tools));
        return tools;
    }

    private McpServerTool CreateTool(Section section)
    {
        var sectionName = section.Name;

        return McpServerTool.Create(
            (LatestContentProvider provider, [Description(PageDescription)] int page = 1, CancellationToken cancellationToken = default)
                => provider.GetLatestAsync(sectionName, page, cancellationToken),
            new McpServerToolCreateOptions
            {
                Name = GetToolName(sectionName),
                Title = $"Latest {section.Title} content",
                Description = $"Gets the latest {section.Title} content on Tech Hub: news, blogs, videos, community posts and roundups." + ResultDescription,
                ReadOnly = true,
                Idempotent = true,
                Destructive = false,
                OpenWorld = false,
                Services = _services,
            });
    }

    private sealed record Snapshot(IReadOnlyList<Section> Sections, IReadOnlyList<McpServerTool> Tools);
}
