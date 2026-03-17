using System.Text;
using System.Xml;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;

namespace TechHub.Api.Endpoints;

/// <summary>
/// API endpoint for XML sitemap generation.
/// Returns a sitemap containing all static pages and internal content pages.
/// External-only content (news, blogs, community) is excluded because those items
/// redirect to third-party sources and have no detail page on this site.
/// </summary>
public static class SitemapEndpoints
{
    private const string SitemapNs = "http://www.sitemaps.org/schemas/sitemap/0.9";

    public static IEndpointRouteBuilder MapSitemapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/sitemap", GetSitemap)
            .WithName("GetSitemap")
            .WithTags("Sitemap")
            .WithSummary("Get XML sitemap")
            .WithDescription(
                "Returns an XML sitemap containing static pages (homepage, sections, collections) " +
                "and internal content pages (videos, roundups, custom pages). " +
                "Excludes news, blogs, and community items because those link to external sources.")
            .Produces(StatusCodes.Status200OK, contentType: "application/xml")
            .ExcludeFromDescription();

        return endpoints;
    }

    private static async Task<IResult> GetSitemap(
        IContentRepository contentRepository,
        IOptions<AppSettings> settings)
    {
        var baseUrl = settings.Value.BaseUrl.TrimEnd('/');

        var sections = await contentRepository.GetAllSectionsAsync();
        var contentItems = await contentRepository.GetSitemapItemsAsync();

        var xml = BuildSitemapXml(baseUrl, sections, contentItems);
        return Results.Content(xml, "application/xml; charset=utf-8");
    }

    private static string BuildSitemapXml(
        string baseUrl,
        IReadOnlyList<Core.Models.Section> sections,
        IReadOnlyList<Core.Models.SitemapItem> contentItems)
    {
        var sb = new StringBuilder();
        var settings = new XmlWriterSettings
        {
            Indent = true,
            Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
            OmitXmlDeclaration = false
        };

        using var writer = XmlWriter.Create(sb, settings);

        writer.WriteStartDocument();
        writer.WriteStartElement("urlset", SitemapNs);

        // ── Homepage ──────────────────────────────────────────────────
        WriteUrl(writer, $"{baseUrl}/", changeFreq: "daily", priority: "1.0");

        // ── Section pages (skip "all" — it's not a real navigable section) ──
        foreach (var section in sections.Where(s => s.Name != "all"))
        {
            WriteUrl(writer, $"{baseUrl}{section.Url}", changeFreq: "daily", priority: "0.9");

            // Collection pages within this section
            foreach (var collection in section.Collections)
            {
                WriteUrl(writer, $"{baseUrl}{collection.Url}", changeFreq: "daily", priority: "0.8");
            }
        }

        // ── Internal content pages ────────────────────────────────────
        // Only videos, roundups, and custom-collection items have real detail pages.
        // Items where LinksExternally() == true (news/blogs/community) are excluded at the DB query level.
        foreach (var item in contentItems)
        {
            var path = item.CollectionName == "roundups"
                ? $"/all/roundups/{item.Slug}"
                : $"/{item.PrimarySectionName}/{item.CollectionName}/{item.Slug}";

            var lastMod = DateTimeOffset.FromUnixTimeSeconds(item.DateEpoch)
                .ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            WriteUrl(writer, $"{baseUrl}{path}", changeFreq: "monthly", priority: "0.6", lastMod: lastMod);
        }

        writer.WriteEndElement(); // </urlset>
        writer.WriteEndDocument();
        writer.Flush();

        return sb.ToString();
    }

    private static void WriteUrl(
        XmlWriter writer,
        string loc,
        string changeFreq,
        string priority,
        string? lastMod = null)
    {
        writer.WriteStartElement("url", SitemapNs);

        writer.WriteElementString("loc", SitemapNs, loc);

        if (lastMod != null)
        {
            writer.WriteElementString("lastmod", SitemapNs, lastMod);
        }

        writer.WriteElementString("changefreq", SitemapNs, changeFreq);
        writer.WriteElementString("priority", SitemapNs, priority);

        writer.WriteEndElement(); // </url>
    }
}
