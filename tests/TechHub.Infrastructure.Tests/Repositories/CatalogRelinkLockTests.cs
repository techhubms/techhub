using Dapper;
using FluentAssertions;
using Npgsql;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// A catalog change and a write of article links never run side by side, so a link that is written while a product is
/// being added is still linked to that product once both have committed.
/// </summary>
public class CatalogRelinkLockTests : IClassFixture<DatabaseFixture<CatalogRelinkLockTests>>
{
    private static readonly TimeSpan _blockedFor = TimeSpan.FromMilliseconds(750);

    private readonly DatabaseFixture<CatalogRelinkLockTests> _fixture;

    public CatalogRelinkLockTests(DatabaseFixture<CatalogRelinkLockTests> fixture)
    {
        _fixture = fixture;
    }

    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    private async Task<string> SeedArticleAsync(string productName)
    {
        var slug = $"lock-{Guid.NewGuid():N}";
        await _fixture.Connection.ExecuteAsync(
            """
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch,
                 primary_section_name, external_url, author, feed_name, tags_csv,
                 sections_bitmask, content_hash, is_ai, ai_metadata, created_at)
            VALUES (@Slug, 'news', 'Lock article', '', '', 1711699200,
                    'ai', @Url, 'Author', 'Feed', '', 1, 'hash-lock', TRUE, @Metadata::jsonb, NOW())
            """,
            new { Slug = slug, Url = $"https://example.com/{slug}", Metadata = $$"""{"schema_version":2,"products":["{{productName}}"]}""" });
        return slug;
    }

    private async Task<(NpgsqlConnection Connection, NpgsqlTransaction Transaction)> BeginAsync()
    {
        var connection = await _fixture.DataSource.OpenConnectionAsync(Ct);
        return (connection, await connection.BeginTransactionAsync(Ct));
    }

    private static async Task<bool> CompletesWithinAsync(Task task, TimeSpan time)
    {
        var finished = await Task.WhenAny(task, Task.Delay(time, Ct));
        return finished == task;
    }

    [Fact]
    public async Task LinkWrite_DuringACatalogChange_WaitsForItAndLinksTheNewProduct()
    {
        var name = $"Lock Product {Guid.NewGuid():N}";
        var slug = await SeedArticleAsync(name);

        var (catalogConnection, catalogTransaction) = await BeginAsync();
        await using var catalogConnectionScope = catalogConnection;
        await using var catalogTransactionScope = catalogTransaction;
        await CatalogRelink.LockForCatalogChangeAsync(catalogConnection, catalogTransaction, Ct);
        var productId = await catalogConnection.ExecuteScalarAsync<long>(
            new CommandDefinition("INSERT INTO products (name) VALUES (@Name) RETURNING id", new { Name = name }, catalogTransaction, cancellationToken: Ct));

        var writer = Task.Run(async () =>
        {
            var (connection, transaction) = await BeginAsync();
            await using var _ = connection;
            await using var __ = transaction;
            await ProductLinkWriter.ReplaceItemLinksAsync(connection, transaction, "news", slug, Ct);
            await transaction.CommitAsync(Ct);
        }, Ct);

        (await CompletesWithinAsync(writer, _blockedFor)).Should().BeFalse("the link write waits for the open catalog change");

        await catalogTransaction.CommitAsync(Ct);
        await writer;

        var linked = await _fixture.Connection.ExecuteScalarAsync<long?>(
            "SELECT product_id FROM content_item_products WHERE collection_name = 'news' AND item_slug = @Slug", new { Slug = slug });
        linked.Should().Be(productId);
    }

    [Fact]
    public async Task CatalogChange_DuringALinkWrite_WaitsForItsCommit()
    {
        var name = $"Lock Product {Guid.NewGuid():N}";
        var slug = await SeedArticleAsync(name);

        var (writerConnection, writerTransaction) = await BeginAsync();
        await using var writerConnectionScope = writerConnection;
        await using var writerTransactionScope = writerTransaction;
        await ProductLinkWriter.ReplaceItemLinksAsync(writerConnection, writerTransaction, "news", slug, Ct);

        var catalog = Task.Run(async () =>
        {
            var (connection, transaction) = await BeginAsync();
            await using var _ = connection;
            await using var __ = transaction;
            await CatalogRelink.LockForCatalogChangeAsync(connection, transaction, Ct);
            await connection.ExecuteAsync(new CommandDefinition("INSERT INTO products (name) VALUES (@Name)", new { Name = name }, transaction, cancellationToken: Ct));
            await CatalogRelink.RelinkAsync(connection, transaction, Ct);
            await transaction.CommitAsync(Ct);
        }, Ct);

        (await CompletesWithinAsync(catalog, _blockedFor)).Should().BeFalse("the catalog change waits for the running link write");

        await writerTransaction.CommitAsync(Ct);
        await catalog;

        var linked = await _fixture.Connection.ExecuteScalarAsync<long?>(
            "SELECT l.product_id FROM content_item_products l WHERE l.collection_name = 'news' AND l.item_slug = @Slug", new { Slug = slug });
        linked.Should().NotBeNull("the relink of the catalog change covers the link that was written before it");
    }

    [Fact]
    public async Task LinkWrites_DoNotBlockEachOther()
    {
        var first = await SeedArticleAsync("Shared Lock A");
        var second = await SeedArticleAsync("Shared Lock B");

        var (connection1, transaction1) = await BeginAsync();
        await using var _ = connection1;
        await using var __ = transaction1;
        await ProductLinkWriter.ReplaceItemLinksAsync(connection1, transaction1, "news", first, Ct);

        var (connection2, transaction2) = await BeginAsync();
        await using var ___ = connection2;
        await using var ____ = transaction2;
        var other = ProductLinkWriter.ReplaceItemLinksAsync(connection2, transaction2, "news", second, Ct);

        (await CompletesWithinAsync(other, TimeSpan.FromSeconds(10))).Should().BeTrue();
    }
}
