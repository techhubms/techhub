using TechHub.Core.Models;

namespace TechHub.Web.Services;

/// <summary>
/// Typed HTTP client for calling Tech Hub API endpoints.
/// Uses the unified /api/sections hierarchy for all content access.
/// Public to allow mocking in unit tests (virtual methods require public class for Moq proxies).
/// </summary>
public class TechHubApiClient : ITechHubApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TechHubApiClient> _logger;

    public TechHubApiClient(HttpClient httpClient, ILogger<TechHubApiClient> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(logger);

        _httpClient = httpClient;
        _logger = logger;
    }

    // ================================================================
    // Section endpoints
    // ================================================================

    /// <summary>
    /// Get all sections with their collections
    /// GET /api/sections
    /// </summary>
    public virtual async Task<IEnumerable<Section>?> GetAllSectionsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Fetching all sections from API");
            var sections = await _httpClient.GetFromJsonAsync<IEnumerable<Section>>(
                "/api/sections",
                cancellationToken);

            _logger.LogDebug("Successfully fetched {Count} sections", sections?.Count() ?? 0);
            return sections;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch sections from API");
            throw;
        }
    }

    /// <summary>
    /// Get a specific section by name
    /// GET /api/sections/{sectionName}
    /// </summary>
    public virtual async Task<Section?> GetSectionAsync(string sectionName, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Fetching section: {SectionName}", sectionName);
            var response = await _httpClient.GetAsync($"/api/sections/{sectionName}", cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Section not found: {SectionName}", sectionName);
                return null;
            }

            response.EnsureSuccessStatusCode();
            var section = await response.Content.ReadFromJsonAsync<Section>(cancellationToken: cancellationToken);
            return section;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch section {SectionName}", sectionName);
            throw;
        }
    }
    /// <summary>
    /// Get all collections in a section
    /// GET /api/sections/{sectionName}/collections
    /// </summary>
    public virtual async Task<IEnumerable<Collection>?> GetSectionCollectionsAsync(
        string sectionName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Fetching collections for section: {SectionName}", sectionName);
            var response = await _httpClient.GetAsync($"/api/sections/{sectionName}/collections", cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Section not found: {SectionName}", sectionName);
                return null;
            }

            response.EnsureSuccessStatusCode();
            var collections = await response.Content.ReadFromJsonAsync<IEnumerable<Collection>>(cancellationToken: cancellationToken);
            return collections;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch collections for section {SectionName}", sectionName);
            throw;
        }
    }

    // ================================================================
    // Collection endpoints
    // ================================================================

    /// <summary>
    /// Get a specific collection in a section
    /// GET /api/sections/{sectionName}/collections/{collectionName}
    /// </summary>
    public virtual async Task<Collection?> GetCollectionAsync(
        string sectionName,
        string collectionName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Fetching collection: {SectionName}/{CollectionName}", sectionName, collectionName);
            var response = await _httpClient.GetAsync(
                $"/api/sections/{sectionName}/collections/{collectionName}",
                cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Collection not found: {SectionName}/{CollectionName}", sectionName, collectionName);
                return null;
            }

            response.EnsureSuccessStatusCode();
            var collection = await response.Content.ReadFromJsonAsync<Collection>(cancellationToken: cancellationToken);
            return collection;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch collection {SectionName}/{CollectionName}", sectionName, collectionName);
            throw;
        }
    }

    /// <summary>
    /// Get items in a collection with optional filtering
    /// GET /api/sections/{sectionName}/collections/{collectionName}/items?take=&amp;skip=&amp;q=&amp;tags=&amp;subcollection=&amp;lastDays=&amp;from=&amp;to=&amp;includeDraft=
    /// </summary>
    public virtual async Task<IEnumerable<ContentItem>?> GetCollectionItemsAsync(
        string sectionName,
        string collectionName,
        int? take = null,
        int? skip = null,
        string? query = null,
        string? tags = null,
        string? subcollection = null,
        int? lastDays = null,
        string? fromDate = null,
        string? toDate = null,
        bool includeDraft = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var queryParams = new List<string>();
            if (take.HasValue)
            {
                queryParams.Add($"take={take.Value}");
            }

            if (skip.HasValue)
            {
                queryParams.Add($"skip={skip.Value}");
            }

            if (!string.IsNullOrEmpty(query))
            {
                queryParams.Add($"q={Uri.EscapeDataString(query)}");
            }

            if (!string.IsNullOrEmpty(tags))
            {
                queryParams.Add($"tags={Uri.EscapeDataString(tags)}");
            }

            if (!string.IsNullOrEmpty(subcollection))
            {
                queryParams.Add($"subcollection={Uri.EscapeDataString(subcollection)}");
            }

            if (lastDays.HasValue)
            {
                queryParams.Add($"lastDays={lastDays.Value}");
            }

            if (!string.IsNullOrEmpty(fromDate))
            {
                queryParams.Add($"from={Uri.EscapeDataString(fromDate)}");
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                queryParams.Add($"to={Uri.EscapeDataString(toDate)}");
            }

            if (includeDraft)
            {
                queryParams.Add("includeDraft=true");
            }

            var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
            var url = $"/api/sections/{sectionName}/collections/{collectionName}/items{queryString}";

            _logger.LogDebug("Fetching items for collection: {SectionName}/{CollectionName}", sectionName, collectionName);
            var items = await _httpClient.GetFromJsonAsync<IEnumerable<ContentItem>>(url, cancellationToken);

            _logger.LogDebug("Successfully fetched {Count} items for collection {SectionName}/{CollectionName}",
                items?.Count() ?? 0, sectionName, collectionName);
            return items;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch items for collection {SectionName}/{CollectionName}", sectionName, collectionName);
            throw;
        }
    }

    /// <summary>
    /// Get tag cloud for a collection
    /// GET /api/sections/{sectionName}/collections/{collectionName}/tags
    /// </summary>
    public virtual async Task<IReadOnlyList<TagCloudItem>?> GetCollectionTagsAsync(
        string sectionName,
        string collectionName,
        int? maxTags = null,
        int? minUses = null,
        int? lastDays = null,
        List<string>? selectedTags = null,
        List<string>? tagsToCount = null,
        string? fromDate = null,
        string? toDate = null,
        string? searchQuery = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var queryParams = new List<string>();
            if (maxTags.HasValue)
            {
                queryParams.Add($"maxTags={maxTags.Value}");
            }

            if (minUses.HasValue)
            {
                queryParams.Add($"minUses={minUses.Value}");
            }

            if (lastDays.HasValue)
            {
                queryParams.Add($"lastDays={lastDays.Value}");
            }

            // Dynamic counts: Add selected tags parameter (for intersection counts)
            if (selectedTags != null && selectedTags.Count > 0)
            {
                var tagsParam = string.Join(",", selectedTags.Select(t => Uri.EscapeDataString(t)));
                queryParams.Add($"tags={tagsParam}");
            }

            // Specific tags to get counts for (baseline tags)
            if (tagsToCount != null && tagsToCount.Count > 0)
            {
                var tagsToCountParam = string.Join(",", tagsToCount.Select(t => Uri.EscapeDataString(t)));
                queryParams.Add($"tagsToCount={tagsToCountParam}");
            }

            // Dynamic counts: Add date range parameters
            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                queryParams.Add($"from={Uri.EscapeDataString(fromDate)}");
            }

            if (!string.IsNullOrWhiteSpace(toDate))
            {
                queryParams.Add($"to={Uri.EscapeDataString(toDate)}");
            }

            // Text search filter
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                queryParams.Add($"q={Uri.EscapeDataString(searchQuery)}");
            }

            var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
            var url = $"/api/sections/{sectionName}/collections/{collectionName}/tags{queryString}";

            _logger.LogDebug("Fetching tag cloud for collection: {SectionName}/{CollectionName}", sectionName, collectionName);
            var tagCloud = await _httpClient.GetFromJsonAsync<IReadOnlyList<TagCloudItem>>(url, cancellationToken);

            _logger.LogDebug("Successfully fetched {Count} tags for collection {SectionName}/{CollectionName}",
                tagCloud?.Count ?? 0, sectionName, collectionName);
            return tagCloud;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch tag cloud for collection {SectionName}/{CollectionName}", sectionName, collectionName);
            throw;
        }
    }

    // ================================================================
    // Content item endpoints
    // ================================================================

    /// <summary>
    /// Get content item detail by section, collection, and slug
    /// GET /api/sections/{sectionName}/collections/{collectionName}/{slug}
    /// </summary>
    public virtual async Task<ContentItemDetail?> GetContentDetailAsync(
        string sectionName,
        string collectionName,
        string slug,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Fetching content detail: {SectionName}/{CollectionName}/{Slug}",
                sectionName, collectionName, slug);

            var response = await _httpClient.GetAsync(
                $"/api/sections/{sectionName}/collections/{collectionName}/{slug}",
                cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Content not found: {SectionName}/{CollectionName}/{Slug}",
                    sectionName, collectionName, slug);
                return null;
            }

            response.EnsureSuccessStatusCode();
            var item = await response.Content.ReadFromJsonAsync<ContentItemDetail>(cancellationToken: cancellationToken);
            return item;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch content detail for {SectionName}/{CollectionName}/{Slug}",
                sectionName, collectionName, slug);
            throw;
        }
    }

    // ================================================================
    // Convenience methods (built on top of the unified API)
    // ================================================================

    /// <summary>
    /// Get the latest items across all sections (for homepage sidebar).
    /// Fetches items from "all" virtual section with "all" collection.
    /// </summary>
    public virtual async Task<IEnumerable<ContentItem>?> GetLatestItemsAsync(
        int count = 10,
        CancellationToken cancellationToken = default)
    {
        // Use "all" virtual section and "all" collection to get items across everything
        return await GetCollectionItemsAsync("all", "all", take: count, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Get the latest roundup (for homepage sidebar)
    /// </summary>
    public virtual async Task<ContentItem?> GetLatestRoundupAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Get latest 1 item from roundups collection in all sections
            var items = await GetCollectionItemsAsync(
                "all",
                "roundups",
                take: 1,
                cancellationToken: cancellationToken);

            return items?.FirstOrDefault();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch latest roundup");
            throw;
        }
    }

    /// <summary>
    /// Get GitHub Copilot feature videos (subcollection=ghc-features), including drafts.
    /// This is the ONLY endpoint that returns draft content.
    /// </summary>
    public virtual async Task<IEnumerable<ContentItem>?> GetGhcFeaturesAsync(
        CancellationToken cancellationToken = default)
    {
        return await GetCollectionItemsAsync(
            "github-copilot",
            "videos",
            subcollection: "ghc-features",
            includeDraft: true,
            cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Get tag cloud for specified scope.
    /// Uses /api/sections/{sectionName}/collections/{collectionName}/tags endpoint.
    /// Pass "all" as collectionName for section-level tag cloud.
    /// Supports dynamic counts via selectedTags and date range parameters.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2119:Seal methods that satisfy private interfaces", Justification = "Virtual methods are intentional for testing/mocking support")]
    public virtual async Task<IReadOnlyList<TagCloudItem>?> GetTagCloudAsync(
        string sectionName,
        string collectionName,
        int? maxTags = null,
        int? minUses = null,
        int? lastDays = null,
        List<string>? selectedTags = null,
        List<string>? tagsToCount = null,
        string? fromDate = null,
        string? toDate = null,
        string? searchQuery = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sectionName);
        ArgumentException.ThrowIfNullOrWhiteSpace(collectionName);

        return await GetCollectionTagsAsync(sectionName, collectionName, maxTags, minUses, lastDays, selectedTags, tagsToCount, fromDate, toDate, searchQuery, cancellationToken);
    }

    // ================================================================
    // RSS feed endpoints
    // ================================================================

    /// <summary>
    /// Get RSS feed for all content
    /// </summary>
    public virtual async Task<string> GetAllContentRssFeedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Fetching RSS feed for all content");
            var response = await _httpClient.GetAsync(new Uri("/api/rss/all", UriKind.Relative), cancellationToken);
            response.EnsureSuccessStatusCode();

            var xml = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogDebug("Successfully fetched RSS feed for all content");
            return xml;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch RSS feed for all content");
            throw;
        }
    }

    /// <summary>
    /// Get RSS feed for a specific section
    /// </summary>
    public virtual async Task<string> GetSectionRssFeedAsync(string sectionName, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Fetching RSS feed for section: {SectionName}", sectionName);
            var response = await _httpClient.GetAsync(new Uri($"/api/rss/{sectionName}", UriKind.Relative), cancellationToken);
            response.EnsureSuccessStatusCode();

            var xml = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogDebug("Successfully fetched RSS feed for section: {SectionName}", sectionName);
            return xml;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch RSS feed for section {SectionName}", sectionName);
            throw;
        }
    }

    /// <summary>
    /// Get RSS feed for a specific collection within a section (or across all sections)
    /// </summary>
    public virtual async Task<string> GetCollectionRssFeedAsync(string collectionName, string sectionName = "all", CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Fetching RSS feed for collection: {CollectionName} in section: {SectionName}", collectionName, sectionName);
            var response = await _httpClient.GetAsync(new Uri($"/api/rss/{sectionName}/{collectionName}", UriKind.Relative), cancellationToken);
            response.EnsureSuccessStatusCode();

            var xml = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogDebug("Successfully fetched RSS feed for collection: {CollectionName} in section: {SectionName}", collectionName, sectionName);
            return xml;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch RSS feed for collection {CollectionName} in section {SectionName}", collectionName, sectionName);
            throw;
        }
    }

    // ================================================================
    // Custom pages endpoints
    // ================================================================

    /// <summary>
    /// Get DX Space page data
    /// </summary>
    public virtual async Task<DXSpacePageData?> GetDXSpaceDataAsync(CancellationToken cancellationToken = default)
    {
        return await GetCustomPageDataAsync<DXSpacePageData>("/api/custom-pages/dx-space", "DX Space", cancellationToken);
    }

    /// <summary>
    /// Get Handbook page data
    /// </summary>
    public virtual async Task<HandbookPageData?> GetHandbookDataAsync(CancellationToken cancellationToken = default)
    {
        return await GetCustomPageDataAsync<HandbookPageData>("/api/custom-pages/handbook", "Handbook", cancellationToken);
    }

    /// <summary>
    /// Get Levels of Enlightenment page data
    /// </summary>
    public virtual async Task<LevelsPageData?> GetLevelsDataAsync(CancellationToken cancellationToken = default)
    {
        return await GetCustomPageDataAsync<LevelsPageData>("/api/custom-pages/levels", "Levels", cancellationToken);
    }

    /// <summary>
    /// Get Features page data
    /// </summary>
    public virtual async Task<FeaturesPageData?> GetFeaturesDataAsync(CancellationToken cancellationToken = default)
    {
        return await GetCustomPageDataAsync<FeaturesPageData>("/api/custom-pages/features", "Features", cancellationToken);
    }

    /// <summary>
    /// Get GenAI Basics page data
    /// </summary>
    public virtual async Task<GenAIPageData?> GetGenAIBasicsDataAsync(CancellationToken cancellationToken = default)
    {
        return await GetCustomPageDataAsync<GenAIPageData>("/api/custom-pages/genai-basics", "GenAI Basics", cancellationToken);
    }

    /// <summary>
    /// Get GenAI Advanced page data
    /// </summary>
    public virtual async Task<GenAIPageData?> GetGenAIAdvancedDataAsync(CancellationToken cancellationToken = default)
    {
        return await GetCustomPageDataAsync<GenAIPageData>("/api/custom-pages/genai-advanced", "GenAI Advanced", cancellationToken);
    }

    /// <summary>
    /// Get GenAI Applied page data
    /// </summary>
    public virtual async Task<GenAIPageData?> GetGenAIAppliedDataAsync(CancellationToken cancellationToken = default)
    {
        return await GetCustomPageDataAsync<GenAIPageData>("/api/custom-pages/genai-applied", "GenAI Applied", cancellationToken);
    }

    /// <summary>
    /// Get SDLC page data
    /// </summary>
    public virtual async Task<SDLCPageData?> GetSDLCDataAsync(CancellationToken cancellationToken = default)
    {
        return await GetCustomPageDataAsync<SDLCPageData>("/api/custom-pages/sdlc", "SDLC", cancellationToken);
    }

    // ================================================================
    // Helper methods
    // ================================================================

    private async Task<T?> GetCustomPageDataAsync<T>(string url, string pageName, CancellationToken cancellationToken)
        where T : class
    {
        try
        {
            _logger.LogDebug("Fetching {PageName} page data", pageName);
            var response = await _httpClient.GetAsync(url, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("{PageName} data not found", pageName);
                return null;
            }

            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
            return data;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch {PageName} data", pageName);
            throw;
        }
    }
}
