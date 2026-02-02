using System.Data;
using System.Reflection;
using Dapper;
using Microsoft.Extensions.Logging;
using TechHub.Core.Interfaces;

namespace TechHub.Infrastructure.Data;

/// <summary>
/// Executes SQL migration scripts automatically on application startup.
/// Runs all migration scripts in sequential order based on filename (001_, 002_, etc.).
/// Tracks executed migrations in a _migrations table to ensure each script runs only once.
/// Uses file-based locking to prevent concurrent migration execution (critical for SQLite in parallel tests).
/// </summary>
public class MigrationRunner
{
    private readonly IDbConnection _connection;
    private readonly ISqlDialect _dialect;
    private readonly ILogger<MigrationRunner> _logger;
    private static readonly SemaphoreSlim _migrationLock = new(1, 1);

    public MigrationRunner(
        IDbConnection connection,
        ISqlDialect dialect,
        ILogger<MigrationRunner> logger)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(dialect);
        ArgumentNullException.ThrowIfNull(logger);

        _connection = connection;
        _dialect = dialect;
        _logger = logger;
    }

    /// <summary>
    /// Run all pending migrations for the configured database provider.
    /// Acquires a global lock to prevent concurrent migrations (critical for SQLite parallel tests).
    /// </summary>
    public async Task RunMigrationsAsync(CancellationToken cancellationToken = default)
    {
        // Acquire lock to prevent concurrent migration execution
        // This is critical when running parallel E2E tests with SQLite
        await _migrationLock.WaitAsync(cancellationToken);
        try
        {
            await RunMigrationsInternalAsync(cancellationToken);
        }
        finally
        {
            _migrationLock.Release();
        }
    }

    private async Task RunMigrationsInternalAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Running database migrations for {Provider}...", _dialect.ProviderName);

        // Ensure migration tracking table exists
        await EnsureMigrationTableExistsAsync();

        var migrationFolder = _dialect.ProviderName.ToLowerInvariant() switch
        {
            "sqlite" => "sqlite",
            "postgresql" => "postgres",
            _ => throw new NotSupportedException($"Unknown database provider: {_dialect.ProviderName}")
        };

        var assembly = Assembly.GetExecutingAssembly();
        var migrationPrefix = $"TechHub.Infrastructure.Data.Migrations.{migrationFolder}.";

        // Get all migration scripts sorted by filename
        var migrationScripts = assembly.GetManifestResourceNames()
            .Where(name => name.StartsWith(migrationPrefix, StringComparison.Ordinal) && name.EndsWith(".sql", StringComparison.Ordinal))
            .OrderBy(name => name)
            .ToList();

        if (migrationScripts.Count == 0)
        {
            _logger.LogWarning("No migration scripts found for {Provider}", _dialect.ProviderName);
            return;
        }

        _logger.LogInformation("Found {Count} migration scripts", migrationScripts.Count);

        // Get already executed migrations
        var executedMigrations = await GetExecutedMigrationsAsync();

        var pendingCount = 0;
        foreach (var scriptName in migrationScripts)
        {
            var scriptFileName = Path.GetFileName(scriptName);

            if (executedMigrations.Contains(scriptFileName))
            {
                _logger.LogDebug("Skipping already executed migration: {ScriptName}", scriptFileName);
                continue;
            }

            await RunMigrationScriptAsync(assembly, scriptName, cancellationToken);
            pendingCount++;
        }

        if (pendingCount == 0)
        {
            _logger.LogInformation("No pending migrations to run");
        }
        else
        {
            _logger.LogInformation("Database migrations completed successfully ({Count} migrations executed)", pendingCount);
        }
    }

    private async Task EnsureMigrationTableExistsAsync()
    {
        var sql = _dialect.CreateMigrationTableSql();
        await _connection.ExecuteAsync(sql);
    }

    private async Task<HashSet<string>> GetExecutedMigrationsAsync()
    {
        const string sql = "SELECT script_name FROM _migrations";
        var migrations = await _connection.QueryAsync<string>(sql);
        return migrations.ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    private async Task RecordMigrationAsync(string scriptFileName)
    {
        const string sql = "INSERT INTO _migrations (script_name) VALUES (@ScriptName)";
        await _connection.ExecuteAsync(sql, new { ScriptName = scriptFileName });
    }

    private async Task RunMigrationScriptAsync(
        Assembly assembly,
        string scriptName,
        CancellationToken cancellationToken)
    {
        var scriptFileName = Path.GetFileName(scriptName);
        _logger.LogInformation("Running migration: {ScriptName}", scriptFileName);

        using var stream = assembly.GetManifestResourceStream(scriptName) ?? throw new InvalidOperationException($"Migration script not found: {scriptName}");
        using var reader = new StreamReader(stream);
        var sql = await reader.ReadToEndAsync(cancellationToken);

        try
        {
            await _connection.ExecuteAsync(sql);
            await RecordMigrationAsync(scriptFileName);
            _logger.LogInformation("Migration {ScriptName} completed successfully", scriptFileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Migration {ScriptName} failed", scriptFileName);
            throw;
        }
    }
}
