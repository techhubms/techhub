namespace TechHub.Web.Configuration;

/// <summary>
/// Single source of truth for CSS file configuration.
/// App.razor references these files via @Assets["path"] for fingerprinted URLs (cache busting).
/// This array is used by tests to verify all CSS files are referenced in App.razor.
/// <para>
/// IMPORTANT: When adding a CSS file, also add the @Assets["path"] reference in App.razor.
/// </para>
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
        "css/admin.css"
    ];
}
