using System.Text.RegularExpressions;

namespace techhub.webapp.ApiService.Domain.Models;

/// <summary>
/// Represents a top-level organizational unit in the Tech Hub.
/// Examples: AI, GitHub Copilot, ML, Azure, .NET, DevOps, Security
/// </summary>
public class Section
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// URL-safe key (e.g., "github-copilot").
    /// </summary>
    public required string Key { get; set; }

    /// <summary>
    /// Display name (e.g., "GitHub Copilot").
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Section description for meta tags and display.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// Section URL path (e.g., "/github-copilot").
    /// </summary>
    public required string Url { get; set; }

    /// <summary>
    /// Category name for filtering (matches section titles).
    /// </summary>
    public required string Category { get; set; }

    /// <summary>
    /// Background image path for section header.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// List of collections in this section.
    /// </summary>
    public required List<Collection> Collections { get; set; } = [];

    /// <summary>
    /// Display order for navigation (lower numbers first).
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Validates the section according to business rules.
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Key))
            throw new ArgumentException("Key cannot be empty", nameof(Key));

        if (!Regex.IsMatch(Key, @"^[a-z-]+$"))
            throw new ArgumentException("Key must contain only lowercase letters and hyphens", nameof(Key));

        if (string.IsNullOrWhiteSpace(Title))
            throw new ArgumentException("Title cannot be empty", nameof(Title));

        if (string.IsNullOrWhiteSpace(Url))
            throw new ArgumentException("Url cannot be empty", nameof(Url));

        if (!Url.StartsWith('/'))
            throw new ArgumentException("Url must start with '/'", nameof(Url));

        if (string.IsNullOrWhiteSpace(Category))
            throw new ArgumentException("Category cannot be empty", nameof(Category));

        if (Collections.Count == 0)
            throw new ArgumentException("At least one collection is required", nameof(Collections));

        var validCategories = new[] { "AI", "GitHub Copilot", "ML", "Azure", "Coding", "DevOps", "Security", "All" };
        if (!validCategories.Contains(Category))
            throw new ArgumentException($"Category must be one of: {string.Join(", ", validCategories)}", nameof(Category));
    }
}
