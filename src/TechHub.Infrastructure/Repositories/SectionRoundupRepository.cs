using System.Data;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Dapper;
using Microsoft.Extensions.Logging;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// Queries <c>content_items</c> using <c>primary_section_name</c> and AI metadata
/// to retrieve article candidates for weekly roundup generation.
/// </summary>
public sealed class SectionRoundupRepository : ISectionRoundupRepository
{
    private const int AiBit = 1 << 0;
    private const int AzureBit = 1 << 1;
    private const int DotnetBit = 1 << 2;
    private const int DevopsBit = 1 << 3;
    private const int GhcBit = 1 << 4;
    private const int MlBit = 1 << 5;
    private const int SecurityBit = 1 << 6;
    private static readonly Dictionary<string, int> _sectionBits = new(StringComparer.OrdinalIgnoreCase)
    {
        ["ai"] = AiBit,
        ["azure"] = AzureBit,
        ["dotnet"] = DotnetBit,
        ["devops"] = DevopsBit,
        ["github-copilot"] = GhcBit,
        ["ml"] = MlBit,
        ["security"] = SecurityBit
    };

    private readonly IDbConnection _connection;
    private readonly ILogger<SectionRoundupRepository> _logger;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    private static readonly HashSet<string> _internalCollections = new(StringComparer.OrdinalIgnoreCase)
    {
        "videos", "roundups", "custom"
    };

    public SectionRoundupRepository(
        IDbConnection connection,
        ILogger<SectionRoundupRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(logger);

        _connection = connection;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<RoundupArticle>> GetArticlesForSectionForWeekAsync(
        string sectionName,
        DateOnly weekStart,
        DateOnly weekEnd,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sectionName);

        var normalizedSection = sectionName.ToLowerInvariant().Trim();
        if (!_sectionBits.TryGetValue(normalizedSection, out var sectionBit))
        {
            return [];
        }

        const string Sql = """
            SELECT
                @SectionName            AS SectionName,
                ci.primary_section_name AS PrimarySectionName,
                ci.title                AS Title,
                ci.external_url         AS ExternalUrl,
                ci.slug                 AS Slug,
                ci.collection_name      AS CollectionName,
                ci.ai_metadata          AS AiMetadataJson,
                ci.excerpt              AS Excerpt,
                ci.content              AS Content,
                ci.feed_name            AS FeedName,
                ci.date_epoch           AS DateEpoch
            FROM content_items ci
            WHERE ci.created_at >= @WeekStart
              AND ci.created_at < @WeekEndExclusive
              AND ci.collection_name != 'roundups'
              AND (ci.sections_bitmask & @SectionBit) > 0
            ORDER BY ci.created_at DESC
            """;

        var rows = await _connection.QueryAsync<RoundupRow>(new CommandDefinition(
            Sql,
            new
            {
                SectionName = normalizedSection,
                SectionBit = sectionBit,
                WeekStart = weekStart.ToDateTime(TimeOnly.MinValue),
                WeekEndExclusive = weekEnd.AddDays(1).ToDateTime(TimeOnly.MinValue)
            },
            cancellationToken: ct));

        return rows
            .Select(ParseRow)
            .Where(a => a is not null)
            .Cast<RoundupArticle>()
            .ToList();
    }

    private RoundupArticle? ParseRow(RoundupRow row)
    {
        RoundupAiMetadata? meta = null;

        if (!string.IsNullOrWhiteSpace(row.AiMetadataJson))
        {
            try
            {
                meta = JsonSerializer.Deserialize<RoundupAiMetadata>(row.AiMetadataJson, _jsonOptions);
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex, "Failed to parse ai_metadata for {Collection}/{Slug}", row.CollectionName, row.Slug);
            }
        }

        var article = new RoundupArticle
        {
            SectionName = row.SectionName,
            PrimarySectionName = row.PrimarySectionName,
            Title = row.Title,
            ExternalUrl = row.ExternalUrl,
            Slug = row.Slug,
            CollectionName = row.CollectionName,
            IsInternal = _internalCollections.Contains(row.CollectionName),
            Summary = meta?.RoundupSummary ?? string.Empty,
            KeyTopics = meta?.KeyTopics ?? [],
            Relevance = meta?.RoundupRelevance ?? "medium",
            TopicType = meta?.TopicType ?? "news",
            ImpactLevel = meta?.ImpactLevel ?? "medium",
            TimeSensitivity = meta?.TimeSensitivity ?? "this-week",
            NeedsAiMetadata = meta is null || meta.SchemaVersion < RoundupMetadata.CurrentSchemaVersion,
            Excerpt = row.Excerpt,
            Content = row.Content,
            FeedName = row.FeedName,
            DateEpoch = row.DateEpoch
        };

