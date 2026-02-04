using System.Data;
using System.Text;
using Dapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// PostgreSQL implementation of content repository using tsvector for full-text search.
/// Extends DatabaseContentRepositoryBase with PostgreSQL-specific tsvector search.
/// All non-FTS operations are handled by the base class.
/// </summary>
public class PostgresContentRepository : DatabaseContentRepositoryBase
{
    public PostgresContentRepository(
        IDbConnection connection,
        ISqlDialect dialect,
        IMemoryCache cache,
        IMarkdownService markdownService,
        IOptions<AppSettings> settings)
        : base(connection, dialect, cache, markdownService, settings)
    {
    }

    /// <summary>
    /// Full-text search using PostgreSQL tsvector.
    /// Only this method is PostgreSQL-specific due to tsvector syntax (@@, plainto_tsquery, ts_rank).
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
        // This reduces tsvector search from 4000+ items to potentially just 10-20
        if (hasTags)
        {
            var tagsQuery = BuildTagsTableQuery(request, parameters);

            sql.Append($@"
            SELECT {ListViewColumns}
            FROM content_items c
            WHERE (c.collection_name, c.slug) IN (
                {tagsQuery}
            )
            AND c.draft = {(request.IncludeDraft ? $"{Dialect.GetBooleanLiteral(false)} OR c.draft = {Dialect.GetBooleanLiteral(true)}" : Dialect.GetBooleanLiteral(false))}");

            if (!string.IsNullOrWhiteSpace(request.Subcollection) && 
                !request.Subcollection.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                sql.Append(" AND c.subcollection_name = @subcollection");
                parameters.Add("subcollection", request.Subcollection);
            }

            if (hasQuery)
            {
                sql.Append(@"
            AND c.search_vector @@ plainto_tsquery('english', @query)");
                parameters.Add("query", request.Query);
                sql.Append(" ORDER BY ts_rank(c.search_vector, plainto_tsquery('english', @query)) DESC");
            }
            else
            {
                sql.Append(" ORDER BY c.date_epoch DESC");
            }

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

            // Build WHERE clause
            var whereClauses = new List<string>();

            if (!request.IncludeDraft)
            {
                whereClauses.Add($"c.draft = {Dialect.GetBooleanLiteral(false)}");
            }

            if (hasQuery)
            {
                whereClauses.Add("c.search_vector @@ plainto_tsquery('english', @query)");
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
                whereClauses.Add("c.collection_name = ANY(@collections)");
                parameters.Add("collections", request.Collections!.Select(c => c.ToLowerInvariant().Trim()).ToArray());
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

            // ORDER BY: Full-text search uses ts_rank, otherwise date descending
            if (hasQuery)
            {
                sql.Append(" ORDER BY ts_rank(c.search_vector, plainto_tsquery('english', @query)) DESC");
            }
            else
            {
                sql.Append(" ORDER BY c.date_epoch DESC");
            }

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
    /// PostgreSQL-specific due to tsvector syntax (@@, plainto_tsquery).
    /// </summary>
    private string BuildCountSql(SearchRequest request)
    {
        var hasTags = request.Tags != null && request.Tags.Count > 0;
        var hasSections = request.Sections != null && request.Sections.Count > 0;
        var hasQuery = !string.IsNullOrWhiteSpace(request.Query);

        if (hasTags)
        {
            var sql = new StringBuilder();

            if (hasQuery)
            {
                // Count with tsvector: pre-filter by tags, then apply tsvector match
                sql.Append(@"
                SELECT COUNT(DISTINCT c.slug)
                FROM content_items c
                WHERE (c.collection_name, c.slug) IN (
                    SELECT DISTINCT collection_name, slug
                    FROM (");

                for (int i = 0; i < request.Tags!.Count; i++)
                {
                    if (i > 0)
                    {
                        sql.Append(" INTERSECT ");
                    }

                    sql.Append(@"
                        SELECT collection_name, slug 
                        FROM content_tags_expanded 
                        WHERE tag_word = @tag").Append(i);
                }

                sql.Append(@"
                    ) AS tag_results
                )
                AND c.draft = {Dialect.GetBooleanLiteral(false)}
                AND c.search_vector @@ plainto_tsquery('english', @query)");
            }
            else
            {
                // Count with tags only: use content_tags_expanded for pre-filtering
                sql.Append(@"
                SELECT COUNT(DISTINCT c.slug)
                FROM content_items c
                WHERE (c.collection_name, c.slug) IN (
                    SELECT DISTINCT collection_name, slug
                    FROM (");

                for (int i = 0; i < request.Tags!.Count; i++)
                {
                    if (i > 0)
                    {
                        sql.Append(" INTERSECT ");
                    }

                    sql.Append(@"
                        SELECT collection_name, slug 
                        FROM content_tags_expanded 
                        WHERE tag_word = @tag").Append(i);
                }

                sql.Append(@"
                    ) AS tag_results
                )
                AND c.draft = {Dialect.GetBooleanLiteral(false)}");
            }

            return sql.ToString();
        }

        // Standard count (no tags pre-filter)
        var countSql = new StringBuilder($"SELECT COUNT(*) FROM content_items c WHERE c.draft = {Dialect.GetBooleanLiteral(false)}");

        if (hasQuery)
        {
            countSql.Append(" AND c.search_vector @@ plainto_tsquery('english', @query)");
        }

        if (hasSections)
        {
            var sectionBitmask = CalculateSectionBitmask(request.Sections!);
            if (sectionBitmask > 0)
            {
                countSql.Append($" AND (c.sections_bitmask & {sectionBitmask}) > 0");
            }
        }

        if (request.Collections != null && request.Collections.Count > 0 && 
            !request.Collections.Any(c => c.Equals("all", StringComparison.OrdinalIgnoreCase)))
        {
            countSql.Append(" AND c.collection_name = ANY(@collections)");
        }

        if (request.DateFrom.HasValue)
        {
            countSql.Append(" AND c.date_epoch >= @fromDate");
        }

        if (request.DateTo.HasValue)
        {
            countSql.Append(" AND c.date_epoch <= @toDate");
        }

        return countSql.ToString();
    }
}
