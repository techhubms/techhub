namespace TechHub.Core.Helpers;

/// <summary>
/// Helper class to determine the primary section for content items based on lowercase section names
/// and the section order in the top menu bar.
/// ContentItem.SectionNames contains lowercase section names (e.g., "ai", "github-copilot"), NOT display titles.
/// </summary>
public static class SectionPriorityHelper
{
    /// <summary>
    /// Section priority order (matches the menubar order).
    /// These are lowercase section NAMES matching Section.Name and ContentItem.SectionNames.
    /// </summary>
    private static readonly string[] _sectionPriorityOrder =
    [
        "github-copilot",
        "ai",
        "ml",
        "coding",
        "azure",
        "devops",
        "security"
    ];

    /// <summary>
    /// Section name to display title mappings.
    /// Maps from Section.Name (lowercase) to Section.Title (display name).
    /// </summary>
    private static readonly Dictionary<string, string> _sectionNameToTitle = new()
    {
        { "github-copilot", "GitHub Copilot" },
        { "ai", "AI" },
        { "ml", "ML" },
        { "coding", "Coding" },
        { "azure", "Azure" },
        { "devops", "DevOps" },
        { "security", "Security" }
    };

    /// <summary>
    /// Determines the primary section URL for a content item based on its lowercase section names and collection
    /// </summary>
    /// <param name="sectionNames">List of lowercase section names from ContentItem.SectionNames (e.g., "ai", "github-copilot")</param>
    /// <param name="collectionName">Optional collection name (e.g., "roundups")</param>
    /// <returns>The URL of the primary section (e.g., "github-copilot", "ai"), or "all" if no match</returns>
    public static string GetPrimarySectionUrl(IReadOnlyList<string> sectionNames, string? collectionName = null)
    {
        ArgumentNullException.ThrowIfNull(sectionNames);

        // Special case: Roundups always belong to "all" section
        if (collectionName?.Equals("roundups", StringComparison.OrdinalIgnoreCase) == true)
            return "all";

        if (sectionNames.Count == 0)
            return "all";

        // Find the first section that matches in priority order
        foreach (var sectionName in _sectionPriorityOrder)
        {
            if (sectionNames.Contains(sectionName, StringComparer.OrdinalIgnoreCase))
            {
                return sectionName; // Return the section name directly (already lowercase URL)
            }
        }

        // No match found, default to "all"
        return "all";
    }

    /// <summary>
    /// Determines the primary section name for a content item based on its lowercase section names and collection
    /// </summary>
    /// <param name="sectionNames">List of lowercase section names from ContentItem.SectionNames (e.g., "ai", "github-copilot")</param>
    /// <param name="collectionName">Optional collection name (e.g., "roundups")</param>
    /// <returns>The lowercase name of the primary section (e.g., "github-copilot", "ai"), or "all" if no match</returns>
    public static string GetPrimarySectionName(IReadOnlyList<string> sectionNames, string? collectionName = null)
    {
        ArgumentNullException.ThrowIfNull(sectionNames);

        // Special case: Roundups always belong to "all" section
        if (collectionName?.Equals("roundups", StringComparison.OrdinalIgnoreCase) == true)
            return "all";

        if (sectionNames.Count == 0)
            return "all";

        // Find the first section that matches in priority order
        foreach (var sectionName in _sectionPriorityOrder)
        {
            if (sectionNames.Contains(sectionName, StringComparer.OrdinalIgnoreCase))
            {
                return sectionName; // Return lowercase name
            }
        }

        // No match found, default to "all"
        return "all";
    }
}