        if (meta is not null && meta.SchemaVersion >= RoundupMetadata.CurrentSchemaVersion)
        {
            article = article.WithMetadata(new RoundupMetadata
            {
                SchemaVersion = meta.SchemaVersion,
                KeyFacts = meta.KeyFacts,
                Significance = meta.Significance,
                ContentKind = meta.ContentKind,
                Lifecycle = meta.Lifecycle,
                Products = meta.Products,
                SectionRoles = meta.SectionRoles
            });
        }

        return article;
    }

    // ── Private DTOs ──────────────────────────────────────────────────────────

    private sealed class RoundupRow
    {
        public string SectionName { get; init; } = string.Empty;
        public string PrimarySectionName { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string ExternalUrl { get; init; } = string.Empty;
        public string Slug { get; init; } = string.Empty;
        public string CollectionName { get; init; } = string.Empty;
        public string? AiMetadataJson { get; init; }
        public string Excerpt { get; init; } = string.Empty;
        public string Content { get; init; } = string.Empty;
        public string FeedName { get; init; } = string.Empty;
        public long DateEpoch { get; init; }
    }

    private sealed class RoundupAiMetadata
    {
        public int SchemaVersion { get; init; } = 1;
        public IReadOnlyList<string> KeyFacts { get; init; } = [];
        public string Significance { get; init; } = "medium";
        public string ContentKind { get; init; } = "announcement";
        public string Lifecycle { get; init; } = "none";
        public IReadOnlyList<string> Products { get; init; } = [];
        public Dictionary<string, string> SectionRoles { get; init; } = [];
        public string RoundupSummary { get; init; } = string.Empty;
        public IReadOnlyList<string> KeyTopics { get; init; } = [];
        public string RoundupRelevance { get; init; } = "medium";
        public string TopicType { get; init; } = "news";
        public string ImpactLevel { get; init; } = "medium";
        public string TimeSensitivity { get; init; } = "this-week";
    }

    // ── ISectionRoundupRepository additional methods ──────────────────────────

    /// <inheritdoc />
    public async Task<IReadOnlyList<RoundupDateEvent>> GetUpcomingDateEventsAsync(
        string sectionName,
        DateOnly weekStart,
        DateOnly weekEnd,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sectionName);

        if (!_sectionBits.TryGetValue(sectionName.ToLowerInvariant().Trim(), out var sectionBit))
        {
            return [];
        }

        const string Sql = """
            SELECT e.kind           AS Kind,
                   e.product        AS Product,
                   e.feature        AS Feature,
                   e.description    AS Description,
                   e.event_date     AS EventDate,
                   e.date_precision AS DatePrecision,
                   ci.title         AS SourceTitle,
                   ci.external_url  AS SourceUrl
            FROM content_timeline_events e
            JOIN content_items ci
              ON ci.collection_name = e.collection_name AND ci.slug = e.item_slug
            WHERE ci.created_at >= @WeekStart
              AND ci.created_at < @WeekEndExclusive
              AND ci.collection_name != 'roundups'
              AND (ci.sections_bitmask & @SectionBit) > 0
              AND e.event_date > @WeekEnd
            ORDER BY e.event_date, e.id
            """;

        var rows = await _connection.QueryAsync<DateEventRow>(new CommandDefinition(
            Sql,
            new
            {
                WeekStart = weekStart.ToDateTime(TimeOnly.MinValue),
                WeekEndExclusive = weekEnd.AddDays(1).ToDateTime(TimeOnly.MinValue),
                WeekEnd = weekEnd.ToDateTime(TimeOnly.MinValue),
                SectionBit = sectionBit
            },
            cancellationToken: ct));

        return rows.Select(r => new RoundupDateEvent
        {
            Kind = r.Kind,
            Product = r.Product,
            Feature = r.Feature,
            Description = r.Description,
            Date = DateOnly.FromDateTime(r.EventDate),
            DatePrecision = r.DatePrecision,
            SourceTitle = r.SourceTitle,
            SourceUrl = r.SourceUrl
        }).ToList();
    }

    private sealed class DateEventRow
    {
        public string Kind { get; init; } = string.Empty;
        public string Product { get; init; } = string.Empty;
        public string? Feature { get; init; }
        public string Description { get; init; } = string.Empty;
        public DateTime EventDate { get; init; }
        public string DatePrecision { get; init; } = string.Empty;
        public string SourceTitle { get; init; } = string.Empty;
        public string SourceUrl { get; init; } = string.Empty;
    }

    /// <inheritdoc />
    public async Task<bool> RoundupExistsAsync(string sectionName, string slug, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sectionName);
        sectionName = sectionName.ToLowerInvariant().Trim();
        var count = await _connection.ExecuteScalarAsync<int>(new CommandDefinition(
            @"SELECT COUNT(*)
              FROM content_items
              WHERE collection_name = 'roundups'
                AND slug = @Slug
                AND primary_section_name = @SectionName",
            new
            {
                Slug = slug,
                SectionName = sectionName
            },
            cancellationToken: ct));

        return count > 0;
    }

    /// <inheritdoc />
    public async Task<string?> GetPreviousRoundupContentAsync(string sectionName, DateOnly weekStart, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sectionName);
        sectionName = sectionName.ToLowerInvariant().Trim();
        var brusselsZone = TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows() ? "Romance Standard Time" : "Europe/Brussels");
        // Roundups are stamped 09:00 on the Monday they are published. The roundup that precedes the week starting
        // on weekStart was published on that very Monday, so the cutoff is the start of the next day.
        var cutoffDt = weekStart.AddDays(1).ToDateTime(TimeOnly.MinValue);
        var weekStartEpoch = (long)TimeZoneInfo.ConvertTimeToUtc(cutoffDt, brusselsZone)
            .Subtract(DateTime.UnixEpoch).TotalSeconds;

        var content = await _connection.QueryFirstOrDefaultAsync<string>(new CommandDefinition(
            @"SELECT content
              FROM content_items
              WHERE collection_name = 'roundups'
                AND primary_section_name = @SectionName
                AND date_epoch < @WeekStartEpoch
              ORDER BY date_epoch DESC
              LIMIT 1",
            new
            {
                SectionName = sectionName,
                WeekStartEpoch = weekStartEpoch
            },
            cancellationToken: ct));

        return content;
    }

    /// <inheritdoc />
    public async Task WriteRoundupAsync(
        string sectionName,
        string slug,
        DateOnly publishDate,
        string title,
        string content,
        string introduction,
        IReadOnlyList<string> tags,
        long? jobId = null,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sectionName);
        ArgumentNullException.ThrowIfNull(tags);
        sectionName = sectionName.ToLowerInvariant().Trim();

        var brusselsZone = TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows() ? "Romance Standard Time" : "Europe/Brussels");

        // Publish at 9:00 AM Brussels time on publish date.
        var publishDt = publishDate.ToDateTime(new TimeOnly(9, 0, 0));
        var publishUtc = TimeZoneInfo.ConvertTimeToUtc(publishDt, brusselsZone);
        var dateEpoch = (long)publishUtc.Subtract(DateTime.UnixEpoch).TotalSeconds;

        var externalUrl = string.Create(CultureInfo.InvariantCulture, $"/{sectionName}/roundups/{slug}");
        var sectionFlags = BuildSectionFlags(sectionName);

        // Ensure collection-name tag is always present.
        const string CollectionTag = "Roundups";
        var allTags = tags.Any(t => t.Equals(CollectionTag, StringComparison.OrdinalIgnoreCase))
            ? tags
            : tags.Concat([CollectionTag]).ToList();

        var tagsCsv = allTags.Count > 0
            ? string.Create(CultureInfo.InvariantCulture, $",{string.Join(",", allTags)},")
            : string.Empty;

        var computedHash = Convert.ToHexString(
            SHA256.HashData(Encoding.UTF8.GetBytes(content)));

        if (_connection.State != ConnectionState.Open)
        {
            _connection.Open();
        }

        using var transaction = _connection.BeginTransaction();

        try
        {
            await _connection.ExecuteAsync(new CommandDefinition(
                @"INSERT INTO content_items
                    (slug, collection_name, title, content, excerpt, date_epoch,
                     primary_section_name, external_url, author, feed_name,
                     tags_csv, is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
                      is_ml, is_security, sections_bitmask, content_hash)
                   VALUES
                    (@Slug, 'roundups', @Title, @Content, @Excerpt, @DateEpoch,
                     @SectionName, @ExternalUrl, 'TechHub', 'TechHub',
                     @TagsCsv, @IsAi, @IsAzure, @IsDotnet, @IsDevops, @IsGhc,
                     @IsMl, @IsSecurity, @Bitmask, @ContentHash)
                   ON CONFLICT (collection_name, slug) DO UPDATE SET
                    title            = EXCLUDED.title,
                    content          = EXCLUDED.content,
                    excerpt          = EXCLUDED.excerpt,
                    tags_csv         = EXCLUDED.tags_csv,
                    content_hash     = EXCLUDED.content_hash,
                    updated_at       = NOW()",
                new
                {
                    Slug = slug,
                    SectionName = sectionName,
                    Title = title,
                    Content = content,
                    Excerpt = introduction,
                    DateEpoch = dateEpoch,
                    ExternalUrl = externalUrl,
                    TagsCsv = tagsCsv,
                    IsAi = sectionFlags.IsAi,
                    IsAzure = sectionFlags.IsAzure,
                    IsDotnet = sectionFlags.IsDotnet,
                    IsDevops = sectionFlags.IsDevops,
                    IsGhc = sectionFlags.IsGhc,
                    IsMl = sectionFlags.IsMl,
                    IsSecurity = sectionFlags.IsSecurity,
                    Bitmask = sectionFlags.Bitmask,
                    ContentHash = computedHash
                },
                transaction: transaction,
                cancellationToken: ct));

            // Record in processed_urls atomically with the content_item insert.
            await _connection.ExecuteAsync(new CommandDefinition(
                @"INSERT INTO processed_urls (external_url, status, feed_name, collection_name, reason, job_id, slug)
                  VALUES (@ExternalUrl, 'succeeded', 'TechHub', 'roundups', 'roundup-generated', @JobId, @Slug)
                  ON CONFLICT (external_url) DO UPDATE SET
                    status          = 'succeeded',
                    error_message   = NULL,
                    feed_name       = 'TechHub',
                    collection_name = 'roundups',
                    reason          = 'roundup-generated',
                    job_id          = COALESCE(@JobId, processed_urls.job_id),
                    slug            = @Slug,
                    updated_at      = NOW()",
                new
                {
                    ExternalUrl = externalUrl,
                    JobId = jobId,
                    Slug = slug
                },
                transaction: transaction,
                cancellationToken: ct));

            // Rebuild expanded tags for the roundup (with word expansion for multi-word tags).
            await _connection.ExecuteAsync(new CommandDefinition(
                "DELETE FROM content_tags_expanded WHERE collection_name = 'roundups' AND slug = @Slug",
                new { Slug = slug },
                transaction: transaction,
                cancellationToken: ct));

            if (allTags.Count > 0)
            {
                var tagRows = ContentItemWriteRepository.BuildTagWords(
                    allTags, "roundups", slug, dateEpoch,
                    isAi: sectionFlags.IsAi,
                    isAzure: sectionFlags.IsAzure,
                    isDotnet: sectionFlags.IsDotnet,
                    isDevops: sectionFlags.IsDevops,
                    isGhc: sectionFlags.IsGhc,
                    isMl: sectionFlags.IsMl,
                    isSecurity: sectionFlags.IsSecurity,
                    bitmask: sectionFlags.Bitmask);

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
                        transaction: transaction,
                        cancellationToken: ct));
                }
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    private static SectionFlags BuildSectionFlags(string sectionName)
    {
        var normalized = sectionName.ToLowerInvariant();
        var bitmask = _sectionBits.TryGetValue(normalized, out var sectionBit) ? sectionBit : 0;
        var isAi = (bitmask & AiBit) != 0;
        var isAzure = (bitmask & AzureBit) != 0;
        var isDotnet = (bitmask & DotnetBit) != 0;
        var isDevops = (bitmask & DevopsBit) != 0;
        var isGhc = (bitmask & GhcBit) != 0;
        var isMl = (bitmask & MlBit) != 0;
        var isSecurity = (bitmask & SecurityBit) != 0;

        return new SectionFlags(isAi, isAzure, isDotnet, isDevops, isGhc, isMl, isSecurity, bitmask);
    }

    private sealed record SectionFlags(
        bool IsAi,
        bool IsAzure,
        bool IsDotnet,
        bool IsDevops,
        bool IsGhc,
        bool IsMl,
        bool IsSecurity,
        int Bitmask);
}
