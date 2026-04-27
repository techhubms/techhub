namespace TechHub.Core.Models;

/// <summary>
/// Result of a legacy slug lookup. Contains the canonical URL to redirect to.
/// For items in externally-linking collections (news, blogs, community) this is the
/// original source URL. For all other items it is the internal path /{section}/{collection}/{slug}.
/// </summary>
public sealed record LegacyRedirectResult(string Url);
