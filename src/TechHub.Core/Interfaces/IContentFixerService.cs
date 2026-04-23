using TechHub.Core.Models.Admin;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Validates and repairs content items in the database.
/// Used both inline during content creation (per-item) and in bulk via admin triggers.
/// </summary>
public interface IContentFixerService
{
    /// <summary>
    /// Repairs markdown formatting issues in a single content string.
    /// Safe to call inline during content processing — fast, in-process, no I/O.
    /// </summary>
    string RepairMarkdown(string content);

    /// <summary>
    /// Validates markdown structural integrity for all content items using Markdig AST parsing.
    /// Does NOT modify content — only detects and logs issues.
    /// Returns the count and per-item details of structural problems.
    /// </summary>
    Task<(int Count, IReadOnlyList<ValidationIssueDetail> Details)> ValidateMarkdownAsync(string? collectionFilter = null, CancellationToken ct = default);

    /// <summary>
    /// Normalizes tags for all content items (adds collection/section tags, removes deprecated tags).
    /// Returns (scanned, fixed) counts.
    /// </summary>
    Task<(int Scanned, int Fixed)> NormalizeTagsAsync(string? collectionFilter = null, CancellationToken ct = default);

    /// <summary>
    /// Normalizes author names for all content items.
    /// </summary>
    Task<int> NormalizeAuthorsAsync(string? collectionFilter = null, CancellationToken ct = default);

    /// <summary>
    /// Validates and repairs markdown formatting for all content items in the database.
    /// Returns (scanned, fixed) counts.
    /// </summary>
    Task<(int Scanned, int Fixed)> FixMarkdownAsync(string? collectionFilter = null, CancellationToken ct = default);

    /// <summary>
    /// Runs all content fixes (tags, authors, markdown) and returns detailed per-step results.
    /// When <paramref name="jobId"/> is provided, changes are queued for review instead of applied directly.
    /// </summary>
    Task<ContentFixerResult> FixAllAsync(string? collectionFilter = null, long? jobId = null, CancellationToken ct = default);
}
