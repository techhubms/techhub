using TechHub.Core.Models;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Service for synchronizing markdown files from the filesystem into the database.
/// Uses hash-based change detection for incremental updates.
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
    /// Used to determine if sync should run (sync only when database is empty).
    /// </summary>
    Task<bool> HasContentAsync(CancellationToken ct = default);
}

