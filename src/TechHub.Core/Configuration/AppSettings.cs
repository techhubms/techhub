namespace TechHub.Core.Configuration;

/// <summary>
/// Application configuration settings
/// </summary>
public class AppSettings
{
    public required ContentSettings Content { get; init; }
    public required CachingSettings Caching { get; init; }
    public required SeoSettings Seo { get; init; }
    public required PerformanceSettings Performance { get; init; }
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

    /// <summary>
    /// Timezone for date handling (e.g., "Europe/Brussels")
    /// </summary>
    public required string Timezone { get; init; }

    /// <summary>
    /// Maximum excerpt length in characters
    /// </summary>
    public int MaxExcerptLength { get; init; } = 1000;

    /// <summary>
    /// Display names for collections in page titles (e.g., "blogs" -> "Blog Posts")
    /// </summary>
    public Dictionary<string, string> CollectionDisplayNames { get; init; } = [];
}

/// <summary>
/// Section configuration from appsettings.json
/// </summary>
public class SectionConfig
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Url { get; init; }
    public required string Section { get; init; }
    public required string Image { get; init; }
    public required string Category { get; init; }
    public required List<CollectionConfig> Collections { get; init; }
}

/// <summary>
/// Collection configuration from appsettings.json
/// </summary>
public class CollectionConfig
{
    public required string Title { get; init; }
    public required string Url { get; init; }
    public string? Collection { get; init; }
    public required string Description { get; init; }
    public bool Custom { get; init; }
}

/// <summary>
/// Caching-related configuration
/// </summary>
public class CachingSettings
{
    /// <summary>
    /// Absolute expiration in minutes for content cache
    /// </summary>
    public int ContentAbsoluteExpirationMinutes { get; init; } = 60;

    /// <summary>
    /// Sliding expiration in minutes for content cache
    /// </summary>
    public int ContentSlidingExpirationMinutes { get; init; } = 30;

    /// <summary>
    /// Absolute expiration in minutes for API responses
    /// </summary>
    public int ApiResponseAbsoluteExpirationMinutes { get; init; } = 60;

    /// <summary>
    /// Enable output caching for static pages
    /// </summary>
    public bool EnableOutputCaching { get; init; } = true;
}

/// <summary>
/// SEO-related configuration
/// </summary>
public class SeoSettings
{
    /// <summary>
    /// Site base URL (e.g., "https://tech.hub.ms")
    /// </summary>
    public required string BaseUrl { get; init; }

    /// <summary>
    /// Site title
    /// </summary>
    public required string SiteTitle { get; init; }

    /// <summary>
    /// Site description
    /// </summary>
    public required string SiteDescription { get; init; }

    /// <summary>
    /// Google Analytics tracking ID (optional)
    /// </summary>
    public string? GoogleAnalyticsId { get; init; }
}

/// <summary>
/// Performance-related configuration
/// </summary>
public class PerformanceSettings
{
    /// <summary>
    /// Enable response compression (Brotli/Gzip)
    /// </summary>
    public bool EnableCompression { get; init; } = true;

    /// <summary>
    /// Enable HTTP/2
    /// </summary>
    public bool EnableHttp2 { get; init; } = true;

    /// <summary>
    /// Enable HTTP/3
    /// </summary>
    public bool EnableHttp3 { get; init; } = false;

    /// <summary>
    /// Max concurrent requests
    /// </summary>
    public int MaxConcurrentRequests { get; init; } = 10000;
}

/// <summary>
/// Web-specific application settings (simplified for frontend)
/// </summary>
public class WebAppSettings
{
    /// <summary>
    /// Display names for collections in page titles (e.g., "blogs" -> "Blog Posts")
    /// </summary>
    public Dictionary<string, string> CollectionDisplayNames { get; init; } = [];

    public required SeoSettings Seo { get; init; }
}
