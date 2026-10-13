using System.Diagnostics;
using System.Globalization;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using ModelContextProtocol;
using TechHub.Core.Models;
using TechHub.Core.Validation;
using TechHub.Web.Services;

namespace TechHub.Web.MCP;

/// <summary>
/// Shared implementation behind every latest-content MCP tool: pages through a section's content with optional
/// filters, caches the API response, and records telemetry.
/// </summary>
internal sealed class LatestContentProvider
{
    internal const int PageSize = 10;
    internal const int MaxPage = 100;

    // Mirror the limits enforced by the content API so callers get a readable error instead of a failed request.
    internal const int MaxDays = 3650;
    internal const int MaxQueryLength = 200;
    internal const int MaxTagCount = 20;
    internal const int MaxTagLength = 100;
    internal const int MaxContentTypeCount = 10;

    private const string SinceFormat = "yyyy-MM-dd";

    private static readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);

    private readonly ITechHubApiClient _apiClient;
    private readonly IMemoryCache _cache;
    private readonly SectionCache _sectionCache;
    private readonly TimeProvider _timeProvider;
    private readonly string _baseUrl;

    public LatestContentProvider(ITechHubApiClient apiClient, IMemoryCache cache, SectionCache sectionCache, IOptions<McpOptions> options, TimeProvider timeProvider)
    {
        _apiClient = apiClient;
        _cache = cache;
        _sectionCache = sectionCache;
        _timeProvider = timeProvider;
        _baseUrl = options.Value.BaseUrl.TrimEnd('/');
    }

    public async Task<ContentPage> GetLatestAsync(string sectionName, ContentFilter filter, CancellationToken cancellationToken)
    {
        var request = ApiRequest.From(filter, GetValidCollectionNames(sectionName));

        using var activity = McpTelemetry.ActivitySource.StartActivity("McpContent.GetLatest");
        activity?.SetTag("techhub.section", sectionName);
        activity?.SetTag("techhub.page", request.Page);

        var startedAt = Stopwatch.GetTimestamp();
        var result = "error";
        try
        {
            // Only the unfiltered default window is cached (key space: sections x MaxPage); any filter value
            // would let unauthenticated callers mint unbounded distinct keys in the shared cache.
            var cacheable = request is { Query: null, Tags: null, LastDays: null, FromDate: null, Types: null };
            var cacheKey = $"mcp:latest:{sectionName}:{request.Page}";

            CollectionItemsResponse? response = null;
            var cacheHit = cacheable && _cache.TryGetValue(cacheKey, out response) && response is not null;

            if (!cacheHit)
            {
                response = await _apiClient.GetCollectionItemsAsync(
                    sectionName,
                    "all",
                    take: PageSize,
                    skip: (request.Page - 1) * PageSize,
                    query: request.Query,
                    tags: request.Tags,
                    lastDays: request.LastDays,
                    fromDate: request.FromDate,
                    types: request.Types,
                    cancellationToken: cancellationToken);

                if (cacheable && response is not null)
                {
                    _cache.Set(cacheKey, response, _cacheDuration);
                }
            }

            result = !cacheable ? "cache_bypass" : cacheHit ? "cache_hit" : "cache_miss";
            activity?.SetTag("techhub.result", result);

            return ToContentPage(response ?? new CollectionItemsResponse([], 0), request.Page, filter.IncludeTags);
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

    // Null while the section is not cached yet; the API then reports the unknown section itself.
    private HashSet<string>? GetValidCollectionNames(string sectionName) =>
        _sectionCache.GetSectionByName(sectionName)?.Collections
            .Where(c => !c.IsCustom)
            .Select(c => c.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

    private ContentPage ToContentPage(CollectionItemsResponse response, int page, bool includeTags)
    {
        var items = response.Items
            .Select(i => new ContentPageItem(
                i.Title,
                i.Author,
                DateHelper.FormatTimestampIso(i.DateEpoch),
                i.CollectionName,
                i.Excerpt,
                $"{_baseUrl}{i.GetCanonicalHref()}")
            {
                Tags = includeTags ? i.Tags : null,
            })
            .ToList();

        // Pages beyond MaxPage are unreachable, so never advertise them.
        var totalPages = (int)Math.Min(MaxPage, Math.Ceiling(response.TotalCount / (double)PageSize));

        return new ContentPage(items, page, PageSize, totalPages, response.TotalCount, DateHelper.FormatTimestampIso(_timeProvider.GetUtcNow()));
    }

    /// <summary>Validated, normalized filter values in the shape the content API expects.</summary>
    private sealed record ApiRequest(int Page, string? Query, string? Tags, int? LastDays, string? FromDate, string? Types)
    {
        public static ApiRequest From(ContentFilter filter, HashSet<string>? validCollectionNames)
        {
            if (filter.Days is < 0 or > MaxDays)
            {
                throw new McpException($"days must be between 0 and {MaxDays}. Use 0 to remove the default date window.");
            }

            string? fromDate = null;
            if (!string.IsNullOrWhiteSpace(filter.Since))
            {
                if (!DateOnly.TryParseExact(filter.Since.Trim(), SinceFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var since))
                {
                    throw new McpException($"since must be a date in {SinceFormat} format, for example 2026-09-29.");
                }

                fromDate = since.ToString(SinceFormat, CultureInfo.InvariantCulture);
            }

            var query = filter.Query?.Trim();
            if (query?.Length > MaxQueryLength)
            {
                throw new McpException($"query must not exceed {MaxQueryLength} characters.");
            }

            var tags = Split(filter.Tags);
            if (tags.Length > MaxTagCount || tags.Any(t => t.Length > MaxTagLength))
            {
                throw new McpException($"tags accepts at most {MaxTagCount} comma-separated tags of up to {MaxTagLength} characters each.");
            }

            var types = Split(filter.ContentTypes).Select(t => t.ToLowerInvariant()).ToArray();
            if (types.Length > MaxContentTypeCount || !types.All(RouteParameterValidator.IsValidNameSegment))
            {
                throw new McpException("contentType must be a comma-separated list of collection names, for example news,blogs,videos,community,roundups.");
            }

            // The API silently ignores unknown types and falls back to all collections, so reject them here.
            if (validCollectionNames is not null && types.FirstOrDefault(t => !validCollectionNames.Contains(t)) is { } unknownType)
            {
                throw new McpException($"contentType '{unknownType}' is not a collection of this section. Valid values: {string.Join(", ", validCollectionNames.Order(StringComparer.OrdinalIgnoreCase))}.");
            }

            return new ApiRequest(
                Math.Clamp(filter.Page, 1, MaxPage),
                string.IsNullOrEmpty(query) ? null : query,
                tags.Length == 0 ? null : string.Join(',', tags),
                // The API gives an explicit start date precedence over a day count, so only forward one of them.
                fromDate is null ? filter.Days : null,
                fromDate,
                types.Length == 0 ? null : string.Join(',', types));
        }

        private static string[] Split(string? value) =>
            value?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? [];
    }
}

/// <summary>Optional filters accepted by the latest-content tools.</summary>
internal sealed record ContentFilter
{
    public int Page { get; init; } = 1;
    public int? Days { get; init; }
    public string? Since { get; init; }
    public string? ContentTypes { get; init; }
    public string? Tags { get; init; }
    public string? Query { get; init; }
    public bool IncludeTags { get; init; }
}

internal record ContentPage(
    IReadOnlyList<ContentPageItem> Items,
    int Page,
    int PageSize,
    int TotalPages,
    long TotalCount,
    string GeneratedAt);

internal record ContentPageItem(
    string Title,
    string Author,
    string Date,
    string Collection,
    string Excerpt,
    string Url)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<string>? Tags { get; init; }
}
