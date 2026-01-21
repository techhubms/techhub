using TechHub.Core.DTOs;

namespace TechHub.Web.Services;

/// <summary>
/// Typed HTTP client for calling Tech Hub API endpoints.
/// Public to allow mocking in unit tests (virtual methods require public class for Moq proxies).
/// </summary>
#pragma warning disable CA1515 // Public type in non-public assembly - required for unit test mocking
public class TechHubApiClient(HttpClient httpClient, ILogger<TechHubApiClient> logger) : ITechHubApiClient
#pragma warning restore CA1515
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<TechHubApiClient> _logger = logger;

    /// <summary>
    /// Get all sections with their collections
    /// </summary>
    public virtual async Task<IEnumerable<SectionDto>?> GetAllSectionsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching all sections from API");
            var sections = await _httpClient.GetFromJsonAsync<IEnumerable<SectionDto>>(
                "/api/sections",
                cancellationToken);

            _logger.LogInformation("Successfully fetched {Count} sections", sections?.Count() ?? 0);
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
    /// </summary>
    public virtual async Task<SectionDto?> GetSectionAsync(string sectionName, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching section: {SectionName}", sectionName);
            var response = await _httpClient.GetAsync($"/api/sections/{sectionName}", cancellationToken);

            // Return null for 404 (not found is a valid state)
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Section not found: {SectionName}", sectionName);
                return null;
            }

            response.EnsureSuccessStatusCode();
            var section = await response.Content.ReadFromJsonAsync<SectionDto>(cancellationToken: cancellationToken);
            return section;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch section {SectionName}", sectionName);
            throw;
        }
    }

    /// <summary>
    /// Get all items in a section
    /// </summary>
    public virtual async Task<IEnumerable<ContentItemDto>?> GetSectionItemsAsync(
        string sectionName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching items for section: {SectionName}", sectionName);
            var items = await _httpClient.GetFromJsonAsync<IEnumerable<ContentItemDto>>(
                $"/api/sections/{sectionName}/items",
                cancellationToken);

            _logger.LogInformation("Successfully fetched {Count} items for section {SectionName}",
                items?.Count() ?? 0, sectionName);
            return items;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch items for section {SectionName}", sectionName);
            throw;
        }
    }

    /// <summary>
    /// Get items in a specific collection within a section
    /// </summary>
    public virtual async Task<IEnumerable<ContentItemDto>?> GetSectionCollectionItemsAsync(
        string sectionName,
        string collectionName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching {CollectionName} items for section: {SectionName}",
                collectionName, sectionName);
            var items = await _httpClient.GetFromJsonAsync<IEnumerable<ContentItemDto>>(
                $"/api/sections/{sectionName}/collections/{collectionName}/items",
                cancellationToken);

            _logger.LogInformation("Successfully fetched {Count} {CollectionName} items for section {SectionName}",
                items?.Count() ?? 0, collectionName, sectionName);
            return items;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch {CollectionName} items for section {SectionName}",
                collectionName, sectionName);
            throw;
        }
    }

    /// <summary>
    /// Filter content by multiple criteria
    /// </summary>
    public virtual async Task<IEnumerable<ContentItemDto>?> FilterContentAsync(
        string? sections = null,
        string? collections = null,
        string? tags = null,
        string? searchQuery = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var queryParams = new List<string>();
            if (!string.IsNullOrWhiteSpace(sections))
                queryParams.Add($"sections={Uri.EscapeDataString(sections)}");
            if (!string.IsNullOrWhiteSpace(collections))
                queryParams.Add($"collections={Uri.EscapeDataString(collections)}");
            if (!string.IsNullOrWhiteSpace(tags))
                queryParams.Add($"tags={Uri.EscapeDataString(tags)}");
            if (!string.IsNullOrWhiteSpace(searchQuery))
                queryParams.Add($"q={Uri.EscapeDataString(searchQuery)}");

            var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
            var url = $"/api/content/filter{queryString}";

            _logger.LogInformation("Filtering content with query: {QueryString}", queryString);
            var items = await _httpClient.GetFromJsonAsync<IEnumerable<ContentItemDto>>(url, cancellationToken);

            _logger.LogInformation("Filter returned {Count} items", items?.Count() ?? 0);
            return items;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to filter content");
            throw;
        }
    }

    /// <summary>
    /// Get all unique tags across all content
    /// </summary>
    public virtual async Task<IEnumerable<string>?> GetAllTagsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching all tags from API");
            var response = await _httpClient.GetFromJsonAsync<AllTagsResponse>(
                "/api/tags/all",
                cancellationToken);

            var tags = response?.Tags.Select(t => t.Tag).AsEnumerable();
            _logger.LogInformation("Successfully fetched {Count} tags", tags?.Count() ?? 0);
            return tags;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch tags from API");
            throw;
        }
    }

    /// <summary>
    /// Get content items by section and collection.
    /// Pass null for section to get all items in the collection regardless of section.
    /// </summary>
    public virtual async Task<IEnumerable<ContentItemDto>?> GetContentAsync(
        string? sectionName,
        string collectionName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching content for section: {SectionName}, collection: {CollectionName}",
                sectionName ?? "(all)", collectionName);

            var url = string.IsNullOrWhiteSpace(sectionName)
                ? $"/api/content?collectionName={Uri.EscapeDataString(collectionName)}"
                : $"/api/content?sectionName={Uri.EscapeDataString(sectionName)}&collectionName={Uri.EscapeDataString(collectionName)}";

            var items = await _httpClient.GetFromJsonAsync<IEnumerable<ContentItemDto>>(
                url,
                cancellationToken);

            _logger.LogInformation("Successfully fetched {Count} items", items?.Count() ?? 0);
            return items;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch content for section {SectionName}, collection {CollectionName}",
                sectionName ?? "(all)", collectionName);
            throw;
        }
    }

    /// <summary>
    /// Get detailed content item by sectionName, collectionName, and slug
    /// </summary>
    public virtual async Task<ContentItemDetailDto?> GetContentDetailAsync(
        string sectionName,
        string collectionName,
        string slug,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching content detail: {SectionName}/{CollectionName}/{Slug}",
                sectionName, collectionName, slug);

            var response = await _httpClient.GetAsync(
                $"/api/content/{sectionName}/{collectionName}/{slug}",
                cancellationToken);

            // Return null for 404 (not found is a valid state)
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Content not found: {SectionName}/{CollectionName}/{Slug}",
                    sectionName, collectionName, slug);
                return null;
            }

            response.EnsureSuccessStatusCode();
            var item = await response.Content.ReadFromJsonAsync<ContentItemDetailDto>(cancellationToken: cancellationToken);
            return item;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch content detail for {SectionName}/{CollectionName}/{Slug}",
                sectionName, collectionName, slug);
            throw;
        }
    }

    /// <summary>
    /// Get the latest items across all sections (for homepage sidebar)
    /// </summary>
    public virtual async Task<IEnumerable<ContentItemDto>?> GetLatestItemsAsync(
        int count = 10,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching latest {Count} items", count);

            // Use filter endpoint without filters to get all items
            var allItems = await FilterContentAsync(cancellationToken: cancellationToken);

            // Sort by published date descending and take the requested count
            var latestItems = allItems?
                .OrderByDescending(i => i.DateEpoch)
                .Take(count);

            _logger.LogInformation("Successfully fetched {Count} latest items", latestItems?.Count() ?? 0);
            return latestItems;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch latest items");
            throw;
        }
    }

    /// <summary>
    /// Get popular tags (tags that appear most frequently across all content)
    /// </summary>
    public virtual async Task<IEnumerable<string>?> GetPopularTagsAsync(
        int count = 15,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching popular tags (top {Count})", count);

            // Get all tags
            var allTags = await GetAllTagsAsync(cancellationToken);

            // For now, return all tags sorted alphabetically
            // In the future, this could be enhanced to track tag frequency
            var popularTags = allTags?
                .OrderBy(t => t)
                .Take(count);

            _logger.LogInformation("Successfully fetched {Count} popular tags", popularTags?.Count() ?? 0);
            return popularTags;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch popular tags");
            throw;
        }
    }

    /// <summary>
    /// Get RSS feed for all content
    /// </summary>
    public virtual async Task<string> GetAllContentRssFeedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching RSS feed for all content");
            var response = await _httpClient.GetAsync(new Uri("/api/rss/all", UriKind.Relative), cancellationToken);
            response.EnsureSuccessStatusCode();

            var xml = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogInformation("Successfully fetched RSS feed for all content");
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
            _logger.LogInformation("Fetching RSS feed for section: {SectionName}", sectionName);
            var response = await _httpClient.GetAsync(new Uri($"/api/rss/{sectionName}", UriKind.Relative), cancellationToken);
            response.EnsureSuccessStatusCode();

            var xml = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogInformation("Successfully fetched RSS feed for section: {SectionName}", sectionName);
            return xml;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch RSS feed for section {SectionName}", sectionName);
            throw;
        }
    }

    /// <summary>
    /// Get RSS feed for a specific collection
    /// </summary>
    public virtual async Task<string> GetCollectionRssFeedAsync(string collectionName, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching RSS feed for collection: {CollectionName}", collectionName);
            var response = await _httpClient.GetAsync(new Uri($"/api/rss/collection/{collectionName}", UriKind.Relative), cancellationToken);
            response.EnsureSuccessStatusCode();

            var xml = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogInformation("Successfully fetched RSS feed for collection: {CollectionName}", collectionName);
            return xml;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch RSS feed for collection {CollectionName}", collectionName);
            throw;
        }
    }

    /// <summary>
    /// Get DX Space page data
    /// </summary>
    public virtual async Task<DXSpacePageData?> GetDXSpaceDataAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching DX Space page data");
            var response = await _httpClient.GetAsync("/api/custom-pages/dx-space", cancellationToken);

            // Return null for 404 (not found is a valid state)
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("DX Space data not found");
                return null;
            }

            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<DXSpacePageData>(cancellationToken: cancellationToken);
            return data;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch DX Space data");
            throw;
        }
    }

    /// <summary>
    /// Get Handbook page data
    /// </summary>
    public virtual async Task<HandbookPageData?> GetHandbookDataAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching Handbook page data");
            var response = await _httpClient.GetAsync("/api/custom-pages/handbook", cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Handbook data not found");
                return null;
            }

            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<HandbookPageData>(cancellationToken: cancellationToken);
            return data;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch Handbook data");
            throw;
        }
    }

    /// <summary>
    /// Get Levels of Enlightenment page data
    /// </summary>
    public virtual async Task<LevelsPageData?> GetLevelsDataAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching Levels page data");
            var response = await _httpClient.GetAsync("/api/custom-pages/levels", cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Levels data not found");
                return null;
            }

            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<LevelsPageData>(cancellationToken: cancellationToken);
            return data;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch Levels data");
            throw;
        }
    }

    /// <summary>
    /// Get VS Code Updates page data
    /// </summary>
    public virtual async Task<VSCodeUpdatesPageData?> GetVSCodeUpdatesDataAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching VS Code Updates page data");
            var response = await _httpClient.GetAsync("/api/custom-pages/vscode-updates", cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("VS Code Updates data not found");
                return null;
            }

            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<VSCodeUpdatesPageData>(cancellationToken: cancellationToken);
            return data;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch VS Code Updates data");
            throw;
        }
    }

    /// <summary>
    /// Get Features page data
    /// </summary>
    public virtual async Task<FeaturesPageData?> GetFeaturesDataAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching Features page data");
            var response = await _httpClient.GetAsync("/api/custom-pages/features", cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Features data not found");
                return null;
            }

            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<FeaturesPageData>(cancellationToken: cancellationToken);
            return data;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch Features data");
            throw;
        }
    }

    /// <summary>
    /// Get GenAI Basics page data
    /// </summary>
    public virtual async Task<GenAIBasicsPageData?> GetGenAIBasicsDataAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching GenAI Basics page data");
            var response = await _httpClient.GetAsync("/api/custom-pages/genai-basics", cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("GenAI Basics data not found");
                return null;
            }

            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<GenAIBasicsPageData>(cancellationToken: cancellationToken);
            return data;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch GenAI Basics data");
            throw;
        }
    }

    /// <summary>
    /// Get GenAI Advanced page data
    /// </summary>
    public virtual async Task<GenAIAdvancedPageData?> GetGenAIAdvancedDataAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching GenAI Advanced page data");
            var response = await _httpClient.GetAsync("/api/custom-pages/genai-advanced", cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("GenAI Advanced data not found");
                return null;
            }

            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<GenAIAdvancedPageData>(cancellationToken: cancellationToken);
            return data;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch GenAI Advanced data");
            throw;
        }
    }

    /// <summary>
    /// Get GenAI Applied page data
    /// </summary>
    public virtual async Task<GenAIAppliedPageData?> GetGenAIAppliedDataAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching GenAI Applied page data");
            var response = await _httpClient.GetAsync("/api/custom-pages/genai-applied", cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("GenAI Applied data not found");
                return null;
            }

            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<GenAIAppliedPageData>(cancellationToken: cancellationToken);
            return data;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch GenAI Applied data");
            throw;
        }
    }

    /// <summary>
    /// Get SDLC page data
    /// </summary>
    public virtual async Task<SDLCPageData?> GetSDLCDataAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching SDLC page data");
            var response = await _httpClient.GetAsync("/api/custom-pages/sdlc", cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("SDLC data not found");
                return null;
            }

            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<SDLCPageData>(cancellationToken: cancellationToken);
            return data;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch SDLC data");
            throw;
        }
    }

    /// <summary>
    /// Get tag cloud for specified scope (Homepage, Section, Collection, or Content)
    /// </summary>
    /// <param name="scope">Scope for tag cloud (Homepage/Section/Collection/Content)</param>
    /// <param name="sectionName">Section name (required for Section/Collection/Content scopes)</param>
    /// <param name="collectionName">Collection name (required for Collection scope)</param>
    /// <param name="slug">Content item slug (required for Content scope)</param>
    /// <param name="maxTags">Maximum number of tags to return (default: 20)</param>
    /// <param name="minUses">Minimum usage count for tag inclusion (default: 1)</param>
    /// <param name="lastDays">Only include tags from content published within this many days (default: 90)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public virtual async Task<IReadOnlyList<TagCloudItem>?> GetTagCloudAsync(
        TagCloudScope scope,
        string? sectionName = null,
        string? collectionName = null,
        string? slug = null,
        int? maxTags = null,
        int? minUses = null,
        int? lastDays = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var queryParams = new List<string>
            {
                $"scope={scope}"
            };

            if (!string.IsNullOrWhiteSpace(sectionName))
                queryParams.Add($"section={Uri.EscapeDataString(sectionName)}");
            if (!string.IsNullOrWhiteSpace(collectionName))
                queryParams.Add($"collection={Uri.EscapeDataString(collectionName)}");
            if (!string.IsNullOrWhiteSpace(slug))
                queryParams.Add($"contentId={Uri.EscapeDataString(slug)}");
            if (maxTags.HasValue)
                queryParams.Add($"maxTags={maxTags.Value}");
            if (minUses.HasValue)
                queryParams.Add($"minUses={minUses.Value}");
            if (lastDays.HasValue)
                queryParams.Add($"lastDays={lastDays.Value}");

            var queryString = string.Join("&", queryParams);
            var url = $"/api/tags/cloud?{queryString}";

            _logger.LogInformation("Fetching tag cloud for scope: {Scope}, section: {SectionName}, collection: {CollectionName}",
                scope, sectionName ?? "(none)", collectionName ?? "(none)");

            var tagCloud = await _httpClient.GetFromJsonAsync<IReadOnlyList<TagCloudItem>>(url, cancellationToken);

            _logger.LogInformation("Successfully fetched {Count} tags", tagCloud?.Count ?? 0);
            return tagCloud;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch tag cloud for scope {Scope}", scope);
            throw;
        }
    }
}
