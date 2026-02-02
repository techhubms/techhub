using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// Synchronizes content from markdown files to database using hash-based change detection.
/// Entire sync runs in a transaction - rolls back on any error (fail-fast approach).
/// </summary>
public class ContentSyncService : IContentSyncService
{
    private readonly IDbConnection _connection;
    private readonly IMarkdownService _markdownService;
    private readonly ILogger<ContentSyncService> _logger;
    private readonly SyncPerformanceLogger _perfLogger;
    private readonly ContentSyncOptions _options;
    private readonly string _collectionsPath;
    private static readonly char[] _tagSplitSeparators = [' ', '-', '_'];
    private static readonly char[] _excerptSplitSeparators = [' ', '\n', '\r'];
    
    // Runtime-captured database schema objects (populated during DisableIndexesAndTriggersAsync)
    private List<string>? _capturedIndexDefinitions;
    private List<string>? _capturedTriggerDefinitions;

    private sealed record ParsedContent(
        string CompositeId,
        string CollectionName,
        string Slug,
        string? SubcollectionName,
        Dictionary<string, object?> FrontMatter,
        string Content,
        string Excerpt,
        long DateEpoch,
        List<string> Sections,
        List<string> Tags,
        List<string> Plans,
        string ContentHash);

    /// <summary>
    /// Strongly-typed record for tag words to avoid reflection overhead during bulk insert.
    /// Uses int (0/1) for boolean flags to match SQLite storage format.
    /// SectionsBitmask: Bit 0=AI, Bit 1=Azure, Bit 2=Coding, Bit 3=DevOps, Bit 4=GitHubCopilot, Bit 5=ML, Bit 6=Security
    /// </summary>
    private sealed record TagWord(
        string CollectionName,
        string Slug,
        string Word,
        long DateEpoch,
        int IsAi,
        int IsAzure,
        int IsCoding,
        int IsDevOps,
        int IsGitHubCopilot,
        int IsMl,
        int IsSecurity,
        int SectionsBitmask);

    public ContentSyncService(
        IDbConnection connection,
        IMarkdownService markdownService,
        ILogger<ContentSyncService> logger,
        IOptions<ContentSyncOptions> options,
        IOptions<ContentOptions> contentOptions)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(markdownService);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(contentOptions);

