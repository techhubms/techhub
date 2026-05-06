using TechHub.Core.Models;

namespace TechHub.Web.Services;

/// <summary>
/// In-memory cache of sections loaded at application startup.
/// Provides synchronous access to section data for immediate rendering without flicker.
/// Periodically refreshes from the API to stay current after deployments.
/// </summary>
public class SectionCache
{
    /// <summary>
    /// All cache data bundled into a single immutable record so it can be published
    /// with a single volatile reference swap. Readers get a consistent snapshot
    /// without any locking: they read <c>_data</c> once and use that snapshot throughout.
    /// </summary>
    private record CacheData(
        Dictionary<string, Section> ByName,
        Dictionary<string, HashSet<string>> BySection,
        HashSet<string> AllCollections,
        IReadOnlyList<Section> Sections);

    private static readonly CacheData _empty = new(
        new Dictionary<string, Section>(),
        new Dictionary<string, HashSet<string>>(),
        new HashSet<string>(),
        Array.Empty<Section>());

    private CacheData _data = _empty;

    public IReadOnlyList<Section> Sections => Volatile.Read(ref _data).Sections;

    /// <summary>
    /// True once the cache has been populated from the API. False only if the API was
    /// completely unavailable at startup, in which case callers should not treat unknown
    /// paths as invalid.
    /// </summary>
    public bool IsReady => Sections.Count > 0;

    public void Initialize(IReadOnlyList<Section> sections)
    {
        var byName = sections.ToDictionary(s => s.Name, StringComparer.OrdinalIgnoreCase);
        var bySection = sections.ToDictionary(
            s => s.Name,
            s => new HashSet<string>(s.Collections.Select(c => c.Name), StringComparer.OrdinalIgnoreCase),
            StringComparer.OrdinalIgnoreCase);

        var allCollections = new HashSet<string>(
            sections.SelectMany(s => s.Collections.Select(c => c.Name)),
            StringComparer.OrdinalIgnoreCase);

        // Single volatile write: any reader that picks up the new reference sees a fully
        // consistent snapshot — all three dictionaries and Sections are always in sync.
        Volatile.Write(ref _data, new CacheData(byName, bySection, allCollections, sections));
    }

    /// <summary>
    /// Get a section by name from the cache. Case-insensitive lookup.
    /// </summary>
    public Section? GetSectionByName(string sectionName)
    {
        Volatile.Read(ref _data).ByName.TryGetValue(sectionName, out var section);
        return section;
    }

    /// <summary>
    /// Returns true if <paramref name="collectionName"/> is a known collection within
    /// <paramref name="sectionName"/>. Both lookups are O(1). Returns false if either
    /// name is unknown, including when the cache is not yet ready.
    /// </summary>
    public bool IsKnownCollection(string sectionName, string collectionName)
    {
        var data = Volatile.Read(ref _data);
        return data.BySection.TryGetValue(sectionName, out var collections)
            && collections.Contains(collectionName);
    }

    /// <summary>
    /// Returns true if <paramref name="collectionName"/> is a known collection in ANY
    /// section. Used to validate sub-paths of virtual sections (e.g. /all/news, /all/videos).
    /// O(1) via the pre-built all-collections set.
    /// Returns false when the cache is not ready.
    /// </summary>
    public bool IsKnownCollectionInAnySection(string collectionName)
    {
        return Volatile.Read(ref _data).AllCollections.Contains(collectionName);
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
