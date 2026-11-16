using System.Data;
using System.Data.Common;

namespace TechHub.Infrastructure.Data;

public static class DbConnectionExtensions
{
    /// <summary>
    /// Opens the connection when it is closed, waiting for a pooled connection asynchronously.
    /// A cancelled caller stops waiting on an exhausted pool instead of blocking a thread until the pool timeout.
    /// </summary>
    public static async Task EnsureOpenAsync(this IDbConnection connection, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(connection);

        if (connection.State == ConnectionState.Open)
        {
            return;
        }

        if (connection is DbConnection dbConnection)
        {
            await dbConnection.OpenAsync(ct);
        }
        else
        {
            connection.Open();
        }
    }
}
