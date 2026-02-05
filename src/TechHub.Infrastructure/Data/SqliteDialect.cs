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

    public string GetTimestampType()
    {
        // SQLite stores timestamps as TEXT in ISO8601 format
        return "TEXT";
    }

    public string GetCurrentTimestampDefault()
    {
        // SQLite datetime function
        return "datetime('now')";
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

    public string GetFullTextJoinClause()
    {
        // SQLite uses separate FTS5 virtual table
        return "INNER JOIN content_fts ON c.rowid = content_fts.rowid";
    }

    public string GetFullTextWhereClause(string paramName)
    {
        // SQLite FTS5 MATCH syntax
        return $"content_fts MATCH @{paramName}";
    }

    public string GetFullTextOrderByClause(string paramName)
    {
        // SQLite FTS5 bm25() ranking function (lower is better, so no DESC needed)
        return "bm25(content_fts)";
    }

    public string GetCollectionFilterClause(string paramName, int count)
    {
        // SQLite uses IN for list matching
        return $"IN @{paramName}";
    }
}
