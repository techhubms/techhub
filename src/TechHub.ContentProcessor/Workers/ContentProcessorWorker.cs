using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using TechHub.ContentProcessor.Models;
using TechHub.ContentProcessor.Options;
using TechHub.ContentProcessor.Services;

namespace TechHub.ContentProcessor.Workers;

/// <summary>
/// Background worker that runs the content processing pipeline on a configurable schedule.
/// On each tick it:
/// 1. Reads the RSS feed configuration.
/// 2. Downloads and parses each feed.
/// 3. Fetches full article content for new items.
/// 4. Categorizes each item via Azure OpenAI.
/// 5. Writes the result directly to the PostgreSQL database.
/// </summary>
public sealed class ContentProcessorWorker : BackgroundService
{
    private readonly RssFeedIngestionService _ingestionService;
    private readonly ArticleContentService _articleService;
    private readonly AiCategorizationService _aiService;
    private readonly ContentDatabaseWriter _dbWriter;
    private readonly ContentProcessorOptions _options;
    private readonly AiCategorizationOptions _aiOptions;
    private readonly ILogger<ContentProcessorWorker> _logger;

    private static readonly JsonSerializerOptions FeedJsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public ContentProcessorWorker(
        RssFeedIngestionService ingestionService,
        ArticleContentService articleService,
        AiCategorizationService aiService,
        ContentDatabaseWriter dbWriter,
        IOptions<ContentProcessorOptions> options,
        IOptions<AiCategorizationOptions> aiOptions,
        ILogger<ContentProcessorWorker> logger)
    {
        ArgumentNullException.ThrowIfNull(ingestionService);
        ArgumentNullException.ThrowIfNull(articleService);
        ArgumentNullException.ThrowIfNull(aiService);
        ArgumentNullException.ThrowIfNull(dbWriter);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(aiOptions);
        ArgumentNullException.ThrowIfNull(logger);

        _ingestionService = ingestionService;
        _articleService = articleService;
        _aiService = aiService;
        _dbWriter = dbWriter;
        _options = options.Value;
        _aiOptions = aiOptions.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_options.Enabled)
        {
            _logger.LogInformation("Content processor is disabled (ContentProcessor:Enabled = false). Worker will not run.");
            return;
        }

        _logger.LogInformation(
            "Content processor started. Interval: {Interval}, RSS config: {Path}",
            _options.Interval, _options.RssFeedsConfigPath);

        // Run immediately on startup, then on the configured interval
        await RunPipelineAsync(stoppingToken);

        using var timer = new PeriodicTimer(_options.Interval);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await timer.WaitForNextTickAsync(stoppingToken);
                await RunPipelineAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Graceful shutdown
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception in content processor pipeline. Will retry next interval.");
            }
        }

        _logger.LogInformation("Content processor stopped.");
    }

    private async Task RunPipelineAsync(CancellationToken ct)
    {
        _logger.LogInformation("Starting content processing run at {Time}", DateTimeOffset.UtcNow);

        var feedConfigs = LoadFeedConfigs();
        if (feedConfigs.Count == 0)
        {
            _logger.LogWarning("No RSS feed configurations found at {Path}", _options.RssFeedsConfigPath);
            return;
        }

        _logger.LogInformation("Processing {Count} RSS feed configurations", feedConfigs.Count);

        var totalNew = 0;
        var totalSkipped = 0;
        var totalErrors = 0;
        var totalItems = 0;

        foreach (var feedConfig in feedConfigs)
        {
            if (ct.IsCancellationRequested)
            {
                break;
            }

            try
            {
                var (newItems, skipped, errors) = await ProcessFeedAsync(feedConfig, ct);
                totalNew += newItems;
                totalSkipped += skipped;
                totalErrors += errors;
                totalItems += newItems + skipped + errors;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process feed: {FeedName}", feedConfig.Name);
                totalErrors++;
            }
        }

        _logger.LogInformation(
            "Content processing run complete. Total: {Total}, New: {New}, Skipped: {Skipped}, Errors: {Errors}",
            totalItems, totalNew, totalSkipped, totalErrors);
    }

    private async Task<(int newItems, int skipped, int errors)> ProcessFeedAsync(
        FeedConfig feedConfig,
        CancellationToken ct)
    {
        var rawItems = await _ingestionService.IngestAsync(feedConfig, ct);

        if (rawItems.Count == 0)
        {
            return (0, 0, 0);
        }

        var newItems = 0;
        var skipped = 0;
        var errors = 0;
        var itemsProcessed = 0;

        foreach (var rawItem in rawItems)
        {
            if (ct.IsCancellationRequested)
            {
                break;
            }

            // Check limit per run
            if (_options.MaxItemsPerRun > 0 && newItems >= _options.MaxItemsPerRun)
            {
                _logger.LogInformation("MaxItemsPerRun ({Max}) reached, stopping feed processing", _options.MaxItemsPerRun);
                break;
            }

            try
            {
                // Skip items already in the database
                if (await _dbWriter.ExistsAsync(rawItem.ExternalUrl, ct))
                {
                    skipped++;
                    continue;
                }

                // Enrich with full article content (skip YouTube items)
                var enrichedItem = await _articleService.EnrichWithContentAsync(rawItem, ct);

                // Apply request delay to prevent rate limiting on external sites
                if (_options.RequestDelayMs > 0 && !rawItem.IsYouTube)
                {
                    await Task.Delay(_options.RequestDelayMs, ct);
                }

                // AI categorization
                var processed = await _aiService.CategorizeAsync(enrichedItem, ct);
                if (processed == null)
                {
                    // AI decided to skip this item
                    skipped++;
                    continue;
                }

                // Write to database
                await _dbWriter.WriteAsync(processed, ct);
                newItems++;
                itemsProcessed++;

                // Delay between AI calls
                if (_aiOptions.Enabled && _aiOptions.RateLimitDelaySeconds > 0)
                {
                    await Task.Delay(TimeSpan.FromSeconds(_aiOptions.RateLimitDelaySeconds), ct);
                }
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing item: {Title} ({Url})", rawItem.Title, rawItem.ExternalUrl);
                errors++;
            }
        }

        _logger.LogInformation(
            "Feed {FeedName}: processed {Processed} items (new: {New}, skipped: {Skipped}, errors: {Errors})",
            feedConfig.Name, rawItems.Count, newItems, skipped, errors);

        return (newItems, skipped, errors);
    }

    private List<FeedConfig> LoadFeedConfigs()
    {
        var path = _options.RssFeedsConfigPath;

        if (!Path.IsPathRooted(path))
        {
            path = Path.Combine(AppContext.BaseDirectory, path);
        }

        if (!File.Exists(path))
        {
            _logger.LogError("RSS feeds configuration file not found: {Path}", path);
            return [];
        }

        try
        {
            var json = File.ReadAllText(path);
            var configs = JsonSerializer.Deserialize<List<FeedConfig>>(json, FeedJsonOptions);

            if (configs == null || configs.Count == 0)
            {
                return [];
            }

            // Normalise outputDir: strip leading underscore if present
            return configs
                .Select(c => new FeedConfig
                {
                    Name = c.Name,
                    OutputDir = c.OutputDir.TrimStart('_'),
                    Url = c.Url
                })
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load RSS feeds configuration from {Path}", path);
            return [];
        }
    }
}
