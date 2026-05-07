namespace TechHub.Web.Middleware;

/// <summary>
/// Middleware that adds security headers to all responses.
/// Provides defense-in-depth against XSS, clickjacking, MIME sniffing, and other attacks.
/// </summary>
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(next);

        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.Response.OnStarting(() =>
        {
            var headers = context.Response.Headers;

            // Prevent MIME type sniffing
            headers["X-Content-Type-Options"] = "nosniff";

            // Prevent clickjacking
            headers["X-Frame-Options"] = "DENY";

            // Control referrer information
            headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

            // Opt out of FLoC/Topics API tracking
            headers["Permissions-Policy"] = "interest-cohort=()";

            // Content Security Policy (report-only mode to avoid breaking changes while validating the policy)
            // Blazor Server requires 'unsafe-inline' for scoped CSS and 'unsafe-eval' is not needed.
            // External scripts: Google Analytics/Tag Manager and Application Insights.
            headers["Content-Security-Policy-Report-Only"] =
                "default-src 'self'; " +
                "script-src 'self' 'unsafe-inline' https://*.googletagmanager.com https://*.google-analytics.com https://js.monitor.azure.com; " +
                "style-src 'self' 'unsafe-inline'; " +
                "img-src 'self' data: https://*.google-analytics.com https://*.googletagmanager.com; " +
                "connect-src 'self' https://*.google-analytics.com https://*.analytics.google.com https://*.googletagmanager.com https://*.applicationinsights.azure.com https://dc.services.visualstudio.com; " +
                "font-src 'self'; " +
                "frame-ancestors 'none'; " +
                "base-uri 'self'; " +
                "form-action 'self'";

            return Task.CompletedTask;
        });

        await _next(context);
    }
}

public static class SecurityHeadersMiddlewareExtensions
{
    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SecurityHeadersMiddleware>();
    }
}
