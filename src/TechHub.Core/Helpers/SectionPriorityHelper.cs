namespace TechHub.Core.Helpers;

/// <summary>
/// Helper class to determine the primary section for content items based on categories
/// and the section order in the top menu bar
/// </summary>
public static class SectionPriorityHelper
{
    /// <summary>
    /// Section priority order (matches the menubar order)
    /// </summary>
    private static readonly string[] SectionPriorityOrder =
    [
        "GitHub Copilot",
        "AI",
        "ML",
        "Coding",
        "Azure",
        "DevOps",
        "Security"
    ];

    /// <summary>
    /// Section name mappings to URL paths
    /// </summary>
    private static readonly Dictionary<string, string> SectionNameToUrl = new()
    {
        { "GitHub Copilot", "github-copilot" },
        { "AI", "ai" },
        { "ML", "ml" },
        { "Coding", "coding" },
        { "Azure", "azure" },
        { "DevOps", "devops" },
        { "Security", "security" }
    };

    /// <summary>
    /// Determines the primary section URL for a content item based on its categories and collection
    /// </summary>
    /// <param name="categories">List of categories from the content item</param>
    /// <param name="collectionName">Optional collection name (e.g., "roundups")</param>
    /// <returns>The URL of the primary section (e.g., "github-copilot", "ai"), or "all" if no match</returns>
    public static string GetPrimarySectionUrl(IReadOnlyList<string> categories, string? collectionName = null)
    {
        ArgumentNullException.ThrowIfNull(categories);
        
        // Special case: Roundups always belong to "all" section
        if (collectionName?.Equals("roundups", StringComparison.OrdinalIgnoreCase) == true)
            return "all";
        
        if (categories.Count == 0)
            return "all";

        // Find the first category that matches a section in priority order
        foreach (var sectionName in SectionPriorityOrder)
        {
            if (categories.Contains(sectionName, StringComparer.OrdinalIgnoreCase))
            {
                return SectionNameToUrl[sectionName];
            }
        }

        // No match found, default to "all"
        return "all";
    }

    /// <summary>
    /// Determines the primary section name (display name) for a content item based on its categories and collection
    /// </summary>
    /// <param name="categories">List of categories from the content item</param>
    /// <param name="collectionName">Optional collection name (e.g., "roundups")</param>
    /// <returns>The display name of the primary section (e.g., "GitHub Copilot", "AI"), or "All" if no match</returns>
    public static string GetPrimarySectionName(IReadOnlyList<string> categories, string? collectionName = null)
    {
        ArgumentNullException.ThrowIfNull(categories);
        
        // Special case: Roundups always belong to "All" section
        if (collectionName?.Equals("roundups", StringComparison.OrdinalIgnoreCase) == true)
            return "All";
        
        if (categories.Count == 0)
            return "All";

        // Find the first category that matches a section in priority order
        foreach (var sectionName in SectionPriorityOrder)
        {
            if (categories.Contains(sectionName, StringComparer.OrdinalIgnoreCase))
            {
                return sectionName;
            }
        }

        // No match found, default to "All"
        return "All";
    }
}
