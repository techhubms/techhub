using System.Data;
using Dapper;
using Microsoft.Extensions.Caching.Memory;
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
        IMarkdownService markdownService)
        : base(cache, markdownService)
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
        var draftFilter = includeDraft ? "" : "AND c.draft = 0";
        
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
        var whereClause = includeDraft ? "" : "WHERE c.draft = 0";
        
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
            whereClause += " AND c.draft = 0";
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

        // Handle virtual "all" section - return all items
        if (sectionName.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            return await GetAllInternalAsync(includeDraft, limit, offset, ct);
        }

        // Build section condition directly in SQL (not parameterized) so SQLite can use partial indexes
        // idx_section_ai, idx_section_azure, etc. have WHERE is_* = 1 AND draft = 0
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
            _ => "1=0" // Unknown section - no match (safe default)
        };

        // Build WHERE clauses for index-optimized filtering
        var whereClauses = new List<string> { sectionCondition };

        // Draft filtering - directly in SQL for index usage
        if (!includeDraft)
        {
            whereClauses.Add("c.draft = 0");
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
}
