using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TechHub.Core.Models;
using TechHub.Web.Services;

namespace TechHub.Web.MCP;

/// <summary>
/// Shared implementation behind every per-section MCP tool: pages through a section's latest content,
/// caches the API response, and records telemetry.
/// </summary>
internal sealed class LatestContentProvider
{
    internal const int PageSize = 10;
    internal const int MaxPage = 100;

    private static readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);

    private readonly ITechHubApiClient _apiClient;
    private readonly IMemoryCache _cache;
    private readonly string _baseUrl;

    public LatestContentProvider(ITechHubApiClient apiClient, IMemoryCache cache, IOptions<McpOptions> options)
    {
        _apiClient = apiClient;
        _cache = cache;
        _baseUrl = options.Value.BaseUrl.TrimEnd('/');
    }

    public async Task<ContentPage> GetLatestAsync(string sectionName, int page, CancellationToken cancellationToken)
    {
        page = Math.Clamp(page, 1, MaxPage);

        using var activity = McpTelemetry.ActivitySource.StartActivity("McpContent.GetLatest");
        activity?.SetTag("techhub.section", sectionName);
        activity?.SetTag("techhub.page", page);

        var startedAt = Stopwatch.GetTimestamp();
        var result = "error";
        try
        {
            // The cache holds the raw API response; absolute URLs are built after the lookup.
            var cacheKey = $"mcp:latest:{sectionName}:{page}";
            var cacheHit = _cache.TryGetValue(cacheKey, out CollectionItemsResponse? response) && response is not null;

            if (!cacheHit)
            {
                response = await _apiClient.GetCollectionItemsAsync(
                    sectionName, "all", take: PageSize, skip: (page - 1) * PageSize, cancellationToken: cancellationToken);

                if (response is not null)
                {
                    _cache.Set(cacheKey, response, _cacheDuration);
                }
            }

            result = cacheHit ? "cache_hit" : "cache_miss";
            activity?.SetTag("techhub.result", result);

            return ToContentPage(response ?? new CollectionItemsResponse([], 0), page);
        }
        catch
        {
            activity?.SetStatus(ActivityStatusCode.Error);
            throw;
        }
        finally
        {
            McpTelemetry.RecordRequest(sectionName, result, Stopwatch.GetElapsedTime(startedAt));
        }
    }

    private ContentPage ToContentPage(CollectionItemsResponse response, int page)
    {
        var items = response.Items
            .Select(i => new ContentPageItem(i.Title, i.Author, i.DateEpoch, i.Tags, i.Excerpt, $"{_baseUrl}{i.GetCanonicalHref()}"))
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
