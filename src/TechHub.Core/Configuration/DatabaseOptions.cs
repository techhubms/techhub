namespace TechHub.Core.Configuration;

/// <summary>
/// Configuration options for database connection and provider.
/// </summary>
public class DatabaseOptions
{
    /// <summary>
    /// Database provider: "SQLite" or "PostgreSQL"
    /// </summary>
    public required string Provider { get; set; }

    /// <summary>
    /// Database connection string
    /// </summary>
    public required string ConnectionString { get; set; }
}
