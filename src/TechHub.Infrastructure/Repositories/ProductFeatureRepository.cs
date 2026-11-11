using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using TechHub.Core.Interfaces;
using TechHub.Core.Logging;
using TechHub.Core.Models.Admin;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// PostgreSQL implementation of <see cref="IProductFeatureRepository"/>: curated features and their links to both
/// dated events and articles whose detected feature mentions match the catalog.
/// </summary>
public sealed class ProductFeatureRepository : IProductFeatureRepository
{
    /// <summary>The longest description of a feature, in characters.</summary>
    public const int MaxDescriptionLength = 4000;

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
    public async Task<IReadOnlyList<ProductUnmatchedFeatures>> GetUnmatchedSummaryAsync(CancellationToken ct = default)
    {
        const string Sql = """
            WITH mentions AS (
                SELECT e.product_id, e.feature, e.collection_name, e.item_slug, TRUE AS is_event
                FROM content_timeline_events e
                WHERE e.feature_id IS NULL
                  AND e.product_id IS NOT NULL
                  AND btrim(COALESCE(e.feature, '')) <> ''
                UNION ALL
                SELECT resolved.out_product_id, n.value ->> 'feature', ci.collection_name, ci.slug, FALSE
                FROM content_items ci
                CROSS JOIN LATERAL jsonb_array_elements(
                    CASE WHEN jsonb_typeof(ci.ai_metadata -> 'product_features') = 'array'
                         THEN ci.ai_metadata -> 'product_features'
                         ELSE '[]'::jsonb END) AS n (value)
                CROSS JOIN LATERAL resolve_event_catalog(n.value ->> 'product', n.value ->> 'feature') AS resolved
                WHERE jsonb_typeof(n.value) = 'object'
                  AND btrim(COALESCE(n.value ->> 'product', '')) <> ''
                  AND btrim(COALESCE(n.value ->> 'feature', '')) <> ''
                  AND resolved.out_product_id IS NOT NULL
                  AND resolved.out_feature_id IS NULL
            )
            SELECT m.product_id AS ProductId,
                   p.name AS ProductName,
                   COUNT(DISTINCT catalog_key(m.feature))::int AS NameCount,
                   COUNT(DISTINCT (m.collection_name, m.item_slug)) FILTER (WHERE NOT m.is_event)::int AS ContentCount,
                   COUNT(*) FILTER (WHERE m.is_event)::int AS EventCount
            FROM mentions m
            JOIN products p ON p.id = m.product_id
            GROUP BY m.product_id, p.name
            ORDER BY COUNT(*) DESC, p.name
            """;

        var rows = await _connection.QueryAsync<ProductUnmatchedFeatures>(new CommandDefinition(Sql, cancellationToken: ct));
        return rows.ToList();
    }

