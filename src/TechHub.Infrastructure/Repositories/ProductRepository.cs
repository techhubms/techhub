using System.Data;
using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using TechHub.Core.Interfaces;
using TechHub.Core.Logging;
using TechHub.Core.Models.Admin;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// PostgreSQL implementation of <see cref="IProductRepository"/>: the curated product catalog and the links between
/// the product names the AI returned (articles and timeline events) and the catalog.
/// </summary>
public sealed class ProductRepository : IProductRepository
{
    /// <summary>Longest product name or alias that is accepted.</summary>
    public const int MaxNameLength = 200;

    private const string RelinkSql = CatalogRelink.Sql;

    private const string NameInUseSql = """
        SELECT p.name AS Name FROM products p WHERE lower(p.name) = lower(@Name)
        UNION ALL
        SELECT p.name FROM product_aliases a JOIN products p ON p.id = a.product_id WHERE lower(a.alias) = lower(@Name)
        LIMIT 1
        """;

    private readonly IDbConnection _connection;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(IDbConnection connection, ILogger<ProductRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(logger);

        _connection = connection;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ProductSummary>> GetProductsAsync(string? search = null, CancellationToken ct = default)
    {
        const string Sql = """
            WITH RECURSIVE tree (root_id, id) AS (
                SELECT id, id FROM products
                UNION
                SELECT t.root_id, p.id FROM products p JOIN tree t ON p.parent_product_id = t.id
            )
            SELECT p.id   AS Id,
                   p.name AS Name,
                   p.parent_product_id AS ParentId,
                   parent.name AS ParentName,
                   (SELECT COUNT(DISTINCT (l.collection_name, l.item_slug))
                      FROM tree t JOIN content_item_products l ON l.product_id = t.id
                     WHERE t.root_id = p.id)::int AS ItemCount,
                   (SELECT COUNT(*)
                      FROM tree t JOIN content_timeline_events e ON e.product_id = t.id
                     WHERE t.root_id = p.id)::int AS EventCount
            FROM products p
            LEFT JOIN products parent ON parent.id = p.parent_product_id
            WHERE @Pattern::text IS NULL
               OR p.name ILIKE @Pattern::text
               OR EXISTS (SELECT 1 FROM product_aliases a WHERE a.product_id = p.id AND a.alias ILIKE @Pattern::text)
            ORDER BY lower(p.name), p.name
            """;

        var pattern = string.IsNullOrWhiteSpace(search) ? null : $"%{EscapeLike(search.Trim())}%";
        var products = (await _connection.QueryAsync<ProductSummary>(
            new CommandDefinition(Sql, new { Pattern = pattern }, cancellationToken: ct))).ToList();

        var aliases = await GetAliasesAsync(products.Select(p => p.Id).ToArray(), ct);
        foreach (var product in products)
        {
            product.Aliases = aliases.TryGetValue(product.Id, out var list) ? list : [];
        }

        return products;
    }

    /// <inheritdoc />
    public async Task<ProductDetail?> GetDetailAsync(long id, CancellationToken ct = default)
    {
        var product = await _connection.QuerySingleOrDefaultAsync<ProductDetail>(
            new CommandDefinition("SELECT id AS Id, name AS Name FROM products WHERE id = @Id", new { Id = id }, cancellationToken: ct));
        if (product is null)
        {
            return null;
        }

        var aliases = await GetAliasesAsync([id], ct);
        product.Aliases = aliases.TryGetValue(id, out var list) ? list : [];

        product.Parent = await _connection.QuerySingleOrDefaultAsync<ProductReference>(new CommandDefinition(
            "SELECT p.id AS Id, p.name AS Name FROM products c JOIN products p ON p.id = c.parent_product_id WHERE c.id = @Id",
            new { Id = id },
            cancellationToken: ct));

        product.Children = (await _connection.QueryAsync<ProductReference>(new CommandDefinition(
            "SELECT id AS Id, name AS Name FROM products WHERE parent_product_id = @Id ORDER BY lower(name), name",
            new { Id = id },
            cancellationToken: ct))).ToList();

        const string ArticlesSql = """
            SELECT l.collection_name AS CollectionName,
                   l.item_slug       AS Slug,
                   ci.title          AS Title,
                   ci.date_epoch     AS DateEpoch,
                   l.product_name    AS ProductName,
                   p.name            AS CatalogName,
                   q.name            AS QualifierName
            FROM content_item_products l
            JOIN products p ON p.id = l.product_id
            LEFT JOIN product_qualifiers q ON q.id = l.qualifier_id
            JOIN content_items ci ON ci.collection_name = l.collection_name AND ci.slug = l.item_slug
            WHERE l.product_id IN (SELECT product_tree_ids(@Id))
            ORDER BY ci.date_epoch DESC, ci.slug, l.position
            """;

        product.Articles = (await _connection.QueryAsync<ProductArticleLink>(
            new CommandDefinition(ArticlesSql, new { Id = id }, cancellationToken: ct))).ToList();

        const string EventsSql = """
            SELECT e.id             AS Id,
                   e.kind           AS Kind,
                   e.feature        AS Feature,
                   e.description    AS Description,
                   e.event_date     AS EventDate,
                   e.date_precision AS DatePrecision,
                   e.collection_name AS CollectionName,
                   e.item_slug      AS ItemSlug,
                   ci.title         AS ItemTitle,
                   e.product        AS ProductName,
                   p.name           AS CatalogName
            FROM content_timeline_events e
            JOIN products p ON p.id = e.product_id
            JOIN content_items ci ON ci.collection_name = e.collection_name AND ci.slug = e.item_slug
            WHERE e.product_id IN (SELECT product_tree_ids(@Id))
            ORDER BY e.event_date, e.id
            """;

        var events = (await _connection.QueryAsync<EventRow>(new CommandDefinition(EventsSql, new { Id = id }, cancellationToken: ct))).ToList();

        const string EventQualifiersSql = """
            SELECT eq.event_id AS EventId, q.id AS Id, q.kind AS Kind, q.name AS Name
            FROM content_timeline_event_qualifiers eq
            JOIN product_qualifiers q ON q.id = eq.qualifier_id
            WHERE eq.event_id = ANY(@EventIds)
            ORDER BY q.kind, lower(q.name)
            """;

        var qualifierRows = await _connection.QueryAsync<EventQualifierRow>(new CommandDefinition(
            EventQualifiersSql, new { EventIds = events.Select(e => e.Id).ToArray() }, cancellationToken: ct));
        var qualifiers = qualifierRows
            .GroupBy(r => r.EventId)
            .ToDictionary(
                g => g.Key,
                g => (IReadOnlyList<QualifierReference>)g.Select(r => new QualifierReference { Id = r.Id, Kind = r.Kind, Name = r.Name }).ToList());

        product.Events = events.Select(e => new ProductEventLink
        {
            Id = e.Id,
            Kind = e.Kind,
            Feature = e.Feature,
            Description = e.Description,
            EventDate = DateOnly.FromDateTime(e.EventDate),
            DatePrecision = e.DatePrecision,
            CollectionName = e.CollectionName,
            ItemSlug = e.ItemSlug,
            ItemTitle = e.ItemTitle,
            ProductName = e.ProductName,
            CatalogName = e.CatalogName,
            Qualifiers = qualifiers.TryGetValue(e.Id, out var eventQualifiers) ? eventQualifiers : []
        }).ToList();

        return product;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<UnmatchedProductName>> GetUnmatchedNamesAsync(CancellationToken ct = default)
    {
        const string Sql = """
            WITH names AS (
                SELECT lower(l.product_name) AS key, l.product_name AS name,
                       l.collection_name || '/' || l.item_slug AS item, NULL::bigint AS event_id
                FROM content_item_products l
                WHERE l.product_id IS NULL
                UNION ALL
                SELECT lower(regexp_replace(btrim(e.product), '\s+', ' ', 'g')),
                       regexp_replace(btrim(e.product), '\s+', ' ', 'g'),
                       NULL, e.id
                FROM content_timeline_events e
                WHERE e.product_id IS NULL
            )
            SELECT MIN(n.name)                      AS Name,
                   COUNT(DISTINCT n.item)::int      AS ItemCount,
                   COUNT(n.event_id)::int           AS EventCount
            FROM names n
            WHERE NOT EXISTS (SELECT 1 FROM ignored_product_names i WHERE lower(i.name) = n.key)
            GROUP BY n.key
            ORDER BY COUNT(DISTINCT n.item) + COUNT(n.event_id) DESC, MIN(n.name)
            """;

        var rows = await _connection.QueryAsync<UnmatchedProductName>(new CommandDefinition(Sql, cancellationToken: ct));
        return rows.ToList();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<string>> GetIgnoredNamesAsync(CancellationToken ct = default)
    {
        var rows = await _connection.QueryAsync<string>(new CommandDefinition(
            "SELECT name FROM ignored_product_names ORDER BY lower(name)", cancellationToken: ct));
        return rows.ToList();
    }

    /// <inheritdoc />
    public Task<ProductOperationResult> CreateAsync(string name, CancellationToken ct = default) =>
        InTransactionAsync(async transaction =>
        {
            if (Normalize(name) is not { } normalized)
            {
                return InvalidName();
            }

            if (await FindOwnerAsync(normalized, transaction, ct) is { } owner)
            {
                return ProductOperationResult.Conflict($"'{normalized}' is already used by '{owner}'.");
            }

            var id = await _connection.ExecuteScalarAsync<long>(new CommandDefinition(
                "INSERT INTO products (name) VALUES (@Name) RETURNING id",
                new { Name = normalized },
                transaction,
                cancellationToken: ct));

            await UnignoreAsync(normalized, transaction, ct);
            await RelinkAsync(transaction, ct);

            _logger.LogInformation("Created product {ProductId}: {ProductName}", id, normalized.Sanitize());
            return ProductOperationResult.Success(id);
        });

    /// <inheritdoc />
    public Task<ProductOperationResult> RenameAsync(long id, string name, CancellationToken ct = default) =>
        InTransactionAsync(async transaction =>
        {
            if (Normalize(name) is not { } normalized)
            {
                return InvalidName();
            }

            var current = await GetNameAsync(id, transaction, ct);
            if (current is null)
            {
                return ProductOperationResult.NotFound("Product not found.");
            }

            var onlyCaseChanges = string.Equals(current, normalized, StringComparison.OrdinalIgnoreCase);
            if (!onlyCaseChanges)
            {
                var ownerId = await FindOwnerIdAsync(normalized, transaction, ct);
                if (ownerId is { } other && other != id)
                {
                    return ProductOperationResult.Conflict($"'{normalized}' is already used by another product.");
                }

                // The new name was an alias of this product: it is the name now.
                await _connection.ExecuteAsync(new CommandDefinition(
                    "DELETE FROM product_aliases WHERE product_id = @Id AND lower(alias) = lower(@Name)",
                    new { Id = id, Name = normalized },
                    transaction,
                    cancellationToken: ct));
            }

            await _connection.ExecuteAsync(new CommandDefinition(
                "UPDATE products SET name = @Name, updated_at = NOW() WHERE id = @Id",
                new { Id = id, Name = normalized },
                transaction,
                cancellationToken: ct));

            if (!onlyCaseChanges)
            {
                await _connection.ExecuteAsync(new CommandDefinition(
                    "INSERT INTO product_aliases (product_id, alias) VALUES (@Id, @Alias)",
                    new { Id = id, Alias = current },
                    transaction,
                    cancellationToken: ct));
            }

            await UnignoreAsync(normalized, transaction, ct);
            await RelinkAsync(transaction, ct);

            _logger.LogInformation("Renamed product {ProductId} from {OldName} to {NewName}", id, current.Sanitize(), normalized.Sanitize());
            return ProductOperationResult.Success(id);
        });

    /// <inheritdoc />
    public Task<ProductOperationResult> AddAliasAsync(long id, string aliasName, CancellationToken ct = default) =>
        AddAliasAsync(id, aliasName, null, ct);

    /// <inheritdoc />
    public Task<ProductOperationResult> AddAliasAsync(long id, string aliasName, long? qualifierId, CancellationToken ct = default) =>
        InTransactionAsync(async transaction =>
        {
            if (Normalize(aliasName) is not { } normalized)
            {
                return InvalidName();
            }

            if (await GetNameAsync(id, transaction, ct) is null)
            {
                return ProductOperationResult.NotFound("Product not found.");
            }

            if (await FindOwnerAsync(normalized, transaction, ct) is { } owner)
            {
                return ProductOperationResult.Conflict($"'{normalized}' is already used by '{owner}'.");
            }

            if (qualifierId is { } qualifier && !await QualifierExistsAsync(qualifier, transaction, ct))
            {
                return ProductOperationResult.NotFound("Plan or surface not found.");
            }

            await _connection.ExecuteAsync(new CommandDefinition(
                "INSERT INTO product_aliases (product_id, alias, qualifier_id) VALUES (@Id, @Alias, @QualifierId)",
                new { Id = id, Alias = normalized, QualifierId = qualifierId },
                transaction,
                cancellationToken: ct));

            await UnignoreAsync(normalized, transaction, ct);
            await RelinkAsync(transaction, ct);

            _logger.LogInformation("Added alias {Alias} to product {ProductId}", normalized.Sanitize(), id);
            return ProductOperationResult.Success(id);
        });

    /// <inheritdoc />
    public Task<ProductOperationResult> SetParentAsync(long id, long? parentId, CancellationToken ct = default) =>
        InTransactionAsync(async transaction =>
        {
            if (await GetNameAsync(id, transaction, ct) is null)
            {
                return ProductOperationResult.NotFound("Product not found.");
            }

            if (parentId is { } parent)
            {
                if (parent == id)
                {
                    return ProductOperationResult.Invalid("A product cannot be its own parent.");
                }

                if (await GetNameAsync(parent, transaction, ct) is null)
                {
                    return ProductOperationResult.NotFound("Parent product not found.");
                }

                var isDescendant = await _connection.ExecuteScalarAsync<bool>(new CommandDefinition(
                    "SELECT EXISTS (SELECT 1 FROM product_tree_ids(@Id) AS t (id) WHERE t.id = @ParentId)",
                    new { Id = id, ParentId = parent },
                    transaction,
                    cancellationToken: ct));
                if (isDescendant)
                {
                    return ProductOperationResult.Invalid("A product cannot be moved under one of its own child products.");
                }
            }

            await _connection.ExecuteAsync(new CommandDefinition(
                "UPDATE products SET parent_product_id = @ParentId, updated_at = NOW() WHERE id = @Id",
                new { Id = id, ParentId = parentId },
                transaction,
                cancellationToken: ct));

            _logger.LogInformation("Set parent of product {ProductId} to {ParentId}", id, parentId);
            return ProductOperationResult.Success(id);
        });

    /// <inheritdoc />
    public Task<ProductOperationResult> SetAliasQualifierAsync(long id, long aliasId, long? qualifierId, CancellationToken ct = default) =>
        InTransactionAsync(async transaction =>
        {
            if (qualifierId is { } qualifier && !await QualifierExistsAsync(qualifier, transaction, ct))
            {
                return ProductOperationResult.NotFound("Plan or surface not found.");
            }

            var rows = await _connection.ExecuteAsync(new CommandDefinition(
                "UPDATE product_aliases SET qualifier_id = @QualifierId WHERE id = @AliasId AND product_id = @Id",
                new { Id = id, AliasId = aliasId, QualifierId = qualifierId },
                transaction,
                cancellationToken: ct));
            if (rows == 0)
            {
                return ProductOperationResult.NotFound("Alias not found.");
            }

            await RelinkAsync(transaction, ct);

            _logger.LogInformation("Set qualifier of alias {AliasId} of product {ProductId} to {QualifierId}", aliasId, id, qualifierId);
            return ProductOperationResult.Success(id);
        });

    /// <inheritdoc />
    public Task<ProductOperationResult> RemoveAliasAsync(long id, long aliasId, CancellationToken ct = default) =>
        InTransactionAsync(async transaction =>
        {
            var rows = await _connection.ExecuteAsync(new CommandDefinition(
                "DELETE FROM product_aliases WHERE id = @AliasId AND product_id = @Id",
                new { Id = id, AliasId = aliasId },
                transaction,
                cancellationToken: ct));
            if (rows == 0)
            {
                return ProductOperationResult.NotFound("Alias not found.");
            }

            await RelinkAsync(transaction, ct);

            _logger.LogInformation("Removed alias {AliasId} from product {ProductId}", aliasId, id);
            return ProductOperationResult.Success(id);
        });

    /// <inheritdoc />
    public Task<ProductOperationResult> MergeAsync(long sourceId, long targetId, CancellationToken ct = default) =>
        InTransactionAsync(async transaction =>
        {
            if (sourceId == targetId)
            {
                return ProductOperationResult.Invalid("A product cannot be merged into itself.");
            }

            var sourceName = await GetNameAsync(sourceId, transaction, ct);
            var targetName = await GetNameAsync(targetId, transaction, ct);
            if (sourceName is null || targetName is null)
            {
                return ProductOperationResult.NotFound("Product not found.");
            }

            await _connection.ExecuteAsync(new CommandDefinition(
                "UPDATE product_aliases SET product_id = @TargetId WHERE product_id = @SourceId",
                new { SourceId = sourceId, TargetId = targetId },
                transaction,
                cancellationToken: ct));

            // The children of the source move to the target. A child that is the target or one of its ancestors (the
            // target is a descendant of the source) takes the place of the source in the hierarchy instead.
            await _connection.ExecuteAsync(new CommandDefinition(
                """
                WITH RECURSIVE up (id) AS (
                    SELECT @TargetId
                    UNION
                    SELECT p.parent_product_id FROM products p JOIN up ON p.id = up.id WHERE p.parent_product_id IS NOT NULL
                )
                UPDATE products c
                SET parent_product_id = CASE
                    WHEN c.id IN (SELECT id FROM up) THEN (SELECT s.parent_product_id FROM products s WHERE s.id = @SourceId)
                    ELSE @TargetId END
                WHERE c.parent_product_id = @SourceId
                """,
                new { SourceId = sourceId, TargetId = targetId },
                transaction,
                cancellationToken: ct));

            await _connection.ExecuteAsync(new CommandDefinition(
                "DELETE FROM products WHERE id = @SourceId",
                new { SourceId = sourceId },
                transaction,
                cancellationToken: ct));

            await _connection.ExecuteAsync(new CommandDefinition(
                "INSERT INTO product_aliases (product_id, alias) VALUES (@TargetId, @Alias)",
                new { TargetId = targetId, Alias = sourceName },
                transaction,
                cancellationToken: ct));

            await RelinkAsync(transaction, ct);

            _logger.LogInformation("Merged product {SourceId} ({SourceName}) into {TargetId} ({TargetName})",
                sourceId, sourceName.Sanitize(), targetId, targetName.Sanitize());
            return ProductOperationResult.Success(targetId);
        });

    /// <inheritdoc />
    public Task<ProductOperationResult> DeleteAsync(long id, CancellationToken ct = default) =>
        InTransactionAsync(async transaction =>
        {
            // The links of the product become unmatched through the foreign keys (ON DELETE SET NULL).
            var rows = await _connection.ExecuteAsync(new CommandDefinition(
                "DELETE FROM products WHERE id = @Id",
                new { Id = id },
                transaction,
                cancellationToken: ct));
            if (rows == 0)
            {
                return ProductOperationResult.NotFound("Product not found.");
            }

            _logger.LogInformation("Deleted product {ProductId}", id);
            return ProductOperationResult.Success();
        });

    /// <inheritdoc />
    public Task<ProductOperationResult> IgnoreNameAsync(string name, CancellationToken ct = default) =>
        InTransactionAsync(async transaction =>
        {
            if (Normalize(name) is not { } normalized)
            {
                return InvalidName();
            }

            if (await FindOwnerAsync(normalized, transaction, ct) is { } owner)
            {
                return ProductOperationResult.Conflict($"'{normalized}' is already used by '{owner}'.");
            }

            await _connection.ExecuteAsync(new CommandDefinition(
                "INSERT INTO ignored_product_names (name) VALUES (@Name) ON CONFLICT (lower(name)) DO NOTHING",
                new { Name = normalized },
                transaction,
                cancellationToken: ct));

            _logger.LogInformation("Ignoring product name {ProductName}", normalized.Sanitize());
            return ProductOperationResult.Success();
        });

    /// <inheritdoc />
    public Task<ProductOperationResult> UnignoreNameAsync(string name, CancellationToken ct = default) =>
        InTransactionAsync(async transaction =>
        {
            if (Normalize(name) is not { } normalized)
            {
                return InvalidName();
            }

            return await UnignoreAsync(normalized, transaction, ct)
                ? ProductOperationResult.Success()
                : ProductOperationResult.NotFound("Name is not ignored.");
        });

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

    private async Task<Dictionary<long, IReadOnlyList<ProductAlias>>> GetAliasesAsync(long[] productIds, CancellationToken ct)
    {
        const string Sql = """
            SELECT a.id AS Id, a.product_id AS ProductId, a.alias AS Alias,
                   a.qualifier_id AS QualifierId, q.name AS QualifierName, q.kind AS QualifierKind
            FROM product_aliases a
            LEFT JOIN product_qualifiers q ON q.id = a.qualifier_id
            WHERE a.product_id = ANY(@ProductIds)
            ORDER BY lower(a.alias), a.alias
            """;

        var rows = await _connection.QueryAsync<AliasRow>(new CommandDefinition(Sql, new { ProductIds = productIds }, cancellationToken: ct));
        return rows
            .GroupBy(r => r.ProductId)
            .ToDictionary(
                g => g.Key,
                g => (IReadOnlyList<ProductAlias>)g.Select(r => new ProductAlias
                {
                    Id = r.Id,
                    Alias = r.Alias,
                    QualifierId = r.QualifierId,
                    QualifierName = r.QualifierName,
                    QualifierKind = r.QualifierKind
                }).ToList());
    }

    private Task<bool> QualifierExistsAsync(long id, IDbTransaction transaction, CancellationToken ct) =>
        _connection.ExecuteScalarAsync<bool>(new CommandDefinition(
            "SELECT EXISTS (SELECT 1 FROM product_qualifiers WHERE id = @Id)", new { Id = id }, transaction, cancellationToken: ct));

    private Task<string?> GetNameAsync(long id, IDbTransaction transaction, CancellationToken ct) =>
        _connection.QuerySingleOrDefaultAsync<string?>(new CommandDefinition(
            "SELECT name FROM products WHERE id = @Id FOR UPDATE", new { Id = id }, transaction, cancellationToken: ct));

    /// <summary>The name of the product that uses <paramref name="name"/> as its name or an alias, or null if it is free.</summary>
    private Task<string?> FindOwnerAsync(string name, IDbTransaction transaction, CancellationToken ct) =>
        _connection.QuerySingleOrDefaultAsync<string?>(new CommandDefinition(NameInUseSql, new { Name = name }, transaction, cancellationToken: ct));

    private Task<long?> FindOwnerIdAsync(string name, IDbTransaction transaction, CancellationToken ct) =>
        _connection.QuerySingleAsync<long?>(new CommandDefinition("SELECT find_product_id(@Name)", new { Name = name }, transaction, cancellationToken: ct));

    private async Task<bool> UnignoreAsync(string name, IDbTransaction transaction, CancellationToken ct)
    {
        var rows = await _connection.ExecuteAsync(new CommandDefinition(
            "DELETE FROM ignored_product_names WHERE lower(name) = lower(@Name)",
            new { Name = name },
            transaction,
            cancellationToken: ct));
        return rows > 0;
    }

    private async Task RelinkAsync(IDbTransaction transaction, CancellationToken ct) =>
        await _connection.ExecuteAsync(new CommandDefinition(RelinkSql, transaction: transaction, cancellationToken: ct));

    private static ProductOperationResult InvalidName() =>
        ProductOperationResult.Invalid($"A name is required and can be at most {MaxNameLength} characters.");

    /// <summary>Trims a name and collapses inner whitespace. Returns null for an empty or too long name.</summary>
    internal static string? Normalize(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        var builder = new StringBuilder(name.Length);
        var previousWasSpace = false;
        foreach (var c in name.Trim())
        {
            if (char.IsWhiteSpace(c))
            {
                if (!previousWasSpace)
                {
                    builder.Append(' ');
                }

                previousWasSpace = true;
            }
            else
            {
                builder.Append(c);
                previousWasSpace = false;
            }
        }

        return builder.Length > MaxNameLength ? null : builder.ToString();
    }

    private static string EscapeLike(string value) =>
        value.Replace("\\", "\\\\", StringComparison.Ordinal).Replace("%", "\\%", StringComparison.Ordinal).Replace("_", "\\_", StringComparison.Ordinal);

    private sealed class AliasRow
    {
        public long Id { get; init; }
        public long ProductId { get; init; }
        public string Alias { get; init; } = string.Empty;
        public long? QualifierId { get; init; }
        public string? QualifierName { get; init; }
        public string? QualifierKind { get; init; }
    }

    private sealed class EventQualifierRow
    {
        public long EventId { get; init; }
        public long Id { get; init; }
        public string Kind { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
    }

    private sealed class EventRow
    {
        public long Id { get; init; }
        public string Kind { get; init; } = string.Empty;
        public string? Feature { get; init; }
        public string Description { get; init; } = string.Empty;
        public DateTime EventDate { get; init; }
        public string DatePrecision { get; init; } = string.Empty;
        public string CollectionName { get; init; } = string.Empty;
        public string ItemSlug { get; init; } = string.Empty;
        public string ItemTitle { get; init; } = string.Empty;
        public string ProductName { get; init; } = string.Empty;
        public string CatalogName { get; init; } = string.Empty;
    }
}
