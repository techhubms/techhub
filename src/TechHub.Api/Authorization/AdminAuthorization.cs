using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;

namespace TechHub.Api.Authorization;

/// <summary>
/// Configures the "AdminOnly" authorization policy used by all admin endpoints.
/// </summary>
public static class AdminAuthorization
{
    public const string PolicyName = "AdminOnly";

    // The scope is part of the app registration's public contract (defined by Manage-EntraId.ps1).
    // Entra ID places the short scope name (without the "api://<client-id>/" prefix) in the "scp" claim.
    public const string RequiredScope = "Admin.Access";

    /// <summary>
    /// Anonymous admin access is a local/test convenience. It is only allowed when Azure AD is not
    /// configured AND the app is not running in Production, so a lost setting can never open admin endpoints.
    /// </summary>
    public static bool AllowsAnonymousAdmin(string? azureAdClientId, IHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(environment);
        return string.IsNullOrEmpty(azureAdClientId) && !environment.IsProduction();
    }

    public static void Configure(AuthorizationPolicyBuilder policy, string? azureAdClientId, IHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(policy);

        if (!string.IsNullOrEmpty(azureAdClientId))
        {
            policy.RequireAuthenticatedUser();
            policy.RequireScope(RequiredScope);
        }
        else if (AllowsAnonymousAdmin(azureAdClientId, environment))
        {
            policy.RequireAssertion(_ => true);
        }
        else
        {
            policy.RequireAssertion(_ => false);
        }
    }
}
