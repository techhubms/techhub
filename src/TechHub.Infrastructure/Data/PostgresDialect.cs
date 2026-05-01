using System.Text.RegularExpressions;
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
    /// Parts shorter than 3 characters are excluded to avoid overly broad prefix matches.
    /// </summary>
    private static readonly Dictionary<string, string[]> _compoundWords = new(StringComparer.OrdinalIgnoreCase)
    {
        ["vscode"] = ["code"],
        ["dotnet"] = ["dot", "net"],
        ["devops"] = ["dev", "ops"],
        ["csharp"] = ["sharp"],
        ["github"] = ["git", "hub"],
        ["openai"] = ["open"],
        ["chatgpt"] = ["chat", "gpt"],
        ["copilot"] = ["pilot"],
    };

    /// <summary>
    /// Matches runs of alphanumeric characters (word tokens).
    /// Separates terms on anything that is not a letter or digit:
    /// hyphens ("-"), slashes ("/"), underscores ("_"), periods ("."),
    /// apostrophes ("'"), tsquery operators (&amp; | ! ( ) : * &lt; &gt;), etc.
    /// </summary>
    private static readonly Regex _wordTokenPattern =
        new(@"[a-zA-Z0-9]+", RegexOptions.Compiled);

    public string TransformFullTextQuery(string query)
    {
        // PostgreSQL prefix search with OR logic for better recall:
        // - Extracts alphanumeric word tokens from the raw query, stripping
        //   ALL non-alphanumeric characters including:
        //     • hyphens ("-") — PostgreSQL to_tsquery treats "-" as NOT operator,
        //       so "auto-approval" would mean "auto AND NOT approval" without this fix
        //     • underscores, slashes, periods — natural word separators in tech text
        //     • tsquery operators (&, |, !, :, *, (, ), <, >) — syntax errors if passed through
        // - Uses | (OR) so matching ANY term surfaces results (ranked by relevance)
        // - Appends :* for prefix matching ("reinie" → "reinie:*" matches "Reinier")
        // - Expands known compound words ("vscode" → "vscode:* | code:*")
        // Combined with ts_rank ordering, best matches still appear first
        if (string.IsNullOrWhiteSpace(query))
        {
            return query;
        }

        var seenTerms = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var expandedTerms = new List<string>();

        foreach (Match match in _wordTokenPattern.Matches(query))
        {
            var term = match.Value;

            // Require at least 2 characters to avoid overly broad prefix matches from
            // single-character fragments produced by splitting (e.g., "A" from "A-Z")
            if (term.Length < 2)
            {
                continue;
            }

            if (!seenTerms.Add(term))
            {
                continue;
            }

            expandedTerms.Add($"{term}:*");

            // Expand known compound words to also match their parts
            if (_compoundWords.TryGetValue(term, out var parts))
            {
                foreach (var part in parts)
                {
                    // Parts shorter than 3 characters are excluded to avoid overly broad matches
                    if (part.Length >= 3 && seenTerms.Add(part))
                    {
                        expandedTerms.Add($"{part}:*");
                    }
                }
            }
        }

        // If nothing usable was extracted (e.g., all single characters), return original
        // so the caller can handle the empty/invalid case consistently
        if (expandedTerms.Count == 0)
        {
            return query;
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
