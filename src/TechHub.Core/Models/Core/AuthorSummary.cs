namespace TechHub.Core.Models;

/// <summary>
/// Summary of an author with total content item count.
/// Used for the authors index page to display all known authors.
/// </summary>
public record AuthorSummary
{
    /// <summary>
    /// Author's display name as stored in content frontmatter.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Total number of published content items attributed to this author.
    /// </summary>
    public required int ItemCount { get; init; }
}
