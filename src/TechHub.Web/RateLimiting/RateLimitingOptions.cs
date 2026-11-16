namespace TechHub.Web.RateLimiting;

/// <summary>
/// Settings for the per-IP rate limiting, bound from the "RateLimiting" configuration section.
/// </summary>
internal sealed class RateLimitingOptions
{
    public const string ConfigurationKey = "RateLimiting";

    /// <summary>
    /// Shared secret that lets the E2E test suite skip the per-IP limits: every E2E browser shares one runner IP and
    /// would otherwise queue behind the limiter. Empty (the default) disables the bypass.
    /// </summary>
    public string E2EBypassToken { get; init; } = string.Empty;
}
