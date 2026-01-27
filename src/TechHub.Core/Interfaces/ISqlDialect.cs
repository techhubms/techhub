namespace TechHub.Core.Interfaces;

/// <summary>
/// Abstraction for database-specific SQL syntax differences between SQLite and PostgreSQL.
/// Allows repository implementations to write dialect-neutral queries where possible,
/// with dialect-specific overrides for features like full-text search.
/// </summary>
public interface ISqlDialect
{
    /// <summary>
    /// Database provider name (e.g., "SQLite", "PostgreSQL")
    /// </summary>
    string ProviderName { get; }

    /// <summary>
    /// Parameter prefix for SQL queries (@ for both SQLite and PostgreSQL)
    /// </summary>
    string ParameterPrefix { get; }

    /// <summary>
    /// Get SQL for upserting a content item (INSERT ... ON CONFLICT for both)
    /// </summary>
    string GetUpsertContentSql();

    /// <summary>
    /// Get SQL for full-text search (FTS5 for SQLite, tsvector for PostgreSQL)
    /// </summary>
    string GetFullTextSearchSql(string searchQuery);

    /// <summary>
    /// Get SQL for calculating relevance rank in full-text search
    /// </summary>
    string GetRelevanceRankExpression();

    /// <summary>
    /// Get SQL for highlighting search results
    /// </summary>
    string GetHighlightExpression(string columnName, string searchQuery);

    /// <summary>
    /// Get SQL for LIMIT/OFFSET pagination
    /// </summary>
    string GetPaginationSql(int take, string? continuationToken);

    /// <summary>
    /// Supports full-text search natively (true for both SQLite FTS5 and PostgreSQL tsvector)
    /// </summary>
    bool SupportsFullTextSearch { get; }
}
