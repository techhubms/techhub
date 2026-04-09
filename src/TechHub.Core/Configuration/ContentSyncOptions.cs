namespace TechHub.Core.Configuration;

/// <summary>
/// Configuration options for content synchronization from markdown files.
/// </summary>
public class ContentSyncOptions
{
    public bool Enabled { get; set; } = true;
    public bool ForceFullSync { get; set; }
    public int MaxParallelFiles { get; set; } = 10;
    public int BulkOperationThreshold { get; set; } = 50;
}