        _connection = connection;
        _markdownService = markdownService;
        _logger = logger;
        _perfLogger = new SyncPerformanceLogger(logger);
        _options = options.Value;
        _collectionsPath = contentOptions.Value.CollectionsPath;
    }

    public async Task<SyncResult> SyncAsync(CancellationToken ct = default)
    {
        if (!_options.Enabled)
        {
            _logger.LogInformation("Content sync disabled (ContentSync:Enabled = false)");
            return new SyncResult(0, 0, 0, 0, TimeSpan.Zero);
        }

        // Force full sync if explicitly configured
        if (_options.ForceFullSync)
        {
            return await ForceSyncAsync(ct);
        }

        // Check if database has content
        var hasContent = await HasContentAsync(ct);

        if (!hasContent)
        {
            // Database is empty - do a full sync
            _logger.LogInformation("Database is empty, performing initial full sync");
            return await ForceSyncAsync(ct);
        }

        // Database has content - do incremental sync by default
        return await IncrementalSyncAsync(ct);
    }

    public async Task<bool> HasContentAsync(CancellationToken ct = default)
    {
        var count = await _connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM content_items");
        return count > 0;
    }

    public async Task<SyncResult> ForceSyncAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("Starting FULL content sync (deleting all existing content)...");
        var stopwatch = Stopwatch.StartNew();

        try
        {
            // Disable indexes and triggers for maximum write performance
            await DisableIndexesAndTriggersAsync();

            // Start transaction
            var transaction = _connection.BeginTransaction();

            try
            {
                // Delete all existing content (cascade deletes junction tables)
                await _connection.ExecuteAsync("DELETE FROM content_items", transaction: transaction);
                _logger.LogInformation("Deleted all existing content");

                transaction.Commit();
                stopwatch.Stop();

                _logger.LogInformation("Delete complete ({Duration:N0}ms)", stopwatch.ElapsedMilliseconds);
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Full sync failed - delete transaction rolled back");
            throw;
        }

        // Now sync content with batched commits (no single giant transaction)
        stopwatch.Restart();
        try
        {
            // Load and insert all content with batched transactions
            // For full sync we always use bulk mode (indexes already disabled above)
            var (added, updated, deleted, unchanged, _) = await SyncContentAsync(ct, bulkModeAlreadyEnabled: true);

            // Update sync metadata in separate transaction
            var metaTrans = _connection.BeginTransaction();
            try
            {
                await UpdateSyncMetadataAsync(added, metaTrans);
                metaTrans.Commit();

                stopwatch.Stop();

                _logger.LogInformation(
                    "Full sync complete: {Added} added, {Duration:N0}ms",
                    added, stopwatch.ElapsedMilliseconds);

                return new SyncResult(added, 0, 0, 0, stopwatch.Elapsed);
            }
            catch
            {
                metaTrans.Rollback();
                throw;
            }
            finally
            {
                // Re-enable indexes and triggers
                await EnableIndexesAndTriggersAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Full sync failed - metadata transaction rolled back");
            throw;
        }
    }

    public async Task<bool> IsContentChangedAsync(CancellationToken ct = default)
    {
        var markdownFiles = GetMarkdownFiles();
        var fileHashes = await ComputeFileHashesAsync(markdownFiles, ct);
        var dbHashes = await GetDatabaseHashesAsync();

        // Check if any files are new, changed, or deleted
        var newFiles = fileHashes.Keys.Except(dbHashes.Keys).Any();
        var deletedFiles = dbHashes.Keys.Except(fileHashes.Keys).Any();
        var changedFiles = fileHashes.Where(kvp =>
            dbHashes.TryGetValue(kvp.Key, out var dbHash) && dbHash != kvp.Value).Any();

        return newFiles || deletedFiles || changedFiles;
    }

    // Threshold for disabling indexes during sync - only worth it for bulk operations
    private const int BulkOperationThreshold = 50;

    private async Task<SyncResult> IncrementalSyncAsync(CancellationToken ct)
    {
        _logger.LogInformation("Starting incremental content sync...");
        var stopwatch = Stopwatch.StartNew();

        try
        {
            // Sync content with batched transactions
            // Pass flag to indicate whether to use bulk mode (disable indexes)
            var (added, updated, deleted, unchanged, usedBulkMode) = await SyncContentAsync(ct);

            // Update sync metadata in separate transaction
            var transaction = _connection.BeginTransaction();
            try
            {
                // Total items = new items + updated items + unchanged items
                await UpdateSyncMetadataAsync(added + updated + unchanged, transaction);

                transaction.Commit();
                stopwatch.Stop();

                _logger.LogInformation(
                    "Incremental sync complete: {Added} added, {Updated} updated, {Deleted} deleted, {Unchanged} unchanged ({Duration:N0}ms)",
                    added, updated, deleted, unchanged, stopwatch.ElapsedMilliseconds);

                return new SyncResult(added, updated, deleted, unchanged, stopwatch.Elapsed);
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                // Re-enable indexes and triggers only if we disabled them
                if (usedBulkMode)
                {
                    await EnableIndexesAndTriggersAsync();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Incremental sync failed - transaction rolled back");
            throw;
        }
    }

    private async Task<(int added, int updated, int deleted, int unchanged, bool usedBulkMode)> SyncContentAsync(
        CancellationToken ct,
        bool bulkModeAlreadyEnabled = false)
    {
        // Get all markdown files
        var markdownFilesList = GetMarkdownFiles();
        _logger.LogInformation("Found {Count} markdown files", markdownFilesList.Count);

        // Create dictionary for file lookup by slug
        var markdownFiles = markdownFilesList.ToDictionary(f => GetContentSlugFromFile(f), f => f);

        // Compute hashes for all files
        var fileHashes = await ComputeFileHashesAsync(markdownFilesList, ct);

        // Get existing hashes from database
        var dbHashes = await GetDatabaseHashesAsync();

        // Determine changes
        var newFiles = fileHashes.Keys.Except(dbHashes.Keys).ToList();
        var deletedFiles = dbHashes.Keys.Except(fileHashes.Keys).ToList();
        var potentiallyChanged = fileHashes.Keys.Intersect(dbHashes.Keys)
            .Where(id => fileHashes[id] != dbHashes[id])
            .ToList();
        var unchanged = fileHashes.Keys.Intersect(dbHashes.Keys)
            .Where(id => fileHashes[id] == dbHashes[id])
            .ToList();

        _logger.LogInformation(
            "Changes detected: {New} new, {Updated} updated, {Deleted} deleted, {Unchanged} unchanged",
            newFiles.Count, potentiallyChanged.Count, deletedFiles.Count, unchanged.Count);

        // Determine if we should use bulk mode (disable indexes) based on change count
        // Skip if caller already enabled bulk mode
        var totalChanges = newFiles.Count + potentiallyChanged.Count + deletedFiles.Count;
        var usedBulkMode = bulkModeAlreadyEnabled || totalChanges >= BulkOperationThreshold;

        if (!bulkModeAlreadyEnabled && totalChanges >= BulkOperationThreshold)
        {
            _logger.LogInformation("Using bulk mode for {Count} changes (threshold: {Threshold})", totalChanges, BulkOperationThreshold);
            await DisableIndexesAndTriggersAsync();
        }
        else if (totalChanges > 0 && !bulkModeAlreadyEnabled)
        {
            _logger.LogInformation("Using normal mode for {Count} changes (below threshold: {Threshold})", totalChanges, BulkOperationThreshold);
        }

        // Process deletions with batched transactions
        if (deletedFiles.Count > 0)
        {
            const int BatchSize = 100;
            for (int i = 0; i < deletedFiles.Count; i += BatchSize)
            {
                var batch = deletedFiles.Skip(i).Take(BatchSize);
                var transaction = _connection.BeginTransaction();
                try
                {
                    foreach (var compositeId in batch)
                    {
                        var parts = compositeId.Split(':');
                        var collectionName = parts[0];
                        var slug = parts[1];
                        await _connection.ExecuteAsync(
                            "DELETE FROM content_items WHERE collection_name = @CollectionName AND slug = @Slug",
                            new { CollectionName = collectionName, Slug = slug },
                            transaction);
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        // Phase 1: Parse files in parallel (I/O and CPU intensive)
        var filesToProcess = newFiles.Concat(potentiallyChanged).ToList();
        _logger.LogInformation("Parsing {Count} files in parallel...", filesToProcess.Count);
        var parsedFiles = await ParseFilesInParallelAsync(filesToProcess, markdownFiles, fileHashes, ct);

        // Track which items are new (INSERT) vs updated (UPDATE) to optimize tag deletion
        var newItemIds = newFiles.ToHashSet();

        // Phase 2: Write to database with batched commits (commit every 500 items for better performance)
        _logger.LogInformation("Writing {Count} items to database...", parsedFiles.Count);
        const int WriteBatchSize = 500;
        var writeCount = 0;
        
        for (int i = 0; i < parsedFiles.Count; i += WriteBatchSize)
        {
            var batchStopwatch = Stopwatch.StartNew();
            var batch = parsedFiles.Skip(i).Take(WriteBatchSize).ToList();
            var transaction = _connection.BeginTransaction();
            var itemStopwatch = Stopwatch.StartNew();
            var totalDeleteMs = 0L;
            var totalInsertContentMs = 0L;
            var allBatchTags = new List<TagWord>();
            try
            {
                foreach (var parsed in batch)
                {
                    var isNewItem = newItemIds.Contains(parsed.CompositeId);
                    itemStopwatch.Restart();
                    var (deleteMs, insertContentMs, tagWords) = await WriteToDatabase(parsed, transaction, isNewItem);
                    totalDeleteMs += deleteMs;
                    totalInsertContentMs += insertContentMs;
                    allBatchTags.AddRange(tagWords);
                    writeCount++;
                }

                // Bulk insert all tags for this batch in chunks (SQLite has a limit of ~999 parameters)
                var insertTagsStopwatch = Stopwatch.StartNew();
                if (allBatchTags.Count > 0)
                {
                    const int TagsPerChunk = 90; // 90 tags × 11 params = 990 parameters (just under limit)
                    for (int chunkStart = 0; chunkStart < allBatchTags.Count; chunkStart += TagsPerChunk)
                    {
                        var chunkSize = Math.Min(TagsPerChunk, allBatchTags.Count - chunkStart);
                        var sb = new System.Text.StringBuilder();
                        sb.Append("INSERT OR IGNORE INTO content_tags_expanded ");
                        sb.Append("(collection_name, slug, tag_word, date_epoch, is_ai, is_azure, is_coding, is_devops, is_github_copilot, is_ml, is_security, sections_bitmask) VALUES ");
                        
                        using var cmd = _connection.CreateCommand();
                        cmd.Transaction = transaction;
                        
                        for (int tagIdx = 0; tagIdx < chunkSize; tagIdx++)
                        {
                            var tag = allBatchTags[chunkStart + tagIdx];

                            if (tagIdx > 0)
                            {
                                sb.Append(", ");
                            }

                            sb.Append(System.Globalization.CultureInfo.InvariantCulture, $"(@cn{tagIdx}, @s{tagIdx}, @w{tagIdx}, @de{tagIdx}, @ai{tagIdx}, @az{tagIdx}, @c{tagIdx}, @do{tagIdx}, @gc{tagIdx}, @ml{tagIdx}, @sec{tagIdx}, @bm{tagIdx})");
                            
                            // Add parameters using direct property access (no reflection)
                            cmd.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter($"@cn{tagIdx}", tag.CollectionName));
                            cmd.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter($"@s{tagIdx}", tag.Slug));
                            cmd.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter($"@w{tagIdx}", tag.Word));
                            cmd.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter($"@de{tagIdx}", tag.DateEpoch));
                            cmd.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter($"@ai{tagIdx}", tag.IsAi));
                            cmd.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter($"@az{tagIdx}", tag.IsAzure));
                            cmd.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter($"@c{tagIdx}", tag.IsCoding));
                            cmd.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter($"@do{tagIdx}", tag.IsDevOps));
                            cmd.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter($"@gc{tagIdx}", tag.IsGitHubCopilot));
                            cmd.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter($"@ml{tagIdx}", tag.IsMl));
                            cmd.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter($"@sec{tagIdx}", tag.IsSecurity));
                            cmd.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter($"@bm{tagIdx}", tag.SectionsBitmask));
                        }
                        
                        cmd.CommandText = sb.ToString();
                        await ((System.Data.Common.DbCommand)cmd).ExecuteNonQueryAsync(ct);
                    }
                }

                var totalInsertTagsMs = insertTagsStopwatch.ElapsedMilliseconds;

                var commitStopwatch = Stopwatch.StartNew();
                transaction.Commit();
                var commitMs = commitStopwatch.ElapsedMilliseconds;
                var totalBatchMs = batchStopwatch.ElapsedMilliseconds;
                
                _perfLogger.LogBatchProgress(
                    (i / WriteBatchSize) + 1,
                    batch.Count,
                    totalBatchMs,
                    totalBatchMs - commitMs,
                    commitMs);
                
                _logger.LogInformation("  ↳ Breakdown: DELETE {DeleteMs}ms, INSERT content {InsertContentMs}ms, INSERT tags {InsertTagsMs}ms ({TagCount} tags)",
                    totalDeleteMs, totalInsertContentMs, totalInsertTagsMs, allBatchTags.Count);
                
                if (writeCount % 500 == 0 || writeCount >= parsedFiles.Count)
                {
                    _logger.LogInformation("Progress: {Written}/{Total} items written", writeCount, parsedFiles.Count);
                }
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        
        _logger.LogInformation("Database writes complete: {Total} items", parsedFiles.Count);

        return (newFiles.Count, potentiallyChanged.Count, deletedFiles.Count, unchanged.Count, usedBulkMode);
    }

    private List<FileInfo> GetMarkdownFiles()
    {
        var files = new List<FileInfo>();
        var collectionsDir = new DirectoryInfo(_collectionsPath);

        if (!collectionsDir.Exists)
        {
            throw new DirectoryNotFoundException($"Collections directory not found: {_collectionsPath}");
        }

        // Get all subdirectories starting with underscore (collections)
        var collectionDirs = collectionsDir.GetDirectories("_*");

        foreach (var dir in collectionDirs)
        {
            // Get all markdown files recursively (including subfolders like ghc-features)
            var mdFiles = dir.GetFiles("*.md", SearchOption.AllDirectories)
                .Where(f => !f.Name.Equals("AGENTS.md", StringComparison.OrdinalIgnoreCase) &&
                           !f.Name.EndsWith("-guidelines.md", StringComparison.OrdinalIgnoreCase));

            files.AddRange(mdFiles);
        }

        return files;
    }

    private static async Task<Dictionary<string, string>> ComputeFileHashesAsync(
        List<FileInfo> files,
        CancellationToken ct)
    {
        var hashes = new System.Collections.Concurrent.ConcurrentDictionary<string, string>();

        await Parallel.ForEachAsync(files, new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount * 2,
            CancellationToken = ct
        }, async (file, token) =>
        {
            var slug = GetContentSlugFromFile(file);
            var content = await File.ReadAllTextAsync(file.FullName, token);
            var hash = ComputeSha256Hash(content);
            hashes[slug] = hash;
        });

        return hashes.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    private async Task<Dictionary<string, string>> GetDatabaseHashesAsync()
    {
        var rows = await _connection.QueryAsync<(string CollectionName, string Slug, string ContentHash)>(
            "SELECT collection_name, slug, content_hash FROM content_items");

        // Use composite key "collection:slug" to allow same slug in different collections
        return rows.ToDictionary(r => $"{r.CollectionName}:{r.Slug}", r => r.ContentHash);
    }

    private async Task<List<ParsedContent>> ParseFilesInParallelAsync(
        List<string> compositeIds,
        Dictionary<string, FileInfo> markdownFiles,
        Dictionary<string, string> fileHashes,
        CancellationToken ct)
    {
        var results = new System.Collections.Concurrent.ConcurrentBag<ParsedContent>();

        await Parallel.ForEachAsync(compositeIds, new ParallelOptions
        {
            MaxDegreeOfParallelism = _options.MaxParallelFiles,
            CancellationToken = ct
        }, async (compositeId, token) =>
        {
            try
            {
                var file = markdownFiles[compositeId];
                var contentHash = fileHashes[compositeId];

                // Extract collection and slug from composite key "collection:slug"
                var parts = compositeId.Split(':');
                var collectionName = parts[0];
                var slug = parts[1];

                // Parse frontmatter and content (I/O intensive)
                var frontMatter = await _markdownService.ParseFrontMatterAsync(file.FullName, token);
                var fileContent = await File.ReadAllTextAsync(file.FullName, token);

                // Extract content and excerpt (CPU intensive)
                var (excerpt, content) = ExtractExcerptAndContent(fileContent);

                // Extract metadata from frontmatter and file path
                var subcollectionName = GetSubcollectionNameFromPath(file);
                var dateEpoch = GetDateEpochFromFrontMatter(frontMatter, file.Name);
                var sections = GetSectionsFromFrontMatter(frontMatter);
                var tags = GetTagsFromFrontMatter(frontMatter);
                var plans = GetPlansFromFrontMatter(frontMatter);

                // Validate primary_section is present in frontmatter
                var primarySection = frontMatter.GetValueOrDefault("primary_section", null)?.ToString();
                if (string.IsNullOrWhiteSpace(primarySection))
                {
                    throw new InvalidOperationException(
                        $"Missing required 'primary_section' in frontmatter for file: {file.FullName}. " +
                        "Run ContentFixer to add missing frontmatter fields.");
                }

                results.Add(new ParsedContent(
                    compositeId,
                    collectionName,
                    slug,
                    subcollectionName,
                    frontMatter,
                    content,
                    excerpt,
                    dateEpoch,
                    sections,
                    tags,
                    plans,
                    contentHash));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to parse file for composite ID: {CompositeId}", compositeId);
                throw;
            }
        });

        return [.. results];
    }

    private async Task<(long deleteMs, long insertContentMs, List<TagWord> tagWords)> WriteToDatabase(ParsedContent parsed, IDbTransaction transaction, bool isNewItem)
    {
        long deleteMs;
        long insertContentMs;

        try
        {
            var primarySection = parsed.FrontMatter.GetValueOrDefault("primary_section", null)?.ToString()!;

            // Build tags_csv for in-memory parsing: ",AI,GitHub Copilot,DevOps,"
            var tagsCsv = parsed.Tags.Count > 0 ? $",{string.Join(",", parsed.Tags)}," : "";

            // Build section boolean flags
            var sectionFlags = new
            {
                IsAi = parsed.Sections.Contains("ai") ? 1 : 0,
                IsAzure = parsed.Sections.Contains("azure") ? 1 : 0,
                IsCoding = parsed.Sections.Contains("coding") ? 1 : 0,
                IsDevOps = parsed.Sections.Contains("devops") ? 1 : 0,
                IsGitHubCopilot = parsed.Sections.Contains("github-copilot") ? 1 : 0,
                IsMl = parsed.Sections.Contains("ml") ? 1 : 0,
                IsSecurity = parsed.Sections.Contains("security") ? 1 : 0
            };

            // Upsert main content item with section booleans
            var insertContentStopwatch = Stopwatch.StartNew();
            
            // Calculate sections bitmask (Bit 0=AI, Bit 1=Azure, Bit 2=Coding, Bit 3=DevOps, Bit 4=GitHubCopilot, Bit 5=ML, Bit 6=Security)
            var sectionsBitmask = (sectionFlags.IsAi * 1) +
                                  (sectionFlags.IsAzure * 2) +
                                  (sectionFlags.IsCoding * 4) +
                                  (sectionFlags.IsDevOps * 8) +
                                  (sectionFlags.IsGitHubCopilot * 16) +
                                  (sectionFlags.IsMl * 32) +
                                  (sectionFlags.IsSecurity * 64);
            
            await _connection.ExecuteAsync(@"
                INSERT INTO content_items (
                    slug, title, content, excerpt, date_epoch, collection_name, subcollection_name,
                    primary_section_name, external_url, author, feed_name, ghes_support, draft, plans, tags_csv, content_hash,
                    is_ai, is_azure, is_coding, is_devops, is_github_copilot, is_ml, is_security, sections_bitmask,
                    created_at, updated_at
                ) VALUES (
                    @Slug, @Title, @Content, @Excerpt, @DateEpoch, @CollectionName, @SubcollectionName,
                    @PrimarySectionName, @ExternalUrl, @Author, @FeedName, @GhesSupport, @Draft, @Plans, @TagsCsv, @ContentHash,
                    @IsAi, @IsAzure, @IsCoding, @IsDevOps, @IsGitHubCopilot, @IsMl, @IsSecurity, @SectionsBitmask,
                    CURRENT_TIMESTAMP, CURRENT_TIMESTAMP
                )
                ON CONFLICT(collection_name, slug) DO UPDATE SET
                    title = @Title,
                    content = @Content,
                    excerpt = @Excerpt,
                    date_epoch = @DateEpoch,
                    subcollection_name = @SubcollectionName,
                    primary_section_name = @PrimarySectionName,
                    external_url = @ExternalUrl,
                    author = @Author,
                    feed_name = @FeedName,
                    ghes_support = @GhesSupport,
                    draft = @Draft,
                    plans = @Plans,
                    tags_csv = @TagsCsv,
                    content_hash = @ContentHash,
                    is_ai = @IsAi,
                    is_azure = @IsAzure,
                    is_coding = @IsCoding,
                    is_devops = @IsDevOps,
                    is_github_copilot = @IsGitHubCopilot,
                    is_ml = @IsMl,
                    is_security = @IsSecurity,
                    sections_bitmask = @SectionsBitmask,
                    updated_at = CURRENT_TIMESTAMP",
                new
                {
                    Slug = parsed.Slug,
                    Title = parsed.FrontMatter.GetValueOrDefault("title", "")?.ToString() ?? "",
                    Content = parsed.Content,
                    Excerpt = parsed.Excerpt,
                    DateEpoch = parsed.DateEpoch,
                    CollectionName = parsed.CollectionName,
                    SubcollectionName = parsed.SubcollectionName,
                    PrimarySectionName = primarySection,
                    ExternalUrl = parsed.FrontMatter.GetValueOrDefault("external_url", null)?.ToString(),
                    Author = parsed.FrontMatter.GetValueOrDefault("author", null)?.ToString(),
                    FeedName = parsed.FrontMatter.GetValueOrDefault("feed_name", null)?.ToString(),
                    GhesSupport = (parsed.FrontMatter.GetValueOrDefault("ghes_support", false) is bool ghesSupport && ghesSupport) ? 1 : 0,
                    Draft = ConvertBoolToInt(parsed.FrontMatter, "draft"),
                    Plans = parsed.Plans.Count > 0 ? string.Join(",", parsed.Plans) : null,
                    TagsCsv = tagsCsv,
                    ContentHash = parsed.ContentHash,
                    sectionFlags.IsAi,
                    sectionFlags.IsAzure,
                    sectionFlags.IsCoding,
                    sectionFlags.IsDevOps,
                    sectionFlags.IsGitHubCopilot,
                    sectionFlags.IsMl,
                    sectionFlags.IsSecurity,
                    SectionsBitmask = sectionsBitmask
                },
                transaction);
            insertContentMs = insertContentStopwatch.ElapsedMilliseconds;

            // Delete existing content_tags_expanded entries (only for updates, not new inserts)
            var deleteStopwatch = Stopwatch.StartNew();
            if (!isNewItem)
            {
                await _connection.ExecuteAsync(
                    "DELETE FROM content_tags_expanded WHERE collection_name = @CollectionName AND slug = @Slug",
                    new { Slug = parsed.Slug, CollectionName = parsed.CollectionName },
                    transaction);
            }

            deleteMs = deleteStopwatch.ElapsedMilliseconds;

            // Collect all tag words for bulk insert (eliminates N+1 query problem)
            // NOTE: Skip tags for draft items - they shouldn't be included in tag clouds
            var tagWords = new List<TagWord>();
            var isDraft = ConvertBoolToInt(parsed.FrontMatter, "draft") == 1;
            
            if (!isDraft)
            {
                foreach (var tag in parsed.Tags)
                {
                    var tagNormalized = tag.ToLowerInvariant().Trim();
                    
                    // Calculate sections bitmask (Bit 0=AI, Bit 1=Azure, Bit 2=Coding, Bit 3=DevOps, Bit 4=GitHubCopilot, Bit 5=ML, Bit 6=Security)
                    var bitmask = (sectionFlags.IsAi * 1) +
                                  (sectionFlags.IsAzure * 2) +
                                  (sectionFlags.IsCoding * 4) +
                                  (sectionFlags.IsDevOps * 8) +
                                  (sectionFlags.IsGitHubCopilot * 16) +
                                  (sectionFlags.IsMl * 32) +
                                  (sectionFlags.IsSecurity * 64);
                    
                    // Add the full tag (lowercased) for exact matching
                    tagWords.Add(new TagWord(
                        parsed.CollectionName,
                        parsed.Slug,
                        tagNormalized,
                        parsed.DateEpoch,
                        sectionFlags.IsAi,
                        sectionFlags.IsAzure,
                        sectionFlags.IsCoding,
                        sectionFlags.IsDevOps,
                        sectionFlags.IsGitHubCopilot,
                        sectionFlags.IsMl,
                        sectionFlags.IsSecurity,
                        bitmask
                    ));
                    
                    // Also expand tags into words for subset matching
                    var words = tag.Split(_tagSplitSeparators, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var word in words)
                    {
                        var wordNormalized = word.ToLowerInvariant().Trim();
                        
                        // Skip if it's the same as the full tag (already added above)
                        if (wordNormalized == tagNormalized)
                        {
                            continue;
                        }
                        
                        tagWords.Add(new TagWord(
                            parsed.CollectionName,
                            parsed.Slug,
                            wordNormalized,
                            parsed.DateEpoch,
                            sectionFlags.IsAi,
                            sectionFlags.IsAzure,
                            sectionFlags.IsCoding,
                            sectionFlags.IsDevOps,
                            sectionFlags.IsGitHubCopilot,
                            sectionFlags.IsMl,
                            sectionFlags.IsSecurity,
                            bitmask
                        ));
                    }
                }
            }

            // Return tags for batch insertion later
            return (deleteMs, insertContentMs, tagWords);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to write content to database for: {CollectionName}:{Slug}", parsed.CollectionName, parsed.Slug);
            throw;
        }
    }

    private static string GetContentSlugFromFile(FileInfo file)
    {
        // Composite key: "collection:slug" to allow same slug in different collections
        var collectionName = GetCollectionNameFromPath(file);
        // Strip date prefix from filename to create clean slug and lowercase for URL consistency
        var filename = Path.GetFileNameWithoutExtension(file.Name);
        var slug = StripDatePrefixFromSlug(filename).ToLowerInvariant();
        return $"{collectionName}:{slug}";
    }

    /// <summary>
    /// Strips YYYY-MM-DD- date prefix from filename to create clean slug.
    /// </summary>
    private static string StripDatePrefixFromSlug(string filename)
    {
        return System.Text.RegularExpressions.Regex.Replace(
            filename,
            @"^\d{4}-\d{2}-\d{2}-",
            string.Empty);
    }

    private static string GetCollectionNameFromPath(FileInfo file)
    {
        // Extract collection from path: collections/_videos/ghc-features/file.md → videos
        // or collections/_blogs/file.md → blogs
        // NOTE: Subcollections don't change the collection name - they're just for filtering
        var pathParts = file.DirectoryName?.Split(Path.DirectorySeparatorChar) ?? [];

        // Find the collection directory (starts with _)
        var collectionDir = pathParts.FirstOrDefault(p => p.StartsWith('_'))
            ?? throw new InvalidOperationException($"Cannot determine collection from path: {file.FullName}");

        // Always use the main collection name without underscore
        // Subcollections (subfolders) are for filtering only, not for changing collection name
        return collectionDir[1..]; // Remove leading underscore
    }

    private static string? GetSubcollectionNameFromPath(FileInfo file)
    {
        var pathParts = file.DirectoryName?.Split(Path.DirectorySeparatorChar) ?? [];
        var collectionDir = pathParts.FirstOrDefault(p => p.StartsWith('_'));

        if (collectionDir != null)
        {
            var collectionIndex = Array.IndexOf(pathParts, collectionDir);
            if (collectionIndex < pathParts.Length - 1)
            {
                var subfolder = pathParts[collectionIndex + 1];
                if (!string.IsNullOrEmpty(subfolder) && subfolder != file.Name)
                {
                    return subfolder;
                }
            }
        }

        return null;
    }

    private static long GetDateEpochFromFrontMatter(Dictionary<string, object?> frontMatter, string fileName)
    {
        // Try to get date from frontmatter first
        if (frontMatter.TryGetValue("date", out var dateValue) && dateValue != null)
        {
            if (DateTime.TryParse(dateValue.ToString(), out var parsedDate))
            {
                return ((DateTimeOffset)parsedDate).ToUnixTimeSeconds();
            }
        }

        // Fallback to filename (YYYY-MM-DD-title.md)
        var datePart = fileName[..10]; // First 10 chars: YYYY-MM-DD
        if (DateTime.TryParse(datePart, out var fileDate))
        {
            return ((DateTimeOffset)fileDate).ToUnixTimeSeconds();
        }

        throw new InvalidOperationException($"Cannot determine date from frontmatter or filename: {fileName}");
    }

    private static List<string> GetSectionsFromFrontMatter(Dictionary<string, object?> frontMatter)
    {
        if (frontMatter.TryGetValue("section_names", out var value) && value is IEnumerable<object> list)
        {
            return [.. list.Select(v => v.ToString() ?? "").Where(s => !string.IsNullOrEmpty(s))];
        }

        return ["all"]; // Default section
    }

    private static List<string> GetTagsFromFrontMatter(Dictionary<string, object?> frontMatter)
    {
        if (frontMatter.TryGetValue("tags", out var value) && value is IEnumerable<object> list)
        {
            return [.. list.Select(v => v.ToString() ?? "").Where(s => !string.IsNullOrEmpty(s))];
        }

        return [];
    }

    private static List<string> GetPlansFromFrontMatter(Dictionary<string, object?> frontMatter)
    {
        if (frontMatter.TryGetValue("plans", out var value) && value is IEnumerable<object> list)
        {
            return [.. list.Select(v => v.ToString() ?? "").Where(s => !string.IsNullOrEmpty(s))];
        }

        return [];
    }

    private static (string excerpt, string content) ExtractExcerptAndContent(string fileContent)
    {
        // Remove frontmatter
        var lines = fileContent.Split('\n');
        var frontMatterEnd = 0;
        var dashCount = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Trim() == "---")
            {
                dashCount++;
                if (dashCount == 2)
                {
                    frontMatterEnd = i + 1;
                    break;
                }
            }
        }

        var bodyLines = lines.Skip(frontMatterEnd).ToList();
        var body = string.Join('\n', bodyLines);

        // Extract excerpt (content before <!--excerpt_end-->)
        var excerptEndMarker = "<!--excerpt_end-->";
        var excerptEndIndex = body.IndexOf(excerptEndMarker, StringComparison.OrdinalIgnoreCase);

        if (excerptEndIndex > 0)
        {
            var excerpt = body[..excerptEndIndex].Trim();
            return (excerpt, body);
        }

        // No excerpt marker - use first 200 words as excerpt
        var words = body.Split(_excerptSplitSeparators, StringSplitOptions.RemoveEmptyEntries);
        var excerptWords = words.Take(200);
        var fallbackExcerpt = string.Join(' ', excerptWords);

        return (fallbackExcerpt, body);
    }

    private static string ComputeSha256Hash(string content)
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    private async Task UpdateSyncMetadataAsync(int totalItems, IDbTransaction transaction)
    {
        await _connection.ExecuteAsync(@"
            INSERT INTO sync_metadata (key, value, updated_at) VALUES (@Key, @Value, CURRENT_TIMESTAMP)
            ON CONFLICT(key) DO UPDATE SET value = @Value, updated_at = CURRENT_TIMESTAMP",
            new { Key = "last_sync", Value = DateTime.UtcNow.ToString("O") },
            transaction: transaction);

        await _connection.ExecuteAsync(@"
            INSERT INTO sync_metadata (key, value, updated_at) VALUES (@Key, @Value, CURRENT_TIMESTAMP)
            ON CONFLICT(key) DO UPDATE SET value = @Value, updated_at = CURRENT_TIMESTAMP",
            new { Key = "total_items", Value = totalItems.ToString(System.Globalization.CultureInfo.InvariantCulture) },
            transaction: transaction);
    }

    private static int ConvertBoolToInt(Dictionary<string, object?> frontMatter, string key)
    {
        var value = frontMatter.GetValueOrDefault(key, false);

        // Handle various types YamlDotNet might return
        return value switch
        {
            bool b => b ? 1 : 0,
            string s => s.Equals("true", StringComparison.OrdinalIgnoreCase) ? 1 : 0,
            int i => i != 0 ? 1 : 0,
            _ => 0
        };
    }

    private async Task DisableIndexesAndTriggersAsync()
    {
        _logger.LogInformation("Capturing and disabling indexes and FTS triggers for bulk insert performance...");

        // STEP 1: Capture index definitions from sqlite_master BEFORE dropping them
        var indexDefinitions = await _connection.QueryAsync<string>(
            @"SELECT sql FROM sqlite_master 
              WHERE type = 'index' 
              AND tbl_name IN ('content_items', 'content_tags_expanded')
              AND name NOT LIKE 'sqlite_autoindex_%'
              AND sql IS NOT NULL");
        _capturedIndexDefinitions = indexDefinitions.ToList();
        _logger.LogInformation("Captured {Count} index definitions", _capturedIndexDefinitions.Count);

        // STEP 2: Capture trigger definitions BEFORE dropping them
        var triggerDefinitions = await _connection.QueryAsync<string>(
            @"SELECT sql FROM sqlite_master 
              WHERE type = 'trigger' 
              AND tbl_name = 'content_items'
              AND sql IS NOT NULL");
        _capturedTriggerDefinitions = triggerDefinitions.ToList();
        _logger.LogInformation("Captured {Count} trigger definitions", _capturedTriggerDefinitions.Count);

        // STEP 3: Drop all indexes
        var indexNames = await _connection.QueryAsync<string>(
            @"SELECT name FROM sqlite_master 
              WHERE type = 'index' 
              AND tbl_name IN ('content_items', 'content_tags_expanded')
              AND name NOT LIKE 'sqlite_autoindex_%'");
        
        foreach (var indexName in indexNames)
        {
            await _connection.ExecuteAsync($"DROP INDEX IF EXISTS {indexName}");
        }
        _logger.LogInformation("Dropped {Count} indexes", indexNames.Count());

        // STEP 4: Drop all triggers
        var triggerNames = await _connection.QueryAsync<string>(
            @"SELECT name FROM sqlite_master 
              WHERE type = 'trigger' 
              AND tbl_name = 'content_items'");
        
        foreach (var triggerName in triggerNames)
        {
            await _connection.ExecuteAsync($"DROP TRIGGER IF EXISTS {triggerName}");
        }
        _logger.LogInformation("Dropped {Count} triggers", triggerNames.Count());

        _logger.LogInformation("Indexes and triggers disabled");
    }

    private async Task EnableIndexesAndTriggersAsync()
    {
        _logger.LogInformation("Re-creating indexes and FTS triggers from captured definitions...");

        if (_capturedIndexDefinitions == null || _capturedTriggerDefinitions == null)
        {
            _logger.LogError("No captured definitions found - indexes/triggers were not properly captured during disable phase");
            throw new InvalidOperationException("Cannot recreate indexes/triggers: definitions were not captured");
        }

        // STEP 1: Recreate all indexes from captured definitions
        var dbCmd = (DbCommand)_connection.CreateCommand();
        var indexStopwatch = Stopwatch.StartNew();
        
        foreach (var indexSql in _capturedIndexDefinitions)
        {
            dbCmd.CommandText = indexSql;
            await dbCmd.ExecuteNonQueryAsync();
        }
        _logger.LogInformation("⏱️ Recreated {Count} indexes in {ElapsedMs}ms", _capturedIndexDefinitions.Count, indexStopwatch.ElapsedMilliseconds);

        // STEP 2: Recreate all triggers from captured definitions
        var triggerStopwatch = Stopwatch.StartNew();
        foreach (var triggerSql in _capturedTriggerDefinitions)
        {
            dbCmd.CommandText = triggerSql;
            await dbCmd.ExecuteNonQueryAsync();
        }
        _logger.LogInformation("⏱️ Recreated {Count} triggers in {ElapsedMs}ms", _capturedTriggerDefinitions.Count, triggerStopwatch.ElapsedMilliseconds);

        // Rebuild FTS index from scratch (more efficient than incremental updates)
        _logger.LogInformation("Rebuilding FTS index...");
        var ftsRebuildStopwatch = Stopwatch.StartNew();
        var rebuildCmd = (DbCommand)_connection.CreateCommand();
        rebuildCmd.CommandText = "INSERT INTO content_fts(content_fts) VALUES('rebuild')";
        await rebuildCmd.ExecuteNonQueryAsync();
        rebuildCmd.Dispose();
        _logger.LogInformation("⏱️ FTS rebuild: {ElapsedMs}ms", ftsRebuildStopwatch.ElapsedMilliseconds);

        // Update statistics for query planner
        _logger.LogInformation("Updating query planner statistics...");
        var analyzeStopwatch = Stopwatch.StartNew();
        var analyzeCmd = (DbCommand)_connection.CreateCommand();
        analyzeCmd.CommandText = "ANALYZE";
        await analyzeCmd.ExecuteNonQueryAsync();
        analyzeCmd.Dispose();
        _logger.LogInformation("⏱️ ANALYZE: {ElapsedMs}ms", analyzeStopwatch.ElapsedMilliseconds);

        _logger.LogInformation("Indexes and triggers re-created");
    }
}

/// <summary>
/// Configuration options for content synchronization.
/// </summary>
public class ContentSyncOptions
{
    public bool Enabled { get; set; } = true;
    public bool ForceFullSync { get; set; }
    public int MaxParallelFiles { get; set; } = 10;
}

/// <summary>
/// Configuration options for content paths.
/// </summary>
public class ContentOptions
{
    public required string CollectionsPath { get; set; }
}
