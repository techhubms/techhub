using System.Text.Json;

namespace TechHub.Web.Services;

/// <summary>
/// Background service that periodically refreshes the <see cref="FeatureToggleCache"/> from the API.
/// A failed refresh keeps the previous toggles.
/// </summary>
public sealed class FeatureToggleCacheRefreshService : BackgroundService
{
    private static readonly TimeSpan _refreshInterval = TimeSpan.FromSeconds(30);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly FeatureToggleCache _cache;
    private readonly ILogger<FeatureToggleCacheRefreshService> _logger;

    public FeatureToggleCacheRefreshService(
        IServiceScopeFactory scopeFactory,
        FeatureToggleCache cache,
        ILogger<FeatureToggleCacheRefreshService> logger)
    {
        ArgumentNullException.ThrowIfNull(scopeFactory);
        ArgumentNullException.ThrowIfNull(cache);
        ArgumentNullException.ThrowIfNull(logger);

        _scopeFactory = scopeFactory;
        _cache = cache;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
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
            var apiClient = scope.ServiceProvider.GetRequiredService<ITechHubApiClient>();
            _cache.Initialize(await apiClient.GetFeatureTogglesAsync(stoppingToken));
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // Normal shutdown
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Failed to refresh the feature toggles, will retry in {Interval}", _refreshInterval);
        }
        catch (TaskCanceledException ex) when (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogWarning(ex, "Refreshing the feature toggles timed out, will retry in {Interval}", _refreshInterval);
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(ex, "Feature toggle refresh received malformed JSON, will retry in {Interval}", _refreshInterval);
        }
    }
}
