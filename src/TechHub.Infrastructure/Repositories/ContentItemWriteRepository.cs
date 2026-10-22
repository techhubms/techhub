using System.Data;
using System.Text.Json;
using Dapper;
using Microsoft.Extensions.Logging;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// Write operations for content items used by the content processing pipeline.
/// Handles upserts into <c>content_items</c> and <c>content_tags_expanded</c>.
/// </summary>
public sealed class ContentItemWriteRepository : IContentItemWriteRepository
{
    private readonly IDbConnection _connection;
    private readonly ILogger<ContentItemWriteRepository> _logger;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public ContentItemWriteRepository(
        IDbConnection connection,
        ILogger<ContentItemWriteRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(logger);

        _connection = connection;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByExternalUrlAsync(string externalUrl, CancellationToken ct = default)
    {
        var exists = await _connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                "SELECT EXISTS(SELECT 1 FROM content_items WHERE external_url = @ExternalUrl)",
                new { ExternalUrl = externalUrl },
                cancellationToken: ct));
        return exists;
    }

    /// <inheritdoc />
    public async Task UpsertProcessedItemAsync(ProcessedContentItem item, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(item);

        using var transaction = BeginTransaction();
        try
        {
            await UpsertAsync(item, transaction, ct);
            await TimelineEventWriter.ReplaceAsync(
                _connection,
                transaction,
                item.CollectionName,
                item.Slug,
                new TimelineEventReplacement(item.TimelineEvents, ITimelineEventRepository.ExtractedFromSource, ITimelineEventRepository.CurrentPromptVersion),
                ct);
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    private IDbTransaction BeginTransaction()
    {
        if (_connection.State != ConnectionState.Open)
        {
            _connection.Open();
        }

        return _connection.BeginTransaction();
    }

    private async Task UpsertAsync(ProcessedContentItem item, IDbTransaction transaction, CancellationToken ct)
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

        // Ensure the collection-name tag (e.g. "News", "Blogs") is always present.
        var tags = new List<string>(item.Tags);
        var collectionTag = char.ToUpperInvariant(item.CollectionName[0]) + item.CollectionName[1..];
        if (!tags.Any(t => t.Equals(collectionTag, StringComparison.OrdinalIgnoreCase)))
        {
            tags.Add(collectionTag);
        }

        var tagsCsv = tags.Count > 0 ? $",{string.Join(",", tags)}," : string.Empty;

        var aiMetadataJson = item.RoundupMetadata != null
            ? SerializeAiMetadata(item.RoundupMetadata)
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
                PrimarySection = item.PrimarySectionName,
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
            transaction,
            cancellationToken: ct));

        // Rebuild expanded tags
        await _connection.ExecuteAsync(new CommandDefinition(
            "DELETE FROM content_tags_expanded WHERE collection_name = @Collection AND slug = @Slug",
            new { Collection = item.CollectionName, Slug = item.Slug },
            transaction,
            cancellationToken: ct));

        if (tags.Count > 0)
        {
            var tagRows = BuildTagWords(tags, item.CollectionName, item.Slug, item.DateEpoch,
                isAi, isAzure, isDotnet, isDevops, isGhc, isMl, isSecurity, bitmask);
            foreach (var row in tagRows)
            {
                await _connection.ExecuteAsync(new CommandDefinition(
                    @"INSERT INTO content_tags_expanded
                        (collection_name, slug, tag_word, tag_display, is_full_tag,
                         date_epoch, is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
                         is_ml, is_security, sections_bitmask)
                      VALUES
                        (@CollectionName, @Slug, @TagWord, @TagDisplay, @IsFullTag,
                         @DateEpoch, @IsAi, @IsAzure, @IsDotnet, @IsDevops, @IsGhc,
                         @IsMl, @IsSecurity, @Bitmask)
                      ON CONFLICT DO NOTHING",
                    row,
                    transaction,
                    cancellationToken: ct));
            }
        }

        // Populate subcollection lookup table for vscode-updates items
        if (string.Equals(item.SubcollectionName, "vscode-updates", StringComparison.OrdinalIgnoreCase))
        {
            await _connection.ExecuteAsync(new CommandDefinition(
                "INSERT INTO vscode_update_items (collection_name, slug) VALUES (@Collection, @Slug) ON CONFLICT DO NOTHING",
                new { Collection = item.CollectionName, Slug = item.Slug },
                transaction,
                cancellationToken: ct));
        }

        _logger.LogDebug("Upserted content item {Collection}/{Slug}", item.CollectionName, item.Slug);
    }

    /// <inheritdoc />
    public async Task UpdateAiMetadataAsync(string collectionName, string slug, string aiMetadataJson, CancellationToken ct = default)
    {
        await ExecuteUpdateAiMetadataAsync(collectionName, slug, aiMetadataJson, transaction: null, ct);
    }

    /// <inheritdoc />
    public async Task UpdateAiMetadataAsync(string collectionName, string slug, string aiMetadataJson, TimelineEventReplacement timeline, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(timeline);

        using var transaction = BeginTransaction();
        try
        {
            await ExecuteUpdateAiMetadataAsync(collectionName, slug, aiMetadataJson, transaction, ct);
            await TimelineEventWriter.ReplaceAsync(_connection, transaction, collectionName, slug, timeline, ct);
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    private async Task ExecuteUpdateAiMetadataAsync(string collectionName, string slug, string aiMetadataJson, IDbTransaction? transaction, CancellationToken ct)
    {
        const string Sql = @"
UPDATE content_items
   SET ai_metadata = @AiMetadata::jsonb,
       updated_at  = NOW()
 WHERE collection_name = @CollectionName
   AND slug           = @Slug";

        await _connection.ExecuteAsync(new CommandDefinition(
            Sql,
            new { CollectionName = collectionName, Slug = slug, AiMetadata = aiMetadataJson },
            transaction,
            cancellationToken: ct));
    }

    /// <summary>
    /// Serializes roundup metadata to the <c>ai_metadata</c> JSON: schema version 2 stores the classification output
    /// (key facts, significance, section roles, ...), version 1 the original summary-based fields.
    /// </summary>
    public static string SerializeAiMetadata(RoundupMetadata metadata)
    {
        ArgumentNullException.ThrowIfNull(metadata);

        if (metadata.SchemaVersion >= RoundupMetadata.CurrentSchemaVersion)
        {
            return JsonSerializer.Serialize(new
            {
                schema_version = metadata.SchemaVersion,
                key_facts = metadata.KeyFacts,
                significance = metadata.Significance,
                content_kind = metadata.ContentKind,
                lifecycle = metadata.Lifecycle,
                products = metadata.Products,
                section_roles = metadata.SectionRoles
            }, _jsonOptions);
        }

        return JsonSerializer.Serialize(new
        {
            roundup_summary = metadata.Summary,
            key_topics = metadata.KeyTopics,
            roundup_relevance = metadata.Relevance,
            topic_type = metadata.TopicType,
            impact_level = metadata.ImpactLevel,
            time_sensitivity = metadata.TimeSensitivity
        }, _jsonOptions);
    }

    internal static List<object> BuildTagWords(
        IReadOnlyList<string> tags, string collection, string slug, long dateEpoch,
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
                DateEpoch = dateEpoch,
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
            var words = tag.Split([' ', '-', '_'], StringSplitOptions.RemoveEmptyEntries);
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
}
