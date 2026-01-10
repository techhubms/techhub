using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TechHub.Core.Interfaces;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// Background service that eagerly loads all data at application startup.
/// Runs asynchronously without blocking server startup, ensuring fast initial response.
/// </summary>
public class DataCacheWarmer(
    ILogger<DataCacheWarmer> logger,
    ISectionRepository sectionRepository,
    IContentRepository contentRepository) : BackgroundService
{
    private readonly ILogger<DataCacheWarmer> _logger = logger;
    private readonly ISectionRepository _sectionRepository = sectionRepository;
    private readonly IContentRepository _contentRepository = contentRepository;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting data cache warm-up in background...");

        var startTime = DateTimeOffset.UtcNow;

        try
        {
            // Load all data into memory by calling GetAllAsync on both repositories
            // This triggers the lazy loading and populates the cache
            var sections = await _sectionRepository.GetAllAsync(stoppingToken);
            _logger.LogInformation("Loaded {SectionCount} sections from configuration", sections.Count);

            var content = await _contentRepository.GetAllAsync(stoppingToken);
            _logger.LogInformation("Loaded {ContentCount} content items from collections", content.Count);

            var elapsed = DateTimeOffset.UtcNow - startTime;
            _logger.LogInformation("Data cache warmed successfully in {ElapsedMs}ms", elapsed.TotalMilliseconds);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to warm data cache - will retry on first request");
            // Don't throw - allow server to start even if cache warming fails
            // The repositories will lazy-load on first request
        }
    }
}
