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
                id, title, content, excerpt, date_epoch, collection_name, subcollection_name,
                primary_section_name, external_url, author, feed_name, ghes_support, draft, content_hash
            ) VALUES (
                @Id, @Title, @Content, @Excerpt, @DateEpoch, @CollectionName, @SubcollectionName,
                @PrimarySectionName, @ExternalUrl, @Author, @FeedName, @GhesSupport, @Draft, @ContentHash
            )
            ON CONFLICT(id) DO UPDATE SET
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
        // SQLite FTS5 syntax
        return @"
            SELECT c.*, bm25(fts) AS rank
            FROM content_items c
            JOIN content_fts fts ON c.rowid = fts.rowid
            WHERE fts MATCH @SearchQuery";
    }

    public string GetRelevanceRankExpression()
    {
        return "bm25(fts)";
    }

    public string GetHighlightExpression(string columnName, string searchQuery)
    {
        // SQLite FTS5 snippet function
        return $"snippet(fts, -1, '<mark>', '</mark>', '...', 64)";
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
}
