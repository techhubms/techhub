using System.Data;
using System.Text.Json;
using Dapper;
using Microsoft.Extensions.Logging;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// Queries <c>content_items</c> directly using section boolean columns and AI metadata
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

    public SectionRoundupRepository(IDbConnection connection, ILogger<SectionRoundupRepository> logger)
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
        // Expand each content item into one row per section it belongs to using the
        // denormalized boolean columns. Excludes roundups to prevent circular inclusion.
        const string Sql = @"
            SELECT
                s.section_name      AS SectionName,
                ci.title            AS Title,
                ci.external_url     AS ExternalUrl,
                ci.slug             AS Slug,
                ci.collection_name  AS CollectionName,
                ci.ai_metadata      AS AiMetadataJson
            FROM content_items ci
            CROSS JOIN LATERAL (VALUES
                ('ai',              ci.is_ai),
                ('azure',           ci.is_azure),
                ('dotnet',          ci.is_dotnet),
                ('devops',          ci.is_devops),
                ('github-copilot',  ci.is_github_copilot),
                ('ml',              ci.is_ml),
                ('security',        ci.is_security)
            ) AS s(section_name, is_member)
            WHERE s.is_member = TRUE
              AND ci.created_at >= @WeekStart
              AND ci.created_at < @WeekEndExclusive
              AND ci.ai_metadata IS NOT NULL
              AND ci.collection_name != 'roundups'
            ORDER BY s.section_name, ci.created_at";

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
            TimeSensitivity = meta?.TimeSensitivity ?? "this-week"
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
}
