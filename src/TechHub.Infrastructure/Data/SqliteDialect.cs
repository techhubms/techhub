using TechHub.Core.Interfaces;

namespace TechHub.Infrastructure.Data;

/// <summary>
/// SQLite-specific SQL dialect implementation.
/// Uses FTS5 for full-text search.
/// </summary>
public class SqliteDialect : ISqlDialect
{
    public string ProviderName => "SQLite";

    public string ParameterPrefix => "@";

    public bool SupportsFullTextSearch => true;

    public string GetUpsertContentSql()
    {
        return @"
            INSERT INTO content_items (
                slug, title, content, excerpt, date_epoch, collection_name, subcollection_name,
                primary_section_name, external_url, author, feed_name, ghes_support, draft, content_hash
            ) VALUES (
                @Slug, @Title, @Content, @Excerpt, @DateEpoch, @CollectionName, @SubcollectionName,
                @PrimarySectionName, @ExternalUrl, @Author, @FeedName, @GhesSupport, @Draft, @ContentHash
            )
            ON CONFLICT(slug) DO UPDATE SET
                title = @Title,
                content = @Content,
                excerpt = @Excerpt,
                date_epoch = @DateEpoch,
                collection_name = @CollectionName,
                subcollection_name = @SubcollectionName,
                primary_section_name = @PrimarySectionName,
                external_url = @ExternalUrl,
                author = @Author,
                feed_name = @FeedName,
                ghes_support = @GhesSupport,
                draft = @Draft,
                content_hash = @ContentHash,
                updated_at = CURRENT_TIMESTAMP";
    }

    public string GetFullTextSearchSql(string searchQuery)
    {
        // SQLite FTS5 syntax - must use table name, not alias, for MATCH and bm25()
        return @"
            SELECT c.*, bm25(content_fts) AS rank
            FROM content_items c
            JOIN content_fts ON c.rowid = content_fts.rowid
            WHERE content_fts MATCH @SearchQuery";
    }

    public string GetRelevanceRankExpression()
    {
        return "bm25(content_fts)";
    }

    public string GetHighlightExpression(string columnName, string searchQuery)
    {
        // SQLite FTS5 snippet function
        return $"snippet(content_fts, -1, '<mark>', '</mark>', '...', 64)";
    }

    public string GetPaginationSql(int take, string? continuationToken)
    {
        // Keyset pagination for SQLite
        if (string.IsNullOrEmpty(continuationToken))
        {
            return $"ORDER BY date_epoch DESC, id DESC LIMIT {take}";
        }

        return $"AND (date_epoch, id) < (@CursorDate, @CursorId) ORDER BY date_epoch DESC, id DESC LIMIT {take}";
    }

    public string CreateMigrationTableSql()
    {
        return """
            CREATE TABLE IF NOT EXISTS _migrations (
                script_name TEXT PRIMARY KEY,
                executed_at TEXT NOT NULL DEFAULT (datetime('now'))
            )
            """;
    }

    public object ConvertBooleanParameter(bool value)
    {
        // SQLite doesn't have native BOOLEAN type, uses INTEGER 0/1
        return value ? 1 : 0;
    }

    public string GetInsertIgnorePrefix(string tableName, string columns)
    {
        return $"INSERT OR IGNORE INTO {tableName} {columns} VALUES ";
    }

    public string GetInsertIgnoreSuffix()
    {
        // SQLite uses INSERT OR IGNORE syntax, no suffix needed
        return string.Empty;
    }
    public string GetBooleanLiteral(bool value)
    {
        // SQLite uses 0/1 for booleans
        return value ? "1" : "0";
    }
}
