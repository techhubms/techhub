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

    public string GetTimestampType()
    {
        // PostgreSQL native timestamp with timezone
        return "TIMESTAMPTZ";
    }

    public string GetCurrentTimestampDefault()
    {
        // PostgreSQL NOW() function
        return "NOW()";
    }

    public object ConvertBooleanParameter(bool value)
    {
        // PostgreSQL has native BOOLEAN type
        return value;
    }

    public string GetInsertIgnorePrefix(string tableName, string columns)
    {
        return $"INSERT INTO {tableName} {columns} VALUES ";
    }

    public string GetInsertIgnoreSuffix()
    {
        // PostgreSQL uses ON CONFLICT clause after VALUES
        return " ON CONFLICT DO NOTHING";
    }

    public string GetBooleanLiteral(bool value)
    {
        // PostgreSQL uses native boolean type
        return value ? "true" : "false";
    }
    public string GetFullTextJoinClause()
    {
        // PostgreSQL uses tsvector column directly, no join needed
        return string.Empty;
    }

    public string GetFullTextWhereClause(string paramName)
    {
        // PostgreSQL tsvector @@ operator with plainto_tsquery
        return $"c.search_vector @@ plainto_tsquery('english', @{paramName})";
    }

    public string GetFullTextOrderByClause(string paramName)
    {
        // PostgreSQL ts_rank function (higher is better)
        return $"ts_rank(c.search_vector, plainto_tsquery('english', @{paramName})) DESC";
    }

    public string GetCollectionFilterClause(string paramName, int count)
    {
        // PostgreSQL uses ANY for array matching (requires array parameter)
        return $"= ANY(@{paramName})";
    }
}
