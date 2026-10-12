using System.ComponentModel;
using ModelContextProtocol.Server;
using TechHub.Web.Services;

namespace TechHub.Web.MCP;

[McpServerToolType]
internal class ContentTools(ITechHubApiClient techHubApiClient, IHttpContextAccessor httpContextAccessor)
{
    private const int PageSize = 10;

    [McpServerTool, Description("Gets the latest GitHub Copilot related content, newest first. Returns 10 items per page along with page, totalPages and totalCount. Use the page parameter (1-based, default 1) to retrieve further pages. Each item's URL links to the item on Tech Hub.")]
    public async Task<ContentPage?> GetGitHubCopilotContentAsync(
        [Description("Page number to retrieve, starting at 1. Each page contains up to 10 items.")] int page = 1)
    {
        page = Math.Max(page, 1);

        var response = await techHubApiClient.GetCollectionItemsAsync("github-copilot", "all", take: PageSize, skip: (page - 1) * PageSize);
        if (response is null)
        {
            return null;
        }

        var request = httpContextAccessor.HttpContext?.Request;
        var baseUrl = request is null ? string.Empty : $"{request.Scheme}://{request.Host}";

        var items = response.Items
            .Select(i => new ContentPageItem(i.Title, i.Author, i.DateEpoch, i.Tags, i.Excerpt, $"{baseUrl}{i.GetCanonicalHref()}"))
            .ToList();

        return new ContentPage(items, page, PageSize, (int)Math.Ceiling(response.TotalCount / (double)PageSize), response.TotalCount);
    }
}

internal record ContentPage(
    IReadOnlyList<ContentPageItem> Items,
    int Page,
    int PageSize,
    int TotalPages,
    long TotalCount);

internal record ContentPageItem(
    string Title,
    string Author,
    long DateEpoch,
    IReadOnlyList<string> Tags,
    string Excerpt,
    string Url);