namespace TechHub.TestUtilities;

/// <summary>
/// Constants defining expected counts in the TestCollections test data.
/// These values are hardcoded based on the actual markdown files in TestCollections directory.
/// Update these when test data changes.
/// </summary>
/// <remarks>
/// Last updated: 2026-04-28 after adding vscode-updates FTS test video.
/// 
/// Test data composition:
/// - 41 published items total (was 40, added 1 vscode-updates video for FTS testing)
/// - 4 draft items (not counted in published totals)
/// - Collections: blogs (22), news (7), videos (9), community (2), roundups (1)
/// - Tag distributions: AI (12 items), DevOps (1 item), GitHub Copilot (4 tag symmetry test items)
/// - Date ranges: 2024 items (22), 2026 items (19)
/// </remarks>
public static class TestDataConstants
{
    // Total counts
    public const int TotalPublishedItems = 41;
    public const int TotalDraftItems = 4;
    public const int TotalItems = TotalPublishedItems + TotalDraftItems;

    // Collection counts (published only)
    public const int BlogsCount = 22;
    public const int NewsCount = 7;
    public const int VideosCount = 9;  // Includes root videos + subcollections (2 root + 5 ghc-features + 2 vscode-updates)
    public const int CommunityCount = 2;
    public const int RoundupsCount = 1;

    // Tag counts (published items only)
    // Word-based counting: tags are split by space, dash, underscore
    // Example: "GitHub Copilot" contains words "github" and "copilot"
    public const int AiTagCount = 26;  // Items with "ai" as a word in any tag after ContentFixer normalization (e.g., "AI", "Agentic AI", "AI Development", "GitHub Copilot"). Excludes related1/related2/related3 which use unique test tags.
    public const int DevOpsTagCount = 1;

    // Date range counts (published items only)
    public const int Items2024Count = 22;
    public const int Items2026Count = 19;

    // Facet-specific counts (context-dependent based on filtering)
    /// <summary>AI tag appears in facets with this count when no filters applied</summary>
    public const int AiFacetCountUnfiltered = 26;  // Facet shows word "ai" appearing in this many items after ContentFixer normalization

    /// <summary>Expected total count when filtering by AI tag (word-based matching)</summary>
    public const int FilteredByAiTotalCount = 26;  // Items containing "ai" as a word in any tag after ContentFixer normalization
}
