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
    /// Supports full-text search natively (true for both SQLite FTS5 and PostgreSQL tsvector)
    /// </summary>
    bool SupportsFullTextSearch { get; }

    /// <summary>
    /// Get SQL type for timestamp columns
    /// (TEXT for SQLite, TIMESTAMPTZ for PostgreSQL)
    /// </summary>
    string GetTimestampType();

    /// <summary>
    /// Get SQL default expression for current timestamp
    /// (datetime('now') for SQLite, NOW() for PostgreSQL)
    /// </summary>
    string GetCurrentTimestampDefault();

    /// <summary>
    /// Convert a boolean value to the appropriate type for this database
    /// (int 0/1 for SQLite, bool for PostgreSQL)
    /// </summary>
    object ConvertBooleanParameter(bool value);

    /// <summary>
    /// Get SQL for INSERT with conflict handling
    /// (INSERT OR IGNORE for SQLite, INSERT ... ON CONFLICT DO NOTHING for PostgreSQL)
    /// Returns the prefix (e.g., "INSERT OR IGNORE INTO tableName columns VALUES")
    /// </summary>
    string GetInsertIgnorePrefix(string tableName, string columns);

    /// <summary>
    /// Get SQL suffix for INSERT with conflict handling
    /// (empty for SQLite, " ON CONFLICT DO NOTHING" for PostgreSQL)
    /// </summary>
    string GetInsertIgnoreSuffix();

    /// <summary>
    /// Get SQL literal for boolean values in WHERE clauses
    /// (0/1 for SQLite, false/true for PostgreSQL)
    /// </summary>
    string GetBooleanLiteral(bool value);

    /// <summary>
    /// Get SQL JOIN clause for full-text search
    /// (INNER JOIN content_fts for SQLite, empty for PostgreSQL which uses tsvector column)
    /// </summary>
    string GetFullTextJoinClause();

    /// <summary>
    /// Get SQL WHERE clause for full-text search matching
    /// (content_fts MATCH @param for SQLite, search_vector @@ plainto_tsquery for PostgreSQL)
    /// </summary>
    string GetFullTextWhereClause(string paramName);

    /// <summary>
    /// Get SQL ORDER BY clause for full-text search relevance ranking
    /// (content_fts.rank for SQLite FTS5, ts_rank(search_vector, plainto_tsquery) DESC for PostgreSQL)
    /// </summary>
    string GetFullTextOrderByClause(string paramName);

    /// <summary>
    /// Get SQL IN clause for list filtering (collections, tags, etc.)
    /// (IN @param for SQLite, = ANY(@param) for PostgreSQL arrays)
    /// </summary>
    /// <param name="paramName">Parameter name (without @ prefix)</param>
    /// <param name="count">Number of items in the list (unused but kept for compatibility)</param>
    string GetListFilterClause(string paramName, int count);

    /// <summary>
    /// Convert a list of values to the appropriate parameter type for this database.
    /// (List for SQLite/Dapper IN expansion, Array for PostgreSQL ANY operator)
    /// </summary>
    /// <typeparam name="T">Type of list elements</typeparam>
    /// <param name="values">Values to convert</param>
    /// <returns>List for SQLite, Array for PostgreSQL</returns>
    object ConvertListParameter<T>(IEnumerable<T> values);

    /// <summary>
    /// DEPRECATED: Use GetListFilterClause instead.
    /// Get SQL IN clause for collection filtering
    /// (IN @param for SQLite, = ANY(@param) for PostgreSQL arrays)
    /// </summary>
    [Obsolete("Use GetListFilterClause instead for consistent list filtering")]
    string GetCollectionFilterClause(string paramName, int count);
}
