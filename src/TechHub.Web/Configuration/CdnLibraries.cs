namespace TechHub.Web.Configuration;

/// <summary>
/// Configuration for external CDN libraries used in the application.
/// Centralizes version management and SRI (Subresource Integrity) hashes.
/// </summary>
/// <remarks>
/// <para>
/// <strong>Why This Exists</strong>
/// </para>
/// <para>
/// External libraries (Highlight.js, Mermaid) are loaded from CDNs for performance.
/// This file centralizes all version numbers and SRI hashes so updates are made in ONE place.
/// </para>
/// <para>
/// <strong>Security: SRI Hashes</strong>
/// </para>
/// <para>
/// Subresource Integrity (SRI) hashes ensure CDN-served files haven't been tampered with.
/// When updating versions, you MUST update the corresponding SRI hashes.
/// </para>
/// <para>
/// To get SRI hashes:
/// <list type="bullet">
/// <item>Use <see href="https://www.srihash.org/">srihash.org</see></item>
/// <item>Or run: <c>curl -s [URL] | openssl dgst -sha512 -binary | openssl base64 -A</c></item>
/// </list>
/// </para>
/// <para>
/// <strong>Updating Versions</strong>
/// </para>
/// <para>
/// 1. Update the version constant (e.g., <see cref="HighlightJs.Version"/>)
/// 2. Update the SRI hash (e.g., <see cref="HighlightJs.CoreIntegrity"/>)
/// 3. Test locally to verify the library loads correctly
/// 4. Commit the changes
/// </para>
/// </remarks>
#pragma warning disable CA1034 // Nested types should not be visible - intentional for configuration grouping
public static class CdnLibraries
{
    /// <summary>
    /// Highlight.js configuration for syntax highlighting
    /// </summary>
    /// <remarks>
    /// <para>Latest version check: https://github.com/highlightjs/highlight.js/releases</para>
    /// <para>CDN: https://cdnjs.com/libraries/highlight.js</para>
    /// </remarks>
    public static class HighlightJs
    {
        /// <summary>
        /// Highlight.js version number
        /// </summary>
        public const string Version = "11.11.1";

        /// <summary>
        /// Base CDN URL (without version)
        /// </summary>
        public const string CdnBase = "https://cdnjs.cloudflare.com/ajax/libs/highlight.js";

        /// <summary>
        /// Full CDN URL including version
        /// </summary>
        public static string CdnUrl => $"{CdnBase}/{Version}";

        /// <summary>
        /// SRI hash for the core highlight.min.js library
        /// </summary>
        public const string CoreIntegrity = "sha512-EBLzUfMbsAVRgD3Qg2xjsLfdPK12lIdOW/GNVeAhgOE1NG1tt7ZJlrCHVXrqiEi/5lrlazEB1vBh8FJpKl1hA==";

        /// <summary>
        /// SRI hash for the github-dark.min.css theme
        /// </summary>
        public const string ThemeIntegrity = "sha512-s53i9OTIfN+bRrefHgLw3SXGq91pHkuL7kFvM44KTkB4ZWAfSUnDqhv3HCpTNoLgqmRJso9z/WeMRNONRKABpA==";

        /// <summary>
        /// Theme CSS file name
        /// </summary>
        public const string ThemeFile = "styles/github-dark.min.css";

        /// <summary>
        /// Core languages to load for syntax highlighting (bundled with highlight.js)
        /// </summary>
        /// <remarks>
        /// These languages are included in the standard highlight.js CDN build.
        /// </remarks>
        public static readonly string[] Languages =
        [
            "csharp",
            "javascript",
            "typescript",
            "python",
            "bash",
            "yaml",
            "json",
            "sql",
            "powershell",
            "xml",      // Also covers HTML
            "css",
            "markdown",
            "dockerfile",
            "http",
            "plaintext"
        ];

        /// <summary>
        /// Third-party language plugins for highlight.js
        /// </summary>
        /// <remarks>
        /// <para>
        /// These languages require separate 3rd party plugins. They are loaded from GitHub via jsDelivr
        /// since most aren't published to npm.
        /// </para>
        /// <para>
        /// Available plugins for Azure/IaC languages:
        /// <list type="bullet">
        /// <item><c>highlightjs-terraform</c> - Terraform/HCL syntax (aliases: terraform, tf, hcl)</item>
        /// </list>
        /// </para>
        /// <para>
        /// <strong>Not Yet Available:</strong>
        /// <list type="bullet">
        /// <item>Bicep - Listed in SUPPORTED_LANGUAGES.md but plugin doesn't exist yet</item>
        /// <item>ARM templates - Just JSON, use built-in 'json' language</item>
        /// </list>
        /// </para>
        /// </remarks>
        public static class ThirdPartyLanguages
        {
            /// <summary>
            /// Terraform/HCL plugin configuration
            /// </summary>
            /// <remarks>
            /// Repository: https://github.com/highlightjs/highlightjs-terraform
            /// Loaded via jsDelivr GitHub CDN (not published to npm)
            /// </remarks>
            public const string TerraformCdnUrl = "https://cdn.jsdelivr.net/gh/highlightjs/highlightjs-terraform/terraform.js";

            // Note: Bicep plugin (highlightjs-bicep) is listed in highlight.js SUPPORTED_LANGUAGES.md
            // but the repository doesn't actually exist. ARM templates are JSON-based.
        }
    }

    /// <summary>
    /// Mermaid.js configuration for diagrams
    /// </summary>
    /// <remarks>
    /// <para>Latest version check: https://github.com/mermaid-js/mermaid/releases</para>
    /// <para>CDN: https://www.jsdelivr.com/package/npm/mermaid</para>
    /// </remarks>
    public static class Mermaid
    {
        /// <summary>
        /// Mermaid version number (pinned for stability)
        /// </summary>
        /// <remarks>
        /// Latest at time of writing: 11.12.2 (Dec 2, 2025)
        /// </remarks>
        public const string Version = "11.12.2";

        /// <summary>
        /// Full CDN URL for Mermaid (pinned version)
        /// </summary>
        public static string CdnUrl => $"https://cdn.jsdelivr.net/npm/mermaid@{Version}/dist/mermaid.min.js";

        // Note: jsdelivr generates SRI hashes dynamically via ?integrity query param if needed
    }
}
#pragma warning restore CA1034
