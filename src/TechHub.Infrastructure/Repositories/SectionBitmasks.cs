namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// The bit of each section in <c>content_items.sections_bitmask</c>. An article is in a section when its bit is set.
/// </summary>
internal static class SectionBitmasks
{
    /// <summary>All section bits set: an article that is in any section.</summary>
    public const int AllSections = 1 | 2 | 4 | 8 | 16 | 32 | 64;

    /// <summary>The bit of a section (AI = 1, Azure = 2, .NET = 4, DevOps = 8, GitHub Copilot = 16, ML = 32, Security = 64), or 0 for an unknown section.</summary>
    public static int For(string sectionName)
    {
        ArgumentNullException.ThrowIfNull(sectionName);

        return sectionName.ToLowerInvariant().Trim() switch
        {
            "ai" => 1,
            "azure" => 2,
            "dotnet" => 4,
            "devops" => 8,
            "github-copilot" => 16,
            "ml" => 32,
            "security" => 64,
            _ => 0
        };
    }
}
