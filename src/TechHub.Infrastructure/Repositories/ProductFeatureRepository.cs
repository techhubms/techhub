using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using TechHub.Core.Interfaces;
using TechHub.Core.Logging;
using TechHub.Core.Models.Admin;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// PostgreSQL implementation of <see cref="IProductFeatureRepository"/>: the curated features of a product and the
/// links between the feature text the AI returned for timeline events and those features.
/// </summary>
public sealed class ProductFeatureRepository : IProductFeatureRepository
{
    private readonly IDbConnection _connection;
    private readonly ILogger<ProductFeatureRepository> _logger;

    public ProductFeatureRepository(IDbConnection connection, ILogger<ProductFeatureRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(logger);

        _connection = connection;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<ProductFeatureOverview> GetOverviewAsync(long productId, CancellationToken ct = default)
    {
        const string FeaturesSql = """
            SELECT f.id         AS Id,
                   f.product_id AS ProductId,
                   f.name       AS Name,
                   (SELECT COUNT(*) FROM content_timeline_events e WHERE e.feature_id = f.id)::int AS EventCount
            FROM product_features f
            WHERE f.product_id = @ProductId
            ORDER BY lower(f.name), f.name
            """;

        var features = (await _connection.QueryAsync<ProductFeature>(
            new CommandDefinition(FeaturesSql, new { ProductId = productId }, cancellationToken: ct))).ToList();

        var aliasRows = await _connection.QueryAsync<AliasRow>(new CommandDefinition(
            """
            SELECT a.id AS Id, a.feature_id AS FeatureId, a.alias AS Alias
            FROM product_feature_aliases a
            JOIN product_features f ON f.id = a.feature_id
            WHERE f.product_id = @ProductId
            ORDER BY lower(a.alias), a.alias
            """,
            new { ProductId = productId },
            cancellationToken: ct));
        var aliases = aliasRows
            .GroupBy(a => a.FeatureId)
            .ToDictionary(g => g.Key, g => (IReadOnlyList<FeatureAlias>)g.Select(a => new FeatureAlias { Id = a.Id, Alias = a.Alias }).ToList());
        foreach (var feature in features)
        {
            feature.Aliases = aliases.TryGetValue(feature.Id, out var list) ? list : [];
        }

        const string UnmatchedSql = """
            SELECT MIN(regexp_replace(btrim(e.feature), '\s+', ' ', 'g')) AS Name,
                   COUNT(*)::int AS EventCount
            FROM content_timeline_events e
            WHERE e.product_id IN (SELECT product_tree_ids(@ProductId))
              AND e.feature_id IS NULL
              AND btrim(COALESCE(e.feature, '')) <> ''
            GROUP BY catalog_key(e.feature)
            ORDER BY COUNT(*) DESC, MIN(regexp_replace(btrim(e.feature), '\s+', ' ', 'g'))
            """;

        var unmatched = await _connection.QueryAsync<UnmatchedFeatureName>(
            new CommandDefinition(UnmatchedSql, new { ProductId = productId }, cancellationToken: ct));

        return new ProductFeatureOverview { Features = features, Unmatched = unmatched.ToList() };
    }

    /// <inheritdoc />
    public Task<ProductOperationResult> CreateAsync(long productId, string name, CancellationToken ct = default) =>
        InTransactionAsync(async transaction =>
        {
            if (ProductRepository.Normalize(name) is not { } normalized)
            {
                return InvalidName();
            }

            if (!await ProductExistsAsync(productId, transaction, ct))
            {
                return ProductOperationResult.NotFound("Product not found.");
            }

            if (await FindOwnerAsync(productId, normalized, transaction, ct) is { } owner)
            {
                return ProductOperationResult.Conflict($"'{normalized}' is already used by '{owner}'.");
            }

            var id = await _connection.ExecuteScalarAsync<long>(new CommandDefinition(
                "INSERT INTO product_features (product_id, name) VALUES (@ProductId, @Name) RETURNING id",
                new { ProductId = productId, Name = normalized },
                transaction,
                cancellationToken: ct));

            await CatalogRelink.RelinkAsync(_connection, transaction, ct);

            _logger.LogInformation("Created feature {FeatureId} of product {ProductId}: {FeatureName}", id, productId, normalized.Sanitize());
            return ProductOperationResult.Success(id);
        });

    /// <inheritdoc />
    public Task<ProductOperationResult> RenameAsync(long productId, long id, string name, CancellationToken ct = default) =>
        InTransactionAsync(async transaction =>
        {
            if (ProductRepository.Normalize(name) is not { } normalized)
            {
                return InvalidName();
            }

            var current = await GetNameAsync(productId, id, transaction, ct);
            if (current is null)
            {
                return ProductOperationResult.NotFound("Feature not found.");
            }

            var onlyCaseChanges = string.Equals(current, normalized, StringComparison.OrdinalIgnoreCase);
            if (!onlyCaseChanges)
            {
                var ownerId = await FindOwnerIdAsync(productId, normalized, transaction, ct);
                if (ownerId is { } other && other != id)
                {
                    return ProductOperationResult.Conflict($"'{normalized}' is already used by another feature of this product.");
                }

                await _connection.ExecuteAsync(new CommandDefinition(
                    "DELETE FROM product_feature_aliases WHERE feature_id = @Id AND lower(alias) = lower(@Name)",
                    new { Id = id, Name = normalized },
                    transaction,
                    cancellationToken: ct));
            }

            await _connection.ExecuteAsync(new CommandDefinition(
                "UPDATE product_features SET name = @Name WHERE id = @Id",
                new { Id = id, Name = normalized },
                transaction,
                cancellationToken: ct));

            if (!onlyCaseChanges)
            {
                await _connection.ExecuteAsync(new CommandDefinition(
                    "INSERT INTO product_feature_aliases (feature_id, alias) VALUES (@Id, @Alias)",
                    new { Id = id, Alias = current },
                    transaction,
                    cancellationToken: ct));
            }

            await CatalogRelink.RelinkAsync(_connection, transaction, ct);

            _logger.LogInformation("Renamed feature {FeatureId} from {OldName} to {NewName}", id, current.Sanitize(), normalized.Sanitize());
            return ProductOperationResult.Success(id);
        });

    /// <inheritdoc />
    public Task<ProductOperationResult> DeleteAsync(long productId, long id, CancellationToken ct = default) =>
        InTransactionAsync(async transaction =>
        {
            // The events lose the feature through the foreign key (ON DELETE SET NULL); their feature text stays.
            var rows = await _connection.ExecuteAsync(new CommandDefinition(
                "DELETE FROM product_features WHERE id = @Id AND product_id = @ProductId",
                new { Id = id, ProductId = productId },
                transaction,
                cancellationToken: ct));
            if (rows == 0)
            {
                return ProductOperationResult.NotFound("Feature not found.");
            }

            _logger.LogInformation("Deleted feature {FeatureId}", id);
            return ProductOperationResult.Success();
        });

    /// <inheritdoc />
    public Task<ProductOperationResult> AddAliasAsync(long productId, long id, string aliasName, CancellationToken ct = default) =>
        InTransactionAsync(async transaction =>
        {
            if (ProductRepository.Normalize(aliasName) is not { } normalized)
            {
                return InvalidName();
            }

            if (await GetNameAsync(productId, id, transaction, ct) is null)
            {
                return ProductOperationResult.NotFound("Feature not found.");
            }

            if (await FindOwnerAsync(productId, normalized, transaction, ct) is { } owner)
            {
                return ProductOperationResult.Conflict($"'{normalized}' is already used by '{owner}'.");
            }

            await _connection.ExecuteAsync(new CommandDefinition(
                "INSERT INTO product_feature_aliases (feature_id, alias) VALUES (@Id, @Alias)",
                new { Id = id, Alias = normalized },
                transaction,
                cancellationToken: ct));

            await CatalogRelink.RelinkAsync(_connection, transaction, ct);

            _logger.LogInformation("Added alias {Alias} to feature {FeatureId}", normalized.Sanitize(), id);
            return ProductOperationResult.Success(id);
        });

    /// <inheritdoc />
    public Task<ProductOperationResult> RemoveAliasAsync(long productId, long id, long aliasId, CancellationToken ct = default) =>
        InTransactionAsync(async transaction =>
        {
            var rows = await _connection.ExecuteAsync(new CommandDefinition(
                """
                DELETE FROM product_feature_aliases a
                USING product_features f
                WHERE a.id = @AliasId AND a.feature_id = @Id AND f.id = a.feature_id AND f.product_id = @ProductId
                """,
                new { Id = id, AliasId = aliasId, ProductId = productId },
                transaction,
                cancellationToken: ct));
            if (rows == 0)
            {
                return ProductOperationResult.NotFound("Alias not found.");
            }

            await CatalogRelink.RelinkAsync(_connection, transaction, ct);

            _logger.LogInformation("Removed alias {AliasId} from feature {FeatureId}", aliasId, id);
            return ProductOperationResult.Success(id);
        });

    private Task<bool> ProductExistsAsync(long productId, IDbTransaction transaction, CancellationToken ct) =>
        _connection.ExecuteScalarAsync<bool>(new CommandDefinition(
            "SELECT EXISTS (SELECT 1 FROM products WHERE id = @Id)", new { Id = productId }, transaction, cancellationToken: ct));

    private Task<string?> GetNameAsync(long productId, long id, IDbTransaction transaction, CancellationToken ct) =>
        _connection.QuerySingleOrDefaultAsync<string?>(new CommandDefinition(
            "SELECT name FROM product_features WHERE id = @Id AND product_id = @ProductId FOR UPDATE",
            new { Id = id, ProductId = productId },
            transaction,
            cancellationToken: ct));

    /// <summary>The feature of the product that uses <paramref name="name"/> as its name or an alias, or null if it is free.</summary>
    private Task<string?> FindOwnerAsync(long productId, string name, IDbTransaction transaction, CancellationToken ct) =>
        _connection.QuerySingleOrDefaultAsync<string?>(new CommandDefinition(
            """
            SELECT f.name FROM product_features f WHERE f.product_id = @ProductId AND lower(f.name) = lower(@Name)
            UNION ALL
            SELECT f.name FROM product_feature_aliases a JOIN product_features f ON f.id = a.feature_id
            WHERE f.product_id = @ProductId AND lower(a.alias) = lower(@Name)
            LIMIT 1
            """,
            new { ProductId = productId, Name = name },
            transaction,
            cancellationToken: ct));

    private Task<long?> FindOwnerIdAsync(long productId, string name, IDbTransaction transaction, CancellationToken ct) =>
        _connection.QuerySingleOrDefaultAsync<long?>(new CommandDefinition(
            """
            SELECT f.id FROM product_features f WHERE f.product_id = @ProductId AND lower(f.name) = lower(@Name)
            UNION ALL
            SELECT a.feature_id FROM product_feature_aliases a JOIN product_features f ON f.id = a.feature_id
            WHERE f.product_id = @ProductId AND lower(a.alias) = lower(@Name)
            LIMIT 1
            """,
            new { ProductId = productId, Name = name },
            transaction,
            cancellationToken: ct));

    private async Task<ProductOperationResult> InTransactionAsync(Func<IDbTransaction, Task<ProductOperationResult>> operation)
    {
        if (_connection.State != ConnectionState.Open)
        {
            _connection.Open();
        }

        using var transaction = _connection.BeginTransaction();
        try
        {
            var result = await operation(transaction);
            if (result.Succeeded)
            {
                transaction.Commit();
            }
            else
            {
                transaction.Rollback();
            }

            return result;
        }
        catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            // A concurrent change took the name between the check and the write.
            transaction.Rollback();
            return ProductOperationResult.Conflict("The name is already used.");
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    private static ProductOperationResult InvalidName() =>
        ProductOperationResult.Invalid($"A name is required and can be at most {ProductRepository.MaxNameLength} characters.");

    private sealed class AliasRow
    {
        public long Id { get; init; }
        public long FeatureId { get; init; }
        public string Alias { get; init; } = string.Empty;
    }
}
