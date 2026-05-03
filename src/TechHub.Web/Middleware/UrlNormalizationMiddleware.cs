using System.Text.RegularExpressions;
using TechHub.Core.Validation;
using TechHub.Web.Services;

namespace TechHub.Web.Middleware;

/// <summary>
/// Unified URL normalization middleware that handles URL cleanup, legacy redirects,
/// and early structural validation — all in a single pass with at most one 301 per request.
///
/// Normalizations applied to every path segment (in order):
///   1. Strip .html extension
///   2. Strip YYYY-MM-DD- date prefix
///
/// After normalization:
///   - Multi-segment paths: validate segment[0] against known sections/pages and segment[1]
///     against the section's collections. Unknown section → 404. Unknown collection → 404.
///     Validation is skipped when the section cache is not ready (API down at startup).
///     If the path changed (normalization applied) and is valid → 301 to the cleaned path.
///   - Single-segment paths that are not a known section, page, or static file →
///     API legacy lookup to resolve the canonical /{section}/{collection}/{slug} URL.
///     If the API returns a result, redirect there directly (one redirect to the final URL).
///     If not (null result or transient error after retries), return 404.
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
        "all",
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

        // Legacy RSS feed redirects: handled before segment normalization so that
        // /{sectionName}.xml paths are caught here rather than rejected as probes
        // by InvalidRouteSegmentMiddleware further down the pipeline.
        if (TryRedirectLegacyFeed(context, path))
        {
            return;
        }

        // Normalize each segment: strip .html extension, strip YYYY-MM-DD- date prefix.
        var rawSegments = path.TrimStart('/').Split('/');
        var normalizedSegments = rawSegments.Select(NormalizeSegment).ToArray();
        var normalizedPath = "/" + string.Join("/", normalizedSegments);
        var pathChanged = !string.Equals(normalizedPath, path, StringComparison.Ordinal);

        // Multi-segment paths: validate section/collection against the cache, then redirect
        // or pass through. Validation runs on the NORMALIZED segments so that a redirect
        // never points at a URL that would 404 on the next request.
        if (normalizedSegments.Length > 1)
        {
            if (!IsValidMultiSegmentPath(normalizedSegments))
            {
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

        // Legacy lookup: call the API with the already-normalized slug.
        // This avoids an intermediate redirect (e.g. /2026-01-12-article → /article → /ai/videos/article)
        // by going directly to the canonical URL in one step.
        var rawSection = context.Request.Query["section"].FirstOrDefault();
        var sectionHint = RouteParameterValidator.IsValidNameSegment(rawSection) ? rawSection : null;

        try
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var apiClient = scope.ServiceProvider.GetRequiredService<ITechHubApiClient>();
            var result = await apiClient.GetLegacyRedirectAsync(segment, sectionHint, context.RequestAborted);

            if (result != null)
            {
                _logger.LogInformation("Legacy slug redirect: /{Segment} -> {Url}", segment, result.Url);
                Redirect(context, result.Url + context.Request.QueryString);
                return;
            }

            // Slug confirmed not in the DB — return a hard 404 so crawlers and caches treat this
            // as a permanent absence, not a transient failure.
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            // Rethrow when the client disconnected — avoid writing a redirect onto an aborted connection.
            throw;
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            // Transient API failure (network error or timeout after all retries).
            // Graceful degradation: still clean up .html / date prefix even when the API is unavailable.
            // Do NOT return 404 here — a legitimate legacy URL would get a cacheable permanent 404
            // just because the API was briefly down.
            _logger.LogWarning(ex, "Legacy slug lookup failed for {Slug}; falling back to normalized path", segment);
        }

        // API was transiently unavailable — clean up the URL if the path changed, then pass through.
        if (pathChanged)
        {
            Redirect(context, normalizedPath + context.Request.QueryString);
            return;
        }

        await _next(context);
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

        // If there is a second segment, it must be a known collection of that section.
        if (segments.Length >= 2 && !_sectionCache.IsKnownCollection(section.Name, segments[1]))
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
        // Framework-internal paths (e.g. _blazor, _framework) are never content slugs.
        if (segment.StartsWith('_'))
        {
            return false;
        }

        if (_knownNonSectionPages.Contains(segment))
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

        // /{sectionName}.xml → /{sectionName}/feed.xml
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
