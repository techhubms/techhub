using System.Text.RegularExpressions;
using TechHub.Core.Models;
using TechHub.Core.Validation;
using TechHub.Web.Services;

namespace TechHub.Web.Middleware;

/// <summary>
/// Unified URL normalization middleware that handles URL cleanup, legacy redirects,
/// and early structural validation — all in a single pass with at most one 301 per request.
///
/// Normalization steps applied in order:
///   1. Strip .html extension from each path segment
///   2. Strip YYYY-MM-DD- date prefixes from each path segment
///   3. Strip a trailing slash from the full path
///   4. Rename legacy section names in the first path segment (for example /coding/* → /dotnet/*)
///
/// After normalization:
///   - Multi-segment paths: validate segment[0] against known sections/pages and segment[1]
///     against the section's collections. Unknown section → 404. Unknown collection → 404.
///     Validation is skipped when the section cache is not ready (API down at startup).
///     If the path changed (normalization applied) and is valid → 301 to the cleaned path.
///   - Single-segment paths that are not a known section, page, or static file:
///     A legacy API lookup is performed ONLY when the original URL contained .html (the format
///     used on the old website). Bare slugs without .html are not legacy URLs — they return
///     404 immediately without an API call. If the API lookup resolves the slug, redirect
///     directly to the canonical URL. If not found, return 404. On transient API failures,
///     redirect to the cleaned path when normalization changed it; otherwise return 503.
///   - The optional ?section= query parameter is remapped through the section rename dictionary
///     before being forwarded to the API (e.g. ?section=coding → section hint "dotnet").
///
/// Case normalization is NOT performed here. The infrastructure layer handles
/// case-insensitive DB lookups so URLs work regardless of capitalisation.
/// </summary>
public partial class UrlNormalizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly SectionCache _sectionCache;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<UrlNormalizationMiddleware> _logger;

    private static readonly HashSet<string> _knownNonSectionPages = new(StringComparer.OrdinalIgnoreCase)
    {
        "about",
        "not-found",
        "error",
        "admin",
        "health",
        "alive",
        // Deployment version probe — returns { tag, deployedAt } for the deploy readiness script.
        "version",
        // OIDC callback path — must pass through to UseAuthentication, never treated as content.
        "signin-oidc",
    };

    // Sections that have been permanently renamed. When a path starts with an old section name,
    // the middleware redirects to the equivalent path under the new name. Applied after segment
    // normalization so that a rename + .html strip produces a single 301.
    private static readonly Dictionary<string, string> _renamedSections =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["coding"] = "dotnet",
        };

    // Virtual sections that exist in the app's routing but are not stored in the API section cache.
    // Each entry maps the virtual section name to dedicated sub-page names that have their own
    // Blazor page and do NOT appear as collections in any real section.
    // The virtual "all" keyword (/{sectionName}/all) and real collection names (news, videos, etc.)
    // are handled dynamically in IsValidMultiSegmentPath via the section cache.
    private static readonly Dictionary<string, HashSet<string>> _knownVirtualSections =
        new(StringComparer.OrdinalIgnoreCase)
        {
            // "/all" aggregates content across all real sections.
            // Dedicated sub-pages: /all/authors (Authors.razor — not a real API collection).
            // All standard collections (roundups, news, videos, etc.) are validated dynamically
            // via IsKnownCollectionInAnySection so new collections are picked up automatically.
            ["all"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "authors" },
        };

    // Matches YYYY-MM-DD- at the start of a segment. Capture group 1 is the slug remainder.
    [GeneratedRegex(@"^\d{4}-\d{2}-\d{2}-(.+)$", RegexOptions.Compiled)]
    private static partial Regex DatePrefixPattern();

    public UrlNormalizationMiddleware(
        RequestDelegate next,
        SectionCache sectionCache,
        IServiceScopeFactory scopeFactory,
        ILogger<UrlNormalizationMiddleware> logger)
    {
        ArgumentNullException.ThrowIfNull(next);
        ArgumentNullException.ThrowIfNull(sectionCache);
        ArgumentNullException.ThrowIfNull(scopeFactory);
        ArgumentNullException.ThrowIfNull(logger);

        _next = next;
        _sectionCache = sectionCache;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var path = context.Request.Path.Value;

        if (string.IsNullOrEmpty(path) || path == "/")
        {
            await _next(context);
            return;
        }

        // Strip trailing slash before segment processing so that a combined
        // trailing-slash + rename/normalization produces a single 301 rather than two
        // (e.g. /coding/ → /dotnet in one redirect, not /coding/ → /coding → /dotnet).
        // This also means a trailing-slash variant of an RSS feed URL (e.g. /feed.xml/)
        // will have the slash removed here before TryRedirectLegacyFeed is called,
        // so it is still matched and redirected correctly.
        if (path.Length > 1 && path.EndsWith('/'))
        {
            path = path.TrimEnd('/');
        }

        // Legacy RSS feed redirects: handled before segment normalization so that
        // /{sectionName}.xml paths are caught here rather than rejected as probes
        // by InvalidRouteSegmentMiddleware further down the pipeline.
        if (TryRedirectLegacyFeed(context, path))
        {
            return;
        }

        // Normalize each segment: strip .html extension, strip YYYY-MM-DD- date prefix.
        var rawSegments = path.TrimStart('/').Split('/');

        // Track whether ANY raw segment contained .html before normalization.
        // Legacy API lookups are gated on this: the old website always used date-slug.html
        // as its URL format. Bare slugs (no .html) are not legacy URLs and are hard-404'd
        // immediately instead of triggering an API call.
        var hadHtmlExtension = Array.Exists(rawSegments, s => s.EndsWith(".html", StringComparison.OrdinalIgnoreCase));

        var normalizedSegments = rawSegments.Select(NormalizeSegment).ToArray();

        // Apply section rename redirects (e.g., /coding/* → /dotnet/*). Done after segment
        // normalization so that /coding/2025-01-01-slug.html → /dotnet/slug in one redirect.
        if (normalizedSegments.Length > 0 && _renamedSections.TryGetValue(normalizedSegments[0], out var newSectionName))
        {
            normalizedSegments[0] = newSectionName;
        }

        var normalizedPath = "/" + string.Join("/", normalizedSegments);
        // Compare against the original request path (not the trailing-slash-stripped `path`)
        // so that a slash-only change is also detected as a path change.
        var pathChanged = !string.Equals(normalizedPath, context.Request.Path.Value, StringComparison.Ordinal);

        // Multi-segment paths: validate section/collection against the cache, then redirect
        // or pass through. Validation runs on the NORMALIZED segments so that a redirect
        // never points at a URL that would 404 on the next request.
        if (normalizedSegments.Length > 1)
        {
            if (!IsValidMultiSegmentPath(normalizedSegments))
            {
                // Two-segment paths with a valid section but an unrecognised second segment
                // may be old Jekyll-style /{section}/{slug}.html URLs (without a collection
                // in the path). Try a legacy lookup before hard-404ing.
                if (normalizedSegments.Length == 2
                    && _sectionCache.IsReady
                    && _sectionCache.GetSectionByName(normalizedSegments[0]) != null
                    && IsLegacyLookupCandidate(normalizedSegments[1])
                    && hadHtmlExtension)
                {
                    var lookupHandled = await TryLegacyRedirectAsync(context, normalizedSegments[1], normalizedSegments[0]);
                    if (!lookupHandled)
                    {
                        // Transient API failure — if normalization already produced a cleaner URL
                        // (e.g. stripped .html/date/trailing slash or renamed the section), redirect
                        // there first so the browser retries the canonical-looking form once the API
                        // recovers. Otherwise there is no better fallback than 503 + no-store.
                        if (pathChanged)
                        {
                            Redirect(context, normalizedPath + context.Request.QueryString);
                        }
                        else
                        {
                            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                            context.Response.Headers.CacheControl = "no-store";
                        }
                    }

                    return;
                }

                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            if (pathChanged)
            {
                Redirect(context, normalizedPath + context.Request.QueryString);
            }
            else
            {
                await _next(context);
            }

            return;
        }

        // Single-segment path below.
        var segment = normalizedSegments[0];

        // Edge case: segment is empty after normalization (e.g. /.html) → redirect to root.
        if (string.IsNullOrEmpty(segment))
        {
            Redirect(context, "/" + context.Request.QueryString);
            return;
        }

        // When the section cache is not ready, skip all cache-dependent logic and legacy API
        // lookups. Apply any static normalization (.html / date prefix) that was already
        // computed, then pass through. The SectionCacheHealthCheck keeps this instance out
        // of the load balancer until the cache is populated, so this branch is only hit during
        // the brief window between startup and first API response (or a mid-deployment outage).
        if (!_sectionCache.IsReady)
        {
            if (pathChanged)
            {
                Redirect(context, normalizedPath + context.Request.QueryString);
            }
            else
            {
                await _next(context);
            }

            return;
        }

        // Not a legacy lookup candidate: redirect to clean URL if changed, else pass through.
        if (!IsLegacyLookupCandidate(segment))
        {
            if (pathChanged)
            {
                Redirect(context, normalizedPath + context.Request.QueryString);
            }
            else
            {
                await _next(context);
            }

            return;
        }

        // Legacy lookups are only triggered for URLs that originally contained .html —
        // the format used on the old website (e.g. /2024-01-15-my-article.html).
        // Bare slugs without .html are not legacy URLs; return 404 immediately without
        // an API call so that scanner noise and misspelled paths produce no external traffic.
        if (!hadHtmlExtension)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        // Legacy lookup: call the API with the already-normalized slug.
        // This avoids an intermediate redirect (e.g. /2026-01-12-article.html → /article → /ai/videos/article)
        // by going directly to the canonical URL in one step.
        var rawSection = context.Request.Query["section"].FirstOrDefault();
        var sectionHint = RouteParameterValidator.IsValidNameSegment(rawSection) ? rawSection : null;

        // Remap legacy section names so the API lookup is scoped to the current name.
        // E.g. ?section=coding in an old bookmark maps to ?section=dotnet for the API call.
        if (sectionHint != null && _renamedSections.TryGetValue(sectionHint, out var remappedHint))
        {
            sectionHint = remappedHint;
        }

        var handled = await TryLegacyRedirectAsync(context, segment, sectionHint);
        if (handled)
        {
            return;
        }

        // Transient API failure — return 503 so the error is not cached as a permanent absence.
        // Cache-Control: no-store prevents CDNs and browsers from storing the error response.
        // If the path changed (e.g. /article.html → /article), redirect to the clean URL first
        // so the browser retries the canonical form once the API recovers.
        if (pathChanged)
        {
            Redirect(context, normalizedPath + context.Request.QueryString);
            return;
        }

        context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
        context.Response.Headers.CacheControl = "no-store";
    }

    /// <summary>
    /// Calls the legacy-redirect API for <paramref name="slug"/> with an optional section hint.
    /// Returns <c>true</c> when the request has been fully handled (either redirected or 404'd).
    /// Returns <c>false</c> on a transient API failure so the caller can apply graceful fallback.
    /// </summary>
    private async Task<bool> TryLegacyRedirectAsync(HttpContext context, string slug, string? sectionHint)
    {
        try
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var apiClient = scope.ServiceProvider.GetRequiredService<ITechHubApiClient>();
            var result = await apiClient.GetLegacyRedirectAsync(slug, sectionHint, context.RequestAborted);

            if (result != null)
            {
                _logger.LogInformation("Legacy redirect: {Slug} ({Section}) -> {Url}", slug, sectionHint, result.Url);
                Redirect(context, result.Url + context.Request.QueryString);
                return true;
            }

            // Slug confirmed not in the DB — hard 404 so crawlers treat this as permanently absent.
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return true;
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            // Rethrow: avoid writing a redirect onto an already-aborted connection.
            throw;
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            _logger.LogWarning(ex, "Legacy lookup failed for {Slug} ({Section}); caller will apply fallback", slug, sectionHint);
            return false;
        }
    }

    /// <summary>
    /// Validates a multi-segment normalized path against the section/collection cache.
    /// Returns <c>false</c> (404) when:
    /// <list type="bullet">
    ///   <item>segment[0] is not a known section, known page, or framework prefix and the last segment has no file extension</item>
    ///   <item>segment[0] is a known section and segment[1] is not a known collection of that section</item>
    /// </list>
    /// Returns <c>true</c> (pass through) when the cache is not ready, or when the path
    /// starts with a framework/auth prefix, or when the last segment has a file extension
    /// (static assets like /css/article.css, /images/section-backgrounds/ai.jxl),
    /// or when the section and collection are both valid.
    /// Slug segments (segment[2+]) are not validated here — they are verified by the DB query.
    /// </summary>
    private bool IsValidMultiSegmentPath(string[] segments)
    {
        // Guard: if the cache is empty (API was down at startup), do not false-404.
        if (!_sectionCache.IsReady)
        {
            return true;
        }

        var first = segments[0];

        // Framework internals (/_blazor, /_framework, /_content) — always valid.
        if (first.StartsWith('_'))
        {
            return true;
        }

        // Microsoft Identity auth paths (/MicrosoftIdentity/Account/SignIn etc.)
        if (string.Equals(first, "MicrosoftIdentity", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        // Known non-section pages (about, admin, error, etc.) may have sub-paths.
        if (_knownNonSectionPages.Contains(first))
        {
            return true;
        }

        // Virtual sections (not in the API cache — e.g. /all which aggregates across all sections).
        // Valid second segments: "all" keyword, file extensions (feeds/sitemaps),
        // dedicated virtual sub-pages (authors), or any collection known in any real section.
        if (_knownVirtualSections.TryGetValue(first, out var virtualDedicatedPages))
        {
            if (segments.Length < 2)
            {
                return true;
            }

            var second = segments[1];

            // "all" keyword, file extensions, and dedicated virtual pages are always valid.
            if (string.Equals(second, "all", StringComparison.OrdinalIgnoreCase)
                || Path.HasExtension(second)
                || virtualDedicatedPages.Contains(second))
            {
                return true;
            }

            // Accept any collection that exists in at least one real section
            // (roundups, news, videos, blogs, etc.) — new collections are picked up automatically.
            // O(1) via the pre-built all-collections HashSet in SectionCache.
            return _sectionCache.IsKnownCollectionInAnySection(second);
        }

        // If the last segment of the path has a file extension, pass through without validation.
        // This covers static assets at any depth (e.g. /css/article.css, /images/section-backgrounds/ai.jxl)
        // and registered endpoints with extensions (e.g. /all/feed.xml, /security/feed.xml, /sitemap.xml).
        // UseStaticFiles or an endpoint handler downstream will serve or reject the request.
        if (Path.HasExtension(segments[^1]))
        {
            return true;
        }

        // Must be a known section — otherwise 404.
        var section = _sectionCache.GetSectionByName(first);
        if (section == null)
        {
            return false;
        }

        // If there is a second segment, it must either be the virtual "all" keyword
        // (/{sectionName}/all shows all content in that section and is handled by
        // SectionCollection.razor — it is not stored as a real API collection) or a
        // known collection of that section.
        if (segments.Length >= 2
            && !string.Equals(segments[1], "all", StringComparison.OrdinalIgnoreCase)
            && !_sectionCache.IsKnownCollection(section.Name, segments[1]))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Returns true when the segment should be looked up against the legacy slug API.
    /// Skips known non-section pages, known section names, and static file extensions.
    /// (.html has already been stripped before this check is reached.)
    /// </summary>
    private bool IsLegacyLookupCandidate(string segment)
    {
        // Note: the !_sectionCache.IsReady guard lives in InvokeAsync (before this call)
        // so that it can distinguish pathChanged vs. not-changed. Here we only check
        // structural properties of the segment itself.

        // Framework-internal paths (e.g. _blazor, _framework) are never content slugs.
        if (segment.StartsWith('_'))
        {
            return false;
        }

        if (_knownNonSectionPages.Contains(segment))
        {
            return false;
        }

        if (_knownVirtualSections.ContainsKey(segment))
        {
            return false;
        }

        if (_sectionCache.GetSectionByName(segment) != null)
        {
            return false;
        }

        // Skip static files (e.g. .css, .js, .png). At this point .html has already been stripped,
        // so any remaining extension is a genuine static file.
        if (Path.HasExtension(segment))
        {
            return false;
        }

        // Segments that can never be valid slugs (contain underscores, dots, special characters,
        // etc.) would only produce a 400 from the API. Skip the call.
        if (!RouteParameterValidator.IsValidSlug(segment))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Redirects legacy RSS feed URL patterns to their canonical equivalents:
    /// <list type="bullet">
    ///   <item><c>/feed.xml</c> → <c>/all/feed.xml</c></item>
    ///   <item><c>/{sectionName}.xml</c> → <c>/{sectionName}/feed.xml</c> (known section only)</item>
    /// </list>
    /// Unknown single-segment .xml paths (e.g. <c>/wordpress.xml</c>) return <c>false</c> and
    /// fall through to <see cref="InvalidRouteSegmentMiddleware"/> which rejects them as probes.
    /// </summary>
    private bool TryRedirectLegacyFeed(HttpContext context, string path)
    {
        // Single-segment .xml paths are candidates for legacy feed redirection.
        // Multi-segment paths like /all/feed.xml are already correct — leave them alone.
        // /sitemap.xml is also excluded: it is not an RSS feed and needs no redirect.
        if (!path.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var segment = path.TrimStart('/');
        if (segment.Contains('/', StringComparison.Ordinal))
        {
            return false;
        }

        // /feed.xml → /all/feed.xml (the canonical "everything" feed)
        if (segment.Equals("feed.xml", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogDebug("Legacy feed redirect: /feed.xml -> /all/feed.xml");
            Redirect(context, "/all/feed.xml" + context.Request.QueryString);
            return true;
        }

        // /sitemap.xml is not an RSS feed — never redirect it.
        // Guard explicitly so a section accidentally named "sitemap" can't cause
        // /sitemap.xml to redirect to /sitemap/feed.xml.
        if (segment.Equals("sitemap.xml", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // /{sectionName}.xml → /{sectionName}/feed.xml
        // If the section cache is not ready (API down at startup), pass through rather
        // than blocking legitimate feed URLs — consistent with the broader policy of
        // avoiding false-404s during cache warmup/unavailability.
        if (!_sectionCache.IsReady)
        {
            return false;
        }

        // Strip ".xml" (4 chars) to get the candidate section name.
        var nameWithoutXml = segment[..^4];
        var section = _sectionCache.GetSectionByName(nameWithoutXml);
        if (section != null)
        {
            var target = $"/{section.Name}/feed.xml";
            _logger.LogDebug("Legacy feed redirect: /{OldPath} -> {Target}", segment, target);
            Redirect(context, target + context.Request.QueryString);
            return true;
        }

        return false;
    }

    private static string NormalizeSegment(string segment)
    {
        // 1. Strip .html extension (case-insensitive)
        if (segment.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
        {
            segment = segment[..^5];
        }

        // 2. Strip YYYY-MM-DD- date prefix
        var match = DatePrefixPattern().Match(segment);
        if (match.Success)
        {
            segment = match.Groups[1].Value;
        }

        return segment;
    }

    private static void Redirect(HttpContext context, string url)
    {
        context.Response.StatusCode = StatusCodes.Status301MovedPermanently;
        context.Response.Headers.Location = url;
    }
}

/// <summary>
/// Extension methods for registering UrlNormalizationMiddleware.
/// </summary>
public static class UrlNormalizationMiddlewareExtensions
{
    public static IApplicationBuilder UseUrlNormalization(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UrlNormalizationMiddleware>();
    }
}
