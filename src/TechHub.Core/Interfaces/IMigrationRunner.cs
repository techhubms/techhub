namespace TechHub.Core.Interfaces;

/// <summary>
/// Runs database migrations on application startup.
/// </summary>
public interface IMigrationRunner
{
    /// <summary>
    /// Run all pending migrations for the configured database provider.
    /// </summary>
    Task RunMigrationsAsync(CancellationToken cancellationToken = default);
}
