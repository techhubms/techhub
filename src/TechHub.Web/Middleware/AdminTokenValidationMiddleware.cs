using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;

namespace TechHub.Web.Middleware;

/// <summary>
/// Middleware that validates the MSAL token cache for authenticated users accessing
/// <c>/admin</c> routes. When the user has a valid authentication cookie but the
/// in-memory MSAL token cache is empty (e.g. after a server restart), this middleware
/// issues an OIDC challenge to re-authenticate the user at the HTTP level — before
/// Blazor starts rendering. This prevents circuit crashes and console errors.
/// </summary>
public sealed class AdminTokenValidationMiddleware
{
    private readonly RequestDelegate _next;

    public AdminTokenValidationMiddleware(RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(next);
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        // Only check admin pages, not static files, _blazor, or API calls
        var path = context.Request.Path.Value;
        if (path == null
            || !path.StartsWith("/admin", StringComparison.OrdinalIgnoreCase)
            || path.StartsWith("/admin/login", StringComparison.OrdinalIgnoreCase)
            || path.StartsWith("/admin/logout", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        // Only validate for authenticated users — unauthenticated users will be handled
        // by [Authorize] on the page components, which triggers the OIDC challenge.
        if (context.User?.Identity?.IsAuthenticated != true)
        {
            await _next(context);
            return;
        }

        // Try to acquire a token from the MSAL cache. If it fails, the cache is empty
        // and we need to re-authenticate the user.
        var tokenAcquisition = context.RequestServices.GetService<ITokenAcquisition>();
        var configuration = context.RequestServices.GetRequiredService<IConfiguration>();
        var scopes = configuration.GetValue<string>("AzureAd:Scopes")
            ?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? [];

        if (tokenAcquisition == null || scopes.Length == 0)
        {
            // Azure AD not configured (local dev) — skip validation
            await _next(context);
            return;
        }

        try
        {
            // This will succeed if the token is in the cache, or fail if the cache was cleared
            await tokenAcquisition.GetAccessTokenForUserAsync(scopes);
        }
        catch (MsalUiRequiredException)
        {
            await ChallengeAsync(context);
            return;
        }
        catch (MicrosoftIdentityWebChallengeUserException)
        {
            await ChallengeAsync(context);
            return;
        }

        await _next(context);
    }

    private static Task ChallengeAsync(HttpContext context)
    {
        // Issue an OIDC challenge — this triggers a redirect to the Microsoft login page.
        // After sign-in, the user is redirected back to the original admin URL.
        return context.ChallengeAsync(
            OpenIdConnectDefaults.AuthenticationScheme,
            new AuthenticationProperties { RedirectUri = context.Request.Path + context.Request.QueryString });
    }
}

/// <summary>
/// Extension method to register the middleware.
/// </summary>
public static class AdminTokenValidationMiddlewareExtensions
{
    public static IApplicationBuilder UseAdminTokenValidation(this IApplicationBuilder app)
    {
        return app.UseMiddleware<AdminTokenValidationMiddleware>();
    }
}
