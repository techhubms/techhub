using TechHub.Core.Logging;
using TechHub.Core.Models;
using TechHub.Core.Models.Admin;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Web.Services;

/// <summary>
/// Typed HTTP client for calling Tech Hub API endpoints.
/// Uses the unified /api/sections hierarchy for all content access.
/// Public to allow mocking in unit tests (virtual methods require public class for Moq proxies).
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2119:Seal methods that satisfy private interfaces", Justification = "Virtual methods are intentional for testing/mocking support via the ITechHubApiClient interface")]
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
        sectionName = sectionName.Sanitize();
        try
        {
            _logger.LogDebug("Fetching section: {SectionName}", sectionName);
            var response = await _httpClient.GetAsync($"/api/sections/{Uri.EscapeDataString(sectionName)}", cancellationToken);

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
        sectionName = sectionName.Sanitize();
        try
        {
            _logger.LogDebug("Fetching collections for section: {SectionName}", sectionName);
            var response = await _httpClient.GetAsync($"/api/sections/{Uri.EscapeDataString(sectionName)}/collections", cancellationToken);

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
        sectionName = sectionName.Sanitize();
        collectionName = collectionName.Sanitize();
        try
        {
            _logger.LogDebug("Fetching collection: {SectionName}/{CollectionName}", sectionName, collectionName);
            var response = await _httpClient.GetAsync(
                $"/api/sections/{Uri.EscapeDataString(sectionName)}/collections/{Uri.EscapeDataString(collectionName)}",
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
    public virtual async Task<CollectionItemsResponse?> GetCollectionItemsAsync(
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
        string? types = null,
        CancellationToken cancellationToken = default)
    {
        sectionName = sectionName.Sanitize();
        collectionName = collectionName.Sanitize();
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

            if (!string.IsNullOrEmpty(types))
            {
                queryParams.Add($"types={Uri.EscapeDataString(types)}");
            }

            var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
            var url = $"/api/sections/{Uri.EscapeDataString(sectionName)}/collections/{Uri.EscapeDataString(collectionName)}/items{queryString}";

            _logger.LogDebug("Fetching items for collection: {SectionName}/{CollectionName}", sectionName, collectionName);
            var result = await _httpClient.GetFromJsonAsync<CollectionItemsResponse>(url, cancellationToken);

            _logger.LogDebug("Successfully fetched {Count} items (total: {TotalCount}) for collection {SectionName}/{CollectionName}",
                result?.Items.Count ?? 0, result?.TotalCount ?? 0, sectionName, collectionName);
            return result;
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
        sectionName = sectionName.Sanitize();
        collectionName = collectionName.Sanitize();
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
            var url = $"/api/sections/{Uri.EscapeDataString(sectionName)}/collections/{Uri.EscapeDataString(collectionName)}/tags{queryString}";

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
        sectionName = sectionName.Sanitize();
        collectionName = collectionName.Sanitize();
        slug = slug.Sanitize();
        try
        {
            _logger.LogDebug("Fetching content detail: {SectionName}/{CollectionName}/{Slug}",
                sectionName, collectionName, slug);

            var response = await _httpClient.GetAsync(
                $"/api/sections/{Uri.EscapeDataString(sectionName)}/collections/{Uri.EscapeDataString(collectionName)}/{Uri.EscapeDataString(slug)}",
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
        var result = await GetCollectionItemsAsync("all", "all", take: count, cancellationToken: cancellationToken);
        return result?.Items;
    }

    /// <summary>
    /// Get the latest roundup (for homepage sidebar)
    /// </summary>
    public virtual async Task<ContentItem?> GetLatestRoundupAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await GetCollectionItemsAsync(
                "all",
                "roundups",
                take: 1,
                cancellationToken: cancellationToken);

            return result?.Items.Count > 0 ? result.Items[0] : null;
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
        const int PageSize = 50;
        var allItems = new List<ContentItem>();
        var skip = 0;

        while (true)
        {
            var result = await GetCollectionItemsAsync(
                "github-copilot",
                "videos",
                subcollection: "ghc-features",
                includeDraft: true,
                lastDays: 0,
                take: PageSize,
                skip: skip,
                cancellationToken: cancellationToken);

            if (result?.Items == null || result.Items.Count == 0)
            {
                break;
            }

            allItems.AddRange(result.Items);

            if (allItems.Count >= result.TotalCount)
            {
                break;
            }

            skip += result.Items.Count;
        }

        return allItems;
    }

    /// <summary>
    /// Get tag cloud for specified scope.
    /// Uses /api/sections/{sectionName}/collections/{collectionName}/tags endpoint.
    /// Pass "all" as collectionName for section-level tag cloud.
    /// Supports dynamic counts via selectedTags and date range parameters.
    /// </summary>
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
        sectionName = sectionName.Sanitize();
        try
        {
            _logger.LogDebug("Fetching RSS feed for section: {SectionName}", sectionName);
            var response = await _httpClient.GetAsync(new Uri($"/api/rss/{Uri.EscapeDataString(sectionName)}", UriKind.Relative), cancellationToken);
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
        collectionName = collectionName.Sanitize();
        sectionName = sectionName.Sanitize();
        try
        {
            _logger.LogDebug("Fetching RSS feed for collection: {CollectionName} in section: {SectionName}", collectionName, sectionName);
            var response = await _httpClient.GetAsync(new Uri($"/api/rss/{Uri.EscapeDataString(sectionName)}/{Uri.EscapeDataString(collectionName)}", UriKind.Relative), cancellationToken);
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

    /// <summary>
    /// Get Tool Tips page data
    /// </summary>
    public virtual async Task<ToolTipsPageData?> GetToolTipsDataAsync(CancellationToken cancellationToken = default)
    {
        return await GetCustomPageDataAsync<ToolTipsPageData>("/api/custom-pages/tool-tips", "Tool Tips", cancellationToken);
    }

    /// <summary>
    /// Get Getting Started page data
    /// </summary>
    public virtual async Task<GettingStartedPageData?> GetGettingStartedDataAsync(CancellationToken cancellationToken = default)
    {
        return await GetCustomPageDataAsync<GettingStartedPageData>("/api/custom-pages/getting-started", "Getting Started", cancellationToken);
    }

    /// <summary>
    /// Get hero banner data
    /// </summary>
    public virtual async Task<HeroBannerData?> GetHeroBannerDataAsync(CancellationToken cancellationToken = default)
    {
        return await GetCustomPageDataAsync<HeroBannerData>("/api/custom-pages/hero-banner", "Hero Banner", cancellationToken);
    }

    // ================================================================
    // Sitemap endpoint
    // ================================================================

    /// <summary>
    /// Get XML sitemap
    /// GET /api/sitemap
    /// </summary>
    public virtual async Task<string> GetSitemapAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Fetching sitemap from API");
            var response = await _httpClient.GetAsync(new Uri("/api/sitemap", UriKind.Relative), cancellationToken);
            response.EnsureSuccessStatusCode();

            var xml = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogDebug("Successfully fetched sitemap");
            return xml;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch sitemap");
            throw;
        }
    }

    // ================================================================
    // Author endpoints
    // ================================================================

    /// <summary>
    /// Get all authors with content item counts.
    /// GET /api/authors
    /// </summary>
    public virtual async Task<IReadOnlyList<AuthorSummary>?> GetAuthorsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Fetching all authors from API");
            var authors = await _httpClient.GetFromJsonAsync<IReadOnlyList<AuthorSummary>>(
                "/api/authors",
                cancellationToken);

            _logger.LogDebug("Successfully fetched {Count} authors", authors?.Count ?? 0);
            return authors;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch authors from API");
            throw;
        }
    }

    /// <summary>
    /// Get content items for a specific author.
    /// GET /api/authors/{authorName}/items
    /// </summary>
    public virtual async Task<CollectionItemsResponse?> GetAuthorItemsAsync(
        string authorName,
        int? take = null,
        int? skip = null,
        CancellationToken cancellationToken = default)
    {
        authorName = authorName.Sanitize();
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

            var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
            var url = $"/api/authors/{Uri.EscapeDataString(authorName)}/items{queryString}";

            _logger.LogDebug("Fetching items for author: {AuthorName}", authorName);
            var response = await _httpClient.GetAsync(url, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Author not found: {AuthorName}", authorName);
                return null;
            }

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<CollectionItemsResponse>(cancellationToken: cancellationToken);

            _logger.LogDebug("Successfully fetched {Count} items (total: {TotalCount}) for author {AuthorName}",
                result?.Items.Count ?? 0, result?.TotalCount ?? 0, authorName);
            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch items for author {AuthorName}", authorName);
            throw;
        }
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

    // ================================================================
    // Admin methods
    // ================================================================

    /// <summary>
    /// Trigger an immediate content processing run.
    /// POST /api/admin/processing/trigger
    /// </summary>
    public virtual async Task TriggerContentProcessingAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Triggering content processing run");
            using var response = await _httpClient.PostAsync("/api/admin/processing/trigger", null, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to trigger content processing");
            throw;
        }
    }

    /// <summary>
    /// Trigger an immediate roundup generation run.
    /// POST /api/admin/roundup/trigger
    /// </summary>
    public virtual async Task TriggerRoundupGenerationAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Triggering roundup generation run");
            using var response = await _httpClient.PostAsync("/api/admin/roundup/trigger", null, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to trigger roundup generation");
            throw;
        }
    }

    /// <summary>
    /// Trigger a bulk content fix run.
    /// POST /api/admin/content-fixer/trigger
    /// </summary>
    public virtual async Task TriggerContentFixerAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Triggering content fixer run");
            using var response = await _httpClient.PostAsync("/api/admin/content-fixer/trigger", null, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to trigger content fixer");
            throw;
        }
    }

    /// <summary>
    /// Cancel the currently running background job.
    /// POST /api/admin/processing/cancel
    /// </summary>
    public virtual async Task CancelRunningJobAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Requesting cancellation of running background job");
            using var response = await _httpClient.PostAsync("/api/admin/processing/cancel", null, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to cancel running job");
            throw;
        }
    }

    /// <summary>
    /// Get recent content processing job history.
    /// GET /api/admin/processing/jobs
    /// </summary>
    public virtual async Task<IReadOnlyList<ContentProcessingJob>> GetProcessingJobsAsync(
        int count = 20,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Fetching processing jobs (count: {Count})", count);
            var jobs = await _httpClient.GetFromJsonAsync<IReadOnlyList<ContentProcessingJob>>(
                $"/api/admin/processing/jobs?count={count}",
                cancellationToken);
            return jobs ?? [];
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch processing jobs");
            throw;
        }
    }

    /// <summary>
    /// Get a specific content processing job by ID.
    /// GET /api/admin/processing/jobs/{id}
    /// </summary>
    public virtual async Task<ContentProcessingJob?> GetProcessingJobByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var job = await _httpClient.GetFromJsonAsync<ContentProcessingJob>(
                $"/api/admin/processing/jobs/{id}",
                cancellationToken);
            return job;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch processing job {JobId}", id);
            throw;
        }
    }

    // ================================================================
    // RSS Feed config methods
    // ================================================================

    /// <summary>
    /// Get all RSS feed configurations.
    /// GET /api/admin/feeds
    /// </summary>
    public virtual async Task<IReadOnlyList<FeedConfig>> GetFeedConfigsAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var feeds = await _httpClient.GetFromJsonAsync<IReadOnlyList<FeedConfig>>(
                "/api/admin/feeds",
                cancellationToken);
            return feeds ?? [];
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch feed configs");
            throw;
        }
    }

    /// <summary>
    /// Get a specific RSS feed config by ID.
    /// GET /api/admin/feeds/{id}
    /// </summary>
    public virtual async Task<FeedConfig?> GetFeedConfigByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var feed = await _httpClient.GetFromJsonAsync<FeedConfig>(
                $"/api/admin/feeds/{id}",
                cancellationToken);
            return feed;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch feed config {FeedId}", id);
            throw;
        }
    }

    /// <summary>
    /// Create a new RSS feed config.
    /// POST /api/admin/feeds
    /// </summary>
    public virtual async Task<FeedConfig> CreateFeedConfigAsync(
        FeedConfig config,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await _httpClient.PostAsJsonAsync("/api/admin/feeds", config, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<FeedConfig>(cancellationToken)
                ?? throw new InvalidOperationException("API returned null for created feed config");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to create feed config");
            throw;
        }
    }

    /// <summary>
    /// Update an existing RSS feed config.
    /// PUT /api/admin/feeds/{id}
    /// </summary>
    public virtual async Task<FeedConfig> UpdateFeedConfigAsync(
        FeedConfig config,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(config);

        try
        {
            using var response = await _httpClient.PutAsJsonAsync($"/api/admin/feeds/{config.Id}", config, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<FeedConfig>(cancellationToken)
                ?? throw new InvalidOperationException("API returned null for updated feed config");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to update feed config {FeedId}", config.Id);
            throw;
        }
    }

    /// <summary>
    /// Delete an RSS feed config.
    /// DELETE /api/admin/feeds/{id}
    /// </summary>
    public virtual async Task DeleteFeedConfigAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await _httpClient.DeleteAsync($"/api/admin/feeds/{id}", cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to delete feed config {FeedId}", id);
            throw;
        }
    }

    // ================================================================
    // Database statistics methods
    // ================================================================

    /// <summary>
    /// Get database statistics for the admin dashboard.
    /// GET /api/admin/statistics
    /// </summary>
    public virtual async Task<DatabaseStatistics?> GetDatabaseStatisticsAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Fetching database statistics");
            return await _httpClient.GetFromJsonAsync<DatabaseStatistics>(
                "/api/admin/statistics",
                cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch database statistics");
            throw;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "Failed to fetch database statistics (timeout or unexpected error)");
            throw;
        }
    }

    // ================================================================
    // Processed URLs methods
    // ================================================================

    /// <summary>
    /// Get a paginated list of processed URLs with optional filters.
    /// GET /api/admin/processed-urls
    /// </summary>
    public virtual async Task<PagedResult<ProcessedUrlListItem>> GetProcessedUrlsAsync(
        int page = 1,
        int pageSize = 100,
        string? status = null,
        string? search = null,
        string? feedName = null,
        string? collectionName = null,
        long? jobId = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = $"/api/admin/processed-urls?page={page}&pageSize={pageSize}";
            if (!string.IsNullOrEmpty(status))
            {
                query += $"&status={Uri.EscapeDataString(status)}";
            }

            if (!string.IsNullOrEmpty(search))
            {
                query += $"&search={Uri.EscapeDataString(search)}";
            }

            if (!string.IsNullOrEmpty(feedName))
            {
                query += $"&feedName={Uri.EscapeDataString(feedName)}";
            }

            if (!string.IsNullOrEmpty(collectionName))
            {
                query += $"&collectionName={Uri.EscapeDataString(collectionName)}";
            }

            if (jobId.HasValue)
            {
                query += $"&jobId={jobId.Value}";
            }

            var result = await _httpClient.GetFromJsonAsync<PagedResult<ProcessedUrlListItem>>(
                query, cancellationToken);
            return result ?? new PagedResult<ProcessedUrlListItem> { Items = [], TotalCount = 0 };
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch processed URLs");
            throw;
        }
    }

    /// <summary>
    /// Delete a specific processed URL so it can be retried.
    /// DELETE /api/admin/processed-urls?url={url}
    /// </summary>
    public virtual async Task<bool> DeleteProcessedUrlAsync(
        string url,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await _httpClient.DeleteAsync(
                $"/api/admin/processed-urls?url={Uri.EscapeDataString(url)}", cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to delete processed URL");
            throw;
        }
    }

    /// <summary>
    /// Delete all failed processed URL records.
    /// DELETE /api/admin/processed-urls/failed
    /// </summary>
    public virtual async Task<int> DeleteAllFailedProcessedUrlsAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await _httpClient.DeleteAsync(
                "/api/admin/processed-urls/failed", cancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<DeletedCountResponse>(cancellationToken);
            return result?.Deleted ?? 0;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to delete all failed processed URLs");
            throw;
        }
    }

    private sealed class DeletedCountResponse
    {
        public int Deleted { get; init; }
    }

    // ================================================================
    // Admin – Custom page data methods
    // ================================================================

    /// <summary>
    /// List all custom page entries.
    /// GET /api/admin/custom-pages
    /// </summary>
    public virtual async Task<IReadOnlyList<CustomPageEntry>> GetCustomPageEntriesAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var entries = await _httpClient.GetFromJsonAsync<IReadOnlyList<CustomPageEntry>>(
                "/api/admin/custom-pages",
                cancellationToken);
            return entries ?? [];
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch custom page entries");
            throw;
        }
    }

    /// <summary>
    /// Get a single custom page entry with full JSON.
    /// GET /api/admin/custom-pages/{key}
    /// </summary>
    public virtual async Task<CustomPageEntry?> GetCustomPageEntryAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<CustomPageEntry>(
                $"/api/admin/custom-pages/{Uri.EscapeDataString(key)}",
                cancellationToken);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch custom page entry {Key}", key);
            throw;
        }
    }

    /// <summary>
    /// Update the raw JSON for a custom page.
    /// PUT /api/admin/custom-pages/{key}
    /// </summary>
    public virtual async Task<CustomPageEntry> UpdateCustomPageAsync(
        string key,
        string jsonData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await _httpClient.PutAsJsonAsync(
                $"/api/admin/custom-pages/{Uri.EscapeDataString(key)}",
                new { JsonData = jsonData },
                cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CustomPageEntry>(cancellationToken)
                ?? throw new InvalidOperationException("API returned null for updated custom page entry");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to update custom page {Key}", key);
            throw;
        }
    }

    // ================================================================
    // Admin – Content item ai_metadata methods
    // ================================================================

    /// <summary>
    /// Get the ai_metadata JSON for a content item by primary key.
    /// GET /api/admin/content-items/ai-metadata?collection={collection}&amp;slug={slug}
    /// </summary>
    public virtual async Task<ContentItemAiMetadataResult?> GetContentItemAiMetadataAsync(
        string collectionName,
        string slug,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<ContentItemAiMetadataResult>(
                $"/api/admin/content-items/ai-metadata?collection={Uri.EscapeDataString(collectionName)}&slug={Uri.EscapeDataString(slug)}",
                cancellationToken);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch ai_metadata for {Collection}/{Slug}", collectionName.Sanitize(), slug.Sanitize());
            throw;
        }
    }

    /// <summary>
    /// Update the ai_metadata JSON for a content item by primary key.
    /// PUT /api/admin/content-items/ai-metadata?collection={collection}&amp;slug={slug}
    /// </summary>
    public virtual async Task UpdateContentItemAiMetadataAsync(
        string collectionName,
        string slug,
        string aiMetadata,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await _httpClient.PutAsJsonAsync(
                $"/api/admin/content-items/ai-metadata?collection={Uri.EscapeDataString(collectionName)}&slug={Uri.EscapeDataString(slug)}",
                new { AiMetadata = aiMetadata },
                cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to update ai_metadata for {Collection}/{Slug}", collectionName.Sanitize(), slug.Sanitize());
            throw;
        }
    }

    // ================================================================
    // Admin – Content item editing methods
    // ================================================================

    /// <summary>
    /// Get all editable fields for a content item by primary key.
    /// GET /api/admin/content-items/edit-data?collection={collection}&amp;slug={slug}
    /// </summary>
    public virtual async Task<ContentItemEditData?> GetContentItemEditDataAsync(
        string collectionName,
        string slug,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<ContentItemEditData>(
                $"/api/admin/content-items/edit-data?collection={Uri.EscapeDataString(collectionName)}&slug={Uri.EscapeDataString(slug)}",
                cancellationToken);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch edit data for {Collection}/{Slug}", collectionName.Sanitize(), slug.Sanitize());
            throw;
        }
    }

    /// <summary>
    /// Update all editable fields for a content item by primary key.
    /// PUT /api/admin/content-items/edit-data?collection={collection}&amp;slug={slug}
    /// </summary>
    public virtual async Task UpdateContentItemEditDataAsync(
        string collectionName,
        string slug,
        ContentItemEditData editData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await _httpClient.PutAsJsonAsync(
                $"/api/admin/content-items/edit-data?collection={Uri.EscapeDataString(collectionName)}&slug={Uri.EscapeDataString(slug)}",
                editData,
                cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to update edit data for {Collection}/{Slug}", collectionName.Sanitize(), slug.Sanitize());
            throw;
        }
    }

    // ================================================================
    // Background job settings methods
    // ================================================================

    /// <summary>
    /// Get all background job settings.
    /// GET /api/admin/job-settings
    /// </summary>
    public virtual async Task<IReadOnlyList<BackgroundJobSetting>> GetJobSettingsAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var settings = await _httpClient.GetFromJsonAsync<IReadOnlyList<BackgroundJobSetting>>(
                "/api/admin/job-settings",
                cancellationToken);
            return settings ?? [];
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch job settings");
            throw;
        }
    }

    /// <summary>
    /// Update the enabled state for a background job.
    /// PUT /api/admin/job-settings/{jobName}
    /// </summary>
    public virtual async Task UpdateJobSettingAsync(
        string jobName,
        bool enabled,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await _httpClient.PutAsJsonAsync(
                $"/api/admin/job-settings/{Uri.EscapeDataString(jobName)}",
                new { Enabled = enabled },
                cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to update job setting {JobName}", jobName.Sanitize());
            throw;
        }
    }

    // ================================================================
    // Cache management methods
    // ================================================================

    /// <summary>
    /// Invalidate all server-side caches.
    /// POST /api/admin/cache/invalidate
    /// </summary>
    public virtual async Task InvalidateCachesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Invalidating all server-side caches");
            using var response = await _httpClient.PostAsync("/api/admin/cache/invalidate", null, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to invalidate caches");
            throw;
        }
    }

    // ================================================================
    // Content review methods
    // ================================================================

    /// <summary>
    /// Get content reviews filtered by status.
    /// GET /api/admin/reviews
    /// </summary>
    public virtual async Task<IReadOnlyList<ContentReview>> GetContentReviewsAsync(
        string? status = null,
        int limit = 100,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = $"/api/admin/reviews?limit={limit}";
            if (!string.IsNullOrEmpty(status))
            {
                query += $"&status={Uri.EscapeDataString(status)}";
            }

            var reviews = await _httpClient.GetFromJsonAsync<IReadOnlyList<ContentReview>>(
                query, cancellationToken);
            return reviews ?? [];
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch content reviews");
            throw;
        }
    }

    /// <summary>
    /// Get review summary counts.
    /// GET /api/admin/reviews/summary
    /// </summary>
    public virtual async Task<ContentReviewSummary> GetContentReviewSummaryAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var summary = await _httpClient.GetFromJsonAsync<ContentReviewSummary>(
                "/api/admin/reviews/summary", cancellationToken);
            return summary ?? new ContentReviewSummary();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch review summary");
            throw;
        }
    }

    /// <summary>
    /// Approve a single content review.
    /// POST /api/admin/reviews/{id}/approve
    /// </summary>
    public virtual async Task<bool> ApproveContentReviewAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await _httpClient.PostAsync(
                $"/api/admin/reviews/{id}/approve", null, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to approve review {Id}", id);
            throw;
        }
    }

    /// <summary>
    /// Reject a single content review.
    /// POST /api/admin/reviews/{id}/reject
    /// </summary>
    public virtual async Task<bool> RejectContentReviewAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await _httpClient.PostAsync(
                $"/api/admin/reviews/{id}/reject", null, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to reject review {Id}", id);
            throw;
        }
    }

    /// <summary>
    /// Approve all pending content reviews.
    /// POST /api/admin/reviews/approve-all
    /// </summary>
    public virtual async Task<int> ApproveAllContentReviewsAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await _httpClient.PostAsync(
                "/api/admin/reviews/approve-all", null, cancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApprovedCountResponse>(cancellationToken);
            return result?.Approved ?? 0;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to approve all reviews");
            throw;
        }
    }

    private sealed class ApprovedCountResponse
    {
        public int Approved { get; init; }
    }

    /// <summary>
    /// Reject all pending content reviews.
    /// POST /api/admin/reviews/reject-all
    /// </summary>
    public virtual async Task<int> RejectAllContentReviewsAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await _httpClient.PostAsync(
                "/api/admin/reviews/reject-all", null, cancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<RejectedCountResponse>(cancellationToken);
            return result?.Rejected ?? 0;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to reject all reviews");
            throw;
        }
    }

    private sealed class RejectedCountResponse
    {
        public int Rejected { get; init; }
    }

    /// <summary>
    /// Update the fixed value of a pending review.
    /// PUT /api/admin/reviews/{id}
    /// </summary>
    public virtual async Task<bool> UpdateContentReviewFixedValueAsync(
        long id,
        string fixedValue,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await _httpClient.PutAsJsonAsync(
                $"/api/admin/reviews/{id}",
                new { FixedValue = fixedValue },
                cancellationToken);

            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to update review {ReviewId}", id);
            throw;
        }
    }

    // ================================================================
    // Content preview methods
    // ================================================================

    /// <summary>
    /// Render markdown to HTML for preview.
    /// POST /api/admin/content-items/preview-markdown
    /// </summary>
    public virtual async Task<string> PreviewMarkdownAsync(
        string markdown,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await _httpClient.PostAsJsonAsync(
                "/api/admin/content-items/preview-markdown",
                new { Markdown = markdown },
                cancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<MarkdownPreviewResponse>(cancellationToken);
            return result?.Html ?? string.Empty;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to preview markdown");
            throw;
        }
    }

    private sealed class MarkdownPreviewResponse
    {
        public string? Html { get; init; }
    }
}
