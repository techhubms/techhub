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
/// Base class for database-backed content repositories.
/// Extends ContentRepositoryBase with shared Dapper-based database operations for SQLite and PostgreSQL.
/// </summary>
public abstract class DatabaseContentRepositoryBase : ContentRepositoryBase
{
    /// <summary>
    /// Column selection for list views - excludes content column for performance.
    /// Maps to ContentItem record which doesn't have a Content property.
    /// </summary>
    protected const string ListViewColumns = @"
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
                c.subcollection_name AS SubcollectionName,
                c.plans AS Plans,
                c.ghes_support AS GhesSupport,
                c.tags_csv AS TagsCsv,
                c.is_ai AS IsAi,
                c.is_azure AS IsAzure,
                c.is_coding AS IsCoding,
                c.is_devops AS IsDevOps,
                c.is_github_copilot AS IsGitHubCopilot,
                c.is_ml AS IsMl,
                c.is_security AS IsSecurity";

    /// <summary>
    /// Column selection for detail views - includes content column for markdown rendering.
    /// Maps to ContentItemDetail record which extends ContentItem with the Content property.
    /// </summary>
    protected const string DetailViewColumns = @"
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
                c.ghes_support AS GhesSupport,
                c.tags_csv AS TagsCsv,
                c.is_ai AS IsAi,
                c.is_azure AS IsAzure,
                c.is_coding AS IsCoding,
                c.is_devops AS IsDevOps,
                c.is_github_copilot AS IsGitHubCopilot,
                c.is_ml AS IsMl,
                c.is_security AS IsSecurity";

    protected IDbConnection Connection { get; }
    protected ISqlDialect Dialect { get; }

    static DatabaseContentRepositoryBase()
    {
        // Register custom type handler to convert SQLite's Int64 booleans to C# bool
        SqlMapper.AddTypeHandler(new BooleanTypeHandler());
    }

    /// <summary>
    /// Type handler to convert SQLite's Int64 (0/1) to C# bool.
    /// SQLite doesn't have a native boolean type, so it stores booleans as integers.
    /// </summary>
    private sealed class BooleanTypeHandler : SqlMapper.TypeHandler<bool>
    {
        public override bool Parse(object value)
        {
            return value switch
            {
                long l => l != 0,
                int i => i != 0,
                bool b => b,
                _ => throw new ArgumentException($"Cannot convert {value.GetType()} to bool")
            };
        }

        public override void SetValue(System.Data.IDbDataParameter parameter, bool value)
        {
            parameter.Value = value ? 1 : 0;
        }
    }

    protected DatabaseContentRepositoryBase(
        IDbConnection connection,
        ISqlDialect dialect,
        IMemoryCache cache,
        IMarkdownService markdownService,
        IOptions<AppSettings> settings)
        : base(cache, markdownService, settings)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(dialect);

