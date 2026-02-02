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

    /// <summary>
    /// Calculate bitmask value for section filtering.
    /// Bit 0 (1) = AI, Bit 1 (2) = Azure, Bit 2 (4) = Coding, Bit 3 (8) = DevOps,
    /// Bit 4 (16) = GitHub Copilot, Bit 5 (32) = ML, Bit 6 (64) = Security.
    /// </summary>
    /// <param name="sections">Collection of section names to include in bitmask</param>
    /// <returns>Integer bitmask with bits set for each matching section</returns>
    protected static int CalculateSectionBitmask(IEnumerable<string> sections)
    {
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
}
