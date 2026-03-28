namespace TechHub.Core.Models.Admin;

/// <summary>
/// Database statistics for the admin dashboard.
/// </summary>
public sealed class DatabaseStatistics
{
    /// <summary>Total number of content items in the database.</summary>
    public int TotalContentItems { get; init; }

    /// <summary>Content items grouped by collection.</summary>
    public required IReadOnlyList<CollectionCount> ItemsByCollection { get; init; }

    /// <summary>Content items grouped by primary section.</summary>
    public required IReadOnlyList<SectionCount> ItemsBySection { get; init; }

    /// <summary>Total number of unique tags.</summary>
    public int TotalUniqueTags { get; init; }

    /// <summary>Total number of tag word expansions.</summary>
    public int TotalTagWords { get; init; }

    /// <summary>Most recent content items added to the database.</summary>
    public required IReadOnlyList<RecentItem> LatestItems { get; init; }

    /// <summary>Database size information (PostgreSQL-specific).</summary>
    public required DatabaseSizeInfo DatabaseSize { get; init; }

    /// <summary>Table-level size breakdown.</summary>
    public required IReadOnlyList<TableSizeInfo> TableSizes { get; init; }

    /// <summary>Top 10 slowest query patterns from pg_stat_statements (if available).</summary>
    public IReadOnlyList<SlowQuery>? SlowQueries { get; init; }

    /// <summary>Content processing statistics.</summary>
    public required ProcessingStats Processing { get; init; }

    /// <summary>When these statistics were generated (Europe/Brussels).</summary>
    public DateTimeOffset GeneratedAt { get; init; }
}

/// <summary>Content count per collection.</summary>
public sealed class CollectionCount
{
    public required string CollectionName { get; init; }
    public int Count { get; init; }
}

/// <summary>Content count per section.</summary>
public sealed class SectionCount
{
    public required string SectionName { get; init; }
    public int Count { get; init; }
}

/// <summary>Recent content item summary.</summary>
public sealed class RecentItem
{
    public required string Title { get; init; }
    public required string CollectionName { get; init; }
    public required string Slug { get; init; }
    public required string PrimarySectionName { get; init; }
    public long DateEpoch { get; init; }
    public DateTimeOffset? CreatedAt { get; init; }
}

/// <summary>Database size information.</summary>
public sealed class DatabaseSizeInfo
{
    public required string DatabaseName { get; init; }
    public required string TotalSize { get; init; }
    public required string DataSize { get; init; }
    public required string IndexSize { get; init; }
}

/// <summary>Individual table size information.</summary>
public sealed class TableSizeInfo
{
    public required string TableName { get; init; }
    public int RowCount { get; init; }
    public required string TotalSize { get; init; }
    public required string DataSize { get; init; }
    public required string IndexSize { get; init; }
}

/// <summary>Slow query information from pg_stat_statements.</summary>
public sealed class SlowQuery
{
    public required string Query { get; init; }
    public long Calls { get; init; }
    public double MeanTimeMs { get; init; }
    public double TotalTimeMs { get; init; }
}

/// <summary>Content processing job statistics.</summary>
public sealed class ProcessingStats
{
    public int TotalJobs { get; init; }
    public int CompletedJobs { get; init; }
    public int FailedJobs { get; init; }
    public int TotalItemsAdded { get; init; }
    public int TotalProcessedUrls { get; init; }
    public DateTimeOffset? LastRunAt { get; init; }
}
