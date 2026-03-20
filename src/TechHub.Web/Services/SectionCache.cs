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

    public IReadOnlyList<Section> Sections { get; private set; } = Array.Empty<Section>();

    public void Initialize(IReadOnlyList<Section> sections)
    {
        Sections = sections;
        _sectionsByName = sections.ToDictionary(s => s.Name, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Get a section by name from the cache. Case-insensitive lookup.
    /// </summary>
    /// <param name="sectionName">The section name to look up</param>
    /// <returns>The section if found, otherwise null</returns>
    public Section? GetSectionByName(string sectionName)
    {
        _sectionsByName.TryGetValue(sectionName, out var section);
        return section;
    }
}

/// <summary>
/// Background service that periodically refreshes the SectionCache from the API.
/// Prevents stale navigation data when the API is redeployed with new configuration.
/// </summary>
public class SectionCacheRefreshService : BackgroundService
{
    private static readonly TimeSpan RefreshInterval = TimeSpan.FromMinutes(5);

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
            await Task.Delay(RefreshInterval, stoppingToken);

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
                _logger.LogWarning(ex, "Failed to refresh SectionCache, will retry in {Interval}", RefreshInterval);
            }
            catch (TaskCanceledException ex) when (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogWarning(ex, "SectionCache refresh timed out, will retry in {Interval}", RefreshInterval);
            }
        }
    }
}
