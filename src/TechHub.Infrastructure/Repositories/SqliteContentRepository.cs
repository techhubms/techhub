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

        // Base SELECT - columns map directly to ContentItem model
        var hasQuery = !string.IsNullOrWhiteSpace(request.Query);

        sql.Append(@"
            SELECT 
                c.slug AS Slug,
                c.title AS Title,
                c.author AS Author,
                c.date_epoch AS DateEpoch,
                c.collection_name AS CollectionName,
                c.feed_name AS FeedName,
                c.primary_section_name AS PrimarySectionName,
                c.excerpt AS Excerpt,
                c.external_url AS ExternalUrl,
                c.draft AS Draft,
                c.content AS Content,
                c.subcollection_name AS SubcollectionName,
                c.plans AS Plans,
                c.ghes_support AS GhesSupport");

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
        // Uses content_tags table with tag_normalized for full tag matching
        if (request.Tags != null && request.Tags.Count > 0)
        {
            for (int i = 0; i < request.Tags.Count; i++)
            {
                whereClauses.Add($@"
                    EXISTS (
                        SELECT 1 FROM content_tags ct{i}
                        WHERE ct{i}.slug = c.slug
                        AND ct{i}.collection_name = c.collection_name
                        AND ct{i}.tag_normalized = @tag{i}
                    )");
                // Normalize tag to lowercase for case-insensitive matching
                parameters.Add($"tag{i}", request.Tags[i].ToLowerInvariant().Trim());
            }
        }

        // Section filtering (normalize to lowercase for case-insensitive matching)
        if (request.Sections != null && request.Sections.Count > 0)
        {
            whereClauses.Add("c.primary_section_name IN @sections");
            parameters.Add("sections", request.Sections.Select(s => s.ToLowerInvariant().Trim()).ToList());
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

        var hydratedItems = await HydrateRelationshipsAsync([.. items], ct);

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
            Items = hydratedItems,
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
            var sectionsSql = $@"
                SELECT cs.section_name AS Value, COUNT(DISTINCT c.slug) AS Count
                FROM content_sections cs
                INNER JOIN content_items c ON cs.slug = c.slug
                {whereClause}
                GROUP BY cs.section_name
                ORDER BY Count DESC, cs.section_name";

            var sections = await Connection.QueryAsync<FacetValue>(
                new CommandDefinition(sectionsSql, parameters, cancellationToken: ct));

            facets["sections"] = [.. sections];
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
        const string RelatedSql = @"
            WITH RankedArticles AS (
                SELECT 
                    c.slug,
                    c.collection_name,
                    COUNT(ct.tag) AS SharedTagCount
                FROM content_items c
                INNER JOIN content_tags ct ON c.slug = ct.slug AND c.collection_name = ct.collection_name
                WHERE ct.tag IN @sourceTags
                  AND c.slug != @excludeSlug
                  AND c.draft = 0
                GROUP BY c.slug, c.collection_name
            )
            SELECT 
                c.slug AS Slug,
                c.title AS Title,
                c.author AS Author,
                c.date_epoch AS DateEpoch,
                c.collection_name AS CollectionName,
                c.feed_name AS FeedName,
                c.primary_section_name AS PrimarySectionName,
                c.excerpt AS Excerpt,
                c.external_url AS ExternalUrl,
                c.draft AS Draft,
                c.content AS Content,
                c.subcollection_name AS SubcollectionName,
                c.plans AS Plans,
                c.ghes_support AS GhesSupport
            FROM content_items c
            INNER JOIN RankedArticles ra ON c.slug = ra.slug AND c.collection_name = ra.collection_name
            ORDER BY ra.SharedTagCount DESC, c.date_epoch DESC
            LIMIT @count";

        var items = await Connection.QueryAsync<ContentItem>(
            new CommandDefinition(RelatedSql, new { sourceTags, excludeSlug, count }, cancellationToken: ct));

        return await HydrateRelationshipsAsync([.. items], ct);
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
                        SELECT 1 FROM content_tags ct{i}
                        WHERE ct{i}.slug = c.slug
                        AND ct{i}.collection_name = c.collection_name
                        AND ct{i}.tag_normalized = @tag{i}
                    )");
            }
        }

        if (request.Sections != null && request.Sections.Count > 0)
        {
            whereClauses.Add("c.primary_section_name IN @sections");
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
                        SELECT 1 FROM content_tags ct{i}
                        WHERE ct{i}.slug = c.slug
                        AND ct{i}.collection_name = c.collection_name
                        AND ct{i}.tag_normalized = @tag{i}
                    )");
                // Normalize tag to lowercase for case-insensitive matching
                parameters.Add($"tag{i}", request.Tags[i].ToLowerInvariant().Trim());
            }
        }

        if (request.Sections != null && request.Sections.Count > 0)
        {
            whereClauses.Add("c.primary_section_name IN @sections");
            parameters.Add("sections", request.Sections.Select(s => s.ToLowerInvariant().Trim()).ToList());
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
    /// Get tag counts using efficient SQL GROUP BY query.
    /// This is MUCH faster than loading all items and counting in memory.
    /// Uses INNER JOIN instead of EXISTS for better performance with covering indexes.
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

        // Base query - use INNER JOIN with content_sections when filtering by section
        // This leverages the idx_sections_name_covering index for optimal performance
        var needsSectionJoin = !string.IsNullOrWhiteSpace(sectionName) && !sectionName.Equals("all", StringComparison.OrdinalIgnoreCase);

        if (needsSectionJoin)
        {
            sql.Append(@"
            SELECT ct.tag AS Tag, COUNT(DISTINCT ct.slug) AS Count
            FROM content_tags ct
            INNER JOIN content_sections cs ON ct.collection_name = cs.collection_name AND ct.slug = cs.slug
            INNER JOIN content_items c ON ct.collection_name = c.collection_name AND ct.slug = c.slug
            WHERE c.draft = 0
            AND cs.section_name = @sectionName");
            parameters.Add("sectionName", sectionName);
        }
        else
        {
            sql.Append(@"
            SELECT ct.tag AS Tag, COUNT(DISTINCT ct.slug) AS Count
            FROM content_tags ct
            INNER JOIN content_items c ON ct.collection_name = c.collection_name AND ct.slug = c.slug
            WHERE c.draft = 0");
        }

        // Date range filtering
        if (dateFrom.HasValue)
        {
            sql.Append(" AND c.date_epoch >= @dateFrom");
            parameters.Add("dateFrom", dateFrom.Value.ToUnixTimeSeconds());
        }

        if (dateTo.HasValue)
        {
            sql.Append(" AND c.date_epoch <= @dateTo");
            parameters.Add("dateTo", dateTo.Value.ToUnixTimeSeconds());
        }

        // Collection filtering (skip "all" as it means no filter)
        if (!string.IsNullOrWhiteSpace(collectionName) && !collectionName.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            sql.Append(" AND ct.collection_name = @collectionName");
            parameters.Add("collectionName", collectionName);
        }

        sql.Append(" GROUP BY ct.tag");

        // Filter by minimum uses
        if (minUses > 1)
        {
            sql.Append(" HAVING COUNT(DISTINCT ct.slug) >= @minUses");
            parameters.Add("minUses", minUses);
        }

        sql.Append(" ORDER BY Count DESC, ct.tag");

        // Limit results
        if (maxTags.HasValue)
        {
            sql.Append(" LIMIT @maxTags");
            parameters.Add("maxTags", maxTags.Value);
        }

        var results = await Connection.QueryAsync<TagWithCount>(
            new CommandDefinition(sql.ToString(), parameters, cancellationToken: ct));

        return [.. results];
    }
}
