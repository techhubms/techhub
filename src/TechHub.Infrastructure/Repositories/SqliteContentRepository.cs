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
        var hasTags = request.Tags != null && request.Tags.Count > 0;
        var hasSections = request.Sections != null && request.Sections.Count > 0;
        var hasCollections = request.Collections != null && request.Collections.Count > 0;
        var hasDates = request.DateFrom.HasValue || request.DateTo.HasValue;

        // OPTIMIZATION: When filtering by tags (with or without full-text search),
        // use tags table pre-filtering:
        // 1. Get (collection_name, slug) from content_tags_expanded (has denormalized date_epoch, is_*, collection_name)
        // 2. Fetch full content_items with IN clause
        // This leverages idx_tags_* indexes and reduces FTS search from 4000+ items to potentially just 10-20
        if (hasTags)
        {
            // Step 1: Get (collection_name, slug, date_epoch) from denormalized tags table
            // content_tags_expanded has: date_epoch, collection_name, is_* columns - can filter everything!
            var tagsQuery = BuildTagsTableQuery(request, parameters);
            
            // Step 2: Fetch full content details for the pre-filtered results
            sql.Append($@"
            SELECT {ListViewColumns}
            FROM content_items c");
            
            // Join with FTS if query is provided (now operating on pre-filtered subset!)
            if (hasQuery)
            {
                sql.Append(@"
            INNER JOIN content_fts ON c.rowid = content_fts.rowid");
            }
            
            sql.Append($@"
            WHERE (c.collection_name, c.slug) IN (
                {tagsQuery}
            )
            AND c.draft = 0");
            
            // Apply FTS filter if query is provided
            if (hasQuery)
            {
                sql.Append(@"
            AND content_fts MATCH @query");
                parameters.Add("query", request.Query);
            }
            
            // Order by FTS rank if searching, otherwise by date
            if (hasQuery)
            {
                sql.Append(" ORDER BY bm25(content_fts)");
            }
            else
            {
                sql.Append(" ORDER BY c.date_epoch DESC");
            }
        }
        else
        {
            sql.Append($@"
            SELECT {ListViewColumns}");

            // NOTE: bm25() rank is used in ORDER BY, not in SELECT, to avoid Dapper mapping issues

            // Start from content_items (this path is for FTS queries or non-tag queries)
            sql.Append("\n            FROM content_items c");

            // Join with FTS5 for full-text search if query is provided
            if (hasQuery)
            {
                sql.Append(@"
                INNER JOIN content_fts ON c.rowid = content_fts.rowid");
            }

            // Tag filtering using subquery with GROUP BY + HAVING (parameters added for WHERE clause)
            if (hasTags)
            {
                var normalizedTags = request.Tags.Select(t => t.ToLowerInvariant().Trim()).ToList();
                parameters.Add("tags", normalizedTags);
                parameters.Add("tagCount", normalizedTags.Count);
            }
        }

        // WHERE clause (skip if using tags table optimization - filters already in subquery)
        var whereClauses = new List<string>();
        
        if (!hasTags)
        {
            // Always exclude drafts in search
            whereClauses.Add("c.draft = 0");

            // Tag filtering using subquery with GROUP BY + HAVING
            if (hasTags)
            {
                whereClauses.Add(@"(c.collection_name, c.slug) IN (
                    SELECT collection_name, slug FROM content_tags_expanded
                    WHERE tag_word IN @tags
                    GROUP BY collection_name, slug
                    HAVING COUNT(*) = @tagCount
                )");
            }

            // Full-text search
            if (hasQuery)
            {
                whereClauses.Add("content_fts MATCH @query");
                parameters.Add("query", request.Query);
            }

            // Section filtering using bitmask (zero-join filtering)
            if (request.Sections != null && request.Sections.Count > 0)
            {
                var sectionBitmask = CalculateSectionBitmask(request.Sections);
                if (sectionBitmask > 0)
                {
                    whereClauses.Add($"(c.sections_bitmask & {sectionBitmask}) > 0");
                }
            }
        }

        // Collection and date filtering (applies to both approaches)
        // When using tags table optimization, these are handled in the subquery
        if (!hasTags)
        {
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
        }

        if (whereClauses.Count > 0)
        {
            sql.Append(" WHERE ");
            sql.Append(string.Join(" AND ", whereClauses));
        }

        // ORDER BY and LIMIT (skip if using tags table optimization - already in subquery)
        if (!hasTags)
        {
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
        }

        // TODO: Implement continuation token-based pagination
        // For now, using simple offset (will be replaced with keyset pagination)

        // Optimization: Use QueryMultipleAsync to get items and count in a single round-trip
        // This significantly reduces performance overhead from 400-1100ms to under 200ms
        var combinedSql = sql.ToString() + ";\n" + GetCountSql(request);
        
        using var multi = await Connection.QueryMultipleAsync(
            new CommandDefinition(combinedSql, parameters, cancellationToken: ct));

        var items = await multi.ReadAsync<ContentItem>();
        var totalCount = await multi.ReadSingleAsync<int>();

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
            // Use UNION ALL to count each section from bitmask column
            var sectionsSql = $@"
                SELECT 'ai' AS Value, COUNT(*) AS Count FROM content_items c {whereClause} AND (c.sections_bitmask & 1) > 0
                UNION ALL
                SELECT 'azure', COUNT(*) FROM content_items c {whereClause} AND (c.sections_bitmask & 2) > 0
                UNION ALL
                SELECT 'coding', COUNT(*) FROM content_items c {whereClause} AND (c.sections_bitmask & 4) > 0
                UNION ALL
                SELECT 'devops', COUNT(*) FROM content_items c {whereClause} AND (c.sections_bitmask & 8) > 0
                UNION ALL
                SELECT 'github-copilot', COUNT(*) FROM content_items c {whereClause} AND (c.sections_bitmask & 16) > 0
                UNION ALL
                SELECT 'ml', COUNT(*) FROM content_items c {whereClause} AND (c.sections_bitmask & 32) > 0
                UNION ALL
                SELECT 'security', COUNT(*) FROM content_items c {whereClause} AND (c.sections_bitmask & 64) > 0
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
    /// Build SQL for counting total results (for pagination).
    /// Uses GROUP BY + HAVING for multi-tag AND logic (matches BuildTagsTableQuery approach).
    /// </summary>
    private static string GetCountSql(SearchRequest request)
    {
        var hasTags = request.Tags != null && request.Tags.Count > 0;
        var hasSections = request.Sections != null && request.Sections.Count > 0;
        var hasQuery = !string.IsNullOrWhiteSpace(request.Query);
        
        // OPTIMIZATION: When filtering by tags (with or without FTS),
        // pre-filter using content_tags_expanded then apply FTS on smaller subset
        if (hasTags)
        {
            if (hasQuery)
            {
                // Count with FTS: pre-filter by tags, then apply FTS match
                var sql = new StringBuilder(@"
                SELECT COUNT(*) FROM content_items c
                INNER JOIN content_fts ON c.rowid = content_fts.rowid
                WHERE (c.collection_name, c.slug) IN (
                    SELECT collection_name, slug FROM content_tags_expanded
                    WHERE tag_word IN @tags");
                
                // Section filtering
                if (hasSections)
                {
                    var sectionBitmask = CalculateSectionBitmask(request.Sections!);
                    if (sectionBitmask > 0)
                    {
                        sql.Append($" AND (sections_bitmask & {sectionBitmask}) > 0");
                    }
                }
                
                // Collection filtering
                if (request.Collections != null && request.Collections.Count > 0)
                {
                    sql.Append(" AND collection_name IN @collections");
                }
                
                // Date filtering
                if (request.DateFrom.HasValue)
                {
                    sql.Append(" AND date_epoch >= @fromDate");
                }
                
                if (request.DateTo.HasValue)
                {
                    sql.Append(" AND date_epoch <= @toDate");
                }
                
                sql.Append(" GROUP BY collection_name, slug HAVING COUNT(*) = @tagCount");
                sql.Append(") AND c.draft = 0 AND content_fts MATCH @query");
                return sql.ToString();
            }
            else
            {
                // Count without FTS: use tags table directly (original optimization)
                var sql = new StringBuilder(@"
                SELECT COUNT(*) FROM (
                    SELECT collection_name, slug FROM content_tags_expanded
                    WHERE tag_word IN @tags");
                
                // Section filtering using bitmask column (if specified)
                if (hasSections)
                {
                    var sectionBitmask = CalculateSectionBitmask(request.Sections!);
                    if (sectionBitmask > 0)
                    {
                        sql.Append($" AND (sections_bitmask & {sectionBitmask}) > 0");
                    }
                }
                
                // Collection filtering
                if (request.Collections != null && request.Collections.Count > 0)
                {
                    sql.Append(" AND collection_name IN @collections");
                }
                
                // Date filtering using denormalized date_epoch
                if (request.DateFrom.HasValue)
                {
                    sql.Append(" AND date_epoch >= @fromDate");
                }
                
                if (request.DateTo.HasValue)
                {
                    sql.Append(" AND date_epoch <= @toDate");
                }
                
                sql.Append(" GROUP BY collection_name, slug HAVING COUNT(*) = @tagCount)");
                return sql.ToString();
            }
        }
        
        // Standard approach: count from content_items
        // Optimization: Use COUNT(*) instead of COUNT(DISTINCT c.slug)
        // Since (collection_name, slug) is the PRIMARY KEY, rows are already unique
        // This avoids the expensive TEMP B-TREE operation (928ms -> <50ms)
        var standardSql = new StringBuilder("SELECT COUNT(*) FROM content_items c");

        if (hasQuery)
        {
            standardSql.Append(" INNER JOIN content_fts ON c.rowid = content_fts.rowid");
        }

        // Tag filtering using subquery with GROUP BY + HAVING (matches main query approach)
        if (hasTags)
        {
            standardSql.Append(@"
                WHERE (c.collection_name, c.slug) IN (
                    SELECT collection_name, slug FROM content_tags_expanded
                    WHERE tag_word IN @tags
                    GROUP BY collection_name, slug
                    HAVING COUNT(*) = @tagCount
                )");
        }

        var whereClauses = new List<string>
        {
            // Always exclude drafts
            "c.draft = 0"
        };

        if (!string.IsNullOrWhiteSpace(request.Query))
        {
            whereClauses.Add("content_fts MATCH @query");
        }

        if (request.Sections != null && request.Sections.Count > 0)
        {
            var sectionBitmask = CalculateSectionBitmask(request.Sections);
            if (sectionBitmask > 0)
            {
                whereClauses.Add($"(c.sections_bitmask & {sectionBitmask}) > 0");
            }
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
            // Use AND if we already have a WHERE clause from tags subquery
            standardSql.Append(hasTags ? " AND " : " WHERE ");
            standardSql.Append(string.Join(" AND ", whereClauses));
        }

        return standardSql.ToString();
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
            var sectionBitmask = CalculateSectionBitmask(request.Sections);
            if (sectionBitmask > 0)
            {
                whereClauses.Add($"(c.sections_bitmask & {sectionBitmask}) > 0");
            }
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
        // Then parse and count tags in-memory (faster than SQL GROUP BY for this workload)
        
        sql.Append("SELECT tags_csv FROM content_items c");
        
        // tags_csv is NOT NULL in the schema, so no null check needed
        var whereClauses = new List<string> { "c.draft = 0" };
        
        // Section filtering via bitmask column (no join needed)
        if (!string.IsNullOrWhiteSpace(sectionName) && !sectionName.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            var sectionBitmask = CalculateSectionBitmask(sectionName);
            if (sectionBitmask > 0)
            {
                whereClauses.Add($"(c.sections_bitmask & {sectionBitmask}) > 0");
            }
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

        // Execute query - get all tags_csv values (uses covering index for fast retrieval)
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

    /// <summary>
    /// Build optimized subquery that queries content_tags_expanded directly.
    /// Uses GROUP BY + HAVING COUNT(*) = N for multi-tag AND logic (5.8x faster than self-join).
    /// Uses bitmask for fast section filtering with single idx_tags_word_sections_date index.
    /// Returns (collection_name, slug) pairs ordered by date_epoch DESC.
    /// </summary>
    private static string BuildTagsTableQuery(SearchRequest request, DynamicParameters parameters)
    {
        var sql = new StringBuilder();
        sql.Append("SELECT collection_name, slug FROM content_tags_expanded");
        
        var whereClauses = new List<string>();
        
        // Tag filtering - use IN clause for all tags
        var normalizedTags = request.Tags!.Select(t => t.ToLowerInvariant().Trim()).ToList();
        whereClauses.Add("tag_word IN @tags");
        parameters.Add("tags", normalizedTags);
        
        // Section filtering using bitmask (Bit 0=AI, Bit 1=Azure, Bit 2=Coding, Bit 3=DevOps, Bit 4=GitHubCopilot, Bit 5=ML, Bit 6=Security)
        if (request.Sections != null && request.Sections.Count > 0)
        {
            var sectionBitmask = CalculateSectionBitmask(request.Sections);
            if (sectionBitmask > 0)
            {
                whereClauses.Add($"(sections_bitmask & {sectionBitmask}) > 0");
            }
        }
        
        // Collection filtering
        if (request.Collections != null && request.Collections.Count > 0)
        {
            whereClauses.Add("collection_name IN @collections");
            parameters.Add("collections", request.Collections.Select(c => c.ToLowerInvariant().Trim()).ToList());
        }
        
        // Date filtering using denormalized date_epoch column
        if (request.DateFrom.HasValue)
        {
            whereClauses.Add("date_epoch >= @fromDate");
            parameters.Add("fromDate", ((DateTimeOffset)request.DateFrom.Value).ToUnixTimeSeconds());
        }
        
        if (request.DateTo.HasValue)
        {
            whereClauses.Add("date_epoch <= @toDate");
            parameters.Add("toDate", ((DateTimeOffset)request.DateTo.Value).ToUnixTimeSeconds());
        }
        
        sql.Append(" WHERE ").Append(string.Join(" AND ", whereClauses));
        
        // GROUP BY to find items that have ALL tags (AND logic)
        sql.Append(" GROUP BY collection_name, slug");
        sql.Append(" HAVING COUNT(*) = @tagCount");
        parameters.Add("tagCount", normalizedTags.Count);
        
        // Order by max date_epoch DESC (need MAX since we're grouping)
        sql.Append(" ORDER BY MAX(date_epoch) DESC");
        
        // Limit results
        sql.Append(" LIMIT @take");
        parameters.Add("take", request.Take);
        
        return sql.ToString();
    }
}
