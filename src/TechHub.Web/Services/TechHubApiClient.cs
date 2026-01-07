using System.Net.Http.Json;
using TechHub.Core.DTOs;

namespace TechHub.Web.Services;

/// <summary>
/// Typed HTTP client for calling Tech Hub API endpoints.
/// Public to allow mocking in unit tests (virtual methods require public class for Moq proxies).
/// </summary>
#pragma warning disable CA1515 // Public type in non-public assembly - required for unit test mocking
public class TechHubApiClient
#pragma warning restore CA1515
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TechHubApiClient> _logger;

    public TechHubApiClient(HttpClient httpClient, ILogger<TechHubApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

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
            var section = await _httpClient.GetFromJsonAsync<SectionDto>(
                $"/api/sections/{sectionName}",
                cancellationToken);
            
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
            _logger.LogInformation("Fetching {Collection} items for section: {SectionName}", 
                collectionName, sectionName);
            var items = await _httpClient.GetFromJsonAsync<IEnumerable<ContentItemDto>>(
                $"/api/sections/{sectionName}/collections/{collectionName}/items",
                cancellationToken);
            
            _logger.LogInformation("Successfully fetched {Count} {Collection} items for section {SectionName}", 
                items?.Count() ?? 0, collectionName, sectionName);
            return items;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch {Collection} items for section {SectionName}", 
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
            var tags = await _httpClient.GetFromJsonAsync<IEnumerable<string>>(
                "/api/content/tags",
                cancellationToken);
            
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
    /// Get content items by category and collection.
    /// Pass null for category to get all items in the collection regardless of category.
    /// </summary>
    public virtual async Task<IEnumerable<ContentItemDto>?> GetContentAsync(
        string? category,
        string collection,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching content for category: {Category}, collection: {Collection}", 
                category ?? "(all)", collection);
            
            var url = string.IsNullOrWhiteSpace(category)
                ? $"/api/content?collectionName={Uri.EscapeDataString(collection)}"
                : $"/api/content?category={Uri.EscapeDataString(category)}&collectionName={Uri.EscapeDataString(collection)}";
            
            var items = await _httpClient.GetFromJsonAsync<IEnumerable<ContentItemDto>>(
                url,
                cancellationToken);
            
            _logger.LogInformation("Successfully fetched {Count} items", items?.Count() ?? 0);
            return items;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch content for category {Category}, collection {Collection}", 
                category ?? "(all)", collection);
            throw;
        }
    }

    /// <summary>
    /// Get detailed content item by section, collection, and item ID
    /// </summary>
    public virtual async Task<ContentItemDetailDto?> GetContentDetailAsync(
        string sectionName,
        string collection,
        string itemId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching content detail: {Section}/{Collection}/{ItemId}", 
                sectionName, collection, itemId);
            
            var item = await _httpClient.GetFromJsonAsync<ContentItemDetailDto>(
                $"/api/content/{sectionName}/{collection}/{itemId}",
                cancellationToken);
            
            return item;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch content detail for {Section}/{Collection}/{ItemId}", 
                sectionName, collection, itemId);
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
}
