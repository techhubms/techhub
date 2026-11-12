using System.Data;
using Dapper;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// Re-evaluates which catalog entries the product names and features of articles and timeline events match.
/// Runs inside the transaction of the catalog change, so the links never disagree with the catalog.
/// </summary>
/// <remarks>
/// A relink only sees the links that are committed. Without coordination, an article write that commits its links while a
/// catalog change is still open (and that cannot see the new entry yet) would stay unlinked after both commit. Therefore
/// a catalog change holds an exclusive transaction-scoped advisory lock from its start, and every write of article links or
/// timeline events holds the shared lock: writers wait for a running catalog change and see its result, and a catalog
/// change waits until the running writers have committed, so its relink covers their links. A writer takes the shared lock
/// only after it has written or locked the row of its article, which keeps the three parties free of deadlocks.
/// </remarks>
internal static class CatalogRelink
{
    // Arbitrary, but it must differ from every other advisory lock key of the application.
    private const long LockKey = 0x7465_6368_6875_6201;

    /// <summary>SQL that relinks all articles and events.</summary>
    public const string Sql = "SELECT relink_item_products(); SELECT relink_timeline_events();";

    public static Task RelinkAsync(IDbConnection connection, IDbTransaction transaction, CancellationToken ct) =>
        connection.ExecuteAsync(new CommandDefinition(Sql, transaction: transaction, cancellationToken: ct));

    /// <summary>Starts a catalog change: waits for the writers of links and events, and blocks new ones until the transaction ends.</summary>
    public static Task LockForCatalogChangeAsync(IDbConnection connection, IDbTransaction transaction, CancellationToken ct) =>
        connection.ExecuteAsync(new CommandDefinition("SELECT pg_advisory_xact_lock(@Key)", new { Key = LockKey }, transaction, cancellationToken: ct));

    /// <summary>Starts a write of article links or timeline events: waits for a running catalog change, then lets it wait for this transaction.</summary>
    public static Task LockForLinkWriteAsync(IDbConnection connection, IDbTransaction transaction, CancellationToken ct) =>
        connection.ExecuteAsync(new CommandDefinition("SELECT pg_advisory_xact_lock_shared(@Key)", new { Key = LockKey }, transaction, cancellationToken: ct));
}
