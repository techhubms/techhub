using System.Security.Cryptography;
using System.Text;

namespace TechHub.Web.RateLimiting;

/// <summary>
/// Recognises requests from the E2E test suite. The token travels in a header (HTTP clients) or a cookie (browsers:
/// a cookie reaches the page, the SignalR negotiate request and the WebSocket upgrade, and unlike an extra header it is
/// never sent to the CDNs the page loads scripts from).
/// </summary>
internal static class E2ERateLimitBypass
{
    public const string HeaderName = "X-E2E-Token";
    public const string CookieName = "e2e-token";

    private const string UnresolvedKeyVaultReferencePrefix = "@Microsoft.KeyVault(";

    public static bool Matches(HttpContext context, string expectedToken)
    {
        ArgumentNullException.ThrowIfNull(context);

        // App Service passes a Key Vault reference through as literal text when it cannot resolve it. That text is
        // public (it is in the Bicep), so it must never count as a token.
        if (string.IsNullOrEmpty(expectedToken) || expectedToken.StartsWith(UnresolvedKeyVaultReferencePrefix, StringComparison.Ordinal))
        {
            return false;
        }

        string? presented = context.Request.Headers[HeaderName];
        if (string.IsNullOrEmpty(presented))
        {
            presented = context.Request.Cookies[CookieName];
        }

        if (string.IsNullOrEmpty(presented))
        {
            return false;
        }

        // Constant-time comparison so the token cannot be recovered through response timing.
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(presented),
            Encoding.UTF8.GetBytes(expectedToken));
    }
}
