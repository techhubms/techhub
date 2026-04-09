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
    public async Task<IReadOnlyDictionary<string, IReadOnlyList<RoundupArticle>>> GetArticlesForWeekAsync(
        DateOnly weekStart,
        DateOnly weekEnd,
        CancellationToken ct = default)
    {
        // Use primary_section_name so each article appears in exactly one section.
        // This prevents the same article from being counted multiple times and gives
        // the AI a clearer signal of what content belongs where.
        const string Sql = @"
            SELECT
                ci.primary_section_name AS SectionName,
                ci.title                AS Title,
                ci.external_url         AS ExternalUrl,
                ci.slug                 AS Slug,
                ci.collection_name      AS CollectionName,
                ci.ai_metadata          AS AiMetadataJson,
                ci.content              AS Content,
                ci.feed_name            AS FeedName,
                ci.date_epoch           AS DateEpoch
            FROM content_items ci
            WHERE ci.created_at >= @WeekStart
              AND ci.created_at < @WeekEndExclusive
              AND ci.collection_name != 'roundups'
              AND ci.primary_section_name IS NOT NULL
              AND ci.primary_section_name != 'all'
            ORDER BY ci.primary_section_name, ci.created_at";

        // WeekEnd is Sunday (inclusive) — add 1 day for exclusive upper bound.
        var rows = await _connection.QueryAsync<RoundupRow>(new CommandDefinition(
            Sql,
            new
            {
                WeekStart = weekStart.ToDateTime(TimeOnly.MinValue),
                WeekEndExclusive = weekEnd.AddDays(1).ToDateTime(TimeOnly.MinValue)
            },
            cancellationToken: ct));

        var grouped = new Dictionary<string, List<RoundupArticle>>(StringComparer.OrdinalIgnoreCase);

        foreach (var row in rows)
        {
            var article = ParseRow(row);
            if (article is null)
            {
                continue;
            }

            if (!grouped.TryGetValue(row.SectionName, out var list))
            {
                list = [];
                grouped[row.SectionName] = list;
            }

            list.Add(article);
        }

        _logger.LogInformation(
            "Loaded roundup articles for week {WeekStart}–{WeekEnd}: {SectionCount} sections, {ArticleCount} total",
            weekStart, weekEnd,
            grouped.Count,
            grouped.Values.Sum(l => l.Count));

        return grouped.ToDictionary(
            kvp => kvp.Key,
            kvp => (IReadOnlyList<RoundupArticle>)kvp.Value);
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

        return new RoundupArticle
        {
            SectionName = row.SectionName,
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
            NeedsAiMetadata = meta is null,
            Content = row.Content,
            FeedName = row.FeedName,
            DateEpoch = row.DateEpoch
        };
    }

    // ── Private DTOs ──────────────────────────────────────────────────────────

    private sealed class RoundupRow
    {
        public string SectionName { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string ExternalUrl { get; init; } = string.Empty;
        public string Slug { get; init; } = string.Empty;
        public string CollectionName { get; init; } = string.Empty;
        public string? AiMetadataJson { get; init; }
        public string Content { get; init; } = string.Empty;
        public string FeedName { get; init; } = string.Empty;
        public long DateEpoch { get; init; }
    }

    private sealed class RoundupAiMetadata
    {
        public string RoundupSummary { get; init; } = string.Empty;
        public IReadOnlyList<string> KeyTopics { get; init; } = [];
        public string RoundupRelevance { get; init; } = "medium";
        public string TopicType { get; init; } = "news";
        public string ImpactLevel { get; init; } = "medium";
        public string TimeSensitivity { get; init; } = "this-week";
    }

    // ── ISectionRoundupRepository additional methods ──────────────────────────

    /// <inheritdoc />
    public async Task<bool> RoundupExistsAsync(string slug, CancellationToken ct = default)
    {
        var count = await _connection.ExecuteScalarAsync<int>(new CommandDefinition(
            "SELECT COUNT(*) FROM content_items WHERE collection_name = 'roundups' AND slug = @Slug",
            new { Slug = slug },
            cancellationToken: ct));

        return count > 0;
    }

    /// <inheritdoc />
    public async Task<string?> GetPreviousRoundupContentAsync(DateOnly weekStart, CancellationToken ct = default)
    {
        var brusselsZone = TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows() ? "Romance Standard Time" : "Europe/Brussels");
        var weekStartDt = weekStart.ToDateTime(TimeOnly.MinValue);
        var weekStartEpoch = (long)TimeZoneInfo.ConvertTimeToUtc(weekStartDt, brusselsZone)
            .Subtract(DateTime.UnixEpoch).TotalSeconds;

        var content = await _connection.QueryFirstOrDefaultAsync<string>(new CommandDefinition(
            @"SELECT content
              FROM content_items
              WHERE collection_name = 'roundups'
                AND date_epoch < @WeekStartEpoch
              ORDER BY date_epoch DESC
              LIMIT 1",
            new { WeekStartEpoch = weekStartEpoch },
            cancellationToken: ct));

        return content;
    }

    /// <inheritdoc />
    public async Task WriteRoundupAsync(
        string slug,
        DateOnly publishDate,
        string title,
        string description,
        string content,
        string introduction,
        IReadOnlyList<string> tags,
        long? jobId = null,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(tags);

        var brusselsZone = TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows() ? "Romance Standard Time" : "Europe/Brussels");

        // Publish at 9:00 AM Brussels time on publish date.
        var publishDt = publishDate.ToDateTime(new TimeOnly(9, 0, 0));
        var publishUtc = TimeZoneInfo.ConvertTimeToUtc(publishDt, brusselsZone);
        var dateEpoch = (long)publishUtc.Subtract(DateTime.UnixEpoch).TotalSeconds;

        var externalUrl = string.Create(CultureInfo.InvariantCulture, $"/all/roundups/{slug}");

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

        var aiMetadataJson = JsonSerializer.Serialize(new
        {
            roundup_summary = description,
            key_topics = tags,
            roundup_relevance = "high",
            topic_type = "news"
        }, _jsonOptions);

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
                     is_ml, is_security, sections_bitmask, content_hash, ai_metadata)
                  VALUES
                    (@Slug, 'roundups', @Title, @Content, @Excerpt, @DateEpoch,
                     'github-copilot', @ExternalUrl, 'TechHub', 'TechHub',
                     @TagsCsv, TRUE, TRUE, TRUE, TRUE, TRUE,
                     TRUE, TRUE, 127, @ContentHash, @AiMetadata::jsonb)
                  ON CONFLICT (collection_name, slug) DO UPDATE SET
                    title            = EXCLUDED.title,
                    content          = EXCLUDED.content,
                    excerpt          = EXCLUDED.excerpt,
                    tags_csv         = EXCLUDED.tags_csv,
                    content_hash     = EXCLUDED.content_hash,
                    ai_metadata      = EXCLUDED.ai_metadata,
                    updated_at       = NOW()",
                new
                {
                    Slug = slug,
                    Title = title,
                    Content = content,
                    Excerpt = introduction,
                    DateEpoch = dateEpoch,
                    ExternalUrl = externalUrl,
                    TagsCsv = tagsCsv,
                    ContentHash = computedHash,
                    AiMetadata = aiMetadataJson
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
                    isAi: true, isAzure: true, isDotnet: true, isDevops: true,
                    isGhc: true, isMl: true, isSecurity: true, bitmask: 127);

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
}
