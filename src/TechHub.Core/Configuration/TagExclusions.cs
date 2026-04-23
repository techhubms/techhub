namespace TechHub.Core.Configuration;

/// <summary>
/// Centralized list of generic/noise tags excluded from tag clouds.
/// These tags appear frequently but don't provide useful filtering value.
/// Used by both the API (ContentRepository) and Web (ContentItem page).
/// </summary>
public static class TagExclusions
{
    /// <summary>
    /// Tags that are too generic or too frequent to be useful in tag clouds.
    /// Structural tags (section tags, collection names) are excluded separately
    /// based on configuration data.
    /// </summary>
    public static readonly string[] GenericTags =
    [
        "github",
        "copilot",
        "microsoft",
        "code",
        "developer",
        "management",
        "company",
        "engineering",
        "improvement",
        "automation",
        "vs",
        "agent",
        "agents",
        "chat",
        "software",
        "integration",
        "development",
        "vs code",
        "visual studio",
        "visual studio code",
        "actions",
        "repository",
        "repositories",
        "data"
    ];

    /// <summary>
    /// Builds the full set of tags to exclude from tag clouds.
    /// Combines generic tags with section tags and collection-name tags.
    /// </summary>
    /// <param name="sectionTags">Section display tags (e.g. "AI", "Azure", ".NET").</param>
    /// <param name="collectionTags">Collection-derived tags (e.g. "Blogs", "News", "Videos").</param>
    public static HashSet<string> BuildExcludeSet(
        IEnumerable<string> sectionTags,
        IEnumerable<string> collectionTags)
    {
        ArgumentNullException.ThrowIfNull(sectionTags);
        ArgumentNullException.ThrowIfNull(collectionTags);
        var excludeSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var tag in sectionTags)
        {
            if (!string.IsNullOrWhiteSpace(tag))
            {
                excludeSet.Add(tag);
            }
        }

        foreach (var tag in collectionTags)
        {
            if (!string.IsNullOrWhiteSpace(tag))
            {
                excludeSet.Add(tag);
            }
        }

        foreach (var tag in GenericTags)
        {
            excludeSet.Add(tag);
        }

        return excludeSet;
    }
}
