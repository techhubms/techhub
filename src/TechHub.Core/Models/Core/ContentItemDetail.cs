namespace TechHub.Core.Models;

/// <summary>
/// Extended content item with full markdown content - used for detail views only.
/// Inherits all list-view properties from ContentItem and adds Content for rendering.
/// This separation improves list query performance by not loading ~3.8KB content per item.
/// </summary>
public record ContentItemDetail : ContentItem
{
    /// <summary>
    /// Raw markdown content (from file or database).
    /// Used internally for rendering. Not serialized to API responses.
    /// Set to null when RenderedHtml is populated to save memory.
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public string? Content { get; private set; }

    /// <summary>
    /// Full rendered HTML content (only populated after markdown rendering).
    /// Setting this will null out Content to save memory.
    /// </summary>
    public string? RenderedHtml { get; private set; }

    /// <summary>
    /// Set the rendered HTML and null out the raw content to save memory.
    /// </summary>
    public void SetRenderedHtml(string renderedHtml)
    {
        if (string.IsNullOrWhiteSpace(renderedHtml))
        {
            throw new ArgumentException("Rendered HTML cannot be empty", nameof(renderedHtml));
        }

        RenderedHtml = renderedHtml;
        Content = null; // Free up memory
    }

    /// <summary>
    /// Database constructor - used by Dapper to materialize from database queries.
    /// Parameter order and types must match the SELECT column order exactly.
    /// </summary>
    public ContentItemDetail(
        string slug,
        string title,
        string author,
        long dateEpoch,
        string collectionName,
        string feedName,
        string primarySectionName,
        string excerpt,
        string externalUrl,
        bool draft,
        string? content,
        string? subcollectionName,
        string? plans,
        bool ghesSupport,
        string? tagsCsv = null,
        bool isAi = false,
        bool isAzure = false,
        bool isCoding = false,
        bool isDevOps = false,
        bool isGitHubCopilot = false,
        bool isMl = false,
        bool isSecurity = false)
        : base(slug, title, author, dateEpoch, collectionName, feedName, primarySectionName,
               excerpt, externalUrl, draft, subcollectionName, plans, ghesSupport,
               tagsCsv, isAi, isAzure, isCoding, isDevOps, isGitHubCopilot, isMl, isSecurity)
    {
        Content = content;
    }
}
