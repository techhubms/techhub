using System.Text.RegularExpressions;
using TechHub.Core.Models;
using TechHub.Web.Services;

namespace TechHub.Web.Middleware;

/// <summary>
/// Unified URL normalization middleware that handles all URL cleanup and legacy redirects
/// in a single pass, ensuring at most one 301 redirect per request.
///
/// Normalizations applied to every path segment (in order):
///   1. Strip .html extension
///   2. Strip YYYY-MM-DD- date prefix
///
/// After normalization:
///   - Multi-segment paths that changed → 301 to the cleaned path.
///   - Single-segment paths that are not a known section, page, or static file →
///     API legacy lookup to resolve the canonical /{section}/{collection}/{slug} URL.
///     If the API returns a result, redirect there directly (one redirect to the final URL).
///     If not, redirect to the normalized path when the URL changed, or pass through.
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

        // Normalize each segment: strip .html extension, strip YYYY-MM-DD- date prefix.
        var rawSegments = path.TrimStart('/').Split('/');
        var normalizedSegments = rawSegments.Select(NormalizeSegment).ToArray();
        var normalizedPath = "/" + string.Join("/", normalizedSegments);
        var pathChanged = !string.Equals(normalizedPath, path, StringComparison.Ordinal);

        // Multi-segment paths: redirect to cleaned path if anything changed, else pass through.
        if (normalizedSegments.Length > 1)
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
        var sectionHint = context.Request.Query["section"].FirstOrDefault();

        LegacyRedirectResult? result = null;
        try
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var apiClient = scope.ServiceProvider.GetRequiredService<ITechHubApiClient>();
            result = await apiClient.GetLegacyRedirectAsync(segment, sectionHint, context.RequestAborted);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            // Rethrow when the client disconnected — avoid writing a redirect onto an aborted connection.
            throw;
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or OperationCanceledException)
        {
            _logger.LogWarning(ex, "Legacy slug lookup failed for {Slug}; falling back to normalized path", segment);

            // Graceful degradation: still clean up .html / date prefix even when API is unavailable.
            if (pathChanged)
            {
                Redirect(context, normalizedPath + context.Request.QueryString);
                return;
            }

            await _next(context);
            return;
        }

        if (result != null)
        {
            _logger.LogInformation("Legacy slug redirect: /{Segment} -> {Url}", segment, result.Url);
            Redirect(context, result.Url + context.Request.QueryString);
            return;
        }

        // Slug not found in DB: redirect to cleaned URL if the path changed, otherwise pass through.
        // Passing through lets Blazor routing render /not-found.
        if (pathChanged)
        {
            Redirect(context, normalizedPath + context.Request.QueryString);
            return;
        }

        await _next(context);
    }

    /// <summary>
    /// Returns true when the segment should be looked up against the legacy slug API.
    /// Skips known non-section pages, known section names, and static file extensions.
    /// (.html has already been stripped before this check is reached.)
    /// </summary>
    private bool IsLegacyLookupCandidate(string segment)
    {
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

        return true;
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
