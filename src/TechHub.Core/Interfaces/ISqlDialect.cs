namespace TechHub.Core.Interfaces;

/// <summary>
/// Abstraction for database-specific SQL syntax.
/// Allows repository implementations to use dialect-specific features like full-text search.
/// Currently only PostgreSQL is supported.
/// </summary>
public interface ISqlDialect
{
    /// <summary>
    /// Database provider name (e.g., "PostgreSQL")
    /// </summary>
    string ProviderName { get; }

    /// <summary>
    /// Parameter prefix for SQL queries (@)
    /// </summary>
    string ParameterPrefix { get; }

    /// <summary>
    /// Whether the database supports full-text search natively
    /// </summary>
    bool SupportsFullTextSearch { get; }

    /// <summary>
    /// Get SQL type for timestamp columns (TIMESTAMPTZ for PostgreSQL)
    /// </summary>
    string GetTimestampType();

    /// <summary>
    /// Get SQL default expression for current timestamp (NOW() for PostgreSQL)
    /// </summary>
    string GetCurrentTimestampDefault();

    /// <summary>
    /// Convert a boolean value to the appropriate parameter type
    /// </summary>
    object ConvertBooleanParameter(bool value);

    /// <summary>
    /// Get SQL for INSERT with conflict handling (INSERT ... ON CONFLICT DO NOTHING)
    /// Returns the prefix (e.g., "INSERT INTO tableName columns VALUES")
    /// </summary>
    string GetInsertIgnorePrefix(string tableName, string columns);

    /// <summary>
    /// Get SQL suffix for INSERT with conflict handling (" ON CONFLICT DO NOTHING")
    /// </summary>
    string GetInsertIgnoreSuffix();

    /// <summary>
    /// Get SQL literal for boolean values in WHERE clauses (false/true)
    /// </summary>
    string GetBooleanLiteral(bool value);

    /// <summary>
    /// Get SQL JOIN clause for full-text search (empty for PostgreSQL which uses tsvector column)
    /// </summary>
    string GetFullTextJoinClause();

    /// <summary>
    /// Get SQL WHERE clause for full-text search matching (search_vector @@ plainto_tsquery)
    /// </summary>
    string GetFullTextWhereClause(string paramName);

    /// <summary>
    /// Get SQL ORDER BY clause for full-text search relevance ranking
    /// </summary>
    string GetFullTextOrderByClause(string paramName);

    /// <summary>
    /// Transform user search query to enable prefix matching.
    /// PostgreSQL: Appends :* and escapes for tsquery prefix search (e.g., "reinie" → "reinie:*")
    /// </summary>
    /// <param name="query">User's search query</param>
    /// <returns>Transformed query for full-text search</returns>
    string TransformFullTextQuery(string query);

    /// <summary>
    /// Get SQL clause for list filtering (= ANY(@param) for PostgreSQL arrays)
    /// </summary>
    /// <param name="paramName">Parameter name (without @ prefix)</param>
    /// <param name="count">Number of items in the list</param>
    string GetListFilterClause(string paramName, int count);

    /// <summary>
    /// Convert a list of values to the appropriate parameter type (Array for PostgreSQL ANY operator)
    /// </summary>
    /// <typeparam name="T">Type of list elements</typeparam>
    /// <param name="values">Values to convert</param>
    /// <returns>Array for PostgreSQL</returns>
    object ConvertListParameter<T>(IEnumerable<T> values);

    /// <summary>
    /// DEPRECATED: Use GetListFilterClause instead.
    /// Get SQL IN clause for collection filtering (= ANY(@param) for PostgreSQL arrays)
    /// </summary>
    [Obsolete("Use GetListFilterClause instead for consistent list filtering")]
    string GetCollectionFilterClause(string paramName, int count);
}
