namespace TechHub.TestUtilities;

/// <summary>
/// Constants defining expected counts in the TestCollections test data.
/// These values are hardcoded based on the actual markdown files in TestCollections directory.
/// Update these when test data changes.
/// </summary>
/// <remarks>
/// Last updated: 2026-05-19 after adding legacy 'all' primary_section roundup for ExcludeAllPrimarySection filter test.
/// 
/// Test data composition:
/// - 44 published items total (was 43, added 1 legacy 'all' roundup for section filter test)
/// - Collections: blogs (22), news (7), videos (10), community (2), roundups (3)
/// - Tag distributions: AI (12 items), DevOps (1 item), GitHub Copilot (4 tag symmetry test items)
/// - Date ranges: 2023 items (1), 2024 items (22), 2025 items (10), 2026 items (11)
/// </remarks>
public static class TestDataConstants
{
    // Total counts
    public const int TotalPublishedItems = 44;
    public const int TotalItems = TotalPublishedItems;

    // Collection counts (published only)
    public const int BlogsCount = 22;
    public const int NewsCount = 7;
    public const int VideosCount = 10;  // Includes all video items (2 root + 6 ghc-features + 2 vscode-updates)
    public const int CommunityCount = 2;
    public const int RoundupsCount = 3;

    // Tag counts (published items only)
    // Word-based counting: tags are split by space, dash, underscore
    // Example: "GitHub Copilot" contains words "github" and "copilot"
    public const int AiTagCount = 27;  // Items with "ai" as a word in any tag after ContentFixer normalization (e.g., "AI", "Agentic AI", "AI Development", "GitHub Copilot"). Excludes related1/related2/related3 which use unique test tags.
    public const int DevOpsTagCount = 1;

    // Date range counts (published items only)
    public const int Items2023Count = 1;
    public const int Items2024Count = 22;
    public const int Items2025Count = 10;
    public const int Items2026Count = 11;

    // Facet-specific counts (context-dependent based on filtering)
    /// <summary>AI tag appears in facets with this count when no filters applied</summary>
    public const int AiFacetCountUnfiltered = 27;  // Facet shows word "ai" appearing in this many items after ContentFixer normalization

    /// <summary>Expected total count when filtering by AI tag (word-based matching)</summary>
    public const int FilteredByAiTotalCount = 27;  // Items containing "ai" as a word in any tag after ContentFixer normalization
}
