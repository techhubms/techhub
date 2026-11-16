using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Microsoft.Playwright;

namespace TechHub.E2E.Tests.Helpers;

/// <summary>
/// Keeps the E2E suite clear of the Web app's per-IP rate limiter.
///
/// Every test browser runs from the same IP, so without help the suite queues behind the limiter and pages take up to a
/// minute to become interactive. The deployed sites exempt requests that carry the secret in <c>E2E_BYPASS_TOKEN</c>.
/// It is sent as a cookie scoped to the site rather than as an extra HTTP header, because Playwright sends extra headers to
/// every origin (the CDNs the pages load scripts from) and a cookie also covers the SignalR negotiate and WebSocket requests.
///
/// A 429 means the token is missing or wrong, so the guard also records them and fails the test with that explanation
/// instead of letting it time out later with a generic message.
/// </summary>
internal static class RateLimitGuard
{
    private const string CookieName = "e2e-token";

    private static readonly ConditionalWeakTable<IBrowserContext, ConcurrentQueue<string>> _rateLimitedUrls = [];

    private static string Token => Environment.GetEnvironmentVariable("E2E_BYPASS_TOKEN") ?? string.Empty;

    /// <summary>Sends the bypass token (when configured) and starts recording 429 responses for the context.</summary>
    public static async Task ApplyAsync(IBrowserContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var urls = _rateLimitedUrls.GetOrCreateValue(context);
        context.Response += (_, response) =>
        {
            if (response.Status == 429 && response.Url.StartsWith(BlazorHelpers.BaseUrl, StringComparison.OrdinalIgnoreCase))
            {
                urls.Enqueue(response.Url);
            }
        };

        if (Token.Length == 0)
        {
            return;
        }

        await context.AddCookiesAsync(
        [
            new Cookie
            {
                Name = CookieName,
                Value = Token,
                Url = BlazorHelpers.BaseUrl,
                HttpOnly = true,
                Secure = BlazorHelpers.BaseUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase),
            },
        ]);
    }

    /// <summary>Throws when the Web app answered any request of the context with 429 Too Many Requests.</summary>
    public static void ThrowIfRateLimited(IBrowserContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (!_rateLimitedUrls.TryGetValue(context, out var urls) || urls.IsEmpty)
        {
            return;
        }

        var tokenState = Token.Length == 0
            ? "E2E_BYPASS_TOKEN is not set"
            : "E2E_BYPASS_TOKEN is set, so the site probably has no (or a different) token configured";
        throw new InvalidOperationException(
            $"The site rate-limited the test browser (429 Too Many Requests): {tokenState}. First rejected request: {urls.First()}");
    }
}
