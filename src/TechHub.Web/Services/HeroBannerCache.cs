using System.Text.Json;
using TechHub.Core.Models;

namespace TechHub.Web.Services;

/// <summary>
/// In-memory cache of hero banner data loaded at application startup.
/// Provides synchronous access to banner data so every section page renders
/// without an extra API round-trip per request.
/// Periodically refreshed and also invalidated by the admin cache-invalidation button.
/// </summary>
public class HeroBannerCache
{
    public HeroBannerData? Data { get; private set; }

    public bool IsInitialized { get; private set; }

    public void Initialize(HeroBannerData? data)
    {
        Data = data;
        IsInitialized = true;
    }

    public void Invalidate()
    {
        Data = null;
        IsInitialized = false;
    }
}

/// <summary>
/// Background service that periodically refreshes the HeroBannerCache from the API.
/// </summary>
public class HeroBannerCacheRefreshService : BackgroundService
{
    private static readonly TimeSpan _refreshInterval = TimeSpan.FromMinutes(5);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly HeroBannerCache _heroBannerCache;
    private readonly ILogger<HeroBannerCacheRefreshService> _logger;

    public HeroBannerCacheRefreshService(
        IServiceScopeFactory scopeFactory,
        HeroBannerCache heroBannerCache,
        ILogger<HeroBannerCacheRefreshService> logger)
    {
        _scopeFactory = scopeFactory;
        _heroBannerCache = heroBannerCache;
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
                var data = await apiClient.GetHeroBannerDataAsync(stoppingToken);
                _heroBannerCache.Initialize(data);
                _logger.LogDebug("HeroBannerCache refreshed");
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // Normal shutdown
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "Failed to refresh HeroBannerCache, will retry in {Interval}", _refreshInterval);
            }
            catch (TaskCanceledException ex) when (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogWarning(ex, "HeroBannerCache refresh timed out, will retry in {Interval}", _refreshInterval);
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex, "HeroBannerCache refresh received malformed response, will retry in {Interval}", _refreshInterval);
            }
        }
    }
}
