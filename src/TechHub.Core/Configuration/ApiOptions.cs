namespace TechHub.Core.Configuration;

/// <summary>
/// Configuration options for API pagination and response limits
/// </summary>
public class ApiOptions
{
    /// <summary>
    /// Default number of items to return when no limit is specified
    /// </summary>
    public int DefaultPageSize { get; set; } = 20;

    /// <summary>
    /// Maximum number of items that can be requested in a single call
    /// </summary>
    public int MaxPageSize { get; set; } = 50;
}
