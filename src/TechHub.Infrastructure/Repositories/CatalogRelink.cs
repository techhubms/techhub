using System.Data;
using Dapper;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// Re-evaluates which catalog entries the product names, plans and surfaces of articles and timeline events match.
/// Runs inside the transaction of the catalog change, so the links never disagree with the catalog.
/// </summary>
internal static class CatalogRelink
{
    /// <summary>SQL that relinks all articles and events.</summary>
    public const string Sql = "SELECT relink_item_products(); SELECT relink_timeline_events();";

    public static Task RelinkAsync(IDbConnection connection, IDbTransaction transaction, CancellationToken ct) =>
        connection.ExecuteAsync(new CommandDefinition(Sql, transaction: transaction, cancellationToken: ct));
}
