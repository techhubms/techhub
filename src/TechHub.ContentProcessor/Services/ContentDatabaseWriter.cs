using System.Data;
using System.Text;
using Dapper;
using TechHub.ContentProcessor.Models;
using TechHub.Core.Interfaces;

namespace TechHub.ContentProcessor.Services;

/// <summary>
/// Writes processed content items directly to the PostgreSQL database,
/// using the same schema as <see cref="TechHub.Infrastructure.Services.ContentSyncService"/>.
/// </summary>
public sealed class ContentDatabaseWriter
{
    private readonly IDbConnection _connection;
    private readonly ISqlDialect _dialect;
    private readonly ILogger<ContentDatabaseWriter> _logger;

    private static readonly char[] TagSplitSeparators = [' ', '-', '_'];

    public ContentDatabaseWriter(
        IDbConnection connection,
        ISqlDialect dialect,
        ILogger<ContentDatabaseWriter> logger)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(dialect);
        ArgumentNullException.ThrowIfNull(logger);

        _connection = connection;
        _dialect = dialect;
        _logger = logger;
    }

    /// <summary>
    /// Returns <see langword="true"/> if an item with the given <paramref name="externalUrl"/>
    /// already exists in the database.
    /// </summary>
    public async Task<bool> ExistsAsync(string externalUrl, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(externalUrl);

        var count = await _connection.ExecuteScalarAsync<int>(
            new Dapper.CommandDefinition(
                "SELECT COUNT(*) FROM content_items WHERE external_url = @ExternalUrl",
                new { ExternalUrl = externalUrl },
                cancellationToken: ct));

        return count > 0;
    }

    /// <summary>
    /// Inserts or updates a processed content item and its tag expansions in the database.
    /// Uses a single transaction that rolls back on any failure.
    /// </summary>
    public async Task WriteAsync(ProcessedContentItem item, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(item);

        _logger.LogDebug(
            "Writing item to database: {CollectionName}/{Slug} ({Url})",
            item.CollectionName, item.Slug, item.ExternalUrl);

        var isNew = !await ExistsAsync(item.ExternalUrl, ct);

        using var transaction = _connection.BeginTransaction();

        try
        {
            await UpsertContentItemAsync(item, transaction);

            // Delete existing tag expansions only on update (not on fresh insert)
            if (!isNew)
            {
                await _connection.ExecuteAsync(
                    "DELETE FROM content_tags_expanded WHERE collection_name = @CollectionName AND slug = @Slug",
                    new { item.CollectionName, item.Slug },
                    transaction);
            }

            await InsertTagExpansionsAsync(item, transaction, ct);

            transaction.Commit();

            _logger.LogInformation(
                "{Action} content item: {CollectionName}/{Slug}",
                isNew ? "Inserted" : "Updated", item.CollectionName, item.Slug);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Failed to write item {CollectionName}/{Slug}", item.CollectionName, item.Slug);
            throw;
        }
    }

    private async Task UpsertContentItemAsync(ProcessedContentItem item, IDbTransaction transaction)
    {
        var tagsCsv = item.Tags.Count > 0 ? $",{string.Join(",", item.Tags)}," : string.Empty;
        var sectionFlags = BuildSectionFlags(item.Sections);
        var sectionsBitmask = ComputeSectionsBitmask(sectionFlags);

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
                item.Slug,
                item.Title,
                item.Content,
                item.Excerpt,
                item.DateEpoch,
                item.CollectionName,
                item.SubcollectionName,
                item.PrimarySectionName,
                item.ExternalUrl,
                item.Author,
                item.FeedName,
                GhesSupport = _dialect.ConvertBooleanParameter(false),
                Draft = _dialect.ConvertBooleanParameter(false),
                Plans = (string?)null,
                TagsCsv = tagsCsv,
                item.ContentHash,
                IsAi = _dialect.ConvertBooleanParameter(sectionFlags.IsAi),
                IsAzure = _dialect.ConvertBooleanParameter(sectionFlags.IsAzure),
                IsDotNet = _dialect.ConvertBooleanParameter(sectionFlags.IsDotNet),
                IsDevOps = _dialect.ConvertBooleanParameter(sectionFlags.IsDevOps),
                IsGitHubCopilot = _dialect.ConvertBooleanParameter(sectionFlags.IsGitHubCopilot),
                IsMl = _dialect.ConvertBooleanParameter(sectionFlags.IsMl),
                IsSecurity = _dialect.ConvertBooleanParameter(sectionFlags.IsSecurity),
                SectionsBitmask = sectionsBitmask
            },
            transaction);
    }

    private async Task InsertTagExpansionsAsync(ProcessedContentItem item, IDbTransaction transaction, CancellationToken ct)
    {
        if (item.Tags.Count == 0)
        {
            return;
        }

        var sectionFlags = BuildSectionFlags(item.Sections);
        var sectionInts = (
            IsAi: sectionFlags.IsAi ? 1 : 0,
            IsAzure: sectionFlags.IsAzure ? 1 : 0,
            IsDotNet: sectionFlags.IsDotNet ? 1 : 0,
            IsDevOps: sectionFlags.IsDevOps ? 1 : 0,
            IsGitHubCopilot: sectionFlags.IsGitHubCopilot ? 1 : 0,
            IsMl: sectionFlags.IsMl ? 1 : 0,
            IsSecurity: sectionFlags.IsSecurity ? 1 : 0
        );

        var bitmask = ComputeSectionsBitmask(sectionFlags);
        var tagWords = BuildTagWords(item, sectionInts, bitmask);

        if (tagWords.Count == 0)
        {
            return;
        }

        const int TagsPerChunk = 70;

        for (int chunkStart = 0; chunkStart < tagWords.Count; chunkStart += TagsPerChunk)
        {
            var chunkSize = Math.Min(TagsPerChunk, tagWords.Count - chunkStart);
            var sb = new StringBuilder();

            sb.Append(_dialect.GetInsertIgnorePrefix(
                "content_tags_expanded",
                "(collection_name, slug, tag_word, tag_display, is_full_tag, date_epoch, is_ai, is_azure, is_dotnet, is_devops, is_github_copilot, is_ml, is_security, sections_bitmask)"));

            using var cmd = _connection.CreateCommand();
            cmd.Transaction = transaction;

            for (int tagIdx = 0; tagIdx < chunkSize; tagIdx++)
            {
                var tag = tagWords[chunkStart + tagIdx];

                if (tagIdx > 0)
                {
                    sb.Append(", ");
                }

                sb.Append(System.Globalization.CultureInfo.InvariantCulture,
                    $"(@cn{tagIdx}, @s{tagIdx}, @w{tagIdx}, @td{tagIdx}, @ft{tagIdx}, @de{tagIdx}, @ai{tagIdx}, @az{tagIdx}, @c{tagIdx}, @do{tagIdx}, @gc{tagIdx}, @ml{tagIdx}, @sec{tagIdx}, @bm{tagIdx})");

                AddParam(cmd, $"@cn{tagIdx}", tag.CollectionName);
                AddParam(cmd, $"@s{tagIdx}", tag.Slug);
                AddParam(cmd, $"@w{tagIdx}", tag.Word);
                AddParam(cmd, $"@td{tagIdx}", (object?)tag.TagDisplay ?? DBNull.Value);
                AddParam(cmd, $"@ft{tagIdx}", _dialect.ConvertBooleanParameter(tag.IsFullTag));
                AddParam(cmd, $"@de{tagIdx}", tag.DateEpoch);
                AddParam(cmd, $"@ai{tagIdx}", _dialect.ConvertBooleanParameter(tag.IsAi));
                AddParam(cmd, $"@az{tagIdx}", _dialect.ConvertBooleanParameter(tag.IsAzure));
                AddParam(cmd, $"@c{tagIdx}", _dialect.ConvertBooleanParameter(tag.IsDotNet));
                AddParam(cmd, $"@do{tagIdx}", _dialect.ConvertBooleanParameter(tag.IsDevOps));
                AddParam(cmd, $"@gc{tagIdx}", _dialect.ConvertBooleanParameter(tag.IsGitHubCopilot));
                AddParam(cmd, $"@ml{tagIdx}", _dialect.ConvertBooleanParameter(tag.IsMl));
                AddParam(cmd, $"@sec{tagIdx}", _dialect.ConvertBooleanParameter(tag.IsSecurity));
                AddParam(cmd, $"@bm{tagIdx}", tag.SectionsBitmask);
            }

            sb.Append(_dialect.GetInsertIgnoreSuffix());
            cmd.CommandText = sb.ToString();
            await ((System.Data.Common.DbCommand)cmd).ExecuteNonQueryAsync(ct);
        }
    }

    private List<TagWordRecord> BuildTagWords(
        ProcessedContentItem item,
        (int IsAi, int IsAzure, int IsDotNet, int IsDevOps, int IsGitHubCopilot, int IsMl, int IsSecurity) sectionInts,
        int bitmask)
    {
        var tagWords = new List<TagWordRecord>();

        foreach (var tag in item.Tags)
        {
            var tagTrimmed = tag.Trim();

            // Full tag (is_full_tag = true)
            tagWords.Add(new TagWordRecord(
                item.CollectionName,
                item.Slug,
                tagTrimmed.ToLowerInvariant(),
                tagTrimmed,
                true,
                item.DateEpoch,
                sectionInts.IsAi == 1,
                sectionInts.IsAzure == 1,
                sectionInts.IsDotNet == 1,
                sectionInts.IsDevOps == 1,
                sectionInts.IsGitHubCopilot == 1,
                sectionInts.IsMl == 1,
                sectionInts.IsSecurity == 1,
                bitmask));

            // Expand multi-word tags into individual words
            var words = tagTrimmed.Split(TagSplitSeparators, StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                var wordTrimmed = word.Trim();

                if (string.Equals(wordTrimmed, tagTrimmed, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                tagWords.Add(new TagWordRecord(
                    item.CollectionName,
                    item.Slug,
                    wordTrimmed.ToLowerInvariant(),
                    wordTrimmed,
                    false,
                    item.DateEpoch,
                    sectionInts.IsAi == 1,
                    sectionInts.IsAzure == 1,
                    sectionInts.IsDotNet == 1,
                    sectionInts.IsDevOps == 1,
                    sectionInts.IsGitHubCopilot == 1,
                    sectionInts.IsMl == 1,
                    sectionInts.IsSecurity == 1,
                    bitmask));
            }
        }

        return tagWords;
    }

    private static void AddParam(IDbCommand cmd, string name, object value)
    {
        var p = cmd.CreateParameter();
        p.ParameterName = name;
        p.Value = value;
        cmd.Parameters.Add(p);
    }

    private static (bool IsAi, bool IsAzure, bool IsDotNet, bool IsDevOps, bool IsGitHubCopilot, bool IsMl, bool IsSecurity)
        BuildSectionFlags(IReadOnlyList<string> sections)
    {
        return (
            sections.Contains("ai", StringComparer.OrdinalIgnoreCase),
            sections.Contains("azure", StringComparer.OrdinalIgnoreCase),
            sections.Contains("dotnet", StringComparer.OrdinalIgnoreCase),
            sections.Contains("devops", StringComparer.OrdinalIgnoreCase),
            sections.Contains("github-copilot", StringComparer.OrdinalIgnoreCase),
            sections.Contains("ml", StringComparer.OrdinalIgnoreCase),
            sections.Contains("security", StringComparer.OrdinalIgnoreCase)
        );
    }

    private static int ComputeSectionsBitmask(
        (bool IsAi, bool IsAzure, bool IsDotNet, bool IsDevOps, bool IsGitHubCopilot, bool IsMl, bool IsSecurity) flags)
    {
        return (flags.IsAi ? 1 : 0)
            + (flags.IsAzure ? 2 : 0)
            + (flags.IsDotNet ? 4 : 0)
            + (flags.IsDevOps ? 8 : 0)
            + (flags.IsGitHubCopilot ? 16 : 0)
            + (flags.IsMl ? 32 : 0)
            + (flags.IsSecurity ? 64 : 0);
    }

    private sealed record TagWordRecord(
        string CollectionName,
        string Slug,
        string Word,
        string? TagDisplay,
        bool IsFullTag,
        long DateEpoch,
        bool IsAi,
        bool IsAzure,
        bool IsDotNet,
        bool IsDevOps,
        bool IsGitHubCopilot,
        bool IsMl,
        bool IsSecurity,
        int SectionsBitmask);
}
