using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using TechHub.Core.Interfaces;
using TechHub.Core.Logging;
using TechHub.Core.Models.Admin;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// PostgreSQL implementation of <see cref="IProductQualifierRepository"/>: the vocabulary of plans and surfaces that
/// timeline events and product aliases are linked to.
/// </summary>
public sealed class ProductQualifierRepository : IProductQualifierRepository
{
    private readonly IDbConnection _connection;
    private readonly ILogger<ProductQualifierRepository> _logger;

    public ProductQualifierRepository(IDbConnection connection, ILogger<ProductQualifierRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(logger);

        _connection = connection;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ProductQualifier>> GetQualifiersAsync(CancellationToken ct = default)
    {
        const string Sql = """
            SELECT q.id   AS Id,
                   q.kind AS Kind,
                   q.name AS Name,
                   (SELECT COUNT(*) FROM content_timeline_event_qualifiers eq WHERE eq.qualifier_id = q.id)::int AS EventCount,
                   (SELECT COUNT(DISTINCT (l.collection_name, l.item_slug)) FROM content_item_products l WHERE l.qualifier_id = q.id)::int AS ItemCount
            FROM product_qualifiers q
            ORDER BY q.kind, lower(q.name), q.name
            """;

        var qualifiers = (await _connection.QueryAsync<ProductQualifier>(new CommandDefinition(Sql, cancellationToken: ct))).ToList();

        var aliasRows = await _connection.QueryAsync<AliasRow>(new CommandDefinition(
            "SELECT id AS Id, qualifier_id AS QualifierId, alias AS Alias FROM product_qualifier_aliases ORDER BY lower(alias), alias",
            cancellationToken: ct));
        var aliases = aliasRows
            .GroupBy(a => a.QualifierId)
            .ToDictionary(g => g.Key, g => (IReadOnlyList<QualifierAlias>)g.Select(a => new QualifierAlias { Id = a.Id, Alias = a.Alias }).ToList());

        foreach (var qualifier in qualifiers)
        {
            qualifier.Aliases = aliases.TryGetValue(qualifier.Id, out var list) ? list : [];
        }

        return qualifiers;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<UnmatchedQualifierName>> GetUnmatchedNamesAsync(CancellationToken ct = default)
    {
        const string Sql = """
            WITH names AS (
                SELECT 'plan' AS kind, catalog_key(x) AS key, regexp_replace(btrim(x), '\s+', ' ', 'g') AS name, e.id AS event_id
                FROM content_timeline_events e CROSS JOIN LATERAL unnest(e.plans) AS x
                UNION ALL
                SELECT 'surface', catalog_key(x), regexp_replace(btrim(x), '\s+', ' ', 'g'), e.id
                FROM content_timeline_events e CROSS JOIN LATERAL unnest(e.surfaces) AS x
            )
            SELECT n.kind AS Kind,
                   MIN(n.name) AS Name,
                   COUNT(DISTINCT n.event_id)::int AS EventCount
            FROM names n
            WHERE n.key <> '' AND find_qualifier_id(n.kind, n.name) IS NULL
            GROUP BY n.kind, n.key
            ORDER BY COUNT(DISTINCT n.event_id) DESC, MIN(n.name)
            """;

        var rows = await _connection.QueryAsync<UnmatchedQualifierName>(new CommandDefinition(Sql, cancellationToken: ct));
        return rows.ToList();
    }

    /// <inheritdoc />
    public Task<ProductOperationResult> CreateAsync(string kind, string name, CancellationToken ct = default) =>
        InTransactionAsync(async transaction =>
        {
            if (!QualifierKinds.All.Contains(kind))
            {
                return ProductOperationResult.Invalid("The kind must be 'plan' or 'surface'.");
            }

            if (ProductRepository.Normalize(name) is not { } normalized)
            {
                return InvalidName();
            }

            if (await FindOwnerAsync(kind, normalized, transaction, ct) is { } owner)
            {
                return ProductOperationResult.Conflict($"'{normalized}' is already used by '{owner}'.");
            }

            var id = await _connection.ExecuteScalarAsync<long>(new CommandDefinition(
                "INSERT INTO product_qualifiers (kind, name) VALUES (@Kind, @Name) RETURNING id",
                new { Kind = kind, Name = normalized },
                transaction,
                cancellationToken: ct));

            await CatalogRelink.RelinkAsync(_connection, transaction, ct);

            _logger.LogInformation("Created {Kind} {QualifierId}: {QualifierName}", kind, id, normalized.Sanitize());
            return ProductOperationResult.Success(id);
        });

    /// <inheritdoc />
    public Task<ProductOperationResult> RenameAsync(long id, string name, CancellationToken ct = default) =>
        InTransactionAsync(async transaction =>
        {
            if (ProductRepository.Normalize(name) is not { } normalized)
            {
                return InvalidName();
            }

            var current = await GetQualifierAsync(id, transaction, ct);
            if (current is null)
            {
                return ProductOperationResult.NotFound("Plan or surface not found.");
            }

            var onlyCaseChanges = string.Equals(current.Name, normalized, StringComparison.OrdinalIgnoreCase);
            if (!onlyCaseChanges)
            {
                var ownerId = await _connection.QuerySingleAsync<long?>(new CommandDefinition(
                    "SELECT find_qualifier_id(@Kind, @Name)",
                    new { current.Kind, Name = normalized },
                    transaction,
                    cancellationToken: ct));
                if (ownerId is { } other && other != id)
                {
                    return ProductOperationResult.Conflict($"'{normalized}' is already used by another {current.Kind}.");
                }

                await _connection.ExecuteAsync(new CommandDefinition(
                    "DELETE FROM product_qualifier_aliases WHERE qualifier_id = @Id AND lower(alias) = lower(@Name)",
                    new { Id = id, Name = normalized },
                    transaction,
                    cancellationToken: ct));
            }

            await _connection.ExecuteAsync(new CommandDefinition(
                "UPDATE product_qualifiers SET name = @Name WHERE id = @Id",
                new { Id = id, Name = normalized },
                transaction,
                cancellationToken: ct));

            if (!onlyCaseChanges)
            {
                await _connection.ExecuteAsync(new CommandDefinition(
                    "INSERT INTO product_qualifier_aliases (qualifier_id, alias) VALUES (@Id, @Alias)",
                    new { Id = id, Alias = current.Name },
                    transaction,
                    cancellationToken: ct));
            }

            await CatalogRelink.RelinkAsync(_connection, transaction, ct);

            _logger.LogInformation("Renamed {Kind} {QualifierId} from {OldName} to {NewName}", current.Kind, id, current.Name.Sanitize(), normalized.Sanitize());
            return ProductOperationResult.Success(id);
        });

    /// <inheritdoc />
    public Task<ProductOperationResult> DeleteAsync(long id, CancellationToken ct = default) =>
        InTransactionAsync(async transaction =>
        {
            // Event links and aliases go through the foreign keys: aliases and event links cascade, product aliases and article links lose the qualifier.
            var rows = await _connection.ExecuteAsync(new CommandDefinition(
                "DELETE FROM product_qualifiers WHERE id = @Id",
                new { Id = id },
                transaction,
                cancellationToken: ct));
            if (rows == 0)
            {
                return ProductOperationResult.NotFound("Plan or surface not found.");
            }

            _logger.LogInformation("Deleted qualifier {QualifierId}", id);
            return ProductOperationResult.Success();
        });

    /// <inheritdoc />
    public Task<ProductOperationResult> AddAliasAsync(long id, string aliasName, CancellationToken ct = default) =>
        InTransactionAsync(async transaction =>
        {
            if (ProductRepository.Normalize(aliasName) is not { } normalized)
            {
                return InvalidName();
            }

            var qualifier = await GetQualifierAsync(id, transaction, ct);
            if (qualifier is null)
            {
                return ProductOperationResult.NotFound("Plan or surface not found.");
            }

            if (await FindOwnerAsync(qualifier.Kind, normalized, transaction, ct) is { } owner)
            {
                return ProductOperationResult.Conflict($"'{normalized}' is already used by '{owner}'.");
            }

            await _connection.ExecuteAsync(new CommandDefinition(
                "INSERT INTO product_qualifier_aliases (qualifier_id, alias) VALUES (@Id, @Alias)",
                new { Id = id, Alias = normalized },
                transaction,
                cancellationToken: ct));

            await CatalogRelink.RelinkAsync(_connection, transaction, ct);

            _logger.LogInformation("Added alias {Alias} to qualifier {QualifierId}", normalized.Sanitize(), id);
            return ProductOperationResult.Success(id);
        });

    /// <inheritdoc />
    public Task<ProductOperationResult> RemoveAliasAsync(long id, long aliasId, CancellationToken ct = default) =>
        InTransactionAsync(async transaction =>
        {
            var rows = await _connection.ExecuteAsync(new CommandDefinition(
                "DELETE FROM product_qualifier_aliases WHERE id = @AliasId AND qualifier_id = @Id",
                new { Id = id, AliasId = aliasId },
                transaction,
                cancellationToken: ct));
            if (rows == 0)
            {
                return ProductOperationResult.NotFound("Alias not found.");
            }

            await CatalogRelink.RelinkAsync(_connection, transaction, ct);

            _logger.LogInformation("Removed alias {AliasId} from qualifier {QualifierId}", aliasId, id);
            return ProductOperationResult.Success(id);
        });

    private Task<QualifierReference?> GetQualifierAsync(long id, IDbTransaction transaction, CancellationToken ct) =>
        _connection.QuerySingleOrDefaultAsync<QualifierReference?>(new CommandDefinition(
            "SELECT id AS Id, kind AS Kind, name AS Name FROM product_qualifiers WHERE id = @Id FOR UPDATE",
            new { Id = id },
            transaction,
            cancellationToken: ct));

    /// <summary>The qualifier that uses <paramref name="name"/> as its name or an alias, or null if it is free.</summary>
    private Task<string?> FindOwnerAsync(string kind, string name, IDbTransaction transaction, CancellationToken ct) =>
        _connection.QuerySingleOrDefaultAsync<string?>(new CommandDefinition(
            """
            SELECT q.name FROM product_qualifiers q WHERE q.kind = @Kind AND lower(q.name) = lower(@Name)
            UNION ALL
            SELECT q.name FROM product_qualifier_aliases a JOIN product_qualifiers q ON q.id = a.qualifier_id WHERE lower(a.alias) = lower(@Name)
            LIMIT 1
            """,
            new { Kind = kind, Name = name },
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
        public long QualifierId { get; init; }
        public string Alias { get; init; } = string.Empty;
    }
}