    /// <inheritdoc />
    public async Task<ProductFeatureOverview> GetOverviewAsync(long productId, CancellationToken ct = default)
    {
        const string FeaturesSql = """
            SELECT f.id           AS Id,
                   f.product_id   AS ProductId,
                   f.name         AS Name,
                   f.description  AS Description,
                   f.release_date AS ReleaseDate,
                   (SELECT COUNT(*) FROM content_timeline_events e WHERE e.feature_id = f.id)::int AS EventCount
            FROM product_features f
            WHERE f.product_id = @ProductId
            ORDER BY lower(f.name), f.name
            """;

        var features = (await _connection.QueryAsync<FeatureRow>(
                new CommandDefinition(FeaturesSql, new { ProductId = productId }, cancellationToken: ct)))
            .Select(r => new ProductFeature
            {
                Id = r.Id,
                ProductId = r.ProductId,
                Name = r.Name,
                Description = r.Description ?? string.Empty,
                ReleaseDate = r.ReleaseDate is { } epoch ? ToDate(epoch) : null,
                EventCount = r.EventCount
            })
            .ToList();

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

        var contentRows = await _connection.QueryAsync<ContentRow>(new CommandDefinition(
            """
            WITH detected AS (
                SELECT DISTINCT e.feature_id, e.collection_name, e.item_slug
                FROM content_timeline_events e
                JOIN product_features f ON f.id = e.feature_id
                WHERE f.product_id = @ProductId
                UNION
                SELECT DISTINCT resolved.out_feature_id, ci.collection_name, ci.slug
                FROM content_items ci
                CROSS JOIN LATERAL jsonb_array_elements(
                    CASE WHEN jsonb_typeof(ci.ai_metadata -> 'product_features') = 'array'
                         THEN ci.ai_metadata -> 'product_features'
                         ELSE '[]'::jsonb END) AS n (value)
                CROSS JOIN LATERAL resolve_event_catalog(n.value ->> 'product', n.value ->> 'feature') AS resolved
                JOIN product_features f ON f.id = resolved.out_feature_id
                WHERE f.product_id = @ProductId
                  AND jsonb_typeof(n.value) = 'object'
                  AND btrim(COALESCE(n.value ->> 'product', '')) <> ''
                  AND btrim(COALESCE(n.value ->> 'feature', '')) <> ''
            ),
            links AS (
                SELECT l.feature_id AS FeatureId, l.collection_name AS CollectionName, l.item_slug AS ItemSlug,
                       ci.title AS Title, l.is_thumbnail AS IsThumbnail, l.sort_order AS SortOrder, FALSE AS IsDetected
                FROM product_feature_content l
                JOIN product_features f ON f.id = l.feature_id
                JOIN content_items ci ON ci.collection_name = l.collection_name AND ci.slug = l.item_slug
                WHERE f.product_id = @ProductId
                UNION ALL
                SELECT d.feature_id, d.collection_name, d.item_slug,
                       ci.title, FALSE, 2147483647, TRUE
                FROM detected d
                JOIN content_items ci ON ci.collection_name = d.collection_name AND ci.slug = d.item_slug
                WHERE NOT EXISTS (
                    SELECT 1 FROM product_feature_content manual
                    WHERE manual.feature_id = d.feature_id
                      AND manual.collection_name = d.collection_name
                      AND manual.item_slug = d.item_slug
                )
            )
            SELECT FeatureId, CollectionName, ItemSlug, Title, IsThumbnail, SortOrder, IsDetected
            FROM links
            ORDER BY SortOrder, ItemSlug
            """,
            new { ProductId = productId },
            cancellationToken: ct));
        var content = contentRows
            .GroupBy(c => c.FeatureId)
            .ToDictionary(
                g => g.Key,
                g => (IReadOnlyList<ProductFeatureContent>)g.Select(c => new ProductFeatureContent
                {
                    CollectionName = c.CollectionName,
                    ItemSlug = c.ItemSlug,
                    Title = c.Title,
                    IsThumbnail = c.IsThumbnail,
                    SortOrder = c.SortOrder,
                    IsDetected = c.IsDetected
                }).ToList());

        foreach (var feature in features)
        {
            feature.Aliases = aliases.TryGetValue(feature.Id, out var list) ? list : [];
            feature.Content = content.TryGetValue(feature.Id, out var links) ? links : [];
        }

        const string UnmatchedSql = """
            WITH mentions AS (
                SELECT e.product_id, e.feature, e.collection_name, e.item_slug, TRUE AS is_event
                FROM content_timeline_events e
                WHERE e.product_id IN (SELECT product_tree_ids(@ProductId))
                  AND e.feature_id IS NULL
                  AND btrim(COALESCE(e.feature, '')) <> ''
                UNION ALL
                SELECT resolved.out_product_id, n.value ->> 'feature', ci.collection_name, ci.slug, FALSE
                FROM content_items ci
                CROSS JOIN LATERAL jsonb_array_elements(
                    CASE WHEN jsonb_typeof(ci.ai_metadata -> 'product_features') = 'array'
                         THEN ci.ai_metadata -> 'product_features'
                         ELSE '[]'::jsonb END) AS n (value)
                CROSS JOIN LATERAL resolve_event_catalog(n.value ->> 'product', n.value ->> 'feature') AS resolved
                WHERE jsonb_typeof(n.value) = 'object'
                  AND btrim(COALESCE(n.value ->> 'product', '')) <> ''
                  AND btrim(COALESCE(n.value ->> 'feature', '')) <> ''
                  AND resolved.out_product_id IN (SELECT product_tree_ids(@ProductId))
                  AND resolved.out_feature_id IS NULL
            )
            SELECT MIN(regexp_replace(btrim(m.feature), '\s+', ' ', 'g')) AS Name,
                   COUNT(DISTINCT (m.collection_name, m.item_slug)) FILTER (WHERE NOT m.is_event)::int AS ContentCount,
                   COUNT(*) FILTER (WHERE m.is_event)::int AS EventCount
            FROM mentions m
            GROUP BY catalog_key(m.feature)
            ORDER BY COUNT(*) DESC, MIN(regexp_replace(btrim(m.feature), '\s+', ' ', 'g'))
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
        }, ct);

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
        }, ct);

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
        }, ct);

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
        }, ct);

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
        }, ct);

    /// <inheritdoc />
    public Task<ProductOperationResult> UpdateDetailsAsync(long productId, long id, string? description, DateOnly? releaseDate, CancellationToken ct = default) =>
        InTransactionAsync(async transaction =>
        {
            var text = description?.Trim() ?? string.Empty;
            if (text.Length > MaxDescriptionLength)
            {
                return ProductOperationResult.Invalid($"The description can be at most {MaxDescriptionLength} characters.");
            }

            var rows = await _connection.ExecuteAsync(new CommandDefinition(
                """
                UPDATE product_features
                SET description = @Description, release_date = @ReleaseDate, updated_at = NOW()
                WHERE id = @Id AND product_id = @ProductId
                """,
                new { Id = id, ProductId = productId, Description = text, ReleaseDate = releaseDate is { } date ? ToEpoch(date) : (long?)null },
                transaction,
                cancellationToken: ct));
            if (rows == 0)
            {
                return ProductOperationResult.NotFound("Feature not found.");
            }

            _logger.LogInformation("Updated the details of feature {FeatureId}", id);
            return ProductOperationResult.Success(id);
        }, ct);

    /// <inheritdoc />
    public Task<ProductOperationResult> AddContentAsync(long productId, long id, string collectionName, string itemSlug, bool isThumbnail, CancellationToken ct = default) =>
        InTransactionAsync(async transaction =>
        {
            if (string.IsNullOrWhiteSpace(collectionName) || string.IsNullOrWhiteSpace(itemSlug))
            {
                return ProductOperationResult.Invalid("A collection and a slug are required.");
            }

            if (await GetNameAsync(productId, id, transaction, ct) is null)
            {
                return ProductOperationResult.NotFound("Feature not found.");
            }

            var itemExists = await _connection.ExecuteScalarAsync<bool>(new CommandDefinition(
                "SELECT EXISTS (SELECT 1 FROM content_items WHERE collection_name = @CollectionName AND slug = @ItemSlug)",
                new { CollectionName = collectionName, ItemSlug = itemSlug },
                transaction,
                cancellationToken: ct));
            if (!itemExists)
            {
                return ProductOperationResult.NotFound("Content item not found.");
            }

            if (isThumbnail)
            {
                await ClearThumbnailAsync(id, transaction, ct);
            }

            await _connection.ExecuteAsync(new CommandDefinition(
                """
                INSERT INTO product_feature_content (feature_id, collection_name, item_slug, is_thumbnail, sort_order)
                VALUES (@Id, @CollectionName, @ItemSlug, @IsThumbnail,
                        COALESCE((SELECT MAX(sort_order) + 1 FROM product_feature_content WHERE feature_id = @Id), 0))
                ON CONFLICT (feature_id, collection_name, item_slug) DO UPDATE SET is_thumbnail = EXCLUDED.is_thumbnail
                """,
                new { Id = id, CollectionName = collectionName, ItemSlug = itemSlug, IsThumbnail = isThumbnail },
                transaction,
                cancellationToken: ct));

            _logger.LogInformation("Linked {Collection}/{Slug} to feature {FeatureId}", collectionName.Sanitize(), itemSlug.Sanitize(), id);
            return ProductOperationResult.Success(id);
        }, ct);

    /// <inheritdoc />
    public Task<ProductOperationResult> RemoveContentAsync(long productId, long id, string collectionName, string itemSlug, CancellationToken ct = default) =>
        InTransactionAsync(async transaction =>
        {
            var rows = await _connection.ExecuteAsync(new CommandDefinition(
                """
                DELETE FROM product_feature_content l
                USING product_features f
                WHERE l.feature_id = f.id AND f.id = @Id AND f.product_id = @ProductId
                  AND l.collection_name = @CollectionName AND l.item_slug = @ItemSlug
                """,
                new { Id = id, ProductId = productId, CollectionName = collectionName, ItemSlug = itemSlug },
                transaction,
                cancellationToken: ct));
            if (rows == 0)
            {
                return ProductOperationResult.NotFound("Link not found.");
            }

            _logger.LogInformation("Unlinked {Collection}/{Slug} from feature {FeatureId}", collectionName.Sanitize(), itemSlug.Sanitize(), id);
            return ProductOperationResult.Success(id);
        }, ct);

    /// <inheritdoc />
    public Task<ProductOperationResult> SetThumbnailAsync(long productId, long id, string collectionName, string itemSlug, CancellationToken ct = default) =>
        InTransactionAsync(async transaction =>
        {
            if (await GetNameAsync(productId, id, transaction, ct) is null)
            {
                return ProductOperationResult.NotFound("Feature not found.");
            }

            await ClearThumbnailAsync(id, transaction, ct);

            var rows = await _connection.ExecuteAsync(new CommandDefinition(
                """
                UPDATE product_feature_content SET is_thumbnail = TRUE
                WHERE feature_id = @Id AND collection_name = @CollectionName AND item_slug = @ItemSlug
                """,
                new { Id = id, CollectionName = collectionName, ItemSlug = itemSlug },
                transaction,
                cancellationToken: ct));

            // Returning a failure rolls back the cleared thumbnail.
            return rows == 0 ? ProductOperationResult.NotFound("Link not found.") : ProductOperationResult.Success(id);
        }, ct);

    private Task<int> ClearThumbnailAsync(long id, IDbTransaction transaction, CancellationToken ct) =>
        _connection.ExecuteAsync(new CommandDefinition(
            "UPDATE product_feature_content SET is_thumbnail = FALSE WHERE feature_id = @Id AND is_thumbnail",
            new { Id = id },
            transaction,
            cancellationToken: ct));

    // The release date is stored as the epoch seconds of midnight UTC of the date, like the dates of the timeline events.
    private static DateOnly ToDate(long epochSeconds) => DateOnly.FromDateTime(DateTimeOffset.FromUnixTimeSeconds(epochSeconds).UtcDateTime);

    private static long ToEpoch(DateOnly date) => new DateTimeOffset(date.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero).ToUnixTimeSeconds();

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

    private async Task<ProductOperationResult> InTransactionAsync(Func<IDbTransaction, Task<ProductOperationResult>> operation, CancellationToken ct)
    {
        if (_connection.State != ConnectionState.Open)
        {
            _connection.Open();
        }

        using var transaction = _connection.BeginTransaction();
        try
        {
            await CatalogRelink.LockForCatalogChangeAsync(_connection, transaction, ct);
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

    private sealed class FeatureRow
    {
        public long Id { get; init; }
        public long ProductId { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
        public long? ReleaseDate { get; init; }
        public int EventCount { get; init; }
    }

    private sealed class ContentRow
    {
        public long FeatureId { get; init; }
        public string CollectionName { get; init; } = string.Empty;
        public string ItemSlug { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public bool IsThumbnail { get; init; }
        public int SortOrder { get; init; }
        public bool IsDetected { get; init; }
    }

    private sealed class AliasRow
    {
        public long Id { get; init; }
        public long FeatureId { get; init; }
        public string Alias { get; init; } = string.Empty;
    }
}
