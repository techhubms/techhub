using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TechHub.Core.Interfaces;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// Background service that eagerly loads all data at application startup.
/// This ensures all repository data is cached in memory before first request.
/// </summary>
public class DataCacheWarmer : IHostedService
{
    private readonly ILogger<DataCacheWarmer> _logger;
    private readonly ISectionRepository _sectionRepository;
    private readonly IContentRepository _contentRepository;

    public DataCacheWarmer(
        ILogger<DataCacheWarmer> logger,
        ISectionRepository sectionRepository,
        IContentRepository contentRepository)
    {
        _logger = logger;
        _sectionRepository = sectionRepository;
        _contentRepository = contentRepository;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Warming data cache - loading all content from disk...");
        
        var startTime = DateTimeOffset.UtcNow;

        try
        {
            // Load all data into memory by calling GetAllAsync on both repositories
            // This triggers the lazy loading and populates the cache
            var sections = await _sectionRepository.GetAllAsync(cancellationToken);
            _logger.LogInformation("Loaded {SectionCount} sections from configuration", sections.Count);

            var content = await _contentRepository.GetAllAsync(cancellationToken);
            _logger.LogInformation("Loaded {ContentCount} content items from collections", content.Count);

            var elapsed = DateTimeOffset.UtcNow - startTime;
            _logger.LogInformation("Data cache warmed successfully in {ElapsedMs}ms", elapsed.TotalMilliseconds);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to warm data cache");
            throw; // Fail fast if cache warming fails
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // No cleanup needed
        return Task.CompletedTask;
    }
}
