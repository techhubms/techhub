using Azure.Core;
using Azure.Identity;
using Npgsql;

namespace TechHub.Infrastructure.Data;

/// <summary>
/// Creates an <see cref="NpgsqlDataSource"/> configured for either password-based or
/// Azure Entra ID (managed identity) authentication.
/// </summary>
public static class NpgsqlDataSourceFactory
{
    // Azure AD token scope required for PostgreSQL Flexible Server Entra auth.
    private static readonly string[] PostgresTokenScope =
        ["https://ossrdbms-aad.database.windows.net/.default"];

    // DefaultAzureCredential is thread-safe and reusable; share a single instance
    // across all token refreshes for the data source.
    private static readonly DefaultAzureCredential SharedCredential = new();

    // Interval at which a fresh token is proactively fetched.
    // AAD tokens expire after 1 hour; use 50 minutes so token is always fresh when
    // new pooled connections are opened between refresh cycles.
    private static readonly TimeSpan TokenRefreshSuccessInterval = TimeSpan.FromMinutes(50);

    // Retry interval when token acquisition fails.
    private static readonly TimeSpan TokenRefreshFailureInterval = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Creates an <see cref="NpgsqlDataSource"/> for the given connection string.
    /// When <paramref name="useEntraAuth"/> is <see langword="true"/> the data source
    /// acquires an Azure AD token from the container's managed identity instead of using
    /// a password. This is used for PR preview environments, where each environment has its
    /// own user-assigned managed identity registered as the sole Entra admin on its PITR
    /// PostgreSQL server—so no shared credentials exist between PR and production.
    /// </summary>
    public static NpgsqlDataSource Create(string connectionString, bool useEntraAuth = false)
    {
        if (!useEntraAuth)
        {
            return NpgsqlDataSource.Create(connectionString);
        }

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

        // DefaultAzureCredential picks up the user-assigned managed identity automatically
        // when only one is assigned to the Container App.
        dataSourceBuilder.UsePeriodicPasswordProvider(
            async (_, ct) =>
            {
                var token = await SharedCredential.GetTokenAsync(
                    new TokenRequestContext(PostgresTokenScope), ct);
                return token.Token;
            },
            TokenRefreshSuccessInterval,
            TokenRefreshFailureInterval);

        return dataSourceBuilder.Build();
    }
}
