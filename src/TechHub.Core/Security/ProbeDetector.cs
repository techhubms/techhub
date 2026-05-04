namespace TechHub.Core.Security;

/// <summary>
/// Detects scanner/attacker probe requests based on well-known exploit probe patterns.
/// Used both in middleware (to return 404 immediately) and in the OpenTelemetry filter
/// (to suppress telemetry entirely, so no Activity span is ever created).
/// </summary>
public static class ProbeDetector
{
    // File extensions that are never served by this site and always indicate a probe.
    // Legitimate static assets (.js, .css, .png, etc.) are handled by UseStaticFiles
    // before the middleware runs and never reach this point.
    // .xml is handled separately to allow /feed.xml and /sitemap.xml endpoints through.
    private static readonly HashSet<string> _probeExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        // Server-side scripts
        ".php", ".asp", ".aspx", ".cfm", ".cgi", ".pl", ".py", ".rb", ".jsp",
        // Config / credential files
        ".env", ".htaccess", ".htpasswd",
        // Backup / leftover files
        ".bak", ".backup", ".old", ".orig", ".swp",
        // Executables / binaries
        ".exe", ".dll", ".sh", ".bat", ".cmd",
        // Database dumps
        ".sql",
        // Certificates and keys
        ".pem", ".key", ".crt", ".p12", ".pfx",
        // Archives
        ".zip", ".tar", ".gz", ".rar", ".7z",
        // Source maps — never published to production; browser devtools / scanners only
        ".map",
    };

    // Path substrings whose presence anywhere in the URL path always indicates a probe.
    // These are framework/CMS-specific paths that can never exist on this site.
    private static readonly string[] _probePathSubstrings =
    [
        // WordPress attack surface (most common automated scanner targets)
        "wp-admin", "wp-content", "wp-includes", "wp-login",
        // WordPress XML-RPC exploit vector
        "xmlrpc",
        // PHP admin panels
        "phpmyadmin",
        // CGI directory traversal
        "cgi-bin",
        // Spring Boot actuator endpoints
        "actuator",
        // Generic application probes
        "app", "login", "ip",
    ];

    // Paths that are legitimate application routes on this site but whose final segment
    // happens to match a probe keyword. These are checked before the substring scan so
    // that real admin/auth endpoints are never mistakenly blocked or telemetry-suppressed.
    private static readonly HashSet<string> _allowedPaths = new(StringComparer.OrdinalIgnoreCase)
    {
        "/admin/login",
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

        // Known legitimate application paths that happen to match a probe keyword.
        if (_allowedPaths.Contains(path))
        {
            return false;
        }

        // Require a segment boundary so substrings only match complete path segments.
        // e.g. "/actuator/health" is a probe but "/ai/actuator-systems" is not.
        // EndsWith covers "/wp-admin" (final segment); Contains covers "/wp-admin/...".
        if (_probePathSubstrings.Any(probe =>
            path.EndsWith("/" + probe, StringComparison.OrdinalIgnoreCase) ||
            path.Contains("/" + probe + "/", StringComparison.OrdinalIgnoreCase)))
        {
            return true;
        }

        // Trim trailing slashes and extract the extension from the final segment only,
        // so paths like /.env/ or /random.xml/ are correctly identified as probes.
        var normalized = path.TrimEnd('/');
        var lastSlash = normalized.LastIndexOf('/');
        var lastSegment = normalized[(lastSlash + 1)..];
        var lastDot = lastSegment.LastIndexOf('.');
        if (lastDot < 0)
        {
            return false;
        }

        var ext = lastSegment[lastDot..];

        // .xml is used for RSS feeds (/all/feed.xml, /{section}/feed.xml) and the
        // sitemap (/sitemap.xml). Allow those through; block all other .xml paths.
        if (ext.Equals(".xml", StringComparison.OrdinalIgnoreCase))
        {
            return !normalized.EndsWith("/feed.xml", StringComparison.OrdinalIgnoreCase)
                && !normalized.Equals("/sitemap.xml", StringComparison.OrdinalIgnoreCase);
        }

        return _probeExtensions.Contains(ext);
    }
}
