namespace TechHub.Core.Models;

/// <summary>
/// Result of a content sync operation.
/// </summary>
public record SyncResult(
    int Added,
    int Updated,
    int Deleted,
    int Unchanged,
    TimeSpan Duration);
