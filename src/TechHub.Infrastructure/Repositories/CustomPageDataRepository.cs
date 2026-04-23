using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.Admin;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// Stores and retrieves custom page JSON data from PostgreSQL.
/// </summary>
public sealed class CustomPageDataRepository : ICustomPageDataRepository
{
    private readonly IDbConnection _connection;
    private readonly ILogger<CustomPageDataRepository> _logger;

    public CustomPageDataRepository(
        IDbConnection connection,
        ILogger<CustomPageDataRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(logger);

        _connection = connection;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<CustomPageEntry>> GetAllAsync(CancellationToken ct = default)
    {
        const string Sql = @"
SELECT key, description, json_data AS JsonData, updated_at AS UpdatedAt
FROM custom_page_data
ORDER BY key";

        var result = await _connection.QueryAsync<CustomPageEntry>(
            new CommandDefinition(Sql, cancellationToken: ct));
        return result.ToList();
    }

    /// <inheritdoc/>
    public async Task<CustomPageEntry?> GetByKeyAsync(string key, CancellationToken ct = default)
    {
        const string Sql = @"
SELECT key, description, json_data AS JsonData, updated_at AS UpdatedAt
FROM custom_page_data
WHERE key = @Key";

        return await _connection.QueryFirstOrDefaultAsync<CustomPageEntry>(
            new CommandDefinition(Sql, new { Key = key }, cancellationToken: ct));
    }

    /// <inheritdoc/>
    public async Task UpsertAsync(string key, string description, string jsonData, CancellationToken ct = default)
    {
        const string Sql = @"
INSERT INTO custom_page_data (key, description, json_data, updated_at)
VALUES (@Key, @Description, @JsonData, NOW())
ON CONFLICT (key) DO UPDATE
SET json_data  = EXCLUDED.json_data,
    updated_at = NOW()";

        await _connection.ExecuteAsync(
            new CommandDefinition(Sql, new { Key = key, Description = description, JsonData = jsonData }, cancellationToken: ct));

        _logger.LogDebug("Upserted custom page data for key {Key}", key);
    }

    /// <inheritdoc/>
    public async Task<bool> IsEmptyAsync(CancellationToken ct = default)
    {
        const string Sql = "SELECT COUNT(*) FROM custom_page_data";
        var count = await _connection.ExecuteScalarAsync<int>(
            new CommandDefinition(Sql, cancellationToken: ct));
        return count == 0;
    }
}
