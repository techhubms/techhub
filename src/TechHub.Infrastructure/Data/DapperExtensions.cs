using System.Data;
using System.Diagnostics;
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

            LogQuery(logger, sql, parameters, stopwatch.ElapsedMilliseconds, result.Count());

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

            LogQuery(logger, sql, parameters, stopwatch.ElapsedMilliseconds, null);

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
    /// Log query with SQL, sanitized parameters, execution time, and row count.
    /// </summary>
    private static void LogQuery(ILogger logger, string sql, object? parameters, long elapsedMs, int? rowCount)
    {
        var sanitizedParams = SanitizeParameters(parameters);
        var rowInfo = rowCount.HasValue ? $", Rows={rowCount}" : "";

        logger.LogInformation(
            "Query executed: {ElapsedMs}ms{RowInfo}\nSQL: {Sql}\nParams: {Params}",
            elapsedMs,
            rowInfo,
            sql.Trim(),
            sanitizedParams);
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
            var props = new List<string>();

            // Note: DynamicParameters doesn't expose its internal dictionary publicly
            // We can only log the type info for security
            // To get actual values, we'd need reflection which isn't worth the complexity/security risk
            props.Add($"DynamicParameters({dynamicParams.ParameterNames.Count()} params)");

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
        if (lowerName.Contains("password") || lowerName.Contains("secret") || lowerName.Contains("token"))
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
