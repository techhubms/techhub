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
public class SqliteContentRepository(
    IDbConnection connection,
    ISqlDialect dialect,
    IMemoryCache cache,
    IMarkdownService markdownService)
    : DatabaseContentRepositoryBase(connection, dialect, cache, markdownService)
{
    /// <summary>
    /// Full-text search using SQLite FTS5.
    /// Supports query string, tag filtering, section filtering, and date filtering.
    /// Implementation called by base class cached wrapper.
    /// </summary>
    protected override async Task<SearchResults<ContentItem>> SearchInternalAsync(SearchRequest request, CancellationToken ct = default)
    {
        var sql = new StringBuilder();
        var parameters = new DynamicParameters();
        
        // Base SELECT - only include rank when FTS is needed
        var hasQuery = !string.IsNullOrWhiteSpace(request.Query);
        
        sql.Append(@"
            SELECT 
                c.slug AS Slug,
                c.title AS Title,
                c.content AS Content,
                c.excerpt AS Excerpt,
                c.date_epoch AS DateEpoch,
                c.collection_name AS CollectionName,
                c.subcollection_name AS SubcollectionName,
                c.feed_name AS FeedName,
                c.external_url AS ExternalUrl,
                c.author AS Author,
                c.ghes_support AS GhesSupport,
                c.draft AS Draft");
        
        if (hasQuery)
        {
            sql.Append(",\n                bm25(fts) AS Rank");
        }
        
        sql.Append("\n            FROM content_items c");

        // Join with FTS5 for full-text search if query is provided
        if (hasQuery)
        {
            sql.Append(@"
                INNER JOIN content_fts fts ON c.rowid = fts.rowid");
        }

        // WHERE clause
        var whereClauses = new List<string>();

        // Always exclude drafts in search
        whereClauses.Add("c.draft = 0");

        // Full-text search
        if (hasQuery)
        {
            whereClauses.Add("fts MATCH @query");
            parameters.Add("query", request.Query);
        }

        // Tag filtering (AND logic - all selected tags must match)
        if (request.Tags != null && request.Tags.Count > 0)
        {
            for (int i = 0; i < request.Tags.Count; i++)
            {
                whereClauses.Add($@"
                    EXISTS (
                        SELECT 1 FROM content_tags_expanded cte{i}
                        WHERE cte{i}.slug = c.slug
                        AND cte{i}.tag_word = @tag{i}
                    )");
                // Normalize tag to lowercase for case-insensitive matching
                parameters.Add($"tag{i}", request.Tags[i].ToLowerInvariant().Trim());
            }
        }

        // Section filtering
        if (request.Sections != null && request.Sections.Count > 0)
        {
            whereClauses.Add("c.primary_section_name IN @sections");
            parameters.Add("sections", request.Sections);
        }

        // Collection filtering
        if (request.Collections != null && request.Collections.Count > 0)
        {
            whereClauses.Add("c.collection_name IN @collections");
            parameters.Add("collections", request.Collections);
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
        if (hasQuery)
        {
            sql.Append(" ORDER BY Rank");
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

        var hydratedItems = await HydrateRelationshipsAsync(items.ToList(), ct);

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
                FacetFields = new[] { "tags", "collections", "sections" }
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
        if (request.FacetFields.Contains("tags"))
        {
            var tagsSql = $@"
                SELECT ct.tag AS Value, COUNT(DISTINCT c.slug) AS Count
                FROM content_tags ct
                INNER JOIN content_items c ON ct.slug = c.slug
                {whereClause}
                GROUP BY ct.tag
                ORDER BY Count DESC, ct.tag
                LIMIT @maxFacetValues";

            parameters.Add("maxFacetValues", request.MaxFacetValues);

            var tags = await Connection.QueryAsync<FacetValue>(
                new CommandDefinition(tagsSql, parameters, cancellationToken: ct));

            facets["tags"] = tags.ToList();
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

            facets["collections"] = collections.ToList();
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

            facets["sections"] = sections.ToList();
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
        string articleId,
        int count = 5,
        CancellationToken ct = default)
    {
        // Get tags for the source article
        const string getTagsSql = @"
            SELECT tag FROM content_tags WHERE slug = @articleId";

        var sourceTags = (await Connection.QueryAsync<string>(
            new CommandDefinition(getTagsSql, new { articleId }, cancellationToken: ct))).ToList();

        if (sourceTags.Count == 0)
        {
            // Fallback: return recent articles from same collection
            const string fallbackSql = @"
                SELECT 
                    c.slug AS Slug,
                    c.title AS Title,
                    c.content AS Content,
                    c.excerpt AS Excerpt,
                    c.date_epoch AS DateEpoch,
                    c.collection_name AS CollectionName,
                    c.subcollection_name AS SubcollectionName,
                    c.feed_name AS FeedName,
                    c.external_url AS ExternalUrl,
                    c.author AS Author,
                    c.ghes_support AS GhesSupport,
                    c.draft AS Draft
                FROM content_items c
                WHERE c.collection_name = (SELECT collection_name FROM content_items WHERE slug = @articleId)
                  AND c.slug != @articleId
                  AND c.draft = 0
                ORDER BY c.date_epoch DESC
                LIMIT @count";

            var fallbackItems = await Connection.QueryAsync<ContentItem>(
                new CommandDefinition(fallbackSql, new { articleId, count }, cancellationToken: ct));

            return await HydrateRelationshipsAsync(fallbackItems.ToList(), ct);
        }

        // Find articles with tag overlap
        const string relatedSql = @"
            SELECT 
                c.slug AS Slug,
                c.title AS Title,
                c.content AS Content,
                c.excerpt AS Excerpt,
                c.date_epoch AS DateEpoch,
                c.collection_name AS CollectionName,
                c.subcollection_name AS SubcollectionName,
                c.feed_name AS FeedName,
                c.external_url AS ExternalUrl,
                c.author AS Author,
                c.ghes_support AS GhesSupport,
                c.draft AS Draft,
                COUNT(ct.tag) AS SharedTagCount
            FROM content_items c
            INNER JOIN content_tags ct ON c.slug = ct.slug
            WHERE ct.tag IN @sourceTags
              AND c.slug != @articleId
              AND c.draft = 0
            GROUP BY c.slug
            ORDER BY SharedTagCount DESC, c.date_epoch DESC
            LIMIT @count";

        var items = await Connection.QueryAsync<ContentItem>(
            new CommandDefinition(relatedSql, new { sourceTags, articleId, count }, cancellationToken: ct));

        return await HydrateRelationshipsAsync(items.ToList(), ct);
    }

    /// <summary>
    /// Build SQL for counting total results (for pagination).
    /// </summary>
    private string GetCountSql(SearchRequest request)
    {
        var sql = new StringBuilder("SELECT COUNT(DISTINCT c.slug) FROM content_items c");

        if (!string.IsNullOrWhiteSpace(request.Query))
        {
            sql.Append(" INNER JOIN content_fts fts ON c.rowid = fts.rowid");
        }

        var parameters = new DynamicParameters();
        var whereClauses = new List<string>();

        // Always exclude drafts
        whereClauses.Add("c.draft = 0");

        if (!string.IsNullOrWhiteSpace(request.Query))
        {
            whereClauses.Add("fts MATCH @query");
        }

        if (request.Tags != null && request.Tags.Count > 0)
        {
            for (int i = 0; i < request.Tags.Count; i++)
            {
                whereClauses.Add($@"
                    EXISTS (
                        SELECT 1 FROM content_tags_expanded cte{i}
                        WHERE cte{i}.slug = c.slug
                        AND cte{i}.tag_word = @tag{i}
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
    private List<string> BuildFilterWhereClauses(FacetRequest request, DynamicParameters parameters)
    {
        var whereClauses = new List<string>();

        // Always exclude drafts
        whereClauses.Add("c.draft = 0");

        if (request.Tags != null && request.Tags.Count > 0)
        {
            for (int i = 0; i < request.Tags.Count; i++)
            {
                whereClauses.Add($@"
                    EXISTS (
                        SELECT 1 FROM content_tags_expanded cte{i}
                        WHERE cte{i}.slug = c.slug
                        AND cte{i}.tag_word = @tag{i}
                    )");
                // Normalize tag to lowercase for case-insensitive matching
                parameters.Add($"tag{i}", request.Tags[i].ToLowerInvariant().Trim());
            }
        }

        if (request.Sections != null && request.Sections.Count > 0)
        {
            whereClauses.Add("c.primary_section_name IN @sections");
            parameters.Add("sections", request.Sections);
        }

        if (request.Collections != null && request.Collections.Count > 0)
        {
            whereClauses.Add("c.collection_name IN @collections");
            parameters.Add("collections", request.Collections);
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

        sql.Append(@"
            SELECT ct.tag AS Tag, COUNT(DISTINCT c.slug) AS Count
            FROM content_tags ct
            INNER JOIN content_items c ON ct.collection_name = c.collection_name AND ct.slug = c.slug
            WHERE c.draft = 0");

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

        // Section filtering (skip "all" as it means no filter)
        if (!string.IsNullOrWhiteSpace(sectionName) && !sectionName.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            // Join with content_sections for section filtering
            sql.Append(@" AND EXISTS (
                SELECT 1 FROM content_sections cs
                WHERE cs.collection_name = c.collection_name
                AND cs.slug = c.slug
                AND cs.section_name = @sectionName
            )");
            parameters.Add("sectionName", sectionName);
        }

        // Collection filtering (skip "all" as it means no filter)
        if (!string.IsNullOrWhiteSpace(collectionName) && !collectionName.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            sql.Append(" AND c.collection_name = @collectionName");
            parameters.Add("collectionName", collectionName);
        }

        sql.Append(" GROUP BY ct.tag");

        // Filter by minimum uses
        if (minUses > 1)
        {
            sql.Append(" HAVING COUNT(DISTINCT c.slug) >= @minUses");
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

        return results.ToList();
    }
}
