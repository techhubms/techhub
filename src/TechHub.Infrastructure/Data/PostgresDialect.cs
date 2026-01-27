using TechHub.Core.Interfaces;

namespace TechHub.Infrastructure.Data;

/// <summary>
/// PostgreSQL-specific SQL dialect implementation.
/// Uses tsvector for full-text search.
/// </summary>
public class PostgresDialect : ISqlDialect
{
    public string ProviderName => "PostgreSQL";

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
                updated_at = NOW()";
    }

    public string GetFullTextSearchSql(string searchQuery)
    {
        // PostgreSQL tsvector syntax
        return @"
            SELECT c.*, ts_rank(c.search_vector, query) AS rank
            FROM content_items c,
                 plainto_tsquery('english', @SearchQuery) AS query
            WHERE c.search_vector @@ query";
    }

    public string GetRelevanceRankExpression()
    {
        return "ts_rank(search_vector, plainto_tsquery('english', @SearchQuery))";
    }

    public string GetHighlightExpression(string columnName, string searchQuery)
    {
        // PostgreSQL ts_headline function
        return $"ts_headline('english', {columnName}, plainto_tsquery('english', @SearchQuery), 'StartSel=<mark>, StopSel=</mark>')";
    }

    public string GetPaginationSql(int take, string? continuationToken)
    {
        // Keyset pagination for PostgreSQL
        if (string.IsNullOrEmpty(continuationToken))
        {
            return $"ORDER BY date_epoch DESC, id DESC LIMIT {take}";
        }

        return $"AND (date_epoch, id) < (@CursorDate, @CursorId) ORDER BY date_epoch DESC, id DESC LIMIT {take}";
    }
}
