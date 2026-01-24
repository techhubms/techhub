namespace TechHub.Web.Middleware;

/// <summary>
/// Middleware that takes full control of cache headers for all static files.
/// Place this FIRST in the pipeline (before MapStaticAssets) to override any
/// built-in cache headers with our own policies.
/// </summary>
public class StaticFilesCacheMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    // Extensions that should be cached for 1 year (immutable)
    // These are files that rarely change and benefit from aggressive caching
    private static readonly HashSet<string> ImmutableCacheExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        // Images
        ".jpg", ".jpeg", ".png", ".gif", ".webp", ".jxl", ".svg", ".ico", ".bmp",
        // Fonts
        ".woff", ".woff2", ".ttf", ".eot", ".otf"
    };

    // Extensions that should use short cache with revalidation
    // These are files that may change but should still be cached
    private static readonly HashSet<string> ShortCacheExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".css", ".js", ".json", ".xml", ".html", ".htm"
    };

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var path = context.Request.Path.Value ?? string.Empty;
        var extension = Path.GetExtension(path);

        // Only process requests that look like static files
        if (!string.IsNullOrEmpty(extension))
        {
            // Register callback to set cache headers AFTER the response is prepared
            // OnStarting callbacks run in reverse order, so registering first means running last
            // This allows us to override any headers set by MapStaticAssets
            context.Response.OnStarting(() =>
            {
                // Only set cache headers for successful responses
                if (context.Response.StatusCode != 200)
                {
                    return Task.CompletedTask;
                }

                // Check if this is a fingerprinted file (contains hash in filename like .abc123xyz.)
                // Fingerprinted files can be cached forever since content changes = new URL
                var isFingerprinted = IsFingerprinted(path);

                if (isFingerprinted)
                {
                    // Fingerprinted assets: cache forever (immutable)
                    context.Response.Headers.CacheControl = "public, max-age=31536000, immutable";
                    // Remove Vary header that might prevent caching
                    context.Response.Headers.Remove("Vary");
                }
                else if (ImmutableCacheExtensions.Contains(extension))
                {
                    // Images and fonts: cache for 1 year (immutable)
                    // These rarely change and users benefit from not re-downloading
                    context.Response.Headers.CacheControl = "public, max-age=31536000, immutable";
                    // Remove Vary header that might prevent caching
                    context.Response.Headers.Remove("Vary");
                }
                else if (ShortCacheExtensions.Contains(extension))
                {
                    // CSS/JS/HTML: short cache with revalidation
                    // Non-fingerprinted versions should revalidate frequently
                    context.Response.Headers.CacheControl = "public, max-age=3600, must-revalidate";
                }
                // Other files: leave default headers (MapStaticAssets defaults)

                return Task.CompletedTask;
            });
        }

        await _next(context);
    }

    /// <summary>
    /// Checks if a path contains a fingerprint hash (10+ alphanumeric chars in any segment).
    /// Examples: styles.r2lq8zdogi.css, blazor.web.2i0r6wwb69.js, TechHub.Web.r2lq8zdogi.styles.css
    /// </summary>
    private static bool IsFingerprinted(string path)
    {
        // Get filename without directory
        var fileName = Path.GetFileName(path);
        if (string.IsNullOrEmpty(fileName))
        {
            return false;
        }

        // Split by dots and check if ANY middle segment looks like a hash
        // Fingerprint hashes are typically 10 lowercase alphanumeric chars
        var parts = fileName.Split('.');
        if (parts.Length < 3)
        {
            return false; // Need at least name.hash.ext
        }

        // Check all middle segments (skip first and last) for hash-like patterns
        for (int i = 1; i < parts.Length - 1; i++)
        {
            var segment = parts[i];
            // Hash pattern: 10+ chars, all lowercase alphanumeric (letters lowercase, digits allowed)
            if (segment.Length >= 10 && segment.All(c => char.IsDigit(c) || char.IsLower(c)))
            {
                return true;
            }
        }

        return false;
    }
}

/// <summary>
/// Extension methods for registering the middleware.
/// </summary>
public static class StaticFilesCacheMiddlewareExtensions
{
    /// <summary>
    /// Adds middleware that controls cache headers for all static files.
    /// Must be placed BEFORE MapStaticAssets() in the pipeline.
    /// </summary>
    public static IApplicationBuilder UseStaticFilesCaching(this IApplicationBuilder app)
    {
        return app.UseMiddleware<StaticFilesCacheMiddleware>();
    }
}
