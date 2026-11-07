using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.FeatureToggles;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// Stores and retrieves the feature toggles from PostgreSQL.
/// </summary>
public sealed class FeatureToggleRepository : IFeatureToggleRepository
{
    private readonly IDbConnection _connection;
    private readonly ILogger<FeatureToggleRepository> _logger;

    public FeatureToggleRepository(
        IDbConnection connection,
        ILogger<FeatureToggleRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(logger);

        _connection = connection;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<FeatureToggle>> GetAllAsync(CancellationToken ct = default)
    {
        const string Sql = @"
SELECT feature_key AS Key, mode AS Mode, description AS Description
FROM feature_toggles
ORDER BY feature_key";

        var result = await _connection.QueryAsync<FeatureToggle>(
            new CommandDefinition(Sql, cancellationToken: ct));
        return result.ToList();
    }

    /// <inheritdoc/>
    public async Task<bool> SetModeAsync(string featureKey, FeatureToggleMode mode, CancellationToken ct = default)
    {
        const string Sql = @"
UPDATE feature_toggles
SET mode = @Mode, updated_at = NOW()
WHERE feature_key = @FeatureKey";

        var affected = await _connection.ExecuteAsync(
            new CommandDefinition(
                Sql,
                new { FeatureKey = featureKey, Mode = mode.ToString().ToLowerInvariant() },
                cancellationToken: ct));

        if (affected > 0)
        {
            _logger.LogInformation("Feature {FeatureKey} is now {Mode}", featureKey, mode);
        }

        return affected > 0;
    }
}
