using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Renderers.Html.Inlines;
using Markdig.Syntax.Inlines;

namespace TechHub.Infrastructure.Markdown;

/// <summary>
/// Markdig extension that rewrites links in markdown content:
/// - Adds target="_blank" and rel="noopener noreferrer" to external links
/// - Rewrites internal .html links to proper section/collection URLs
/// - Fixes hash-only links to include full current page URL
/// </summary>
public class LinkRewriterExtension : IMarkdownExtension
{
    private readonly string? _currentPagePath;
    private readonly string? _sectionName;
    private readonly string? _collectionName;

    public LinkRewriterExtension(string? currentPagePath = null, string? sectionName = null, string? collectionName = null)
    {
        _currentPagePath = currentPagePath;
        _sectionName = sectionName;
        _collectionName = collectionName;
    }

    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        // No setup needed
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        if (renderer is HtmlRenderer htmlRenderer)
        {
            var linkRenderer = htmlRenderer.ObjectRenderers.FindExact<LinkInlineRenderer>();
            if (linkRenderer != null)
            {
                htmlRenderer.ObjectRenderers.Remove(linkRenderer);
            }

            htmlRenderer.ObjectRenderers.AddIfNotAlready(
                new CustomLinkRenderer(_currentPagePath, _sectionName, _collectionName));
        }
    }

    /// <summary>
    /// Custom link renderer that processes link URLs
    /// </summary>
    private class CustomLinkRenderer : HtmlObjectRenderer<LinkInline>
    {
        private readonly string? _currentPagePath;
        private readonly string? _sectionName;
        private readonly string? _collectionName;

        public CustomLinkRenderer(string? currentPagePath, string? sectionName, string? collectionName)
        {
            _currentPagePath = currentPagePath;
            _sectionName = sectionName;
            _collectionName = collectionName;
        }

        protected override void Write(HtmlRenderer renderer, LinkInline link)
        {
            var url = link.Url;

            if (string.IsNullOrEmpty(url))
            {
                // No URL, render as plain text
                renderer.WriteChildren(link);
                return;
            }

            // Determine if link is external
            var isExternal = IsExternalLink(url);

            // Rewrite URL if needed
            var rewrittenUrl = RewriteUrl(url);

            // Write opening <a> tag
            renderer.Write("<a href=\"");
            renderer.WriteEscapeUrl(rewrittenUrl);
            renderer.Write("\"");

            // Add target and rel for external links
            if (isExternal)
            {
                renderer.Write(" target=\"_blank\" rel=\"noopener noreferrer\"");
            }

            // Add title if present
            if (!string.IsNullOrEmpty(link.Title))
            {
                renderer.Write(" title=\"");
                renderer.WriteEscape(link.Title);
                renderer.Write("\"");
            }

            // Add any attributes
            renderer.WriteAttributes(link);

            renderer.Write(">");

            // Write link text
            renderer.WriteChildren(link);

            // Close tag
            renderer.Write("</a>");
        }

        /// <summary>
        /// Check if a URL is external (different domain or protocol)
        /// </summary>
        private static bool IsExternalLink(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            // Hash-only links are internal
            if (url.StartsWith('#'))
                return false;

            // Relative paths are internal
            if (url.StartsWith('/'))
                return false;

            // Check if it has a protocol (http://, https://, mailto:, etc.)
            if (url.Contains("://"))
            {
                // URLs with protocols are external unless they're our own domain
                var uri = new Uri(url, UriKind.Absolute);
                return uri.Host != "tech.hub.ms" && uri.Host != "localhost";
            }

            // Relative paths without / are internal
            return false;
        }

        /// <summary>
        /// Rewrite URL based on link type
        /// </summary>
        private string RewriteUrl(string url)
        {
            // 1. Hash-only links (#heading) - prepend current page path
            if (url.StartsWith('#') && !string.IsNullOrEmpty(_currentPagePath))
            {
                return $"{_currentPagePath}{url}";
            }

            // 2. Internal .html links - rewrite to /section/collection/slug format
            if (url.EndsWith(".html") && !url.Contains("://"))
            {
                // Extract filename without extension
                var filename = Path.GetFileNameWithoutExtension(url);

                // Remove date prefix if present (YYYY-MM-DD-)
                var slugMatch = System.Text.RegularExpressions.Regex.Match(filename, @"^\d{4}-\d{2}-\d{2}-(.+)$");
                var slug = slugMatch.Success ? slugMatch.Groups[1].Value : filename;

                // Build proper URL
                if (!string.IsNullOrEmpty(_sectionName) && !string.IsNullOrEmpty(_collectionName))
                {
                    return $"/{_sectionName}/{_collectionName}/{slug}";
                }
            }

            // 3. No changes needed
            return url;
        }
    }
}
