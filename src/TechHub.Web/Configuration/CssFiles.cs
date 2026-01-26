namespace TechHub.Web.Configuration;

/// <summary>
/// Single source of truth for CSS file configuration.
/// Used by both App.razor (for development individual files) and Program.cs (for production bundling).
/// </summary>
public static class CssFiles
{
    /// <summary>
    /// All CSS files in the order they should be loaded/bundled.
    /// Order matters: design-tokens must be first, base.css second, then feature-specific styles.
    /// </summary>
    public static readonly string[] All =
    [
        "css/design-tokens.css",
        "css/base.css",
        "css/buttons.css",
        "css/cards.css",
        "css/article.css",
        "css/sidebar.css",
        "css/page-container.css",
        "css/loading.css",
        "css/nav-helpers.css",
        "css/date-slider.css",
        "css/tag-dropdown.css"
    ];
}