        Connection = connection;
        Dialect = dialect;
    }

    /// <summary>
    /// Internal implementation for getting content by slug.
    /// Uses SQL to query the database. Includes content column for detail view rendering.
    /// Returns ContentItemDetail which includes the markdown content.
    /// </summary>
    protected override async Task<ContentItemDetail?> GetBySlugInternalAsync(
        string collectionName,
        string slug,
        bool includeDraft,
        CancellationToken ct)
    {
        // Build WHERE clause conditionally to allow index usage
        var draftFilter = includeDraft ? "" : $"AND c.draft = {Dialect.GetBooleanLiteral(false)}";

        var sql = $@"
            SELECT {DetailViewColumns}
            FROM content_items c
            WHERE c.collection_name = @collectionName
              AND c.slug = @slug
              {draftFilter}";

        var item = await Connection.QuerySingleOrDefaultAsync<ContentItemDetail>(
            new CommandDefinition(sql, new { collectionName, slug }, cancellationToken: ct));

        return item;
    }

    /// <summary>
    /// Internal implementation for getting all content.
    /// Uses SQL to query the database. Excludes content column for performance.
    /// </summary>
    protected override async Task<IReadOnlyList<ContentItem>> GetAllInternalAsync(
        bool includeDraft,
        int limit,
        int offset,
        CancellationToken ct)
    {
        // Build WHERE clause conditionally to allow index usage (idx_content_draft_date)
        var whereClause = includeDraft ? "" : $"WHERE c.draft = {Dialect.GetBooleanLiteral(false)}";

        var sql = $@"
            SELECT {ListViewColumns}
            FROM content_items c
            {whereClause}
            ORDER BY c.date_epoch DESC
            LIMIT @limit OFFSET @offset";

        var items = await Connection.QueryAsync<ContentItem>(
            new CommandDefinition(sql, new { limit, offset }, cancellationToken: ct));

        return [.. items];
    }

    /// <summary>
    /// Internal implementation for getting content by collection.
    /// Uses SQL to query the database. Excludes content column for performance.
    /// </summary>
    protected override async Task<IReadOnlyList<ContentItem>> GetByCollectionInternalAsync(
        string collectionName,
        string? subcollectionName,
        bool includeDraft,
        int limit,
        int offset,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(collectionName);

        // Handle virtual "all" collection - return all items
        if (collectionName.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            return await GetAllInternalAsync(includeDraft, limit, offset, ct);
        }

        // Build WHERE clause - filter by collection and optionally by subcollection
        var whereClause = !string.IsNullOrWhiteSpace(subcollectionName)
            ? "c.collection_name = @collectionName AND c.subcollection_name = @subcollectionName"
            : "c.collection_name = @collectionName";

        // Add draft filter conditionally to allow index usage
        if (!includeDraft)
        {
            whereClause += $" AND c.draft = {Dialect.GetBooleanLiteral(false)}";
        }

        var sql = $@"
            SELECT {ListViewColumns}
            FROM content_items c
            WHERE {whereClause}
            ORDER BY c.date_epoch DESC
            LIMIT @limit OFFSET @offset";

        var items = await Connection.QueryAsync<ContentItem>(
            new CommandDefinition(sql, new { collectionName, subcollectionName, limit, offset }, cancellationToken: ct));

        return [.. items];
    }

    /// <summary>
    /// Internal implementation for getting content by section.
    /// Optionally filter by collection and subcollection.
    /// Uses SQL to query the database with dynamic section conditions for optimal index usage.
    /// Excludes content column for performance.
    /// </summary>
    protected override async Task<IReadOnlyList<ContentItem>> GetBySectionInternalAsync(
        string sectionName,
        string? collectionName,
        string? subcollectionName,
        bool includeDraft,
        int limit,
        int offset,
        CancellationToken ct)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sectionName);

        // Handle virtual "all" section
        if (sectionName.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            // If a specific collection is requested, filter by collection
            // Otherwise return all items
            if (!string.IsNullOrWhiteSpace(collectionName))
            {
                return await GetByCollectionInternalAsync(collectionName, subcollectionName, includeDraft, limit, offset, ct);
            }

            return await GetAllInternalAsync(includeDraft, limit, offset, ct);
        }

        // Build section condition using bitmask for idx_section_date index usage
        // idx_section_date: (collection_name, date_epoch DESC, sections_bitmask, tags_csv) WHERE draft = 0
        var sectionBitmask = CalculateSectionBitmask(sectionName);

        // Build WHERE clauses for index-optimized filtering
        var whereClauses = new List<string>();

        if (sectionBitmask > 0)
        {
            whereClauses.Add($"(c.sections_bitmask & {sectionBitmask}) > 0");
        }
        else
        {
            // Unknown section - no match (safe default)
            whereClauses.Add("1=0");
        }

        // Draft filtering - directly in SQL for index usage
        if (!includeDraft)
        {
            whereClauses.Add($"c.draft = {Dialect.GetBooleanLiteral(false)}");
        }

        // Add collection filter if specified
        if (!string.IsNullOrWhiteSpace(collectionName))
        {
            whereClauses.Add("c.collection_name = @collectionName");
        }

        // Add subcollection filter if specified
        if (!string.IsNullOrWhiteSpace(subcollectionName))
        {
            whereClauses.Add("c.subcollection_name = @subcollectionName");
        }

        var whereClause = string.Join(" AND ", whereClauses);

        var sql = $@"
            SELECT {ListViewColumns}
            FROM content_items c
            WHERE {whereClause}
            ORDER BY c.date_epoch DESC
            LIMIT @limit OFFSET @offset";

        var items = await Connection.QueryAsync<ContentItem>(
            new CommandDefinition(sql, new { collectionName, subcollectionName, limit, offset }, cancellationToken: ct));

        return [.. items];
    }

    /// <summary>
    /// Calculate bitmask value for section filtering.
    /// Bit 0 (1) = AI, Bit 1 (2) = Azure, Bit 2 (4) = Coding, Bit 3 (8) = DevOps,
    /// Bit 4 (16) = GitHub Copilot, Bit 5 (32) = ML, Bit 6 (64) = Security.
    /// </summary>
    /// <param name="sections">Collection of section names to include in bitmask</param>
    /// <returns>Integer bitmask with bits set for each matching section</returns>
    protected static int CalculateSectionBitmask(IEnumerable<string> sections)
    {
        ArgumentNullException.ThrowIfNull(sections);

        var bitmask = 0;
        foreach (var section in sections)
        {
            bitmask |= CalculateSectionBitmask(section);
        }

        return bitmask;
    }

    /// <summary>
    /// Calculate bitmask value for a single section.
    /// </summary>
    /// <param name="section">Section name</param>
    /// <returns>Integer bitmask value for the section (0 if unknown)</returns>
    protected static int CalculateSectionBitmask(string section)
    {
        ArgumentNullException.ThrowIfNull(section);

        var sectionNormalized = section.ToLowerInvariant().Trim();
        return sectionNormalized switch
        {
            "ai" => 1,
            "azure" => 2,
            "coding" => 4,
            "devops" => 8,
            "github-copilot" => 16,
            "ml" => 32,
            "security" => 64,
            _ => 0
        };
    }

    // ==================== Shared Implementations ====================
    // These methods use standard SQL that works with SQLite and PostgreSQL

    /// <summary>
    /// Get facet counts for tags, collections, and sections within the filtered scope.
    /// Uses standard SQL - works with both SQLite and PostgreSQL.
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
    /// Get tag counts using in-memory aggregation from tags_csv column.
    /// ULTRA-FAST: Parses comma-delimited tags from filtered content items in-memory.
    /// Uses standard SQL - works with both SQLite and PostgreSQL.
    /// Automatically excludes section and collection titles from tag counts.
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
        // Build exclude set from section/collection titles
        var excludeSet = await BuildSectionCollectionExcludeSet();
        
        var sql = new StringBuilder("SELECT tags_csv FROM content_items c");
        var parameters = new DynamicParameters();

        var whereClauses = new List<string> { $"c.draft = {Dialect.GetBooleanLiteral(false)}" };

        // Section filtering via bitmask column
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

        var tagsCsvRows = await Connection.QueryAsync<string>(
            new CommandDefinition(sql.ToString(), parameters, cancellationToken: ct));

        // Parse tags and count in-memory
        var tagCounts = new Dictionary<string, int>(StringComparer.Ordinal);

        foreach (var tagsCsv in tagsCsvRows)
        {
            if (string.IsNullOrEmpty(tagsCsv))
            {
                continue;
            }

            var tags = tagsCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var tag in tags)
            {
                if (!string.IsNullOrEmpty(tag) && !excludeSet.Contains(tag))
                {
                    tagCounts[tag] = tagCounts.GetValueOrDefault(tag) + 1;
                }
            }
        }

        // Filter by minimum uses, sort, and limit results (AFTER excluding section/collection tags)
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
    /// Build WHERE clauses for filtering content items.
    /// Reusable helper for facets, search, and other queries.
    /// </summary>
    protected List<string> BuildFilterWhereClauses(FacetRequest request, DynamicParameters parameters)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(parameters);

        var whereClauses = new List<string> { $"c.draft = {Dialect.GetBooleanLiteral(false)}" };

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
    /// Build optimized subquery for tag filtering using content_tags_expanded.
    /// Uses GROUP BY + HAVING COUNT(*) = N for multi-tag AND logic.
    /// Returns (collection_name, slug) pairs.
    /// </summary>
    protected static string BuildTagsTableQuery(SearchRequest request, DynamicParameters parameters)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(parameters);

        var sql = new StringBuilder("SELECT collection_name, slug FROM content_tags_expanded");

        var whereClauses = new List<string>();

        // Tag filtering
        var normalizedTags = request.Tags!.Select(t => t.ToLowerInvariant().Trim()).ToList();
        whereClauses.Add("tag_word IN @tags");
        parameters.Add("tags", normalizedTags);

        // Section filtering using bitmask
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

        // Date filtering
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

        // Order by max date_epoch DESC
        sql.Append(" ORDER BY MAX(date_epoch) DESC");

        // Limit results
        sql.Append(" LIMIT @take");
        parameters.Add("take", request.Take);

        return sql.ToString();
    }
}
