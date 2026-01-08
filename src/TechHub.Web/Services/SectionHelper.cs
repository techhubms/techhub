using TechHub.Core.DTOs;

namespace TechHub.Web.Services;

/// <summary>
/// Helper service for section and collection display name mappings.
/// Provides fallback display names when section data isn't available.
/// </summary>
internal static class SectionHelper
{
    /// <summary>
    /// Gets the display title for a section URL.
    /// Prefer using section.Title from SectionDto when available.
    /// This is only a fallback for when section data isn't loaded yet.
    /// </summary>
    public static string GetSectionDisplayName(string sectionUrl)
    {
        return sectionUrl?.ToLowerInvariant() switch
        {
            "github-copilot" => "GitHub Copilot",
            "ai" => "Artificial Intelligence",
            "ml" => "Machine Learning",
            "coding" => ".NET",
            "azure" => "Azure",
            "devops" => "DevOps",
            "security" => "Security",
            "all" => "All",
            _ => System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(sectionUrl?.ToLowerInvariant() ?? "")
        };
    }

    /// <summary>
    /// Gets the section URL from a category/section name.
    /// </summary>
    public static string GetSectionUrl(string sectionName)
    {
        return sectionName?.ToLowerInvariant() switch
        {
            "github copilot" => "github-copilot",
            "artificial intelligence" or "ai" => "ai",
            "machine learning" or "ml" => "ml",
            "coding" or ".net" => "coding",
            "azure" => "azure",
            "devops" => "devops",
            "security" => "security",
            "all" => "all",
            _ => sectionName?.ToLowerInvariant().Replace(" ", "-", StringComparison.Ordinal) ?? ""
        };
    }

    /// <summary>
    /// Gets the display name for a collection.
    /// </summary>
    public static string GetCollectionDisplayName(string collectionName)
    {
        if (string.IsNullOrWhiteSpace(collectionName))
            return collectionName ?? string.Empty;
        
        return System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(collectionName.ToLowerInvariant());
    }
}
