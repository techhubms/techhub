using TechHub.Core.Models;

namespace TechHub.Web.Services;

/// <summary>
/// In-memory cache of sections loaded at application startup.
/// Provides synchronous access to section data for immediate rendering without flicker.
/// Periodically refreshes from the API to stay current after deployments.
/// </summary>
public class SectionCache
{
    private Dictionary<string, Section> _sectionsByName = new();
    // sectionName → set of collection names (both O(1), built once at Initialize)
    private Dictionary<string, HashSet<string>> _collectionsBySection = new();

    public IReadOnlyList<Section> Sections { get; private set; } = Array.Empty<Section>();

    /// <summary>
    /// True once the cache has been populated from the API. False only if the API was
    /// completely unavailable at startup, in which case callers should not treat unknown
    /// paths as invalid.
    /// </summary>
    public bool IsReady => Sections.Count > 0;

    public void Initialize(IReadOnlyList<Section> sections)
    {
        // Build lookup dictionaries before publishing Sections so that any reader
        // that observes IsReady=true also sees the fully populated dictionaries.
        var byName = sections.ToDictionary(s => s.Name, StringComparer.OrdinalIgnoreCase);
        var bySection = sections.ToDictionary(
            s => s.Name,
            s => new HashSet<string>(s.Collections.Select(c => c.Name), StringComparer.OrdinalIgnoreCase),
            StringComparer.OrdinalIgnoreCase);

        _sectionsByName = byName;
        _collectionsBySection = bySection;
        Sections = sections; // set last — IsReady checks Sections.Count
    }

    /// <summary>
    /// Get a section by name from the cache. Case-insensitive lookup.
    /// </summary>
    public Section? GetSectionByName(string sectionName)
    {
        _sectionsByName.TryGetValue(sectionName, out var section);
        return section;
    }

    /// <summary>
    /// Returns true if <paramref name="collectionName"/> is a known collection within
    /// <paramref name="sectionName"/>. Both lookups are O(1). Returns false if either
    /// name is unknown, including when the cache is not yet ready.
    /// </summary>
    public bool IsKnownCollection(string sectionName, string collectionName)
    {
        return _collectionsBySection.TryGetValue(sectionName, out var collections)
            && collections.Contains(collectionName);
    }
}

/// <summary>
/// Background service that periodically refreshes the SectionCache from the API.
/// Prevents stale navigation data when the API is redeployed with new configuration.
/// </summary>
public class SectionCacheRefreshService : BackgroundService
{
    private static readonly TimeSpan _refreshInterval = TimeSpan.FromMinutes(5);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly SectionCache _sectionCache;
    private readonly ILogger<SectionCacheRefreshService> _logger;

    public SectionCacheRefreshService(
        IServiceScopeFactory scopeFactory,
        SectionCache sectionCache,
        ILogger<SectionCacheRefreshService> logger)
    {
        _scopeFactory = scopeFactory;
        _sectionCache = sectionCache;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_refreshInterval, stoppingToken);

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var apiClient = scope.ServiceProvider.GetRequiredService<TechHubApiClient>();
                var sections = await apiClient.GetAllSectionsAsync(stoppingToken);

                if (sections != null)
                {
                    var sectionList = sections.ToList();
                    if (sectionList.Count > 0)
                    {
                        _sectionCache.Initialize(sectionList);
                        _logger.LogDebug("SectionCache refreshed with {Count} sections", sectionList.Count);
                    }
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // Normal shutdown
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "Failed to refresh SectionCache, will retry in {Interval}", _refreshInterval);
            }
            catch (TaskCanceledException ex) when (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogWarning(ex, "SectionCache refresh timed out, will retry in {Interval}", _refreshInterval);
            }
        }
    }
}
