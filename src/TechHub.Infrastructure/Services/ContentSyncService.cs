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
    private readonly string _collectionsPath;
    
    public ContentSyncService(
        IDbConnection connection,
        IMarkdownService markdownService,
        ILogger<ContentSyncService> logger,
        IOptions<ContentSyncOptions> options,
        IOptions<ContentOptions> contentOptions)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _markdownService = markdownService ?? throw new ArgumentNullException(nameof(markdownService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _collectionsPath = contentOptions?.Value.CollectionsPath ?? throw new ArgumentNullException(nameof(contentOptions));
    }
    
    public async Task<SyncResult> SyncAsync(CancellationToken ct = default)
    {
        if (!_options.Enabled)
        {
            _logger.LogInformation("Content sync disabled (ContentSync:Enabled = false)");
            return new SyncResult(0, 0, 0, 0, TimeSpan.Zero);
        }
        
        if (_options.ForceFullSync)
        {
            return await ForceSyncAsync(ct);
        }
        
        return await IncrementalSyncAsync(ct);
    }
    
    public async Task<SyncResult> ForceSyncAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("Starting FULL content sync (deleting all existing content)...");
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            // Start transaction
            var transaction = _connection.BeginTransaction();
            
            try
            {
                // Delete all existing content (cascade deletes junction tables)
                await _connection.ExecuteAsync("DELETE FROM content_items", transaction: transaction);
                _logger.LogInformation("Deleted all existing content");
                
                // Load and insert all content
                var (added, updated, deleted, unchanged) = await SyncContentAsync(isFullSync: true, ct);
                
                // Update sync metadata
                await UpdateSyncMetadataAsync(added, transaction, ct);
                
                transaction.Commit();
                stopwatch.Stop();
                
                _logger.LogInformation(
                    "Full sync complete: {Added} added, {Duration:N0}ms",
                    added, stopwatch.ElapsedMilliseconds);
                
                return new SyncResult(added, 0, 0, 0, stopwatch.Elapsed);
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Full sync failed - transaction rolled back");
            throw;
        }
    }
    
    public async Task<bool> IsContentChangedAsync(CancellationToken ct = default)
    {
        var markdownFiles = GetMarkdownFiles();
        var fileHashes = await ComputeFileHashesAsync(markdownFiles, ct);
        var dbHashes = await GetDatabaseHashesAsync(ct);
        
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
            var transaction = _connection.BeginTransaction();
            
            try
            {
                var (added, updated, deleted, unchanged) = await SyncContentAsync(isFullSync: false, ct);
                
                await UpdateSyncMetadataAsync(added + updated, transaction, ct);
                
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
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Incremental sync failed - transaction rolled back");
            throw;
        }
    }
    
    private async Task<(int added, int updated, int deleted, int unchanged)> SyncContentAsync(
        bool isFullSync,
        CancellationToken ct)
    {
        // Get all markdown files
        var markdownFilesList = GetMarkdownFiles();
        _logger.LogInformation("Found {Count} markdown files", markdownFilesList.Count);
        
        // Create dictionary for file lookup by slug
        var markdownFiles = markdownFilesList.ToDictionary(f => GetContentSlugFromFile(f), f => f);
        
        // Compute hashes for all files
        var fileHashes = await ComputeFileHashesAsync(markdownFilesList, ct);
        
        // Get existing hashes from database
        var dbHashes = await GetDatabaseHashesAsync(ct);
        
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
        
        // Process deletions
        foreach (var compositeId in deletedFiles)
        {
            var parts = compositeId.Split(':');
            var collectionName = parts[0];
            var slug = parts[1];
            await _connection.ExecuteAsync(
                "DELETE FROM content_items WHERE collection_name = @CollectionName AND slug = @Slug",
                new { CollectionName = collectionName, Slug = slug });
        }
        
        // Process new and updated files sequentially to avoid SQLite thread-safety issues
        // SQLite does not support concurrent writes even with WAL mode in all configurations
        var filesToProcess = newFiles.Concat(potentiallyChanged).ToList();
        
        foreach (var id in filesToProcess)
        {
            await ProcessFileAsync(id, markdownFiles[id], fileHashes[id], ct);
        }
        
        return (newFiles.Count, potentiallyChanged.Count, deletedFiles.Count, unchanged.Count);
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
    
    private async Task<Dictionary<string, string>> ComputeFileHashesAsync(
        List<FileInfo> files,
        CancellationToken ct)
    {
        var hashes = new Dictionary<string, string>();
        
        foreach (var file in files)
        {
            var slug = GetContentSlugFromFile(file);
            var content = await File.ReadAllTextAsync(file.FullName, ct);
            var hash = ComputeSha256Hash(content);
            hashes[slug] = hash;
        }
        
        return hashes;
    }
    
    private async Task<Dictionary<string, string>> GetDatabaseHashesAsync(CancellationToken ct)
    {
        var rows = await _connection.QueryAsync<(string CollectionName, string Slug, string ContentHash)>(
            "SELECT collection_name, slug, content_hash FROM content_items");
        
        // Use composite key "collection:slug" to allow same slug in different collections
        return rows.ToDictionary(r => $"{r.CollectionName}:{r.Slug}", r => r.ContentHash);
    }
    
    private async Task ProcessFileAsync(
        string compositeId,
        FileInfo file,
        string contentHash,
        CancellationToken ct)
    {
        try
        {
            // Extract collection and slug from composite key "collection:slug"
            var parts = compositeId.Split(':');
            var collectionName = parts[0];
            var slug = parts[1];
            
            // Parse frontmatter and content
            var frontMatter = await _markdownService.ParseFrontMatterAsync(file.FullName, ct);
            var fileContent = await File.ReadAllTextAsync(file.FullName, ct);
            
            // Extract content and excerpt
            var (excerpt, content) = ExtractExcerptAndContent(fileContent);
            
            // Extract metadata from frontmatter and file path
            var subcollectionName = GetSubcollectionNameFromPath(file);
            var dateEpoch = GetDateEpochFromFrontMatter(frontMatter, file.Name);
            var sections = GetSectionsFromFrontMatter(frontMatter);
            var tags = GetTagsFromFrontMatter(frontMatter);
            var plans = GetPlansFromFrontMatter(frontMatter);
            
            // Upsert main content item
            await _connection.ExecuteAsync(@"
                INSERT INTO content_items (
                    slug, title, content, excerpt, date_epoch, collection_name, subcollection_name,
                    primary_section_name, external_url, author, feed_name, ghes_support, draft, content_hash,
                    created_at, updated_at
                ) VALUES (
                    @Slug, @Title, @Content, @Excerpt, @DateEpoch, @CollectionName, @SubcollectionName,
                    @PrimarySectionName, @ExternalUrl, @Author, @FeedName, @GhesSupport, @Draft, @ContentHash,
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
                    content_hash = @ContentHash,
                    updated_at = CURRENT_TIMESTAMP",
                new
                {
                    Slug = slug,
                    Title = frontMatter.GetValueOrDefault("title", "")?.ToString() ?? "",
                    Content = content,
                    Excerpt = excerpt,
                    DateEpoch = dateEpoch,
                    CollectionName = collectionName,
                    SubcollectionName = subcollectionName,
                    PrimarySectionName = frontMatter.GetValueOrDefault("primary_section", null)?.ToString() ?? sections.FirstOrDefault() ?? "all",
                    ExternalUrl = frontMatter.GetValueOrDefault("canonical_url", null)?.ToString(),
                    Author = frontMatter.GetValueOrDefault("author", null)?.ToString(),
                    FeedName = frontMatter.GetValueOrDefault("feed", null)?.ToString(),
                    GhesSupport = (frontMatter.GetValueOrDefault("ghes_support", false) is bool ghesSupport && ghesSupport) ? 1 : 0,
                    Draft = ConvertBoolToInt(frontMatter, "draft"),
                    ContentHash = contentHash
                });
            
            // Delete existing junction table entries
            await _connection.ExecuteAsync("DELETE FROM content_tags WHERE collection_name = @CollectionName AND slug = @Slug", new { Slug = slug, CollectionName = collectionName });
            await _connection.ExecuteAsync("DELETE FROM content_tags_expanded WHERE collection_name = @CollectionName AND slug = @Slug", new { Slug = slug, CollectionName = collectionName });
            await _connection.ExecuteAsync("DELETE FROM content_sections WHERE collection_name = @CollectionName AND slug = @Slug", new { Slug = slug, CollectionName = collectionName });
            await _connection.ExecuteAsync("DELETE FROM content_plans WHERE collection_name = @CollectionName AND slug = @Slug", new { Slug = slug, CollectionName = collectionName });
            
            // Insert tags and expanded tags
            foreach (var tag in tags)
            {
                var tagNormalized = tag.ToLowerInvariant().Trim();
                await _connection.ExecuteAsync(
                    "INSERT INTO content_tags (collection_name, slug, tag, tag_normalized) VALUES (@CollectionName, @Slug, @Tag, @TagNormalized)",
                    new { CollectionName = collectionName, Slug = slug, Tag = tag, TagNormalized = tagNormalized });
                
                // Expand tags into words for subset matching
                var words = tag.Split(new[] { ' ', '-', '_' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in words)
                {
                    var wordNormalized = word.ToLowerInvariant().Trim();
                    await _connection.ExecuteAsync(
                        "INSERT OR IGNORE INTO content_tags_expanded (collection_name, slug, tag_word) VALUES (@CollectionName, @Slug, @Word)",
                        new { CollectionName = collectionName, Slug = slug, Word = wordNormalized });
                }
            }
            
            // Insert sections
            foreach (var section in sections)
            {
                await _connection.ExecuteAsync(
                    "INSERT INTO content_sections (collection_name, slug, section_name) VALUES (@CollectionName, @Slug, @Section)",
                    new { CollectionName = collectionName, Slug = slug, Section = section });
            }
            
            // Insert plans
            foreach (var plan in plans)
            {
                await _connection.ExecuteAsync(
                    "INSERT INTO content_plans (collection_name, slug, plan_name) VALUES (@CollectionName, @Slug, @Plan)",
                    new { CollectionName = collectionName, Slug = slug, Plan = plan });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process file {FilePath}", file.FullName);
            throw;
        }
    }
    
    private static string GetContentSlugFromFile(FileInfo file)
    {
        // Composite key: "collection:slug" to allow same slug in different collections
        var collectionName = GetCollectionNameFromPath(file);
        var slug = Path.GetFileNameWithoutExtension(file.Name);
        return $"{collectionName}:{slug}";
    }
    
    private static string GetCollectionNameFromPath(FileInfo file)
    {
        // Extract collection from path: collections/_videos/ghc-features/file.md → ghc-features
        // or collections/_blogs/file.md → blogs
        var pathParts = file.DirectoryName?.Split(Path.DirectorySeparatorChar) ?? [];
        
        // Find the collection directory (starts with _)
        var collectionDir = pathParts.FirstOrDefault(p => p.StartsWith('_'));
        if (collectionDir == null)
            throw new InvalidOperationException($"Cannot determine collection from path: {file.FullName}");
        
        // Check if there's a subfolder (alt-collection)
        var collectionIndex = Array.IndexOf(pathParts, collectionDir);
        if (collectionIndex < pathParts.Length - 1)
        {
            // Has subfolder - use subfolder name as collection
            var subfolder = pathParts[collectionIndex + 1];
            if (!string.IsNullOrEmpty(subfolder) && subfolder != file.Name)
            {
                return subfolder; // e.g., "ghc-features"
            }
        }
        
        // No subfolder - use main collection name without underscore
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
    
    private static long GetDateEpochFromFrontMatter(Dictionary<string, object> frontMatter, string fileName)
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
    
    private static List<string> GetSectionsFromFrontMatter(Dictionary<string, object> frontMatter)
    {
        if (frontMatter.TryGetValue("section_names", out var value) && value is IEnumerable<object> list)
        {
            return list.Select(v => v.ToString() ?? "").Where(s => !string.IsNullOrEmpty(s)).ToList();
        }
        
        return ["all"]; // Default section
    }
    
    private static List<string> GetTagsFromFrontMatter(Dictionary<string, object> frontMatter)
    {
        if (frontMatter.TryGetValue("tags", out var value) && value is IEnumerable<object> list)
        {
            return list.Select(v => v.ToString() ?? "").Where(s => !string.IsNullOrEmpty(s)).ToList();
        }
        
        return [];
    }
    
    private static List<string> GetPlansFromFrontMatter(Dictionary<string, object> frontMatter)
    {
        if (frontMatter.TryGetValue("copilot_plans", out var value) && value is IEnumerable<object> list)
        {
            return list.Select(v => v.ToString() ?? "").Where(s => !string.IsNullOrEmpty(s)).ToList();
        }
        
        return [];
    }
    
    private static (string excerpt, string content) ExtractExcerptAndContent(string fileContent)
    {
        // Remove frontmatter
        var lines = fileContent.Split('\n');
        var inFrontMatter = false;
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
        var words = body.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
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
    
    private async Task UpdateSyncMetadataAsync(int totalItems, IDbTransaction transaction, CancellationToken ct)
    {
        await _connection.ExecuteAsync(@"
            INSERT INTO sync_metadata (key, value, updated_at) VALUES (@Key, @Value, CURRENT_TIMESTAMP)
            ON CONFLICT(key) DO UPDATE SET value = @Value, updated_at = CURRENT_TIMESTAMP",
            new { Key = "last_sync", Value = DateTime.UtcNow.ToString("O") },
            transaction: transaction);
        
        await _connection.ExecuteAsync(@"
            INSERT INTO sync_metadata (key, value, updated_at) VALUES (@Key, @Value, CURRENT_TIMESTAMP)
            ON CONFLICT(key) DO UPDATE SET value = @Value, updated_at = CURRENT_TIMESTAMP",
            new { Key = "total_items", Value = totalItems.ToString() },
            transaction: transaction);
    }

    private static int ConvertBoolToInt(Dictionary<string, object> frontMatter, string key)
    {
        var value = frontMatter.GetValueOrDefault(key, false);
        return (value is bool b && b) ? 1 : 0;
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
