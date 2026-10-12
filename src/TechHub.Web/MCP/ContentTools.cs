using System.ComponentModel;
using ModelContextProtocol.Server;

namespace TechHub.Web.MCP;

/// <summary>
/// One explicit MCP tool per section. Every tool delegates to <see cref="LatestContentProvider"/>,
/// so a new section only needs one more method here.
/// </summary>
[McpServerToolType]
internal sealed class ContentTools
{
    private const string PageDescription = "Page number to retrieve, starting at 1 (maximum 100). Each page contains up to 10 items.";

    private const string ResultDescription =
        " Returns the newest items first, 10 per page, along with page, pageSize, totalPages and totalCount. " +
        "Use the page parameter to retrieve further pages. Each item's url links to the item on Tech Hub.";

    private readonly LatestContentProvider _provider;

    public ContentTools(LatestContentProvider provider)
    {
        _provider = provider;
    }

    [McpServerTool(Name = "get_latest_github_copilot_content", Title = "Latest GitHub Copilot content", ReadOnly = true, Idempotent = true, Destructive = false, OpenWorld = false)]
    [Description("Gets the latest GitHub Copilot content on Tech Hub: news, blogs, videos, community posts and roundups." + ResultDescription)]
    public Task<ContentPage> GetLatestGitHubCopilotContentAsync(
        [Description(PageDescription)] int page = 1,
        CancellationToken cancellationToken = default)
        => _provider.GetLatestAsync("github-copilot", page, cancellationToken);

    [McpServerTool(Name = "get_latest_ai_content", Title = "Latest AI content", ReadOnly = true, Idempotent = true, Destructive = false, OpenWorld = false)]
    [Description("Gets the latest Artificial Intelligence (AI) content on Tech Hub: news, blogs, videos, community posts and roundups." + ResultDescription)]
    public Task<ContentPage> GetLatestAiContentAsync(
        [Description(PageDescription)] int page = 1,
        CancellationToken cancellationToken = default)
        => _provider.GetLatestAsync("ai", page, cancellationToken);

    [McpServerTool(Name = "get_latest_ml_content", Title = "Latest Machine Learning content", ReadOnly = true, Idempotent = true, Destructive = false, OpenWorld = false)]
    [Description("Gets the latest Machine Learning (ML) content on Tech Hub: news, blogs, videos, community posts and roundups." + ResultDescription)]
    public Task<ContentPage> GetLatestMlContentAsync(
        [Description(PageDescription)] int page = 1,
        CancellationToken cancellationToken = default)
        => _provider.GetLatestAsync("ml", page, cancellationToken);

    [McpServerTool(Name = "get_latest_dotnet_content", Title = "Latest .NET content", ReadOnly = true, Idempotent = true, Destructive = false, OpenWorld = false)]
    [Description("Gets the latest .NET content on Tech Hub: news, blogs, videos, community posts and roundups." + ResultDescription)]
    public Task<ContentPage> GetLatestDotNetContentAsync(
        [Description(PageDescription)] int page = 1,
        CancellationToken cancellationToken = default)
        => _provider.GetLatestAsync("dotnet", page, cancellationToken);

    [McpServerTool(Name = "get_latest_azure_content", Title = "Latest Azure content", ReadOnly = true, Idempotent = true, Destructive = false, OpenWorld = false)]
    [Description("Gets the latest Azure content on Tech Hub: news, blogs, videos, community posts and roundups." + ResultDescription)]
    public Task<ContentPage> GetLatestAzureContentAsync(
        [Description(PageDescription)] int page = 1,
        CancellationToken cancellationToken = default)
        => _provider.GetLatestAsync("azure", page, cancellationToken);

    [McpServerTool(Name = "get_latest_devops_content", Title = "Latest DevOps content", ReadOnly = true, Idempotent = true, Destructive = false, OpenWorld = false)]
    [Description("Gets the latest DevOps content on Tech Hub: news, blogs, videos, community posts and roundups." + ResultDescription)]
    public Task<ContentPage> GetLatestDevOpsContentAsync(
        [Description(PageDescription)] int page = 1,
        CancellationToken cancellationToken = default)
        => _provider.GetLatestAsync("devops", page, cancellationToken);

    [McpServerTool(Name = "get_latest_security_content", Title = "Latest Security content", ReadOnly = true, Idempotent = true, Destructive = false, OpenWorld = false)]
    [Description("Gets the latest Security content on Tech Hub: news, blogs, videos, community posts and roundups." + ResultDescription)]
    public Task<ContentPage> GetLatestSecurityContentAsync(
        [Description(PageDescription)] int page = 1,
        CancellationToken cancellationToken = default)
        => _provider.GetLatestAsync("security", page, cancellationToken);
}
