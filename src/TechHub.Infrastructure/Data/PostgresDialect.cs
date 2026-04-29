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
        // Uses 'simple' config to preserve stop words (e.g., "vs" in "VS Code")
        return $"c.search_vector @@ to_tsquery('simple', @{paramName})";
    }

    public string GetFullTextOrderByClause(string paramName)
    {
        // PostgreSQL ts_rank function (higher is better)
        // Using to_tsquery with 'simple' config to match WHERE clause
        return $"ts_rank(c.search_vector, to_tsquery('simple', @{paramName})) DESC";
    }

    /// <summary>
    /// Known compound words and their expansions for search.
    /// Maps compound terms (lowercase) to their constituent parts.
    /// </summary>
    private static readonly Dictionary<string, string[]> _compoundWords = new(StringComparer.OrdinalIgnoreCase)
    {
        ["vscode"] = ["vs", "code"],
        ["dotnet"] = ["dot", "net"],
        ["devops"] = ["dev", "ops"],
        ["csharp"] = ["c", "sharp"],
        ["github"] = ["git", "hub"],
        ["openai"] = ["open", "ai"],
        ["chatgpt"] = ["chat", "gpt"],
        ["copilot"] = ["co", "pilot"],
    };

    public string TransformFullTextQuery(string query)
    {
        // PostgreSQL prefix search with OR logic for better recall:
        // - Uses | (OR) so matching ANY term surfaces results (ranked by relevance)
        // - Appends :* for prefix matching ("reinie" → "reinie:*" matches "Reinier")
        // - Expands known compound words ("vscode" → "vscode:* | vs:* | code:*")
        // Combined with ts_rank ordering, best matches still appear first
        if (string.IsNullOrWhiteSpace(query))
        {
            return query;
        }

        var terms = query.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var expandedTerms = new List<string>();

        foreach (var term in terms)
        {
            expandedTerms.Add($"{term}:*");

            // Expand known compound words to also match their parts
            if (_compoundWords.TryGetValue(term, out var parts))
            {
                foreach (var part in parts)
                {
                    expandedTerms.Add($"{part}:*");
                }
            }
        }

        return string.Join(" | ", expandedTerms);
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
