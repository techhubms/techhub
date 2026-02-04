using System.Data;
using System.Globalization;
using System.Text;
using Dapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// SQLite implementation of content repository using FTS5 for full-text search.
/// Extends DatabaseContentRepositoryBase with SQLite-specific FTS5 search.
/// All non-FTS operations are handled by the base class.
/// </summary>
public class SqliteContentRepository : DatabaseContentRepositoryBase
{
    public SqliteContentRepository(
        IDbConnection connection,
        ISqlDialect dialect,
        IMemoryCache cache,
        IMarkdownService markdownService,
        IOptions<AppSettings> settings)
        : base(connection, dialect, cache, markdownService, settings)
    {
    }

    /// <summary>
    /// Full-text search using SQLite FTS5.
    /// Only this method is SQLite-specific due to FTS5 syntax (MATCH, bm25).
    /// </summary>
    protected override async Task<SearchResults<ContentItem>> SearchInternalAsync(SearchRequest request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var sql = new StringBuilder();
        var parameters = new DynamicParameters();

        var hasQuery = !string.IsNullOrWhiteSpace(request.Query);
        var hasTags = request.Tags != null && request.Tags.Count > 0;
        var hasSections = request.Sections != null && request.Sections.Count > 0 && 
                          !request.Sections.Any(s => s.Equals("all", StringComparison.OrdinalIgnoreCase));
        var hasCollections = request.Collections != null && request.Collections.Count > 0 && 
                             !request.Collections.Any(c => c.Equals("all", StringComparison.OrdinalIgnoreCase));

        // OPTIMIZATION: When filtering by tags, pre-filter using tags table
        // This reduces FTS search from 4000+ items to potentially just 10-20
        if (hasTags)
        {
            var tagsQuery = BuildTagsTableQuery(request, parameters);

            sql.Append($@"
            SELECT {ListViewColumns}
            FROM content_items c");

            // Join with FTS if query is provided (now operating on pre-filtered subset!)
            if (hasQuery)
            {
                sql.Append(@"
            INNER JOIN content_fts ON c.rowid = content_fts.rowid");
            }

            sql.Append(CultureInfo.InvariantCulture, $@"
            WHERE (c.collection_name, c.slug) IN (
                {tagsQuery}
            )
            AND c.draft = {(request.IncludeDraft ? "0 OR c.draft = 1" : "0")}");

            if (!string.IsNullOrWhiteSpace(request.Subcollection) && 
                !request.Subcollection.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                sql.Append(" AND c.subcollection_name = @subcollection");
                parameters.Add("subcollection", request.Subcollection);
            }

            if (hasQuery)
            {
                sql.Append(@"
            AND content_fts MATCH @query");
                parameters.Add("query", request.Query);
            }

            sql.Append(hasQuery ? " ORDER BY bm25(content_fts)" : " ORDER BY c.date_epoch DESC");
            sql.Append(" LIMIT @take OFFSET @skip");
            parameters.Add("take", request.Take);
            parameters.Add("skip", request.Skip);
        }
        else
        {
            // Non-tags path: standard query from content_items
            sql.Append($@"
            SELECT {ListViewColumns}
            FROM content_items c");

            if (hasQuery)
            {
                sql.Append(@"
            INNER JOIN content_fts ON c.rowid = content_fts.rowid");
            }

            // Build WHERE clause
            var whereClauses = new List<string>();

            if (!request.IncludeDraft)
            {
                whereClauses.Add("c.draft = 0");
            }

            if (hasQuery)
            {
                whereClauses.Add("content_fts MATCH @query");
                parameters.Add("query", request.Query);
            }

            if (hasSections)
            {
                var sectionBitmask = CalculateSectionBitmask(request.Sections!);
                if (sectionBitmask > 0)
                {
                    whereClauses.Add($"(c.sections_bitmask & {sectionBitmask}) > 0");
                }
            }

            if (hasCollections)
            {
                whereClauses.Add("c.collection_name IN @collections");
                parameters.Add("collections", request.Collections!.Select(c => c.ToLowerInvariant().Trim()).ToList());
            }

            if (!string.IsNullOrWhiteSpace(request.Subcollection) && 
                !request.Subcollection.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                whereClauses.Add("c.subcollection_name = @subcollection");
                parameters.Add("subcollection", request.Subcollection);
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

            sql.Append(" WHERE ").Append(string.Join(" AND ", whereClauses));
            sql.Append(hasQuery ? " ORDER BY bm25(content_fts)" : " ORDER BY c.date_epoch DESC");
            sql.Append(" LIMIT @take OFFSET @skip");
            parameters.Add("take", request.Take);
            parameters.Add("skip", request.Skip);
        }

        // Get items and count in a single round-trip
        var combinedSql = sql.ToString() + ";\n" + BuildCountSql(request);

        using var multi = await Connection.QueryMultipleAsync(
            new CommandDefinition(combinedSql, parameters, cancellationToken: ct));

        var items = await multi.ReadAsync<ContentItem>();
        var totalCount = await multi.ReadSingleAsync<int>();

        // Compute facets if requested
        FacetResults? facets = null;
        if (request.IncludeFacets)
        {
            facets = await GetFacetsAsync(new FacetRequest(
                facetFields: ["tags", "collections", "sections"],
                tags: request.Tags,
                sections: request.Sections,
                collections: request.Collections,
                dateFrom: request.DateFrom,
                dateTo: request.DateTo
            ), ct);
        }

        return new SearchResults<ContentItem>
        {
            Items = [.. items],
            TotalCount = totalCount,
            Facets = facets,
            ContinuationToken = null
        };
    }

    /// <summary>
    /// Build SQL for counting total results.
    /// SQLite-specific due to FTS5 syntax (MATCH, content_fts).
    /// </summary>
    private static string BuildCountSql(SearchRequest request)
    {
        var hasTags = request.Tags != null && request.Tags.Count > 0;
        var hasSections = request.Sections != null && request.Sections.Count > 0;
        var hasQuery = !string.IsNullOrWhiteSpace(request.Query);

        if (hasTags)
        {
            var sql = new StringBuilder();

            if (hasQuery)
            {
                // Count with FTS: pre-filter by tags, then apply FTS match
                sql.Append(@"
                SELECT COUNT(*) FROM content_items c
                INNER JOIN content_fts ON c.rowid = content_fts.rowid
                WHERE (c.collection_name, c.slug) IN (
                    SELECT collection_name, slug FROM content_tags_expanded
                    WHERE tag_word IN @tags");
            }
            else
            {
                // Count without FTS: use tags table directly
                sql.Append(@"
                SELECT COUNT(*) FROM (
                    SELECT collection_name, slug FROM content_tags_expanded
                    WHERE tag_word IN @tags");
            }

            if (hasSections)
            {
                var sectionBitmask = CalculateSectionBitmask(request.Sections!);
                if (sectionBitmask > 0)
                {
                    sql.Append(CultureInfo.InvariantCulture, $" AND (sections_bitmask & {sectionBitmask}) > 0");
                }
            }

            if (request.Collections != null && request.Collections.Count > 0 && 
                !request.Collections.Any(c => c.Equals("all", StringComparison.OrdinalIgnoreCase)))
            {
                sql.Append(" AND collection_name IN @collections");
            }

            if (request.DateFrom.HasValue)
            {
                sql.Append(" AND date_epoch >= @fromDate");
            }

            if (request.DateTo.HasValue)
            {
                sql.Append(" AND date_epoch <= @toDate");
            }

            sql.Append(" GROUP BY collection_name, slug HAVING COUNT(*) = @tagCount");

            if (hasQuery)
            {
                sql.Append(") AND c.draft = 0 AND content_fts MATCH @query");
            }
            else
            {
                sql.Append(")");
            }

            return sql.ToString();
        }

        // Standard count query
        var countSql = new StringBuilder("SELECT COUNT(*) FROM content_items c");

        if (hasQuery)
        {
            countSql.Append(" INNER JOIN content_fts ON c.rowid = content_fts.rowid");
        }

        var whereClauses = new List<string> { "c.draft = 0" };

        if (hasQuery)
        {
            whereClauses.Add("content_fts MATCH @query");
        }

        if (hasSections)
        {
            var sectionBitmask = CalculateSectionBitmask(request.Sections!);
            if (sectionBitmask > 0)
            {
                whereClauses.Add($"(c.sections_bitmask & {sectionBitmask}) > 0");
            }
        }

        if (request.Collections != null && request.Collections.Count > 0 && 
            !request.Collections.Any(c => c.Equals("all", StringComparison.OrdinalIgnoreCase)))
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

        countSql.Append(" WHERE ").Append(string.Join(" AND ", whereClauses));

        return countSql.ToString();
    }
}
