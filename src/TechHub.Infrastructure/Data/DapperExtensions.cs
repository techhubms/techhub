using System.Data;
using System.Diagnostics;
using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;

namespace TechHub.Infrastructure.Data;

/// <summary>
/// Dapper extensions for query logging and diagnostics.
/// Provides optional query logging with SQL, parameters, and execution timing.
/// </summary>
public static class DapperExtensions
{
    /// <summary>
    /// Execute a query with optional logging of SQL, parameters, and execution time.
    /// </summary>
    public static async Task<IEnumerable<T>> QueryWithLoggingAsync<T>(
        this IDbConnection connection,
        CommandDefinition command,
        ILogger? logger,
        bool enableLogging)
    {
        if (!enableLogging || logger == null)
        {
            return await connection.QueryAsync<T>(command);
        }

        var stopwatch = Stopwatch.StartNew();
        var sql = command.CommandText;
        var parameters = command.Parameters;

        try
        {
            var result = await connection.QueryAsync<T>(command);
            stopwatch.Stop();

            LogQuery(logger, sql, parameters, stopwatch.ElapsedMilliseconds, result.Count(), connection);

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            logger.LogError(ex, "Query failed after {ElapsedMs}ms: {Sql}", stopwatch.ElapsedMilliseconds, sql);
            throw;
        }
    }

    /// <summary>
    /// Execute multiple queries with optional logging of SQL, parameters, and execution time.
    /// </summary>
    public static async Task<SqlMapper.GridReader> QueryMultipleWithLoggingAsync(
        this IDbConnection connection,
        CommandDefinition command,
        ILogger? logger,
        bool enableLogging)
    {
        if (!enableLogging || logger == null)
        {
            return await connection.QueryMultipleAsync(command);
        }

        var stopwatch = Stopwatch.StartNew();
        var sql = command.CommandText;
        var parameters = command.Parameters;

        try
        {
            var result = await connection.QueryMultipleAsync(command);
            stopwatch.Stop();

            LogQuery(logger, sql, parameters, stopwatch.ElapsedMilliseconds, null, connection);

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            logger.LogError(ex, "Multi-query failed after {ElapsedMs}ms: {Sql}", stopwatch.ElapsedMilliseconds, sql);
            throw;
        }
    }

    /// <summary>
    /// Threshold in milliseconds for automatic EXPLAIN logging.
    /// Queries exceeding this duration will have their EXPLAIN plan logged.
    /// </summary>
    internal const int SlowQueryThresholdMs = 1000;

    /// <summary>
    /// Log query with SQL, sanitized parameters, execution time, and row count.
    /// If the query exceeds <see cref="SlowQueryThresholdMs"/>, automatically runs
    /// EXPLAIN on the query and logs the execution plan at Warning level.
    /// </summary>
    private static void LogQuery(ILogger logger, string sql, object? parameters, long elapsedMs, int? rowCount, IDbConnection? connection = null)
    {
        var sanitizedParams = SanitizeParameters(parameters);
        var rowInfo = rowCount.HasValue ? $", Rows={rowCount}" : "";

        if (elapsedMs > SlowQueryThresholdMs)
        {
            logger.LogWarning(
                "SLOW QUERY: {ElapsedMs}ms{RowInfo}\nSQL: {Sql}\nParams: {Params}",
                elapsedMs,
                rowInfo,
                sql.Trim(),
                sanitizedParams);

            // Automatically run EXPLAIN for slow queries to aid debugging
            if (connection != null)
            {
                LogExplainPlan(connection, sql, parameters, logger);
            }
        }
        else
        {
            logger.LogInformation(
                "Query executed: {ElapsedMs}ms{RowInfo}\nSQL: {Sql}\nParams: {Params}",
                elapsedMs,
                rowInfo,
                sql.Trim(),
                sanitizedParams);
        }
    }

    /// <summary>
    /// Run EXPLAIN on the given SQL and log the execution plan.
    /// Uses EXPLAIN (ANALYZE, BUFFERS) for PostgreSQL.
    /// </summary>
    private static void LogExplainPlan(IDbConnection connection, string sql, object? parameters, ILogger logger)
    {
        try
        {
            var explainSql = $"EXPLAIN (ANALYZE, BUFFERS) {sql}";

            var command = new CommandDefinition(explainSql, parameters);
            var explainRows = connection.Query<dynamic>(command);

            var sb = new StringBuilder();
            sb.AppendLine("EXPLAIN plan for slow query:");
            foreach (var row in explainRows)
            {
                // PostgreSQL EXPLAIN returns a single "QUERY PLAN" text column
                var dict = (IDictionary<string, object>)row;
                foreach (var val in dict.Values)
                {
                    sb.Append("  ").AppendLine(val?.ToString());
                }
            }

            logger.LogWarning("{ExplainPlan}", sb.ToString().TrimEnd());
        }
        catch (Exception ex) when (ex is not OutOfMemoryException)
        {
            logger.LogWarning(ex, "Failed to retrieve EXPLAIN plan for slow query");
        }
    }

    /// <summary>
    /// Convert parameters to a readable string format.
    /// Sanitizes potentially sensitive values.
    /// </summary>
    private static string SanitizeParameters(object? parameters)
    {
        if (parameters == null)
        {
            return "(none)";
        }

        // If DynamicParameters, extract to dictionary for logging
        if (parameters is DynamicParameters dynamicParams)
        {
            var props = new List<string>
            {
                // Note: DynamicParameters doesn't expose its internal dictionary publicly
                // We can only log the type info for security
                // To get actual values, we'd need reflection which isn't worth the complexity/security risk
                $"DynamicParameters({dynamicParams.ParameterNames.Count()} params)"
            };

            return string.Join(", ", props);
        }

        // For anonymous objects, extract properties
        var properties = parameters.GetType().GetProperties();
        var values = properties.Select(p =>
        {
            var value = p.GetValue(parameters);
            var sanitizedValue = SanitizeValue(p.Name, value);
            return $"{p.Name}={sanitizedValue}";
        });

        return string.Join(", ", values);
    }

    /// <summary>
    /// Sanitize individual parameter values to prevent logging sensitive data.
    /// </summary>
    private static string SanitizeValue(string name, object? value)
    {
        if (value == null)
        {
            return "null";
        }

        var lowerName = name.ToLowerInvariant();

        // Sanitize potentially sensitive fields
        if (lowerName.Contains("password", StringComparison.Ordinal) || lowerName.Contains("secret", StringComparison.Ordinal) || lowerName.Contains("token", StringComparison.Ordinal))
        {
            return "***";
        }

        // Truncate long strings
        if (value is string str && str.Length > 100)
        {
            return $"\"{str[..97]}...\"";
        }

        // Format common types
        if (value is string s)
        {
            return $"\"{s}\"";
        }

        if (value is DateTime || value is DateTimeOffset)
        {
            return value.ToString() ?? "null";
        }

        if (value is IEnumerable<object> enumerable && value is not string)
        {
            var items = enumerable.Take(10).ToList();
            var preview = string.Join(", ", items);
            return items.Count < 10 ? $"[{preview}]" : $"[{preview}, ...]";
        }

        return value.ToString() ?? "null";
    }
}
