using Microsoft.Extensions.Diagnostics.HealthChecks;
using TechHub.Core.Interfaces;

namespace TechHub.Api.HealthChecks;

/// <summary>
/// Health check that verifies PostgreSQL connectivity by running a simple query.
/// Detects connection pool exhaustion and network partitions quickly.
/// </summary>
public class DatabaseHealthCheck : IHealthCheck
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DatabaseHealthCheck(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT 1";
            await ((System.Data.Common.DbCommand)command).ExecuteScalarAsync(cancellationToken);

            return HealthCheckResult.Healthy("Database connection is healthy");
        }
#pragma warning disable CA1031 // Intentionally broad: health checks must return Unhealthy, not throw
        catch (Exception ex)
#pragma warning restore CA1031
        {
            return HealthCheckResult.Unhealthy("Database connection failed", ex);
        }
    }
}
