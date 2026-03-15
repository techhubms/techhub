namespace TechHub.Core.Configuration;

/// <summary>
/// Application configuration settings
/// </summary>
public class AppSettings
{
    public required ContentSettings Content { get; init; }

    /// <summary>
    /// Base URL for the site (e.g., "https://tech.hub.ms" or "https://localhost:7245")
    /// Used for RSS feeds and absolute URL generation
    /// </summary>
    public required string BaseUrl { get; init; }
}

/// <summary>
/// Content-related configuration
/// </summary>
public class ContentSettings
{
    /// <summary>
    /// Path to collections directory (relative to repository root)
    /// </summary>
    public required string CollectionsPath { get; init; }

    /// <summary>
    /// Sections configuration (replaces sections.json file)
    /// </summary>
    public required Dictionary<string, SectionConfig> Sections { get; init; }
}

/// <summary>
/// Section configuration from appsettings.json
/// </summary>
public class SectionConfig
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Url { get; init; }
    public required string Tag { get; init; }
    public required Dictionary<string, CollectionConfig> Collections { get; init; }
}

/// <summary>
/// Collection configuration from appsettings.json
/// </summary>
public class CollectionConfig
{
    public required string Title { get; init; }
    public required string Url { get; init; }
    public required string Description { get; init; }
    public bool Custom { get; init; }

    /// <summary>
    /// Display order for custom pages (lower values appear first).
    /// Only used when Custom=true. Defaults to 0.
    /// </summary>
    public int Order { get; init; }
}
