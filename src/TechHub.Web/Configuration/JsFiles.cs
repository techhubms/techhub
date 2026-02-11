namespace TechHub.Web.Configuration;

/// <summary>
/// Single source of truth for JavaScript file configuration.
/// Used by App.razor for loading scripts with proper fingerprinting.
/// </summary>
/// <remarks>
/// <para>
/// <strong>JavaScript Loading in Blazor SSR</strong>
/// </para>
/// <para>
/// Unlike CSS files which can be loaded via &lt;link&gt; tags, JavaScript files in Blazor
/// require different loading strategies depending on when they need to execute:
/// </para>
/// <list type="bullet">
/// <item>
/// <term>Static scripts (nav-helpers.js)</term>
/// <description>Loaded via &lt;script src="@Assets[...]"&gt; with defer attribute.
/// These run after DOM is ready and don't need Blazor interactivity.</description>
/// </item>
/// <item>
/// <term>Dynamic/conditional scripts (toc-scroll-spy.js, custom-pages.js)</term>
/// <description>Loaded on-demand via dynamic import() when specific elements exist.
/// ImportMap component enables fingerprinted paths for dynamic imports.</description>
/// </item>
/// <item>
/// <term>External CDN scripts (highlight.js, mermaid)</term>
/// <description>Loaded dynamically when content needs them. Use SRI integrity hashes.</description>
/// </item>
/// </list>
/// <para>
/// <strong>Fingerprinting</strong>
/// </para>
/// <para>
/// All local JS files use @Assets["..."] helper which generates fingerprinted URLs
/// (e.g., "js/nav-helpers.abc123.js") for cache busting. The ImportMap component
/// allows dynamic import() to also use fingerprinted paths.
/// </para>
/// <para>
/// <strong>Important</strong>: Never use raw paths like "/js/file.js" in script tags
/// or loadScript() calls for local files. Always use @Assets["js/file.js"] for static
/// loading, or import() for ES modules (which uses ImportMap for fingerprinting).
/// </para>
/// </remarks>
public static class JsFiles
{
    /// <summary>
    /// Scripts loaded immediately on every page via script tags.
    /// These should be minimal - only scripts needed on every page.
    /// </summary>
    public static readonly string[] AlwaysLoad =
    [
        "js/page-timing.js",  // Performance monitoring
        "js/nav-helpers.js"   // Back to top, back to previous navigation
    ];

    /// <summary>
    /// Scripts loaded dynamically only when specific content exists on the page.
    /// These use ES module import() for lazy loading and explicit initialization.
    /// </summary>
    public static readonly string[] DynamicLoad =
    [
        "js/toc-scroll-spy.js",     // ES module - Loaded when [data-toc-scroll-spy] exists
        "js/custom-pages.js",       // ES module - Loaded when [data-collapsible] exists
        "js/date-range-slider.js"   // ES module - Loaded by DateRangeSlider component for client-side clamping
    ];

    /// <summary>
    /// All local JavaScript files (for reference/documentation).
    /// </summary>
    public static readonly string[] All =
    [
        .. AlwaysLoad,
        .. DynamicLoad
    ];
}
