using System.Data;
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
    private readonly ContentSyncOptions _options;
    private readonly ISqlDialect _dialect;
    private readonly string _collectionsPath;
    private static readonly char[] _tagSplitSeparators = [' ', '-', '_'];
    private static readonly char[] _excerptSplitSeparators = [' ', '\n', '\r'];

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
    /// Uses int (0/1) for boolean flags for database storage.
    /// SectionsBitmask: Bit 0=AI, Bit 1=Azure, Bit 2=.NET, Bit 3=DevOps, Bit 4=GitHubCopilot, Bit 5=ML, Bit 6=Security
    /// TagDisplay: Original case tag name for full tags, or original-cased word for word expansions
    /// </summary>
    private sealed record TagWord(
        string CollectionName,
        string Slug,
        string Word,           // Always lowercase for efficient querying
        string? TagDisplay,    // Original case from the full tag (e.g., "GitHub" from "GitHub Copilot")
        int IsFullTag,         // 1 = actual tag, 0 = word expansion
        long DateEpoch,
        int IsAi,
        int IsAzure,
        int IsDotNet,
        int IsDevOps,
        int IsGitHubCopilot,
        int IsMl,
        int IsSecurity,
        int SectionsBitmask);

    public ContentSyncService(
        IDbConnection connection,
        IMarkdownService markdownService,
        ILogger<ContentSyncService> logger,
        ISqlDialect dialect,
        IOptions<ContentSyncOptions> options,
        IOptions<ContentOptions> contentOptions)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(markdownService);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(dialect);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(contentOptions);

        _connection = connection;
        _markdownService = markdownService;
        _logger = logger;
        _dialect = dialect;
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
        var usedBulkMode = bulkModeAlreadyEnabled || totalChanges >= _options.BulkOperationThreshold;

        if (!bulkModeAlreadyEnabled && totalChanges >= _options.BulkOperationThreshold)
        {
            _logger.LogInformation("Using bulk mode for {Count} changes (threshold: {Threshold})", totalChanges, _options.BulkOperationThreshold);
            await DisableIndexesAndTriggersAsync();
        }
        else if (totalChanges > 0 && !bulkModeAlreadyEnabled)
        {
            _logger.LogInformation("Using normal mode for {Count} changes (below threshold: {Threshold})", totalChanges, _options.BulkOperationThreshold);
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

                // Bulk insert all tags for this batch in chunks (PostgreSQL has parameter limits)
                var insertTagsStopwatch = Stopwatch.StartNew();
                if (allBatchTags.Count > 0)
                {
                    const int TagsPerChunk = 70; // 70 tags × 14 params = 980 parameters per chunk
                    for (int chunkStart = 0; chunkStart < allBatchTags.Count; chunkStart += TagsPerChunk)
                    {
                        var chunkSize = Math.Min(TagsPerChunk, allBatchTags.Count - chunkStart);
                        var sb = new System.Text.StringBuilder();
                        sb.Append(_dialect.GetInsertIgnorePrefix("content_tags_expanded", "(collection_name, slug, tag_word, tag_display, is_full_tag, date_epoch, is_ai, is_azure, is_dotnet, is_devops, is_github_copilot, is_ml, is_security, sections_bitmask)"));

                        using var cmd = _connection.CreateCommand();
                        cmd.Transaction = transaction;

                        for (int tagIdx = 0; tagIdx < chunkSize; tagIdx++)
                        {
                            var tag = allBatchTags[chunkStart + tagIdx];

                            if (tagIdx > 0)
                            {
                                sb.Append(", ");
                            }

                            sb.Append(System.Globalization.CultureInfo.InvariantCulture, $"(@cn{tagIdx}, @s{tagIdx}, @w{tagIdx}, @td{tagIdx}, @ft{tagIdx}, @de{tagIdx}, @ai{tagIdx}, @az{tagIdx}, @c{tagIdx}, @do{tagIdx}, @gc{tagIdx}, @ml{tagIdx}, @sec{tagIdx}, @bm{tagIdx})");

                            // Add parameters using database-agnostic approach
                            var cnParam = cmd.CreateParameter();
                            cnParam.ParameterName = $"@cn{tagIdx}";
                            cnParam.Value = tag.CollectionName;
                            cmd.Parameters.Add(cnParam);

                            var sParam = cmd.CreateParameter();
                            sParam.ParameterName = $"@s{tagIdx}";
                            sParam.Value = tag.Slug;
                            cmd.Parameters.Add(sParam);

                            var wParam = cmd.CreateParameter();
                            wParam.ParameterName = $"@w{tagIdx}";
                            wParam.Value = tag.Word;
                            cmd.Parameters.Add(wParam);

                            var tdParam = cmd.CreateParameter();
                            tdParam.ParameterName = $"@td{tagIdx}";
                            tdParam.Value = tag.TagDisplay ?? (object)DBNull.Value;
                            cmd.Parameters.Add(tdParam);

                            var ftParam = cmd.CreateParameter();
                            ftParam.ParameterName = $"@ft{tagIdx}";
                            ftParam.Value = _dialect.ConvertBooleanParameter(tag.IsFullTag == 1);
                            cmd.Parameters.Add(ftParam);

                            var deParam = cmd.CreateParameter();
                            deParam.ParameterName = $"@de{tagIdx}";
                            deParam.Value = tag.DateEpoch;
                            cmd.Parameters.Add(deParam);

                            var aiParam = cmd.CreateParameter();
                            aiParam.ParameterName = $"@ai{tagIdx}";
                            aiParam.Value = _dialect.ConvertBooleanParameter(tag.IsAi == 1);
                            cmd.Parameters.Add(aiParam);

                            var azParam = cmd.CreateParameter();
                            azParam.ParameterName = $"@az{tagIdx}";
                            azParam.Value = _dialect.ConvertBooleanParameter(tag.IsAzure == 1);
                            cmd.Parameters.Add(azParam);

                            var cParam = cmd.CreateParameter();
                            cParam.ParameterName = $"@c{tagIdx}";
                            cParam.Value = _dialect.ConvertBooleanParameter(tag.IsDotNet == 1);
                            cmd.Parameters.Add(cParam);

                            var doParam = cmd.CreateParameter();
                            doParam.ParameterName = $"@do{tagIdx}";
                            doParam.Value = _dialect.ConvertBooleanParameter(tag.IsDevOps == 1);
                            cmd.Parameters.Add(doParam);

                            var gcParam = cmd.CreateParameter();
                            gcParam.ParameterName = $"@gc{tagIdx}";
                            gcParam.Value = _dialect.ConvertBooleanParameter(tag.IsGitHubCopilot == 1);
                            cmd.Parameters.Add(gcParam);

                            var mlParam = cmd.CreateParameter();
                            mlParam.ParameterName = $"@ml{tagIdx}";
                            mlParam.Value = _dialect.ConvertBooleanParameter(tag.IsMl == 1);
                            cmd.Parameters.Add(mlParam);

                            var secParam = cmd.CreateParameter();
                            secParam.ParameterName = $"@sec{tagIdx}";
                            secParam.Value = _dialect.ConvertBooleanParameter(tag.IsSecurity == 1);
                            cmd.Parameters.Add(secParam);

                            var bmParam = cmd.CreateParameter();
                            bmParam.ParameterName = $"@bm{tagIdx}";
                            bmParam.Value = tag.SectionsBitmask;
                            cmd.Parameters.Add(bmParam);
                        }

                        sb.Append(_dialect.GetInsertIgnoreSuffix());
                        cmd.CommandText = sb.ToString();
                        await ((System.Data.Common.DbCommand)cmd).ExecuteNonQueryAsync(ct);
                    }
                }

                var totalInsertTagsMs = insertTagsStopwatch.ElapsedMilliseconds;

                var commitStopwatch = Stopwatch.StartNew();
                transaction.Commit();
                var commitMs = commitStopwatch.ElapsedMilliseconds;
                var totalBatchMs = batchStopwatch.ElapsedMilliseconds;

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
            var sectionBools = new
            {
                IsAi = parsed.Sections.Contains("ai"),
                IsAzure = parsed.Sections.Contains("azure"),
                IsDotNet = parsed.Sections.Contains("dotnet"),
                IsDevOps = parsed.Sections.Contains("devops"),
                IsGitHubCopilot = parsed.Sections.Contains("github-copilot"),
                IsMl = parsed.Sections.Contains("ml"),
                IsSecurity = parsed.Sections.Contains("security")
            };

            // Convert to integers for TagWord records
            var sectionInts = new
            {
                IsAi = sectionBools.IsAi ? 1 : 0,
                IsAzure = sectionBools.IsAzure ? 1 : 0,
                IsDotNet = sectionBools.IsDotNet ? 1 : 0,
                IsDevOps = sectionBools.IsDevOps ? 1 : 0,
                IsGitHubCopilot = sectionBools.IsGitHubCopilot ? 1 : 0,
                IsMl = sectionBools.IsMl ? 1 : 0,
                IsSecurity = sectionBools.IsSecurity ? 1 : 0
            };

            // Upsert main content item with section booleans
            var insertContentStopwatch = Stopwatch.StartNew();

            // Calculate sections bitmask (Bit 0=AI, Bit 1=Azure, Bit 2=.NET, Bit 3=DevOps, Bit 4=GitHubCopilot, Bit 5=ML, Bit 6=Security)
            var sectionsBitmask = (sectionInts.IsAi * 1) +
                                  (sectionInts.IsAzure * 2) +
                                  (sectionInts.IsDotNet * 4) +
                                  (sectionInts.IsDevOps * 8) +
                                  (sectionInts.IsGitHubCopilot * 16) +
                                  (sectionInts.IsMl * 32) +
                                  (sectionInts.IsSecurity * 64);

            await _connection.ExecuteAsync(@"
                INSERT INTO content_items (
                    slug, title, content, excerpt, date_epoch, collection_name, subcollection_name,
                    primary_section_name, external_url, author, feed_name, ghes_support, draft, plans, tags_csv, content_hash,
                    is_ai, is_azure, is_dotnet, is_devops, is_github_copilot, is_ml, is_security, sections_bitmask,
                    created_at, updated_at
                ) VALUES (
                    @Slug, @Title, @Content, @Excerpt, @DateEpoch, @CollectionName, @SubcollectionName,
                    @PrimarySectionName, @ExternalUrl, @Author, @FeedName, @GhesSupport, @Draft, @Plans, @TagsCsv, @ContentHash,
                    @IsAi, @IsAzure, @IsDotNet, @IsDevOps, @IsGitHubCopilot, @IsMl, @IsSecurity, @SectionsBitmask,
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
                    is_dotnet = @IsDotNet,
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
                    ExternalUrl = parsed.FrontMatter.GetValueOrDefault("external_url", "")?.ToString() ?? "",
                    Author = parsed.FrontMatter.GetValueOrDefault("author", null)?.ToString(),
                    FeedName = parsed.FrontMatter.GetValueOrDefault("feed_name", null)?.ToString(),
                    GhesSupport = _dialect.ConvertBooleanParameter(parsed.FrontMatter.GetValueOrDefault("ghes_support", false) is bool ghesSupport && ghesSupport),
                    Draft = _dialect.ConvertBooleanParameter(ConvertBoolToBool(parsed.FrontMatter, "draft")),
                    Plans = parsed.Plans.Count > 0 ? string.Join(",", parsed.Plans) : null,
                    TagsCsv = tagsCsv,
                    ContentHash = parsed.ContentHash,
                    IsAi = _dialect.ConvertBooleanParameter(sectionBools.IsAi),
                    IsAzure = _dialect.ConvertBooleanParameter(sectionBools.IsAzure),
                    IsDotNet = _dialect.ConvertBooleanParameter(sectionBools.IsDotNet),
                    IsDevOps = _dialect.ConvertBooleanParameter(sectionBools.IsDevOps),
                    IsGitHubCopilot = _dialect.ConvertBooleanParameter(sectionBools.IsGitHubCopilot),
                    IsMl = _dialect.ConvertBooleanParameter(sectionBools.IsMl),
                    IsSecurity = _dialect.ConvertBooleanParameter(sectionBools.IsSecurity),
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
            var isDraft = ConvertBoolToBool(parsed.FrontMatter, "draft");

            if (!isDraft)
            {
                foreach (var tag in parsed.Tags)
                {
                    var tagTrimmed = tag.Trim();

                    // Calculate sections bitmask (Bit 0=AI, Bit 1=Azure, Bit 2=.NET, Bit 3=DevOps, Bit 4=GitHubCopilot, Bit 5=ML, Bit 6=Security)
                    var bitmask = (sectionInts.IsAi * 1) +
                                  (sectionInts.IsAzure * 2) +
                                  (sectionInts.IsDotNet * 4) +
                                  (sectionInts.IsDevOps * 8) +
                                  (sectionInts.IsGitHubCopilot * 16) +
                                  (sectionInts.IsMl * 32) +
                                  (sectionInts.IsSecurity * 64);

                    // Add the full tag (lowercase in tag_word for querying, original case in tag_display for display)
                    tagWords.Add(new TagWord(
                        parsed.CollectionName,
                        parsed.Slug,
                        tagTrimmed.ToLowerInvariant(), // Lowercase for efficient querying
                        tagTrimmed,                    // Original case preserved for display
                        1, // is_full_tag = true
                        parsed.DateEpoch,
                        sectionInts.IsAi,
                        sectionInts.IsAzure,
                        sectionInts.IsDotNet,
                        sectionInts.IsDevOps,
                        sectionInts.IsGitHubCopilot,
                        sectionInts.IsMl,
                        sectionInts.IsSecurity,
                        bitmask
                    ));

                    // Also expand tags into words for subset matching
                    var words = tag.Split(_tagSplitSeparators, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var word in words)
                    {
                        var wordTrimmed = word.Trim();

                        // Skip if it's the same as the full tag (already added above)
                        if (string.Equals(wordTrimmed, tagTrimmed, StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        tagWords.Add(new TagWord(
                            parsed.CollectionName,
                            parsed.Slug,
                            wordTrimmed.ToLowerInvariant(), // Lowercase for efficient querying
                            wordTrimmed,                   // Original casing from full tag (e.g., "GitHub" from "GitHub Copilot")
                            0, // is_full_tag = false (word expansion)
                            parsed.DateEpoch,
                            sectionInts.IsAi,
                            sectionInts.IsAzure,
                            sectionInts.IsDotNet,
                            sectionInts.IsDevOps,
                            sectionInts.IsGitHubCopilot,
                            sectionInts.IsMl,
                            sectionInts.IsSecurity,
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

    private static bool ConvertBoolToBool(Dictionary<string, object?> frontMatter, string key)
    {
        var value = frontMatter.GetValueOrDefault(key, false);

        // Handle various types YamlDotNet might return
        return value switch
        {
            bool b => b,
            string s => s.Equals("true", StringComparison.OrdinalIgnoreCase),
            int i => i != 0,
            _ => false
        };
    }

    private static Task DisableIndexesAndTriggersAsync()
    {
        // Index/trigger optimization is not needed for PostgreSQL bulk inserts
        return Task.CompletedTask;
    }

    private static Task EnableIndexesAndTriggersAsync()
    {
        // No index/trigger restoration needed for PostgreSQL
        return Task.CompletedTask;
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
    public int BulkOperationThreshold { get; set; } = 50;
}

/// <summary>
/// Configuration options for content paths.
/// </summary>
public class ContentOptions
{
    public required string CollectionsPath { get; set; }
}
