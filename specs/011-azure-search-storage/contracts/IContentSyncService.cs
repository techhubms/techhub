namespace TechHub.Infrastructure.Services;

/// <summary>
/// Service for syncing markdown files from Git repository to database.
/// Implements hash-based incremental sync for fast startup.
/// </summary>
public interface IContentSyncService
{
    /// <summary>
    /// Perform incremental sync (default behavior).
    /// Compares file hashes vs database hashes to determine changes.
    /// Only processes INSERT/UPDATE/DELETE for changed files.
    /// </summary>
    /// <returns>Sync result with counts and duration.</returns>
    Task<SyncResult> SyncAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Force full re-import (ignores hashes, deletes all content and re-imports).
    /// Used when ContentSync:ForceFullSync = true in appsettings.
    /// </summary>
    /// <returns>Sync result with counts and duration.</returns>
    Task<SyncResult> ForceSyncAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Check if any content has changed since last sync (fast hash comparison only).
    /// Does not perform sync, just checks if sync would process any changes.
    /// </summary>
    /// <returns>True if changes detected, false otherwise.</returns>
    Task<bool> IsContentChangedAsync(CancellationToken ct = default);
}

/// <summary>
/// Result of content sync operation.
/// </summary>
public record SyncResult(
    int Added,      // Files added (new content)
    int Updated,    // Files updated (changed content)
    int Deleted,    // Files deleted (removed content)
    int Unchanged,  // Files skipped (hash match)
    TimeSpan Duration);
