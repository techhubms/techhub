namespace TechHub.TestUtilities;

/// <summary>
/// Constants defining expected counts in the TestCollections test data.
/// These values are hardcoded based on the actual markdown files in TestCollections directory.
/// Update these when test data changes.
/// </summary>
/// <remarks>
/// Last updated: 2026-02-09 after adding tag symmetry test data (4 new blog files).
/// 
/// Test data composition:
/// - 36 published items total (was 32, added 4 for tag symmetry bug test)
/// - 3 draft items (not counted in published totals)
/// - Collections: blogs (22), news (7), videos (4), community (2), roundups (1)
/// - Tag distributions: AI (12 items), DevOps (1 item), GitHub Copilot (4 tag symmetry test items)
/// - Date ranges: 2024 items (17), 2026 items (19)
/// </remarks>
public static class TestDataConstants
{
    // Total counts
    public const int TotalPublishedItems = 36;
    public const int TotalDraftItems = 3;
    public const int TotalItems = TotalPublishedItems + TotalDraftItems;

    // Collection counts (published only)
    public const int BlogsCount = 22;
    public const int NewsCount = 7;
    public const int VideosCount = 4;  // Includes root videos + subcollections
    public const int CommunityCount = 2;
    public const int RoundupsCount = 1;

    // Tag counts (published items only)
    // Word-based counting: tags are split by space, dash, underscore
    // Example: "GitHub Copilot" contains words "github" and "copilot"
    public const int AiTagCount = 25;  // Items with "ai" as a word in any tag after ContentFixer normalization (e.g., "AI", "Agentic AI", "AI Development", "GitHub Copilot"). Excludes related1/related2/related3 which use unique test tags.
    public const int DevOpsTagCount = 1;

    // Date range counts (published items only)
    public const int Items2024Count = 17;
    public const int Items2026Count = 19;

    // Facet-specific counts (context-dependent based on filtering)
    /// <summary>AI tag appears in facets with this count when no filters applied</summary>
    public const int AiFacetCountUnfiltered = 25;  // Facet shows word "ai" appearing in this many items after ContentFixer normalization

    /// <summary>Expected total count when filtering by AI tag (word-based matching)</summary>
    public const int FilteredByAiTotalCount = 25;  // Items containing "ai" as a word in any tag after ContentFixer normalization
}
