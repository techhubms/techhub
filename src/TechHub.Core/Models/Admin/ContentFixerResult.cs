namespace TechHub.Core.Models.Admin;

/// <summary>
/// Detailed results from a content fixer run, showing per-step statistics.
/// </summary>
public sealed class ContentFixerResult
{
    public required int TagsScanned { get; init; }
    public required int TagsFixed { get; init; }
    public required int AuthorsFixed { get; init; }
    public required int MarkdownScanned { get; init; }
    public required int MarkdownFixed { get; init; }
    public required int ValidationIssues { get; init; }

    /// <summary>
    /// Per-item validation issue details (collection/slug → issue descriptions).
    /// Populated when validation finds structural problems.
    /// </summary>
    public IReadOnlyList<ValidationIssueDetail> ValidationDetails { get; init; } = [];

    public int TotalFixed => TagsFixed + AuthorsFixed + MarkdownFixed;

    public string ToLogOutput()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append(System.Globalization.CultureInfo.InvariantCulture, $"Content cleanup completed — {TotalFixed} items fixed.\n");
        sb.Append(System.Globalization.CultureInfo.InvariantCulture, $"  Tags:       {TagsFixed} fixed out of {TagsScanned} scanned\n");
        sb.Append(System.Globalization.CultureInfo.InvariantCulture, $"  Authors:    {AuthorsFixed} fixed\n");
        sb.Append(System.Globalization.CultureInfo.InvariantCulture, $"  Markdown:   {MarkdownFixed} fixed out of {MarkdownScanned} scanned\n");
        sb.Append(System.Globalization.CultureInfo.InvariantCulture, $"  Validation: {ValidationIssues} items with structural issues\n");

        foreach (var detail in ValidationDetails)
        {
            sb.Append(System.Globalization.CultureInfo.InvariantCulture, $"    ⚠ [{detail.CollectionName}/{detail.Slug}]: {detail.Issues}\n");
        }

        return sb.ToString().TrimEnd();
    }
}

/// <summary>
/// Details about a single content item's validation issues.
/// </summary>
public sealed class ValidationIssueDetail
{
    public required string Slug { get; init; }
    public required string CollectionName { get; init; }
    public required string Issues { get; init; }
}
