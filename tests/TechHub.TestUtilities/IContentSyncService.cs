namespace TechHub.TestUtilities;

/// <summary>
/// Service for synchronizing markdown files from the filesystem into the database.
/// Used only in tests for seeding test databases from markdown fixtures.
/// </summary>
public interface IContentSyncService
{
    /// <summary>
    /// Synchronize content from markdown files to database.
    /// Uses hash-based diff to detect changes (INSERT/UPDATE/DELETE).
    /// Entire sync runs in a transaction - rolls back on any error.
    /// </summary>
    Task<SyncResult> SyncAsync(CancellationToken ct = default);

    /// <summary>
    /// Force a full resync of all content (ignores hashes).
    /// Deletes all existing content and re-imports everything.
    /// </summary>
    Task<SyncResult> ForceSyncAsync(CancellationToken ct = default);

    /// <summary>
    /// Check if content has changed since last sync without actually syncing.
    /// Uses hash comparison for fast detection.
    /// </summary>
    Task<bool> IsContentChangedAsync(CancellationToken ct = default);

    /// <summary>
    /// Check if the database has any content items.
    /// </summary>
    Task<bool> HasContentAsync(CancellationToken ct = default);
}

/// <summary>
/// Result of a content sync operation.
/// </summary>
public record SyncResult(
    int Added,
    int Updated,
    int Deleted,
    int Unchanged,
    TimeSpan Duration);
