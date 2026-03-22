namespace TechHub.Core.Configuration;

/// <summary>
/// Configuration for the admin section.
/// Credentials are intentionally simple (username/password from configuration) to bootstrap
/// the admin area quickly. The architecture is designed to be replaceable with IdentityServer
/// when external user account support is required in the future.
/// </summary>
public class AdminOptions
{
    /// <summary>Configuration section name.</summary>
    public const string SectionName = "Admin";

    /// <summary>Admin username. Change from default before deploying to production.</summary>
    public string Username { get; init; } = "admin";

    /// <summary>Admin password. Must be changed from default before deploying to production.</summary>
    public string Password { get; init; } = string.Empty;
}
