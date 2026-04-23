using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.Admin;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// Stores and retrieves background job enabled/disabled settings from PostgreSQL.
/// </summary>
public sealed class BackgroundJobSettingRepository : IBackgroundJobSettingRepository
{
    private readonly IDbConnection _connection;
    private readonly ILogger<BackgroundJobSettingRepository> _logger;

    public BackgroundJobSettingRepository(
        IDbConnection connection,
        ILogger<BackgroundJobSettingRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(logger);

        _connection = connection;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<BackgroundJobSetting>> GetAllAsync(CancellationToken ct = default)
    {
        const string Sql = @"
SELECT job_name AS JobName, enabled, description
FROM background_job_settings
ORDER BY job_name";

        var result = await _connection.QueryAsync<BackgroundJobSetting>(
            new CommandDefinition(Sql, cancellationToken: ct));
        return result.ToList();
    }

    /// <inheritdoc/>
    public async Task<BackgroundJobSetting?> GetByNameAsync(string jobName, CancellationToken ct = default)
    {
        const string Sql = @"
SELECT job_name AS JobName, enabled, description
FROM background_job_settings
WHERE job_name = @JobName";

        return await _connection.QuerySingleOrDefaultAsync<BackgroundJobSetting>(
            new CommandDefinition(Sql, new { JobName = jobName }, cancellationToken: ct));
    }

    /// <inheritdoc/>
    public async Task<bool> IsEnabledAsync(string jobName, CancellationToken ct = default)
    {
        const string Sql = @"
SELECT enabled
FROM background_job_settings
WHERE job_name = @JobName";

        var result = await _connection.QuerySingleOrDefaultAsync<bool?>(
            new CommandDefinition(Sql, new { JobName = jobName }, cancellationToken: ct));
        return result ?? false;
    }

    /// <inheritdoc/>
    public async Task<bool> SetEnabledAsync(string jobName, bool enabled, CancellationToken ct = default)
    {
        const string Sql = @"
UPDATE background_job_settings
SET enabled = @Enabled
WHERE job_name = @JobName";

        var affected = await _connection.ExecuteAsync(
            new CommandDefinition(Sql, new { JobName = jobName, Enabled = enabled }, cancellationToken: ct));

        if (affected > 0)
        {
            _logger.LogInformation("Background job {JobName} {State}", jobName, enabled ? "enabled" : "disabled");
        }

        return affected > 0;
    }
}
