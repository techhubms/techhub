namespace TechHub.Core.Models;

/// <summary>
/// Predefined date range presets for filtering
/// </summary>
public enum DateRangePreset
{
    /// <summary>Last 7 days from today</summary>
    Last7Days = 0,

    /// <summary>Last 30 days from today</summary>
    Last30Days = 1,

    /// <summary>Last 90 days from today (default)</summary>
    Last90Days = 2,

    /// <summary>All time (no date filter)</summary>
    AllTime = 3,

    /// <summary>Custom date range (user-defined)</summary>
    Custom = 4
}
