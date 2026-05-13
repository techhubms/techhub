using System.Text.RegularExpressions;

namespace TechHub.Core.Security;

/// <summary>
/// Detects scanner/attacker probe requests based on well-known exploit probe patterns.
/// Used both in middleware (to return 404 immediately) and in the OpenTelemetry filter
/// (to suppress telemetry entirely, so no Activity span is ever created).
/// </summary>
public static partial class ProbeDetector
{
    // Single-pass regex that matches any probe path segment at a segment boundary.
    // Compiled at build time via source generator — zero runtime allocation.
    [GeneratedRegex(
        @"/(wp-admin|wp-content|wp-includes|wp-login|xmlrpc|phpmyadmin|cgi-bin|\.well-known|actuator|app|login|ip|assets|static|media|dist|vendor|backend|config)(/|$)",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex ProbeSegmentPattern();

    // Paths that are legitimate application routes on this site but whose final segment
    // happens to match a probe keyword. These are checked before the substring scan so
    // that real admin/auth endpoints are never mistakenly blocked or telemetry-suppressed.
    private static readonly HashSet<string> _allowedPaths = new(StringComparer.OrdinalIgnoreCase)
    {
        "/admin/login",
    };

    // Per-directory extension whitelists for static assets served by this site.
    // Each entry maps a path prefix to the set of extensions that are valid under it.
    // The lookup is a span-based alternate lookup so the extension can be matched
    // directly against a ReadOnlySpan<char> sliced from the path — no string allocation.
    // Any file-extension request that does not match a prefix+extension pair is rejected
    // with 404 before it reaches Blazor or generates telemetry.
    // Order matters for performance: most-frequently-requested directories first.
    private static readonly (string Prefix, HashSet<string>.AlternateLookup<ReadOnlySpan<char>> Extensions)[] _knownStaticDirectories =
    [
        // wwwroot/js/ — plain filenames and fingerprinted variants (name.{hash}.js)
        ("/js/",          BuildExtensionLookup(".js")),
        // wwwroot/css/ — plain filenames and fingerprinted variants
        ("/css/",         BuildExtensionLookup(".css")),
        // Blazor framework bundles
        ("/_framework/",  BuildExtensionLookup(".js", ".wasm", ".gz", ".br")),
        // Root-level app bundle assets: TechHub.Web.{hash}.styles.css, TechHub.Web.lib.module.js
        ("/TechHub.Web.", BuildExtensionLookup(".css", ".js")),
        // Blazor collocated component JS files: /Components/{Path}/{Name}.{hash}.razor.js
        ("/Components/",  BuildExtensionLookup(".js")),
        // RCL static assets — can include js/css/images from component libraries
        ("/_content/",    BuildExtensionLookup(".js", ".css", ".png", ".jpg", ".svg", ".webp", ".woff", ".woff2")),
        // wwwroot/images/ — only image formats actually present in the repository
        ("/images/",      BuildExtensionLookup(".jpg", ".jxl", ".png", ".svg", ".webp")),
    ];

    private static HashSet<string>.AlternateLookup<ReadOnlySpan<char>> BuildExtensionLookup(params string[] extensions)
        => new HashSet<string>(extensions, StringComparer.OrdinalIgnoreCase).GetAlternateLookup<ReadOnlySpan<char>>();

    // Exact root-level files served by this site. HashSet for O(1) lookup.
    private static readonly HashSet<string> _knownRootFiles = new(StringComparer.OrdinalIgnoreCase)
    {
        "/favicon.ico",
        "/robots.txt",
        "/sitemap.xml",
    };

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="path"/> matches a known scanner
    /// or attacker probe pattern.
    /// </summary>
    public static bool IsProbeRequest(string? path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return false;
        }

        // Normalize once: strip trailing slashes so "/admin/login/" and "/admin/login"
        // are treated identically by all checks below.
        var normalized = path.TrimEnd('/');

        // Known legitimate application paths that happen to match a probe keyword.
        if (_allowedPaths.Contains(normalized))
        {
            return false;
        }

        // Single automaton pass over the path — faster than looping EndsWith/Contains
        // and consistent with the ValidSegmentPattern() approach in the middleware.
        if (ProbeSegmentPattern().IsMatch(normalized))
        {
            return true;
        }

        // robots.txt is only meaningful at the site root; any sub-path variant is a probe.
        // Real crawlers always request /robots.txt — never /all/robots.txt or similar.
        if (!normalized.Equals("/robots.txt", StringComparison.OrdinalIgnoreCase)
            && normalized.EndsWith("/robots.txt", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="path"/> matches a known legitimate
    /// static asset pattern served by this site. Used as an allowlist for file-extension
    /// requests: any URL with a file extension that does not match these patterns is not
    /// served by this application and should be rejected with 404.
    /// </summary>
    public static bool IsKnownStaticAssetPath(string? path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return false;
        }

        // Extract the extension from the final path segment for per-directory validation.
        // Stays as ReadOnlySpan<char> — no allocation.
        var lastSlash = path.LastIndexOf('/');
        var lastSegment = path.AsSpan()[(lastSlash + 1)..];
        var dotIndex = lastSegment.LastIndexOf('.');
        var ext = dotIndex >= 0 ? lastSegment[dotIndex..] : ReadOnlySpan<char>.Empty;

        // Hot path: prefix + extension check. Covers js/, css/, /_framework/, /TechHub.Web.*, etc.
        // Most real asset requests short-circuit here on the first or second iteration.
        foreach (var (prefix, extensions) in _knownStaticDirectories)
        {
            if (path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                return extensions.Contains(ext);
            }
        }

        // Exact root-level files (rare — one request per crawl/session).
        if (_knownRootFiles.Contains(path))
        {
            return true;
        }

        // RSS feeds: /all/feed.xml, /{section}/feed.xml (rarest — only feed readers).
        if (path.EndsWith("/feed.xml", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="path"/> is either extension-less
    /// (may be a valid Blazor route — let routing decide) or is a known static asset path.
    /// Returns <see langword="false"/> for file-extension requests that are not served by
    /// this site, so callers can suppress telemetry or return 404 immediately.
    /// </summary>
    public static bool IsKnownStaticAssetOrExtensionless(string? path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return true;
        }

        var lastSlash = path.LastIndexOf('/');
        var lastSegment = path.AsSpan()[(lastSlash + 1)..];

        // No dot in the last segment → no file extension → could be a Blazor route.
        if (!lastSegment.Contains('.'))
        {
            return true;
        }

        return IsKnownStaticAssetPath(path);
    }
}
