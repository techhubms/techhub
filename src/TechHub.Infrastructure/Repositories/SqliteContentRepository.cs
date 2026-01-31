using System.Data;
using System.Text;
using Dapper;
using Microsoft.Extensions.Caching.Memory;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// SQLite implementation of content repository using FTS5 for full-text search.
/// Uses Dapper for database operations and extends DatabaseContentRepositoryBase (which handles common SQL and caching).
/// </summary>
public class SqliteContentRepository : DatabaseContentRepositoryBase
{
    public SqliteContentRepository(
        IDbConnection connection,
        ISqlDialect dialect,
        IMemoryCache cache,
        IMarkdownService markdownService)
        : base(connection, dialect, cache, markdownService)
    {
    }

    /// <summary>
    /// Full-text search using SQLite FTS5.
    /// Supports query string, tag filtering, section filtering, and date filtering.
    /// Implementation called by base class cached wrapper.
    /// </summary>
    protected override async Task<SearchResults<ContentItem>> SearchInternalAsync(SearchRequest request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var sql = new StringBuilder();
        var parameters = new DynamicParameters();

        // Base SELECT - columns map directly to ContentItem model (excludes content for performance)
        var hasQuery = !string.IsNullOrWhiteSpace(request.Query);

        sql.Append($@"
            SELECT {ListViewColumns}");

        // NOTE: bm25() rank is used in ORDER BY, not in SELECT, to avoid Dapper mapping issues

        sql.Append("\n            FROM content_items c");

        // Join with FTS5 for full-text search if query is provided
        if (hasQuery)
        {
            sql.Append(@"
                INNER JOIN content_fts ON c.rowid = content_fts.rowid");
        }

        // WHERE clause
        var whereClauses = new List<string>
        {
            // Always exclude drafts in search
            "c.draft = 0"
        };

        // Full-text search
        if (hasQuery)
        {
            whereClauses.Add("content_fts MATCH @query");
            parameters.Add("query", request.Query);
        }

        // Tag filtering (AND logic - all selected tags must match)
        // Uses content_tags_expanded for word-level subset matching ("AI" matches "AI Engineering")
        if (request.Tags != null && request.Tags.Count > 0)
        {
            for (int i = 0; i < request.Tags.Count; i++)
            {
                whereClauses.Add($@"
                    EXISTS (
                        SELECT 1 FROM content_tags_expanded cte{i}
                        WHERE cte{i}.slug = c.slug
                        AND cte{i}.collection_name = c.collection_name
                        AND cte{i}.tag_word = @tag{i}
                    )");
                // Normalize tag to lowercase for case-insensitive matching
                parameters.Add($"tag{i}", request.Tags[i].ToLowerInvariant().Trim());
            }
        }

        // Section filtering using boolean columns (zero-join filtering)
        if (request.Sections != null && request.Sections.Count > 0)
        {
            var sectionConditions = new List<string>();
            foreach (var section in request.Sections)
            {
                var sectionNormalized = section.ToLowerInvariant().Trim();
                sectionConditions.Add(sectionNormalized switch
                {
                    "ai" => "c.is_ai = 1",
                    "azure" => "c.is_azure = 1",
                    "coding" => "c.is_coding = 1",
                    "devops" => "c.is_devops = 1",
                    "github-copilot" => "c.is_github_copilot = 1",
                    "ml" => "c.is_ml = 1",
                    "security" => "c.is_security = 1",
                    _ => "1=0" // Unknown section - no match
                });
            }
            whereClauses.Add($"({string.Join(" OR ", sectionConditions)})");
        }

        // Collection filtering (normalize to lowercase for case-insensitive matching)
        if (request.Collections != null && request.Collections.Count > 0)
        {
            whereClauses.Add("c.collection_name IN @collections");
            parameters.Add("collections", request.Collections.Select(c => c.ToLowerInvariant().Trim()).ToList());
        }

        // Date range filtering
        if (request.DateFrom.HasValue)
        {
            whereClauses.Add("c.date_epoch >= @fromDate");
            parameters.Add("fromDate", ((DateTimeOffset)request.DateFrom.Value).ToUnixTimeSeconds());
        }

        if (request.DateTo.HasValue)
        {
            whereClauses.Add("c.date_epoch <= @toDate");
            parameters.Add("toDate", ((DateTimeOffset)request.DateTo.Value).ToUnixTimeSeconds());
        }

        if (whereClauses.Count > 0)
        {
            sql.Append(" WHERE ");
            sql.Append(string.Join(" AND ", whereClauses));
        }

        // ORDER BY (FTS rank if search query, otherwise date descending)
        // bm25() returns negative values (lower = better match), so we order ascending
        if (hasQuery)
        {
            sql.Append(" ORDER BY bm25(content_fts)");
        }
        else
        {
            sql.Append(" ORDER BY c.date_epoch DESC");
        }

        // Pagination
        sql.Append(" LIMIT @take");
        parameters.Add("take", request.Take);

        // TODO: Implement continuation token-based pagination
        // For now, using simple offset (will be replaced with keyset pagination)

        var items = await Connection.QueryAsync<ContentItem>(
            new CommandDefinition(sql.ToString(), parameters, cancellationToken: ct));

        // Get total count (without LIMIT/OFFSET)
        var countSql = GetCountSql(request);
        var totalCount = await Connection.ExecuteScalarAsync<int>(
            new CommandDefinition(countSql, parameters, cancellationToken: ct));

        // Only compute facets if explicitly requested (expensive operation)
        FacetResults? facets = null;
        if (request.IncludeFacets)
        {
            facets = await GetFacetsAsync(new FacetRequest
            {
                Tags = request.Tags,
                Sections = request.Sections,
                Collections = request.Collections,
                DateFrom = request.DateFrom,
                DateTo = request.DateTo,
                FacetFields = ["tags", "collections", "sections"]
            }, ct);
        }

        return new SearchResults<ContentItem>
        {
            Items = [.. items],
            TotalCount = totalCount,
            Facets = facets,
            ContinuationToken = null // TODO: Implement continuation token
        };
    }

    /// <summary>
    /// Get facet counts for tags, collections, and sections within the filtered scope.
    /// Implementation called by base class cached wrapper.
    /// </summary>
    protected override async Task<FacetResults> GetFacetsInternalAsync(FacetRequest request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var parameters = new DynamicParameters();
        var facets = new Dictionary<string, IReadOnlyList<FacetValue>>();

        // Build WHERE clause for filtering
        var whereClauses = BuildFilterWhereClauses(request, parameters);
        var whereClause = whereClauses.Count > 0 ? "WHERE " + string.Join(" AND ", whereClauses) : "";

        // Get total count
        var countSql = $"SELECT COUNT(DISTINCT c.slug) FROM content_items c {whereClause}";
        var totalCount = await Connection.ExecuteScalarAsync<long>(
            new CommandDefinition(countSql, parameters, cancellationToken: ct));

        // Get tag facets if requested
        // Uses content_tags_expanded to count items that would match via substring
        // For example, clicking "AI" would match items with "Agentic AI", "AI Development", etc.
        if (request.FacetFields.Contains("tags"))
        {
            var tagsSql = $@"
                SELECT cte.tag_word AS Value, COUNT(DISTINCT c.slug) AS Count
                FROM content_tags_expanded cte
                INNER JOIN content_items c ON cte.slug = c.slug AND cte.collection_name = c.collection_name
                {whereClause}
                GROUP BY cte.tag_word
                ORDER BY Count DESC, cte.tag_word
                LIMIT @maxFacetValues";

            parameters.Add("maxFacetValues", request.MaxFacetValues);

            var tags = await Connection.QueryAsync<FacetValue>(
                new CommandDefinition(tagsSql, parameters, cancellationToken: ct));

            facets["tags"] = [.. tags];
        }

        // Get collection facets if requested
        if (request.FacetFields.Contains("collections"))
        {
            var collectionsSql = $@"
                SELECT c.collection_name AS Value, COUNT(*) AS Count
                FROM content_items c
                {whereClause}
                GROUP BY c.collection_name
                ORDER BY Count DESC, c.collection_name";

            var collections = await Connection.QueryAsync<FacetValue>(
                new CommandDefinition(collectionsSql, parameters, cancellationToken: ct));

            facets["collections"] = [.. collections];
        }

        // Get section facets if requested
        if (request.FacetFields.Contains("sections"))
        {
            // Use UNION ALL to count each section from boolean columns
            var sectionsSql = $@"
                SELECT 'ai' AS Value, COUNT(*) AS Count FROM content_items c {whereClause} AND c.is_ai = 1
                UNION ALL
                SELECT 'azure', COUNT(*) FROM content_items c {whereClause} AND c.is_azure = 1
                UNION ALL
                SELECT 'coding', COUNT(*) FROM content_items c {whereClause} AND c.is_coding = 1
                UNION ALL
                SELECT 'devops', COUNT(*) FROM content_items c {whereClause} AND c.is_devops = 1
                UNION ALL
                SELECT 'github-copilot', COUNT(*) FROM content_items c {whereClause} AND c.is_github_copilot = 1
                UNION ALL
                SELECT 'ml', COUNT(*) FROM content_items c {whereClause} AND c.is_ml = 1
                UNION ALL
                SELECT 'security', COUNT(*) FROM content_items c {whereClause} AND c.is_security = 1
                ORDER BY Count DESC, Value";

            var sections = await Connection.QueryAsync<FacetValue>(
                new CommandDefinition(sectionsSql, parameters, cancellationToken: ct));

            facets["sections"] = [.. sections.Where(s => s.Count > 0)];
        }

        return new FacetResults
        {
            Facets = facets,
            TotalCount = totalCount
        };
    }

    /// <summary>
    /// Get related articles based on tag overlap.
    /// Returns articles ranked by number of shared tags (descending).
    /// Implementation called by base class cached wrapper.
    /// </summary>
    protected override async Task<IReadOnlyList<ContentItem>> GetRelatedInternalAsync(
        IReadOnlyList<string> sourceTags,
        string excludeSlug,
        int count,
        CancellationToken ct = default)
    {
        if (sourceTags.Count == 0)
        {
            return [];
        }

        // Find articles with tag overlap, ranked by shared tag count
        // Uses content_tags_expanded for word-level tag matching (e.g., "AI" matches "AI Engineering")
        var relatedSql = $@"
            WITH RankedArticles AS (
                SELECT 
                    c.slug,
                    c.collection_name,
                    COUNT(DISTINCT cte.tag_word) AS SharedTagCount
                FROM content_items c
                INNER JOIN content_tags_expanded cte ON c.slug = cte.slug AND c.collection_name = cte.collection_name
                WHERE cte.tag_word IN @sourceTags
                  AND c.slug != @excludeSlug
                  AND c.draft = 0
                GROUP BY c.slug, c.collection_name
            )
            SELECT {ListViewColumns}
            FROM content_items c
            INNER JOIN RankedArticles ra ON c.slug = ra.slug AND c.collection_name = ra.collection_name
            ORDER BY ra.SharedTagCount DESC, c.date_epoch DESC
            LIMIT @count";

        var items = await Connection.QueryAsync<ContentItem>(
            new CommandDefinition(relatedSql, new { sourceTags, excludeSlug, count }, cancellationToken: ct));

        return [.. items];
    }

    /// <summary>
    /// Build SQL for counting total results (for pagination).
    /// </summary>
    private static string GetCountSql(SearchRequest request)
    {
        var sql = new StringBuilder("SELECT COUNT(DISTINCT c.slug) FROM content_items c");

        if (!string.IsNullOrWhiteSpace(request.Query))
        {
            sql.Append(" INNER JOIN content_fts ON c.rowid = content_fts.rowid");
        }

        _ = new DynamicParameters();
        var whereClauses = new List<string>
        {
            // Always exclude drafts
            "c.draft = 0"
        };

        if (!string.IsNullOrWhiteSpace(request.Query))
        {
            whereClauses.Add("content_fts MATCH @query");
        }

        if (request.Tags != null && request.Tags.Count > 0)
        {
            for (int i = 0; i < request.Tags.Count; i++)
            {
                whereClauses.Add($@"
                    EXISTS (
                        SELECT 1 FROM content_tags_expanded cte{i}
                        WHERE cte{i}.slug = c.slug
                        AND cte{i}.collection_name = c.collection_name
                        AND cte{i}.tag_word = @tag{i}
                    )");
            }
        }

        if (request.Sections != null && request.Sections.Count > 0)
        {
            var sectionConditions = new List<string>();
            foreach (var section in request.Sections)
            {
                var sectionNormalized = section.ToLowerInvariant().Trim();
                sectionConditions.Add(sectionNormalized switch
                {
                    "ai" => "c.is_ai = 1",
                    "azure" => "c.is_azure = 1",
                    "coding" => "c.is_coding = 1",
                    "devops" => "c.is_devops = 1",
                    "github-copilot" => "c.is_github_copilot = 1",
                    "ml" => "c.is_ml = 1",
                    "security" => "c.is_security = 1",
                    _ => "1=0"
                });
            }
            whereClauses.Add($"({string.Join(" OR ", sectionConditions)})");
        }

        if (request.Collections != null && request.Collections.Count > 0)
        {
            whereClauses.Add("c.collection_name IN @collections");
        }

        if (request.DateFrom.HasValue)
        {
            whereClauses.Add("c.date_epoch >= @fromDate");
        }

        if (request.DateTo.HasValue)
        {
            whereClauses.Add("c.date_epoch <= @toDate");
        }

        if (whereClauses.Count > 0)
        {
            sql.Append(" WHERE ");
            sql.Append(string.Join(" AND ", whereClauses));
        }

        return sql.ToString();
    }

    /// <summary>
    /// Build WHERE clauses for facet filtering.
    /// </summary>
    private static List<string> BuildFilterWhereClauses(FacetRequest request, DynamicParameters parameters)
    {
        var whereClauses = new List<string>
        {
            // Always exclude drafts
            "c.draft = 0"
        };

        if (request.Tags != null && request.Tags.Count > 0)
        {
            for (int i = 0; i < request.Tags.Count; i++)
            {
                whereClauses.Add($@"
                    EXISTS (
                        SELECT 1 FROM content_tags_expanded cte{i}
                        WHERE cte{i}.slug = c.slug
                        AND cte{i}.collection_name = c.collection_name
                        AND cte{i}.tag_word = @tag{i}
                    )");
                // Normalize tag to lowercase for case-insensitive matching
                parameters.Add($"tag{i}", request.Tags[i].ToLowerInvariant().Trim());
            }
        }

        if (request.Sections != null && request.Sections.Count > 0)
        {
            var sectionConditions = new List<string>();
            foreach (var section in request.Sections)
            {
                var sectionNormalized = section.ToLowerInvariant().Trim();
                sectionConditions.Add(sectionNormalized switch
                {
                    "ai" => "c.is_ai = 1",
                    "azure" => "c.is_azure = 1",
                    "coding" => "c.is_coding = 1",
                    "devops" => "c.is_devops = 1",
                    "github-copilot" => "c.is_github_copilot = 1",
                    "ml" => "c.is_ml = 1",
                    "security" => "c.is_security = 1",
                    _ => "1=0"
                });
            }
            whereClauses.Add($"({string.Join(" OR ", sectionConditions)})");
        }

        if (request.Collections != null && request.Collections.Count > 0)
        {
            whereClauses.Add("c.collection_name IN @collections");
            parameters.Add("collections", request.Collections.Select(c => c.ToLowerInvariant().Trim()).ToList());
        }

        if (request.DateFrom.HasValue)
        {
            whereClauses.Add("c.date_epoch >= @fromDate");
            parameters.Add("fromDate", ((DateTimeOffset)request.DateFrom.Value).ToUnixTimeSeconds());
        }

        if (request.DateTo.HasValue)
        {
            whereClauses.Add("c.date_epoch <= @toDate");
            parameters.Add("toDate", ((DateTimeOffset)request.DateTo.Value).ToUnixTimeSeconds());
        }

        return whereClauses;
    }

    /// <summary>
    /// Get tag counts using in-memory aggregation from tags_csv column.
    /// ULTRA-FAST: Parses comma-delimited tags from filtered content items in-memory.
    /// Performance: ~50-90ms for all 4117 items with covering index.
    /// Returns top N tags (sorted by count descending) above minUses threshold.
    /// </summary>
    protected override async Task<IReadOnlyList<TagWithCount>> GetTagCountsInternalAsync(
        DateTimeOffset? dateFrom,
        DateTimeOffset? dateTo,
        string? sectionName,
        string? collectionName,
        int? maxTags,
        int minUses,
        CancellationToken ct)
    {
        var sql = new StringBuilder();
        var parameters = new DynamicParameters();

        // OPTIMIZATION: Query only filtered content items with tags_csv column
        // Then parse and count tags in-memory (much faster than SQL GROUP BY)
        
        sql.Append("SELECT tags_csv FROM content_items c");
        
        // tags_csv is NOT NULL in the schema, so no null check needed
        var whereClauses = new List<string> { "c.draft = 0" };
        
        // Section filtering via denormalized boolean columns (no join needed)
        if (!string.IsNullOrWhiteSpace(sectionName) && !sectionName.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            var sectionNormalized = sectionName.ToLowerInvariant().Trim();
            var sectionCondition = sectionNormalized switch
            {
                "ai" => "c.is_ai = 1",
                "azure" => "c.is_azure = 1",
                "coding" => "c.is_coding = 1",
                "devops" => "c.is_devops = 1",
                "github-copilot" => "c.is_github_copilot = 1",
                "ml" => "c.is_ml = 1",
                "security" => "c.is_security = 1",
                _ => "1=0" // Unknown section - no match
            };
            whereClauses.Add(sectionCondition);
        }

        // Collection filtering
        if (!string.IsNullOrWhiteSpace(collectionName) && !collectionName.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            whereClauses.Add("c.collection_name = @collectionName");
            parameters.Add("collectionName", collectionName);
        }

        // Date filtering
        if (dateFrom.HasValue)
        {
            whereClauses.Add("c.date_epoch >= @dateFrom");
            parameters.Add("dateFrom", dateFrom.Value.ToUnixTimeSeconds());
        }

        if (dateTo.HasValue)
        {
            whereClauses.Add("c.date_epoch <= @dateTo");
            parameters.Add("dateTo", dateTo.Value.ToUnixTimeSeconds());
        }

        sql.Append(" WHERE ").Append(string.Join(" AND ", whereClauses));

        // Execute query - get all tags_csv values
        var tagsCsvRows = await Connection.QueryAsync<string>(
            new CommandDefinition(sql.ToString(), parameters, cancellationToken: ct));

        // Parse tags and count in-memory (FAST!)
        var tagCounts = new Dictionary<string, int>(StringComparer.Ordinal);
        
        foreach (var tagsCsv in tagsCsvRows)
        {
            if (string.IsNullOrEmpty(tagsCsv))
            {
                continue;
            }

            // Parse: ",AI,GitHub Copilot,DevOps," → ["AI", "GitHub Copilot", "DevOps"]
            var tags = tagsCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            
            foreach (var tag in tags)
            {
                if (!string.IsNullOrEmpty(tag))
                {
                    tagCounts[tag] = tagCounts.GetValueOrDefault(tag) + 1;
                }
            }
        }

        // Filter by minimum uses, sort, and limit results
        var results = tagCounts
            .Where(kvp => kvp.Value >= minUses)
            .Select(kvp => new TagWithCount { Tag = kvp.Key, Count = kvp.Value })
            .OrderByDescending(t => t.Count)
            .ThenBy(t => t.Tag)
            .AsEnumerable();

        if (maxTags.HasValue)
        {
            return [.. results.Take(maxTags.Value)];
        }

        return [.. results];
    }
}
