using System.Data;
using System.Reflection;
using Dapper;
using Microsoft.Extensions.Logging;
using TechHub.Core.Interfaces;

namespace TechHub.Infrastructure.Data;

/// <summary>
/// Executes SQL migration scripts automatically on application startup.
/// Runs all migration scripts in sequential order based on filename (001_, 002_, etc.).
/// </summary>
public class MigrationRunner(
    IDbConnection connection,
    ISqlDialect dialect,
    ILogger<MigrationRunner> logger)
{
    private readonly IDbConnection _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    private readonly ISqlDialect _dialect = dialect ?? throw new ArgumentNullException(nameof(dialect));
    private readonly ILogger<MigrationRunner> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Run all pending migrations for the configured database provider.
    /// </summary>
    public async Task RunMigrationsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Running database migrations for {Provider}...", _dialect.ProviderName);

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
            .Where(name => name.StartsWith(migrationPrefix) && name.EndsWith(".sql"))
            .OrderBy(name => name)
            .ToList();

        if (migrationScripts.Count == 0)
        {
            _logger.LogWarning("No migration scripts found for {Provider}", _dialect.ProviderName);
            return;
        }

        _logger.LogInformation("Found {Count} migration scripts", migrationScripts.Count);

        foreach (var scriptName in migrationScripts)
        {
            await RunMigrationScriptAsync(assembly, scriptName, cancellationToken);
        }

        _logger.LogInformation("Database migrations completed successfully");
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
            _logger.LogInformation("Migration {ScriptName} completed successfully", scriptFileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Migration {ScriptName} failed", scriptFileName);
            throw;
        }
    }
}
