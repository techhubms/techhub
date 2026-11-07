namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// The bit of each section in <c>content_items.sections_bitmask</c>. An article is in a section when its bit is set.
/// </summary>
internal static class SectionBitmasks
{
    public const int Ai = 1;
    public const int Azure = 2;
    public const int Dotnet = 4;
    public const int Devops = 8;
    public const int GithubCopilot = 16;
    public const int Ml = 32;
    public const int Security = 64;

    /// <summary>All section bits set: an article that is in any section.</summary>
    public const int AllSections = Ai | Azure | Dotnet | Devops | GithubCopilot | Ml | Security;

    /// <summary>Every section with its bit.</summary>
    public static IReadOnlyList<(string Name, int Bit)> All { get; } =
    [
        ("ai", Ai),
        ("azure", Azure),
        ("dotnet", Dotnet),
        ("devops", Devops),
        ("github-copilot", GithubCopilot),
        ("ml", Ml),
        ("security", Security)
    ];

    /// <summary>The bit of a section, or 0 for an unknown section.</summary>
    public static int For(string sectionName)
    {
        ArgumentNullException.ThrowIfNull(sectionName);

        var normalized = sectionName.ToLowerInvariant().Trim();
        foreach (var (name, bit) in All)
        {
            if (name == normalized)
            {
                return bit;
            }
        }

        return 0;
    }

    /// <summary>The bits of all the given sections together; unknown sections add nothing.</summary>
    public static int For(IEnumerable<string> sectionNames)
    {
        ArgumentNullException.ThrowIfNull(sectionNames);

        var bitmask = 0;
        foreach (var sectionName in sectionNames)
        {
            bitmask |= For(sectionName);
        }

        return bitmask;
    }

    public static bool Has(int bitmask, int bit) => (bitmask & bit) != 0;
}
