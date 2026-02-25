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
        // PostgreSQL tsvector @@ operator with to_tsquery for prefix support
        // Using to_tsquery instead of plainto_tsquery to support :* prefix syntax
        return $"c.search_vector @@ to_tsquery('english', @{paramName})";
    }

    public string GetFullTextOrderByClause(string paramName)
    {
        // PostgreSQL ts_rank function (higher is better)
        // Using to_tsquery to match WHERE clause
        return $"ts_rank(c.search_vector, to_tsquery('english', @{paramName})) DESC";
    }

    public string TransformFullTextQuery(string query)
    {
        // PostgreSQL prefix search: replace spaces with & and append :* for prefix matching
        // Example: "reinie" → "reinie:*" matches "Reinier"
        // Example: "github copilot" → "github:* & copilot:*" for multi-word prefix
        if (string.IsNullOrWhiteSpace(query))
        {
            return query;
        }

        var terms = query.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return string.Join(" & ", terms.Select(term => $"{term}:*"));
    }

    public string GetCollectionFilterClause(string paramName, int count)
    {
        // PostgreSQL uses ANY for array matching (requires array parameter)
        return $"= ANY(@{paramName})";
    }

    public string GetListFilterClause(string paramName, int count)
    {
        // PostgreSQL uses ANY for array matching (same as GetCollectionFilterClause)
        return $"= ANY(@{paramName})";
    }

    public object ConvertListParameter<T>(IEnumerable<T> values)
    {
        // PostgreSQL requires Array<T> for ANY operator
        return values.ToArray();
    }
}
