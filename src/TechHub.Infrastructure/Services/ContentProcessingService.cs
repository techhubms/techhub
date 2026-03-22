using System.Data;
using System.Globalization;
using System.Text;
using System.Text.Json;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;

#pragma warning disable CA1031 // Catch-all intentional: errors must not stop pipeline processing

namespace TechHub.Infrastructure.Services;

/// <summary>
/// Orchestrates the full content processing pipeline:
/// <list type="number">
///   <item>Load RSS feed configuration.</item>
///   <item>Ingest RSS feeds into raw items.</item>
///   <item>Fetch full article content for non-YouTube items.</item>
///   <item>Categorize each item with Azure OpenAI.</item>
///   <item>Write new items directly to the database.</item>
///   <item>Record the run in <see cref="IContentProcessingJobRepository"/>.</item>
/// </list>
/// </summary>
public sealed class ContentProcessingService
{
    private readonly RssFeedIngestionService _rssService;
    private readonly ArticleContentService _articleService;
    private readonly AiCategorizationService _aiService;
    private readonly IDbConnection _connection;
    private readonly IContentProcessingJobRepository _jobRepo;
    private readonly ContentProcessorOptions _options;
    private readonly ILogger<ContentProcessingService> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public ContentProcessingService(
        RssFeedIngestionService rssService,
        ArticleContentService articleService,
        AiCategorizationService aiService,
        IDbConnection connection,
        IContentProcessingJobRepository jobRepo,
        IOptions<ContentProcessorOptions> options,
        ILogger<ContentProcessingService> logger)
    {
        ArgumentNullException.ThrowIfNull(rssService);
        ArgumentNullException.ThrowIfNull(articleService);
        ArgumentNullException.ThrowIfNull(aiService);
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(jobRepo);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _rssService = rssService;
        _articleService = articleService;
        _aiService = aiService;
        _connection = connection;
        _jobRepo = jobRepo;
        _options = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// Runs a complete content processing pipeline run.
    /// All errors are caught and recorded in the job log — the method never throws.
    /// </summary>
    public async Task RunAsync(string triggerType = "scheduled", CancellationToken ct = default)
    {
        var jobId = await _jobRepo.CreateAsync(triggerType, ct);
        var log = new StringBuilder();
        var startedAt = DateTime.UtcNow;
        var feedsProcessed = 0;
        var itemsAdded = 0;
        var itemsSkipped = 0;
        var errorCount = 0;

        void Log(string msg)
        {
            var line = string.Create(CultureInfo.InvariantCulture,
                $"[{DateTime.UtcNow:HH:mm:ss}] {msg}");
            log.AppendLine(line);
            _logger.LogInformation("ContentProcessing[{JobId}] {Message}", jobId, msg);
        }

        try
        {
            if (!_options.Enabled)
            {
                Log("Content processing is disabled. Skipping run.");
                await _jobRepo.CompleteAsync(jobId, 0, 0, 0, 0, log.ToString(), ct);
                return;
            }

            Log(string.Create(CultureInfo.InvariantCulture, $"Starting content processing run (trigger: {triggerType})"));

            var feeds = await LoadFeedConfigsAsync(ct);
            Log(string.Create(CultureInfo.InvariantCulture, $"Loaded {feeds.Count} feed(s) from configuration"));

            foreach (var feed in feeds)
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }

                Log(string.Create(CultureInfo.InvariantCulture, $"Processing feed: {feed.Name}"));
                feedsProcessed++;

                var rawItems = await _rssService.IngestAsync(feed, ct);
                Log(string.Create(CultureInfo.InvariantCulture, $"  {rawItems.Count} items from feed"));

                var limit = _options.MaxItemsPerRun > 0
                    ? Math.Min(rawItems.Count, _options.MaxItemsPerRun - itemsAdded)
                    : rawItems.Count;

                for (var i = 0; i < limit && !ct.IsCancellationRequested; i++)
                {
                    var raw = rawItems[i];

                    // Check duplicate
                    if (await ExistsAsync(raw.ExternalUrl, ct))
                    {
                        itemsSkipped++;
                        continue;
                    }

                    // Fetch full content (non-YouTube only)
                    raw = await _articleService.EnrichWithContentAsync(raw, ct);

                    // Delay between external requests
                    if (_options.RequestDelayMs > 0 && i > 0)
                    {
                        await Task.Delay(_options.RequestDelayMs, ct);
                    }

                    // AI categorization
                    ProcessedContentItem? processed;
                    try
                    {
                        processed = await _aiService.CategorizeAsync(raw, ct);
                    }
                    catch (Exception ex)
                    {
                        Log(string.Create(CultureInfo.InvariantCulture, $"  ERROR categorizing {raw.ExternalUrl}: {ex.Message}"));
                        errorCount++;
                        continue;
                    }

                    if (processed == null)
                    {
                        Log(string.Create(CultureInfo.InvariantCulture, $"  SKIPPED (AI): {raw.Title}"));
                        itemsSkipped++;
                        continue;
                    }

                    // Write to database
                    try
                    {
                        await WriteItemAsync(processed, ct);
                        Log(string.Create(CultureInfo.InvariantCulture, $"  ADDED: {processed.Title}"));
                        itemsAdded++;
                    }
                    catch (Exception ex)
                    {
                        Log(string.Create(CultureInfo.InvariantCulture, $"  ERROR writing {processed.ExternalUrl}: {ex.Message}"));
                        errorCount++;
                    }
                }
            }

            var duration = DateTime.UtcNow - startedAt;
            Log(string.Create(CultureInfo.InvariantCulture,
                $"Run complete. Added: {itemsAdded}, Skipped: {itemsSkipped}, Errors: {errorCount}, Duration: {duration.TotalSeconds:F1}s"));

            await _jobRepo.CompleteAsync(jobId, feedsProcessed, itemsAdded, itemsSkipped, errorCount, log.ToString(), ct);
        }
        catch (OperationCanceledException)
        {
            Log("Run cancelled.");
            await _jobRepo.FailAsync(jobId, log.ToString(), ct);
        }
        catch (Exception ex)
        {
            Log(string.Create(CultureInfo.InvariantCulture, $"FATAL ERROR: {ex.Message}"));
            _logger.LogError(ex, "Content processing run {JobId} failed with unhandled exception", jobId);
            await _jobRepo.FailAsync(jobId, log.ToString(), ct);
        }
    }

    private async Task<bool> ExistsAsync(string externalUrl, CancellationToken ct)
    {
        var count = await _connection.ExecuteScalarAsync<int>(
            new CommandDefinition(
                "SELECT COUNT(*) FROM content_items WHERE external_url = @ExternalUrl",
                new { ExternalUrl = externalUrl },
                cancellationToken: ct));
        return count > 0;
    }

    private async Task WriteItemAsync(ProcessedContentItem item, CancellationToken ct)
    {
        // Compute section booleans and bitmask from section names
        var sectionSet = new HashSet<string>(item.Sections, StringComparer.OrdinalIgnoreCase);
        var isAi = sectionSet.Contains("ai");
        var isAzure = sectionSet.Contains("azure");
        var isDotnet = sectionSet.Contains("dotnet");
        var isDevops = sectionSet.Contains("devops");
        var isGhc = sectionSet.Contains("github-copilot");
        var isMl = sectionSet.Contains("ml");
        var isSecurity = sectionSet.Contains("security");

        var bitmask = 0;
        if (isAi)
        {
            bitmask |= 1;
        }

        if (isAzure)
        {
            bitmask |= 2;
        }

        if (isDotnet)
        {
            bitmask |= 4;
        }

        if (isDevops)
        {
            bitmask |= 8;
        }

        if (isGhc)
        {
            bitmask |= 16;
        }

        if (isMl)
        {
            bitmask |= 32;
        }

        if (isSecurity)
        {
            bitmask |= 64;
        }

        var tagsCsv = item.Tags.Count > 0 ? $",{string.Join(",", item.Tags)}," : string.Empty;
        var primarySection = item.PrimarySectionName ?? (item.Sections.Count > 0 ? item.Sections[0] : "all");

        var aiMetadataJson = item.RoundupMetadata != null
            ? JsonSerializer.Serialize(new
            {
                roundup_summary = item.RoundupMetadata.Summary,
                key_topics = item.RoundupMetadata.KeyTopics,
                roundup_relevance = item.RoundupMetadata.Relevance
            }, JsonOptions)
            : null;

        const string Upsert = @"
INSERT INTO content_items
    (slug, collection_name, title, content, excerpt, date_epoch,
     primary_section_name, external_url, author, feed_name,
     tags_csv, is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
     is_ml, is_security, sections_bitmask, content_hash, ai_metadata)
VALUES
    (@Slug, @Collection, @Title, @Content, @Excerpt, @DateEpoch,
     @PrimarySection, @ExternalUrl, @Author, @FeedName,
     @TagsCsv, @IsAi, @IsAzure, @IsDotnet, @IsDevops, @IsGhc,
     @IsMl, @IsSecurity, @Bitmask, @ContentHash, @AiMetadata::jsonb)
ON CONFLICT (collection_name, slug) DO UPDATE SET
    title                = EXCLUDED.title,
    content              = EXCLUDED.content,
    excerpt              = EXCLUDED.excerpt,
    tags_csv             = EXCLUDED.tags_csv,
    is_ai                = EXCLUDED.is_ai,
    is_azure             = EXCLUDED.is_azure,
    is_dotnet            = EXCLUDED.is_dotnet,
    is_devops            = EXCLUDED.is_devops,
    is_github_copilot    = EXCLUDED.is_github_copilot,
    is_ml                = EXCLUDED.is_ml,
    is_security          = EXCLUDED.is_security,
    sections_bitmask     = EXCLUDED.sections_bitmask,
    content_hash         = EXCLUDED.content_hash,
    ai_metadata          = EXCLUDED.ai_metadata,
    updated_at           = NOW()";

        await _connection.ExecuteAsync(new CommandDefinition(
            Upsert,
            new
            {
                Slug = item.Slug,
                Collection = item.CollectionName,
                item.Title,
                Content = item.Content ?? string.Empty,
                item.Excerpt,
                item.DateEpoch,
                PrimarySection = primarySection,
                item.ExternalUrl,
                Author = item.Author ?? string.Empty,
                FeedName = item.FeedName,
                TagsCsv = tagsCsv,
                IsAi = isAi,
                IsAzure = isAzure,
                IsDotnet = isDotnet,
                IsDevops = isDevops,
                IsGhc = isGhc,
                IsMl = isMl,
                IsSecurity = isSecurity,
                Bitmask = bitmask,
                ContentHash = item.ContentHash,
                AiMetadata = aiMetadataJson
            },
            cancellationToken: ct));

        // Rebuild expanded tags
        await _connection.ExecuteAsync(new CommandDefinition(
            "DELETE FROM content_tags_expanded WHERE collection_name = @Collection AND slug = @Slug",
            new { Collection = item.CollectionName, Slug = item.Slug },
            cancellationToken: ct));

        if (item.Tags.Count > 0)
        {
            var tagRows = BuildTagWords(item.Tags, item.CollectionName, item.Slug,
                isAi, isAzure, isDotnet, isDevops, isGhc, isMl, isSecurity, bitmask);
            foreach (var row in tagRows)
            {
                await _connection.ExecuteAsync(new CommandDefinition(
                    @"INSERT INTO content_tags_expanded
                        (collection_name, slug, tag_word, tag_display, is_full_tag,
                         is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
                         is_ml, is_security, sections_bitmask)
                      VALUES
                        (@CollectionName, @Slug, @TagWord, @TagDisplay, @IsFullTag,
                         @IsAi, @IsAzure, @IsDotnet, @IsDevops, @IsGhc,
                         @IsMl, @IsSecurity, @Bitmask)
                      ON CONFLICT DO NOTHING",
                    row,
                    cancellationToken: ct));
            }
        }
    }

        private static List<object> BuildTagWords(
            IReadOnlyList<string> tags, string collection, string slug,
            bool isAi, bool isAzure, bool isDotnet, bool isDevops, bool isGhc,
            bool isMl, bool isSecurity, int bitmask)
        {
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var rows = new List<object>();

        void Add(string word, string display, bool full)
        {
            if (!seen.Add(word.ToLowerInvariant()))
            {
                return;
            }

            rows.Add(new
            {
                CollectionName = collection,
                Slug = slug,
                TagWord = word.ToLowerInvariant(),
                TagDisplay = display,
                IsFullTag = full,
                IsAi = isAi,
                IsAzure = isAzure,
                IsDotnet = isDotnet,
                IsDevops = isDevops,
                IsGhc = isGhc,
                IsMl = isMl,
                IsSecurity = isSecurity,
                Bitmask = bitmask
            });
        }

        foreach (var tag in tags)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                continue;
            }

            Add(tag, tag, true);

            // Word expansions for multi-word tags
            var words = tag.Split(new char[] { ' ', '-', '_' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length > 1)
            {
                foreach (var word in words)
                {
                    Add(word, word, false);
                }
            }
        }

        return rows;
    }

    private async Task<List<FeedConfig>> LoadFeedConfigsAsync(CancellationToken ct)
    {
        var path = _options.RssFeedsConfigPath;
        if (!Path.IsPathRooted(path))
        {
            path = Path.Combine(AppContext.BaseDirectory, path);
        }

        if (!File.Exists(path))
        {
            _logger.LogWarning("rss-feeds.json not found at {Path}. No feeds to process.", path);
            return [];
        }

        await using var stream = File.OpenRead(path);
        using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);

        // Support both flat array format: [...]
        // and wrapped object format: {"feeds": [...]}
        var feedsArray = doc.RootElement.ValueKind == JsonValueKind.Array
            ? doc.RootElement
            : doc.RootElement.TryGetProperty("feeds", out var fa) ? fa : default;

        if (feedsArray.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        var configs = new List<FeedConfig>();
        foreach (var feedEl in feedsArray.EnumerateArray())
        {
            var enabled = !feedEl.TryGetProperty("enabled", out var ep) || ep.GetBoolean();
            if (!enabled)
            {
                continue;
            }

            var name = feedEl.TryGetProperty("name", out var n) ? n.GetString() ?? string.Empty : string.Empty;
            var url = feedEl.TryGetProperty("url", out var u) ? u.GetString() ?? string.Empty : string.Empty;
            var outputDir = feedEl.TryGetProperty("outputDir", out var od) || feedEl.TryGetProperty("output_dir", out od)
                ? od.GetString() ?? string.Empty : string.Empty;

            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(url) && !string.IsNullOrWhiteSpace(outputDir))
            {
                configs.Add(new FeedConfig { Name = name, Url = url, OutputDir = outputDir });
            }
        }

        return configs;
    }
}

#pragma warning restore CA1031
