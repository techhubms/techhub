using TechHub.Core.Models;

namespace TechHub.Web.Services;

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
        // Load immediately on startup so the cache is populated before traffic arrives.
        // The readiness health check (/health) depends on SectionCache.IsReady, so Container
        // Apps will not route traffic to this instance until this first load succeeds.
        await RefreshAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_refreshInterval, stoppingToken);
            await RefreshAsync(stoppingToken);
        }
    }

    private async Task RefreshAsync(CancellationToken stoppingToken)
    {
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
